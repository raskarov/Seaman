(function () {
    angular.module("blocks.authentication")
        .service("session", session);

    session.$inject = [];

    function session() {
        this.create = create;
        this.destroy = destroy;

        function create(sessionId, userId, roles, fullname, name) {
            this.id = sessionId;
            this.userId = userId;
            this.roles = roles;
            this.fullname = fullname;
            this.name = name;
        };
        function destroy() {
            this.id = null;
            this.userId = null;
            this.roles = null;
            this.fullname = null;
            this.name = null;
        };
    };
})();