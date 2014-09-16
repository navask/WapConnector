using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.AdminApiClient.DataContracts;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.AdminApiClient
{
    public class HybridCloudAdminClient : HybridCloudClient
    {
        public const string AdminSettings = RegisteredPath + "/settings";

        /// <summary>
        /// This constructor takes BearerMessageProcessingHandler which reads token as attach to each request
        /// </summary>
        /// <param name="baseEndpoint"></param>
        /// <param name="handler"></param>
        public HybridCloudAdminClient(Uri baseEndpoint, MessageProcessingHandler handler) : base(baseEndpoint, handler)
        {
            
        }

        /// <summary>
        /// GetAdminSettings returns Hybrid Cloud Resource Provider endpoint information if its registered with Admin API
        /// </summary>
        /// <returns></returns>
        public async Task<AdminSettings> GetAdminSettingsAsync()
        {
            var requestUrl = this.CreateRequestUri(HybridCloudAdminClient.AdminSettings);

            // For simplicity, we make a request synchronously.
            var response = await this.httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<AdminSettings>();
        }

        /// <summary>
        /// UpdateAdminSettings registers Hybrid Cloud Resource Provider endpoint information with Admin API
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAdminSettingsAsync(AdminSettings newSettings)
        {
            var requestUrl = this.CreateRequestUri(HybridCloudAdminClient.AdminSettings);
            var response = await this.httpClient.PutAsJsonAsync<AdminSettings>(requestUrl.ToString(), newSettings);
            response.EnsureSuccessStatusCode();
        }
    }
}
