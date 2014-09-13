// ---------------------------------------------------------
// Copyright (c) Schakra Inc. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient.DataContracts
{
    /// <summary>
    /// Error Resource Contract
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespaces.Default, Name = "Error")]
    public sealed class HybridCloudErrorResource
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error state.
        /// </summary>
        [DataMember]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the error severity.
        /// </summary>
        [DataMember]
        public string Severity { get; set; }
    }
}
