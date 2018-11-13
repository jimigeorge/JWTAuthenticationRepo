using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JsonWebTokenAuthentication.API.Providers;
using System.Data.Entity;
using JsonWebTokenAuthentication.API.Formats;
using System.Configuration;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using System.IdentityModel.Tokens.Jwt;

[assembly: OwinStartup(typeof(JsonWebTokenAuthentication.API.Startup))]

namespace JsonWebTokenAuthentication.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
                
            ConfigureOAuthTokenConsumption(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

          //   Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext1, JsonWebTokenAuthentication.API.Configuration>());

        }
        public void ConfigureOAuth(IAppBuilder app)
        {
          
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //Just for Development Environments, set it to True. (On Production it should be false) 
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://localhost:57293"),
               // RefreshTokenProvider = new CustomRefreshTokenProvider()
            };
            //Token Generation

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = "http://localhost:57293";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            string audienceSecret = ConfigurationManager.AppSettings["as:AudienceSecret"];
            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(audienceSecret));

            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidAudience = "http://localhost:57293",
                ValidIssuer = "http://localhost:57293",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = this.LifetimeValidator,
                IssuerSigningKey = securityKey
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    TokenValidationParameters = validationParameters,
                    TokenHandler = handler
                });

           
            
        }
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}
