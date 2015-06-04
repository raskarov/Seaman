(function() {
    angular.module("seaman.admin")
        .factory("adminService", adminService);

    adminService.$inject = ["$http", "apiList", "$q", "helper"];

    function adminService($http, apiList, $q, helper) {
        var collectionMethods = [];
        var comments = [];
        var physicians = [];

        var service = {
            getCollectionMethods: getCollectionMethods,
            saveCollectionMethod: saveCollectionMethod,
            removeCollectionMethod: removeCollectionMethod,
            getComments: getComments,
            saveComment: saveComment,
            removeComment: removeComment,
            getPhysician: getPhysician,
            savePhysician: savePhysician,
            removePhysician: removePhysician
        };

        return service;

        function getCollectionMethods(reload) {
            var deferred = $q.defer();
            if (collectionMethods.length && !reload) {
                deferred.resolve(collectionMethods);
            } else {
                $http.get(apiList.collectionMethod).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                collectionMethods = data;
                deferred.resolve(data);
            };
        }

        function saveCollectionMethod(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.collectionMethod, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(collectionMethods, { "id": data.id });
                if (index === -1) {
                    collectionMethods.push(data);
                } else {
                    collectionMethods[index] = data;
                }
                deferred.resolve(data);
            }
        };

        function removeCollectionMethod(id) {
            return $http.delete(apiList.collectionMethod + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(collectionMethods, { "id": id });
            }
        }

        function getComments(reload) {
            var deferred = $q.defer();
            if (comments.length && !reload) {
                deferred.resolve(comments);
            } else {
                $http.get(apiList.comment).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                comments = data;
                deferred.resolve(data);
            };
        }

        function saveComment(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.comment, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(comments, { "id": data.id });
                if (index === -1) {
                    comments.push(data);
                } else {
                    comments[index] = data;
                }
                deferred.resolve(data);
            }
        };

        function removeComment(id) {
            return $http.delete(apiList.comment + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(comments, { "id": id });
            }
        }

        function getPhysician(reload) {
            var deferred = $q.defer();
            if (physicians.length && !reload) {
                deferred.resolve(physicians);
            } else {
                $http.get(apiList.physician).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                physicians = data;
                deferred.resolve(data);
            };
        }

        function savePhysician(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.physician, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(physicians, { "id": data.id });
                if (index === -1) {
                    physicians.push(data);
                } else {
                    physicians[index] = data;
                }
                deferred.resolve(data);
            }
        };

        function removePhysician(id) {
            return $http.delete(apiList.physician + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(physicians, { "id": id });
            }
        }
    };
})();