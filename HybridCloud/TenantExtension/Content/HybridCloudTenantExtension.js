/// <reference path="../../../Scripts/_references.js" />
/// <reference path="../../../Scripts/auxfx/api/Shell.UI.js" />
/// <reference path="../../../Scripts/auxfx/api/Shell.UI.Validation.js" />

/// <reference path="scripts/HybridCloudTenant.createwizard.js" />
/// <reference path="scripts/HybridCloudTenant.controller.js" />
/*globals window,jQuery,Shell, HybridCloudTenantExtension, Exp*/

(function ($, global, undefined) {
    "use strict";

    var resources = [],
        HybridCloudTenantExtensionActivationInit,
        navigation,
        serviceName = "HybridCloud";

    function onNavigateAway() {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Global.clear();
        Exp.UI.Commands.update();
    }

    function loadSettingsTab(extension, renderArea, renderData) {
        global.HybridCloudTenantExtension.SettingsTab.loadTab(renderData, renderArea);
    }

    function virtualMachineTab(extension, renderArea, renderData) {
        global.HybridCloudTenantExtension.VirtualMachineTab.loadTab(renderData, renderArea);
    }

    global.HybridCloudTenantExtension = global.HybridCloudTenantExtension || {};

    navigation = {
        tabs: [
            {
                id: "virtualMachines",
                displayName: "Virtual Machines",
                template: "virtualMachineTab",
                activated: virtualMachineTab
            }            
        ],
        types: [
        ]
    };

    HybridCloudTenantExtensionActivationInit = function () {
        var subs = Exp.Rdfe.getSubscriptionList(),
            subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService("HybridCloud"),
            HybridCloudExtension = $.extend(this, global.HybridCloudTenantExtension);

        // Don't activate the extension if user doesn't have a plan that includes the service.
        if (subscriptionRegisteredToService.length === 0) {
            return false; // Don't want to activate? Just bail
        }

        $.extend(HybridCloudExtension, {
            viewModelUris: [HybridCloudExtension.Controller.userInfoUrl],
            displayName: "Hybrid Cloud",
            navigationalViewModelUri: {
                uri: HybridCloudExtension.Controller.listVirtualMachineUrl,
                ajaxData: function () {
                    return global.Exp.Rdfe.getSubscriptionIdsRegisteredToService(serviceName);
                }
            },
            displayStatus: global.waz.interaction.statusIconHelper(global.HybridCloudTenantExtension.VirtualMachineTab.statusIcons, "Status"),
            menuItems: [
                {
                    name: "VirtualMachines",
                    displayName: "Hybrid Cloud",
                    url: "#Workspaces/HybridCloudTenantExtension",
                    preview: "createPreview",
                    subMenu: [
                        {
                            name: "Create",
                            displayName: "Create Virtual Machine",
                            description: "Quickly create virtual machine on Azure",
                            template: "CreateVirtualMachine",
                            label: "Create",
                            subMenu: [
                                {
                                    name: "QuickCreate",
                                    displayName: "VirtualMachines",
                                    template: "CreateFileShare"
                                }
                            ],
                            ok: function (context) {
                                //do your work here
                                return false;
                            }
                        }
                    ]
                    
                }
            ],
            getResources: function () {
                return resources;
            }
        });

        HybridCloudExtension.onNavigateAway = onNavigateAway;
        HybridCloudExtension.navigation = navigation;

        Shell.UI.Pivots.registerExtension(HybridCloudExtension, function () {
            Exp.Navigation.initializePivots(this, this.navigation);
        });

        // Finally activate and give "the" HybridCloudExtension the activated extension since a good bit of code depends on it
        $.extend(global.HybridCloudTenantExtension, Shell.Extensions.activate(HybridCloudExtension));
    };

    function getQuickCreateFileShareMenuItem() {
        return {
            name: "QuickCreate",
            displayName: "Create File Share",
            description: "Create new file share",
            template: "quickCreateWithRdfe",
            label: resources.CreateMenuItem,

            opening: function () {
                AccountsAdminExtension.AccountsTab.renderListOfHostingOffers(offersListSelector);
            },

            open: function () {
                // Enables As-You-Type validation experience on a container specified
                Shell.UI.Validation.setValidationContainer(valContainerSelector);
                // Enables password complexity feedback experience on a container specified
                Shell.UI.PasswordComplexity.parse(valContainerSelector);
            },

            ok: function (object) {
                var dialogFields = object.fields,
                    isValid = validateAccount();

                if (isValid) {
                    createAccountWithRdfeCore(dialogFields);
                }
                return false;
            },

            cancel: function (dialogFields) {
                // you can return false to cancel the closing
            }
        };
    }

    Shell.Namespace.define("HybridCloudTenantExtension", {
        serviceName: serviceName,
        init: HybridCloudTenantExtensionActivationInit,
        getQuickCreateFileShareMenuItem: getQuickCreateFileShareMenuItem
    });
})(jQuery, this);
