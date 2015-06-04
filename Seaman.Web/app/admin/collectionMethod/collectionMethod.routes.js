(function() {
    angular.module("admin.collectionMethod")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/admin/collectionMethod/";

            return [
            {
                state: "admin.collectionMethod",
                requireAuth: true,
                config: {
                    url: "/collectionMethod",
                    title: "Method Of Collection",
                    skipInMenu: false,
                    templateUrl: alias + "collectionMethod.html",
                    controller: "CollectionMethodController",
                    controllerAs: 'cmc',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();