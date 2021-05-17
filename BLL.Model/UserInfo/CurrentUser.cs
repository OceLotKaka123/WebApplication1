using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户Redis信息
        /// </summary>
        [PrimaryKey]
		public string UserCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
		public string TelPhone { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
		public string AvatarUrl { get; set; }
        /// <summary>
        /// token信息
        /// </summary>
        public string Token { get; set; }
    }
}
