using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace JsonWebTokenAuthentication.Client.Controllers
{
    public class BaseController : Controller
    {
        public HttpClient GetHttpClient(string baseAdress)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseAdress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string token;
            if (Session["jwttoken"] != null)
            {
               
                var jwttoken = Request.Cookies["jwttoken"].Value.ToString();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);
                //  var expDate = jwttoken.ValidTo;
                //   if (expDate < DateTime.UtcNow.AddMinutes(1))
                //      token = GetAccessToken().Result;
                //  else
                //      token = Session["jwttoken"] as string;
            }
           
           
            return client;
        }
    }
}