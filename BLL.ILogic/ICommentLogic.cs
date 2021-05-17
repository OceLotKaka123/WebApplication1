using BLL.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ILogic
{
    public interface ICommentLogic
    {
        bool AddSystemCommment(CommentModel commentModel);
        IEnumerable<CommentViewModel> GetAllComments();
    }
}
