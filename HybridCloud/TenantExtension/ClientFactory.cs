//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.Portal.Configuration;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.TenantExtension
{
    public static class ClientFactory
    {
        //Get Service Management API endpoint
        private static Uri tenantApiUri;

        private static BearerMessageProcessingHandler messageHandler;

        //This client is used to communicate with the Hybrid Cloud resource provider
        private static Lazy<HybridCloudClient> HybridCloudRestClient = new Lazy<HybridCloudClient>(
           () => new HybridCloudClient(tenantApiUri, messageHandler),
           LazyThreadSafetyMode.ExecutionAndPublication);

        static ClientFactory()
        {
            tenantApiUri = new Uri(AppManagementConfiguration.Instance.RdfeUnifiedManagementServiceUri);
            messageHandler = new BearerMessageProcessingHandler(new WebRequestHandler());
        }

        public static HybridCloudClient HybridCloudClient
        {
            get
            {
                return HybridCloudRestClient.Value;
            }
        }
    }
}
