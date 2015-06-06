(function() {
    angular.module("seaman.samples")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/samples/";
            var state = "samples";
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
                        authorizedRoles: [roles.admin, roles.embryologist]
                    }
                }
            },
                {
                    state: state + ".edit",
                    config: {
                        url: '/{id}',
                        parent: state,
                        templateUrl: "/app/sample/sample.html",
                        controller: "SampleController",
                        controllerAs: 'sc'
                    }
                }
            ];
        };
    };
})();