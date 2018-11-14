using JsonWebTokenAuthentication.API;
using JsonWebTokenAuthentication.API.Entities;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace JsonWebTokenAuthentication.API.Formats
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "audience";
        private static readonly string _secret = ConfigurationManager.AppSettings["as:AudienceSecret"];
        private static readonly string _audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
        private readonly string _issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // string audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;
            if (string.IsNullOrWhiteSpace(_audienceId)) throw new InvalidOperationException("AuthenticationTicket.Properties does not include audience");
            // Dummy check to see if the Audience is registered. It should actually check against Client table(which has registered audiences details )
            // For this sample application I have hard coded the audience as there is only one audience.
            Audience audience = AudienceStore.FindAudience(_audienceId);
            var issued = data.Properties.IssuedUtc;
            var now = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddMinutes(30);
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(_secret));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            // ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            //{
            //     new Claim(ClaimTypes.Name, username)
            // });
            var handler = new JwtSecurityTokenHandler();

            var _token =
                (JwtSecurityToken)
                    handler.CreateJwtSecurityToken(issuer: "http://localhost:57293", audience: "http://localhost:57293",
                        subject: data.Identity, notBefore: now, expires: expires, signingCredentials: signingCredentials);


            var jwt = handler.WriteToken(_token);
            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}