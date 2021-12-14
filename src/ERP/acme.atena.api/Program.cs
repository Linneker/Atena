using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            logger.Debug("Inicio");
            CreateHostBuilder(args).Build().Run();
            logger.Debug("Fim");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults((webBuilder) =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging(t =>
                {
                    t.ClearProviders();
                    t.SetMinimumLevel(LogLevel.Trace);
                }).UseNLog();
    }
}
