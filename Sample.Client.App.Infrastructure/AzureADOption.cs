using System;

namespace Sample.Client.App.Infrastructure
{
    public class AzureADOption
    {
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string OAuth { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
