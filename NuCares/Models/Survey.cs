using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "課程")]
        public int CourseId { get; set; }
        [JsonIgnore]
        [ForeignKey("CourseId")]
        [Display(Name = "所屬課程")]
        public virtual User MyUser { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question1")]
        public string Question1 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question2")]
        public string Question2 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question3")]
        public string Question3 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question4")]
        public string Question4 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question5")]
        public string Question5 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question6")]
        public string Question6 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question7")]
        public string Question7 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question8")]
        public string Question8 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question9")]
        public string Question9 { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "Question10")]
        public string Question10 { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}