(function() {
    angular.module("layout.sidebar")
        .directive("sidebar", sidebar);

    sidebar.$inject = [];

    function sidebar() {
        return {
            restrict: "A",
            templateUrl: "/app/layout/sidebar/sidebar.html",
            controller: "SidebarController",
            controllerAs: "sc"
        };
    }
})();