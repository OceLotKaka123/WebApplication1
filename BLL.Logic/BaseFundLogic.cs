using AutoMapper;
using BLL.ILogic;
using BLL.Model;
using DAL.ISql;
using Entities;
using Extension;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL.Logic
{
    public class BaseFundLogic : IBaseFundLogic
    {
        private readonly RedisManagerPool _redisManagerPool;
        private readonly IBaseFundSql _baseFundSql;
        private readonly SqlConnection _con;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        public BaseFundLogic(RedisManagerPool redisManagerPool, IBaseFundSql baseFundSql,SqlConnection con,IMapper mapper,CurrentUser currentUser)
        {
            _redisManagerPool = redisManagerPool;
            _baseFundSql = baseFundSql;
            _con = con;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public FundRedisModel GetFundInfoById(FundAnswerModel fundAnswerModel)
        {
            using (var redisManager = _redisManagerPool.GetClient())
            {
                redisManager.Db = (int)RedisDB.DB4;
                var fundRedis = redisManager.As<FundRedisModel>();
                var fundRedisModel = fundRedis.GetById(fundAnswerModel.AnswerFundId);
                if (fundAnswerModel != null)
                {
                    JudgeFundAnswer(fundAnswerModel);
                }
                return fundRedisModel;
            }
        }

        public IEnumerable<FundRedisModel> GetFundInfoByName(string content)
        {
            using (var redisManager = _redisManagerPool.GetClient())
            {
                redisManager.Db = (int)RedisDB.DB4;
                var fundRedis = redisManager.As<FundRedisModel>();
                var fundRedisModels = fundRedis.GetAll().Where(p => content.Contains(p.FundName) || content.Contains(p.FundCode)).Take(10);
                if (fundRedisModels != null && fundRedisModels.Count() == 1)
                {
                    FundAnswerModel fundAnswerModel = new FundAnswerModel
                    {
                        AnswerFundId = fundRedisModels.First().Id,
                        AnswerContent = content
                    };
                    //添加到常用问题
                    JudgeFundAnswer(fundAnswerModel);
                }
                return fundRedisModels;
            }
        }
        /// <summary>
        /// 判断回答该问题是否已存在
        /// </summary>
        /// <param name="answerModel"></param>
        private void JudgeFundAnswer(FundAnswerModel answerModel)
        {
            var fundAnswerEntity=_con.GetEntities<FundAnswerEntity>().Where(p => p.AnswerContent == answerModel.AnswerContent).QueryFirst<FundAnswerEntity>();
            if (fundAnswerEntity != null)
            {
                fundAnswerEntity.AnswerNumber += 1;
                _con.UpdateReord(fundAnswerEntity);
            }
            else
            {
                var answerEntity = _mapper.Map<FundAnswerEntity>(answerModel);
                _con.AddRecord(answerEntity);
            }
        }

        public IEnumerable<FundAnswerModel> GetFundOftenAnwer()
        {
            return _baseFundSql.GetFundOftenAnwer();
        }

        public IEnumerable<FundTypeRedisModel> GetFundTypeValue()
        {
            using(var redisClient = _redisManagerPool.GetClient())
            {
                redisClient.Db = (int)RedisDB.DB4;
                var redisTyped = redisClient.As<FundTypeRedisModel>();
                return redisTyped.GetAll();
            }
        }

        public IEnumerable<FundRedisModel> GetFollowFundByName(string content)
        {
            using (var redisManager = _redisManagerPool.GetClient())
            {
                redisManager.Db = (int)RedisDB.DB4;
                var fundRedis = redisManager.As<FundRedisModel>();
                var fundRedisModels = fundRedis.GetAll().Where(p => p.FundName.Contains(content) || p.FundCode.Contains(content)).Take(10);
                return fundRedisModels;
            }
        }

        public IEnumerable<UserFollowFundViewModel> GetUserFollowFundInfo()
        {
            return _baseFundSql.GetUserFollowFundInfo(_currentUser.Id);
        }
    }
}
