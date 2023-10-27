using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewDailyMealTime
    {
        [MaxLength(200)]
        [Display(Name = "三餐描述")]
        public string MealDescription { get; set; }

        [MaxLength(200)]
        [Display(Name = "三餐照片")]
        public string MealImgUrl { get; set; }

        [Display(Name = "澱粉")]
        public int? Starch { get; set; }

        [Display(Name = "蛋白質")]
        public int? Protein { get; set; }

        [Display(Name = "蔬菜")]
        public int? Vegetable { get; set; }
    }
}