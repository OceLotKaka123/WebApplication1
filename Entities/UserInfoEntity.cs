using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///UserInfo
	 ///</summary>
	 [Table("UserInfo")]	
	 public class UserInfoEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增Id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
		 /// <summary>
        /// 用户编码
        /// </summary>
		public string UserCode { get; set; }
	 
		 /// <summary>
        /// 头像Url
        /// </summary>
		public string AvatarUrl { get; set; }
	 
		 /// <summary>
        /// 姓名
        /// </summary>
		[Required]
		public string Name { get; set; }
	 
		 /// <summary>
        /// 电话号码
        /// </summary>
		public string TelPhone { get; set; }
	 
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
