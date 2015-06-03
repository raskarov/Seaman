(function() {
    angular.module("layout.header")
        .controller("HeaderController", headerController);

    headerController.$inject = ["$rootScope", "userService", "routes", "session", "$state"];

    function headerController($rootScope, userService, routes, session, $state) {
        var vm = this;
        vm.showUserMenu = false;
        vm.showNotifications = false;
        
        vm.logout = logout;
        vm.openProfile = openProfile;
        
        function logout() {
            return userService.logout().then(function () {
                session.destroy();
                $rootScope.setCurrentUser(null);
                $state.go(routes.all().login.state);
            });
        };

        function openProfile() {
            $state.go(routes.all().profile.state);
        };
    };
})();