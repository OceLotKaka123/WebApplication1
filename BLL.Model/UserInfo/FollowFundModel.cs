using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 关注基金
    /// </summary>
    public class FollowFundModel
    {
        /// <summary>
        /// 基金编码
        /// </summary>
        [Required]
        public string FundCode { get; set; }
    }
}
