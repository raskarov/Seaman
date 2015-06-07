(function () {
    angular.module("seaman.sample")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/sample/";
            var state = "sample";
            return [
            {
                state: state,
                requireAuth: true,
                config: {
                    url: "/sample",
                    title: "Sample",
                    skipInMenu: false,
                    templateUrl: alias + "sample.html",
                    controller: "SampleController",
                    controllerAs: 'sc',
                    data: {
                        authorizedRoles: [roles.admin, roles.embryologist]
                    }
                }
            },
            {
                state: state + ".edit",
                config: {
                    url: '/{id}',
                    parent: state
                }
            },
            {
                state: state + ".extract",
                config: {
                    url: '/{id}?{extract}',
                    parent: state
                }
            }
            ];
        };
    };
})();