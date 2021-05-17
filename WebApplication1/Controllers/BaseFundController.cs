using BLL.ILogic;
using BLL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class BaseFundController : ApiBaseController
    {
        private readonly IBaseFundLogic _baseFundLogic;
        public BaseFundController(IBaseFundLogic baseFundLogic)
        {
            _baseFundLogic = baseFundLogic;
        }
        /// <summary>
        /// 基金询问信息
        /// </summary>
        /// <param name="content">询问内容</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<FundRedisModel> GetFundInfoByName([Required]string content)
        {
            return _baseFundLogic.GetFundInfoByName(content);
        }
        /// <summary>
        /// 获取基金信息(Id)
        /// </summary>
        /// <param name="fundAnswerModel"></param>
        /// <returns></returns>
        [HttpGet]
        public FundRedisModel GetFundInfoById([Required]FundAnswerModel fundAnswerModel)
        {
            return _baseFundLogic.GetFundInfoById(fundAnswerModel);
        }
        /// <summary>
        /// 获取基金类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<FundTypeRedisModel> GetFundTypeValue()
        { 
            return _baseFundLogic.GetFundTypeValue();
        }
        /// <summary>
        /// 基民常问问题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<FundAnswerModel> GetFundOftenAnwer()
        {
            return _baseFundLogic.GetFundOftenAnwer();
        }
        /// <summary>
        /// 获取关注基金信息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<FundRedisModel> GetFollowFundByName([Required] string content)
        {
            return _baseFundLogic.GetFollowFundByName(content);
        }
        /// <summary>
        /// 获取用户关注基金信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<UserFollowFundViewModel> GetUserFollowFundInfo()
        {
            return _baseFundLogic.GetUserFollowFundInfo();
        }
    }
}
