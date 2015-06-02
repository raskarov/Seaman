(function () {
    angular.module("seaman.passwordVerify")
        .directive("passwordVerify", passwordVerify);

    passwordVerify.$inject = [];

    function passwordVerify() {
        return {
            require: 'ngModel',
            scope: {
                passwordVerify: '='
            },
            link: function (scope, element, attrs, ctrl) {
                scope.$watch(function () {
                    if (scope.passwordVerify || ctrl.$viewValue) {
                        return scope.passwordVerify + '_' + ctrl.$viewValue;
                    }
                }, function (value) {
                    if (value) {
                        ctrl.$parsers.unshift(function (viewValue) {
                            var origin = scope.passwordVerify;
                            if (origin !== viewValue) {
                                ctrl.$setValidity("passwordVerify", false);
                                return void 0;
                            } else {
                                ctrl.$setValidity("passwordVerify", true);
                                return viewValue;
                            }
                        });
                    }
                });
            }
        };
    };
})();