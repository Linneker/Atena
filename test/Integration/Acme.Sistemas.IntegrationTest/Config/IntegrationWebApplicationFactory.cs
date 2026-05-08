using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Acme.Sistemas.IntegrationTest.Config;

public class IntegrationWebApplicationFactory : WebApplicationFactory<Program>
{
    public string? ConnectionString { get; set; }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseDefaultServiceProvider(opts =>
        {
            opts.ValidateOnBuild = false;
            opts.ValidateScopes = false;
        });
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Default"] = ConnectionString
                });
            }
        });
    }
}
