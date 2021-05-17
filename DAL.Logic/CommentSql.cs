using BLL.Model;
using DAL.ILogic;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DAL.Logic
{
    public class CommentSql : ICommentSql
    {
        private readonly SqlConnection _con;
        public CommentSql(SqlConnection con)
        {
            _con = con;
        }
        public IEnumerable<CommentViewModel> GetAllComments()
        {
            return _con.Query<CommentViewModel>(@"select 
comment.UserId,comment.ComentContent,comment.CreateTime,
userInfo.AvatarUrl,userInfo.Name
 from SystemComment comment join UserInfo userInfo
  on userInfo.Id=comment.UserId where userInfo.Valid=1 
and comment.Valid=1 order by comment.Id desc");
        }
    }
}
