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

// Edit element
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

// Reference: https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript
function generateUUID() { // Public Domain/MIT
    var d = new Date().getTime();
    if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
        d += performance.now(); //use high-precision timer if available
    }
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}

