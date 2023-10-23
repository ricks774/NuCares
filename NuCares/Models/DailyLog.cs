using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NuCares.Models
{
    public class DailyLog
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]    //LINQ不會產生無限迴圈
        [ForeignKey("DailyCourseMenuId")]
        [Display(Name = "訂單編號")]
        public int DailyCourseMenuId { get; set; }

        public virtual ICollection<DailyCourseMenu> DailyCourseMenus { get; set; }

        [MaxLength(2)]
        [Display(Name = "油脂")]
        public int Oil { get; set; } = 0;

        [Display(Name = "油脂描述")]
        public string OilDescription { get; set; }

        [Display(Name = "油脂照片")]
        public string OilImgUrl { get; set; }

        [MaxLength(2)]
        [Display(Name = "水果")]
        public int Fruit { get; set; } = 0;

        [Display(Name = "水果描述")]
        public string FruitDescription { get; set; }

        [Display(Name = "水果照片")]
        public string FruitImgUrl { get; set; }

        [MaxLength(2)]
        [Display(Name = "飲水")]
        public int Water { get; set; } = 0;

        [Display(Name = "飲水描述")]
        public string WaterDescription { get; set; }

        [Display(Name = "飲水照片")]
        public string WaterImgUrl { get; set; }

        [Display(Name = "紀錄日期")]
        public DateTime InsertDate { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}