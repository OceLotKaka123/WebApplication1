using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [Phone]
        public string TelPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string PassWord { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
		public string AvatarUrl { get; set; }
        /// <summary>
        /// 关注基金
        /// </summary>
        public IEnumerable<FollowFundModel> FollowFundModels { get; set; }
    }
}
