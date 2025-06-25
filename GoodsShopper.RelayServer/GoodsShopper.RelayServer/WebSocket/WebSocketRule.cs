using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using GoodsShopper.RelayServer.Model;
using GoodsShopper.RelayServer.Model.Service;
using Live.Libs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using WebSocketUtils.Model;

namespace GoodsShopper.RelayServer.WebSocket
{
    public class WebSocketRule : IWebSocketRule<User>
    {
        /// <summary>
        /// 紀錄Log
        /// </summary>
        private readonly ILogger Logger = LogManager.GetLogger("RelayServer");

        /// <summary>
        /// 指令集合
        /// </summary>
        private readonly IIndex<string, Command.ICommand> cmdSet;

        /// <summary>
        /// config service
        /// </summary>
        private readonly IConfigService configSvc;

        /// <summary>
        /// action 日誌排除對象
        /// </summary>
        private readonly string[] actionLogIgnore = { "checkSN" };

        /// <summary>
        /// action 維護時例外處理指令
        /// </summary>
        private readonly string[] actionMaintainIgnore = { "checkSN" };

        /// <summary>
        /// 連線對象日誌篩選
        /// </summary>
        private readonly UserType[] userTypeLogFilter = { UserType.Anchor, UserType.Company };

        public WebSocketRule(
            IConfigService configSvc,
            IIndex<string, Command.ICommand> cmdSet,
            int port = 6125,
            bool isZlib = false,
            int maxmumUsers = 100,
            int userCollectorNumber = 1)
            : base(port, isZlib, maxmumUsers, userCollectorNumber)
        {
            this.configSvc = configSvc;
            this.cmdSet = cmdSet;
        }

        public override User CreateInfo(string id)
        => new User(id, configSvc);

        public override void Error(Exception ex)
        {
            this.Logger.Error(ex, $"{this.GetType().Name} Error ");
        }

        public override void Info(string msg)
        {
            this.Logger.Info($"{this.GetType().Name} Info: {msg}");
        }

        public override void OnAccept(User userInfo)
        {
        }

        public override void OnClientClose(User userInfo)
        {
            this.Logger.Info($"{this.GetType().Name} OnCloseClientSocket " +
            $"Account: {userInfo.Account}, " +
            $"Guid: {userInfo.Guid}, " +
            $"UserType: {userInfo.UserType}");
        }

        public override User[] OnMessage(User userInfo, string msg)
        {
            try
            {
                // 紀錄用戶最後活動時間
                userInfo.LastTimeActive = DateTime.Now;

                if (msg.IndexOf("Close Signal", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    // 需要剔除得使用者
                    return new User[]
                    {
                        userInfo
                    };
                }

                var action = JObject.Parse(msg).SelectToken("action")?.ToString();

                var cmd = configSvc.CommandMap(userInfo.UserType, action);

                if (!string.IsNullOrEmpty(cmd) && cmdSet.TryGetValue(cmd, out var handler))
                {
                    handler.Process(msg, userInfo);
                    handler.Dispose();
                }
            }
            catch (JsonSerializationException)
            {
   
            }
            // 忽略網路問題造成Json解析異常
            catch (JsonReaderException)
            {
 
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, $"{this.GetType().Name} OnMessage Exception Message:{msg}");
            }

            return null;
        }

        public override void OnRecordSend(User userInfo, string msg)
        {
            this.Logger.Info($"{this.GetType().Name} RecordSend UserType:{(int)userInfo.UserType} Guid:{userInfo.Guid} Acc:{userInfo.Account} >> {msg}");
        }

        public override void Trace(string msg)
        {
            this.Logger.Trace($"{this.GetType().Name} Trace: {msg}");
        }

        public override bool ValidHandShakeHeader(HandShakeInfo handShakeInfo, User userInfo)
        {
            try
            {
                string url = System.Net.WebUtility.UrlDecode(handShakeInfo.Path);
                string guid = Guid.NewGuid().ToString();
                url = url.ToLower().Replace(" ", "").Replace("http/1.1", "");

                if (this.configSvc.Health &&
                    url.Contains("user") && url.Contains("type") && url.Contains("device") )
                {
                    // URL參數
                    Dictionary<string, string> urlParameters = url.Split('?')[1].Split('&').ToDictionary(
                        o => o.Split('=')[0],
                        o => o.Split('=')[1]);

                    // 驗證握手
                    if (urlParameters.ContainsKey("user") && 
                        urlParameters.ContainsKey("type") &&
                        urlParameters.ContainsKey("device") &&
                        int.TryParse(urlParameters["type"], out int type) && Enum.IsDefined(typeof(UserType), type) &&
                        int.TryParse(urlParameters["device"], out int device) && Enum.IsDefined(typeof(DeviceType), device))
                    {
                        // 更改使用者緩存
                        userInfo.Guid = guid;
                        userInfo.Account = urlParameters["user"].ToUpper();
                        userInfo.UserType = (UserType)type;
                        userInfo.Device = (DeviceType)device;
                        userInfo.ConnectHost = handShakeInfo.Host;

                        // 紀錄握手訊息
                        this.Logger.Trace($"握手訊息 {url},Guid:{userInfo.Guid}");

                        // 是會員再取資料
                        if (userInfo.UserType == UserType.Member)
                        {
                            // 使用目前的時間 ticks 當作亂數種子
                            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
                            Random random = new Random(seed);

                            // 產生一個 0 到 int.MaxValue 的亂數
                            int randomNumber = random.Next(0, int.MaxValue); // 雖然 +1 會超出範圍，但 Next 會自動限制為 int.MaxValue

                            userInfo.SerialNumber = randomNumber;
                        }
                        else if (new UserType[] { UserType.Anchor, UserType.Company, UserType.Forum }.Contains(userInfo.UserType))
                        {
                            // 捏一個假的SN代表有取過會員資訊
                            userInfo.SerialNumber = -1;
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, $"{this.GetType().Name} ValidHandShakeHeader path:{System.Net.WebUtility.UrlDecode(handShakeInfo.Path)}");
            }

            return false;
        }

        public override void Warring(string msg)
        {
            this.Logger.Warn($"{this.GetType().Name} Warring: {msg}");
        }
    }
}
