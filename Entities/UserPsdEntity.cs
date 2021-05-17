using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///UserPsd
	 ///</summary>
	 [Table("UserPsd")]	
	 public class UserPsdEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
		 /// <summary>
        /// 用户Id
        /// </summary>
		[Required]
		public int UserId { get; set; }
	 
		 /// <summary>
        /// 用户密码
        /// </summary>
		[Required]
		public string PassWord { get; set; }
	 
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
