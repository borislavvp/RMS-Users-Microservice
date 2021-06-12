using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Users.API.Extensions;
using Users.Service;
using Users.Service.Profile;

namespace Users.API
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(x => x.DefaultAuthenticateScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme);

            services.AddDbContext(Configuration);
            services.AddControllers();

            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<UsersService, UsersService>();

            if (Environment.IsDevelopment())
            {
                services.SetupDevelopmentCorsPolicies(Configuration);
            }

            services.SetupIdentityServer(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors();
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();
            app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always });
            
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
    
}
