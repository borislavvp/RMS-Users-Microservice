using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Data.Models;

namespace Users.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IEventService _events;

        public AuthController(

            //InMemoryUserLoginService loginService,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager,
            IEventService events)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _events = events;
        }

        [HttpGet]
        [Route("1")]
        public async Task<ActionResult> Test(string asd2)
        {
            var user = new ApplicationUser {
                Email = "asd@abv.bg",
                FirstName = "asd",
                LastName = "asd" ,
                Address="asd",
                PhoneNumber="0878833290",
                Subject = new Guid().ToString(),
                Role = new IdentityRole("Customer"),
                UserName = "asd"
            };
            IdentityResult result = await _userManager.CreateAsync(user, "My@Password212");
            var t = await _userManager.FindByEmailAsync(user.Email);

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Subject, user.UserName, clientId: user.Id));

            var props = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(2),
                AllowRefresh = true,
            };

            await _signInManager.SignInAsync(t, props);
            var asd = User;
            return Ok();
        }
        
        [HttpGet]
        [Route("3")]
        public async Task<ActionResult> Test3()
        {
            var asd = User;
            var returnUrl = "https://localhost:4200";
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            return new RedirectResult(returnUrl);
        }
        [HttpGet]
        [Authorize]
        [Route("2")]
        public async Task<ActionResult> Test2(string logoutId)
        {
            var asd = User;
            await _signInManager.SignOutAsync();

            var returnUrl = "https://localhost:4200/signin-oidc";
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

            // this triggers a redirect to the external provider for sign-out
            return new RedirectResult(context.PostLogoutRedirectUri + "?logoutId=" + logoutId);
            //return new RedirectResult(returnUrl);
            //return Ok();
        }
    }
}
