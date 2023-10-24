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
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapGet("/ping", (IPublishEndpoint publisher) => publisher.Publish<PingMessage>(new { Text = $"Ping {DateTime.UtcNow}" }));
app.MapGet("/ping-auth", (IPublishEndpoint publisher, ClaimsPrincipal user) => publisher.Publish<PingMessage>(new { Text = $"Ping {DateTime.UtcNow} - {user.Identity!.Name}" }))
    .RequireAuthorization();

app.Run();
