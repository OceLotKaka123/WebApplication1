using BLL.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.ILogic
{
    public interface ICommentSql
    {
        IEnumerable<CommentViewModel> GetAllComments();
    }
}
