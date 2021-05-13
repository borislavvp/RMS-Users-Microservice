using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Data.Configuration
{
    public static class AppSettingsHelper
    {

        public static readonly Func<IConfiguration, string> DATABASE_CONNECTION = (IConfiguration conf) => loadConnection(conf, "DefaultConnection");
        public static readonly Func<IConfiguration, string> AUTH_ISSUER = (IConfiguration conf) => loadSetting(conf, "ISSUER");
        public static readonly Func<IConfiguration, string> DESKTOP_APP_NAME = (IConfiguration conf) => loadSetting(conf,"DESKTOP_APP_NAME");
        public static readonly Func<IConfiguration, string> DESKTOP_APP_ID = (IConfiguration conf) => loadSetting(conf, "DESKTOP_APP_ID");
        public static readonly Func<IConfiguration, string> DESKTOP_APP_URI = (IConfiguration conf) => loadSetting(conf, "DESKTOP_APP_URI");
        public static readonly Func<IConfiguration, string> DESKTOP_APP_LOGIN_PATH = (IConfiguration conf) => loadSetting(conf, "DESKTOP_APP_LOGIN_PATH");

        public static readonly Func<IConfiguration,string, string> CLIENT_LOGIN_PATH 
            = (IConfiguration conf,string clientURI) 
            => clientURI.Equals(DESKTOP_APP_URI(conf)) ? $"{DESKTOP_APP_URI(conf)}{DESKTOP_APP_LOGIN_PATH(conf)}" : null;


        private static string loadSetting(IConfiguration configuration,string key, string fallback = null)
        {
            try
            {
                var value = configuration.GetValue<string>(key) ?? fallback;

                if (value == null)
                {
                    throw new FormatException($"AppSetting: {key} is empty");
                }

                return value;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"AppSetting: {key} is missing", ex);
            }
        }
        private static string loadConnection(IConfiguration configuration,string key)
        {
            try
            {
                var value = configuration.GetConnectionString(key);

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new FormatException($"ConnectionString: {key} is empty");
                }

                return value;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"ConnectionString: {key} is missing", ex);
            }
        }
    }
}
