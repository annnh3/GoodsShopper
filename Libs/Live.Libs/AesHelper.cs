namespace Live.Libs
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// AES加密小幫手
    /// </summary>
    public class AesHelper
    {
        private static byte[] Key
        {
            get
            {
                return Encoding.UTF8.GetBytes("qSvN4YTBwwrvwHk8SScMQDsHcFKWZyD9");
            }
        }

        private static byte[] IV
        {
            get
            {
                return Encoding.UTF8.GetBytes("QXtkXUGrmVqceqzg");
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string key, string iv)
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(key);
                provider.IV = Encoding.ASCII.GetBytes(iv);

                using (var encryptor = provider.CreateEncryptor())
                {
                    var source = Encoding.UTF8.GetBytes(plainText);
                    var bytes = encryptor.TransformFinalBlock(source, 0, source.Length);

                    return Convert.ToBase64String(bytes);
                }
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string key, string iv, string cipherText)
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(key);
                provider.IV = Encoding.ASCII.GetBytes(iv);

                using (var decryptor = provider.CreateDecryptor())
                {
                    var encryptStrBytes = Convert.FromBase64String(cipherText);
                    var bytes = decryptor.TransformFinalBlock(encryptStrBytes, 0, encryptStrBytes.Length);

                    return Encoding.UTF8.GetString(bytes);
                }
            }
        }

        /// <summary>
        /// 加密-16進制輸出
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <param name="str">待加密字串</param>
        /// <returns></returns>
        public static string EncryptHex(string key, string iv, string str)
        {
            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(key);
                provider.IV = Encoding.ASCII.GetBytes(iv);

                using (var encryptor = provider.CreateEncryptor())
                {
                    var source = Encoding.UTF8.GetBytes(str);
                    var bytes = encryptor.TransformFinalBlock(source, 0, source.Length);

                    return BitConverter.ToString(bytes).Replace("-", string.Empty);
                }
            }
        }

        /// <summary>
        /// 解密-16進制輸入
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <param name="encryptStr">待解密字串</param>
        /// <returns></returns>
        public static string DecryptHex(string key, string iv, string encryptStr)
        {
            Func<string, byte[]> hexToString = (hexStr) =>
            {
                var raw = new byte[hexStr.Length / 2];
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
                }
                return raw;
            };

            using (var provider = new AesCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(key);
                provider.IV = Encoding.ASCII.GetBytes(iv);

                using (var decryptor = provider.CreateDecryptor())
                {
                    var encryptStrBytes = hexToString(encryptStr);
                    var bytes = decryptor.TransformFinalBlock(encryptStrBytes, 0, encryptStrBytes.Length);

                    return Encoding.UTF8.GetString(bytes);
                }
            }
        }

        /// <summary>
        /// 連線字串解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SqlConnectionStringDecrypt(string input)
        {
            try
            {
                string password = string.Empty;

                var tokens = input.Split(new string[] { "pwd=" }, StringSplitOptions.None);
                if (tokens.Length == 2)
                {
                    password = Decrypt(tokens[1]);
                    input = input.Replace(tokens[1], password);
                    return input;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
