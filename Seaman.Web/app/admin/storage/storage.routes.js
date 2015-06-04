(function () {
    angular.module("admin.storage")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/admin/storage/";

            return [
            {
                state: "admin.storage",
                requireAuth: true,
                config: {
                    url: "/storage",
                    title: "Storage",
                    skipInMenu: false,
                    templateUrl: alias + "storage.html",
                    controller: "StorageController",
                    controllerAs: 'sc',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();