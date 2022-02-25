using System;
using System.IO;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using moosik.api.integration.test.ContainerManager.Auth;
using moosik.dal.Contexts;
using moosik.dal.Interfaces;

namespace moosik.api.integration.test.ContainerManager;

public class ContainerManager : IAsyncDisposable
{
    private readonly ICompositeService _container;

    public WebApplicationFactory<Program> Factory;

    public ContainerManager()
    {
        string workingDirectory = Environment.CurrentDirectory;

        string file = Path.Combine(Directory.GetParent(workingDirectory)
            .Parent
            .Parent
            .FullName, "docker-compose.yml");

        _container = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(file)
            .RemoveOrphans()
            .WaitForProcess("db", "postgres", 60000)
            .Build().Start();

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseTestServer();
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IMoosikDatabase>(y =>
                        new MoosikContext(RetrieveConnectionString(6000, "Moosik")));

                    
                    services.AddAuthentication(x =>
                        {
                            x.DefaultAuthenticateScheme = "Test";
                            x.DefaultChallengeScheme = "Test";
                        })
                        .AddScheme<TestAuthHandlerOptions, TestAuthHandler>("Test", options =>
                        {
                            options.FakeUserId = FakeUserIdToUser();
                            options.FakeUserRole = FakeUserRoleToUser();
                        });
                    
                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy("ValidAccessToken", policy =>
                        {
                            policy.AuthenticationSchemes.Add("Test");
                            policy.RequireAuthenticatedUser();
                        });
                        
                        options.AddPolicy("ValidRefreshToken", policy =>
                        {
                            policy.AuthenticationSchemes.Add("Test");
                            policy.RequireAuthenticatedUser();
                        });
                        
                        options.AddPolicy("TestPolicy", policy =>
                        {
                            policy.AuthenticationSchemes.Add("Test");
                            policy.RequireAuthenticatedUser();
                        });

                        options.DefaultPolicy = options.GetPolicy("TestPolicy");
                    });
                });
            });
    }

    private static string RetrieveConnectionString(int port, string db)
    {
        return $"User ID=user;Password=password;Host=localhost;Port={port};Database={db};";
    }

    protected virtual string FakeUserIdToUser() => "1";
    protected virtual string FakeUserRoleToUser() => "User";
    
    
    public ValueTask DisposeAsync()
    {
        _container.Dispose();
        return Factory.DisposeAsync();
    }
}