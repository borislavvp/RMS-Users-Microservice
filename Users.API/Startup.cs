using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Users.API.Extensions;
using Users.Service;
using Users.Service.Profile;
using Users.Service.RequestValidators;

namespace Users.API
{
    public class PublicFacingUrlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _publicFacingUri;

        public PublicFacingUrlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            context.SetIdentityServerOrigin($@"{context.Request.Scheme}://{context.Request.Host.Value}/api/v1/identity");
            context.SetIdentityServerBasePath(request.PathBase.Value.TrimEnd('/'));

            await _next(context);
        }
    }
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

            services.AddAuthentication();

            services.AddDbContext(Configuration);
            services.AddControllers();

            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<UsersService, UsersService>();

            // if (Environment.IsDevelopment())
            // {
            services.SetupDevelopmentCorsPolicies(Configuration);
            // }

            services.SetupIdentityServer(Configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (!Environment.IsDevelopment())
            {
                app.UseMiddleware<PublicFacingUrlMiddleware>();
            }

            app.UseCors();
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax,
                Secure = CookieSecurePolicy.None
            });
            //if (!Environment.IsDevelopment())
            //{
            //    app.UsePathBase(new PathString("/identity"));
            //}
            app.UseRouting();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            if (Environment.IsDevelopment())
            {
                app.UseIdentityServer();
            }
            else
            {
                app.UseMiddleware<IdentityServerMiddleware>();
            }

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

}
