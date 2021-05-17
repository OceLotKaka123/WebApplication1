using AutoMapper;
using BLL.ILogic;
using BLL.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using System.Data.SqlClient;
using Extension;
using DAL.ILogic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Commons;
using System.Threading.Tasks;
using Dapper;
using System.Net.Http.Headers;

namespace BLL.Logic
{
    public class UserInfoLogic:IUserInfoLogic
    {
        private readonly IMapper _mapper;
        private readonly RedisManagerPool _redisManagerPool;
        private readonly SqlConnection _con;
        private readonly IGenerateCodeSql _generateCode;
        private readonly IUserInfoSql _userInfoSql;
        private  readonly CurrentUser _currentUser;
        private readonly IHttpContextAccessor _httpContext;
        private static readonly Object userObject = new Object();
        public UserInfoLogic(IMapper mapper,RedisManagerPool redisManagerPool,SqlConnection con,IGenerateCodeSql generateCode,IUserInfoSql userInfoSql,CurrentUser currentUser,IHttpContextAccessor httpContext)
        {
            _con = con;
            _mapper = mapper;
            _redisManagerPool = redisManagerPool;
            _generateCode = generateCode;
            _userInfoSql = userInfoSql;
            _currentUser = currentUser;
            _httpContext = httpContext;
        }
        public UserInfoViewModel AddUserInfo(UserInfoModel userInfoModel)
        {
            lock (userObject)
            {
                var userInfoEntity = _mapper.Map<UserInfoEntity>(userInfoModel);
                var userInfo = _con.GetEntities<UserInfoEntity>().Where(p => p.Valid == 1 && p.TelPhone == userInfoModel.TelPhone).QueryFirst<UserInfoEntity>();
                //判断电话号码是否被注册
                if (userInfo != null)
                {
                    throw new CNException("电话号码已被注册");
                }
                UserInfoEntity entity = null;
                //将用户数据存到数据库中
                using (var transa = _con.BeginTransaction())
                {
                    try
                    {
                        int codeName = (int)CodeName.用户编码;
                        string userCode = _generateCode.GetCode(codeName, transa);
                        userInfoEntity.UserCode = userCode;
                        entity = _con.AddRecord(userInfoEntity, transa);
                        UserPsdEntity userPsdEntity = new UserPsdEntity
                        {
                            PassWord = userInfoModel.PassWord,
                            UserId = entity.Id
                        };
                        _con.AddRecord(userPsdEntity, transa);
                        transa.Commit();
                    }
                    catch (Exception e)
                    {
                        transa.Rollback();
                        throw e;
                    }
                }
                //将用户的信息、产生的Token存到Redis中
                using (var redisManager = _redisManagerPool.GetClient())
                {
                    redisManager.Db = (int)RedisDB.DB0;
                    var currentUser = _mapper.Map<CurrentUser>(entity);
                    currentUser.Token = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(Guid.NewGuid().ToString()));
                    //存取用户编码(key:token value:用户编码)
                    redisManager.Set<string>(currentUser.Token, currentUser.UserCode,DateTime.Now.AddMinutes(15));
                    //存取用户信息(key:用户编码 value:用户信息)
                    redisManager.Set<CurrentUser>(currentUser.UserCode, currentUser, DateTime.Now.AddMinutes(15));
                    return _mapper.Map<UserInfoViewModel>(currentUser);
                }
            }
        }
        public UserInfoViewModel CheckUserLogin(UserLoginModel userLogin)
        {
            lock (userObject)
            {
                var userInfoViewModel = _userInfoSql.CheckUserLogin(userLogin);
                if (userInfoViewModel == null)
                {
                    _httpContext.HttpContext.Response.StatusCode = 600;
                    throw new CNException("账号或者密码错误");
                }
                using (var redisManager = _redisManagerPool.GetClient())
                {
                    redisManager.Db = (int)RedisDB.DB0;
                    //判断用户是否已登录
                    CheckIsLogin(userInfoViewModel.UserCode, redisManager);
                    userInfoViewModel.Token = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(Guid.NewGuid().ToString()));
                    var currentUser = _mapper.Map<CurrentUser>(userInfoViewModel);
                    redisManager.Set<string>(userInfoViewModel.Token, userInfoViewModel.UserCode, DateTime.Now.AddMinutes(15));
                    redisManager.Set<CurrentUser>(userInfoViewModel.UserCode, currentUser, DateTime.Now.AddMinutes(15));
                    userInfoViewModel.FollowFundModels = _userInfoSql.GetUserFollowFund(userInfoViewModel.Id);
                    return userInfoViewModel;
                }
            }
        }

        private void CheckIsLogin(string userCode, IRedisClient redisManager)
        {
            var currentUser = redisManager.Get<CurrentUser>(userCode);
            if (currentUser != null)
            {
                throw new CNException("账号已登录");
            }
        }

        public bool ExitLogic()
        {
            using(var redisManager = _redisManagerPool.GetClient())
            {
                redisManager.Db = (int)RedisDB.DB0;
                redisManager.Remove(_currentUser.Token);
                redisManager.Remove(_currentUser.UserCode);
                return true;
            }
        }

        public string GetUserAvatarUrl(string telPhone)
        {
            return _con.GetEntities<UserInfoEntity>().Where(p => p.TelPhone == telPhone && p.Valid == 1).QueryFirst<UserInfoEntity>()?.AvatarUrl;
        }

        public bool UpdateUserInfo(UserInfoModel userInfoModel)
        {
            UserInfoEntity userInfo = _mapper.Map<UserInfoEntity>(userInfoModel);
            var userCode = _currentUser.UserCode;
            using (var redisManager = _redisManagerPool.GetClient())
            {
                redisManager.Db = (int)RedisDB.DB0;
                var currentUser = redisManager.Get<CurrentUser>(userCode);
                if (currentUser == null)
                {
                    throw new CNException("用户数据不存在");
                }
                userInfo.Id = _currentUser.Id;
                var current=_mapper.Map<CurrentUser>(userInfo);
                //清除Token信息，回到登录界面
                redisManager.Remove(userCode);
                redisManager.Remove(currentUser.Token);
            }
            var transaction=_con.BeginTransaction();
            try
            {
                //修改用户的关注基金
                UpdateFollowFunds(userInfoModel.FollowFundModels,transaction);
                _con.UpdateReord(userInfo,transaction);
                transaction.Commit();
            }catch(Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            return true;
        }
        //修改关注基金
        private void UpdateFollowFunds(IEnumerable<FollowFundModel> followFundModels,SqlTransaction transaction)
        {
            var followFundEntities = _con.GetEntities<FollowFundEntity>().Where(p => p.Valid == 1 && p.UserId == _currentUser.Id).ToQuery<FollowFundEntity>();
            _con.DeleteRecords(followFundEntities,transaction);
            if (followFundModels.Any())
            {
                var followFunds =followFundModels.Select(p => new FollowFundEntity
                {
                    UserId = _currentUser.Id,
                    FundCode = p.FundCode
                });
                ///添加关注基金
                _con.AddRecords(followFunds,transaction);
            }
        }

        public string GetToken()
        {
            var token=_currentUser.Token;
            var newToken=Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(Guid.NewGuid().ToString()));
            using(var redisClient = _redisManagerPool.GetClient())
            {
                redisClient.Db = (int)RedisDB.DB0;
                var userCode=redisClient.Get<string>(token);
                redisClient.Set<string>(newToken, userCode,DateTime.Now.AddMinutes(6));
            }
            return newToken;
        }
    }
}
