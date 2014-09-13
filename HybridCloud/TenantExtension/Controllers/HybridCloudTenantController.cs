//-----------------------------------------------------------------------
//   Copyright (c) Schakra Inc.  All rights reserved.
//-----------------------------------------------------------------------

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.Common;
using Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.TenantExtension.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.TenantExtension.Controllers
{
    [RequireHttps]
    [OutputCache(Location = OutputCacheLocation.None)]
    [PortalExceptionHandler]
    public sealed class HybridCloudTenantController : ExtensionController
    {
        /// <summary>
        /// List file shares belong to subscription
        /// NOTE: For this sample dummy entries will be displayed
        /// </summary>
        /// <param name="subscriptionIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ListFileShares(string[] subscriptionIds)
        {
            // Make the requests sequentially for simplicity
            var fileShares = new List<FileShareModel>();

            if (subscriptionIds == null || subscriptionIds.Length == 0)
            {
                throw new HttpException("Subscription Id not found");
            }

            foreach (var subId in subscriptionIds)
            {
                var fileSharesFromApi = await ClientFactory.HybridCloudClient.ListFileSharesAsync(subId);
                fileShares.AddRange(fileSharesFromApi.Select(d => new FileShareModel(d)));
            }

            return this.JsonDataSet(fileShares);
        }

        /// <summary>
        /// Create new file share for subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="fileShareToCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateFileShare(string subscriptionId, FileShareModel fileShareToCreate)
        {
            await ClientFactory.HybridCloudClient.CreateFileShareAsync(subscriptionId, fileShareToCreate.ToApiObject());
            return this.Json(fileShareToCreate);
        }

        /// <summary>
        /// Create Virtual Machines
        /// </summary>
        /// <param name="vmName">VM Name</param>
        /// <param name="location">VM location</param>
        /// <param name="image">VM image name</param>
        /// <param name="size">VM size</param>
        /// <param name="affinityGroup">VM affinity group</param>
        /// <param name="userName">username of the virtual machine</param>
        /// <param name="password">password of the virtual machine</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateVirtualMachine(string vmName, string location, string image, string size, string affinityGroup, string userName, string password)
        {
            try
            {
                string imageVM = GetImageName(image, location);
                ComputeManagementClient client = new ComputeManagementClient(PublicAzureUtil.getCredentials());
                //You need a hosted service VM.
                if (client.HostedServices.CheckNameAvailability(vmName).IsAvailable)
                {
                    if (!string.IsNullOrEmpty(affinityGroup))
                        PublicAzureUtil.createCloudServiceByLocation(vmName, affinityGroup);
                    else
                        PublicAzureUtil.createCloudServiceByLocation(vmName, location);
                }

                var OSVHD = client.VirtualMachineVMImages.CreateOSVHD(vmName, vmName, "portalvhds0lpqj8jdcq4k7", "blob");
                var VMROle = client.VirtualMachines.CreateVMRole(vmName, vmName, imageVM, size, userName, password, OSVHD);
                List<Role> roleList = new List<Role> { VMROle }; 
                client.VirtualMachines.CreateVMDeployment(vmName, vmName, roleList);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            // Thread.Sleep(10000);
            return this.Json((object)("VM Created Successfully"), JsonRequestBehavior.AllowGet);
        }

        private Task<ComputeManagementClient> CreateVM()
        {

            return null;
        }

        /// <summary>
        /// List virtual machines in Azure
        /// </summary>
        /// <param name="subscriptionIds"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListVirtualMachines(string[] subscriptionIds)
        {
            ComputeManagementClient client = new ComputeManagementClient(PublicAzureUtil.getCredentials());
            List<VirtualMachineDashboardModel> vmList = new List<VirtualMachineDashboardModel>();
            var allHostedServices = client.HostedServices.List();
            foreach (var hostedService in allHostedServices)
            {
                try
                {
                    var deploymnet = client.Deployments.GetByName(hostedService.ServiceName, hostedService.ServiceName);
                    if (deploymnet != null && !string.IsNullOrEmpty(deploymnet.Name))
                    {
                        var sss = client.VirtualMachines.Get(hostedService.ServiceName, hostedService.ServiceName, hostedService.ServiceName);
                        vmList.Add(new VirtualMachineDashboardModel
                        {
                            VMName = hostedService.ServiceName,
                            DNSName = hostedService.ServiceName.ToLower() + ".cloudapp.net",
                            Location = hostedService.Properties.Location,
                            Subscription = "Visual Studio Premium with MSDN",
                            Status = deploymnet.Status.ToString().Equals("Suspended", StringComparison.OrdinalIgnoreCase)
                            ? "Stopped (Deallocated)" : deploymnet.Status.ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return this.JsonDataSet(vmList);
        }

        /// <summary>
        /// Return exact Imagename from VM image name and location
        /// </summary>
        /// <param name="imageName">VM image name</param>
        /// <param name="location">location</param>
        /// <returns></returns>
        private string GetImageName(string imageName, string location)
        {
            string image = "";
            if (imageName.Equals("WindowsServer2012R2Datacenter", StringComparison.OrdinalIgnoreCase))
            {
                image = location.Equals("East Asia", StringComparison.OrdinalIgnoreCase)
                    ? "WindowsServer2012R2Datacenter" : "WindowsServer2012R2Datacenter-EastUS";
            }
            else if (imageName.Equals("WindowsServer2012Datacenter", StringComparison.OrdinalIgnoreCase))
            {
                image = location.Equals("East Asia", StringComparison.OrdinalIgnoreCase)
                   ? "WindowsServer2012Datacenter-EastAsia" : "WindowsServer2012Datacenter";
            }
            return image;
        }
    }

    public class VirtualMachineDashboardModel
    {
        public string VMName { get; set; }
        public string Status { get; set; }
        public string DNSName { get; set; }
        public string Subscription { get; set; }
        public string Location { get; set; }
    }
}