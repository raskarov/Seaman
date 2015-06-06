(function () {
    angular.module("seaman.loader")
        .directive("loader", loader);

    loader.$inject = [];

    function loader() {
        return {
            restrict: "A",            
            link: link
        };

        function link(scope, element, attrs) {
            var opts = {
                lines: 13, // The number of lines to draw
                length: 8, // The length of each line
                width: 3, // The line thickness
                radius: 8, // The radius of the inner circle
                corners: 1, // Corner roundness (0..1)
                rotate: 0, // The rotation offset
                direction: 1, // 1: clockwise, -1: counterclockwise
                color: '#000', // #rgb or #rrggbb or array of colors
                speed: 1, // Rounds per second
                trail: 60, // Afterglow percentage
                shadow: false, // Whether to render a shadow
                hwaccel: false, // Whether to use hardware acceleration
                className: 'spinner', // The CSS class to assign to the spinner
                zIndex: 2e9, // The z-index (defaults to 2000000000)
                top: '50%', // Top position relative to parent
                left: '50%' // Left position relative to parent
            };

            var spinner = new Spinner(opts).spin(element[0]);
                       
            scope.$watch("showLoader", function (value) {
                if (value) {
                    show();
                } else {
                    hide();
                }
            });

            function show() {
                spinner.spin(element[0]);
            };

            function hide() {
                spinner.stop();
            };
        }
    };
})();