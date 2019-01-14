using AZ_Paas_Demo.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AZ_Paas_Demo.Data.Interfaces
{
    public interface IOrderService
    {
        void AddJuiceToOrder(int juiceId, int storeId);
        List<Orders> GetAllOrders(int storeId);
        Orders GetOrderById(int orderId, int storeId);
        void CancelOrder(int orderId, int storeId);
        void PlaceOrder(int orderId, int storeId);
    }
}
