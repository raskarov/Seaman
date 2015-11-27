(function () {
    angular.module("admin.cryobank")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/admin/cryobank/";

            return [
            {
                state: "admin.cryobank",
                requireAuth: true,
                config: {
                    url: "/cryobank",
                    title: "Cryobanks",
                    skipInMenu: false,
                    templateUrl: alias + "cryobank.html",
                    controller: "CryobankController",
                    controllerAs: 'cb',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();