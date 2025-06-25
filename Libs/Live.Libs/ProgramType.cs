
namespace Live.Libs
{
    using System.Collections.Generic;

    /// <summary>
    /// 節目類型
    /// </summary>
    public enum ProgramType
    {
        /// <summary>
        /// 主播
        /// </summary>
        Anchor = -2,
        /// <summary>
        /// 所有
        /// </summary>
        All = -1,
        /// <summary>
        /// 未區分
        /// </summary>
        None = 0,
        /// <summary>
        /// 足球
        /// </summary>
        SC = 11,
        /// <summary>
        /// 籃球
        /// </summary>
        BK = 12,
        /// <summary>
        /// 棒球
        /// </summary>
        BB = 13,
        /// <summary>
        /// 網球
        /// </summary>
        TN = 14,
        /// <summary>
        /// 冰球
        /// </summary>
        IH = 15,
        /// <summary>
        /// 排球
        /// </summary>
        VL = 16,
        /// <summary>
        /// 羽毛球
        /// </summary>
        BM = 17,
        /// <summary>
        /// 電子競技
        /// </summary>
        ES = 18,
        /// <summary>
        /// 美式足球
        /// </summary>
        AF = 19,
        /// <summary>
        /// 撞球
        /// </summary>
        PB = 20,
        /// <summary>
        /// 乒乓
        /// </summary>
        TT = 21,
        /// <summary>
        /// 手球
        /// </summary>
        HB = 22,
        /// <summary>
        /// 水球
        /// </summary>
        WP = 23,
        /// <summary>
        /// 其他，目前是奧林匹克
        /// </summary>
        Other = 24,
        /// <summary>
        /// 歐洲盃
        /// </summary>
        EuropeCup = 26,
        /// <summary>
        /// 世界盃
        /// </summary>
        WorldCup = 27,
        /// <summary>
        /// 拳擊
        /// </summary>
        BX = 28,
        /// <summary>
        /// 奥林匹克 暫定49
        /// </summary>
        Olympic = 49,
        /// <summary>
        /// 台灣
        /// </summary>
        Taiwan = 51,
        /// <summary>
        /// 中國
        /// </summary>
        China = 52,
        /// <summary>
        /// 越南
        /// </summary>
        Vietnam = 53,
        /// <summary>
        /// 泰國
        /// </summary>
        Thailand = 54,
        /// <summary>
        /// 成人
        /// </summary>
        R18 = 55,
        /// <summary>
        /// 印尼
        /// </summary>
        Indonesia = 56,
        /// <summary>
        /// 直播(台灣)
        /// </summary>
        AnchorTw = 81,
        /// <summary>
        /// 直播(中國)
        /// </summary>
        AnchorCn = 82,
        /// <summary>
        /// 直播(越南)
        /// </summary>
        AnchorVi = 83,
        /// <summary>
        /// 直播(泰國)
        /// </summary>
        AnchorTh = 84,
        /// <summary>
        /// 直播(印尼)
        /// </summary>
        AnchorId = 85
    }

    /// <summary>
    /// 節目類型群組
    /// </summary>
    public static class ProgramTypeGroup
    {
        /// <summary>
        /// 賽事轉播類型
        /// </summary>
        public static IEnumerable<ProgramType> GameSport = new ProgramType[]
        {
            ProgramType.SC,
            ProgramType.BK,
            ProgramType.BB,
            ProgramType.TN,
            ProgramType.IH,
            ProgramType.VL,
            ProgramType.BM,
            ProgramType.ES,
            ProgramType.AF,
            ProgramType.PB,
            ProgramType.TT,
            ProgramType.HB,
            ProgramType.WP,
            ProgramType.Other,
            ProgramType.EuropeCup,
            ProgramType.WorldCup,
            ProgramType.Olympic,
            ProgramType.BX
        };

        /// <summary>
        /// 頻道直播類型
        /// </summary>
        public static IEnumerable<ProgramType> ChannelLive = new ProgramType[]
        {
            ProgramType.Taiwan,
            ProgramType.China,
            ProgramType.Vietnam,
            ProgramType.Thailand,
            ProgramType.R18,
            ProgramType.Indonesia
        };

        /// <summary>
        /// 直播主類型
        /// </summary>
        public static IEnumerable<ProgramType> Anchor = new ProgramType[]
        {
            ProgramType.AnchorTw,
            ProgramType.AnchorCn,
            ProgramType.AnchorVi,
            ProgramType.AnchorTh,
            ProgramType.AnchorId,
        };
    }
}
