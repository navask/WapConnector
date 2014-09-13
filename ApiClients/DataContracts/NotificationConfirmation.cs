﻿//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Notification confirmation data contract
    /// </summary>
    [DataContract(Namespace = "http://schemas.microsoft.com/windowsazure")]
    public class NotificationConfirmation
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        [DataMember]
        public NotificationState State { get; set; }
    }
}
