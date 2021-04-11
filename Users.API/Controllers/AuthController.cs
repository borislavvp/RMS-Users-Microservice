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

        public AuthController(

            //InMemoryUserLoginService loginService,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("1")]
        public async Task<ActionResult> Test()
        {
            var user = new ApplicationUser { Email = "asd@abv.bg",
                FirstName = "asd",
                LastName = "asd" ,
                Address="asd",
                PhoneNumber="0878833290",
                Subject = new Guid().ToString(),
                Role=new IdentityRole("Customer"),
                UserName = "asd"
            };
            IdentityResult result = await _userManager.CreateAsync(user, "My@Password212");
            var t = await _userManager.FindByEmailAsync(user.Email);
            var props = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                AllowRefresh = true,
            };

            await _signInManager.SignInAsync(t, props);
            var asd = User;
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        [Route("2")]
        public ActionResult Test2()
        {
            var asd = User;
            return Ok();
        }
    }
}
