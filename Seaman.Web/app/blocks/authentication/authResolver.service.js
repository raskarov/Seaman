(function () {
    angular.module("blocks.authentication")
        .factory("authResolver", authResolver);

    authResolver.$inject = ["$q", "userService"];

    function authResolver($q, userService) {
        var service = {
            resolve: resolve
        }
        return service;
            

        function resolve() {
            var deferred = $q.defer();

            userService.get().then(function (user) {
                deferred.resolve(user);
            }, function () {
                deferred.reject();
            });

            return deferred.promise;
        }
    };
})();