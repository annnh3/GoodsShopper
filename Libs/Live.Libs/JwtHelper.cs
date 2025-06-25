
namespace Live.Libs
{
    using System;
    using System.Collections.Generic;
    using JWT.Algorithms;
    using JWT.Builder;

    /// <summary>
    /// JWT 小幫手
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dic">payload info (裡面塞會員資訊)</param>
        /// <param name="secret">私有鑰</param>
        /// <param name="expireSec">過期秒數</param>
        /// <returns></returns>
        public static string Encode(Dictionary<string, object> dic, string secret, int expireSec = 10)
            => new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(secret)
            .AddClaims(dic)
            .AddClaim("exp", TimeStampHelper.ToUtcTimeStamp(DateTime.Now.AddSeconds(expireSec)) / 1000)
            .Encode();

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="token"></param>
        /// <param name="secret">私有鑰</param>
        /// <returns></returns>
        public static Dictionary<string, object> Decode(string token, string secret)
            => new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(secret)
            .MustVerifySignature()
            .Decode<Dictionary<string, object>>(token);
    }
}
