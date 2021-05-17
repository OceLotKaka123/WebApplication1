using AutoMapper;
using BLL.ILogic;
using BLL.Model;
using DAL.ILogic;
using Entities;
using Extension;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Logic
{
    public class CommentLogic : ICommentLogic
    {
        private readonly SqlConnection _con;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly ICommentSql _commentSql;
        public CommentLogic(SqlConnection con,IMapper mapper,CurrentUser currentUser,ICommentSql commentSql)
        {
            _con = con;
            _mapper = mapper;
            _currentUser = currentUser;
            _commentSql = commentSql;
        }
        public bool AddSystemCommment(CommentModel commentModel)
        {

            var entity = _mapper.Map<SystemCommentEntity>(commentModel);
            entity.UserId = _currentUser.Id;
            _con.AddRecord(entity);
            return true;
        }

        public IEnumerable<CommentViewModel> GetAllComments()
        { 
            return _commentSql.GetAllComments();
        }
    }
}
