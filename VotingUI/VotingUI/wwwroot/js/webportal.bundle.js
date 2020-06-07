/**
 * @namespace WEBPORTAL
 * 
 * */
var WEBPORTAL = WEBPORTAL || {};

/**
 * @module Version
 * 
 * */
WEBPORTAL.Version = '1.0.0';

/**
 * @module Register
 * 
 * */
WEBPORTAL.Register = {};

/**
 * @module Global
 * 
 * */
WEBPORTAL.Global = {};
WEBPORTAL.Global.SetDropdownCaret = $('.select-wrapper.md-form.md-outline span.caret').css('z-index', '3');

/**
 * @module UserType
 * 
 * */
WEBPORTAL.UserType = {};
WEBPORTAL.UserType.Vendor = "V";

/**
 * @module APIEndpoint
 * 
 * */
WEBPORTAL.ApiEndpoint = {};

/**
 * @module URLContext
 * 
 * */
WEBPORTAL.URLContext = {};
WEBPORTAL.URLContext.GetVotings = '/Voting/GetVotings';
WEBPORTAL.URLContext.InsertVoting = '/Voting/InsertVoting';
WEBPORTAL.URLContext.UpdateVoting = '/Voting/UpdateVoting';
WEBPORTAL.URLContext.DeleteVoting = '/Voting/DeleteVoting';
WEBPORTAL.URLContext.GetCategories = '/Category/GetCategories';
WEBPORTAL.URLContext.InsertCategory = '/Category/InsertCategory';
WEBPORTAL.URLContext.UpdateCategory = '/Category/UpdateCategory';
WEBPORTAL.URLContext.DeleteCategory = '/Category/DeleteCategory';
WEBPORTAL.URLContext.SubmitVote = '/Home/SubmitVote';

WEBPORTAL.GlobalMessage = {
    SubmitError: "Submit Error. Please check your form"
}

WEBPORTAL.VendorStatus = {
    Approved: 'APPROVED',
    Rejected: 'REJECTED',
    Disabled: 'DISABLED'
};

/**
 * @module Services
 * 
 */
WEBPORTAL.Services = (function () {
    function get(urlContext) {
        var deferred = $.Deferred();

        $.ajax({
            type: "GET",
            url: WEBPORTAL.ApiEndpoint.BaseUrl + urlContext,
            success: function (result, status, xhr) {            
                deferred.resolve(result);
            },
            error: function (result, status, xhr) {
                deferred.reject(result);
            }
        });

        return deferred.promise();
    };

    function getLocal(urlContext) {
        var deferred = $.Deferred();

        $.ajax({
            type: "GET",
            url: urlContext,
            success: function (result, status, xhr) {
                deferred.resolve(result);
            },
            error: function (result, status, xhr) {
                deferred.reject(result);
            }
        });

        return deferred.promise();
    };

    function postLocal(data, urlContext, contentType, processData) {
        var deferred = $.Deferred();

        $.ajax({
            type: "POST",
            data: data,
            url: urlContext,
            contentType: contentType !== 'undefined' ? contentType : 'application/x-www-form-urlencoded; charset=UTF-8',
            processData: processData !== 'undefined' ? processData : true,
            success: function (result, status, xhr) {
                deferred.resolve(result);
            },
            error: function (result, status, xhr) {
                deferred.reject(result);
            },
            timeout: 60000
        });

        return deferred.promise();
    }

    function putLocal(data, urlContext, queryString) {
        var deferred = $.Deferred();

        $.ajax({
            type: "PUT",
            data: data,
            url: urlContext + queryString,
            success: function (result, status, xhr) {
                deferred.resolve(result);
            },
            error: function (result, status, xhr) {
                deferred.reject(result);
            },
            timeout: 60000
        });

        return deferred.promise();
    }

    function deleteLocal(data, urlContext, queryString) {
        var deferred = $.Deferred();

        $.ajax({
            type: "DELETE",
            data: data,
            url: urlContext + queryString,
            success: function (result, status, xhr) {
                deferred.resolve(result);
            },
            error: function (result, status, xhr) {
                deferred.reject(result);
            },
            timeout: 60000
        });

        return deferred.promise();
    }

    function downloadFile(fileName) {
        var deferred = $.Deferred();
        console.log("Test");
        console.log(WEBPORTAL.URLContext.DownloadFile + '?fileName=' + fileName);

        $.ajax({
            url: WEBPORTAL.URLContext.DownloadFile + '?fileName=' + fileName,
            method: 'GET',
            success: function (result, status, xhr) {
                var base64str = result.data;

                // decode base64 string, remove space for IE compatibilitys
                var binary = atob(base64str.replace(/\s/g, ''));
                var len = binary.length;
                var buffer = new ArrayBuffer(len);
                var view = new Uint8Array(buffer);
                for (var i = 0; i < len; i++) {
                    view[i] = binary.charCodeAt(i);
                }

                // create the blob object with content-type "application/pdf"               
                var blob = new Blob([view], { type: "application/pdf" });
                var url = URL.createObjectURL(blob);

                deferred.resolve(url);
            },

            error: function (result, status, xhr) {
                deferred.reject(result);
            },
            timeout: 60000
        });

        return deferred.promise();
    }

    return {
        GET: get,
        GETLocal: getLocal,
        POSTLocal: postLocal,
        PUTLocal: putLocal,
        DELETELocal: deleteLocal,
        DownloadFile: downloadFile
    };
})();

