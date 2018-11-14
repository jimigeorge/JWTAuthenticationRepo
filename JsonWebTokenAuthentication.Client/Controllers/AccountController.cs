using JsonWebTokenAuthentication.Client.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace JsonWebTokenAuthentication.Client.Controllers
{
    public class AccountController : Controller
{
        //Base Url of the Server API
        string Baseurl = "http://localhost:57293/";

        public ActionResult Index()
        {
            return View();
        }
   
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register",model);
            }
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));                             
                //Sending request to Register a new user using HttpClient  
                HttpResponseMessage Res = await client.PostAsJsonAsync<RegisterViewModel>(Baseurl + "api/Account/Register", model);
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var RegResponse = Res.Content.ReadAsStringAsync().Result;
                    //Redirect to Login
                    LoginViewModel vm = new LoginViewModel();
                    vm.UserName = model.UserName;
                    return View("Login",vm);

                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            //Removing the cookie
            //Add a new cookie with expiration date in the past with the same name as the cookie that holds the jwt token 
            if (Request.Cookies["jwttoken"] != null)
            {
                HttpCookie tokenCookie = new HttpCookie("jwttoken");
                tokenCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(tokenCookie);
                HttpCookie nameCookie = new HttpCookie("UserName");
                nameCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(nameCookie);
            }
            // Response.Cookies.Remove("jwttoken");
            // Response.Cookies.Clear();
            Session.Abandon();         
            
            return RedirectToAction("Index", "Account");
           
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model,string returnUrl)
        {

                model.grant_type = "password";
                //Hard coded Audience Id, Audience is the client from where the request is being sent 
                model.Client_Id = "099153c2625149bc8ecb3e85e03f0022";

                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    var FormContent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string,string>("grant_type","password"),
                    new KeyValuePair<string, string>("UserName", model.UserName),
                    new KeyValuePair<string, string>("Password", model.Password),
                    new KeyValuePair<string, string>("Client_Id", model.Client_Id)
                });

                    HttpResponseMessage Res = await client.PostAsync(Baseurl + "token", FormContent);
                    if (Res.IsSuccessStatusCode)
                    {
                        var responseJson = await Res.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(responseJson);
                        var token = jObject.GetValue("access_token").ToString();
                        var expires = jObject.GetValue("expires_in").ToString();
                        //Response.Cookies["jwttoken"].Value = token;
                        //Response.Cookies["jwttoken"].Expires = DateTime.Now.AddSeconds(Convert.ToInt64(expires));
                        var timedCookie = new HttpCookie("jwttoken")
                        {
                            Value = token,
                            Expires = DateTime.Now.AddSeconds(Convert.ToInt64(expires))
                        };
                        Response.Cookies.Add( new HttpCookie("UserName", model.UserName));
                        Response.Cookies.Add(timedCookie);
                        return RedirectToAction("Index", "Order");
                    }

                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                return View();
            
        }
    }
}