using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SoftwareEngineerAssignment.Tests.Integration;

public class TestingApiWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    /// <summary>
    /// Use this method to override services in the Program class.
    /// </summary>
    /// <param name="builder"></param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
    }
}