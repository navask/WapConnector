//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    /// <summary>
    /// Interface add-on containers (i.e. Subscription and Plan)
    /// </summary>
    public interface IAddOnContainer
    {
        /// <summary>
        /// Gets the add-on references.
        /// </summary>
        IList<PlanAddOnReference> AddOnReferences { get; }

        /// <summary>
        /// Gets the add-ons.
        /// </summary>
        IList<PlanAddOn> AddOns { get; }
    }
}
