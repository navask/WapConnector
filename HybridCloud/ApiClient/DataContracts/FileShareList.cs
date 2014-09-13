// ---------------------------------------------------------
// Copyright (c) Schakra Inc. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient.DataContracts
{    
    [CollectionDataContract(Name = "FileShares", ItemName = "FileShare", Namespace = Constants.DataContractNamespaces.Default)]
    public class FileShareList : List<FileShare>, IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the structure that contains extra data.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
