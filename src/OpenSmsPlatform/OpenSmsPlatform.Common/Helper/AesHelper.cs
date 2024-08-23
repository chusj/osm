using System.Security.Cryptography;
using System.Text;

namespace OpenSmsPlatform.Common.Helper
{
    /// <summary>
    /// AES加密工具（对称加密）
    /// </summary>
    public class AesHelper
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">明文</param>
        /// <param name="key">key</param>
        /// <param name="iv">初始向量</param>
        /// <returns></returns>
        public static string EncryptString(string plainText, string key, string iv)
        {
            // 创建Aes对象
            using (Aes aes = Aes.Create())
            {
                // 设置密钥和初始向量
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                // 创建加密器
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // 创建内存流
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // 创建加密流
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // 创建写入流
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // 将明文写入加密流
                            swEncrypt.Write(plainText);
                        }
                        // 返回加密后的Base64字符串
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DecryptString(string cipherText, string key, string iv)
        {
            // 创建Aes对象
            using (Aes aes = Aes.Create())
            {
                // 设置密钥和初始向量
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                // 创建解密器
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // 创建内存流
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    // 创建解密流
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // 创建读取流
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // 返回解密后的明文字符串
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
