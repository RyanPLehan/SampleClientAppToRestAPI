using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sample.Client.App.Domain.DTO;

namespace Sample.Client.App.Domain
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationOption _ApplicationConfig;
        private readonly ICustomerApi _CustomerApi;

        public CustomerService(IOptions<ApplicationOption> appConfig, 
                               ICustomerApi api)
        {
            _ApplicationConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            _CustomerApi = api ?? throw new ArgumentNullException(nameof(api));
        }

        public async Task<Customer> GetCustomer(int id, bool includeCreditInfo)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            return await _CustomerApi.GetCustomer(id, includeCreditInfo);
        }

        public async Task<IEnumerable<CustomerStatus>> GetCustomerStatuses()
        {
            return await _CustomerApi.GetCustomerStatuses();
        }
    }
}
