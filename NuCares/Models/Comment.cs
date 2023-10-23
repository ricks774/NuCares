using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        /// <summary>
        /// 課程 Id
        /// </summary>
        [Display(Name = "課程")]
        public int CourseId { get; set; }
        [JsonIgnore]
        [ForeignKey("CourseId")]
        [Display(Name = "所屬課程")]
        public virtual Course MyCourse { get; set; }

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

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}