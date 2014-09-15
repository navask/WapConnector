/*globals window,jQuery,Shell,Exp,waz*/

(function (global, $, undefined) {
    "use strict";

    var resources = [],
        HybridCloudExtensionActivationInit,
        navigation;   

    function clearCommandBar() {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Global.clear();
        Exp.UI.Commands.update();
    }

    function onApplicationStart() {
        Exp.UserSettings.getGlobalUserSetting("Admin-skipQuickStart").then(function (results) {
            var setting = results ? results[0] : null;
            if (setting && setting.Value) {
                global.HybridCloudAdminExtension.settings.skipQuickStart = JSON.parse(setting.Value);
            }
        });
                
        global.HybridCloudAdminExtension.settings.skipQuickStart = false;
    }


    function loadAzureSettingsTab(extension, renderArea, renderData) {
      global.HybridCloudAdminExtension.AzureSettingsTab.loadTab(renderData, renderArea);
    }

    function loadRackspaceSettingsTab(extension, renderArea, renderData) {
      global.HybridCloudAdminExtension.RackspaceSettingsTab.loadTab(renderData, renderArea);
    }

    global.HybridCloudExtension = global.HybridCloudAdminExtension || {};

    navigation = {
        tabs: [
                {
                    id: "azureSettings",
                    displayName: "Azure settings",
                    template: "azureSettingsTab",
                    activated: loadAzureSettingsTab
                },
                {
                  id: "rackspaceSettings",
                    displayName: "Rackspace settings",
                    template: "rackspaceSettingsTab",
                    activated: loadRackspaceSettingsTab
                }
        ],
        types: [
        ]
    };

    HybridCloudExtensionActivationInit = function () {
        var HybridCloudExtension = $.extend(this, global.HybridCloudAdminExtension);

        $.extend(HybridCloudExtension, {
            displayName: "Hybrid Cloud",
            viewModelUris: [
                global.HybridCloudAdminExtension.Controller.adminSettingsUrl,
                global.HybridCloudAdminExtension.Controller.adminProductsUrl,
            ],
            menuItems: [],
            settings: {
                skipQuickStart: true
            },
            getResources: function () {
                return resources;
            }
        });

        HybridCloudExtension.onApplicationStart = onApplicationStart;        
        HybridCloudExtension.setCommands = clearCommandBar();

        Shell.UI.Pivots.registerExtension(HybridCloudExtension, function () {
            Exp.Navigation.initializePivots(this, navigation);
        });

        // Finally activate HybridCloudExtension 
        $.extend(global.HybridCloudAdminExtension, Shell.Extensions.activate(HybridCloudExtension));
    };

    Shell.Namespace.define("HybridCloudAdminExtension", {
        init: HybridCloudExtensionActivationInit
    });

})(this, jQuery, Shell, Exp);