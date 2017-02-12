// ShowAlert
function showAlert(htmlContent, type) {
    if (type === "danger") {
        $("#alert").attr("class", "background-danger");
    }
    // Insert html
    $("#alert").html(htmlContent);
    $("#alert").fadeIn();
}
;
//# sourceMappingURL=sitemethod.js.map