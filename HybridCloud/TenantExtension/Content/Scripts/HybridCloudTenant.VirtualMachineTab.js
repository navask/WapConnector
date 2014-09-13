/// <reference path="HybridCloudtenant.createwizard.js" />
/// <reference path="HybridCloudtenant.controller.js" />
/*globals window,jQuery,HybridCloudTenantExtension,Exp,waz,cdm*/
(function ($, global, undefined) {
    "use strict";

    var grid,
        selectedRow,
        statusIcons = {
            Registered: {
                text: "Registered",
                iconName: "complete"
            },
            Default: {
                iconName: "spinner"
            }
        };

    function dateFormatter(value) {
        try {
            if (value) {
                return $.datepicker.formatDate("m/d/yy", value);
            }
        }
        catch (err) { }  // Display "-" if the date is in an unrecoginzed format.

        return "-";
    }

    function onRowSelected(row) {
        if (row) {
            selectedRow = row;
            updateContextualCommands(row);
        }
    }

    function updateContextualCommands(domain) {
        Exp.UI.Commands.Contextual.clear();
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("showDnsManager", "Connect", Exp.UI.CommandIconDescriptor.getWellKnown("browse"), true, null, onShowDnsManager));
        Exp.UI.Commands.Contextual.add(new Exp.UI.Command("viewDomainInfo", "Info", Exp.UI.CommandIconDescriptor.getWellKnown("viewinfo"), true, null, onViewInfo));
        Exp.UI.Commands.update();
    }

    // Command handlers
    function onShowDnsManager() {
        var userInfo = global.DomainTenantExtension.Controller.getCurrentUserInfo(),
            portalUrl;

        if (!userInfo.GoDaddyShopperPasswordChanged) {
            changePassword(userInfo);
        } else {
            portalUrl = global.DomainTenantExtension.Controller.getCurrentUserInfo().GoDaddyCustomerPortalUrl;
            window.open(portalUrl, "_blank");
        }
    }

    function onViewInfo(item) {
        cdm.stepWizard({
            extension: "DomainTenantExtension",
            steps: [
                {
                    template: "viewInfo",
                    contactInfo: global.DomainTenantExtension.Controller.getCurrentUserInfo(),
                    domain: selectedRow
                }
            ]
        },
        { size: "mediumplus" });
    }

    function changePassword(currentUserInfo) {
        var promise,
            wizardContainerSelector = ".dm-selectPassword";

        cdm.stepWizard({
            extension: "DomainTenantExtension",
            steps: [
                {
                    template: "selectPassword",
                    data: {
                        customerId: currentUserInfo.GoDaddyShopperId
                    },
                    onStepActivate: function () {
                        Shell.UI.Validation.setValidationContainer(wizardContainerSelector);
                    }
                }
            ],

            onComplete: function () {
                if (!Shell.UI.Validation.validateContainer(wizardContainerSelector)) {
                    return false;
                }

                currentUserInfo.GoDaddyShopperPassword = $("#dm-password").val();
                currentUserInfo.GoDaddyShopperPasswordChanged = true;
                promise = global.DomainTenantExtension.Controller.updateUserInfo(currentUserInfo);

                global.waz.interaction.showProgress(
                    promise,
                    {
                        initialText: "Reseting password...",
                        successText: "Successfully reset the password.",
                        failureText: "Failed to reset the password."
                    }
                );

                promise.done(function () {
                    global.DomainTenantExtension.Controller.invalidateUserInfoCache();
                    var portalUrl = global.DomainTenantExtension.Controller.getCurrentUserInfo().GoDaddyCustomerPortalUrl;
                    window.open(portalUrl, "_blank");
                });
            }
        },
        { size: "small" });
    }

    function openQuickCreate() {
        Exp.Drawer.openMenu("AccountsAdminMenuItem/CreateFileShare");
    }

    // Public
    function loadTab(extension, renderArea, initData) {
        var content = '<li style="display: list-item;"><div aria-disabled="true" tabindex="-1" role="button" class="fxs-command fx-button fxs-command-action-connect fxs-fxsCommand-disabled ui-state-disabled fx-button-disabled"><div class="fxs-command-iconwrapper"><div class="fxs-command-icon"><img src="//az213233.vo.msecnd.net/Content/4.9.00298.6.140825-1103/Images/icons-action-32-sprites.png" data-bind="attr: { src: data.url }" alt=""></div></div><div class="fxs-command-text" data-bind="text: data.text">Connect</div><!-- ko if: data.subCommands().length --><!-- /ko --></div></li><li style="display: list-item;"><div aria-disabled="false" tabindex="0" role="button" class="fxs-command fx-button fxs-command-action-play"><div class="fxs-command-iconwrapper"><div class="fxs-command-icon"><img src="//az213233.vo.msecnd.net/Content/4.9.00298.6.140825-1103/Images/icons-action-32-sprites.png" data-bind="attr: { src: data.url }" alt=""></div></div><div class="fxs-command-text" data-bind="text: data.text">Start</div><!-- ko if: data.subCommands().length --><!-- /ko --></div></li><li style="display: list-item;"><div aria-disabled="true" tabindex="-1" role="button" class="fxs-command fx-button fxs-command-action-power fxs-fxsCommand-disabled ui-state-disabled fx-button-disabled"><div class="fxs-command-iconwrapper"><div class="fxs-command-icon"><img src="//az213233.vo.msecnd.net/Content/4.9.00298.6.140825-1103/Images/icons-action-32-sprites.png" data-bind="attr: { src: data.url }" alt=""></div></div><div class="fxs-command-text" data-bind="text: data.text">Shut down</div><!-- ko if: data.subCommands().length --><!-- /ko --></div></li><li style="display: list-item; display: none;"><div aria-disabled="false" tabindex="0" role="button" class="fxs-command fx-button fxs-command-action-link"><div class="fxs-command-iconwrapper"><div class="fxs-command-icon"><img src="//az213233.vo.msecnd.net/Content/4.9.00298.6.140825-1103/Images/icons-action-32-sprites.png" data-bind="attr: { src: data.url }" alt=""></div></div><div class="fxs-command-text" data-bind="text: data.text">Attach</div><!-- ko if: data.subCommands().length --><ul class="fxs-command-flyout" data-bind="foreach: data.subCommands "><li><a href="#" data-bind="attr: { &quot;href&quot;: disabled() ? false : &quot;#&quot; }, text: text">Attach disk</a></li><li><a href="#" data-bind="attr: { &quot;href&quot;: disabled() ? false : &quot;#&quot; }, text: text">Attach empty disk</a></li></ul><!-- /ko --></div></li><li style="display: list-item; display:none;"><div aria-disabled="true" tabindex="-1" role="button" class="fxs-command fx-button fxs-command-action-unlink fxs-fxsCommand-disabled ui-state-disabled fx-button-disabled"><div class="fxs-command-iconwrapper"><div class="fxs-command-icon"><img src="//az213233.vo.msecnd.net/Content/4.9.00298.6.140825-1103/Images/icons-action-32-sprites.png" data-bind="attr: { src: data.url }" alt=""></div></div><div class="fxs-command-text" data-bind="text: data.text">Detach disk</div><!-- ko if: data.subCommands().length --><!-- /ko --></div></li><li style="display: list-item; display:none"><div aria-disabled="false" tabindex="0" role="button" class="fxs-command fx-button fxs-command-action-capture"><div class="fxs-command-iconwrapper"><div class="fxs-command-icon"><img src="//az213233.vo.msecnd.net/Content/4.9.00298.6.140825-1103/Images/icons-action-32-sprites.png" data-bind="attr: { src: data.url }" alt=""></div></div><div class="fxs-command-text" data-bind="text: data.text">Capture</div><!-- ko if: data.subCommands().length --><!-- /ko --></div></li><li style="display: list-item;"><div aria-disabled="false" tabindex="0" role="button" class="fxs-command fx-button fxs-command-action-delete"><div class="fxs-command-iconwrapper"><div class="fxs-command-icon"><img src="//az213233.vo.msecnd.net/Content/4.9.00298.6.140825-1103/Images/icons-action-32-sprites.png" data-bind="attr: { src: data.url }" alt=""></div></div><div class="fxs-command-text" data-bind="text: data.text">Delete</div><!-- ko if: data.subCommands().length --><ul class="fxs-command-flyout" data-bind="foreach: data.subCommands "><li><a href="#" data-bind="attr: { &quot;href&quot;: disabled() ? false : &quot;#&quot; }, text: text">Delete the attached disks</a></li><li><a href="#" data-bind="attr: { &quot;href&quot;: disabled() ? false : &quot;#&quot; }, text: text">Keep the attached disks</a></li></ul><!-- /ko --></div></li>';
        $('.fxs-drawercommands-commands-global').html(content);

        var subs = Exp.Rdfe.getSubscriptionList(),
           subscriptionRegisteredToService = global.Exp.Rdfe.getSubscriptionsRegisteredToService("HybridCloud"),
        localDataSet = {            
            dataSetName: global.HybridCloudTenantExtension.Controller.listVirtualMachineUrl,
            ajaxData: { 
                subscriptionIds: subscriptionRegisteredToService[0].id
                },
            url: global.HybridCloudTenantExtension.Controller.listVirtualMachineUrl
        },
        columns = [
                { name: "Name", field: "VMName", sortable: false },
                { name: "Status", field: "Status", filterable: false, sortable: false },
                { name: "subscription", field: "Subscription", filterable: false, sortable: false },
                { name: "Location", field: "Location", filterable: false, sortable: false },
                { name: "DNS Name", field: "DNSName", filterable: false, sortable: false },
            ];

        renderArea.html("<div class=dm-main><div class=gridContainer /><div id=hs-empty class=hs-environment></div></div>");

        grid = renderArea.find(".gridContainer")
            .wazObservableGrid("destroy")
            .wazObservableGrid({
                lastSelectedRow: null,
                data: localDataSet,
                keyField: "VMName",
                columns: columns,
                gridOptions: {
                    rowSelect: onRowSelected
                },
                emptyListOptions: {
                    extensionName: "HybridCloudTenantExtension",
                    templateName: "virtualMachineTabEmpty"
                }
            });
    }
    
    function cleanUp() {
        if (grid) {
            grid.wazObservableGrid("destroy");
            grid = null;
        }
    }

    global.HybridCloudTenantExtension = global.HybridCloudTenantExtension || {};
    global.HybridCloudTenantExtension.VirtualMachineTab = {
        loadTab: loadTab,
        cleanUp: cleanUp,
        statusIcons: statusIcons
    };
})(jQuery, this);
