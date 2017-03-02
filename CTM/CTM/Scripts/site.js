// Set global variables
class ConstantHelper {
}
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
// #Region Global settings
// Register JS plugins 
registerPlugins();
// #Region Global settings
function registerPlugins() {
    var datepickerOptions = {
        showOtherMonths: true,
        selectOtherMonths: true,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        autoclose: true,
    };
    // DATEPICKER
    $(".datepicker").datepicker(datepickerOptions);
}
/// <reference path="sitemethod.ts"/>
/// <reference path="siteregister.ts"/>
//# sourceMappingURL=site.js.map