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
        public virtual Course Course { get; set; }

        [Display(Name = "Question1")]
        public string Question1 { get; set; }

        [Display(Name = "Question2")]
        public string Question2 { get; set; }

        [Display(Name = "Question3")]
        public string Question3 { get; set; }

        [Display(Name = "Question4")]
        public string Question4 { get; set; }

        [Display(Name = "Question5")]
        public string Question5 { get; set; }

        [Display(Name = "Question6")]
        public string Question6 { get; set; }

        [Display(Name = "Question7")]
        public string Question7 { get; set; }

        [Display(Name = "Question8")]
        public string Question8 { get; set; }

        [Display(Name = "Question9")]
        public string Question9 { get; set; }

        [Display(Name = "Question10")]
        public string Question10 { get; set; }

        [Display(Name = "Question11")]
        public string Question11 { get; set; }

        [Display(Name = "Question12")]
        public string Question12 { get; set; }

        [Display(Name = "Question13")]
        public string Question13 { get; set; }

        [Display(Name = "Question14")]
        public string Question14 { get; set; }

        [Display(Name = "Question15")]
        public string Question15 { get; set; }

        [Display(Name = "Question16")]
        public string Question16 { get; set; }

        [Display(Name = "Question17")]
        public string Question17 { get; set; }

        [Display(Name = "Question18")]
        public string Question18 { get; set; }

        [Display(Name = "Question19")]
        public string Question19 { get; set; }

        [Display(Name = "Question20")]
        public string Question20 { get; set; }

        [Display(Name = "Question21")]
        public string Question21 { get; set; }

        [Display(Name = "Question22")]
        public string Question22 { get; set; }

        [Display(Name = "Question23")]
        public string Question23 { get; set; }

        [Display(Name = "Question24")]
        public string Question24 { get; set; }

        [Display(Name = "Question25")]
        public string Question25 { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}