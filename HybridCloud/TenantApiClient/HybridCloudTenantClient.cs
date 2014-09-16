using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient.DataContracts;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.TenantApiClient
{
    public class HybridCloudTenantClient : HybridCloudClient
    {
        /// <summary>
        /// This constructor takes BearerMessageProcessingHandler which reads token as attach to each request
        /// </summary>
        /// <param name="baseEndpoint"></param>
        /// <param name="handler"></param>
        public HybridCloudTenantClient(Uri baseEndpoint, MessageProcessingHandler handler) : base(baseEndpoint, handler)
        {
            
        }
    }
}
