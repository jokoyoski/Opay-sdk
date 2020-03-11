using System;
using CoreWebApp.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpayCashier;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace CoreWebApp
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
            services.AddMvc();
            services.Configure<OpayCashierSettings>(Configuration.GetSection("OpayCashier"));
            services.AddTransient<ICashierService>(s =>
            {
                var opayCashierSettings = s.GetService<IOptions<OpayCashierSettings>>().Value;
                return new CashierService(opayCashierSettings.BaseUrl,
                    opayCashierSettings.MerchantId,
                    opayCashierSettings.PrivateKey,
                    opayCashierSettings.Iv,
                    opayCashierSettings.Timeout == null
                        ? default(TimeSpan?)
                        : TimeSpan.FromSeconds(opayCashierSettings.Timeout.Value));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}