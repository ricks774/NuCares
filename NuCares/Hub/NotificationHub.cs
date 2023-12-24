using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // 取得連線的帳號 ConnectionId
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public void SendMessageToClient(string connectionId, string message)
        {
            Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
}