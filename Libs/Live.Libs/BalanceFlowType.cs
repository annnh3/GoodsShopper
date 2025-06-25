
namespace Live.Libs
{
    /// <summary>
    /// 點數異動紀錄類型
    /// </summary>
    public enum BalanceFlowType
    {
        /// <summary>
        /// 贈禮
        /// </summary>
        Gift = 1,
        /// <summary>
        /// 購買私密時數
        /// </summary>
        PrivateTime = 2,
        /// <summary>
        /// 重置
        /// </summary>
        Clear = 3,
        /// <summary>
        /// 加點
        /// </summary>
        Inc = 254,
        /// <summary>
        /// 扣點
        /// </summary>
        Dec = 255,
        /// <summary>
        /// B站主帳戶轉入
        /// </summary>
        BIn = 13,
        /// <summary>
        /// B站主帳戶轉出
        /// </summary>
        BOut = 14,
        /// <summary>
        /// 彩球傳入
        /// </summary>
        ColorBallIn = 15,
        /// <summary>
        /// 彩球轉出
        /// </summary>
        ColorBallOut = 16,
        /// <summary>
        /// AG越捕魚轉入
        /// </summary>
        AGFishIn = 17,
        /// <summary>
        /// AG越捕魚轉出
        /// </summary>
        AGFishOut = 18,
        /// <summary>
        /// HG轉入
        /// </summary>
        HGIn = 19,
        /// <summary>
        /// HG轉出
        /// </summary>
        HGOut = 20,
        /// <summary>
        /// OB捕魚轉入
        /// </summary>
        OBFishIn = 21,
        /// <summary>
        /// OB捕魚轉出
        /// </summary>
        OBFishOut = 22,
        /// <summary>
        /// AG轉入
        /// </summary>
        AGIn = 23,
        /// <summary>
        /// AG轉出
        /// </summary>
        AGOut = 24,
        /// <summary>
        /// OB電子轉入
        /// </summary>
        OBSIn = 25,
        /// <summary>
        /// OB電子轉出
        /// </summary>
        OBSOut = 26,
        /// <summary>
        /// 3D轉入
        /// </summary>
        DDDIn = 27,
        /// <summary>
        /// 3D轉出
        /// </summary>
        DDDOut = 28,
        /// <summary>
        /// FC轉入
        /// </summary>
        FCIn = 29,
        /// <summary>
        /// FC轉出
        /// </summary>
        FCOut = 30,
        /// <summary>
        /// 沙巴轉入
        /// </summary>
        SabaIn = 31,
        /// <summary>
        /// 沙巴轉出
        /// </summary>
        SabaOut = 32,
        /// <summary>
        /// OG轉入
        /// </summary>
        OGIn = 33,
        /// <summary>
        /// OG轉出
        /// </summary>
        OGOut = 34,
        /// <summary>
        /// BBIN轉入
        /// </summary>
        BBINIn = 35,
        /// <summary>
        /// BBIN轉出
        /// </summary>
        BBINOut = 36,
        /// <summary>
        /// BBIN轉入
        /// </summary>
        ABIn = 37,
        /// <summary>
        /// BBIN轉出
        /// </summary>
        ABOut = 38,
        /// <summary>
        /// OB對戰轉入
        /// </summary>
        OBBTIn = 39,
        /// <summary>
        /// OB對站轉出
        /// </summary>
        OBBTOut = 40,       
        /// <summary>
        /// CMD轉入
        /// </summary>
        CMDIn = 55,
        /// <summary>
        /// CMD轉出
        /// </summary>
        CMDOut = 56,
        /// <summary>
        /// BNG轉入
        /// </summary>
        BNGIn = 57,
        /// <summary>
        /// BNG轉出
        /// </summary>
        BNGOut = 58,       
        /// <summary>
        /// B站K站轉入
        /// </summary>
        BKIn = 61,
        /// <summary>
        /// B站K站轉出
        /// </summary>
        BKOut = 62,      
        /// <summary>
        /// CQ9轉入
        /// </summary>
        CQ9In = 65,
        /// <summary>
        /// CQ9轉出
        /// </summary>
        CQ9Out = 66,    
        /// <summary>
        /// DT轉入
        /// </summary>
        DTIn = 69,
        /// <summary>
        /// DT轉出
        /// </summary>
        DTOut = 70,
        /// <summary>
        /// VR轉入
        /// </summary>
        VRIn = 71,
        /// <summary>
        /// VR轉出
        /// </summary>
        VROut = 72,
        /// <summary>
        /// LC轉入
        /// </summary>
        LCIn = 73,
        /// <summary>
        /// LC轉出
        /// </summary>
        LCOut = 74,
        /// <summary>
        /// KY轉入
        /// </summary>
        KYIn = 75,
        /// <summary>
        /// KY轉出
        /// </summary>
        KYOut = 76,
        /// <summary>
        /// PIN轉入
        /// </summary>
        PINIn = 77,
        /// <summary>
        /// PIN轉出
        /// </summary>
        PINOut = 78,
        /// <summary>
        /// K站NBB轉入
        /// </summary>
        KNBBIn = 79,
        /// <summary>
        /// K站NBB轉出
        /// </summary>
        KNBBOut = 80,
        /// <summary>
        /// WM真人轉入
        /// </summary>
        WMIn = 81,
        /// <summary>
        /// WM真人轉出
        /// </summary>
        WMOut = 82,
        /// <summary>
        /// KA轉入
        /// </summary>
        KAIn = 83,
        /// <summary>
        /// KA轉出
        /// </summary>
        KAOut = 84,
        /// <summary>
        /// DG轉入
        /// </summary>
        DGIn = 85,
        /// <summary>
        /// DG轉出
        /// </summary>
        DGOut = 86,
        /// <summary>
        /// SA轉入
        /// </summary>
        SAIn = 87,
        /// <summary>
        /// SA轉出
        /// </summary>
        SAOut = 88,
        /// <summary>
        /// DS轉入
        /// </summary>
        DSIn = 89,
        /// <summary>
        /// DS轉出
        /// </summary>
        DSOut = 90,
        /// <summary>
        /// RK轉入
        /// </summary>
        RK5In = 91,
        /// <summary>
        /// RK轉出
        /// </summary>
        RK5Out = 92,
        /// <summary>
        /// SM轉入
        /// </summary>
        SMIn = 93,
        /// <summary>
        /// SM轉出
        /// </summary>
        SMOut = 94,
        /// <summary>
        /// AVIA轉入(待廢棄)
        /// </summary>
        AVIAIn = 95,
        /// <summary>
        /// AVIA轉出(待廢棄)
        /// </summary>
        AVIAOut = 96,
        /// <summary>
        /// IM轉入
        /// </summary>
        IMIn = 97,
        /// <summary>
        /// IM轉出
        /// </summary>
        IMOut = 98,
        /// <summary>
        /// AELive轉入
        /// </summary>
        AELiveIn = 99,
        /// <summary>
        /// AELive轉出
        /// </summary>
        AELiveOut = 100,
        /// <summary>
        /// KS轉入
        /// </summary>
        KSIn = 101,
        /// <summary>
        /// KS轉出
        /// </summary>
        KSOut = 102,
        /// <summary>
        /// PG轉入
        /// </summary>
        PGIn = 103,
        /// <summary>
        /// PG轉出
        /// </summary>
        PGOut = 104,
        /// <summary>
        /// EVO轉入
        /// </summary>
        EVOIn = 105,
        /// <summary>
        /// EVO轉出
        /// </summary>
        EVOOut = 106,
        /// <summary>
        /// AI轉入
        /// </summary>
        AIIn = 107,
        /// <summary>
        /// AI轉出
        /// </summary>
        AIOut = 108,
        /// <summary>
        /// FTG轉入
        /// </summary>
        FTGIn = 109,
        /// <summary>
        /// FTG轉出
        /// </summary>
        FTGOut = 110,     
        /// <summary>
        /// OB體育轉入
        /// </summary>
        OB_SportIn = 113,
        /// <summary>
        /// OB體育轉出
        /// </summary>
        OB_SportOut = 114,
        /// <summary>
        /// OB真人轉入
        /// </summary>
        OB_RealIn = 115,
        /// <summary>
        /// OB真人轉出
        /// </summary>
        OB_RealOut = 116,
        /// <summary>
        /// OB電競轉入
        /// </summary>
        OB_ESportsIn = 117,
        /// <summary>
        /// OB電競轉出
        /// </summary>
        OB_ESportsOut = 118,
        /// <summary>
        /// WG彩票轉入
        /// </summary>
        WGIn = 119,
        /// <summary>
        /// WG彩票轉出
        /// </summary>
        WGOut = 120,
        /// <summary>
        /// HSG轉入(待廢棄)
        /// </summary>
        HSGIn = 121,
        /// <summary>
        /// HSG轉出(待廢棄)
        /// </summary>
        HSGOut = 122,
        /// <summary>
        /// XG轉入(待廢棄)
        /// </summary>
        XGIn = 123,
        /// <summary>
        /// XG轉出(待廢棄)
        /// </summary>
        XGOut = 124, 
        /// <summary>
        /// K站簡站轉入
        /// </summary>
        KCNIn = 211,
        /// <summary>
        /// K站簡站轉出
        /// </summary>
        KCNOut = 212,
        /// <summary>
        /// K站越站轉入
        /// </summary>
        KVIIn = 213,
        /// <summary>
        /// K站越站轉出
        /// </summary>
        KVIOut = 214,
        /// <summary>
        /// K站泰站轉入
        /// </summary>
        KThIn = 215,
        /// <summary>
        /// K站泰站轉出
        /// </summary>
        KThOut = 216,
        /// <summary>
        /// K站印尼站轉入
        /// </summary>
        KIdIn = 217,
        /// <summary>
        /// K站印尼站轉出
        /// </summary>
        KIdOut = 218,
        /// <summary>
        /// 未知轉點傳入
        /// </summary>
        UnKnowIn = 251,
        /// <summary>
        /// 未知轉點傳出
        /// </summary>
        UnKnowOut = 252
    }
}
