using Microsoft.Extensions.Configuration;
using System.IO;

namespace IS_Control.Tools
{
    public static class AppSettingJSON
    {
            public static string ApplicationExeDirectory()
            {
                var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var appRoot = Path.GetDirectoryName(location);
                return appRoot;
            }

            public static IConfigurationRoot GetAppSettings()
            {
                string applicationExeDirectory = ApplicationExeDirectory();
                var builder = new ConfigurationBuilder()
                .SetBasePath(applicationExeDirectory)
                .AddJsonFile("config.json");
                return builder.Build();
            }

    }
}
