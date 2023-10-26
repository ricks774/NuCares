using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static NuCares.Models.EnumList;

namespace NuCares.Models
{
    public class ViewNutritionist
    {
        /// <summary>
        /// 是否公開
        /// </summary>
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
        [StringLength(50, MinimumLength = 1, ErrorMessage = "{0}長度必需介于 1 到 50 字")]
        [Display(Name = "顯示名稱")]
        public string Title { get; set; }

        /// <summary>
        /// 所在縣市 0 ~ 22 由北到南
        /// </summary>
        [Display(Name = "所在縣市")]
        public string City { get; set; }

        /// <summary>
        /// 專長
        /// </summary>
        [Display(Name = "專長")]
        public List<string> Expertise { get; set; }

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
        [Display(Name = "其他通訊軟體 1")]
        public string Option1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 1 ID")]
        public string OptionId1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 2")]
        public string Option2 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 2 ID")]
        public string OptionId2 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 3")]
        public string Option3 { get; set; }

        [MaxLength(100)]
        [Display(Name = "其他通訊軟體 3 ID")]
        public string OptionId3 { get; set; }
    }
}