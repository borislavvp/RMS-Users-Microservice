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
                new ApiScope("orders.read"),
                new ApiScope("orders.write"),
                new ApiScope("meals.fullaccess"),
                new ApiScope("meals.read"),
                new ApiScope("basket.fullaccess"),
                new ApiScope("proeprestaurantgateway.fullaccess"),
                new ApiScope("proepdriversgateway.fullaccess"),
                new ApiScope("proepwebsitegateway.fullaccess"),
          };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("proeprestaurantgateway", "Restaurant App Gateway Service")
                { 
                    Scopes = { "proeprestaurantgateway.fullaccess" }
                },
                new ApiResource("proepwebsitegateway", "Website Gateway Service")
                {
                    Scopes = { "proepwebsitegateway.fullaccess" }
                },
                new ApiResource("proepdriversgateway","Drivers App Gateway Service")
                { 
                    Scopes = { "proepdriversgateway.fullaccess" }
                },
                new ApiResource("orders","Orders Service")
                { 
                    Scopes = { "orders.read", "orders.write" }
                },
                new ApiResource("meals", "Meals Service")
                {
                    Scopes = { "meals.read", "meals.fullaccess" }
                },
                new ApiResource("basket", "Basket Service")
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
                         "openid", "profile", "orders.read", "orders.write", "meals.fullaccess" }
                },
                new Client
                {
                    ClientId = "proepwebsitegatewaytodownstreamtokenexchangeclient",
                    ClientName = "Gateway to Downstream Token Exchange Client",
                    AllowedGrantTypes = new[] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                    RequireConsent = false,
                    ClientSecrets = { new Secret("0cdea0bc-779e-4368-b46b-09956f70712c".Sha256()) },
                    AllowedScopes = {
                         "openid", "profile", "meals.read", "basket.fullaccess", "orders.write" }
                },
                new Client
                {
                    ClientId = "proepdriversgatewaytodownstreamtokenexchangeclient",
                    ClientName = "Gateway to Downstream Token Exchange Client",
                    AllowedGrantTypes = new[] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                    RequireConsent = false,
                    ClientSecrets = { new Secret("1waer6ty-116e-2579-b65o-12357t14663m".Sha256()) },
                    AllowedScopes = {
                         "openid", "profile", "orders.read", "orders.write" }
                },
                new Client
                {
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    AllowAccessTokensViaBrowser = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = AppSettingsHelper.DESKTOP_APP_NAME(configuration),
                    ClientId = AppSettingsHelper.DESKTOP_APP_ID(configuration),
                    ClientUri = AppSettingsHelper.DESKTOP_APP_URI(configuration),
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        $"{AppSettingsHelper.DESKTOP_APP_URI(configuration)}/signin-oidc",
                        $"{AppSettingsHelper.DESKTOP_APP_URI(configuration)}/assets/silent-callback.html",
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        $"{AppSettingsHelper.DESKTOP_APP_URI(configuration)}/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "proeprestaurantgateway.fullaccess",
                    },
                    AllowedCorsOrigins =
                    {
                        AppSettingsHelper.DESKTOP_APP_URI(configuration)
                    }
                }, 
                new Client
                {
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    AllowAccessTokensViaBrowser = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = AppSettingsHelper.MOBILE_APP_NAME(configuration),
                    ClientId = AppSettingsHelper.MOBILE_APP_ID(configuration),
                    ClientUri = AppSettingsHelper.MOBILE_APP_URI(configuration),
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        $"{AppSettingsHelper.MOBILE_APP_URI(configuration)}/signin-oidc",
                        $"{AppSettingsHelper.MOBILE_APP_URI(configuration)}/assets/silent-callback.html",
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        $"{AppSettingsHelper.MOBILE_APP_URI(configuration)}/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "proepdriversgateway.fullaccess",
                    },
                    AllowedCorsOrigins =
                    {
                        AppSettingsHelper.MOBILE_APP_URI(configuration)
                    }
                },
                new Client
                {
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    AllowAccessTokensViaBrowser = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = AppSettingsHelper.WEBSITE_NAME(configuration),
                    ClientId = AppSettingsHelper.WEBSITE_ID(configuration),
                    ClientUri = AppSettingsHelper.WEBSITE_URI(configuration),
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        $"{AppSettingsHelper.WEBSITE_URI(configuration)}/signin-oidc",
                        $"{AppSettingsHelper.WEBSITE_URI(configuration)}/assets/silent-callback.html",
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        $"{AppSettingsHelper.WEBSITE_URI(configuration)}/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "proepwebsitegateway.fullaccess",
                    },
                    AllowedCorsOrigins =
                    {
                        AppSettingsHelper.WEBSITE_URI(configuration)
                    }
                }
            };
    }
}