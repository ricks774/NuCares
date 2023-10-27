using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewOrder
    {
        /// <summary>
        /// 聯絡時間
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "聯絡時間")]
        public string ContactTime { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(10, ErrorMessage = "{0}最多10個字符")]
        [Display(Name = "付款方式")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 發票
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(10, ErrorMessage = "{0}最多10個字符")]
        [Display(Name = "發票")]
        public string Invoice { get; set; }

        /// <summary>
        /// 購買人姓名
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(50, ErrorMessage = "姓名最多50個字符")]
        [Display(Name = "姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 購買人 Email
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "{0}格式錯誤")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        /// <summary>
        /// 購買人手機號碼
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "手機號碼")]
        public string UserPhone { get; set; }

        /// <summary>
        /// 購買人 Line ID
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100, ErrorMessage = "{0}最多100個字")]
        [Display(Name = "Line ID")]
        public string UserLineId { get; set; }
    }
}