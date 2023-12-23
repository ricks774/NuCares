using Microsoft.AspNet.SignalR;
using NuCares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;

namespace NuCares.helper
{
    public class Notice
    {
        public static void AddNotice(NuCaresDBContext db, int userId, string message, string typeId)
        {
            var addNotice = new Notification
            {
                UserId = userId,
                NoticeMessage = message,
                NoticeType = typeId,
            };

            db.Notification.Add(addNotice);
            db.SaveChanges();
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
    }
}