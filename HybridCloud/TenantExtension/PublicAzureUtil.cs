using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.TenantExtension
{
    public static class PublicAzureUtil
    {
        const string subscriptionid = "";
        const string base64EncodedCertificate = "";
        public static void createCloudServiceByLocation(string cloudServiceName, string location)
        {
            ComputeManagementClient client = new ComputeManagementClient(getCredentials());
            HostedServiceCreateParameters hostedServiceCreateParams = new HostedServiceCreateParameters
            {
                ServiceName = cloudServiceName,
                Location = location,
                Label = EncodeToBase64(cloudServiceName),
            };
            try
            {
                client.HostedServices.Create(hostedServiceCreateParams);
            }
            catch (CloudException e)
            {
                throw e;
            }
        }

        public static void createCloudServiceByAffinityGroup(string cloudServiceName, string affinityGroupName)
        {
            ComputeManagementClient client = new ComputeManagementClient(getCredentials());
            HostedServiceCreateParameters hostedServiceCreateParams = new HostedServiceCreateParameters
            {
                ServiceName = cloudServiceName,
                AffinityGroup = affinityGroupName,
                Label = EncodeToBase64(cloudServiceName),
            };
            try
            {
                client.HostedServices.Create(hostedServiceCreateParams);
            }
            catch (CloudException e)
            {
                throw e;
            }
        }
        public static string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes
            = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static SubscriptionCloudCredentials getCredentials()
        {
            return new CertificateCloudCredentials(subscriptionid, new X509Certificate2(Convert.FromBase64String(base64EncodedCertificate)));
        }
    }
}
