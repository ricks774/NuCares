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

    public class ViewDaily
    {
        [Display(Name = "油脂")]
        public int? Oil { get; set; }

        [Display(Name = "油脂描述")]
        public string OilDescription { get; set; }

        [Display(Name = "油脂照片")]
        public string OilImgUrl { get; set; }

        [Display(Name = "水果")]
        public int? Fruit { get; set; }

        [Display(Name = "水果描述")]
        public string FruitDescription { get; set; }

        [Display(Name = "水果照片")]
        public string FruitImgUrl { get; set; }

        [Display(Name = "飲水")]
        public int? Water { get; set; }

        [Display(Name = "飲水描述")]
        public string WaterDescription { get; set; }

        [Display(Name = "飲水照片")]
        public string WaterImgUrl { get; set; }
    }
}