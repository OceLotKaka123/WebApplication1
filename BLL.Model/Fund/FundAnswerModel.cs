using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Model
{
    /// <summary>
    /// 基民常问问题
    /// </summary>
    public class FundAnswerModel
    {
        /// <summary>
        /// 基金询问问题
        /// </summary>
        public string AnswerContent { get; set; }

        /// <summary>
        /// 回答基金id
        /// </summary>
        public int AnswerFundId { get; set; }
    }
}
