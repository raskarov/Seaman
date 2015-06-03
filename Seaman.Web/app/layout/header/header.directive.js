(function() {
    angular.module("layout.header")
        .directive("header", header);

    header.$inject = [];

    function header() {
        return {
            restrict: "A",
            templateUrl: "/app/layout/header/header.html",
            controller: "HeaderController",
            controllerAs: "hc"
        };
    };
})();