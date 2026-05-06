using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Acme.Sistemas.IntegrationTest.Config;

public class IntegrationWebApplicationFactory : WebApplicationFactory<Program>
{
    public string? ConnectionString { get; set; }

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
