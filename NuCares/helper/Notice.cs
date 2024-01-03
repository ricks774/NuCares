using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using NuCares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Xml.Linq;

namespace NuCares.helper
{
    public class Notice
    {
        public static int AddNotice(NuCaresDBContext db, int userId, string message, string typeId)
        {
            var addNotice = new Notification
            {
                UserId = userId,
                NoticeMessage = message,
                NoticeType = typeId,
            };

            db.Notification.Add(addNotice);
            db.SaveChanges();

            return addNotice.Id;
        }

        public static void SendNotice(string name, string message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (message.Contains("已評價"))
            {
                hub.Clients.All.notify($"{name} 對你進行了評價");
            }
            else if (message.Contains("已購課"))
            {
                hub.Clients.All.notify($"{name} 購買了課程");
            }
            else if (message.Contains("已完成生活問卷"))
            {
                hub.Clients.All.notify($"{name} 問卷填寫完成");
            }
            else if (message.Contains("開始課程"))
            {
                hub.Clients.All.notify($"{name} 開始了課程");
            }
            else if (message.Contains("填寫生活問卷"))
            {
                hub.Clients.All.notify($"{name} 記得填寫生活問卷");
            }
        }

        public static void GetNotice(NuCaresDBContext db, string connectionId, int noticeId, Course courseData)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var noticeData = db.Notification.FirstOrDefault(n => n.Id == noticeId);

            var result = new
            {
                NoticeId = noticeId,
                CourseId = courseData.Id,
                CourseName = courseData.Order.Plan.CourseName,
                NutritionistId = courseData.Order.Plan.NutritionistId,
                Message = noticeData.NoticeMessage,
                Title = courseData.Order.Plan.Nutritionist.Title,
                UserName = courseData.Order.User.UserName,
                Date = noticeData.CreateTime.ToString("yyyy/MM/dd HH:mm"),
                IsRead = noticeData.IsRead
            };

            // 向特定的 connectionId 使用者發送通知
            hub.Clients.Client(connectionId).notify(result);
        }
    }
}