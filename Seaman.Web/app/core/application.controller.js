﻿(function() {
    angular.module("seaman.core")
        .controller("ApplicationController", applicationController);

    applicationController.$inject = ["$rootScope", "$scope", "$location", "USER_ROLES", "AUTH_EVENTS", "userService", "routes", "session", "$state"];

    function applicationController($rootScope, $scope, $location, roles, AUTH_EVENTS, userService, routes, session, $state) {
        var vm = this;
        var stateToReload;
        window.$state = $state;
        vm.isLoading = true;
        $scope.brand = "SEAMAN";
        $rootScope.isLoginPage = isLoginPage();
        $rootScope.isAuthorized = userService.isAuthorized;
        $rootScope.setCurrentUser = function (user) {
            $rootScope.currentUser = user;
            if (!user) {
                $rootScope.userDisplayName = null;
            } else {
                $rootScope.userDisplayName = user.fullName.trim() || user.name;
            }
        };

        $rootScope.$on(AUTH_EVENTS.loginSuccess, userReceived);
        $rootScope.$on(AUTH_EVENTS.userReceived, userReceived);
        $rootScope.$on(AUTH_EVENTS.notAuthenticated, notAuthenticated);
        $scope.$on('$stateChangeStart', onStateChangeStart);

        $scope.isAdmin = isAdmin;
        $scope.isEmbryologist = isEmbryologist;
        $scope.isReportGenerator = isReportGenerator;

        function isAdmin() {
            return _.includes(session.roles, roles.admin);
        }

        function isEmbryologist() {
            return _.includes(session.roles, roles.embryologist);
        }

        function isReportGenerator() {
            return _.includes(session.roles, roles.reportGenerator);
        }

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
                    console.log($state.go);
                    $state.go(route.state);
                }
            } else if (vm.isLoading && stateToReload) {
                $state.go(stateToReload.state, stateToReload.params);
                delete stateToReload;
            }
            vm.isLoading = false;
        };

        function notAuthenticated() {
            if (!vm.isLoading) {
                session.destroy();
                $rootScope.setCurrentUser(null);
            } else if (!$scope.isLoginPage) {
                $state.go(routes.all().login.name);
            }
        };

        function onStateChangeStart(event, toState, toParams, fromState, fromParams) {
            $rootScope.isLoginPage = isLoginPage(toState.url);
            if (!$rootScope.currentUser && vm.isLoading && !$rootScope.isLoginPage) {
                userService.get();
                stateToReload = {state: toState, params: toParams};
                event.preventDefault();
                return false;
            }
            if (!toState.data || !toState.data.authorizedRoles || !toState.data.authorizedRoles.length) return true;
            var authorizedRoles = toState.data.authorizedRoles;
            if (!userService.isAuthorized(authorizedRoles)) {
                event.preventDefault();
            }
            return true;
        };
    };
})();