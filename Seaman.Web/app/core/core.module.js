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
        'angular-loading-bar',
        //ui grid
        'ui.grid',
        'ui.grid.pagination',
        'ui.grid.selection',
        'ui.grid.exporter',
        'ui.grid.autoResize',
        'ui.grid.cellNav',
        'ui.grid.expandable'
    ]);
})();