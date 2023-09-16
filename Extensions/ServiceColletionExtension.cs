using Microsoft.Extensions.DependencyInjection;

namespace MessageResolverLib.Abstractions
{
    public static class ServiceColletionExtension
    {
        public static IServiceCollection AddRecipient<TRecipient>(this IServiceCollection services)
            where TRecipient : class, IMessageRecipient
        {
            services.AddSingleton<IMessageRecipient, TRecipient>();

            return services;
        }

        public static IServiceCollection AddDispatcher<TRecipient, TDispatcher>(
            this IServiceCollection services
        )
            where TRecipient : class, IMessageRecipient
            where TDispatcher : class, IMessageDispatcher<TRecipient>
        {
            services.AddSingleton<IMessageDispatcher<TRecipient>, TDispatcher>();
            return services;
        }

        public static IServiceCollection AddHandler<THandler>(this IServiceCollection services)
            where THandler : class, IMessageHandler
        {
            services.AddTransient<THandler>();
            return services;
        }

        public static IServiceCollection AddSender<TSender>(this IServiceCollection services)
            where TSender : class, IMessageSender
        {
            services.AddTransient<IMessageSender, TSender>();
            return services;
        }
    }
}
