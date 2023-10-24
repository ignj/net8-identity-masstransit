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
        cfg.Host(Environment.GetEnvironmentVariable("RMQ_HOST"), Environment.GetEnvironmentVariable("RMQ_VHOST"), h =>
        {
            h.Username(Environment.GetEnvironmentVariable("RMQ_USR"));
            h.Password(Environment.GetEnvironmentVariable("RMQ_PWD"));
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.MapHealthChecks("/healthcheck");

app.Run();
