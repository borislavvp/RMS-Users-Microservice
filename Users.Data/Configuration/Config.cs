using IdentityServer4;
using IdentityServer4.Models;
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

        public static IEnumerable<Client> Clients =>
            new Client[] 
            {
                 new Client
                {
                    AccessTokenLifetime = 1200,
                    AllowOfflineAccess = true,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientName = "TestAPI",
                    ClientId = "testApi",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:4200/response"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:4200/response"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    }
                }
            };
    }
}