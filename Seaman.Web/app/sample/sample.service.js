(function () {
    angular.module("seaman.sample")
        .factory("sampleService", sampleService);

    sampleService.$inject = ['$http', "apiList", 'helper', '$q'];

    function sampleService($http, apiList, helper, $q) {
        var pagedSamples = {};
        var reportSamples = [];
        var extractedSamples = [];
        var service = {
            checkLocation: checkLocation,
            saveSample: saveSample,
            getSamples: getSamples,
            getSample: getSample,
            removeSample: removeSample,
            getSampleReport: getSampleReport,
            getAllReportSamples: getAllReportSamples,
            getSampleLocations: getSampleLocations,
            getExtractedSamples: getExtractedSamples,
            extract: extract,
            removeLocation: removeLocation,
            generateReport: generateReport,
            generateRandomReport: generateRandomReport,
            uploadConsentForm: uploadConsentForm,
            importSamples: importSamples
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

        function getSamples(force) {
            var deferred = $q.defer();
            if (pagedSamples.data && !force) {
                deferred.resolve(pagedSamples);
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

        function getAllReportSamples(force) {
            var deferred = $q.defer();
            if (reportSamples.length && !force) {
                deferred.resolve(reportSamples);
            } else {
                $http.get(apiList.report).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                reportSamples = data;
                deferred.resolve(data);
            }
        }

        function getExtractedSamples(force) {
            var deferred = $q.defer();
            if (extractedSamples.length && !force) {
                deferred.resolve(extractedSamples);
            } else {
                $http.get(apiList.extract).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                extractedSamples = data;
                deferred.resolve(data);
            }
        }

        function extract(model) {
            model = helper.toPascalCase(model);

            return $http.post(apiList.extract, model);
        }

        function getSampleLocations(sampleId) {
            var deferred = $q.defer();
            $http.get(apiList.locations + "/" + sampleId).then(recieved);
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                deferred.resolve(data);
            }
        }

        function removeLocation(id) {
            return $http.delete(apiList.locations + "/" + id);
        }

        function generateReport(model) {
            var deferred = $q.defer();
            model = helper.toPascalCase(model);

            $http.post(apiList.report, model).then(recieved);
            return deferred.promise;
            function recieved(data) {
                data = helper.processData(data.data);
                deferred.resolve(data);
            }
        }

        function generateRandomReport(count) {
            var deferred = $q.defer();
            count = count || 10;
            $http.get(apiList.random + "/" + count).then(recieved);
            return deferred.promise;
            function recieved(data) {
                data = helper.processData(data.data);
                deferred.resolve(data);
            }
        }

        function uploadConsentForm(files, sampleId) {
            if (!files.length) return false;
            var data = new FormData();
            var file = files[0];
            data.append("file", file.file);
            file.cancel();
            if (sampleId) {
                data.append("sampleId", sampleId);
            }
            var request = {
                url: apiList.consentForm,
                method: "POST",
                data: data,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            };

            return $http(request);
        };

        function importSamples(files) {
            if (!files.length) return false;
            var data = new FormData();
            var file = files[0];
            data.append("file", file.file);
            file.cancel();
            var request = {
                url: apiList.importSamples,
                method: "POST",
                data: data,
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            };

            return $http(request);
        }
    }
})();