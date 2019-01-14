using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AZ_Paas_Demo.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderService _orderService;
        private IDistributedCache _cache;
        public int _storeId { get; set; }
        public OrdersController(IOrderService orderService, IDistributedCache cache)
        {
            _orderService = orderService;
            _cache = cache;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _storeId = int.Parse(_cache.GetString(User.Identity.Name));
            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {
            List<Orders> orders = new List<Orders>();
            orders = _orderService.GetAllOrders(_storeId);
            return View(orders);
        }
        public IActionResult Detail(int id)
        {
            var order = _orderService.GetOrderById(id, _storeId);
            return View(order);
        }
        public IActionResult CancelOrder(int id)
        {
            _orderService.CancelOrder(id, _storeId);
            return RedirectToAction("Index");
        }
        public IActionResult PlaceOrder(int id)
        {
            _orderService.PlaceOrder(id, _storeId);
            return RedirectToAction("Index");
        }
        public IActionResult AddJuiceToOrderLine(int juiceId)
        {
            _orderService.AddJuiceToOrder(juiceId, _storeId);
            return RedirectToAction("Index", "Orders");
        }
    }
}
