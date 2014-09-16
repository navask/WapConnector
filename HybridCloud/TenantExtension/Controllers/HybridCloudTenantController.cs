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
        
    }
}