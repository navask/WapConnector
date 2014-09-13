//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Interface for entities that can be configured.
    /// </summary>
    public interface IQuotaConfigurable
    {
        /// <summary>
        /// Gets the state of the configuration.
        /// </summary>
        QuotaConfigurationState ConfigState { get; }
    }
}
