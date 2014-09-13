//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.Portal.Configuration;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.AdminExtension
{
    public static class ClientFactory
    {
        //Get Service Management API endpoint
        private static Uri adminApiUri;

        private static BearerMessageProcessingHandler messageHandler;

        //This client is used to communicate with the Hybrid Cloud resource provider
        private static Lazy<HybridCloudClient> HybridCloudRestClient = new Lazy<HybridCloudClient>(
           () => new HybridCloudClient(adminApiUri, messageHandler),
           LazyThreadSafetyMode.ExecutionAndPublication);

        //This client is used to communicate with the Admin API
        private static Lazy<AdminManagementClient> adminApiRestClient = new Lazy<AdminManagementClient>(
            () => new AdminManagementClient(adminApiUri, messageHandler),
            LazyThreadSafetyMode.ExecutionAndPublication);

        static ClientFactory()
        {
            adminApiUri = new Uri(OnPremPortalConfiguration.Instance.RdfeAdminUri);
            messageHandler = new BearerMessageProcessingHandler(new WebRequestHandler());
        }

        public static HybridCloudClient HybridCloudClient
        {
            get
            {
                return HybridCloudRestClient.Value;
            }
        }

        public static AdminManagementClient AdminManagementClient
        {
            get
            {
                return adminApiRestClient.Value;
            }
        }
    }
}
