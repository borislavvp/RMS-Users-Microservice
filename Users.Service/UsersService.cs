﻿using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Users.Data.Models;
using Users.Service.Common.Dtos;
using Users.Service.Common.Interfaces;
using Users.Service.Common.Models;
using Users.Service.Models;

namespace Users.Service
{
    public class UsersService
    {

        private readonly IConfiguration _configuration;
        private readonly IClientStore _clientStore;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersService(
            IClientStore clientStore,
            IConfiguration configuration,
            IIdentityServerInteractionService interaction,
            IEventService events,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _clientStore = clientStore;
            _configuration = configuration;
            _interaction = interaction;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _events = events;
        }

        public async Task<IRequestResult> RegisterUser(UserRegisterDTO userModel)
        {
            var user = new ApplicationUser
            {
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Address = userModel.Address,
                PhoneNumber = userModel.PhoneNumber,
                Subject = new Guid().ToString(),
                Role = new IdentityRole("Customer"),
                UserName = userModel.Email
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            // If creation of the user fails return a failure
            if (!result.Succeeded)
                return RequestResult.Failure(result.Errors.First().ToString());
            // Else try to sign in the user and return the result
            else
                return await SignInUser(user.Email);
        }

        public async Task<IRequestResult> Login(UserLoginDTO userLoginModel)
        {
            var user = await _userManager.FindByEmailAsync(userLoginModel.Email);

            if(user == null)
            {
                return RequestResult.Failure("There is no user in the system with that email!");
            }

            if(await _userManager.CheckPasswordAsync(user, userLoginModel.Password))
            {
                return await SignInUser(user.Email);
            }
            else
            {
                return RequestResult.Failure("Invalid password!");
            }

        } 
        
        private async Task<IRequestResult> SignInUser(string userEmail)
        {
            var userToLogin = await _userManager.FindByEmailAsync(userEmail);

            if(userToLogin.Id == null)
            {
                return RequestResult.Failure("There is no user in the system with that email!");
            }

            await _events.RaiseAsync(new UserLoginSuccessEvent(userToLogin.UserName, userToLogin.Subject, userToLogin.UserName, clientId: userToLogin.Id));

            var props = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                AllowRefresh = true,
            };

            // This is going to set the auth cookie in the users browser
            await _signInManager.SignInAsync(userToLogin, props);

            return RequestResult.Success();
        }

        public async Task<IRequestResult<string>> Logout(bool isCurrentUserAuthenticated,string logoutId, string subjectId, string clientDisplayName)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);

            if (context == null)
            {
                return RequestResult<string>.Failure("Invalid request!");
            }

            if (isCurrentUserAuthenticated)
            {
                // This is going to remove the auth cookie from the browser
                await _signInManager.SignOutAsync();

                await _events.RaiseAsync(new UserLogoutSuccessEvent(subjectId, clientDisplayName));
            }

            // This returns redirect uri to the external page for sign-out
            return RequestResult<string>.Success(context.PostLogoutRedirectUri + "?logoutId=" + logoutId);
        }

        public async Task<IRequestResult<string>> GetLoginRedirectPageUriAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            
            if(context == null || await _clientStore.FindClientByIdAsync(context.Client.ClientId) == null)
            {
                return RequestResult<string>.Failure("Invalid request!");
            }

            return RequestResult<string>.Success(
                $"{context.Client.ClientUri}{_configuration.GetValue<string>($"{context.Client.ClientName}-LoginPath")}");
        }
        
    }
}
