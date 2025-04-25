using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
    internal class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<Invoice> GenerateInvoiceAsync(Order order)
        {
            var OrderToFind = await _unitOfWork.Repository<Order>().GetByIdAsync(order.Id);

            if (OrderToFind == null)
                throw new KeyNotFoundException($"Order not found.");

            var invoice = new Invoice()
            {
                OrderId = order.Id,
                Order = order,
                InvoiceDate = DateTime.UtcNow,
                TotalAmount = order.TotalAmount
            };

            await _unitOfWork.Repository<Invoice>().AddAsync(invoice);
            await _unitOfWork.CompleteAsync();
            return invoice;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
           return await _unitOfWork.Repository<Invoice>().GetAllAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Invoice>().GetByIdAsync(id);
        }
    }
}
