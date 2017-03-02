function modalOpen() {
    var _this = this;
    // Solve: Multiple modal open focus lost problem
    // Reference: https://github.com/nakupanda/bootstrap3-dialog/issues/70
    var modals = $(".modal").filter(function () {
        return $(_this).attr("id") !== "full_size_modal"; // Return non-fullscreen modals
    });
    modals.on("hidden.bs.modal", function () {
        $("body").addClass("modal-open"); // If closed,add modal-open to body,so focus is back on modal
    });
    $("#full_size_modal").on("hidden.bs.modal", function () {
        $("body").removeClass("modal-open");
    });
}
function fullModalOpenAndClose() {
    // Full modal open and close event handler
    // Add alert and progress bar to full modal and remove
    $("#full_size_modal")
        .on("show.bs.modal", function () {
        console.log("show.bs.modal");
        $("#alert").detach().insertBefore($(this).find(".modal-header"));
        $("#loader").detach().insertAfter($(this).find(".modal-header"));
    })
        .on("hidden.bs.modal", function () {
        $("#alert").detach().prependTo($("nav"));
        $("#loader").detach().appendTo($("nav"));
    });
}
function msgModalOpen() {
    $('#message_box_modal').on("show.bs.modal", function (e) {
        var $invoker = $(e.relatedTarget);
        console.log($invoker);
        $('#message_box_modal').find('.btn-yes').attr('data-url', $invoker.data('url')).attr('data-rowid', $invoker.data('rowid'));
    });
}
function msgModalYesBtnClick() {
    // Modal del confirm button click event
    $(".btn-yes").on("click", function () {
        var dataUrl = $(this).data("url");
        var dataRowid = $(this).data("rowid");
        $.ajax({
            type: "GET",
            url: dataUrl
        }).done(function (data) {
            // Del the row if qurested
            console.log(dataRowid);
            if (dataRowid !== "undefined") {
                // If row id is defined,delete the row
                console.log($("#" + dataRowid));
                $("#" + dataRowid).css("display", "none");
            }
        }).fail(function () {
            console.log("ajax failed");
        });
    });
}
function checkBoxClick() {
    // Islatest checkbox
    $(".material-switch > label").on("click", function () {
        var value = $(this).prev().attr("value");
        var hidableDivId = $(this).prev().data("hidabledivid");
        console.log(hidableDivId);
        if (value !== "true") {
            $(this).prev().attr("value", "true");
            $("#" + hidableDivId).hide();
        }
        else {
            $(this).prev().attr("value", "false");
            $("#" + hidableDivId).show();
        }
    });
}
function checkBoxHidableDivHide() {
    // Hidable divs
    var ids = $("input[type='checkbox'][value='true'][data-hidabledivid]").data("hidabledivid");
    $("#" + ids).hide();
}
// Set global variables
var ConstantHelper = (function () {
    function ConstantHelper() {
    }
    return ConstantHelper;
}());
ConstantHelper.CssColorFontPrimary = "font-primary";
/// <reference path="sitevariable.ts"/>
// ShowAlert
function showAlert(htmlContent, type) {
    if (type === "danger") {
        $("#alert").removeClass().addClass("alert alert-warning");
    }
    // Insert html
    $("#alert").html(htmlContent);
    $("#alert").fadeIn();
}
;
// Open Modal
function openModal(modalId, isRegisterPlugins) {
    if (isRegisterPlugins === void 0) { isRegisterPlugins = false; }
    modalId = "#" + modalId;
    // Open
    $(modalId).modal();
    if (isRegisterPlugins) {
        // Register the form in modal to unobtrusive js, so that local validation would not fail
        $.validator.unobtrusive.parse(modalId + " form");
        // Register Plugins
        registerPlugins();
    }
}
;
// Close Modal
function closeModal(modalId) {
    modalId = "#" + modalId;
    // Open
    $(modalId).modal("hide");
}
;
/// <reference path="sitevariable.ts"/>
function registerPlugins() {
    // DATEPICKER
    $(".datepicker").datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        autoclose: true,
    });
    // Autocomplete
    registerAutoComplete("CCName", true);
}
function registerAjaxGlobalSettings() {
    // AJAX Global Settings
    $(document).ajaxStart(function () {
        $("#loader").show();
    });
    $(document).ajaxStop(function () {
        $("#loader").hide();
    });
    $(document).ajaxError(function (event, jqxhr, settings) {
        console.log("ajaxError" + jqxhr);
        console.log("ajaxError" + settings);
        // If server effor:
        if (jqxhr.status === 500) {
            //showAlert(jqxhr.responseText, "danger");
            showAlert(jqxhr.statusText, "danger");
        }
        else {
            showAlert(jqxhr.statusText, "danger");
        }
    });
}
function registerAutoComplete(id, allowMultipleValues) {
    var _this = this;
    if (allowMultipleValues === void 0) { allowMultipleValues = false; }
    // Autocomplete
    // Reference: https://jqueryui.com/autocomplete/
    var curElement = $("#" + id);
    var cache = {};
    function split(val) {
        return val.split(/,\s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }
    curElement
        .on("keydown", function (event) {
        if (event.keyCode === $.ui.keyCode.TAB &&
            $(_this).autocomplete("instance").menu.active) {
            event.preventDefault();
        }
    });
    curElement
        .autocomplete({
        delay: 100,
        minLength: 1,
        focus: function () { return false; },
        select: function (event, ui) {
            console.log(ui.item.value);
            var curInputVal = curElement.val();
            var curSeletedVal = ui.item.value;
            if (!allowMultipleValues) {
                curElement.val(curSeletedVal);
                return false;
            }
            var term = split(curInputVal);
            // remove the current input
            term.pop();
            // add the selected item
            term.push(ui.item.value);
            // add placeholder to get the comma-and-space at the end
            term.push("");
            curElement.val(term.join(", "));
            return false;
        },
        search: function (event, ui) {
            // custom minLength
            console.log(curElement.val());
            var curInputVal = curElement.val();
            var term = allowMultipleValues ? extractLast(curInputVal) : curInputVal;
            console.log(term);
            if (term.match(/[\u4E00-\u9FFF\u3400-\u4DFF\uF900-\uFAFF]+/g)) {
                return true;
            }
            event.preventDefault();
            return false;
        },
        source: function (request, response) {
            console.log(cache);
            var query = extractLast(request.term);
            // Search cache first
            if (cache.hasOwnProperty(query)) {
                response(cache[query]);
                return;
            }
            // Then ajax
            $.ajax({
                url: curElement.data("url"),
                data: { name: query },
                dataType: "json",
                success: function (data, status, xhr) {
                    cache[query] = data;
                    response(data);
                },
                error: function () {
                    response([]);
                }
            });
        }
    });
    curElement
        .autocomplete("option", "classes.ui-autocomplete", "form-autocompelete-menu");
}
/// <reference path="method.ts"/>
/// <reference path="register.ts"/>
/// <reference path="event_handler.ts"/>
// Register 
registerPlugins();
registerAjaxGlobalSettings();
// Bind event handlers
modalOpen();
fullModalOpenAndClose();
msgModalOpen();
msgModalYesBtnClick();
checkBoxClick();
checkBoxHidableDivHide();
// Client Validation
// Client Validation
function registerIsCabinCrewValidation(url) {
    //Reference：http://stackoverflow.com/questions/7247250/jquery-validation-not-waiting-for-remote-validation-to-return-true-considers-fo
    $.validator.addMethod("iscabincrew", function (value, element, param) {
        var _this = this;
        if (this.optional(element)) {
            return "dependency-mismatch";
        }
        if (!value.match(/[\u4E00-\u9FFF\u3400-\u4DFF\uF900-\uFAFF]+/g)) {
            return false;
        }
        var previous = this.previousValue(element);
        var elementName = element.id;
        if (!this.settings.messages[elementName]) {
            this.settings.messages[elementName] = {};
        }
        previous.originalMessage = this.settings.messages[elementName].remote;
        this.settings.messages[elementName].remote = previous.message;
        param = typeof param === "string" && { url: param } || param;
        if (previous.old === value) {
            return previous.valid;
        }
        previous.old = value;
        this.startRequest(element);
        var data = {};
        data["names"] = value;
        var valid = false;
        $.ajax($.extend(true, {
            url: url,
            async: false,
            mode: "abort",
            port: "validate" + elementName,
            data: data,
            success: function (response) {
                _this.settings.messages[elementName].remote = previous.originalMessage;
                valid = response === true || response === "True";
                if (valid) {
                    var submitted = _this.formSubmitted;
                    _this.prepareElement(element);
                    _this.formSubmitted = submitted;
                    _this.successList.push(element);
                    delete _this.invalid[elementName];
                    _this.showErrors();
                }
                else {
                    var errors = {};
                    var message = response || _this.defaultMessage(element, "remote");
                    errors[elementName] = previous.message = $.isFunction(message) ? message(value) : message;
                    _this.invalid[elementName] = true;
                    _this.showErrors(errors);
                }
                previous.valid = valid;
                _this.stopRequest(element, valid);
            }
        }, param));
        return valid;
    });
    $.validator.unobtrusive.adapters.add("iscabincrew", [], function (options) {
        options.rules["iscabincrew"] = true;
        options.messages["iscabincrew"] = options.message;
    });
}
//# sourceMappingURL=site.js.map