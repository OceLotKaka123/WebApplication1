using BLL.Model;
using DAL.ILogic;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DAL.Logic
{
    public class UserInfoSql : IUserInfoSql
    {
        private readonly SqlConnection _con;
        public UserInfoSql(SqlConnection con)
        {
            _con = con;
        }
        public UserInfoViewModel CheckUserLogin(UserLoginModel userLogin)
        {
            return _con.QueryFirstOrDefault<UserInfoViewModel>(@"select 
              userInfo.Id,userInfo.TelPhone,userInfo.Name,userInfo.UserCode,
                 userInfo.AvatarUrl from 
                   UserInfo userInfo join UserPsd userPsd
           on userInfo.Id=userPsd.UserId where userPsd.Valid=1
        and userInfo.Valid=1 and userInfo.TelPhone=@TelPhone and 
         userPsd.PassWord=@PassWord", 
          userLogin);
        }

        public IEnumerable<FollowFundViewModel> GetUserFollowFund(int id)
        {
            return _con.Query<FollowFundViewModel>(@"select 
              followFund.FundCode,baseFund.FundName from FollowFund 
              followFund join BaseFund baseFund on 
              followFund.FundCode=baseFund.FundCode
              where followFund.Valid=1 and followFund.UserId=@userId", 
              new { userId = id });
        }
    }
}
