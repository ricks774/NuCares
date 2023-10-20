using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Plan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        /// <summary>
        /// 營養師 Id
        /// </summary>
        [Display(Name = "營養師")]
        public int NutritionistId { get; set; }
        [JsonIgnore]
        [ForeignKey("NutritionistId ")]
        [Display(Name = "所屬營養師")]
        public virtual Nutritionist MyNutritionist { get; set; }

        /// <summary>
        /// 課程排序
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "{0}必須為正整数")]
        [Display(Name = "排序順序")]
        public int? Rank { get; set; }

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
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "週數")]
        public int CourseWeek { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Range(0, int.MaxValue, ErrorMessage = "{0}必須為正整数")]
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
        /// 標籤：0 入門首選, 1 最超值
        /// </summary>
        [Display(Name = " 標籤")]
        public EnumList.PlanTag Tag { get; set; }


        [JsonIgnore]
        [Display(Name = "課程訂單")]
        public virtual ICollection<Order> Orders { get; set; }


        [Display(Name = " 已刪除")]
        public bool IsDelete { get; set; } = false;


        [Display(Name = " 建立日期")]

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}