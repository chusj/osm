using System.Security.Cryptography;
using System.Text;

namespace opensmsplatform.Common.Helper
{
    public class Md5Helper
    {
        /// <summary>
        /// 将字符串进行MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptMD5(string input)
        {
            // 创建MD5加密服务的实例
            using (MD5 md5 = MD5.Create())
            {
                // 将输入的字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // 进行MD5加密
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 将加密后的字节数组转换为32位的十六进制字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                // 返回MD5加密后的字符串
                return sb.ToString();
            }
        }
    }
}
