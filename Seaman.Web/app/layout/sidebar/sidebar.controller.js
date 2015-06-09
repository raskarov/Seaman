(function() {
    angular.module("layout.sidebar")
        .controller("SidebarController", sidebarController);

    sidebarController.$inject = ["$rootScope", "$location", "routes", "session", "USER_ROLES"];

    function sidebarController($rootScope, $location, routes, session, roles) {
        var vm = this;
        vm.menuItems = [];
        //vm.isAdmin = isAdmin;
        
        $rootScope.$on('$stateChangeSuccess', function (e, next, current) {
            vm.menuItems = routes.getMenuByRole(session.roles);
            var isCurrentSelected = _.some(vm.menuItems, function (item) {
                return item.selected && isUrlCompare(item);
            });
            if (!isCurrentSelected) {
                var newItem = _.find(vm.menuItems, isUrlCompare);
                vm.select(newItem);
            }
        });

        vm.select = select;
        activate();

        function select(item) {
            _.forEach(vm.menuItems, function (item) {
                item.selected = false;
            });

            if (!item) return false;
            item.selected = true;
            $rootScope.pageTitle = item.title;
        };

        function isUrlCompare(item) {
            return _.startsWith($location.url(), item.url);
        };


        function activate() {
            var itemToSelect = _.find(vm.menuItems, isUrlCompare);
            if (itemToSelect) {
                vm.select(itemToSelect);
            }
        };

        function isAdmin() {
            return session && _.includes(session.roles, roles.admin);
        }
    };
})();