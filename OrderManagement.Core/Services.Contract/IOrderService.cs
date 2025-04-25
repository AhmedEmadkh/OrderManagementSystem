using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int CustomerId, Dictionary<int, int> productQuantities, PaymentMethod paymentMethod);
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
        Task<Order?> GetOrderByIdForCustomerAsync(int orderId,int customerId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
}
