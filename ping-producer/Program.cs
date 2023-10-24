using System.Security.Claims;
using Context;
using contracts;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(o =>
{
    o.SetKebabCaseEndpointNameFormatter();
    o.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(Environment.GetEnvironmentVariable("RMQ_HOST"), Environment.GetEnvironmentVariable("RMQ_VHOST"), h =>
        {
            h.Username(Environment.GetEnvironmentVariable("RMQ_USR"));
            h.Password(Environment.GetEnvironmentVariable("RMQ_PWD"));
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AuthContext>(o => o.UseNpgsql(Environment.GetEnvironmentVariable("AUTH_CONNSTRING")));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<AuthContext>()
    .AddApiEndpoints();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapHealthChecks("/healthcheck");
app.MapGet("/ping", (IPublishEndpoint publisher) => publisher.Publish<PingMessage>(new { Text = $"Ping {DateTime.UtcNow}" }));
app.MapGet("/ping-auth", (IPublishEndpoint publisher, ClaimsPrincipal user) => publisher.Publish<PingMessage>(new { Text = $"Ping {DateTime.UtcNow} - {user.Identity!.Name}" }))
    .RequireAuthorization();

app.Run();
