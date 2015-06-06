(function () {
    angular.module("seaman.loader")
    .config(config);

    config.$inject = ["$httpProvider"];

    function config($httpProvider) {


        $httpProvider.interceptors.push(['$rootScope', '$q', function ($rootScope, $q) {
            return {
                request: request,                
                response: response,
                responseError: responseError
            };

            function request(config) {
                $rootScope.showLoader = true;
                return config;
            }
            function response(response) {
                $rootScope.showLoader = false;
                return response;
            }

            function responseError(rejection) {
                $rootScope.showLoader = false;
                return $q.reject(rejection);
            }
        }]);
    };
})();