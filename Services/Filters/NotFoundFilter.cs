using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services.Filters
{
    public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository) : Attribute, IAsyncActionFilter where T : class where TId : struct
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next();
                return;
            }

            if (idValue is not TId id)
            {
                await next();
                return;
            }

            var anyEntity = await genericRepository.AnyAsync(id);

            if (!anyEntity)
            {
                var entityName = typeof(T).Name;
                var actionName = context.ActionDescriptor.DisplayName;

                var result = ServiceResult.Fail($"Data bulunamamıştır.({entityName})({actionName})");
                context.Result = new NotFoundObjectResult(result);
                return;

            }




            await next();

        }
    }
}
