using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Data
{
    public class DataUtility
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var localHost = configuration.GetConnectionString("DefaultConnection");
            var externalHostUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(externalHostUrl) ? localHost : BuildConnectionString(externalHostUrl);
        }

        private static string BuildConnectionString(string externalHostUrl)
        {
            var hostUri = new Uri(externalHostUrl);
            var userInfo = hostUri.UserInfo.Split(':');

            var output = new NpgsqlConnectionStringBuilder()
            {
                Host = hostUri.Host,
                Port = hostUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = hostUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };

            return output.ToString();
        }
    }
}
