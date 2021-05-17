using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///BaseFund
	 ///</summary>
	 [Table("BaseFund")]	
	 public class BaseFundEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
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
		[Required]
		public string FundName { get; set; }
	 
		 /// <summary>
        /// 基金编码
        /// </summary>
		[Required]
		public string FundCode { get; set; }
	 
		 /// <summary>
        /// 日期
        /// </summary>
		public DateTime? DateNow { get; set; }
	 
	 }
}	 
