(function() {
    angular.module("seaman.samples")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/samples/";
            return [
            {
                state: "samples",
                requireAuth: true,
                config: {
                    url: "/samples",
                    title: "Samples",
                    skipInMenu: false,
                    templateUrl: alias + "samples.html",
                    controller: "SamplesListController",
                    controllerAs: 'slc',
                    data: {
                        authorizedRoles: _.toArray(roles)
                    }
                }
            }];
        };
    };
})();