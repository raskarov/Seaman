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
                    popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="/Content/main.css" />' +
                        '<link rel="stylesheet" type="text/css" href="/Content/print.css" />' + 
                        '</head><body>' + printContents + '</html>');
                    popupWin.document.close();
                }
            }
        };
    }

    function samplePrint($rootScope, sampleService, $timeout) {
        return {
            restrict: "AE",
            replace: true,
            scope: {
                sample: "=samplePrint"
            },
            templateUrl: "/app/widgets/print/sample.html",
            link: link
        };

        function link(scope, element, attrs) {
            scope.printTitle = scope.sample && scope.sample.printTitle || "Receipt";
            scope.date = moment().format("MM/DD/YYYY");

            $rootScope.printSample = printSample;
            function printSample(id, after) {
                loadSample(id).then(function() {
                    $timeout(function () {
                        if (angular.isFunction(after)) {
                            after();
                        }
                        scope.$root.print();
                    }, 200);
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