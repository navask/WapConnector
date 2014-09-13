//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Type of a resource provider.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/windowsazure")]
    public enum ResourceProviderType
    {
        /// <summary>
        /// Standard type
        /// </summary>
        [EnumMember]
        Standard = 0,

        /// <summary>
        /// Usage service type
        /// </summary>
        [EnumMember]
        UsageProvider,

        /// <summary>
        /// The cloud service provider
        /// </summary>
        [EnumMember]
        CloudServiceProvider,
    }
}
