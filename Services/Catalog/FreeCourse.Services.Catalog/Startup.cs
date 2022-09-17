using FreeCourse.Services.Catalog.Services;
using FreeCourse.Services.Catalog.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog
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
            services.AddMassTransit(x =>
            {
                // Default Port : 5672
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];//tokenin dağıttığı yer.appsetting tanımlaması yapılıyor oraya bak.
                options.Audience = "resource_catalog";//gelen tokenin içersinde aud içinde mutalaka resource_catalog olması gerekiyor.bu isim identity servis içindeki config içinden geliyor.
                options.RequireHttpsMetadata = false;//https i iptal ediyoruz. 
            });

            //interfacelerin tanımlandığı yer
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddAutoMapper(typeof(Startup));//Startup=Startup a bağlı tüm yerde kullanabilirsin.
            services.AddControllers(opt =>//tüm controllerlere otomatik Authorize etributeleri eklenmiş olacak
            {
                opt.Filters.Add(new AuthorizeFilter());
            });

            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));//connection string işlemi yapıyoruz. Settings klasörün içindeki (namespace FreeCourse.Services.Catalog.Settings - IDatabaseSettings) interface den geliyor.

            services.AddSingleton<IDatabaseSettings>(sp =>//appsettingdeki verileri okuma işlemi yapıyor.herhangi bir yerden IDatabaseSettings bunu çağırırsam dolu şekilde bağlantı gelecek.
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Catalog", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Catalog v1"));
            }

            app.UseRouting();
            app.UseAuthentication();//
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}