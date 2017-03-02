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
    } as JQueryUI.DatepickerOptions);

    // Autocomplete
    registerAutoComplete("CCName", true);
}

function registerAjaxGlobalSettings() {
    // AJAX Global Settings

    $(document).ajaxStart(
        () => {
            $("#loader").show();
        });

    $(document).ajaxStop(
        () => {
            $("#loader").hide();
        });


    $(document).ajaxError((event, jqxhr, settings) => {
        console.log("ajaxError" + jqxhr);
        console.log("ajaxError" + settings);
        // If server effor:
        if (jqxhr.status === 500) {
            //showAlert(jqxhr.responseText, "danger");
            showAlert(jqxhr.statusText, "danger");
        } else {
            showAlert(jqxhr.statusText, "danger");
        }
    });

}

function registerAutoComplete(id: string, allowMultipleValues: boolean = false) {
    // Autocomplete
    // Reference: https://jqueryui.com/autocomplete/
    let curElement: JQuery = $("#" + id);
    let cache: { [index: string]: string; } = {};

    function split(val: string) {
        return val.split(/,\s*/);
    }
    function extractLast(term: string) {
        return split(term).pop();
    }

    curElement
        .on("keydown",
        (event) => {
            if (event.keyCode === $.ui.keyCode.TAB &&
                $(this).autocomplete("instance").menu.active) {
                event.preventDefault();
            }
        });
    curElement
        .autocomplete({
            delay: 100,
            minLength: 1,
            focus: () => false,
            select: (event, ui) => {
                console.log(ui.item.value);
                let curInputVal = curElement.val();
                let curSeletedVal = ui.item.value;
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
            search: (event, ui) => {
                // custom minLength
                console.log(curElement.val());

                let curInputVal = curElement.val();

                let term:string = allowMultipleValues ? extractLast(curInputVal) : curInputVal;
                console.log(term);

                if (term.match(/[\u4E00-\u9FFF\u3400-\u4DFF\uF900-\uFAFF]+/g)) {
                    return true;
                }
                event.preventDefault();
                return false;
            },
            source: (request: any, response: any) => {
                console.log(cache);
                let query: string = extractLast(request.term);

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
                    success: (data: any, status: any, xhr: any) => {
                        cache[query] = data;
                        response(data);
                    },
                    error: () => {
                        response([]);
                    }
                });
            }
        }
    ); 
    curElement
        .autocomplete("option", "classes.ui-autocomplete", "form-autocompelete-menu");
}