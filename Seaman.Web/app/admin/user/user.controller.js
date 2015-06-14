(function () {
    angular.module("admin.user")
        .controller("UserController", userController);

    userController.$inject = ["validation", "userService", "ROLE_TITLE", 'helper'];

    function userController(validation, userService, roles, helper) {
        var vm = this;
        vm.title = "Users";
        vm.users = [];
        vm.userModel = {};
        vm.roles = [];
        vm.userField = {
            username: validation.newField('User Name', { required: true }),
            password: validation.newField('Password', { required: true, minLength: 6 }),
            firstName: validation.newField('First Name', { required: true }),
            lastName: validation.newField('Last Name', { required: true })
        };

        vm.saveUser = saveUser;
        vm.removeUser = removeUser;
        vm.editUser = editUser;

        activate();

        function activate() {
            getUsers(true);
            getRoles();
        }

        function getUsers(reload) {
            userService.getUsers(reload).then(function (data) {
                vm.users = data;
            });
        }

        function getRoles() {
            userService.getRoles().then(function (data) {
                vm.roles = data;
            });
        }

        function editUser(user) {
            vm.userModel = angular.copy(user);
            vm.userModel.roles = {};
            _.forEach(user.roles, function (item, i) {
                vm.userModel.roles[helper.toCamelCase(item.name)] = true;
            });
        }

        function saveUser() {
            var rolesRoAdd = [];
            var rolesToRemove = [];
            _.forEach(vm.roles, function (item) {
                if (vm.userModel.roles[item.name]) {
                    rolesRoAdd.push(item);
                } else {
                    rolesToRemove.push(item);
                }
            });

            vm.userModel.rolesToAdd = rolesRoAdd;
            vm.userModel.rolesToRemove = rolesToRemove;
            userService.saveUser(vm.userModel).then(function (data) {
                clearForm();
                getUsers();
            });
        }

        function removeUser(e, user) {
            e.preventDefault();
            e.stopPropagation();
            userService.removeUser(user.id).then(function (data) {
                if (user.id === vm.userModel.id) {
                    clearForm();
                }
                getUsers();
            });
        }

        function clearForm() {
            vm.userModel = {};
            vm.userForm.$setPristine();
        }
    }
})();