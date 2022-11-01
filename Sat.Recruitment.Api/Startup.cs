using Correlate.DependencyInjection;
using Demo.Luka.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Reflection;
using MediatR;
using Application.User;
using Sat.Recruitment.Api.Middleware;

namespace Sat.Recruitment.Api
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
            services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));
            services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ModelStateValidateAttribute));
                options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorDetailModel), StatusCodes.Status422UnprocessableEntity));
                options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ErrorDetailModel), StatusCodes.Status500InternalServerError));
            });
            #region Correlation Ids

            services.AddCorrelate(options => options.RequestHeaders = new[] { "X-Correlation-ID" });

            #endregion Correlation Ids

           // services.AddAutoMapper();
            //services.AddControllers();
            services.AddSwaggerGen();

            #region Configuracion ApiBehaviorOptions
            // desactiva el comportamiento por defecto de validacion
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressConsumesConstraintForFormFileParameters = true;
            });

            #endregion Configuracion ApiBehaviorOptions
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
