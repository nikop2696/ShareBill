using Serilog;
using Serilog.Formatting.Json;
using System.Globalization;

namespace ShareBill.LoggerConfigurators


{
    public static class ConfiguredLogger
    {
        public static Serilog.ILogger BaseLogger() 
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Logs");

            var jsonPath = Path.Combine(path, "Json");

            var txtPath = Path.Combine(path, "Txt");

            if (!Directory.Exists(txtPath)) 
            {
                Directory.CreateDirectory(txtPath);
            }
            if (!Directory.Exists(jsonPath))
            {
                Directory.CreateDirectory(jsonPath);
            }

            return new LoggerConfiguration()
                .MinimumLevel.Information()

                .WriteTo.File(
                    Path.Combine(txtPath, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31,
                    outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message}{NewLine:1}{Exception:1}",
                    formatProvider: new CultureInfo("en-GB")
                    )
                
                .WriteTo.File(
                    new JsonFormatter(),
                    Path.Combine(jsonPath,"log-.json"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 31
                    )
                .CreateLogger();
        }
     }
}
