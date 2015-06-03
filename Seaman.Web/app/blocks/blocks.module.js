(function() {
    angular.module("seaman.blocks", [
        'blocks.nav',
        "blocks.authentication",
        "blocks.constantsService",
        "blocks.helpers",
        "blocks.router",
        "blocks.validation"
    ]);
})();