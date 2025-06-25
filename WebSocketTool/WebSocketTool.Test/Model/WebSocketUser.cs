
namespace WebSocketTool.Test.Model
{
    using WebSocketUtils.Model;

    public class WebSocketUser : UserInfo
    {
        public WebSocketUser(string userId) : base(userId)
        {
        }

        public string Account { get; set; }
    }
}
