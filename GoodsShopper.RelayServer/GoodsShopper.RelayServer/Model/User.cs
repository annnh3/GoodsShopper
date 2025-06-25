using GoodsShopper.RelayServer.Domain.ClientAction;
using GoodsShopper.RelayServer.Model.Service;
using Live.Libs;
using WebSocketUtils.Model;

namespace GoodsShopper.RelayServer.Model
{
    public class User : UserInfo
    {
        /// <summary>
        /// config svc
        /// </summary>
        private IConfigService configSvc;

        /// <summary>
        /// 使用者類型
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// 1=PC, 2=手機
        /// </summary>
        public DeviceType Device { get; set; }

        /// <summary>
        /// 會員流水號
        /// </summary>
        public int? SerialNumber { get; set; } = null;

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 在特定的指令時會用到的欄位，將使用者傳送的值傳回去
        /// </summary>
        public int? Dc { get; set; }

        /// <summary>
        /// 使用者唯一碼(1.握手時產生 2.與微服務溝通使用)
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// client使用的線路
        /// </summary>
        public string ConnectHost { get; set; } = string.Empty;

        /// <summary>
        /// 是否向MemberSystem取得會員資料
        /// </summary>
        public bool IsGetUserInfo
            => SerialNumber.HasValue;

        public User(string userId, IConfigService configSvc) : base(userId)
        {
            this.configSvc = configSvc;
        }

        /// <summary>
        /// 加入訊息到訊息佇列 only for Action_out_first and Action_out_checkSN
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="serialNumber"></param>
        public virtual void AddMsgQueue<T>(T clientAction)
            where T : IActionCheckSn
        {
            // 複寫SN
            clientAction.SerialNumber = this.configSvc.SerialNumber;
            DataEnqueue(clientAction.ToString());
            // 釋放資源
            clientAction.Dispose();
        }

        /// <summary>
        /// 加入訊息到訊息佇列
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="serialNumber"></param>
        public virtual void AddMsgQueueWithSn<T>(T clientAction)
            where T : IAction
        {
            // 複寫SN
            clientAction.SerialNumber = clientAction.SerialNumber.HasValue ?
                clientAction.SerialNumber :
                this.configSvc.SerialNumber;

            DataEnqueue(clientAction.ToString());
            // 釋放資源
            clientAction.Dispose();
        }

        /// <summary>
        /// 加入訊息到訊息佇列
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="serialNumber"></param>
        public virtual void AddMsgQueueWithDc<T>(T clientAction)
            where T : IAction
        {
            DataEnqueue(clientAction.ToString());
            // 釋放資源
            clientAction.Dispose();
        }
    }
}
