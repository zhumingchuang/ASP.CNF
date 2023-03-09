using Microsoft.AspNetCore.SignalR;

namespace CNF.Infrastructure.Hubs;

public class UserLoginNotifiHub:Hub
{
    /*
1.登陆成功前端调用SaveCurrentUserInfo，将userid，过期时间，signalr连接时间存入字典
2.根据属性判断是否过期
3.借助定时服务查询是否过期，过期后推送客户端，前端接收到后弹出登录到期提示
4.存在的问题，服务端重启后主动推送信息会丢失
解决方案：过期时间存入浏览器localstrage；轮询增加一个hub连接到客户端，客户端发送给服务端更新。暂不处理
*/
    public static Dictionary<int, CurrentUserHub> currentUsers = new Dictionary<int, CurrentUserHub>();
    public async Task SaveCurrentUserInfo(int userId, bool isLogin)
    {
        string currentConnectionId = Context.ConnectionId;
        if (currentUsers.ContainsKey(userId))
        {
            //如果是同一个用户且是不同的客户端登录，那么给客户端发送通知（下线）
            string upConnectionId = currentUsers[userId].ConnectionId;

            if (!upConnectionId.Equals(currentConnectionId) && isLogin == true)
            {
                //向指定的用户发送                   
                await Clients.Client(upConnectionId).SendAsync("ReceiveMessage", userId);
                currentUsers[userId].ConnectionId = currentConnectionId;
            }
            else
            {
                //此刻这里存储用户的token
                currentUsers[userId].UserId = userId;
                currentUsers[userId].IsLogin = isLogin;
                currentUsers[userId].ConnectionId = currentConnectionId;
            }
        }
        else
        {
            currentUsers.Add(userId, new CurrentUserHub() { UserId = userId, IsLogin = isLogin, ConnectionId = currentConnectionId, LastLoginTime = DateTime.Now });
        }
    }
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public void Clear()
    {
        currentUsers.Clear();
    }
}

public class CurrentUserHub
{
    public int UserId { get; set; }
    public string ConnectionId { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public bool IsLogin { get; set; }
}