(function () {
    angular.module("admin.comment")
        .run(appRun);

    appRun.$inject = ["routerHelper", "USER_ROLES"];

    function appRun(routerHelper, roles) {
        routerHelper.configureStates(getStates());

        function getStates() {
            var alias = "/app/admin/comment/";

            return [
            {
                state: "admin.comment",
                requireAuth: true,
                config: {
                    url: "/comment",
                    title: "Comments",
                    skipInMenu: false,
                    templateUrl: alias + "comment.html",
                    controller: "CommentController",
                    controllerAs: 'cc',
                    data: {
                        authorizedRoles: [roles.admin]
                    }
                }
            }];
        };
    };
})();