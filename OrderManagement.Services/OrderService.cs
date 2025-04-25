using Microsoft.Extensions.Logging;
using OrderManagement.Core;
using OrderManagement.Core.Builders;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using OrderManagement.Core.Services.Contract;

namespace OrderManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderBuilder _orderBuilder;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, IOrderBuilder orderBuilder, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _orderBuilder = orderBuilder;
            _logger = logger;
        }
        public async Task<Order> CreateOrderAsync(int CustomerId,Dictionary<int,int> productQuantities,PaymentMethod paymentMethod)
        {
            // 1. Get Customer
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(CustomerId);
            if (customer == null)
                return null;

            // 2. Get All Products
            var productRepo = _unitOfWork.Repository<Product>();
            var productIds = productQuantities.Keys;
            var allProducts = await productRepo.GetAllAsync();
            var products = allProducts.Where(p => productIds.Contains(p.Id)).ToList();

            // 3. Validate Stock and Add Items to Builder
            foreach (var product in products)
            {
                var quantity = productQuantities[product.Id];

                if (product.Stock < quantity)
                    throw new Exception($"Product {product.Name} doesn't have enough stock :(");

                product.Stock -= quantity;
                _orderBuilder.AddOrderItem(product,quantity);
            }

            // 4. Set Customer, PaymentMethod and Build
            var Order = _orderBuilder
                .SetCustomer(customer)
                .SetPaymentMethod(paymentMethod)
                .Build();

            // 5. ApplyDiscount
            ApplyDiscount(Order);

            // 6. Generate Invoice

            var invoice = new Invoice
            {
                Order = Order,
                OrderId = Order.Id,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = Order.TotalAmount,
            };

            // 7. Save Order and Invoice

            await _unitOfWork.Repository<Order>().AddAsync(Order);
            await _unitOfWork.Repository<Invoice>().AddAsync(invoice);
            var result = await _unitOfWork.CompleteAsync();

            if(result <= 0)
                return null;
            return Order;
        }

        private void ApplyDiscount(Order order)
        {
            if (order.TotalAmount > 200)
                foreach (var item in order.OrderItems)
                    item.Discount = 10;
            else if (order.TotalAmount > 100)
                foreach (var item in order.OrderItems)
                    item.Discount = 5;

            order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice * (1 - i.Discount / 100));
        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
        {
            return await _unitOfWork.Repository<Order>().FindAsync(o => o.CustomerId == customerId);
        }

        public async Task<Order?> GetOrderByIdForCustomerAsync(int orderId, int customerId)
        {
            var orders = await _unitOfWork.Repository<Order>().FindAsync(o => o.CustomerId == customerId && o.Id == orderId);
            return orders.FirstOrDefault();
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException($"Order not found");

            order.OrderStatus = newStatus;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
        }
    }
}
