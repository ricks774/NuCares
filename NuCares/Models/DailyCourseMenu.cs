using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class DailyCourseMenu
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // TODO 尚未連接Course
        //[JsonIgnore]    //LINQ不會產生無限迴圈
        //[ForeignKey("CourseId")]   // 外鍵FK
        //[Display(Name = "訂單編號")]
        //public int CourseId { get; set; }
        //public virtual Course Courses { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "菜單日期")]
        public DateTime MenuDate { get; set; }

        [Display(Name = "澱粉")]
        public string Starch { get; set; }

        [Display(Name = "蛋白質")]
        public string Protein { get; set; }

        [Display(Name = "蔬菜")]
        public string Vegetable { get; set; }

        [Display(Name = "油脂")]
        public int Oil { get; set; } = 0;

        [Display(Name = "水果")]
        public int Fruit { get; set; } = 0;

        [Display(Name = "飲水")]
        public int Water { get; set; } = 0;

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "每日log")]
        public virtual ICollection<DailyLog> DailyLogs { get; set; }
    }
}