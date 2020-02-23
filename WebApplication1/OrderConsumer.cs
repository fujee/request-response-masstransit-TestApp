using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class OrderConsumer : IConsumer<SubmitOrder>
    {
        public Task Consume(ConsumeContext<SubmitOrder> context)
        {                        
            context.Respond<OrderAccepted>(new OrderAccepted
            {                
                Text = $"Received: {context.Message.OrderNumber} {DateTime.Now}"
            });

            return Task.CompletedTask;
        }
    }
}
