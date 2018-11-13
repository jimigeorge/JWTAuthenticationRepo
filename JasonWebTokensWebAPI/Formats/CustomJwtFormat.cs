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

            Audience audience = AudienceStore.FindAudience(_audienceId);

           // string symmetricKeyAsBase64 = audience.Base64Secret;
            //var keyByteArray = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(TextEncodings.Base64Url.Decode(symmetricKeyAsBase64));
            //var signingKey = new Microsoft.IdentityModel.Tokens.SigningCredentials(keyByteArray, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
            //var issued = data.Properties.IssuedUtc;
            //var expires = data.Properties.ExpiresUtc;
           // var _secret = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

           // var keyByteArray = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(_secret);
           // var signingKey = new Microsoft.IdentityModel.Tokens.SigningCredentials(keyByteArray, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


           // var keybytes = Encoding.ASCII.GetBytes("f9a32479-4549-4cf2-ba47-daa00cf2afc");
           // var symmetricKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keybytes);
            // var signingKey1 = new HmacSigningCredentials(keybytes);
           // var signingKey = new Microsoft.IdentityModel.Tokens.SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            var issued = data.Properties.IssuedUtc;
            //var expires = data.Properties.ExpiresUtc;

            //var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, null);

            //const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
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

            //return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(issuer:_issuer, audience:"http://localhost:57293", claims: data.Identity.Claims, notBefore: issued.Value.UtcDateTime, expires: expires.Value.UtcDateTime,signingCredentials: signingKey));
            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}