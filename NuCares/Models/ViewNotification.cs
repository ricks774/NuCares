using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NuCares.Models
{
    public class ViewNotification
    {
        [Display(Name = "通知訊息")]
        public string NoticeMessage { get; set; }

        [Display(Name = "通知種類")]
        public string NoticeType { get; set; }

        [Display(Name = "是否已讀")]
        public bool IsRead { get; set; }
    }
}