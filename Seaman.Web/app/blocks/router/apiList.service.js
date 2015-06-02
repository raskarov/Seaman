(function () {
    angular.module("blocks.router")
        .factory("apiList", apiList);

    apiList.$inject = [];

    function apiList() {
        var prefix = "/api";
        //Accout
        var accountPrefix = prefix + "/account";

        var roles = accountPrefix + "/roles";
        var login = accountPrefix + "/login";
        var logout = accountPrefix + "/logout";
        var reset = accountPrefix + "/reset";
        var changePassword = accountPrefix + "/changePassword";
        var setPassword = accountPrefix + "/setPassword";
        var getUser = accountPrefix + "/getUser";
        var getProfile = accountPrefix + '/getProfile';

        return {
            ////Account api
            roles: roles,
            login: login,
            logout: logout,
            reset: reset,
            changePassword: changePassword,
            setPassword: setPassword,
            getUser: getUser,
            getProfile: getProfile
        };
    };
})();