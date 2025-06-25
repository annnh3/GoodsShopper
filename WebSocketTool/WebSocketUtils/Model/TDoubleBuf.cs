namespace WebSocketUtils.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// 為了執行續安全使用的DoubleQueue 
    /// </summary>
    /// <returns></returns>
    public class TDoubleBuf<T>
    {
        #region Member

        /// <summary>
        /// Queue1
        /// </summary>
        /// <returns></returns>
        protected List<T> mBuf1 = new List<T>();

        /// <summary>
        /// Queue2
        /// </summary>
        /// <returns></returns>
        protected List<T> mBuf2 = new List<T>();

        /// <summary>
        /// MainQueue Lock
        /// </summary>
        /// <returns></returns>
        private readonly object mLockMain = new object();

        /// <summary>
        /// BranchQueue Lock 
        /// </summary>
        /// <returns></returns>
        private readonly object mLockBranch = new object();

        /// <summary>
        /// 用來判斷當前使用的Queue的Tag
        /// </summary>
        /// <returns></returns>
        private bool mPositive;

        #endregion

        /// <summary>
        /// 雙Buffer建構子
        /// </summary>
        public TDoubleBuf()
        {
            mPositive = true;
        }

        /// <summary>
        /// 切換 Queue
        /// </summary>
        /// <returns></returns>
        public void Switch()
        {
            lock (mLockMain)
            {
                lock (mLockBranch)
                {
                    mPositive = !mPositive;
                }
            }
        }

        /// <summary>
        /// 取得 MainQueue 的 Lock
        /// </summary>
        /// <returns></returns>
        public object GetMainLock()
        {
            return mLockMain;
        }

        /// <summary>
        /// 取得 BranchQueue 的 Lock
        /// </summary>
        /// <returns></returns>
        public object GetBranchLock()
        {
            return mLockBranch;
        }

        /// <summary>
        /// 取得 MainQueue 
        /// </summary>
        /// <returns></returns>
        public List<T> GetMainBuf()
        {
            lock (mLockMain)
            {
                return mPositive ? mBuf1 : mBuf2;
            }
        }

        /// <summary>
        /// 取得 BranchQueue 
        /// </summary>
        /// <returns></returns>
        public List<T> GetBranchBuf()
        {
            lock (mLockBranch)
            {
                return mPositive ? mBuf2 : mBuf1;
            }
        }

        /// <summary>
        /// BranchBuf 新增一筆資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool BranchBufAddValue(T value)
        {
            List<T> tBranchBuf = GetBranchBuf();
            lock (mLockBranch)
            {
                tBranchBuf.Add(value);
            }

            return true;
        }

        /// <summary>
        /// 清除MainQueue的資料
        /// </summary>
        /// <returns></returns>
        public void ClearMainBuf()
        {
            lock (mLockMain)
            {
                if (mPositive)
                    mBuf1.Clear();
                else
                    mBuf2.Clear();
            }
        }
    }
}
