using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Client.App.Infrastructure
{
    public class CustomerApiOption
    {
        public string Scope { get; set; }

        public string ApiVersionHeader { get; set; }

        public CustomerApiEndpoint Customer { get; set; }
        public CustomerApiEndpoint CustomerStatuses { get; set; }
    }

    public class CustomerApiEndpoint
    {
        public string ApiVersion { get; set; }
        public string EndPoint { get; set; }
    }
}
