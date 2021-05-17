using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 系统评论
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// 评论意见
        /// </summary>
        [Required]
        public string ComentContent { get; set; }
    }
}
