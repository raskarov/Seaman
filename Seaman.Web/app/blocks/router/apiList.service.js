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
        var user = accountPrefix + "/user";

        //Admin
        var adminPrefix = prefix + "/admin";

        var collectionMethod = adminPrefix + "/collectionMethod";
        var comment = adminPrefix + "/comment";
        var physician = adminPrefix + "/physician";
        var cryobank = adminPrefix + "/cryobank";
        var cane = adminPrefix + "/cane";
        var canister = adminPrefix + "/canister";
        var tank = adminPrefix + "/tank";
        var position = adminPrefix + "/position";
        var location = adminPrefix + "/location";
        var reason = adminPrefix + "/reason";
        //Sample
        var samplePrefix = prefix + "/sample";
        var locationAvailable = samplePrefix + "/available";
        var caneAvailable = samplePrefix + "/caneAvailable";
        var depositorAvailable = samplePrefix + "/depositorAvailable";
        var sample = samplePrefix;
        var report = samplePrefix + "/report";
        var random = samplePrefix + "/random";
        var locations = samplePrefix + "/locations";
        var consentForm = samplePrefix + "/consent";
        var extract = samplePrefix + "/extract";
        var extractSample = samplePrefix + "/extractSample";
        var importSamples = samplePrefix + "/import";
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
            user: user,
            //Admin
            collectionMethod: collectionMethod,
            comment: comment,
            physician: physician,
            cryobank: cryobank,
            cane: cane,
            canister: canister,
            tank: tank,
            position: position,
            location: location,
            reason: reason,
            //Sample
            locationAvailable: locationAvailable,
            caneAvailable: caneAvailable,
            depositorAvailable: depositorAvailable,
            sample: sample,
            report: report,
            random: random,
            locations: locations,
            consentForm: consentForm,
            extract: extract,
            extractSample: extractSample,
            importSamples: importSamples
        };
    };
})();