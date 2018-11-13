using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using JsonWebTokenAuthentication.API;

namespace JsonWebTokenAuthentication.API.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        //Validate the Client Resource (Audience ) from where the request came from
        //In this sample application its hard coded. But in real projects this validation happens against registrerd  Clients in the Database
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (clientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }

            var audience = AudienceStore.FindAudience(clientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);

        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            //Validate User Credentials
            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            //identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim("sub", context.UserName));
            //identity.AddClaim(new Claim("role", "user"));
            ////identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            ////identity.AddClaim(new Claim(ClaimTypes.Role, "Supervisor"));

            //Add Appropriate Claims
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, context.UserName)
            });

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                       "audience", (context.ClientId == null) ? string.Empty : context.ClientId

                    }

                });

            var ticket = new AuthenticationTicket(claimsIdentity, props);
            context.Validated(ticket);

        }


    }
}