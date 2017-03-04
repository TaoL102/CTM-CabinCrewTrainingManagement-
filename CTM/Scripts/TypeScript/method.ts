/// <reference path="sitevariable.ts"/>

// ShowAlert
function showAlert(htmlContent: string, type:string) {
    if (type === "danger") {
        $("#alert").removeClass().addClass("alert alert-warning");
    }
    // Insert html
    $("#alert").html(htmlContent);
    
    $("#alert").fadeIn();
};

// Open Modal
function openModal(modalId:string, isRegisterPlugins:boolean=false) {
    modalId = "#" + modalId;
    // Open
    $(modalId).modal();

    if (isRegisterPlugins) {
        // Register the form in modal to unobtrusive js, so that local validation would not fail
        $.validator.unobtrusive.parse(modalId + " form");

        // Register Plugins
        registerPlugins();
    }
};

// Close Modal
function closeModal(modalId: string) {
    modalId = "#" + modalId;
    // Open
    $(modalId).modal("hide");
};