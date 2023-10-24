using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(o =>
{
    o.SetKebabCaseEndpointNameFormatter();

    var assembly = typeof(Program).Assembly;
    o.AddConsumers(assembly);
    o.AddSagaStateMachines(assembly);
    o.AddSagas(assembly);
    o.AddActivities(assembly);

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

var app = builder.Build();

app.Run();
