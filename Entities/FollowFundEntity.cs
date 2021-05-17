using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///FollowFund
	 ///</summary>
	 [Table("FollowFund")]	
	 public class FollowFundEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
		 /// <summary>
        /// 用户id
        /// </summary>
		[Required]
		public int UserId { get; set; }
	 
		 /// <summary>
        /// 基金编码
        /// </summary>
		[Required]
		public string FundCode { get; set; }
	 
		 /// <summary>
        /// 是否有效
        /// </summary>
		public int? Valid { get; set; }
	 
		 /// <summary>
        /// 唯一标识
        /// </summary>
		public Guid? Uuid { get; set; }
	 
	 }
}	 
