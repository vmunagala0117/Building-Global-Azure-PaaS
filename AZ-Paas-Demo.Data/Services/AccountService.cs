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
        private IQueueService _queueService;
        public AccountService(IOptions<AzureAd> adSettings, IQueueService queueService)
        {
            _adSettings = adSettings.Value;
            _queueService = queueService;
        }

        public async Task<int> GetStoreIdFromUser(string userId)
        {
            int storeId = -1;
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
                            storeId = int.Parse(json?["officeLocation"]?.ToString());
                        }
                    }
                }
            }
            return storeId;
        }

        private string GetUserUrl(string userPrincipalName)
        {
            return string.Format("https://graph.microsoft.com/v1.0/users/{0}", userPrincipalName);
        }

        private async Task<string> GetBearerAccesToken()
        {
            string result = string.Empty;

            // Get OAuth token using client credentials 
            string authString = "https://login.microsoftonline.com/" + _adSettings.TenantId;

            AuthenticationContext authenticationContext = new AuthenticationContext(authString, false);

            // Config for OAuth client credentials  
            ClientCredential clientCred = new ClientCredential(_adSettings.ClientId, _adSettings.AppKey);
            string resource = "https://graph.microsoft.com"; //Microsoft Graph API

            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(resource, clientCred);
            result = authenticationResult.AccessToken;

            return result;
        }

        public void RegisterNewStoreAndUser(Register storeUserData)
        {
            //call queue service
            _queueService.QueueNewStoreCreation(JsonConvert.SerializeObject(storeUserData));
        }

        //https://docs.microsoft.com/en-us/graph/api/user-post-users?view=graph-rest-1.0
        public async Task<string> CreateNewUser(Register userInfo, string storeId)
        {
            string result = string.Empty;
            string accessToken = await GetBearerAccesToken();
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, "https://graph.microsoft.com/v1.0/users"))
                {
                    var newUser = CreateNewUserObject(userInfo, storeId);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    request.Content = new StringContent(newUser.ToString(), Encoding.UTF8, "application/json");

                    result = userInfo.PersonEmail;
                    using (var response = await client.SendAsync(request))
                    {
                        if (response.StatusCode != HttpStatusCode.Created)
                        {
                            result = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            return result;
        }

        private JObject CreateNewUserObject(Register userInfo, string storeId)
        {
            /*
             * {
                  "accountEnabled": true,
                  "displayName": "Abc Test",
                  "mailNickname": "AbcTest",
                  "passwordProfile": {
                    "forceChangePasswordNextSignIn": true,
                    "password": "p@ssword1"
                  },
                  "userPrincipalName": "abc@vamsimunagalahotmail.onmicrosoft.com",
                  "officeLocation": "1"
                }
             */
            JObject newUserJson = new JObject(
                                        new JProperty("accountEnabled", true),
                                        new JProperty("displayName", string.Format("{0} {1}", userInfo.PersonFirstName, userInfo.PersonLastName)),
                                        new JProperty("mailNickname", string.Format("{0}{1}", userInfo.PersonFirstName, userInfo.PersonLastName)),
                                        new JProperty("passwordProfile",
                                                            new JObject(
                                                                new JProperty("forceChangePasswordNextSignIn", true),
                                                                new JProperty("password", "p@ssword1")
                                                                )
                                                      ),
                                        new JProperty("userPrincipalName", userInfo.PersonEmail),
                                        new JProperty("officeLocation", storeId)
                                        );

            return newUserJson;
        }
                
    }
}
