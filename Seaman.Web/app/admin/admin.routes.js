(function() {
    angular.module("seaman.admin")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            return [
            {
                state: "admin",
                config: {
                    url: "/admin",
                    abstract: true,
                    title: "Admin",
                    skipInMenu: false,
                    template: '<ui-view/>',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();