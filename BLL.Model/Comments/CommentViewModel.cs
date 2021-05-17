using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 评论信息
    /// </summary>
    public class CommentViewModel:CommentModel
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
}
