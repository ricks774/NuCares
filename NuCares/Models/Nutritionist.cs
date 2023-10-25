using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Nutritionist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }


        /// <summary>
        /// 是否公開
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "是否公開")]
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// 形象照
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "形象照")]
        public string PortraitImage { get; set; }

        /// <summary>
        /// 顯示名稱
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "顯示名稱")]
        public string Title { get; set; }

        /// <summary>
        /// 所在縣市 0 ~ 22 由北到南
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "所在縣市")]
        public EnumList.CityName City { get; set; }

        /// <summary>
        /// 專長；0  體重控制, 1 上班族營養, 2 孕期營養, 3  樂齡營養與保健
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "專長")]
        public List<EnumList.ExpertiseType> Expertise { get; set; }

        /// <summary>
        /// 學歷
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "學歷")]
        public string Education { get; set; }

        /// <summary>
        /// 經歷
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "經歷")]
        public string Experience { get; set; }

        /// <summary>
        /// 關於我
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "關於我")]
        public string AboutMe { get; set; }

        /// <summary>
        /// 課程介紹
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "課程介紹")]
        public string CourseIntro { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體1")]
        public string Option1{ get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 ID 1")]
        public string OptionId1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體21")]
        public string Option2 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 ID 2")]
        public string OptionId2 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體31")]
        public string Option3 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 ID 3")]
        public string OptionId3 { get; set; }

        [JsonIgnore]
        [Display(Name = "營養師課程方案")]
        public virtual ICollection<Plan> Plans { get; set; }

        [JsonIgnore]
        [Display(Name = "追蹤營養師")]
        public virtual ICollection<FavoriteList> FavoriteLists { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}