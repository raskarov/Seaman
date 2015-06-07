(function () {
    angular.module("seaman.print")
        .directive("appPrint", appPrint)
        .directive("samplePrint", samplePrint);


    appPrint.$inject = ['$window'];

    samplePrint.$inject = ['$rootScope', "sampleService", "$timeout"];

    function appPrint($window) {
        return {
            restrict: "AE",
            replace: true,
            transclude: true,
            templateUrl: "/app/widgets/print/print.html",
            scope: {},
            link: function (scope, element, attrs) {
                var printContents, originalContents, popupWin;
                scope.$root.print = print;

                function print() {
                    printContents = document.getElementById('invoice').innerHTML;
                    originalContents = document.body.innerHTML;
                    popupWin = $window.open();
                    popupWin.document.open();
                    popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="/Content/main.css" /></head><body onload="window.print()">' + printContents + '</html>');
                    popupWin.document.close();
                }
            }
        };
    }

    function samplePrint($rootScope, sampleService, $timeout) {
        return {
            restrict: "AE",
            replace: true,
            scope: false,
            templateUrl: "/app/widgets/print/sample.html",
            link: link
        };

        function link(scope, element, attrs) {
            scope.printTitle = "Receipt";
            scope.year = new Date().getFullYear();

            $rootScope.printSample = printSample;
            function printSample(id, after) {
                loadSample(id).then(function() {
                    $timeout(function () {
                        scope.$root.print();
                        if (angular.isFunction(after)) {
                            after();
                        }
                    });
                });
            }

            function loadSample(id) {
                return sampleService.getSampleReport(id).then(recieved);
                function recieved(data) {
                    scope.sample = data;
                }
            }
        }
    }
})();