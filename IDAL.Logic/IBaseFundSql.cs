using BLL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.ISql
{
    public interface IBaseFundSql
    {
        IEnumerable<FundAnswerModel> GetFundOftenAnwer();
        IEnumerable<UserFollowFundViewModel> GetUserFollowFundInfo(int id);
    }
}
