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

    public class NewebPayReturn
    {
        public string Status { get; set; }
        public string MerchantID { get; set; }
        public string Version { get; set; }
        public string TradeInfo { get; set; }
        public string TradeSha { get; set; }
    }

    public class PaymentResult
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public string MerchantID { get; set; }
        public string Amt { get; set; }
        public string TradeNo { get; set; }
        public string MerchantOrderNo { get; set; }
        public string RespondType { get; set; }
        public string IP { get; set; }
        public string EscrowBank { get; set; }
        public string PaymentType { get; set; }
        public string RespondCode { get; set; }
        public string Auth { get; set; }
        public string Card6No { get; set; }
        public string Card4No { get; set; }
        public string Exp { get; set; }
        public string TokenUseStatus { get; set; }
        public string InstFirst { get; set; }
        public string InstEach { get; set; }
        public string Inst { get; set; }
        public string ECI { get; set; }
        public string PayTime { get; set; }
        public string PaymentMethod { get; set; }
    }
}