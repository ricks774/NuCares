using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewPlan
    {
        /// <summary>
        /// 課程排序
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "{0}必須為正整数")]
        [Display(Name = "排序順序")]
        public int? Rank { get; set; } = 100;

        /// <summary>
        /// 課程名稱
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(18, ErrorMessage = "{0}名稱最多 18 個字")]
        [Display(Name = "課程名稱")]
        public string CourseName { get; set; }

        /// <summary>
        /// 課程週數
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "{0}必須為正整数")]
        [Display(Name = "週數")]
        public int CourseWeek { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "{0}必須為正整数")]
        [Display(Name = "價格")]
        public int CoursePrice { get; set; }

        /// <summary>
        /// 課程說明
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100, ErrorMessage = "{0}最多 100 個字")]
        [Display(Name = "課程說明")]
        public string Detail { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        [MaxLength(500)]
        [Display(Name = " 標籤")]
        public string Tag { get; set; }
    }

    public class ViewEditPlan
    {
        /// <summary>
        /// 課程排序
        /// </summary>
        [Display(Name = "排序順序")]
        public int? Rank { get; set; }

        /// <summary>
        /// 課程名稱
        /// </summary>
        [Display(Name = "課程名稱")]
        public string CourseName { get; set; }

        /// <summary>
        /// 課程週數
        /// </summary>
        [Display(Name = "週數")]
        public int CourseWeek { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        [Display(Name = "價格")]
        public int CoursePrice { get; set; }

        /// <summary>
        /// 課程說明
        /// </summary>
        [StringLength(100, ErrorMessage = "{0}最多 100 個字")]
        [Display(Name = "課程說明")]
        public string Detail { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        [MaxLength(500)]
        [Display(Name = " 標籤")]
        public string Tag { get; set; }
    }
}