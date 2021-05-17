using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
	 ///<summary>
	 ///GeneraCodes
	 ///</summary>
	 [Table("GeneraCodes")]	
	 public class GeneraCodesEntity:BaseEntity
	 {
	  
		 /// <summary>
        /// 自增id
        /// </summary>
		[Key]
		[Required]
		public int Id { get; set; }
	 
		 /// <summary>
        /// 编码名称
        /// </summary>
		[Required]
		public string CodeName { get; set; }
	 
		 /// <summary>
        /// 开始标识
        /// </summary>
		[Required]
		public string StartSign { get; set; }
	 
		 /// <summary>
        /// 编码最小值
        /// </summary>
		[Required]
		public int MinNum { get; set; }
	 
		 /// <summary>
        /// 编码最大值
        /// </summary>
		[Required]
		public int MaxNum { get; set; }
	 
		 /// <summary>
        /// 编码当前值
        /// </summary>
		[Required]
		public int CurrentNum { get; set; }
	 
		 /// <summary>
        /// 唯一标识
        /// </summary>
		public Guid? Uuid { get; set; }
	 
	 }
}	 
