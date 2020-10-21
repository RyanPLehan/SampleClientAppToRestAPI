using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Sample.Client.App.Domain;
using Sample.Client.App.Domain.DTO;
using Sample.Client.App.Infrastructure.DTO;

namespace Sample.Client.App.Infrastructure
{
    public class CustomerApiService : ICustomerApi
    {
        private enum AuthenticationTypeEnum : int
        {
            HTTP = 1,
            MSAL = 2
        }

        private readonly HttpClient _HttpClient;
        private readonly AzureADOption _AzureADConfig;
        private readonly CustomerApiOption _CustomerApiConfig;

        private string _Token = null;   // Obviously this would stored in some type of cache

        public CustomerApiService(HttpClient httpClient, 
                                  IOptions<AzureADOption> azureADConfig, 
                                  IOptions<CustomerApiOption> customerApiConfig)
        {
            _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _AzureADConfig = azureADConfig.Value ?? throw new ArgumentNullException(nameof(azureADConfig));
            _CustomerApiConfig = customerApiConfig.Value ?? throw new ArgumentNullException(nameof(customerApiConfig));
        }

        public async Task<Customer> GetCustomer(int id, bool includeCreditInfo = false)
        {
            Customer ret;
            string endPoint = String.Format(_CustomerApiConfig.Customer.EndPoint, id, includeCreditInfo);

            // Authorization
            await SetAuthToken(AuthenticationTypeEnum.HTTP);
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Token);

            // Custom API Version Header
            _HttpClient.DefaultRequestHeaders.Add(_CustomerApiConfig.ApiVersionHeader, _CustomerApiConfig.Customer.ApiVersion);

            // Call API
            using (HttpResponseMessage httpResponse = await _HttpClient.GetAsync(endPoint))
            {
                string responseData = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    ret = JsonSerializer.Deserialize<Customer>(responseData, jsonSerializerOptions);
                }
                else
                {
                    // Some other non-successful status code.
                    string msg = $"{(int)httpResponse.StatusCode} - {httpResponse.StatusCode.ToString()} ({responseData})";
                    throw new HttpRequestException(msg);
                }
            }

            return ret;
        }

        public async Task<IEnumerable<CustomerStatus>> GetCustomerStatuses()
        {
            IEnumerable<CustomerStatus> ret = Enumerable.Empty<CustomerStatus>();
            string endPoint = _CustomerApiConfig.CustomerStatuses.EndPoint;

            // Authorization
            await SetAuthToken(AuthenticationTypeEnum.MSAL);
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Token);

            // Custom API Version Header
            _HttpClient.DefaultRequestHeaders.Add(_CustomerApiConfig.ApiVersionHeader, _CustomerApiConfig.CustomerStatuses.ApiVersion);

            // Call API
            using (HttpResponseMessage httpResponse = await _HttpClient.GetAsync(endPoint))
            {
                string responseData = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    ret = JsonSerializer.Deserialize<IEnumerable<CustomerStatus>>(responseData, jsonSerializerOptions);
                }
                else
                {
                    // Some other non-successful status code.
                    string msg = $"{(int)httpResponse.StatusCode} - {httpResponse.StatusCode.ToString()} ({responseData})";
                    throw new HttpRequestException(msg);
                }
            }

            return ret;
        }

        private string CreateOAuthEndPoint()
        {
            return String.Format(_AzureADConfig.OAuth, _AzureADConfig.TenantId);
        }

        private async Task SetAuthToken(AuthenticationTypeEnum authenticationType)
        {
            // Guard Clause
            if (!String.IsNullOrWhiteSpace(_Token))
                return;

            switch (authenticationType)
            {
                case AuthenticationTypeEnum.HTTP:
                    AuthenticationHttpRequest httpRequest = new AuthenticationHttpRequest()
                    {
                        ClientId = _AzureADConfig.ClientId,
                        ClientSecret = _AzureADConfig.ClientSecret,
                        Scope = _CustomerApiConfig.Scope,
                    };

                    AuthenticationHttpResponse httpResponse = await AuthenticateViaHttp(httpRequest);
                    _Token = httpResponse.access_token;
                    break;

                case AuthenticationTypeEnum.MSAL:
                    AuthenticationResult msalResponse = await AuthenticateViaMsal();
                    _Token = msalResponse.AccessToken;
                    break;

                default:
                    throw new Exception("Token not set");
                    break;
            }
        }

        /// <summary>
        /// Authenticate via straight HTTP Call
        /// </summary>
        /// <returns></returns>
        private async Task<AuthenticationHttpResponse> AuthenticateViaHttp(AuthenticationHttpRequest authRequest)
        {
            AuthenticationHttpResponse ret = null;

            StringContent content = new StringContent(authRequest.ToString(),
                                                      Encoding.UTF8,
                                                      "application/x-www-form-urlencoded");

            using (HttpResponseMessage httpResponse = await _HttpClient.PostAsync(CreateOAuthEndPoint(), content))
            {
                string responseData = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    ret = JsonSerializer.Deserialize<AuthenticationHttpResponse>(responseData);
                }
                else
                {
                    // Some other non-successful status code.
                    string msg = $"{(int)httpResponse.StatusCode} - {httpResponse.StatusCode.ToString()} ({responseData})";
                    throw new HttpRequestException(msg);
                }
            }

            return ret;
        }


        /// <summary>
        /// Authenticate using MS Identity Library
        /// </summary>
        /// <returns></returns>
        private async Task<AuthenticationResult> AuthenticateViaMsal()
        {
            ConfidentialClientApplicationBuilder builder = ConfidentialClientApplicationBuilder.Create(_AzureADConfig.ClientId);
            //builder.WithTenantId(_AzureADConfig.TenantId);
            builder.WithClientSecret(_AzureADConfig.ClientSecret);

            IConfidentialClientApplication app = builder.Build();

            // Acquire Token
            string[] scopes = new string[] { _CustomerApiConfig.Scope };
            return await app.AcquireTokenForClient(scopes).WithAuthority(CreateOAuthEndPoint(), true).ExecuteAsync();
        }

    }
}
