using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Client.App.Infrastructure.DTO
{
    public class AuthenticationHttpResponse
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
    }
}
