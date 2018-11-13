using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using JsonWebTokenAuthentication.API.Models;
using System.Threading.Tasks;
using JsonWebTokenAuthentication.API.Controllers;

namespace JsonWebTokenAuthentication.API
{
    public class OrderRepository : IDisposable
    {
        private AuthContext1 _ctx;


        public OrderRepository()
        {
            _ctx = new AuthContext1();

        }

        public int AddOrUpdateOrder(Order order)
        {
            var _order = _ctx.Orders.FirstOrDefault(c => c.OrderID == order.OrderID);
            if (_order == null)
            {
                _order = new Entities.Order
                {
                    //OrderID = order.OrderID,
                    CustomerName = order.CustomerName,
                    ShipperCity = order.ShipperCity,
                    IsShipped = order.IsShipped
                };
                _ctx.Orders.Add(_order);
               
            }
            else
                _order.OrderID = order.OrderID;
                _order.CustomerName = order.CustomerName;
                _order.ShipperCity = order.ShipperCity;
                _order.IsShipped = order.IsShipped;

            _ctx.SaveChanges();

            return _order.OrderID;
        }
        public List<Order> GetOrders()
        {


            var result = _ctx.Orders;
            List<Order> _orders = new List<Order>();
            foreach (var item in result)
            {
                Order _order = new Order()
                {
                    OrderID = item.OrderID,
                    CustomerName = item.CustomerName,
                    ShipperCity = item.ShipperCity,
                    IsShipped = item.IsShipped
                };
                _orders.Add(_order);

            }

            return _orders;
        }
        public Order FindOrderById(int id)
        {
            var res = new Order();
            var _order = _ctx.Orders.FirstOrDefault(c => c.OrderID == id);
            if (_order != null)
            {
                res.OrderID = _order.OrderID;
                res.CustomerName = _order.CustomerName;
                res.ShipperCity = _order.ShipperCity;
                res.IsShipped = _order.IsShipped;
           }
            return res;
        }
        public int DeleteOrder(int id)
        {          

            var _order = _ctx.Orders.FirstOrDefault(c => c.OrderID == id);
            if (_order != null)
                _ctx.Entry(_order).State = System.Data.Entity.EntityState.Deleted;
           
            return  _ctx.SaveChanges();

        }
        

        public void Dispose()
        {
            _ctx.Dispose();

        }
    }
}