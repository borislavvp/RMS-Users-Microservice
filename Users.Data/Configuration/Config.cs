using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Users.Data.Configuration
{
    public static class Config
    {
        private static readonly string DESKTOP_NAME = "DESKTOP_APP";
        private static readonly string DESKTOP_ID = "DESKTOP_APP_ID";
        private static readonly string WEBSITE_NAME = "WEBSITE_APP";
        private static readonly string WEBSITE_ID = "WEBSITE_APP_ID";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        public static IEnumerable<ApiScope> ApiScopes =>
          new ApiScope[]
          {
                new ApiScope("orders.read"),
                new ApiScope("orders.write"),
                new ApiScope("meals.fullaccess"),
                new ApiScope("basket.fullaccess"),
                new ApiScope("proeprestaurantgateway.fullaccess"),
          };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("proeprestaurantgateway","Restaurant App Gateway Service")
                { 
                    Scopes = { "proeprestaurantgateway.fullaccess" }
                },
                new ApiResource("orders","Orders Service")
                { 
                    Scopes = { "orders.read" , "orders.write" }
                },
                new ApiResource("meals","Meals Service")
                {
                    Scopes = { "meals.fullaccess" }
                },
                new ApiResource("basket","Basket Service")
                {
                    Scopes = { "basket.fullaccess" }
                },
            };

        public static IEnumerable<Client> Clients(IConfiguration configuration) =>
            new Client[] 
            {
                new Client
                {
                    ClientId = "proeprestaurantgatewaytodownstreamtokenexchangeclient",
                    ClientName = "Gateway to Downstream Token Exchange Client",
                    AllowedGrantTypes = new[] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                    RequireConsent = false,
                    ClientSecrets = { new Secret("0cdea0bc-779e-4368-b46b-09956f70712c".Sha256()) },
                    AllowedScopes = {
                         "openid", "profile", "orders.read","meals.fullaccess" }
                },
                new Client
                {
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    AllowAccessTokensViaBrowser = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = DESKTOP_NAME,
                    ClientId = DESKTOP_ID,
                    ClientUri = $"{configuration.GetValue<string>(DESKTOP_NAME)}",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        $"{configuration.GetValue<string>(DESKTOP_NAME)}/signin-oidc",
                        $"{configuration.GetValue<string>(DESKTOP_NAME)}/assets/silent-callback.html",
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        $"{configuration.GetValue<string>(DESKTOP_NAME)}/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "proeprestaurantgateway.fullaccess",
                    },
                    AllowedCorsOrigins =
                    {
                        $"{configuration.GetValue<string>(DESKTOP_NAME)}"
                    }
                },
                new Client
                {
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    AllowAccessTokensViaBrowser = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = WEBSITE_NAME,
                    ClientId = WEBSITE_ID,
                    ClientUri = $"{configuration.GetValue<string>(WEBSITE_NAME)}",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        $"{configuration.GetValue<string>(WEBSITE_NAME)}/signin-oidc",
                        $"{configuration.GetValue<string>(WEBSITE_NAME)}/assets/silent-callback.html",
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        $"{configuration.GetValue<string>(WEBSITE_NAME)}/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "proeprestaurantgateway.fullaccess",
                    },
                    AllowedCorsOrigins =
                    {
                        $"{configuration.GetValue<string>(WEBSITE_NAME)}"
                    }
                }
            };
    }
}