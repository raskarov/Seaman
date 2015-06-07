(function () {
    angular.module("seaman.login")
        .controller("LoginController", loginController);

    loginController.$inject = ["$rootScope", "$location", "userService", "routes", "validation"];

    function loginController($rootScope, $location, userService, routes, validation) {
        var vm = this;
        $rootScope.pageTitle = "Sign In";
        vm.isPasswordReseting = false;
        vm.requesting = false;
        vm.userModel = {};
        vm.resetModel = {};
        vm.loginField = {
            username: validation.newField('Login', { required: true }),
            password: validation.newField('Password', { required: true, minLength: 6 })
        };

        vm.login = login;
        vm.reset = reset;
        vm.toogleResetForm = toogleResetForm;

        function login() {
            if (vm.loginForm.$invalid) return false;
            return userService.login(vm.userModel)
                .then(function (user) {
                    
                });
        };

        function reset() {
            if (vm.resetForm.$invalid) return false;
            return userService.reset(vm.resetModel)
                .then(function (res) {
                    
                });
        };

        function toogleResetForm() {
            vm.isPasswordReseting = !vm.isPasswordReseting;
        };
    };
})();