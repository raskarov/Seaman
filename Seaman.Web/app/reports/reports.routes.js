(function() {
    angular.module("seaman.reports")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());
        function getStates() {
            var alias = "/app/reports/";
            return [
            {
                state: "reports",
                requireAuth: true,
                config: {
                    url: "/reports",
                    title: "Reports",
                    skipInMenu: false,
                    templateUrl: alias + "reports.html",
                    controller: "ReportsController",
                    controllerAs: 'rc',
                    data: {
                        authorizedRoles: _.toArray(roles)
                    }
                }
            }];
        };
    };
})();