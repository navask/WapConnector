//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Represents a plan quota to update.
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/windowsazure")]
    public class PlanQuotaUpdate
    {
        /// <summary>
        /// Gets the service quotas.
        /// </summary>
        [DataMember(Order = 0)]
        public IList<ServiceQuota> ServiceQuotas { get; internal set; }        
    }
}
