(function() {
    angular.module("seaman.core")
        .config(config);

    config.$inject = ['$mdThemingProvider'];

    function config($mdThemingProvider) {
        $mdThemingProvider.theme('default')
            .primaryPalette('indigo', {
                'default': '500'
            })
            .accentPalette('cyan', {
                'default': '500'
            })
            .warnPalette('red', {
                'default': '500'
            })
            .backgroundPalette('grey');
    };
})();