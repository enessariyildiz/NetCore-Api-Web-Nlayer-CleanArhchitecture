using App.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Services.Filters
{
    public class NotFoundFilter<T>(IGenericRepository<T> genericRepository) : Attribute, IAsyncActionFilter where T : class
    {

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

           
        }
    }
}
