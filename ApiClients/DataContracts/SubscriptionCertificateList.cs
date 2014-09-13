//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// A list of subscription certificates
    /// </summary>
    [CollectionDataContract(Namespace = "http://schemas.microsoft.com/windowsazure", Name = "SubscriptionCertificates", ItemName = "SubscriptionCertificate")]
    public class SubscriptionCertificateList : List<SubscriptionCertificate>
    {
    }
}
