using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebTester2.WebSockets.WebSocketManager
{
    public static class Extensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, PathString path, AbstractWebSocketHandler handler )
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            var handlerBaseType = typeof(AbstractWebSocketHandler);

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == handlerBaseType)
                {
                    services.AddSingleton(type);
                }
            }

            services.AddSingleton(typeof(WebSocketHandlers.WebSocketHandler));
            return services;
        }
    }
}
