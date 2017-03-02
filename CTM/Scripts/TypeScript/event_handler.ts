function modalOpen() {
    // Solve: Multiple modal open focus lost problem
    // Reference: https://github.com/nakupanda/bootstrap3-dialog/issues/70
    let modals = $(".modal").filter(()=> {
        return $(this).attr("id") !== "full_size_modal";// Return non-fullscreen modals
    });
    modals.on("hidden.bs.modal", () => {
        $("body").addClass("modal-open");// If closed,add modal-open to body,so focus is back on modal
    });
    $("#full_size_modal").on("hidden.bs.modal", () => {
        $("body").removeClass("modal-open");
    });
}

function fullModalOpenAndClose() {
    // Full modal open and close event handler
    // Add alert and progress bar to full modal and remove
    $("#full_size_modal")
        .on("show.bs.modal",
        function () {
            console.log("show.bs.modal");
            $("#alert").detach().insertBefore($(this).find(".modal-header"));
            $("#loader").detach().insertAfter($(this).find(".modal-header"));
        })
        .on("hidden.bs.modal", () => {
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
    $(".btn-yes").on("click",
        function () {
            var dataUrl = $(this).data("url");
            var dataRowid = $(this).data("rowid");
            $.ajax({
                type: "GET",
                url: dataUrl
            }).done(data => {

                // Del the row if qurested

                console.log(dataRowid);
                if (dataRowid !== "undefined") {
                    // If row id is defined,delete the row
                    console.log($("#" + dataRowid));
                    $("#" + dataRowid).css("display", "none");
                }

            }).fail(() => {
                console.log("ajax failed");
            });
        });
}

function checkBoxClick() {
    // Islatest checkbox
    $(".material-switch > label").on("click",
        function () {
            var value = $(this).prev().attr("value");
            var hidableDivId = $(this).prev().data("hidabledivid");
            console.log(hidableDivId);
            if (value !== "true") {
                $(this).prev().attr("value", "true");
                $("#" + hidableDivId).hide();
            } else {
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



