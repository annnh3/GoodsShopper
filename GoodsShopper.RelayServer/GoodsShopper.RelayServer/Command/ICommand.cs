using System;
using GoodsShopper.RelayServer.Model;
using NLog;

namespace GoodsShopper.RelayServer.Command
{
    public abstract class ICommand : IDisposable
    {
        protected ILogger logger = LogManager.GetLogger("RelayServer");

        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool Process(string message, User user)
        {
            try
            {
                return this.Execute(message, user);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, $"{this.GetType().Name} Process Exception");
                return false;
            }
        }

        /// <summary>
        /// 執行指令
        /// </summary>
        /// <param name="content">指令內容</param>
        /// <returns></returns>
        public abstract bool Execute(string content, User user);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
