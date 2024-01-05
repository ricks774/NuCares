using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.Provider;
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
        public void Send(string userName)
        {
            // 檢查目前的 ConnectionId
            Clients.All.notify($"{userName}的connectionId: {Context.ConnectionId}");

            var keys = Users.ConnectionIds.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                string connectionId = keys[i];
                string name = Users.ConnectionIds[connectionId];

                Clients.All.notify($"ConnectionIds[{i}]中，id: {connectionId} name: {name}");
            }

            Clients.All.notify();
        }

        // 傳送訊息
        public void Notify(string message)
        {
            // 在這裡可以進一步處理通知，例如記錄日誌或進行其他操作
            Clients.All.notify(message);
        }

        #region "使用者連線時，取得 connectionId"

        // 宣告靜態類別，儲存 user 清單
        public static class Users
        {
            public static Dictionary<string, string> ConnectionIds = new Dictionary<string, string>();
        }

        // 新使用者連線時
        public void UserConnected(string id)
        {
            // 將目前使用者新增至 user 清單
            // 如果已經存在相同使用者Id，則更新該使用者的連線 connectionId
            if (Users.ConnectionIds.ContainsKey(id))
            {
                Users.ConnectionIds[id] = Context.ConnectionId;
            }
            else
            {
                // 將目前使用者新增至 user 清單
                Users.ConnectionIds.Add(id, Context.ConnectionId);
            }
            //// 發送給所有人，取得user清單
            //Clients.All.getList(Users.ConnectionIds.Select(u => new { id = u.Key, name = u.Value }).ToList());
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
    }
}