using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class BodyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Display(Name = "課程")]
        public int CourseId { get; set; }
        [JsonIgnore]
        [ForeignKey("CourseId")]
        [Display(Name = "所屬課程")]
        public virtual Course Course { get; set; }

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

        [Display(Name = "新增日期")]
        public DateTime InsertDate { get; set; } = DateTime.Now;

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}