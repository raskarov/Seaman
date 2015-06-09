(function () {
    angular.module("blocks.router")
        .factory("routes", routes);

    routes.$inject = ["USER_ROLES", "routerHelper", "userService", "$state"];

    function routes(USER_ROLES, routerHelper, userService, $state) {
        var routes = {};

        var service = {
            all: all,
            getAllByRole: getAllByRole,
            getRouteByRole: getRouteByRole,
            getMenu: getMenu,
            getMenuByRole: getMenuByRole,
            getMenuRouteByRole: getMenuRouteByRole,
            getSidebarByState: getSidebarByState,
            getDefaultSidebarByRoute: getDefaultSidebarByRoute,
            initRoutes: mapRoutesWithSidebar
        };

        mapRoutesWithSidebar();

        return service;

        function all() {
            return routes;
        };

        function getSidebarByState(url) {
            if (url && url.length) {
                var mainUrl = url.split('/')[1];
                if (!mainUrl.length) return [];
                var mainRoute = routes[mainUrl];
                var subroutes = mainRoute.subroutes;

                _.forEach(subroutes, function (subroute) {
                    subroute.active = $state.includes(subroute.name);
                });

                return subroutes;
            }
            return [];
        };

        function getDefaultSidebarByRoute(route) {
            if (route && route.length) {
                if (angular.isArray(route)) {
                    route = route[0];
                }
                var menuRoute = routes[route.replace("/", "")];
                if (!menuRoute) return null;
                var subRoute = menuRoute.subroutes && menuRoute.subroutes[0];
                if (!subRoute) return menuRoute;
                var result = angular.copy(subRoute);
                result.url = route + result.url;
                return result;
            }
            return null;
        };

        function getAllByRole(roles) {
            if (roles && roles.length) {
                if (angular.isString(roles)) {
                    roles = _.forEach(roles.split(','), function (item) {
                        return item.trim();
                    });
                }
                return _.where(routes, { "roles": roles });
            }
            return [];
        };

        function getRouteByRole(roles) {
            if (roles && roles.length) {
                if (angular.isString(roles)) {
                    roles = [roles];
                }
                return _.find(routes, { 'roles': roles });
            }
            return null;
        };



        function getMenu() {
            return _.filter(routes, { 'skipInMenu': false });
        };

        function getMenuByRole(roles) {
            if (roles && roles.length) {
                if (angular.isString(roles)) {
                    roles = [roles];
                }
                return _.where(routes, {'skipInMenu': false, 'roles': roles });
            }
            return [];
        };

        function getMenuRouteByRole(roles) {
            if (roles && roles.length) {
                var result = getMenuByRole(roles);
                var subroutes = result.length && result[0].subroutes;

                return subroutes.length && subroutes[0] || result[0];
            }
            return null;
        };

        function mapRoutesWithSidebar() {
            states = routerHelper.getStates();
            states = _.sortBy(states, "order");
            var mainRoutes = _.filter(states, function (item) {
                roles = item && item.data && item.data.authorizedRoles || [];
                return !(item.name.split('.').length - 1) && item.name.length && (userService.isAuthorized(roles) || !roles.length);
            });

            var subRoutes = _.filter(states, function (item) {
                roles = item && item.data && item.data.authorizedRoles || [];
                return !(item.name.split('.').length - 2) && item.name.length && userService.isAuthorized(roles);
            });

            _.forEach(mainRoutes, function (route) {
                route = angular.copy(route);
                route.subroutes = [];
                route.state = route.name;
                route.roles = route.data && route.data.authorizedRoles || [];
                delete route.data;
                var subOrder = 0;
                _.forEach(subRoutes, function (subRoute) {                    
                    if (_.includes(subRoute.name, route.name)) {
                        ++subOrder;
                        subRoute = angular.copy(subRoute);
                        subRoute.parent = route;
                        subRoute.state = subRoute.name;
                        subRoute.url = route.url + subRoute.url;
                        subRoute.roles = subRoute.data && subRoute.data.authorizedRoles || [];
                        subRoute.order = subRoute.order || subOrder;
                        delete subRoute.data;
                        route[subRoute.name.replace(route.name + ".", "")] = subRoute;
                        route.subroutes.push(subRoute);
                    }
                });
                route.subroutes = _.sortBy(route.subroutes, "order");
                routes[route.name] = route;
            });
        };
    };
})();