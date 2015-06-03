(function() {
    angular.module("seaman.login")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/login/";

            return [
            {
                state: "login",
                requireAuth: false,
                config: {
                    url: "/signin",
                    title: "Signin",
                    skipInMenu: true,
                    templateUrl: alias + "login.html",
                    controller: "LoginController",
                    controllerAs: 'lc'
                }
            }];
        };
    };
})();