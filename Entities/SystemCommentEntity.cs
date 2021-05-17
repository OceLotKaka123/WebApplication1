using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///SystemComment
	 ///</summary>
	 [Table("SystemComment")]	
	 public class SystemCommentEntity:BaseEntity
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
        /// 评论意见
        /// </summary>
		public string ComentContent { get; set; }
	 
		 /// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
	 
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
