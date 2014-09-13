(function (global, undefined) {
    "use strict";

    var extensions = [{
        name: "HybridCloudAdminExtension",
        displayName: "Hybrid Cloud",
        iconUri: "/Content/SystemCenterAdmin/SystemCenterAdmin.png",
        iconShowCount: false,
        iconTextOffset: 11,
        iconInvertTextColor: true,
        displayOrderHint: 51
    }];

    global.Shell.Internal.ExtensionProviders.addLocal(extensions);
})(this);