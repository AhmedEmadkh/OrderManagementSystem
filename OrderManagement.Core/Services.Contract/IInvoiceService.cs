using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services.Contract
{
    public interface IInvoiceService
    {
        Task<Invoice> GenerateInvoiceAsync(Order order);
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();

    }
}
