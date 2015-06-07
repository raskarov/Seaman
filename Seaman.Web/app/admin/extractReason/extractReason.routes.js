(function () {
    angular.module("admin.extractReason")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/admin/extractReason/";

            return [
            {
                state: "admin.extractReason",
                requireAuth: true,
                config: {
                    url: "/extractReason",
                    title: "Extract Reason",
                    skipInMenu: false,
                    templateUrl: alias + "extractReason.html",
                    controller: "ExtractReasonController",
                    controllerAs: 'erc',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();