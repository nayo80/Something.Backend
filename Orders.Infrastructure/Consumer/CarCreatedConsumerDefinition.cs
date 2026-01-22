using MassTransit;

namespace Orders.Infrastructure.Consumer;

public class CarCreatedConsumerDefinition : ConsumerDefinition<CarCreatedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<CarCreatedConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind("car-exchange", s =>
            {
                s.RoutingKey = "CarCreated";
                s.ExchangeType = "direct";
            });
        }
    }
}