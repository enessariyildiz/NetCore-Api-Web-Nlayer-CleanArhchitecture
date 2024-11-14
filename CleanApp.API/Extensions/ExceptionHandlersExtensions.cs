using App.Services.ExceptionHandlers;

namespace CleanApp.API.Extensions
{
    public static class ExceptionHandlersExtensions
    {
        public static IServiceCollection AddExceptionHandlerExt(this IServiceCollection services)
        {
           services.AddExceptionHandler<CriticalExceptionHandler>();
           services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}
