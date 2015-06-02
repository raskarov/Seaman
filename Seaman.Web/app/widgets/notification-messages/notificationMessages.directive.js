(function() {
    angular.module("seaman.notificationMessages")
        .directive("notificationMessages", notificationMessages);

    notificationMessages.$inject = ["AUTH_EVENTS", "$timeout"];

    function notificationMessages(AUTH_EVENTS, $timeout) {
        return {
            restrict: 'E',
            templateUrl: "/app/widgets/notification-messages/notification-messages.html",
            link: link
        };

        function link(scope) {
            scope.messages = [];
            scope.$on(AUTH_EVENTS.serverError, addMessage);

            function addMessage(e, res) {
                var message = {
                    type: 'error',
                    title: res.data.message,
                    description: res.data.ModelState ? res.data.ModelState[""].join(", ") : res.data.exceptionMessage,
                    visible: true
                };

                scope.messages.push(message);

                $timeout(function() {
                    message.visible = false;
                    $timeout(function() {
                        scope.messages.splice(message);
                    }, 1000);
                }, 3000);
            };
        }
    };
})();