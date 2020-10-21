using System.Collections.Generic;
using System.Threading.Tasks;
using Sample.Client.App.Domain.DTO;

namespace Sample.Client.App.Domain
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(int id, bool includeCreditInfo);
        Task<IEnumerable<CustomerStatus>> GetCustomerStatuses();
    }
}
