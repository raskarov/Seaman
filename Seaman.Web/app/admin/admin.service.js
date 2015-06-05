(function() {
    angular.module("seaman.admin")
        .factory("adminService", adminService);

    adminService.$inject = ["$http", "apiList", "$q", "helper"];

    function adminService($http, apiList, $q, helper) {
        var collectionMethods = [];
        var comments = [];
        var physicians = [];
        var tanks = [];
        var canisters = [];
        var canes = [];
        var positions = [];

        var service = {
            getCollectionMethods: getCollectionMethods,
            saveCollectionMethod: saveCollectionMethod,
            removeCollectionMethod: removeCollectionMethod,
            getComments: getComments,
            saveComment: saveComment,
            removeComment: removeComment,
            getPhysician: getPhysician,
            savePhysician: savePhysician,
            removePhysician: removePhysician,
            getTanks: getTanks,
            saveTank: saveTank,
            removeTank: removeTank,
            getCanisters: getCanisters,
            saveCanister: saveCanister,
            removeCanister: removeCanister,
            getCanes: getCanes,
            saveCane: saveCane,
            removeCane: removeCane,
            getPositions: getPositions,
            savePosition: savePosition,
            removePosition: removePosition
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

        function getTanks(reload) {
            var deferred = $q.defer();
            if (tanks.length && !reload) {
                deferred.resolve(tanks);
            } else {
                $http.get(apiList.tank).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                tanks = data;
                deferred.resolve(data);
            };
        }

        function saveTank(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.tank, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(tanks, { "id": data.id });
                if (index === -1) {
                    tanks.push(data);
                } else {
                    tanks[index] = data;
                }
                deferred.resolve(data);
            }
        }

        function removeTank(id) {
            return $http.delete(apiList.tank + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(tanks, { "id": id });
            }
        }

        function getCanisters(reload) {
            var deferred = $q.defer();
            if (canisters.length && !reload) {
                deferred.resolve(canisters);
            } else {
                $http.get(apiList.canister).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                canisters = data;
                deferred.resolve(data);
            };
        }

        function saveCanister(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.canister, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(canisters, { "id": data.id });
                if (index === -1) {
                    canisters.push(data);
                } else {
                    canisters[index] = data;
                }
                deferred.resolve(data);
            }
        }

        function removeCanister(id) {
            return $http.delete(apiList.canister + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(canisters, { "id": id });
            }
        }

        function getCanes(reload) {
            var deferred = $q.defer();
            if (canes.length && !reload) {
                deferred.resolve(canes);
            } else {
                $http.get(apiList.cane).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                canes = data;
                deferred.resolve(data);
            };
        }

        function saveCane(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.cane, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(canes, { "id": data.id });
                if (index === -1) {
                    canes.push(data);
                } else {
                    canes[index] = data;
                }
                deferred.resolve(data);
            }
        }

        function removeCane(id) {
            return $http.delete(apiList.cane + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(canes, { "id": id });
            }
        }

        function getPositions(reload) {
            var deferred = $q.defer();
            if (positions.length && !reload) {
                deferred.resolve(positions);
            } else {
                $http.get(apiList.position).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                positions = data;
                deferred.resolve(data);
            };
        }

        function savePosition(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.position, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(positions, { "id": data.id });
                if (index === -1) {
                    positions.push(data);
                } else {
                    positions[index] = data;
                }
                deferred.resolve(data);
            }
        }

        function removePosition(id) {
            return $http.delete(apiList.position + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(positions, { "id": id });
            }
        }
    };
})();