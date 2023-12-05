using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "會員")]
        public int UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        [Display(Name = "會員")]
        public virtual User User { get; set; }

        [Display(Name = "通知訊息")]
        public string NoticeMessage { get; set; }

        [Display(Name = "通知種類")]
        public string NoticeType { get; set; }

        [Display(Name = "是否已讀")]
        public bool? IsRead { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}