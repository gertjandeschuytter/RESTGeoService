using System.IO;
using DataLaag.ADO;
using DomeinLaag.Interfaces;
using DomeinLaag.Services;
using GeoService_BusinessLayer.Interfaces;
using GeoService_BusinessLayer.Services;
using GeoService_DataLayer.ADO;
using GeoService_RESTLayer.LogHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace GeoService_RESTLayer {
    public class Startup {
        private string connectionString;
        public static string url;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("GeoServiceDB");
            url = Configuration.GetSection("Url").Value;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddSingleton<IContinentRepository>(x => new ContinentRepositoryADO(connectionString));
            services.AddSingleton<ICountryRepository>(x => new CountryRepositoryADO(connectionString));
            services.AddSingleton<ICityRepository>(x => new CityRepositoryADO(connectionString));
            services.AddSingleton<IRiverRepository>(x => new RiverRepositoryADO(connectionString));
            services.AddSingleton<ContinentService>();
            services.AddSingleton<CountryService>();
            services.AddSingleton<CityService>();
            services.AddSingleton<RiverService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeoService_RESTLayer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\APILogging.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoService_RESTLayer v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
