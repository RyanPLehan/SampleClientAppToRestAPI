using System.Text;

namespace Sample.Client.App.Domain.DTO
{
    public class CustomerAddress
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Street: {Address} - City: {City} - State: {State} - PostalCode: {PostalCode}");
            return sb.ToString();
        }
    }
}
