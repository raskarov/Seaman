(function() {
    angular.module("seaman.core", [
        // Angular modules
        "ngMessages",
        'ngSanitize',
        'ngAnimate',
        //Cross-app modules
        "seaman.blocks",
        //3rd-party modules
        'ui.router',
        'ngMaterial',
        'ngMask',
        'angular.filter',
        //ui grid
        'ui.grid',
        'ui.grid.pagination',
        'ui.grid.selection',
        'ui.grid.exporter'
    ]);
})();