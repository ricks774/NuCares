using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Course
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Display(Name = "訂單")]
        public int OrderId { get; set; }
        [JsonIgnore]
        [ForeignKey("OrderId")]
        [Display(Name = "所屬訂單")]
        public virtual Order Order { get; set; }

        /// <summary>
        /// 會員 ID
        /// </summary>
        [Display(Name = "會員")]
        public int UserId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        [Display(Name = "所屬會員")]
        public virtual User MyUser { get; set; }

        /// <summary>
        /// 營養師 ID
        /// </summary>
        [Display(Name = "營養師")]
        public int NutritionistId { get; set; }
        [JsonIgnore]
        [ForeignKey("NutritionistId ")]
        [Display(Name = "所屬營養師")]
        public virtual Nutritionist MyNutritionist { get; set; }

        /// <summary>
        /// 目標體重
        /// </summary>
        [Display(Name = "目標體重")]
        public int? GoalWeight { get; set; }

        /// <summary>
        /// 目標體脂
        /// </summary>
        [Display(Name = "目標體脂")]
        public int? GoalBodyFat { get; set; }

        /// <summary>
        /// 課程起始日 yyyy-MM-dd
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "課程起始日")]
        public DateTime? CourseStartDate { get; set; }

        /// <summary>
        /// 課程結束日 yyyy-MM-dd
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "課程結束日")]
        public DateTime? CourseEndDate { get; set; }

        [Display(Name = "課程狀態")]
        public EnumList.CourseState CourseState { get; set; } = 0;

        /// <summary>
        /// 課程問卷是否填寫
        /// </summary>
        [Display(Name = "是否已填問卷")]
        public bool IsQuest { get; set; } = false;

        /// <summary>
        /// 課程是否評價
        /// </summary>
        [Display(Name = "是否評價")]
        public bool IsComment { get; set; } = false;

        /// <summary>
        /// 營養師收入
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "{0}必须為正整数")]
        [Display(Name = "收入")]
        public int? RevenueShare { get; set; }

        /// <summary>
        /// 營養師是否已提領
        /// </summary>
        [Display(Name = "是否提領")]
        public bool IsWithdraw { get; set; } = false;

        [JsonIgnore]
        [Display(Name = "課程問卷")]
        public virtual ICollection<Survey> Surveys { get; set; }

        [JsonIgnore]
        [Display(Name = "課程評論")]
        public virtual ICollection<Comment> Comments { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}