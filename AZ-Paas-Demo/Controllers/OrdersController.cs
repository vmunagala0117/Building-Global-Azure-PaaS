using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AZ_Paas_Demo.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AZ_Paas_Demo.Controllers
{

    public class OrdersController : Controller
    {
        private IOrderService _orderService;
        //private IDistributedCache _cache;
        private int _storeId = 1;
        public OrdersController(IOrderService orderService/*, IDistributedCache cache*/)
        {
            _orderService = orderService;
            //_cache = cache;
        }
        public IActionResult Index()
        {
            var orders = _orderService.GetAllOrders(_storeId);
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
            _orderService.PlaceOrder(id);
            return RedirectToAction("Index");
        }
        public IActionResult AddJuiceToOrderLine(int juiceId)
        {
            _orderService.AddJuiceToOrder(juiceId, _storeId);
            return RedirectToAction("Index", "Orders");
        }
    }
}
