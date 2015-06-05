(function () {
    angular.module("admin.user")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/admin/user/";

            return [
            {
                state: "admin.user",
                requireAuth: true,
                config: {
                    url: "/user",
                    title: "user",
                    skipInMenu: false,
                    templateUrl: alias + "user.html",
                    controller: "UserController",
                    controllerAs: 'uc',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();