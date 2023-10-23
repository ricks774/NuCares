using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "照片")]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100, ErrorMessage = "{0} 長度至少必須為 {2} 個字元。", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,12}$", ErrorMessage = "{0}必須為6-12位英文字母和數字的組合")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [MaxLength(100)]
        [Display(Name = "密碼鹽")]
        public string Salt { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "{0}格式錯誤")]
        [Index(IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 生日 yyyy/MM/dd
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "生日")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性別：0 男, 1 女
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "生理性別")]
        public EnumList.GenderType Gender { set; get; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "手機號碼")]
        public string Phone { get; set; }

        /// <summary>
        /// Line Id
        /// </summary>
        [MaxLength(50)]
        [Display(Name = "Line ID")]
        public string LineId { get; set; }

        /// <summary>
        /// 是否為營養師
        /// </summary>
        [Display(Name = "是否營養師")]
        public bool IsNutritionist { get; set; } = false;

        [JsonIgnore]
        [Display(Name = "會員訂單")]
        public virtual ICollection<Order> Orders { get; set; }

        [JsonIgnore]
        [Display(Name = "會員評論")]
        public virtual ICollection<Comment> Comments { get; set; }

        [JsonIgnore]
        [Display(Name = "追蹤者")]
        public virtual ICollection<FavoriteList> FavoriteLists { get; set; }

        /// <summary>
        /// 營養師證照圖
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "證照圖片")]
        public string CertificateImage { get; set; }

        public Nutritionist Nutritionist { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}