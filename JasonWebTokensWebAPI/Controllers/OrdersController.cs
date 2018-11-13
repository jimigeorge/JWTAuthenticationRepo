using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace JsonWebTokenAuthentication.API.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {

        private OrderRepository _repo = null;
        public OrdersController()
        {
            _repo = new OrderRepository();

        }
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            
            return Ok(_repo.GetOrders());
        }

        [Authorize]
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var res = _repo.FindOrderById(id);
            return Ok(res);
        }
        [Authorize]
        [Route("CreateOrder")]
        public IHttpActionResult CreateOrder(Order order)
        {           

            using (var ctx = new AuthContext1())
            {
               var res= _repo.AddOrUpdateOrder(order);
               
            }

            return Ok();
        
    }

        [Authorize]
        [Route("")]
        public IHttpActionResult Put(Order order)
        {
            var res = _repo.AddOrUpdateOrder(order);
            if (res>0)
            {                
                return Ok(res);
            }

            else return BadRequest("Order Not Found");
        }

        [Authorize]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var res=_repo.DeleteOrder(id);
            return Ok(res);

        }
        }


    #region Helpers

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }


        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>
            {
                new Order {OrderID = 10248, CustomerName = "Taiseer Joudeh", ShipperCity = "Amman", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "Ahmad Hasan", ShipperCity = "Dubai", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Tamer Yaser", ShipperCity = "Jeddah", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Lina Majed", ShipperCity = "Abu Dhabi", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Yasmeen Rami", ShipperCity = "Kuwait", IsShipped = true}
            };

            return OrderList;
        }
    }

    #endregion
}
