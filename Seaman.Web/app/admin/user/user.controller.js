(function() {
    angular.module("admin.user")
        .controller("UserController", userController);

    userController.$inject = ["validation", "userService", "USER_ROLES"];

    function userController(validation, userService, roles) {
        var vm = this;
        vm.title = "Users";
        vm.users = [];
        vm.userModel = {};
        vm.roles = _.toArray(roles);
        vm.userField = {
            login: validation.newField('Login', { required: true }),
            password: validation.newField('Password', { required: true, minLength: 6 })
        };

        vm.saveUser = saveUser;
        vm.removeUser = removeUser;

        activate();

        function activate() {
            getUsers(true);
        }

        function getUsers(reload) {
            userService.getUsers(reload).then(function (data) {
                vm.users = data;
            });
        }

        function saveUser() {
            userService.saveUser(model).then(function(data) {
                getUsers();
            });
        }

        function removeUser(id) {
            userService.removeUser(id).then(function (data) {
                getUsers();
            });
        }
    }
})();