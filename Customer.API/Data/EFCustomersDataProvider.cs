using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.API.Data
{
    public class EFCustomersDataProvider : DbContext, ICustomersDataProvider
    {
        private readonly CustomerDataActionResult _failedCustomerDataActionResult = new CustomerDataActionResult(false, null);

        public EFCustomersDataProvider(DbContextOptions options)
            : base(options)
        {
        }

        private DbSet<CustomerEntity> Customers { get; set; }

        public IEnumerable<CustomerEntity> GetCustomers()
        {
            return Customers;
        }

        public bool CustomerExists(Guid id)
        {
            return Customers.Any(c => c.Id == id);
        }

        public async Task<CustomerEntity> AddCustomerAsync(CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default)
        {
            if (customerDataTransferObject == null)
            {
                throw new ArgumentNullException();
            }

            if (!customerDataTransferObject.ValidateCustomerDataTransferObject())
            {
                throw new ArgumentException();
            }

            var newCustomerEntity = new CustomerEntity();
            UpdateCustomerInfo(newCustomerEntity, customerDataTransferObject);
            var customerEntityAdded = Customers.Add(newCustomerEntity);

            if (customerEntityAdded?.Entity == null)
            {
                return null;
            }

            await SaveChangesAsync(cancellationToken);
            return customerEntityAdded.Entity;
        }

        public async Task<CustomerEntity> DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var customerEntity = Customers.FirstOrDefault(c => c.Id == id);

            if (customerEntity is null)
            {
                return default;
            }
            var customerRemoved = Customers.Remove(customerEntity);

            await SaveChangesAsync(cancellationToken);
            return customerRemoved.Entity;
        }

        public CustomerEntity FindCustomer(Guid id)
        {
            return Customers.FirstOrDefault(c => c.Id == id);
        }

        public async Task<CustomerEntity> UpdateCustomerAsync(Guid id, CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default)
        {
            var customerEntity = FindCustomer(id);
            UpdateCustomerInfo(customerEntity, customerDataTransferObject);

            await SaveChangesAsync();
            return customerEntity;
        }

        public async Task<CustomerDataActionResult> TryAddCustomerAsync(CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default)
        {
            try
            {
                var customerEntity = await AddCustomerAsync(customerDataTransferObject, cancellationToken);

                if (customerEntity == null)
                {
                    return _failedCustomerDataActionResult;
                }

                return new CustomerDataActionResult(true, customerEntity);
            }
            catch
            {
                return _failedCustomerDataActionResult;
            }
        }

        public async Task<CustomerDataActionResult> TryDeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (!CustomerExists(id))
            {
                return _failedCustomerDataActionResult;
            }

            try
            {
                var customerEntity = await DeleteCustomerAsync(id, cancellationToken);

                if (customerEntity == null)
                {
                    return _failedCustomerDataActionResult;
                }

                await SaveChangesAsync(cancellationToken);
                return new CustomerDataActionResult(true, customerEntity);
            }
            catch
            {
                return _failedCustomerDataActionResult;
            }
        }

        public CustomerDataActionResult TryFindCustomer(Guid id)
        {
            try
            {
                var customerEntity = FindCustomer(id);

                if (customerEntity == null)
                {
                    return _failedCustomerDataActionResult;
                }

                return new CustomerDataActionResult(true, customerEntity);
            }
            catch
            {
                return _failedCustomerDataActionResult;
            }
        }

        public async Task<CustomerDataActionResult> TryUpdateCustomerAsync(Guid id, CustomerDataTransferObject customerDataTransferObject, CancellationToken cancellationToken = default)
        {
            if (!CustomerExists(id))
            {
                return _failedCustomerDataActionResult;
            }

            try
            {
                var customerEntity = await UpdateCustomerAsync(id, customerDataTransferObject, default);

                if (customerEntity == null)
                {
                    return _failedCustomerDataActionResult;
                }

                return new CustomerDataActionResult(true, customerEntity);
            }
            catch
            {
                return _failedCustomerDataActionResult;
            }
        }

        private void UpdateCustomerInfo(CustomerEntity customerEntity,
                                        CustomerDataTransferObject customerDataTransferObject)
        {
            if (customerEntity == null
                || customerDataTransferObject == null
                || !customerDataTransferObject.ValidateCustomerDataTransferObject())
            {
                throw new ArgumentException();
            }

            foreach (var item in customerDataTransferObject.GetType().GetTypeInfo().GetProperties())
            {
                var desinationPropertyInfo = typeof(CustomerEntity).GetProperty(item.Name);
                desinationPropertyInfo?.SetValue(customerEntity, item.GetValue(customerDataTransferObject));
            }
        }
    }
}