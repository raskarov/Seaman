(function () {
    angular.module("blocks.router")
    .provider('routerHelper', routerHelperProvider);

    routerHelperProvider.$inject = ['$locationProvider', '$stateProvider', '$urlRouterProvider'];

    function routerHelperProvider($locationProvider, $stateProvider, $urlRouterProvider) {
        this.$get = RouterHelper;

        $locationProvider.html5Mode(true);

        RouterHelper.$inject = ['$state'];

        function RouterHelper($state) {
            var routes = [];
            var service = {
                configureStates: configureStates,
                getStates: getStates,
                routes: routes
            };

            $urlRouterProvider.otherwise(otherwise);

            return service;

            ///////////////////

            function otherwise($injector, $location) {
                var session = $injector.get("session");
                var pathParts = $location.url().split('/');
                var stateName = pathParts[1];
                if (session.id) {
                    var states = $state.get();
                    var state = _.find(states, function(item) {
                        return !item.abstract && !item.skipInMenu;
                    });           
                    if (state) {
                        var subRoute = state.name;
                        $state.go(subRoute);
                    }
                } else {
                    $state.go("login");
                }
            }

            function configureStates(states) {
                states.forEach(function (state) {
                    if (state.requireAuth) {
                        state.config.resolve = {
                            auth: auth
                        };
                    }
                    $stateProvider.state(state.state, state.config);
                });
            }

            function auth(authResolver) {
                return authResolver.resolve();
            };

            function getStates() { return $state.get(); };
        }
    }
})();