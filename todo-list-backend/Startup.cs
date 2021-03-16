using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using todo_list_backend.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace todo_list_backend
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
            services.AddControllersWithViews();

            services.AddMvc()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.Objects,
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                    Converters = new List<JsonConverter>() { new Newtonsoft.Json.Converters.StringEnumConverter() }
                };
                return settings;
            };
            services.AddHttpContextAccessor();
            services.AddCors();

            services.AddDbContext<TodoListDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TodoListDbContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(options =>
            { //TODO: configure CORS
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
                //options.AllowCredentials();
                options.AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
