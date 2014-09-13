﻿//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Represents an add-on list.
    /// </summary>
    [CollectionDataContract(Namespace = "http://schemas.microsoft.com/windowsazure", Name = "PlanAddOns", ItemName = "PlanAddOn")]
    public class PlanAddOnList : List<PlanAddOn>
    {
    }
}
