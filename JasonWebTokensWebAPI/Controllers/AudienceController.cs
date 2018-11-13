using JsonWebTokenAuthentication.API.Entities;
using JsonWebTokenAuthentication.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace  JsonWebTokenAuthentication.API.Controllers
{
    [RoutePrefix("api/audience")]
    public class AudienceController :ApiController
    {
        [Route("")]
        public IHttpActionResult Post(AudienceModel audienceModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Audience newAudience = AudienceStore.AddAudience(audienceModel.name);
            return Ok<Audience>(newAudience);
        }
    }
}