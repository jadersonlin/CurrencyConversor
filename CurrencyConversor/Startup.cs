using CurrencyConversor.Domain.Models;
using CurrencyConversor.Domain.Repositories;
using CurrencyConversor.Infraestructure.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CurrencyConversor.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Currency Conversor",
                    Description = "ASP.NET Core Web API (.NET 5) with Docker and MongoDB.",
                    Contact = new OpenApiContact
                    {
                        Name = "Jaderson Linhares"
                    }
                });
            });

            services.AddScoped<ICurrencyContext, CurrencyContext>();
            services.AddScoped<ICurrencyRepository, CurrencyMongoDbRepository>();
            services.AddScoped(typeof(IConversionTransactionRepository<SuccessTransaction>), typeof(SuccessTransactionMongoDbRepository<SuccessTransaction>));
            services.AddScoped(typeof(IConversionTransactionRepository<FailureTransaction>), typeof(FailureTransactionMongoDbRepository<FailureTransaction>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrencyConversor v1"));
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
