using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        /// <summary>
        /// 課程 Id
        /// </summary>
        [Display(Name = "課程方案")]
        public int PlanId { get; set; }
        [JsonIgnore]
        [ForeignKey("PlanId")]
        [Display(Name = "所屬課程方案")]
        public virtual Plan MyPlan { get; set; }

        /// <summary>
        /// 會員 Id
        /// </summary>
        [Display(Name = "會員")]
        public int UserId { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        [Display(Name = "所屬會員")]
        public virtual User MyUser { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "訂單編號")]
        public string OrderNumber { get; set; }

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
        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "{0}格式錯誤")]
        [Index(IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        /// <summary>
        /// 購買人手機號碼
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
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

        /// <summary>
        /// 是否付款
        /// </summary>
        [Display(Name = "是否付款")]
        public bool IsPayment { get; set; } = false;

        [JsonIgnore]
        [Display(Name = "訂單課程")]
        public virtual ICollection<Course> Courses { get; set; }


        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}