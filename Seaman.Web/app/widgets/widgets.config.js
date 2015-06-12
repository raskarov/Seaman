(function() {
    angular.module("seaman.widgets")
    .config(config);

    config.$inject = ["datepickerPopupConfig", "datepickerConfig"];

    function config(datepickerPopupConfig, datepickerConfig) {
        datepickerPopupConfig.datepickerPopup = "MM/dd/yyyy";
        datepickerPopupConfig.showButtonBar = false;
        datepickerPopupConfig.appendToBody = true;
        datepickerConfig.showWeeks = false;
    };
})();