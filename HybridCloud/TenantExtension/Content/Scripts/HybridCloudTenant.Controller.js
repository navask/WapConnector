/*globals window,jQuery,cdm,HybridCloudTenantExtension,waz,Exp*/

/// <reference path="../../../../Scripts/_references.js" />
/// <reference path="../../../../Scripts/auxfx/api/Shell.UI.js" />
/// <reference path="../../../../Scripts/auxfx/api/Shell.UI.Validation.js" />

(function ($, global, undefined) {
    "use strict";

    var baseUrl = "/HybridCloudTenant",
        listVirtualMachineUrl = baseUrl + "/ListVirtualMachines",
        domainType = "HybridCloud";

    function navigateToListView() {
        Shell.UI.Navigation.navigate("#Workspaces/{0}/HybridCloud".format(HybridCloudTenantExtension.name));
    }

    function getFileShares(subscriptionIds) {
        return makeAjaxCall(listVirtualMachineUrl, { subscriptionIds: subscriptionIds }).data;
    }

    function makeAjaxCall(url, data) {
        return Shell.Net.ajaxPost({
            url: url,
            data: data
        });
    }

    function getLocalPlanDataSet() {
        return Exp.Data.getLocalDataSet(planListUrl);
    }

    function getfileSharesData(subscriptionId) {
        return Exp.Data.getData("fileshare{0}".format(subscriptionId), {
            ajaxData: {
                subscriptionIds: subscriptionId
            },
            url: listVirtualMachineUrl,
            forceCacheRefresh: true
        });
    }

    // TODO: Can we use the waz.dataWrapper in the sample?
    function createFileShare(subscriptionId, fileShareName, size, fileServerName) {
        return new waz.dataWrapper(Exp.Data.getLocalDataSet(listVirtualMachineUrl))
            .add(
            {
                Name: fileShareName,
                SubscriptionId: subscriptionId,
                Size: size,
                FileServerName: fileServerName
            },
            Shell.Net.ajaxPost({
                data: {
                    subscriptionId: subscriptionId,
                    Name: fileShareName,
                    Size: size,
                    FileServerName: fileServerName
                },
                url: baseUrl + "/CreateFileShare"
            })
        );
    }

    global.HybridCloudTenantExtension = global.HybridCloudTenantExtension || {};
    global.HybridCloudTenantExtension.Controller = {
        createFileShare: createFileShare,
        listVirtualMachineUrl: listVirtualMachineUrl,
        getFileShares: getFileShares,
        getLocalPlanDataSet: getLocalPlanDataSet,
        getfileSharesData: getfileSharesData,
        navigateToListView: navigateToListView
    };
})(jQuery, this);

$(document).ready(function () {
    $('.fxs-drawermenu-ok.fx-button').css({ 'right': '200px' });
    $('.drawer-form-item').css({ 'margin-top': '8px !important' });
});

//$(document.body).delegate('#vm-image-wazSelect', 'change', function () {
//    if ($('#vm-image-wazSelect').find(":selected").val() === 'WindowsServer2012Datacenter')
//    {
//        $('#vm-lbl-location').html('East US');
//    }
//    else
//    {
//        $('#vm-lbl-location').html('East Asia');
//    }
//});

$(document.body).delegate('img[alt=Create]', 'click', function () {
    $('#errorVMCreation').css({ 'visibility': 'none' });
    if ($('#vm-name').val() === '') {
        $('#errorVMCreation').html('DNS name field cannot be empty');
        $('#errorVMCreation').css({ 'visibility': 'visible' });
    }
    else if($('#vm-quick-username').val() === '') {
        $('#errorVMCreation').html('UserName field cannot be empty');
        $('#errorVMCreation').css({ 'visibility': 'visible' });
    }
    else if ($('#vm-quickAddPassword').val() === '') {
        $('#errorVMCreation').html('Password field cannot be empty');
        $('#errorVMCreation').css({ 'visibility': 'visible' });
    }
    else if ($('#vm-quickAddPassword').val() !== $('#vm-quickAddConfirmpassword').val()) {
        $('#errorVMCreation').html('Passwords are not matching');
        $('#errorVMCreation').css({ 'visibility': 'visible' });
    }
    else {
        $('#errorVMCreation').css({ 'visibility': 'hidden' });
        $('#errorVMCreation').html('');

        sendAjaxRequestForCreate();
    }
});

function sendAjaxRequestForCreate()
{
    var location = '';
    //if ($('#vm-image-wazSelect').find(":selected").val() === 'WindowsServer2012Datacenter') {
    //    location='East US';
    //}
    //else {
    //    location = 'East Asia';
    //}
    location = $('#vm-quickAddLocation').find(":selected").val();
    $('#vm-loader').css({ 'display': 'block' });
    $.ajax({
        url: "/HybridCloudTenant/CreateVirtualMachine",
        type: "POST", 
        data: {
            vmName: $('#vm-name').val(), location: location, image: $('#vm-image-wazSelect').find(":selected").val(),
            size: $('#vm-quick-size').find(":selected").val(), affinityGroup: '',
            userName: $('#vm-quick-username').val(), password: $('#vm-quickAddPassword').val()
        },
        dataType: "Json"
    }).done(function (msg) {
        $('#vm-loader').css({ 'display': 'none' });
        alert('VM created Successfully');
        window.location.reload();
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $('#vm-loader').css({ 'display': 'none' });
        var responseText = JSON.parse(jqXHR.responseText);
        var error = responseText.stackTrace.substring(0, responseText.stackTrace.indexOf("at Microsoft."));
        error = error.replace('System.Exception: ', '').replace('ResourceNotFound: ', '').replace('BadRequest: ', '').replace('\r\n', '').replace('  ', ' ');
        $('#errorVMCreation').text(error);
        $('#errorVMCreation').css({ 'visibility': 'visible' });
    });
}

$(document.body).delegate('.fxshell-nav1-item.fxshell-nav1-item-legacy', 'click', function () {
    $('.fxs-drawercommands-commands-global').html('');
});

   