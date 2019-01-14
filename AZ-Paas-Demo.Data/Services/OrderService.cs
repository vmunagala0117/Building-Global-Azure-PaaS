using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace AZ_Paas_Demo.Data.Services
{
    public class OrderService : IOrderService
    {
        private IContextFactory _context;
        private IStoreService _storeService;

        public OrderService(IContextFactory context, IStoreService storeService)
        {
            _context = context;
            _storeService = storeService;
        }

        public void AddJuiceToOrder(int juiceId, int storeId)
        {
            var dbContext = _context.GetRoutedContext(storeId);
            //get the existing order if any?
            var storeOrder = dbContext.Orders
                .Where(s => s.StoreId == storeId)
                .Include(ol => ol.OrderLines)
                    .ThenInclude(ol => ol.Juice)
                .Include(o => o.Store)
                .FirstOrDefault();

            if (storeOrder != null)
            {
                //check if orderline item already exists?
                bool orderLineExists = false;
                foreach (var lines in storeOrder.OrderLines)
                {
                    //if exists, then update the line item
                    if (lines.Juice.Id == juiceId)
                    {
                        lines.Quantity++;
                        orderLineExists = true;
                        storeOrder.Price += lines.Juice.Price;
                    }
                }
                //add a new order line item
                if (!orderLineExists)
                {
                    var juice = dbContext.Juices.Where(c => c.Id == juiceId).FirstOrDefault();
                    storeOrder.OrderLines.Add(new OrderLines
                    {
                        Juice = juice,
                        Quantity = 1
                    });
                    storeOrder.Price += juice.Price;
                }
                dbContext.Orders.Update(storeOrder);
                dbContext.SaveChanges();
            }
            else
            {
                var juice = dbContext.Juices.Where(c => c.Id == juiceId).FirstOrDefault();
                storeOrder = new Orders
                {
                    Date = DateTimeOffset.Now,
                    //Store = _storeService.GetStoreById(storeId),
                    Status = "New",
                    StoreId = storeId
                };
                storeOrder.OrderLines.Add(new OrderLines
                {
                    Juice = juice,
                    Quantity = 1
                });
                storeOrder.Price = juice.Price;
                dbContext.Orders.Add(storeOrder);
                dbContext.SaveChanges();
            }
        }
        public void CancelOrder(int orderId, int storeId)
        {
            var order = _context.GetRoutedContext(storeId).Orders.Where(o => o.Id == orderId).FirstOrDefault();
            if (order != null)
            {
                _context.GetRoutedContext(storeId).Remove(order);
                _context.GetRoutedContext(storeId).SaveChanges();
            }
        }

        public List<Orders> GetAllOrders(int storeId)
        {
            var dbContext = _context.GetRoutedContext(storeId);
            var orders = dbContext.Orders.Where(s => s.StoreId == storeId)
                .Include(o => o.OrderLines)
                .Include(o => o.Store)
                .ToList();
            return orders;
        }

        public Orders GetOrderById(int orderId, int storeId)
        {
            //.Include is Eager loading
            var order = _context.GetRoutedContext(storeId).Orders
                .Where(s => s.StoreId == storeId && s.Id == orderId)
                .Include(ol => ol.OrderLines)
                    .ThenInclude(ol => ol.Juice)//including OrderLine property explicitly.
                .Include(o => o.Store)
                .FirstOrDefault();
            return order;
        }

        public void PlaceOrder(int orderId, int storeId)
        {
            var order = _context.GetRoutedContext(storeId).Orders
                                .Where(o => o.Id == orderId)
                                .Include(ol => ol.OrderLines).ThenInclude(ol => ol.Juice)
                                .Include(o => o.Store)
                                .FirstOrDefault();
            if (order != null)
            {
                foreach (var line in order.OrderLines)
                {
                    //have to get a reference to the actual juice, not the attached one, otherwise EF will protest
                    line.Juice = _context.GetRoutedContext(storeId).Juices.Where(c => c.Id == line.Juice.Id).FirstOrDefault();
                }
                order.Status = "Placed";

                //have to get a reference to the actual store, not the attached one, otherwise EF will protest
                order.Store = _context.GetRoutedContext(storeId).Stores.Where(s => s.Id == order.Store.Id).FirstOrDefault();

                //Update status
                _context.GetRoutedContext(storeId).Orders.Update(order);
                if (_context.GetRoutedContext(storeId).SaveChanges() > 0) //check for success
                {
                    //if all went well, remove the item from cache
                }
            }

        }
    }
}

