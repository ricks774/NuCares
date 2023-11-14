using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewComment
    {
        /// <summary>
        /// 評論內容
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(50, ErrorMessage = "{0}內容最多為 50 個字")]
        [Display(Name = "說明")]
        public string Content { get; set; }

        /// <summary>
        /// 評分
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Range(1, 5, ErrorMessage = "{0}必須在 1 到 5 之間")]
        [Display(Name = "評分")]
        public int Rate { get; set; }
    }
}