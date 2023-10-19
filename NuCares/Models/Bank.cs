using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        /// <summary>
        /// Nutritionist Id
        /// </summary>
        [Display(Name = "營養師")]
        public int NutritionistId { get; set; }
        [JsonIgnore]
        [ForeignKey("NutritionistId")]
        [Display(Name = "所屬營養師")]
        public virtual Nutritionist MyNutritionist { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required(ErrorMessage = "銀行代碼必填")]
        [Display(Name = "銀行代碼")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "請輸入正確的銀行代碼（3位數字）")]
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required(ErrorMessage = "銀行帳號必填")]
        [Display(Name = "銀行帳號")]
        [RegularExpression(@"^\d{10,20}$", ErrorMessage = "請輸入正確的銀行帳號（10至20位數字）")]
        public string BankAccount { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}