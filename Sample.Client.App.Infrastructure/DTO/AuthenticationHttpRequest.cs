using System.Web;

namespace Sample.Client.App.Infrastructure.DTO
{
    public class AuthenticationHttpRequest
    {
        private const string GRANT_TYPE = "client_credentials";

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }

        public override string ToString()
        {
            return $"client_id={ClientId}&scope={HttpUtility.UrlEncode(Scope)}&client_secret={ClientSecret}&grant_type={GRANT_TYPE}";
        }
    }
}
