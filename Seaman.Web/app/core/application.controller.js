(function() {
    angular.module("seaman.core")
        .controller("ApplicationController", applicationController);

    applicationController.$inject = ["$rootScope", "$scope", "$q", "$location", "USER_ROLES", "AUTH_EVENTS", "userService", "routes", "session", "$state"];

    function applicationController($rootScope, $scope, $q, $location, USER_ROLES, AUTH_EVENTS, userService, routes, session, $state) {
        var vm = this;
        $scope.brand = "Seaman";
        window.$state = $state;
        vm.loader = $q.defer();
        vm.isLoading = true;
        $rootScope.isLoginPage = isLoginPage();
        $rootScope.userRoles = USER_ROLES;
        $rootScope.isAuthorized = userService.isAuthorized;
        $rootScope.setCurrentUser = function(user) {
            $rootScope.currentUser = user;
        };

        $rootScope.$on(AUTH_EVENTS.loginSuccess, userReceived);
        $rootScope.$on(AUTH_EVENTS.userReceived, userReceived);
        $rootScope.$on(AUTH_EVENTS.notAuthenticated, notAuthenticated);

        var firstStateEvent = $scope.$on('$stateChangeStart', function(e, toState, toParams) {
            $rootScope.isLoginPage = isLoginPage(toState.url);
            if (vm.isLoading && ($scope.isLoginPage || $location.url() === "/")) {
                userService.get();
            }
        });

        function isLoginPage(url) {
            url = url || $location.url();
            return url === routes.all().login.url;
        };

        function userReceived(e, user) {
            $rootScope.setCurrentUser(user);
            routes.initRoutes();
            if ($scope.isLoginPage || $location.url() === "/") {
                var route = routes.getMenuRouteByRole(user.roles);
                if (route && route.state) {
                    $location.url(route.url);
                }
            }
            vm.loader.promise.then(function() {
                loaderResolved();
            });
            vm.loader.resolve();
        };

        function notAuthenticated() {
            if (!vm.isLoading) {
                session.destroy();
                $rootScope.setCurrentUser(null);
            } else if (!$scope.isLoginPage) {
                $state.go(routes.all().login.name);
            }
            if (vm.isLoading) {
                vm.loader.resolve();
            }
        };

        function loaderResolved() {
            vm.isLoading = false;
            if (!vm.offStateChange) {
                vm.offStateChange = $scope.$on('$stateChangeStart', onStateChangeStart);
            }
        };

        function onStateChangeStart(event, toState, toParams, fromState, fromParams) {
            if (!$rootScope.currentUser && vm.offStateChange && !$rootScope.isLoginPage) {
                event.preventDefault();
                return false;
            }
            if (fromState && isLoginPage(fromState.url)) {
                var route = routes.getMenuRouteByRole($rootScope.currentUser.roles);
                if (userService.isAuthenticated() && route && route.url) {
                    $location.url(route.url);
                } else {
                    event.preventDefault();
                }
            }
            if (!toState.data || !toState.data.authorizedRoles || !toState.data.authorizedRoles.length) return true;
            var authorizedRoles = toState.data.authorizedRoles;
            if (!userService.isAuthorized(authorizedRoles)) {
                event.preventDefault();
            }
        };
    }
})();