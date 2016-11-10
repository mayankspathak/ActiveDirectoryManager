using System;
using System.Configuration;

namespace ActiveDirectoryManager.Configurations
{
    /// <summary>
    /// Configuration Manager
    /// </summary>
    public sealed class Configuration
    {
        /// <summary>
        /// Gets app settings 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            string str = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception(string.Format("Failed to get app setting \"{0}\" from configuration", (object)key));
            return str;
        }

        /// <summary>
        /// Gets app settings with Null values
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingAllowNull(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
