using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JsonWebTokenAuthentication.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonWebTokenAuthentication.Client.Controllers
{
    public class OrderController : Controller
    {
        const string apiBaseurl = "http://localhost:57293/";
        const string apiGetOrdersPath = "api/orders";
        
        // GET: Order
        public async Task<ActionResult> Index()
        {
            string token = null;
            IEnumerable<OrderViewModel> ordersList = new List<OrderViewModel>();
            if (Request.Cookies["jwttoken"] != null)
            {
                token = Request.Cookies["jwttoken"].Value.ToString();
                ordersList = await GetOrders(token, apiBaseurl, apiGetOrdersPath);
            }
            else
            {
                ViewBag.Message = "You do not have permission to access this page.Please Login and try again!";
                ModelState.AddModelError(string.Empty,ViewBag.Message);
            }

            return View(ordersList);
        }

        static async Task<IEnumerable<OrderViewModel>> GetOrders(string token,string apiBaseUri, string requestPath)
        {
            IEnumerable<OrderViewModel> _orderList = null;
            using (var client = new HttpClient())
            {
                //client setup
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));               

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Request
                HttpResponseMessage Res = await client.GetAsync(requestPath);
               
                if (Res.IsSuccessStatusCode)
                {
                    //var responseJson = await 
                    var readTask = Res.Content.ReadAsAsync<IList<OrderViewModel>>();
                    readTask.Wait();

                    _orderList = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    _orderList = Enumerable.Empty<OrderViewModel>();

                   
                }

                return _orderList.ToList<OrderViewModel>();
            }
        }

        public ActionResult Edit(int id)
        {
            OrderViewModel order = null;

            using (var client = new HttpClient())
            {
                var token = Request.Cookies["jwttoken"].Value.ToString();
                client.BaseAddress = new Uri(apiBaseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                //HTTP GET
                var responseTask = client.GetAsync("api/orders/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<OrderViewModel>();
                    readTask.Wait();

                    order = readTask.Result;
                }
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(OrderViewModel order)
        {
            using (var client = new HttpClient())
            {
               var token = Request.Cookies["jwttoken"].Value.ToString();
                client.BaseAddress = new Uri(apiBaseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var putTask = client.PutAsJsonAsync<OrderViewModel>("api/orders", order);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(order);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseurl);
                var token = Request.Cookies["jwttoken"].Value.ToString();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/orders/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(OrderViewModel order)
        {
            if (Request.Cookies["jwttoken"] == null)
            {
                ModelState.AddModelError(string.Empty, "You do not have access to this page. Please Login or Signup");
                return RedirectToAction("Index","Account");
            }
            if (!ModelState.IsValid)
                return View(order);
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["jwttoken"].Value.ToString();
                client.BaseAddress = new Uri(apiBaseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HTTP POST
                var addTask = client.PostAsJsonAsync<OrderViewModel>("api/orders/CreateOrder", order);
                addTask.Wait();

                var result = addTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(order);
        }
    }
  
}