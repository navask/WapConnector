// ---------------------------------------------------------
// Copyright (c) Schakra Inc. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient.DataContracts
{
    [CollectionDataContract(Name = "FileServers", ItemName = "FileServer", Namespace = Constants.DataContractNamespaces.Default)]
    public class FileServerList : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the structure that contains extra data.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
