
namespace WebSocketTool.Test.Model
{
    using System;
    using Newtonsoft.Json;
    using WebSocketUtils.Model;

    public class WebSocketRule : IWebSocketRule<WebSocketUser>
    {
        public WebSocketRule(int port, bool isZlib, int maxmumUsers, int userCollectorNumber) 
            : base(port, isZlib, maxmumUsers, userCollectorNumber)
        {
        }

        public override WebSocketUser CreateInfo(string id)
            => new WebSocketUser(id);

        public override void Error(Exception ex)
        {
            Console.WriteLine($"Error:{ex.Message}");
        }

        public override void Info(string msg)
        {
            Console.WriteLine($"Info:{msg}");
        }

        public override void OnAccept(WebSocketUser userInfo)
        {
            Console.WriteLine($"{userInfo.UserID} OnAccept");
        }

        public override void OnClientClose(WebSocketUser userInfo)
        {
            Console.WriteLine($"{userInfo.UserID} OnClientClose");
        }

        public override WebSocketUser[] OnMessage(WebSocketUser userInfo, string msg)
        {
            try
            {
                // 紀錄用戶最後活動時間
                userInfo.LastTimeActive = DateTime.Now;

                Console.WriteLine($"{userInfo.UserID} OnMessage: {msg}");

                if (msg.IndexOf("Close Signal", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    // 需要剔除得使用者
                    return new WebSocketUser[]
                    {
                        userInfo
                    };
                }
            }
            // 忽略網路問題造成Json解析異常
            catch (JsonReaderException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{userInfo.UserID} OnMessage Exception:{ex.Message}");
            }

            return null;
        }

        public override void OnRecordSend(WebSocketUser userInfo, string msg)
        {
            Console.WriteLine($"{userInfo.UserID} OnRecordSend: {msg}");
        }

        public override void Trace(string msg)
        {
            Console.WriteLine($"{this.GetType().Name} Trace: {msg}");
        }

        public override bool ValidHandShakeHeader(HandShakeInfo handShakeInfo, WebSocketUser userInfo)
        {
            Console.WriteLine($"{userInfo.UserID} ValidHandShakeHeader Url:{handShakeInfo.Path}, Host: {handShakeInfo.Host}");
            return true;
        }

        public override void Warring(string msg)
        {
            Console.WriteLine($"{this.GetType().Name} Warring: {msg}");
        }
    }
}
