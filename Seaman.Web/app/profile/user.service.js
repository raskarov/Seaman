(function () {
    angular.module("seaman.profile")
        .factory("userService", userService);

    userService.$inject = ["$http", "apiList", "authService", "helper", "session", "$q"];

    function userService($http, apiList, authService, helper, session, $q) {
        var users = [];
        var roles = [];
        var service = {
            get: getUser,
            login: loginUser,
            logout: logoutUser,
            reset: resetUser,
            changePassword: changePassword,
            setPassword: setPassword,
            isAuthenticated: isAuthenticated,
            isAuthorized: isAuthorized,
            getProfile: getProfile,
            getRoles: getRoles,
            getUsers: getUsers,
            saveUser: saveUser,
            removeUser: removeUser
        };
        return service;

        function processUser(user) {
            user = helper.toCamelCase(user);
            session.create(Date.now(), user.id, user.roles, user.fullName, user.name);
            return user;
        };

        function getUser(userId) {
            var url = apiList.getUser;
            url = userId ? url + '?userId=' + userId : url;
            return $http.get(url).then(function (res) {
                var user = processUser(res.data);
                authService.userReceived(user);
                return user;
            });
        };

        function getRoles() {
            var deferred = $q.defer();
            if (roles.length) {
                deferred.resolve(roles);
            } else {
                $http.get(apiList.roles).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = _.map(data.data, function (item) {
                    item.title = item.name;
                    item.name = helper.toCamelCase(item.name);
                    return item;
                });
                roles = data;
                deferred.resolve(data);
            }
        }

        function loginUser(userModel) {
            return $http.post(apiList.login, userModel).then(function (res) {
                var user = processUser(res.data);
                authService.loginConfirmed(user);
                return user;
            });
        };

        function getProfile(userId) {
            var url = apiList.getProfile;
            url = userId ? url + '?userId=' + userId : url;
            return $http.get(url);
        };

        function logoutUser() {
            return $http.post(apiList.logout);
        };

        function resetUser(username) {
            return $http.post(apiList.reset, { username: username });
        }
        function changePassword(model) {
            model = helper.toPascalCase(model);
            return $http.post(apiList.changePassword, model);
        };

        function setPassword(password) {
            return $http.post(apiList.setPassword, { newPassword: password });
        };
       
        function isAuthenticated() {
            return !!session.userId;
        };

        function isAuthorized(authorizedRoles) {
            if (!angular.isArray(authorizedRoles)) {
                authorizedRoles = [authorizedRoles];
            }
            return (isAuthenticated() && !!_.intersection(authorizedRoles, session.roles).length);
        };

        function getUsers(reload) {
            var deferred = $q.defer();
            if (users.length && !reload) {
                deferred.resolve(users);
            } else {
                $http.get(apiList.user).then(recieved);
            }
            return deferred.promise;

            function recieved(data) {
                data = helper.processData(data.data);
                users = data;
                deferred.resolve(data);
            };

        }

        function saveUser(model) {
            var deferred = $q.defer();
            if (!model.id) {
                model.id = 0;
            }
            model = helper.toPascalCase(model);
            $http.post(apiList.user, model).then(added);
            return deferred.promise;
            function added(data) {
                data = helper.processData(data.data);
                var index = _.findIndex(users, { "id": data.id });
                if (index === -1) {
                    users.push(data);
                } else {
                    users[index] = data;
                }
                deferred.resolve(data);
            }
        }

        function removeUser(id) {
            return $http.delete(apiList.user + "/" + id).then(deleted);

            function deleted(data) {
                _.remove(users, { "id": id });
            }
        }
    };
})();