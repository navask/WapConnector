//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Represents a service quota setting list.
    /// </summary>
    [CollectionDataContract(Namespace = "http://schemas.microsoft.com/windowsazure", Name = "ServiceQuotaSettings", ItemName = "ServiceQuotaSetting")]
    public class ServiceQuotaSettingList : List<ServiceQuotaSetting>
    {
    }
}
