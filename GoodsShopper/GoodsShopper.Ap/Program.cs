using System;
using System.Runtime.InteropServices;
using System.Timers;
using GoodsShopper.Ap.Applibs;
using Microsoft.Owin.Hosting;

namespace GoodsShopper.Ap
{
    internal class Program
    {
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        const uint ENABLE_INSERT_MODE = 0x0020;
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int hConsoleHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);

        public delegate bool ConsoleCtrlDelegate(CtrlTypes ctrlType);

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
        static void Main(string[] args)
        {
            Console.Title = $"GoodsShopper.Ap-{ConfigHelper.Version}";

            using (WebApp.Start(ConfigHelper.ServiceUrl))
            using (WebApp.Start(ConfigHelper.SignalrServiceUrl))
            {
                //// 監聽APP關閉事件
                SetConsoleCtrlHandler(t =>
                {
                    GoodsShopperProcess.ProcessStop();

                    return false;
                },
                true);

                DisbleQuickEditMode();

                GoodsShopperProcess.ProcessStart();

                var timer = new Timer();
                timer.AutoReset = true;
                timer.Interval = 60000;
                timer.Elapsed += TimerElapsed;
                timer.Start();

                while (Console.ReadLine().ToLower() != "exit")
                {
                }

                timer.Stop();
                timer.Dispose();

                GoodsShopperProcess.ProcessStop();
            }
        }

        /// <summary>
        /// 關閉快速編輯模式、插入模式
        /// </summary>
        public static void DisbleQuickEditMode()
        {
            IntPtr hStdin = GetStdHandle(STD_INPUT_HANDLE);
            uint mode;
            GetConsoleMode(hStdin, out mode);
            mode &= ~ENABLE_QUICK_EDIT_MODE; //移除快速編輯模式
            mode &= ~ENABLE_INSERT_MODE; //移除插入模式
            SetConsoleMode(hStdin, mode);
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();
        }
    }
}
