(function() {
    angular.module("layout.header")
        .controller("HeaderController", headerController);

    headerController.$inject = [];

    function headerController() {
        var vm = this;
        vm.brand = "Seaman";
        vm.showUserMenu = false;
    }
})();