using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewCourseTime
    {
        /// <summary>
        /// 課程起始日 yyyy/MM/dd
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "課程起始日")]
        public DateTime? CourseStartDate { get; set; }

        /// <summary>
        /// 課程結束日 yyyy/MM/dd
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "課程結束日")]
        public DateTime? CourseEndDate { get; set; }
    }
}