using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewUser
    {
        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,12}$", ErrorMessage = "密碼必須為6-12位英文字母和數字的組合")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "{0}格式錯誤")]
        //[Index(IsUnique = true)]
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
        public string Gender { set; get; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "手機號碼")]
        public string Phone { get; set; }
    }

    public class ViewEmailCheck
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "{0}格式錯誤")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,12}$", ErrorMessage = "密碼必須為6-12位英文字母和數字的組合")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼確認")]
        public string RePassword { get; set; }
    }

    public class ViewUserLogin
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "{0}格式錯誤")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }

    public class ViewUserInfoEdit
    {
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
        public string Gender { set; get; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [RegularExpression(@"^09\d{8}$", ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "手機號碼")]
        public string Phone { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        [MaxLength(500)]
        [Display(Name = "照片")]
        public string ImgUrl { get; set; }
    }

    public class ViewUserPassChange
    {
        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,12}$", ErrorMessage = "密碼必須為6-12位英文字母和數字的組合")]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        /// <summary>
        /// 密碼, 必須為6-12位英文字母和數字的組合
        /// </summary>
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼確認")]
        public string RePassword { get; set; }
    }
}