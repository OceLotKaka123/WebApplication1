using BLL.ILogic;
using BLL.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class CommentController : ApiBaseController
    {
        private readonly ICommentLogic _commentLogic;
        public CommentController(ICommentLogic commentLogic)
        { 
            _commentLogic = commentLogic;
        }
        /// <summary>
        /// 添加系统评论信息
        /// </summary>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddSystemCommment([Required]CommentModel commentModel)
        {
            return  _commentLogic.AddSystemCommment(commentModel);
        }
        /// <summary>
        /// 获取系统评论信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CommentViewModel> GetAllComments()
        {
            return  _commentLogic.GetAllComments();
        }
    }
}
