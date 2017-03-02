// Client Validation
function registerIsCabinCrewValidation(url:string) {

//Reference：http://stackoverflow.com/questions/7247250/jquery-validation-not-waiting-for-remote-validation-to-return-true-considers-fo
$.validator.addMethod("iscabincrew", function (value, element, param) {
    if (this.optional(element)) {
        return "dependency-mismatch";
    }
    if (!value.match(/[\u4E00-\u9FFF\u3400-\u4DFF\uF900-\uFAFF]+/g)) {
        return false;
    }

    var previous = this.previousValue(element);
    var elementName = element.id;
    if (!this.settings.messages[elementName]) {
        this.settings.messages[elementName] = {};
    }
    previous.originalMessage = this.settings.messages[elementName].remote;
    this.settings.messages[elementName].remote = previous.message;

    param = typeof param === "string" && { url: param } || param;

    if (previous.old === value) {
        return previous.valid;
    }

    previous.old = value;
    this.startRequest(element);
    var data:{[index:string]:string} = {};
    data["names"] = value;
    var valid = false;
    $.ajax($.extend(true, {
        url: url,
        async: false,
        mode: "abort",
        port: "validate" + elementName,
        data: data,
        success: (response:any) => {
            this.settings.messages[elementName].remote = previous.originalMessage;
            valid = response === true || response === "True";
            if (valid) {
                var submitted = this.formSubmitted;
                this.prepareElement(element);
                this.formSubmitted = submitted;
                this.successList.push(element);
                delete this.invalid[elementName];
                this.showErrors();
            } else {
                var errors:{[index:string]:string} = {};
                var message = response || this.defaultMessage(element, "remote");
                errors[elementName] = previous.message = $.isFunction(message) ? message(value) : message;
                this.invalid[elementName] = true;
                this.showErrors(errors);
            }
            previous.valid = valid;
            this.stopRequest(element, valid);
        }
    }, param));
    return valid;
});

$.validator.unobtrusive.adapters.add("iscabincrew", [], (options:any) => {
    options.rules["iscabincrew"] = true;
    options.messages["iscabincrew"] = options.message;
});

}
