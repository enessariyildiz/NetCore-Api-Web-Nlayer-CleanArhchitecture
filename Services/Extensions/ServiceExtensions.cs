﻿using App.Services.Categories;
using App.Services.ExceptionHandlers;
using App.Services.Filters;
using App.Services.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Services.Extensions
{
    public static class ServiceExtensions
    {
        // Extensions must be static.

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped(typeof(NotFoundFilter<,>));

            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }


    }
}