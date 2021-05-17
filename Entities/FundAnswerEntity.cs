using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///FundAnswer
	 ///</summary>
	 [Table("FundAnswer")]	
	 public class FundAnswerEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
		 /// <summary>
        /// 基金询问问题
        /// </summary>
		[Required]
		public string AnswerContent { get; set; }
	 
		 /// <summary>
        /// 回答基金id
        /// </summary>
		[Required]
		public int AnswerFundId { get; set; }
	 
		 /// <summary>
        /// 询问次数
        /// </summary>
		public int? AnswerNumber { get; set; }
	 
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
