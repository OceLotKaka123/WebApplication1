using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///FundCode
	 ///</summary>
	 [Table("FundCode")]	
	 public class FundCodeEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
		 /// <summary>
        /// 基金编码
        /// </summary>
		[Required]
		public string Code { get; set; }
	 
	 }
}	 
