(function () {
    angular.module("admin.physician")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/admin/physician/";

            return [
            {
                state: "admin.physician",
                requireAuth: true,
                config: {
                    url: "/physician",
                    title: "Physicians",
                    skipInMenu: false,
                    templateUrl: alias + "physician.html",
                    controller: "PhysicianController",
                    controllerAs: 'pc',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();