/**
 * @module Utility
 * 
 * */
WEBPORTAL.Utility = (function () {
    function showLoading(obj) {
        obj.append('<span class="loading-animation"><i class="fa fa-circle-o-notch fa-spin fa-3x fa-fw"></i></span>');
    };
    function removeLoading(obj) {
        obj.remove();
    };
    function constructDropdownOptions(elem, data) {
        var option = '<option value="" disabled selected>Select</option>';

        $.each(data, function (key, value) {
            option += "<option value='" + value.id + "'>" + value.name + "</option>";
        });

        elem.html(option);
    };
    function constructDropdownGroupOptions(elem, data) {
        var tempOption = "";

        $.each(data, function (key, value) {
            var optGroup = "<optgroup label='" + value.desc + "'>";

            for (var i = 0; i < value.businessFields.length; i++) {
                optGroup += "<option value='" + value.businessFields[i].id + "'>" + value.businessFields[i].kbli + " " + value.businessFields[i].name + "</option>";
            }

            optGroup += "</optGroup>";
            tempOption += optGroup;         
        });
        elem.html(tempOption);
    };
    function checkEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;

        return regex.test(email);
    };
    function submitLoading(elem) {
        elem.html('<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>Loading...').addClass('disabled');
    };
    function submitRemoveLoading(elem, title) {
        elem.find('span').remove();
        elem.removeClass('disabled');
        elem.text('Save');
        if (typeof title === 'undefined')
            elem.html('<i class="fa fa-save pr-2"></i><span>Save</span>');
        else
            elem.html('<i class="fa fa-save pr-2"></i><span>' + typeof title === 'undefined' ? 'Save' : title + '</span>');
    };
    function constructUpdateButton(elem) {
        elem.find('span').remove();
        elem.removeClass('disabled');
        elem.text('Update');
        elem.html('<i class="fa fa-edit pr-2"></i> <span>Update</span>');
    };
    function constructNotificationSuccess(msg) {
        toastr.options = {
            preventDuplicates: true,
            preventOpenDuplicates: true
        };
        toastr.success(msg, 'Success', { timeOut: 5000 });
    };
    function constructNotificationError(msg) {
        toastr.options = {
            preventDuplicates: true,
            preventOpenDuplicates: true
        };
        toastr.error(msg, 'Error', { timeOut: 5000 })
    };
    function encryptionHandler(msg) {
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

        var encrypted = CryptoJS.AES.encrypt(
            CryptoJS.enc.Utf8.parse(msg),
            key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });

        return encrypted.toString();
    };
    function convertDateStringToJavascript(date) {
        var format = date.split("/");
        var result = format[2] + "-" + format[1] + "-" + format[0];

        return result;
    };
    return {
        ShowLoading: showLoading,
        RemoveLoading: removeLoading,
        ConstructDropdownOptions: constructDropdownOptions,
        ConstructDropdownGroupOptions: constructDropdownGroupOptions,
        CheckEmail: checkEmail,
        SubmitLoading: submitLoading,
        SubmitRemoveLoading: submitRemoveLoading,
        ConstructUpdateButton: constructUpdateButton,
        ConstructNotificationSuccess: constructNotificationSuccess,
        ConstructNotificationError: constructNotificationError,
        EncryptionHandler: encryptionHandler,
        ConvertDateStringToJavascript: convertDateStringToJavascript
    };
})();

