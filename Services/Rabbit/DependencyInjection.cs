using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Rabbit;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMqService(this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? configureBus = null)
    {
        services.AddMassTransit(x =>
        {
            configureBus?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("192.168.0.176", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}