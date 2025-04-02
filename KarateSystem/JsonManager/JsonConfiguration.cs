using Microsoft.Extensions.Configuration;
using System.IO;

namespace KarateSystem.JsonManager
{
    public static class JsonConfiguration
    {
        private static IConfigurationRoot _conf;

        static JsonConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _conf = builder.Build();
        }

        public static string GetSqlConnectionString()
        {
            return _conf.GetConnectionString("DefaultConnection")!;
        }
    }
}
