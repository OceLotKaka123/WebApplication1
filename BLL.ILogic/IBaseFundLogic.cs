using BLL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.ILogic
{
    public interface IBaseFundLogic
    {
        IEnumerable<FundRedisModel> GetFundInfoByName(string content);
        FundRedisModel GetFundInfoById(FundAnswerModel fundAnswerModel);
        IEnumerable<FundTypeRedisModel> GetFundTypeValue();
        IEnumerable<FundAnswerModel> GetFundOftenAnwer();
        IEnumerable<FundRedisModel> GetFollowFundByName(string content);
        IEnumerable<UserFollowFundViewModel> GetUserFollowFundInfo();
    }
}
