using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 基金redis信息
    /// </summary>
    public class FundRedisModel
    {
        /// <summary>
        /// 自增id
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }
        /// <summary>
        /// 基金编码
        /// </summary>
        public string FundCode { get; set; }
        /// <summary>
        /// 真实值
        /// </summary>
		public string RealValue { get; set; }

        /// <summary>
        /// 真实百分之
        /// </summary>
        public string RealPercentValue { get; set; }

        /// <summary>
        /// 估计值
        /// </summary>
        public string ShamValue { get; set; }

        /// <summary>
        /// 累计净值
        /// </summary>
        public string TotalValue { get; set; }

        /// <summary>
        /// 基金名称
        /// </summary>
        public string FundName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? DateNow { get; set; }
    }
}
