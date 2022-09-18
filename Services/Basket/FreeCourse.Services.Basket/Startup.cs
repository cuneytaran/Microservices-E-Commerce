using FreeCourse.Services.Basket.Services;
using FreeCourse.Services.Basket.Settings;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            //nuget paketten Microsoft.Aspnetcore.authentication.jwtbearer kur
            //servisi güvence altına alma işlemi Authentication işlemi
            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//Authenticat olmuş bir user olması lazım işlemi
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//Claim lere gelen her datayı maplıyorsun ancak sub keyini maplama diyorum ki. artık bana bana token içinde ayıklarken sub olarak gösterecek veri sub olarak userid si gelcek.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resource_basket";//identityserver config dosyasında tanımlama yaptık.
                options.RequireHttpsMetadata = false;
            });

            services.AddHttpContextAccessor();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IBasketService, BasketService>();
            //TODO:Redis ayarı 2            
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));//appsettings.json içindeki RedisSettings bilgilerini oku
            //TODO:Redis ayarı 5
            services.AddSingleton<RedisService>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;//appsetting içindeki verileri RedisSettings clasörüne aktaracak ve data gelecek.

                var redis = new RedisService(redisSettings.Host, redisSettings.Port);//redisin ihtiyacı olan host ve port adresi

                redis.Connect();//dockera bağlantı kuruluyor.

                return redis;
            });
            
            //contorllerde otomatik Authentication işlemi yapması bu biraz farklı diğer startuplara bak....
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Basket", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Basket v1"));
            }

            app.UseRouting();
            //Authentication ekleme işlemi
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}