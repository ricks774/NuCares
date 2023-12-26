using Microsoft.AspNet.SignalR;
using NuCares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NuCares
{
    public class NotificationHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void SendMessage(string user, string message)
        {
            Clients.All.ReceiveMessage(user, message);
        }

        public void Notify(string message)
        {
            // 在這裡可以進一步處理通知，例如記錄日誌或進行其他操作
            Clients.All.notify(message);
        }

        #region "使用者連線時，取得 connectionId"

        //宣告靜態類別，儲存user清單
        public static class Users
        {
            public static Dictionary<string, string> ConnectionIds = new Dictionary<string, string>();
        }

        // 新使用者連線時
        public void userConnected(string name)
        {
            // 將目前使用者新增至user清單
            Users.ConnectionIds.Add(Context.ConnectionId, name);

            // 發送給所有人，取得user清單
            Clients.All.getList(Users.ConnectionIds.Select(u => new { id = u.Key, name = u.Value }).ToList());
        }

        // 當使用者斷線時呼叫
        // stopCalled是SignalR 2.1.0版新增的參數
        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.Others.removeList(Context.ConnectionId);
            Users.ConnectionIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        #endregion "使用者連線時，取得 connectionId"

        //傳送訊息給某人
        public void SendOne(string id, string message)
        {
            var from = Users.ConnectionIds.Where(u => u.Key == Context.ConnectionId).FirstOrDefault();
            //var to = Users.ConnectionIds.Where(u => u.Key == id).FirstOrDefault();

            Clients.Client(id).show("<span style='color:red'>" + from.Value + "密你:" + message + "</span>");
        }
    }
}