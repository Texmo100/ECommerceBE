using ECommerceBE.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using ECommerceBE.Repository.IRepository;
using ECommerceBE.Models;
using ECommerceBE.Repository;//Deletethiscomment

namespace ECommerceBE
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Dependency Injection for Repository Pattern
            services.AddScoped<IRepository<Supplier>, SupplierRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Customer>, CustomerRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();

            AddSwagger(services);
        }

        //This method configure swagger
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"EComerceBE {groupName}",
                    Version = groupName,
                    Description = "EComerceBE API",
                    Contact = new OpenApiContact
                    {
                        Name = "Foo Company",
                        Email = string.Empty,
                        Url = new Uri("https://EComerceBE.com/"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EComerceBE API V1");
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