/**
 * @module Cookie
 * */

WEBPORTAL.CookieConfiguration = (function () {
    function setCookie(cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    };

    function getCookie (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    };

    return {
        SetCookie: setCookie,
        GetCookie: getCookie
    }
})();

/**
 * @module jQuery
 * 
 */
jQuery.fn.extend({
    configureMaterialSelect: function () {
        $(this).val("")
            .removeAttr('readonly').attr("placeholder", "Select ").prop('required', true)
            .addClass('form-control').css('background-color', '#fff');
    },
    configureMaterialSelectGroup: function () {
        $(this).val("")
            .removeAttr('readonly').attr("placeholder", "Select Max 5 Bussiness Field ").prop('required', true)
            .addClass('form-control').css('background-color', '#fff');
    },
    removeError: function () {
        $(this).on('change', function () {
            if ($(this).val() != '') {
                $(this).parent().next().next().remove();
            }
        });
    },
    constructUpdateButton: function() {
        $(this).find('span').remove();
        $(this).removeClass('disabled');
        $(this).text('Update');
        $(this).html('<i class="fa fa-edit pr-2"></i> <span>Update</span>');
    },
    constructSaveButton: function () {
        $(this).find('span').remove();
        $(this).removeClass('disabled');
        $(this).text('Save');
        $(this).html('<i class="fa fa-edit pr-2"></i> <span>Save</span>');
    },
    componentAddLoading: function () {
        $(this).html('<div class="d-flex justify-content-center">'
            + '<div class="spinner-grow text-dark" role="status">'
            + '<span class="sr-only">Loading...</span>'
            + '</div>'
            + '</div>');
    },
    settingWrapCenter: function () {
        $(this).addClass('d-flex justify-content-center align-items-center mb-5');
    },
    resetWrapCenter: function () {
        $(this).removeClass('d-flex justify-content-center align-items-center mb-5');
    },
    iframeLoading: function () {
        $(this).html('<div class="spinner-grow text-dark" role="status" style="position: absolute; top: 50%; left: 50%;"><span class="sr-only">Loading...</span></div>');
    },
    titleTooltipOnApproved: function () {
        $(this).find('.vd_status_tooltip').remove('.vd_status_tooltip');
        $(this).append('<i class="fa fa-check-circle text-success pl-1 vd_status_tooltip" aria-hidden="true" data-toggle="tooltip" data-placement="bottom" title="Approved" data-original-title="Approved"></i>');
        $(this).find('.vd_status_tooltip').tooltip();
    },
    titleTooltipOnRejected: function () {
        $(this).find('.vd_status_tooltip').remove('.vd_status_tooltip');
        $(this).append('<i class="fa fa-times-circle text-danger pl-1 vd_status_tooltip" aria-hidden="true" data-toggle="tooltip" data-placement="bottom" title="Reject" data-original-title="Reject"></i>');
        $(this).find('.vd_status_tooltip').tooltip();
    },
    titleTooltipOnDisabled: function () {
        $(this).find('.vd_status_tooltip').remove('.vd_status_tooltip');
        $(this).append('<i class="fa fa-ban text-dark pl-1 vd_status_tooltip" aria-hidden="true" data-toggle="tooltip" data-placement="bottom" title="Disable" data-original-title="Disable"></i>');
        $(this).find('.vd_status_tooltip').tooltip();
    },
    titleTooltipOnPending: function () {
        $(this).find('.vd_status_tooltip').remove('.vd_status_tooltip');
        $(this).append('<i class="fa fa-hourglass-1 text-info pl-1 vd_status_tooltip" aria-hidden="true" data-toggle="tooltip" data-placement="bottom" title="Pending" data-original-title="Pending"></i>');
        $(this).find('.vd_status_tooltip').tooltip();
    },
});




