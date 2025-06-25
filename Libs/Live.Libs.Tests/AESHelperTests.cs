
namespace Live.Libs.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AESHelperTests
    {
        [TestMethod]
        public void AES加解密測試()
        {
            // 私鑰
            var key = "cu1qaz2wsx3edc112233445566778899";
            // 初始向量 注意長度須為16
            var iv = "cu4rfv5tgb6yhn7u";

            var timestamp = TimeStampHelper.ToUtcTimeStamp(DateTime.Now);
            var user = "test123";

            var str = $"{user}:{timestamp}";

            var encryptStr = AesHelper.Encrypt(key, iv, str);

            var decryptStr = AesHelper.Decrypt(key, iv, encryptStr);

            Assert.AreEqual(str, decryptStr);
            Assert.AreEqual(decryptStr.Split(':')[0], user);
            Assert.AreEqual(decryptStr.Split(':')[1], $"{timestamp}");
        }

        [TestMethod]
        public void AES16進制加解密測試()
        {
            // 私鑰
            var key = "cu1qaz2wsx3edc112233445566778899";
            // 初始向量 注意長度須為16
            var iv = "cu4rfv5tgb6yhn7u";

            var timestamp = TimeStampHelper.ToUtcTimeStamp(DateTime.Now);
            var user = "test123";

            var str = $"{user}:{timestamp}";

            var encryptStr = AesHelper.EncryptHex(key, iv, str);

            var decryptStr = AesHelper.DecryptHex(key, iv, encryptStr);

            Assert.AreEqual(str, decryptStr);
            Assert.AreEqual(decryptStr.Split(':')[0], user);
            Assert.AreEqual(decryptStr.Split(':')[1], $"{timestamp}");
        }
    }
}
