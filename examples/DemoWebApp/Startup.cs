using System;
using DemoWebApp.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpayCashier;

namespace DemoWebApp
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
            var opayCashierSettings = new OpayCashierSettings();
            Configuration.GetSection("OpayCashier").Bind(opayCashierSettings);
            services.AddOpayCashier(opayCashierSettings.BaseUrl, opayCashierSettings.MerchantId,
                opayCashierSettings.PrivateKey,
                opayCashierSettings.Iv,
                opayCashierSettings.Timeout == null
                    ? default(TimeSpan?)
                    : TimeSpan.FromSeconds(opayCashierSettings.Timeout.Value));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}