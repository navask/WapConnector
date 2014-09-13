// ---------------------------------------------------------
// Copyright (c) Schakra Inc. All rights reserved.
// ---------------------------------------------------------

using System.Web.Mvc;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
