using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<BusService>();

            services.AddControllers();            

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    Uri u = new UriBuilder("rabbitmq", "10.30.30.206", 5672, "notifier").Uri;

                    cfg.Host(u, h =>
                    {
                        h.Username("obavestajac");
                        h.Password("ciabiamosad123.");
                    });

                    cfg.ReceiveEndpoint("submit-order", e =>
                    {
                        e.ConfigureConsumer<OrderConsumer>(provider);                        

                        EndpointConvention.Map<SubmitOrder>(e.InputAddress);
                    });

                    cfg.AutoStart = true;                    
                }));

                x.AddRequestClient<SubmitOrder>();
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
