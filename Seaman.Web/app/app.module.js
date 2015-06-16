(function() {
    angular.module("seaman", [
        "seaman.core",
        "seaman.layout",
        'seaman.ui',
        //Pages
        "seaman.login",
        "seaman.profile",
        "seaman.samples",
        "seaman.sample",
        "seaman.admin",
        "seaman.reports",
        "seaman.extracted"
    ]);
})();