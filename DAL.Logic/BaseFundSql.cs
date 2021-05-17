using BLL.Model;
using DAL.ISql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.Sql
{
    public class BaseFundSql:IBaseFundSql
    {
        private readonly SqlConnection _con;
        public BaseFundSql(SqlConnection con)
        {
            _con = con;
        }

        public IEnumerable<FundAnswerModel> GetFundOftenAnwer()
        {
            return _con.Query<FundAnswerModel>(@"select top 8 
               AnswerContent,AnswerFundId from 
              FundAnswer order by AnswerNumber desc");
        }

        public IEnumerable<UserFollowFundViewModel> GetUserFollowFundInfo(int id)
        {
           var s =  _con.Query<dynamic>(@"select 
           followFund.FundCode,baseFund.FundName,baseFund.DateNow 
            UpdateTime,baseFund.RealValue from FollowFund followFund join
            BaseFund baseFund on followFund.FundCode=baseFund.FundCode 
            where  followFund.Valid=1 and followFund.UserId=@Id ", new { id });
            if (s != null)
            {
                var userFollowFundViewModels = s.GroupBy(p => p.FundCode).Select(p => new UserFollowFundViewModel
                {
                    FundCode = p.Key,
                    FundName = p.First().FundName,
                    FollowFundValues = p.Select(h1 => new FollowFundValue
                    {
                        RealValue = h1.RealValue,
                        UpdateTime = h1.UpdateTime
                    })
                });
                return userFollowFundViewModels;
            }
            return  null;
        }
    }
}
