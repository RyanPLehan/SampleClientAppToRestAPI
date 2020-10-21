using Microsoft.Extensions.DependencyInjection;
using Sample.Client.App.Domain;
using Sample.Client.App.Domain.DTO;
using System;

namespace Sample.Client.App.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup DI, Appsettings
            Startup startup = new Startup();
            startup.Initialize();

            ICustomerService service = startup.ServiceProvider.GetRequiredService<ICustomerService>();
            //GetCustomer(service);               // Uses Http to authenticate
            GetCustomerStatuses(service);     // Uses MSAL to authenticate
        }

        private static void GetCustomer(ICustomerService service)
        {
            var result = service.GetCustomer(811858, false).GetAwaiter().GetResult();
            Console.WriteLine(result);
        }

        private static void GetCustomerStatuses(ICustomerService service)
        {
            var result = service.GetCustomerStatuses().GetAwaiter().GetResult();
            int count = 0;
            foreach (CustomerStatus status in result)
            {
                Console.WriteLine($"{++count}.  {status.ToString()}");
            }
        }
    }
}
