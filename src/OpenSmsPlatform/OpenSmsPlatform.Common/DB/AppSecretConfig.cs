namespace OpenSmsPlatform.Common.DB
{
    public class AppSecretConfig
    {
        private static string Secret = AppSettings.App(new string[] { "Osm", "Secret" });
        private static string SecretFile = AppSettings.App(new string[] { "Osm", "SecretFile" });

        /// <summary>
        /// 联麓秘钥
        /// </summary>
        public static string Secret_String => InitSecret();


        /// <summary>
        /// 初始化联麓的秘钥
        /// </summary>
        /// <returns></returns>
        private static string InitSecret()
        {
            var securityString = DifDBConnOfSecurity(SecretFile);
            if (!string.IsNullOrEmpty(SecretFile) && !string.IsNullOrEmpty(securityString))
            {
                return securityString;
            }
            else
            {
                return Secret;
            }
        }

        private static string DifDBConnOfSecurity(params string[] conn)
        {
            foreach (var item in conn)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        return File.ReadAllText(item).Trim();
                    }
                }
                catch (System.Exception) { }
            }

            return "";
        }
    }
}
