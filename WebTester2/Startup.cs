using System;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTester2.Config;
using WebTester2.WebSockets.WebSocketHandlers;
using WebTester2.WebSockets.WebSocketManager;
using WebTester2.WebSockets.MessageHandlers;

namespace WebTester2
{
  public class Startup
  {
    public Startup( IConfiguration configuration )
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices( IServiceCollection services )
    {
      services.AddMvc();

      services.AddRabbitMq();

      services.AddWebSocketManager();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider )
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseWebSockets();
      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.MapWebSocketManager("/ws", serviceProvider.GetService<WebSocketHandler>());

      app.UseMvc();

      app.RegisterMessageHandlers( serviceProvider.GetService<MessageDespatcher>(),
                                   serviceProvider.GetService<WebSocketHandler>() );
    }
  }
}
