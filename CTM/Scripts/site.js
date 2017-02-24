// #Region Global settings
// Register JS plugins 
registerPlugins();
// #Region Global settings



// Methods
//  Register JS plugins 
function registerPlugins() {
    // SELECT
    $('select').material_select({

    });

    // DATEPICKER
    $('.datepicker').pickadate({
        selectMonths: true, // Creates a dropdown to control month
        selectYears: 15, // Creates a dropdown of 15 years to control year
    });
}

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
};


