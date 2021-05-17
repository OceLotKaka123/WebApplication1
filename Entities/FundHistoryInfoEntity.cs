using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///FundHistoryInfo
	 ///</summary>
	 [Table("FundHistoryInfo")]	
	 public class FundHistoryInfoEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
		 /// <summary>
        /// 基金信息id
        /// </summary>
		[Required]
		public int FundId { get; set; }
	 
		 /// <summary>
        /// 近一个月
        /// </summary>
		public string OneMonthValue { get; set; }
	 
		 /// <summary>
        /// 近三个月
        /// </summary>
		public string ThreeMonthValue { get; set; }
	 
		 /// <summary>
        /// 近六个月
        /// </summary>
		public string SixMonthValue { get; set; }
	 
		 /// <summary>
        /// 近一年
        /// </summary>
		public string OneYear { get; set; }
	 
		 /// <summary>
        /// 近三年
        /// </summary>
		public string ThreeYear { get; set; }
	 
		 /// <summary>
        /// 成立来
        /// </summary>
		public string FoundValue { get; set; }
	 
		 /// <summary>
        /// 基金类型
        /// </summary>
		public string FundType { get; set; }
	 
		 /// <summary>
        /// 基金风险
        /// </summary>
		public string FundRisk { get; set; }
	 
		 /// <summary>
        /// 基金规模
        /// </summary>
		public string FundScale { get; set; }
	 
		 /// <summary>
        /// 基金规模日期
        /// </summary>
		public DateTime? FundScaleDate { get; set; }
	 
		 /// <summary>
        /// 基金经理
        /// </summary>
		public string Director { get; set; }
	 
		 /// <summary>
        /// 成立日
        /// </summary>
		public DateTime? FoundDate { get; set; }
	 
		 /// <summary>
        /// 管理人
        /// </summary>
		public string Manager { get; set; }
	 
	 }
}	 
