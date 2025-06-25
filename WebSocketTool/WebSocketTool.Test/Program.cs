
namespace WebSocketTool.Test
{
    using System;
    using System.Linq;
    using System.Text;
    using WebSocketTool.Test.Model;
    using WebSocketUtils.Model;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IWebSocketServer<WebSocketUser> wsServer = new WebSocketServer<WebSocketUser>(
                    new WebSocketRule(
                        6126,
                        false,
                        200,
                        1));

                wsServer.Start();


                var str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                var next = new Random();
                var sb = new StringBuilder();
                for (var i = 0; i < 1024000; i++)
                {
                    sb.Append(str[next.Next(0, str.Length)]);
                }

                var randomContents = sb.ToString();

                var cmd = string.Empty;

                while (cmd.ToLower() != "exit")
                {
                    wsServer.GetUser(p => true).ToList().ForEach(user =>
                    {
                        user.DataEnqueue(randomContents);
                    });
                    cmd = Console.ReadLine();
                }

                wsServer.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
