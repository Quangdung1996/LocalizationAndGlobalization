using Domain.Models;

namespace Customer.API.Data
{
    public class CustomerDataActionResult
    {
        public CustomerDataActionResult(bool isSuccess, CustomerEntity customerEntity)
        {
            IsSuccess = isSuccess;
            CustomerEntity = customerEntity;
        }

        public bool IsSuccess { get; }

        public CustomerEntity CustomerEntity { get; private set; }
    }
}