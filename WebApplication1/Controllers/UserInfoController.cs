using BLL.ILogic;
using BLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class UserInfoController : ApiBaseController
    {
        private readonly IUserInfoLogic _userInfoLogic;
        private CurrentUser _currentUser;

        public UserInfoController(IUserInfoLogic userInfoLogic,CurrentUser currentUser)
        {
            _userInfoLogic = userInfoLogic;
            _currentUser = currentUser;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfoModel"></param>
        [HttpPost]
        [AllowAnonymous]
        public UserInfoViewModel AddUserInfo([Required]UserInfoModel userInfoModel)
        {
            return _userInfoLogic.AddUserInfo(userInfoModel);
        }
        /// <summary>
        /// 获取用户头像信息
        /// </summary>
        /// <param name="telPhone">用户电话</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public  string GetUserAvatarUrl([Required]string telPhone)
        {
             return  _userInfoLogic.GetUserAvatarUrl(telPhone); ;
        }
        /// <summary>
        /// 验证用户登入信息
        /// </summary>
        /// <param name="userLoginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public UserInfoViewModel CheckUserLogin([Required]UserLoginModel userLoginModel)
        {
            return _userInfoLogic.CheckUserLogin(userLoginModel);
        } 
        /// <summary>
        /// 退出登入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool ExitLogic()
        {
            return _userInfoLogic.ExitLogic();
        }
        /// <summary>
        /// 修改用户数据
        /// </summary>
        /// <param name="userInfoModel"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateUserInfo([Required]UserInfoModel userInfoModel)
        {
            return _userInfoLogic.UpdateUserInfo(userInfoModel);
        }
        /// <summary>
        /// 获取新的token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetToken()
        {
            return _userInfoLogic.GetToken();
        }
    }
}
