using OrderManagement.Core.Entities;
using OrderManagement.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Builders
{
    public interface IOrderBuilder
    {
        IOrderBuilder SetCustomer(Customer customer);
        IOrderBuilder SetOrderDate(DateTime date);
        IOrderBuilder SetPaymentMethod(PaymentMethod method);
        IOrderBuilder AddOrderItem(Product product,int quantity, decimal descount = 0);
        Order Build();
    }
}
