using BLL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.ILogic
{
    public interface IUserInfoSql
    {
        UserInfoViewModel CheckUserLogin(UserLoginModel userLogin);
        IEnumerable<FollowFundViewModel> GetUserFollowFund(int id);
    }
}
