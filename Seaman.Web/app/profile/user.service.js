(function () {
    angular.module("seaman.profile")
        .factory("userService", userService);

    userService.$inject = ["$http", "apiList", "authService", "helper", "session"];

    function userService($http, apiList, authService, helper, session) {
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
            uploadTemp: uploadTemp
        };
        return service;

        function processUser(user) {
            user = helper.toCamelCase(user);
            session.create(Date.now(), user.id, user.roles);
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
        function uploadTemp(files, purpose) {
            var fd = new FormData();
            fd.append('purpose', purpose);
            if (Array.isArray(files)) {
                for (var i = 0; i < files.length; i++) {
                    fd.append('file', files[i]);
                }
            } else {
                fd.append('file', files);
            }

            return $http.post(apiList.uploadFile, fd, {
                //transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            });

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
    };
})();