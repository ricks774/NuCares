using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewBodyInfo
    {
        [Required(ErrorMessage = "{0}必填")]
        [Range(0, 300, ErrorMessage = "{0}在 0 到 300 之間")]
        [Display(Name = "身高")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Range(0, 500, ErrorMessage = "{0}在 0 到 500 之間")]
        [Display(Name = "體重")]
        public decimal Weight { get; set; }

        /// <summary>
        /// 體脂肪
        /// </summary>
        [Display(Name = "體脂肪")]
        public decimal? BodyFat { get; set; }

        /// <summary>
        /// 內臟脂肪
        /// </summary>
        [Display(Name = "內臟脂肪")]
        public int? VisceralFat { get; set; }

        /// <summary>
        /// 骨骼肌率
        /// </summary>
        [Display(Name = "骨骼肌率")]
        public decimal? SMM { get; set; }

        /// <summary>
        /// BMI
        /// </summary>
        [Display(Name = "BMI")]
        public decimal? Bmi { get; set; }

        /// <summary>
        /// 基礎代謝率
        /// </summary>
        [Display(Name = "BMR")]
        public int? Bmr { get; set; }
    }
}