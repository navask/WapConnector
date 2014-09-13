﻿//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Represents a list of quotas.
    /// </summary>
    [CollectionDataContract(Namespace = "http://schemas.microsoft.com/windowsazure", Name = "Quotas", ItemName = "Quota")]
    public class QuotaList : List<Quota>, IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the extension data.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
