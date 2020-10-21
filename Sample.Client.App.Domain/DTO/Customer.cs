using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Client.App.Domain.DTO
{
    public class Customer
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public bool Prepaid { get; set; }
        public CustomerStatus Status { get; set; }
        public User LtlBroker { get; set; }
        public User PrimaryBroker { get; set; }
        public bool HasSignedLTLAddendum { get; set; }
        public double? CreditLimit { get; set; }
        public double? AvailableCredit { get; set; }
        public string LtlBrokerExt { get; set; }
        public CustomerAddress Address { get; set; }
        public bool PendingCustomer { get; set; }
        public bool CreditApproved { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Id: {Id}");
            sb.AppendLine($"UniqueId: {UniqueId}");
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Prepaid: {Prepaid}");
            sb.AppendLine($"Status: {Status?.ToString()}");
            sb.AppendLine($"LTL Broker: {LtlBroker?.ToString()}");
            sb.AppendLine($"Primary Broker: {PrimaryBroker?.ToString()}");
            sb.AppendLine($"Has Signed LTL Addendum: {HasSignedLTLAddendum}");
            sb.AppendLine($"Credit Limit: {CreditLimit}");
            sb.AppendLine($"Available Credit: {AvailableCredit}");
            sb.AppendLine($"LTL Broker Ext: {LtlBrokerExt}");
            sb.AppendLine($"Address: {Address?.ToString()}");
            sb.AppendLine($"Pending Customer: {PendingCustomer}");
            sb.AppendLine($"Credit Approved: {CreditApproved}");
            return sb.ToString();
        }
    }
}
