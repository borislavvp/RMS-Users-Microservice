using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Data.Configuration;
using Users.Data.Models;

namespace Users.Service.RequestValidators
{
    public class CustomAuthorizeRequestValidator : ICustomAuthorizeRequestValidator
    {
        private IConfiguration _conf;
        public CustomAuthorizeRequestValidator(IConfiguration conf)
        {
            _conf = conf;
        }
        public Task ValidateAsync(CustomAuthorizeRequestValidationContext context)
        {
            var clientID = context.Result.ValidatedRequest.ClientId;
            var subject = context.Result.ValidatedRequest.Subject;

            if(
                clientID.Equals(AppSettingsHelper.DESKTOP_APP_ID(_conf)) && 
                !subject.IsInRole(RoleConstants.ChorbaDeckAdmin) 
                )
            {
                context.Result.IsError = true;
                context.Result.ErrorDescription = "Access denied!";
                context.Result.Error = "Unauthorized";
            }
            return Task.CompletedTask;
        }
    }
}
