﻿// ---------------------------------------------------------
// Copyright (c) Schakra Inc. All rights reserved.
// ---------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.HybridCloud.ApiClient.DataContracts
{
    /// <summary>
    /// AdminSettings define data contract of Hybrid Cloud resource provider endpoint registration information
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespaces.Default)]
    public class AdminSettings
    {
        /// <summary>
        /// Address of HybridCloud resource provider
        /// </summary>
        [DataMember(Order = 0)]
        public string EndpointAddress { get; set; }

        /// <summary>
        /// Username used by Admin API to connect with HybridCloud Resource Provider
        /// </summary>
        [DataMember(Order = 1)]
        public string Username { get; set; }

        /// <summary>
        /// Password used by Admin API to connect with HybridCloud Resource Provider
        /// </summary>
        [DataMember(Order = 2)]
        public string Password { get; set; }
    }
}
