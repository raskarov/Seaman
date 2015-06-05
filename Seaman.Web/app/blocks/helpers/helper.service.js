(function () {
    angular.module("blocks.helpers")
        .factory("helper", helper);

    helper.$inject = [];

    function helper() {
        var service = {
            toLower: toLowerCase,
            format: format,
            toCamelCase: toCamelCase,
            toPascalCase: toPascalCase,
            parseDate: parseDate,
            processData: processData,
            fillToLength: fillToLength
        };

        return service;

        function toLowerCase(target) {
            if (typeof target == "string") {
                return target.toLowerCase();
            }
            if (typeof target == "object") {
                return _.transform(target, function (result, value, key) {
                    delete result[key];
                    result[key.toLowerCase()] = value;
                });
            }
            return target;
        };

        function toCamelCase(target) {
            if (typeof target == "string") {
                return target.substring(0, 1).toLowerCase() + target.substring(1);
            }
            if (typeof target == "object") {
                return _.transform(target, function (result, value, key) {
                    delete result[key];
                    var newKey = key.substring(0, 1).toLowerCase() + key.substring(1);
                    result[newKey] = value;
                });
            }
            return target;
        };

        function toPascalCase(target) {
            if (!target) return null;
            if (typeof target == "string") {
                return target.substring(0, 1).toUpperCase() + target.substring(1);
            }
            if (typeof target == "object") {
                return _.transform(target, function (result, value, key) {
                    delete result[key];
                    var newKey = key.substring(0, 1).toUpperCase() + key.substring(1);
                    result[newKey] = value;
                });
            }
            return target;
        }

        function processData(data) {
            if (angular.isArray(data)) {
                _.forEach(data, function (item, i) {
                    data[i] = toCamelCase(item);
                });
            } else {
                data = toCamelCase(data);
            }
            return data;
        }

        function parseDate(date) {
            if (!date) return false;
            date = date.split("T")[0];
            var parts = date.split("-");
            return new Date(parts[0], parts[1], parts[2]);
        };

        function format() {
            var str = arguments[0];
            var params = Array.prototype.slice.call(arguments, 1);
            _.forEach(params, function (item, i) {
                var pattern = '\\{' + i + '\\}';
                var reg = new RegExp(pattern, 'gi');
                var count = (str.match(reg) || []).length;

                for (var j = 0; j < count; j++) {
                    str = str.replace('{' + i + '}', item);
                }
            });
            return str;
        };

        function fillToLength(value, length, fillChar) {
            if (!value || !length || !value.length || !value.length > length) return value;
            if (!fillChar || !fillChar.length) {
                fillChar = "0";
            }

            var lengthDiff = length - value.length;
            var fillValue = _.times(4, function (n) { return fillChar; }).join("");
            return fillValue + value;
        }
    };

    String.prototype.capitalize = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    }
})();