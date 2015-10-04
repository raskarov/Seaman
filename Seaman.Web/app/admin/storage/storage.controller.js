(function () {
    angular.module("admin.storage")
        .controller("StorageController", storageController);

    storageController.$inject = ['$scope', "$q", "adminService", "COLORS", "COMMON"];

    function storageController($scope, $q, adminService, colors, consts) {
        var vm = this;
        vm.title = "Storage";
        vm.tanks = [];
        //vm.canisters = [];
        //vm.canes = [];
        //vm.positions = [];
        vm.colors = _.toArray(colors);
        vm.letters = consts.alphabet.split("");
        vm.tankModel = {};
        vm.getTanks = getTanks;
        vm.editTank = editTank;
        //vm.getCanisters = getCanisters;
        //vm.getCanes = getCanes;
        //vm.getPositions = getPositions;

        vm.addTank = addTank;
        vm.addCanister = addCanister;
        vm.addCane = addCane;
        vm.addPosition = addPosition;
        vm.remove = remove;
        vm.removeCane = removeCane;
        var letters = [];
        activate();

        function activate() {
            getTanks();

            //getCanisters();

            //getCanes();

            //getPositions();
        };

        function getTanks() {
            adminService.getTanks().then(function (data) {
                vm.letters = consts.alphabet.split("");
                var t = vm.letters;
                for (var i = 0; i < data.length; i++) {
                    if (t.indexOf(data[i].name.toLowerCase()) != -1)
                        vm.letters.splice(t.indexOf(data[i].name.toLowerCase()), 1);
                }
                vm.tanks = data;
                var length = vm.tanks.length;
                if (length >= vm.letters.length) return false;
                vm.tankModel.name = vm.letters[0].toUpperCase();
                letters = [];
                for (var i = 0; i < vm.letters.length; i++) {
                    letters.push({
                        id: vm.letters[i].toUpperCase(),
                        name: vm.letters[i].toUpperCase()
                    });
                }
                $scope.options = letters;
            });
        };

        //function getCanisters() {
        //    adminService.getCanisters().then(function (data) {
        //        vm.canisters = data;
        //    });
        //}

        //function getCanes() {
        //    adminService.getCanes().then(function (data) {
        //        vm.canes = data;
        //    });
        //}

        //function getPositions() {
        //    adminService.getPositions().then(function (data) {
        //        vm.positions = data;
        //    });
        //};

        function addTank() {
            var length = vm.tanks.length;
            if (length >= vm.letters.length) return false;
            adminService.saveTank(vm.tankModel).then(function () {
                vm.tankModel = {};
                vm.tankForm.$setPristine();
                getTanks();
            });
        }

        function editTank(tank) {
            vm.tankModel = tank;
            vm.letters = consts.alphabet.split("");
            adminService.getTanks().then(function (data) {
                var t = vm.letters;
                for (var i = 0; i < data.length; i++) {
                    if (t.indexOf(data[i].name.toLowerCase()) != -1)
                        vm.letters.splice(t.indexOf(data[i].name.toLowerCase()), 1);
                }
                var length = vm.tanks.length;
                if (length >= vm.letters.length) return false;
                letters = [];
                letters.push({
                    id: tank.name.toUpperCase(),
                    name: tank.name.toUpperCase()
                });
                for (var i = 0; i < vm.letters.length; i++) {
                    letters.push({
                        id: vm.letters[i].toUpperCase(),
                        name: vm.letters[i].toUpperCase()
                    });
                }
                $scope.options = letters;
            });
            //vm.tankModel.name = lettersForEdit[tank.name.toUpperCase()];
        }

        function addCanister() {
            var lastCanister = vm.canisters.length && vm.canisters[vm.canisters.length - 1].name || 0;
            if (lastCanister >= 6) return false;
            var model = {
                name: +lastCanister + 1
            };

            adminService.saveCanister(model).then(function () {
                getCanisters();
            });
        }

        function addCane() {
            var length = _.uniq(_.map(vm.canes, "name")).length;
            if (length >= vm.letters.length) return false;
            var newLetter = vm.letters[length].toUpperCase();
            var promises = [];
            _.forEach(colors, function (color) {
                var model = {
                    name: vm.letters[length].toUpperCase(),
                    color: color
                };

                promises.push(adminService.saveCane(model));
            });

            $q.all(promises).then(function () {
                getCanes();
            });
        }

        function addPosition() {
            var lastPosition = vm.positions.length && vm.positions[vm.positions.length - 1].name || 0;
            if (lastPosition >= 6) return false;
            var model = {
                name: +lastPosition + 1
            };

            adminService.savePosition(model).then(function () {
                getCanisters();
            });
        }

        function remove(name) {
            if (!name) return false;
            var collection = vm[name + 's'];
            var id = collection[collection.length - 1].id;
            adminService["remove" + name.capitalize()](id).then(function (data) {
                vm['get' + name.capitalize() + 's']();
            });
            vm.letters = consts.alphabet.split("");
        };

        function removeCane() {
            var length = vm.canes.length;
            var lastCaneLetter = vm.canes[length - 1].name;
            var canesToDelete = _.filter(vm.canes, { 'name': lastCaneLetter });
            var promises = [];
            _.forEach(canesToDelete, function (cane) {
                promises.push(adminService.removeCane(cane.id));
            });

            $q.all(promises).then(function () {
                getCanes();
            });
        }
    };
})();