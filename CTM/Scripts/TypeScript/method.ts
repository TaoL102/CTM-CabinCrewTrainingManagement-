/// <reference path="sitevariable.ts"/>

// ShowAlert
function showAlert(htmlContent: string, type: string) {
    if (type === "danger") {
        $("#alert").css("background-color", "#f44336").show().delay(3000).fadeOut(1000);
    } else {
        $("#alert").css("background-color", "#7cb342").show().delay(3000).fadeOut(1000);
    }
    // Insert html
    $("#alert").html(htmlContent);

};

// Open Modal
function openModal(modalId: string, isRegisterPlugins: boolean = false) {
    modalId = "#" + modalId;
    // Open
    $(modalId).modal();

    if (isRegisterPlugins) {
        // Register the form in modal to unobtrusive js, so that local validation would not fail
        $.validator.unobtrusive.parse(modalId + " form");

        // Register Plugins
        registerPlugins(modalId);
    }
};

// Hide element
function hideElement(elementId: string) {
    elementId = "#" + elementId;
    $(elementId).hide();
};

function editElement(controllerName: string, elementId: string,url:string) {

    $.ajax({
        url: url,
        data: {
            controllerName: controllerName ,
            id:elementId
        },
        dataType: "json",
        global: false,
        success: (data: any, status: any, xhr: any) => {
            var tds = $("#" + elementId + " " + "td[name]");
            for (var i = 0; i < tds.length; i++) {
                var tdName = $(tds[i]).attr("name");
                $(tds[i]).html(data[tdName]);
            }
        },

    });

    

};


