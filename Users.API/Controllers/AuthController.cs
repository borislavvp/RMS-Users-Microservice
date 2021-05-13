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
using Users.Service;
using Users.Service.Common.Dtos;
using Users.Service.Common.Interfaces;
using Users.Service.Models;

namespace Users.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UsersService _usersService;

        public AuthController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(UserRegisterDTO userRegistrationModel)
        {
            try
            {
                IRequestResult result = await _usersService.RegisterUser(userRegistrationModel);

                if (result.IsSuccess)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.FailureReason);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("redirect/login")]
        public async Task<ActionResult> LoginRedirect(string returnUrl)
        {
            try
            {
                 IRequestResult<string> result = await _usersService.GetLoginRedirectPageUriAsync(returnUrl);
                 return result.IsSuccess ? new RedirectResult(result.Data) : BadRequest(result.FailureReason);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginRestaurant(UserLoginDTO userLoginModel)
        {
            try
            {
                IRequestResult result = await _usersService.Login(userLoginModel, this.Request.Headers["Origin"]);
                return result.IsSuccess ? Ok() : BadRequest(result.FailureReason);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        } 

        [HttpGet]
        [Authorize]
        [Route("logout")]
        public async Task<ActionResult> Logout(string logoutId)
        {
            try
            {
                IRequestResult<string> result = await _usersService.Logout(User.Identity.IsAuthenticated, logoutId, User.GetSubjectId(), User.GetDisplayName());

                if (result.IsSuccess)
                {
                    return new RedirectResult(result.Data);
                }
                else
                {
                    return BadRequest(result.FailureReason);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
