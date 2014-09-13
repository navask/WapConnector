(function (global, undefined) {
    "use strict";

    var extensions = [{
        name: "HybridCloudTenantExtension",
        displayName: "Hybrid Cloud",
        iconUri: "/Content/HybridCloudTenant/HybridCloudTenant.png",
        iconShowCount: false,
        iconTextOffset: 11,
        iconInvertTextColor: true,
        displayOrderHint: 2 // Display it right after WebSites extension (order 1)
    }];

    global.Shell.Internal.ExtensionProviders.addLocal(extensions);
})(this);