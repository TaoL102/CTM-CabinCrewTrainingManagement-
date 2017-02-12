// ShowAlert
function showAlert(htmlContent: string, type:string) {
    if (type === "danger") {
        $("#alert").attr("class", "background-danger");
    }

    // Insert html
    $("#alert").html(htmlContent);

    $("#alert").fadeIn();
};