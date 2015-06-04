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

        //Admin
        var adminPrefix = prefix + "/admin";

        var collectionMethod = adminPrefix + "/collectionMethod";
        var comment = adminPrefix + "/comment";
        var physician = adminPrefix + "/physician";
        var cane = adminPrefix + "/cane";
        var canister = adminPrefix + "/canister";
        var tank = adminPrefix + "/tank";
        var location = adminPrefix + "/location";

        return {
            ////Account api
            roles: roles,
            login: login,
            logout: logout,
            reset: reset,
            changePassword: changePassword,
            setPassword: setPassword,
            getUser: getUser,
            getProfile: getProfile,
            //Admin
            collectionMethod: collectionMethod,
            comment: comment,
            physician: physician,
            cane: cane,
            canister: canister,
            tank: tank,
            location: location
        };
    };
})();