using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class FavoriteList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "會員")]
        public int UserId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        [Display(Name = "追蹤者")]
        public virtual User User { get; set; }

        [Display(Name = "營養師")]
        public int NutritionistId { get; set; }
        [JsonIgnore]
        [ForeignKey("NutritionistId ")]
        [Display(Name = "被追蹤者")]
        public virtual Nutritionist Nutritionist { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}