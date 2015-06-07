(function () {
    angular.module("seaman.sample")
        .factory("sampleService", sampleService);

    sampleService.$inject = ['$http', "apiList", 'helper', '$q'];

    function sampleService($http, apiList, helper, $q) {
        var pagedSamples = {};
        var service = {
            checkLocation: checkLocation,
            saveSample: saveSample,
            getSamples: getSamples,
            getSample: getSample,
            removeSample: removeSample,
            getSampleReport: getSampleReport,
            getSamplesReport: getSamplesReport
        };
        return service;

        function checkLocation(location) {
            if (!location) return false;
            location = helper.toPascalCase(location);
            return $http.post(apiList.locationAvailable, location);
        }

        function saveSample(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }

            model = helper.toPascalCase(model);
            $http.post(apiList.sample, model).then(success);
            return deferred.promise;
            function success(data) {
                data = helper.processData(data.data);
                deferred.resolve(data);
            }
        }

        function getSamples(force) { //skip, take, sort
            var deferred = $q.defer();
            if (pagedSamples.data && !force) {
                deferred.resolve(pagedSamples); //var url = getPagingUrl(apiList.sample, skip, take, sort);
            } else {
                $http.get(apiList.sample).then(success);
            }
            return deferred.promise;

            function success(data) {
                data = helper.processData(data.data);
                pagedSamples = data;
                deferred.resolve(data);
            };
        }

        function getSample(id) {
            var deferred = $q.defer();
            $http.get(apiList.sample + "/" + id).success(success);

            return deferred.promise;

            function success(data) {
                data = helper.processData(data);
                deferred.resolve(data);
            };
        }

        function removeSample(id) {
            return $http.delete(apiList.sample + "/" + id);
        }

        function getPagingUrl(url, skip, take, sort) {
            if (angular.isDefined(skip) && take) {
                url = helper.format("{0}?skip={1}&take={2}&sort={3}", url, skip, take, sort);
            }
            return url;
        };

        function getSampleReport(id) {
            var deferred = $q.defer();

            $http.get(apiList.report + "/" + id).then(recieved);
            return deferred.promise;
            function recieved(data) {
                data = helper.processData(data.data);

                deferred.resolve(data);
            }
        }

        function getSamplesReport(ids) {
            var deferred = $q.defer();

            $http.post(apiList.report, ids).then(recieved);
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                deferred.resolve(data);
            }
        }
    }
})();