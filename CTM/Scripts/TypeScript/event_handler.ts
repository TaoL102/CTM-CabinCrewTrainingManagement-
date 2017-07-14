function modalOpen() {
    // Solve: Multiple modal open focus lost problem
    // Reference: https://github.com/nakupanda/bootstrap3-dialog/issues/70
    let modals = $(".modal").filter(()=> {
        return $(this).attr("id") !== ConstantHelper.FullModalId;// Return non-fullscreen modals
    });
    modals.on("hidden.bs.modal", () => {
        $("body").addClass("modal-open");// If closed,add modal-open to body,so focus is back on modal
    });
    $("#"+ConstantHelper.FullModalId).on("hidden.bs.modal", () => {
        $("body").removeClass("modal-open");
    });
}

function fullModalOpenAndClose() {
    // Full modal open and close event handler
    // Add alert and progress bar to full modal and remove
    $("#" + ConstantHelper.FullModalId)
        .on("show.bs.modal",
        function () {
            console.log("show.bs.modal");
            $("#" +ConstantHelper.AlertId).detach().insertBefore($(this).find(".modal-header"));
            $("#" +ConstantHelper.LoaderId).detach().insertAfter($(this).find(".modal-header"));
        })
        .on("hidden.bs.modal", () => {
            $("#" +ConstantHelper.AlertId).detach().prependTo($("nav"));
            $("#" +ConstantHelper.LoaderId).detach().appendTo($("nav"));
        });
}

function msgModalOpen() {
    $("#" +ConstantHelper.MsgModalId).on("show.bs.modal", e => {
        var $invoker = $(e.relatedTarget);
        console.log($invoker);
        $("#" +ConstantHelper.MsgModalId).find('.btn-yes').attr('data-url', $invoker.data('url')).attr('data-rowid', $invoker.data('rowid'));
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
    $(".ctm-checkbox > label").on("click",
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

function downloadBtnClickEvent() {
    var btn = $(event.target);
    var form = btn.parents("form");
    var formHtml = form[0] as HTMLFormElement;
    // Add value to the form, indicating this is the download button
    var isDownloadInput = $('<input />')
        .attr('type', 'hidden')
        .attr('name', "isDownload")
        .attr('value', "true");

    // Append to form and submit
    form.append(isDownloadInput);

    formHtml.submit();

    // After submit, remove the value
    isDownloadInput.remove();
}

// Ajax with file upload
// https://forums.asp.net/t/2026436.aspx?Request+Files+not+working+using+Ajax+BeginForm+on+partial+Views
function uploadBtnClickEvent() {

    var btn = $(event.target);
    var form = btn.parents("form");

    if (form.valid()) {

        var dataString = new FormData(form[0] as HTMLFormElement);

        event.preventDefault();

        $.ajax({
            url: btn.data("uploadurl"),
            data: dataString,
            contentType: false,
            processData: false,
            cache: false,
            type: "POST",
            dataType: "html",
            global: false
        });

    }
};

