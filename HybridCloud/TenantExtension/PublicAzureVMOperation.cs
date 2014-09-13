using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.TenantExtension
{
    public static class PublicAzureVMOperation
    {
        /// <summary>
        /// Instantiate a new VM Role
        /// </summary>
        public static Role CreateVMRole(this IVirtualMachineOperations client, string cloudServiceName, string roleName, string image, string size, string userName, string password, OSVirtualHardDisk osVHD)
        {
            Role vmRole = new Role
            {
                RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                RoleName = roleName,
                Label = roleName,
                RoleSize = size,
                ConfigurationSets = new List<ConfigurationSet>(),
                //OSVirtualHardDisk = osVHD, 
                ProvisionGuestAgent = true,
                ResourceExtensionReferences = null,
                VMImageName = image,
               //OSVersion = "Windows Server 2012 R2 Datacenter"                
            };
            ConfigurationSet configSet = new ConfigurationSet
            {
                ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                EnableAutomaticUpdates = true,
                ResetPasswordOnFirstLogon = false,
                ComputerName = roleName,
                AdminUserName = userName,
                AdminPassword = password,
                InputEndpoints = new BindingList<InputEndpoint>
               {
                   new InputEndpoint { LocalPort = 3389, Name = "RDP", Protocol = "tcp" },
                   new InputEndpoint { LocalPort = 80, Port = 80, Name = "web", Protocol = "tcp" }
               }
            };

            vmRole.ConfigurationSets.Add(configSet);
            return vmRole;
        }

        public static OSVirtualHardDisk CreateOSVHD(this IVirtualMachineVMImageOperations operation, string cloudserviceName, string vmName, string storageAccount, string imageFamiliyName)
        {
            try
            {
                var osVHD = new OSVirtualHardDisk
                {
                    MediaLink = GetVhdUri(string.Format("{0}.blob.core.windows.net/vhds", storageAccount), cloudserviceName, vmName),
                    SourceImageName = GetSourceImageNameByFamliyName(operation, imageFamiliyName)
                };
                //osVHD.OperatingSystem = "Windows";
                return osVHD;
            }
            catch (CloudException e)
            {


                throw e;
            }
        }

        private static string GetSourceImageNameByFamliyName(this IVirtualMachineVMImageOperations operation, string imageFamliyName)
        {
            var disk = operation.List().Where(o => o.ImageFamily == imageFamliyName).FirstOrDefault();
            var ss = operation.List();

            if (disk != null)
            {
                return disk.Name;
            }
            else
            {
                return "SchakraTestVM1-20140821-AfterSysPrep";
                // throw new CloudException(string.Format("Can't find {0} OS image in current subscription"));
            }
        }

        private static Uri GetVhdUri(string blobcontainerAddress, string cloudServiceName, string vmName, bool cacheDisk = false, bool https = false)
        {
            var now = DateTime.UtcNow;
            string dateString = now.Year + "-" + now.Month + "-" + now.Day;


            var address = string.Format("http{0}://{1}/{2}-{3}-{4}-{5}-650.vhd", https ? "s" : string.Empty, blobcontainerAddress, cloudServiceName, vmName, cacheDisk ? "-CacheDisk" : string.Empty, dateString);
            return new Uri(address);
        }

        public static void CreateVMDeployment(this IVirtualMachineOperations operations, string cloudServiceName, string deploymentName, List<Role> roleList, DeploymentSlot slot = DeploymentSlot.Production)
        {
            try
            {
                VirtualMachineCreateDeploymentParameters createDeploymentParams = new VirtualMachineCreateDeploymentParameters
                {
                    Name = deploymentName,
                    Label = cloudServiceName,
                    Roles = roleList,
                    DeploymentSlot = slot
                };
                operations.CreateDeployment(cloudServiceName, createDeploymentParams);
            }
            catch (CloudException e)
            {
                throw e;
            }
        }
    }
}
