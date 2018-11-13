using JsonWebTokenAuthentication.API.Entities;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace JsonWebTokenAuthentication.API
{
    public static class AudienceStore
    {
        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();
        static AudienceStore()
        {
            AudiencesList.TryAdd("099153c2625149bc8ecb3e85e03f0022",
                new Audience { ClientId = "099153c2625149bc8ecb3e85e03f0022",
                Base64Secret= "NDAxYjA5ZWFiM2MwMTNkNGNhNTQ5MjJiYjgwMmJlYzhmZDUzMTgxOTJiMGE3NWYyMDFkOGIzNzI3NDI5MDkwZmIzMzc1OTFhYmQzZTQ0NDUzYjk1NDU1NWI3YTA4MTJlMTA4MWMzOWI3NDAyOTNmNzY1ZWFlNzMxZjVhNjVlZDE=",
                Name="ResourceServer.Api 1"});
        }

        public static Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");
            var key = new Byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);
            Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AudiencesList.TryAdd(clientId, newAudience);
            return newAudience;
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = null;
            if(AudiencesList.TryGetValue(clientId,out audience))
            {
                return audience;
            };
            return null;
        }
    }
}