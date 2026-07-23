using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Api.Tests.Fakes;
using TaskManager.Interfaces;

namespace TaskManager.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? taskStorageDescriptor = services.FirstOrDefault(
                service => service.ServiceType == typeof(ITaskStorage));

            if (taskStorageDescriptor is not null)
            {
                services.Remove(taskStorageDescriptor);
            }

            services.AddSingleton<ITaskStorage, FakeTaskStorage>();
        });
    }
}