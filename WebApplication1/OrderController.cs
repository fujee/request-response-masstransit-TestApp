using DataTransferObjects;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
    [Route("/orders")]
    public class OrderController : Controller
    {
        private readonly IRequestClient<SubmitOrder> _requestClient;

        public OrderController(IRequestClient<SubmitOrder> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] OrderDto order, CancellationToken cancellationToken)
        {
            try
            {
                //var request = _requestClient.Create(new { OrderNumber = order.ON }, cancellationToken);

                //request.UseTransaction();
                //request.TimeToLive = TimeSpan.FromMinutes(1);

                //var response = await request.GetResponse<OrderAccepted>();

                var response = await _requestClient.GetResponse<OrderAccepted>(new { OrderNumber = order.ON }, cancellationToken, TimeSpan.FromMinutes(1));

                return Content($"Order Accepted 123: {response.Message.Text}");
            }
            catch (RequestTimeoutException exception)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }

    public class SubmitOrder
    {
        public int OrderNumber { get; set; }
    }

    public class OrderAccepted
    {
        public string Text { get; set; }
    }
}
