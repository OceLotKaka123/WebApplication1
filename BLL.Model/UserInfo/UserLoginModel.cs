using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 用户登入信息
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// 用户电话
        /// </summary>
        [Required]
        [Phone]
        public string TelPhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string PassWord { get; set; }
    }
}
