using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Users.Data.Configuration
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        public static IEnumerable<ApiScope> ApiScopes =>
          new ApiScope[]
          {
          };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("orders","Orders Service"),
                new ApiResource("products","Products Service"),
                new ApiResource("basket","Basket Service"),
            };

        public static IEnumerable<Client> Clients(IConfiguration configuration) =>
            new Client[] 
            {
                 new Client
                {
                    AccessTokenLifetime = 90,
                    //AccessTokenLifetime = 60*60*2, // 2 hours
                    AllowAccessTokensViaBrowser = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = "TestAPI",
                    ClientId = "testApi",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        $"{configuration.GetValue<string>("CustomerWebsiteURL")}/signin-oidc",
                        $"{configuration.GetValue<string>("CustomerWebsiteURL")}/assets/silent-callback.html",
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        $"{configuration.GetValue<string>("CustomerWebsiteURL")}/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    AllowedCorsOrigins =
                    {
                        $"{configuration.GetValue<string>("CustomerWebsiteURL")}"
                    }
                }
            };
    }
}