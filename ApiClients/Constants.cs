//------------------------------------------------------------
// Copyright (c) Schakra Inc.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.WindowsAzurePack.ResourceProvider.DataContracts
{
    internal static class Constants
    {
        internal static class ParameterNames
        {
            public const string SubscriptionId = "SubscriptionId";
            public const string HeaderPrincipalId = "HeaderPrincipalId";
            public const string SubscriptionCertificate = "SubscriptionCertificate";
            public const string SubscriptionCertificateThumbprint = "SubscriptionCertificateThumbprint";
            public const string SubscriptionCertificatePublicKey = "SubscriptionCertificatePublicKey";
            public const string SubscriptionCertificateData = "SubscriptionCertificateData";
        }

        internal static class Headers
        {
            public const string PrincipalId = "x-ms-principal-id";
        }

        internal static class HttpMethods
        {
            public const string Patch = "PATCH";
        }
    }
}
