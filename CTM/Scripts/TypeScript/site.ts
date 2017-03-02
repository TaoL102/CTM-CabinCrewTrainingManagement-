/// <reference path="method.ts"/>
/// <reference path="register.ts"/>
/// <reference path="event_handler.ts"/>

// Register 
registerPlugins();
registerAjaxGlobalSettings();

// Bind event handlers
modalOpen();
fullModalOpenAndClose();
msgModalOpen();
msgModalYesBtnClick();
checkBoxClick();
checkBoxHidableDivHide();

// Client Validation
