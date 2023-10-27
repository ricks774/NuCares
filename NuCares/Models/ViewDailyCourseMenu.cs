using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewDailyCourseMenu
    {

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
    }
}