using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Client.App.Domain.DTO;

namespace Sample.Client.App.Domain
{
    public interface ICustomerApi
    {
        Task<Customer> GetCustomer(int id, bool includeCreditInfo = false);
        Task<IEnumerable<CustomerStatus>> GetCustomerStatuses();
    }
}
