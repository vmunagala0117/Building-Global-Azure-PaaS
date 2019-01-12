using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AZ_Paas_Demo.Data.Services
{
    public class AccountService : IAccountService
    {
        private AzureAd _adSettings;

        public AccountService(IOptions<AzureAd> adSettings)
        {
            _adSettings = adSettings.Value;
        }

        public async Task<Guid> GetStoreIdFromUser(string userId)
        {
            Guid storeId = new Guid();

            string accessToken = await GetBearerAccesToken();

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, GetUserUrl(userId)))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                            storeId = Guid.Parse(json?["physicalDeliveryOfficeName"]?.ToString());
                        }
                    }
                }
            }

            return storeId;
        }

        private string GetUserUrl(string userPrincipalName)
        {
            return string.Format("https://graph.windows.net/{0}/users/{1}?{2}", _adSettings.TenantId, userPrincipalName, "api-version=1.6");
        }

        private async Task<string> GetBearerAccesToken()
        {
            string result = string.Empty;

            // Get OAuth token using client credentials 
            string authString = "https://login.microsoftonline.com/" + _adSettings.TenantId;

            AuthenticationContext authenticationContext = new AuthenticationContext(authString, false);

            // Config for OAuth client credentials  
            ClientCredential clientCred = new ClientCredential(_adSettings.ClientId, _adSettings.AppKey);
            string resource = "https://graph.windows.net";

            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(resource, clientCred);
            result = authenticationResult.AccessToken;

            return result;
        }
    }
}
