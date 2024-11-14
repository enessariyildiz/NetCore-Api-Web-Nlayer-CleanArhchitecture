using App.Services.Filters;

namespace CleanApp.API.Extensions
{
    public static class ConfigurePiplineExtensions
    {

        public static IApplicationBuilder UseConfigurePiplineExt(this WebApplication app)
        {
            app.UseExceptionHandler(x => { });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerExt();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            return app;
        }
    }
}
