using OrderManagement.Core.Builders;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services.Builders
{
    public class OrderBuilder : IOrderBuilder
    {
        private Order _order;
        public OrderBuilder()
        {
            _order = new Order()
            {
                OrderItems = new List<OrderItem>(),
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending
            };
        }
        public IOrderBuilder AddOrderItem(Product product, int quantity, decimal descount = 0)
        {
            var item = new OrderItem
            {
                Id = product.Id,
                Product = product,
                Quantity = quantity,
                UnitPrice = product.Price,
                Discount = descount
            };
            _order.OrderItems.Add(item);
            return this;
        }
        public IOrderBuilder SetCustomer(Customer customer)
        {
            _order.CustomerId = customer.Id;
            _order.Customer = customer;
            return this;
        }

        public IOrderBuilder SetOrderDate(DateTime date)
        {
            _order.OrderDate = date;
            return this;
        }

        public IOrderBuilder SetPaymentMethod(PaymentMethod method)
        {
            _order.PaymentMethod = method;
            return this;
        }
        public Order Build()
        {
            _order.TotalAmount = _order.OrderItems
                .Sum(i => i.Quantity * i.UnitPrice * (1 - i.Discount / 100));

            return _order;
        }

    }
}
