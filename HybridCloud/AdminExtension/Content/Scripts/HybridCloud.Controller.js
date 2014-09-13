/*globals window,jQuery,cdm, HybridCloudAdminExtension*/
(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/HybridCloudAdmin",
        adminSettingsUrl = baseUrl + "/AdminSettings",
        adminProductsUrl = baseUrl + "/Products",
        adminFileServersUrl = baseUrl + "/FileServers";

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data
        });
    }

    function updateAdminSettings(newSettings) {
        return makeAjaxCall(baseUrl + "/UpdateAdminSettings", newSettings);
    }

    function invalidateAdminSettingsCache() {
        return global.Exp.Data.getData({
            url: global.HybridCloudAdminExtension.Controller.adminSettingsUrl,
            dataSetName: HybridCloudAdminExtension.Controller.adminSettingsUrl,
            forceCacheRefresh: true
        });
    }

    function getCurrentAdminSettings() {
        return makeAjaxCall(global.HybridCloudAdminExtension.Controller.adminSettingsUrl);
    }

    function isResourceProviderRegistered() {
        global.Shell.UI.Spinner.show();
        global.HybridCloudAdminExtension.Controller.getCurrentAdminSettings()
        .done(function (response) {
            if (response && response.data.EndpointAddress) {
                return true;
            }
            else {
                return false;
            }
        })
         .always(function () {
             global.Shell.UI.Spinner.hide();
         });
    }

    // Public
    global.HybridCloudAdminExtension = global.HybridCloudAdminExtension || {};
    global.HybridCloudAdminExtension.Controller = {
        adminSettingsUrl: adminSettingsUrl,
        adminProductsUrl: adminProductsUrl,
        adminFileServersUrl: adminFileServersUrl,
        updateAdminSettings: updateAdminSettings,
        getCurrentAdminSettings: getCurrentAdminSettings,
        invalidateAdminSettingsCache: invalidateAdminSettingsCache,
        isResourceProviderRegistered: isResourceProviderRegistered
    };
})(jQuery, this);
