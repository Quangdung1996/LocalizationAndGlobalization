using Customer.API.Data;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;

namespace Customer.API.Controllers.V1
{
    [AllowAnonymous]
    [Route("/Customers/v{version:apiVersion}/[controller]")]
    public class CustomersController : BaseController
    {
        private readonly ICustomersDataProvider _customersDataProvider;
        private readonly ResourceManager _resourceManager;
        private readonly IStringLocalizer<CustomersController> _localizer;
        private readonly ILogger _logger;

        public CustomersController(ICustomersDataProvider customersDataProvider,
                                   ResourceManager resourceManager,
                                   IStringLocalizer<CustomersController> localizer,
                                   ILogger<CustomersController> logger)
        {
            _customersDataProvider = customersDataProvider;
            _resourceManager = resourceManager;
            _localizer = localizer;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<CustomerEntity> Get()
        {
            _logger.LogInformation(BuildLogInfo(nameof(Get), "LoggingGetCustomers"));
            return _customersDataProvider.GetCustomers();
        }

        [HttpGet("{id}")]
        public ObjectResult Get(Guid id)
        {
            _logger.LogInformation(BuildLogInfo(nameof(Get), "LoggingGetCustomer", id));

            var customerDataActionResult = _customersDataProvider.TryFindCustomer(id);

            if (!customerDataActionResult.IsSuccess)
            {
                var resourceManager = HttpContext.RequestServices.GetService(typeof(ResourceManager)) as ResourceManager;
                _logger.LogError(BuildLogInfo(nameof(Get), "CustomerNotFound", id));
                return new NotFoundObjectResult(_localizer["CustomerNotFound", id].Value);
            }

            return Ok(customerDataActionResult.CustomerEntity);
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromBody] CustomerDataTransferObject customerDataTransferObject,
                                                  [FromServices] ResourceManager resourceManager)
        {
            if (customerDataTransferObject == null || !customerDataTransferObject.ValidateCustomerDataTransferObject())
            {
                _logger.LogError(BuildLogInfo(nameof(PostAsync), "CustomerInfoInvalid"));
                return BadRequest(BuildStringFromResource("CustomerInfoInvalid"));
            }

            var customerName = $"{customerDataTransferObject.FirstName} {customerDataTransferObject.LastName}";

            _logger.LogInformation(BuildLogInfo(nameof(PostAsync), "LoggingAddingCustomer", customerName));
            var customerDataActionResult = await _customersDataProvider.TryAddCustomerAsync(customerDataTransferObject);

            if (!customerDataActionResult.IsSuccess)
            {
                _logger.LogError(BuildLogInfo(nameof(PostAsync), "UnexpectedServerError"));
                return StatusCode(StatusCodes.Status500InternalServerError, BuildStringFromResource("UnexpectedServerError"));
            }

            _logger.LogInformation(BuildLogInfo(nameof(PostAsync), "LoggingAddedCustomer", customerName));
            return Ok(customerDataActionResult.CustomerEntity);
        }

        // PUT api/Customers/5
        [HttpPut("{id}")]
        public async Task<ObjectResult> PutAsync(Guid id, [FromBody] CustomerDataTransferObject customerDataTransferObject)
        {
            if (customerDataTransferObject == null || !customerDataTransferObject.ValidateCustomerDataTransferObject())
            {
                _logger.LogError(BuildLogInfo(nameof(PutAsync), "CustomerInfoInvalid"));
                return BadRequest(BuildStringFromResource("CustomerInfoInvalid"));
            }

            _logger.LogInformation(BuildLogInfo(nameof(PutAsync), "LoggingUpdatingCustomer", id));

            if (!_customersDataProvider.CustomerExists(id))
            {
                _logger.LogError(BuildLogInfo(nameof(PutAsync), "CustomerNotFound", id));
                return new NotFoundObjectResult(BuildStringFromResource("CustomerNotFound", id));
            }

            var customerDataActionResult = await _customersDataProvider.TryUpdateCustomerAsync(id, customerDataTransferObject);

            if (!customerDataActionResult.IsSuccess)
            {
                _logger.LogError(BuildLogInfo(nameof(PutAsync), "UnexpectedServerError"));
                return StatusCode(StatusCodes.Status500InternalServerError, BuildStringFromResource("UnexpectedServerError"));
            }

            _logger.LogInformation(BuildLogInfo(nameof(PutAsync), "LoggingUpdatedCustomer", id));
            return Ok(customerDataActionResult.CustomerEntity);
        }

        [HttpDelete("{id}")]
        public async Task<ObjectResult> DeleteAsync(Guid id)
        {
            _logger.LogInformation(BuildLogInfo(nameof(DeleteAsync), "LoggingDeletingCustomer", id));

            if (!_customersDataProvider.CustomerExists(id))
            {
                _logger.LogError(BuildLogInfo(nameof(DeleteAsync), "CustomerNotFound", id));
                return new NotFoundObjectResult(BuildStringFromResource("CustomerNotFound", id));
            }

            var customerDataActionResult = await _customersDataProvider.TryDeleteCustomerAsync(id);
            if (!customerDataActionResult.IsSuccess)
            {
                _logger.LogError(BuildLogInfo(nameof(DeleteAsync), "UnexpectedServerError"));
                return StatusCode(StatusCodes.Status500InternalServerError, BuildStringFromResource("UnexpectedServerError"));
            }

            _logger.LogInformation(BuildLogInfo(nameof(DeleteAsync), "LoggingDeletedCustomer", id));
            return Ok(customerDataActionResult.CustomerEntity);
        }

        private string BuildStringFromResource(string resourceStringName, params object[] replacements)
        {
            return string.Format(_resourceManager.GetString(resourceStringName), replacements);
        }

        private string BuildLogInfo(string methodName, string resourceStringName, params object[] replacements)
        {
            return $"{methodName}: {_localizer[resourceStringName, replacements]}";
        }
    }
}