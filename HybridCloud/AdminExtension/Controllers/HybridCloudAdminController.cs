// ---------------------------------------------------------
// Copyright (c) Schakra Inc. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.Azure.Portal.Configuration;
using Microsoft.WindowsAzurePack.ResourceProvider.DataContracts;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.AdminExtension.Models;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.AdminApiClient;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.AdminApiClient.DataContracts;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.Common;


namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.AdminExtension.Controllers
{
    [RequireHttps]
    [OutputCache(Location = OutputCacheLocation.None)]
    [PortalExceptionHandler]
    public sealed class HybridCloudAdminController : ExtensionController
    {
        private static readonly string adminAPIUri = OnPremPortalConfiguration.Instance.RdfeAdminUri;
        //This model is used to show registered resource provider information
        public EndpointModel HybridCloudServiceEndPoint { get; set; }

        /// <summary>
        /// Gets the admin settings.
        /// </summary>
        [HttpPost]
        [ActionName("AdminSettings")]
        public async Task<JsonResult> GetAdminSettings()
        {
            try
            {
                var resourceProvider = await ClientFactory.AdminManagementClient.GetResourceProviderAsync
                                                            (HybridCloudClient.RegisteredServiceName, Guid.Empty.ToString());

                this.HybridCloudServiceEndPoint = EndpointModel.FromResourceProviderEndpoint(resourceProvider.AdminEndpoint);
                return this.JsonDataSet(this.HybridCloudServiceEndPoint);
            }
            catch (ManagementClientException managementException)
            {
                // 404 means the Hybrid Cloud resource provider is not yet configured, return an empty record.
                if (managementException.StatusCode == HttpStatusCode.NotFound)
                {
                    return this.JsonDataSet(new EndpointModel());
                }

                //Just throw if there is any other type of exception is encountered
                throw;
            }
        }

        /// <summary>
        /// Update admin settings => Register Resource Provider
        /// </summary>
        /// <param name="newSettings">The new settings.</param>
        [HttpPost]
        [ActionName("UpdateAdminSettings")]
        public async Task<JsonResult> UpdateAdminSettings(EndpointModel newSettings)
        {
            this.ValidateInput(newSettings);

            Microsoft.WindowsAzurePack.ResourceProvider.DataContracts.ResourceProvider HybridCloudResourceProvider;
            string errorMessage = string.Empty;

            try
            {
                //Check if resource provider is already registered or not
                HybridCloudResourceProvider = await ClientFactory.AdminManagementClient.GetResourceProviderAsync(HybridCloudClient.RegisteredServiceName, Guid.Empty.ToString());
            }
            catch (ManagementClientException exception)
            {
                // 404 means the Hybrid Cloud resource provider is not yet configured, return an empty record.
                if (exception.StatusCode == HttpStatusCode.NotFound)
                {
                    HybridCloudResourceProvider = null;
                }
                else
                {
                    //Just throw if there is any other type of exception is encountered
                    throw;
                }
            }

            if (HybridCloudResourceProvider != null)
            {
                //Resource provider already registered so lets update endpoint
                HybridCloudResourceProvider.AdminEndpoint = newSettings.ToAdminEndpoint();
                HybridCloudResourceProvider.TenantEndpoint = newSettings.ToTenantEndpoint();
                HybridCloudResourceProvider.NotificationEndpoint = newSettings.ToNotificationEndpoint();
                HybridCloudResourceProvider.UsageEndpoint = newSettings.ToUsageEndpoint();
            }
            else
            {
                //Resource provider not registered yet so lets register new one now
                HybridCloudResourceProvider = new Microsoft.WindowsAzurePack.ResourceProvider.DataContracts.ResourceProvider()
                {
                    Name = HybridCloudClient.RegisteredServiceName,
                    DisplayName = "Hybrid Cloud",
                    InstanceDisplayName = HybridCloudClient.RegisteredServiceName + " Instance",
                    Enabled = true,
                    PassThroughEnabled = true,
                    AllowAnonymousAccess = false,
                    AdminEndpoint = newSettings.ToAdminEndpoint(),
                    TenantEndpoint = newSettings.ToTenantEndpoint(),
                    NotificationEndpoint = newSettings.ToNotificationEndpoint(),
                    UsageEndpoint = newSettings.ToUsageEndpoint(),
                    MaxQuotaUpdateBatchSize = 3 // Check link http://technet.microsoft.com/en-us/library/dn520926(v=sc.20).aspx
                };
            }

            var testList = new ResourceProviderVerificationTestList()
                               {
                                   new ResourceProviderVerificationTest()
                                   {
                                       TestUri = new Uri(HybridCloudAdminController.adminAPIUri + HybridCloudAdminClient.AdminSettings),
                                       IsAdmin = true
                                   }
                               };
            try
            {
                // Resource Provider Verification to ensure given endpoint and username/password is correct
                // Only validate the admin RP since we don't have a tenant subscription to do it.
                var result = await ClientFactory.AdminManagementClient.VerifyResourceProviderAsync(HybridCloudResourceProvider, testList);
                if (result.HasFailures)
                {
                    throw new HttpException("Invalid endpoint or bad username/password");
                }
            }
            catch (ManagementClientException ex)
            {
                throw new HttpException("Invalid endpoint or bad username/password " + ex.Message.ToString());
            }

            //Finally Create Or Update resource provider
            Task<Microsoft.WindowsAzurePack.ResourceProvider.DataContracts.ResourceProvider> rpTask = (string.IsNullOrEmpty(HybridCloudResourceProvider.Name) || String.IsNullOrEmpty(HybridCloudResourceProvider.InstanceId))
                                                ? ClientFactory.AdminManagementClient.CreateResourceProviderAsync(HybridCloudResourceProvider)
                                                : ClientFactory.AdminManagementClient.UpdateResourceProviderAsync(HybridCloudResourceProvider.Name, HybridCloudResourceProvider.InstanceId, HybridCloudResourceProvider);

            try
            {
                await rpTask;
            }
            catch (ManagementClientException e)
            {
                throw e;
            }

            return this.Json(newSettings);
        }

        private void ValidateInput(EndpointModel newSettings)
        {
            if (newSettings == null)
            {
                throw new ArgumentNullException("newSettings");
            }

            if (String.IsNullOrEmpty(newSettings.EndpointAddress))
            {
                throw new ArgumentNullException("EndpointAddress");
            }

            if (String.IsNullOrEmpty(newSettings.Username))
            {
                throw new ArgumentNullException("Username");
            }

            if (String.IsNullOrEmpty(newSettings.Password))
            {
                throw new ArgumentNullException("Password");
            }
        }
    }
}