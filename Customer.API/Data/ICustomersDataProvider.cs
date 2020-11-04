using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.API.Data
{
    public interface ICustomersDataProvider : IDisposable
    {
        IEnumerable<CustomerEntity> GetCustomers();

        bool CustomerExists(Guid id);

        Task<CustomerEntity> AddCustomerAsync(CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default);

        Task<CustomerEntity> DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default);

        CustomerEntity FindCustomer(Guid id);

        Task<CustomerEntity> UpdateCustomerAsync(Guid id, CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default);

        CustomerDataActionResult TryFindCustomer(Guid id);

        Task<CustomerDataActionResult> TryDeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default);

        Task<CustomerDataActionResult> TryAddCustomerAsync(CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default);

        Task<CustomerDataActionResult> TryUpdateCustomerAsync(Guid id, CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default);
    }
}