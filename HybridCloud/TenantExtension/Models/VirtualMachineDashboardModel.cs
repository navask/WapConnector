using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.TenantExtension.Models
{
    public class VirtualMachineDashboardModel
    {
        public string VMName { get; set; }
        public string Status { get; set; }
        public string DNSName { get; set; }
        public string Subscription { get; set; }
        public string Location { get; set; }
    }

}