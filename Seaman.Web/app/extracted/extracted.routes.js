(function() {
    angular.module("seaman.extracted")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/extracted/";
            var state = "extracted";
            return [
            {
                state: state,
                requireAuth: true,
                config: {
                    url: "/extracted",
                    title: "extracted",
                    skipInMenu: false,
                    templateUrl: alias + "extracted.html",
                    controller: "ExtractedController",
                    controllerAs: 'ec',
                    data: {
                        authorizedRoles: _.toArray(roles)
                    }
                }
            }];
        };
    };
})();