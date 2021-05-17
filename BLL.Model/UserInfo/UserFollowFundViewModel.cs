using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 用户关注基金信息
    /// </summary>
    public class UserFollowFundViewModel:FollowFundViewModel
    {
        /// <summary>
        /// 关注基金的值
        /// </summary>
        public IEnumerable<FollowFundValue> FollowFundValues { get; set; }
    }

    /// <summary>
    /// 关注基金的值
    /// </summary>
    public class FollowFundValue
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 真实值
        /// </summary>
		public string RealValue { get; set; }
    }
}
