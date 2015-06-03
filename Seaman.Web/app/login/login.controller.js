(function () {
    angular.module("seaman.login")
        .controller("LoginController", loginController);

    loginController.$inject = ["$rootScope", "$location", "userService", "routes", "validation"];

    function loginController($rootScope, $location, userService, routes, validation) {
        var vm = this;
        $rootScope.pageTitle = "Вход";
        vm.isPasswordReseting = false;
        vm.requesting = false;
        vm.userModel = {};
        vm.resetModel = {};
        vm.loginField = {
            username: validation.newField('Логин', { required: true }),
            password: validation.newField('Пароль', { required: true, minLength: 6 })
        };

        vm.login = login;
        vm.reset = reset;
        vm.toogleResetForm = toogleResetForm;

        function login() {
            if (vm.loginForm.$invalid) return false;
            return userService.login(vm.userModel)
                .then(function (user) {
                    vm.userModel = {};
                    if ($location.url() === routes.all().login.url) {
                        var routeForRole = routes.getMenuRouteByRole(user.roles);
                        if (routeForRole && routeForRole.url) {
                            $location.url(routeForRole.url);
                        }
                    }
                });
        };

        function reset() {
            if (vm.resetForm.$invalid) return false;
            vm.requesting = true;
            return userService.reset(vm.resetModel)
                .then(function (res) {
                    vm.resetModel = {};
                    vm.requesting = false;
                });
        };

        function toogleResetForm() {
            vm.isPasswordReseting = !vm.isPasswordReseting;
        };
    };
})();