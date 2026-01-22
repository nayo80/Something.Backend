using MassTransit;
using Orders.Infrastructure.Interfaces;
using Shared.Events;

namespace Orders.Infrastructure.Consumer;

public class CarCreatedConsumer(IListenEventRepository<CarEventModel> repository) : IConsumer<CarEventModel>
{
    public async Task Consume(ConsumeContext<CarEventModel> context)
    {
        var message = context.Message;
        
        await repository.InsertOrUpdate(message);
        
        Console.WriteLine($"[Orders Service] Synchronized: {message.Brand} {message.Model}");
    }
}