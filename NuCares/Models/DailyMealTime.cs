using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class DailyMealTime
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "每日記錄")]
        public int DailyLogId { get; set; }

        [JsonIgnore]    //LINQ不會產生無限迴圈
        [ForeignKey("DailyLogId")]
        [Display(Name = "每日記錄Id")]
        public virtual DailyLog DailyLogs { get; set; }

        [Display(Name = "三餐時間")]
        public string MealTime { get; set; }

        [MaxLength(200)]
        [Display(Name = "三餐描述")]
        public string MealDescription { get; set; }

        [MaxLength(200)]
        [Display(Name = "三餐照片")]
        public string MealImgUrl { get; set; }

        [Display(Name = "澱粉")]
        public int Starch { get; set; }

        [Display(Name = "蛋白質")]
        public int Protein { get; set; }

        [Display(Name = "蔬菜")]
        public int Vegetable { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}