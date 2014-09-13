﻿//-----------------------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Class for Setting
    /// </summary>
    [CollectionDataContract]
    public class SettingCollection : List<Setting>, IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the extension data.
        /// </summary>
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
