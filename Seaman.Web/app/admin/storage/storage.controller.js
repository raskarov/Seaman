(function () {
    angular.module("admin.storage")
        .controller("StorageController", storageController);

    storageController.$inject = ["$q", "adminService", "COLORS", "COMMON"];

    function storageController($q, adminService, colors, consts) {
        var vm = this;
        vm.title = "Storage";
        vm.tanks = [];
        vm.canisters = [];
        vm.canes = [];
        vm.positions = [];
        vm.colors = _.toArray(colors);
        vm.letters = consts.alphabet.split("");

        vm.getTanks = getTanks;
        vm.getCanisters = getCanisters;
        vm.getCanes = getCanes;
        vm.getPositions = getPositions;

        vm.addTank = addTank;
        vm.addCanister = addCanister;
        vm.addCane = addCane;
        vm.addPosition = addPosition;
        vm.remove = remove;
        vm.removeCane = removeCane;

        activate();

        function activate() {
            getTanks();

            getCanisters();

            getCanes();

            getPositions();
        };

        function getTanks() {
            adminService.getTanks().then(function (data) {
                vm.tanks = data;
            });
        };

        function getCanisters() {
            adminService.getCanisters().then(function (data) {
                vm.canisters = data;
            });
        }

        function getCanes() {
            adminService.getCanes().then(function (data) {
                vm.canes = data;
            });
        }

        function getPositions() {
            adminService.getPositions().then(function (data) {
                vm.positions = data;
            });
        };

        function addTank() {
            var length = vm.tanks.length;
            if (length >= vm.letters.length) return false;
            var model = {
                name: vm.letters[length].toUpperCase()
            };
            
            adminService.saveTank(model).then(function() {
                getTanks();
            });
        }

        function addCanister() {
            var lastCanister = vm.canisters.length && vm.canisters[vm.canisters.length - 1].name || 0;
            if (lastCanister >= 6) return false;
            var model = {
                name: +lastCanister + 1
            };

            adminService.saveCanister(model).then(function() {
                getCanisters();
            });
        }

        function addCane() {
            var length = _.uniq(_.map(vm.canes, "name")).length;
            if (length >= vm.letters.length) return false;
            var newLetter = vm.letters[length].toUpperCase();
            var promises = [];
            _.forEach(colors, function(color) {
                var model = {
                    name: vm.letters[length].toUpperCase(),
                    color: color
                };

                promises.push(adminService.saveCane(model));
            });

            $q.all(promises).then(function() {
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
        };

        function removeCane() {
            var length = vm.canes.length;
            var lastCaneLetter = vm.canes[length - 1].name;
            var canesToDelete = _.filter(vm.canes, { 'name': lastCaneLetter });
            var promises = [];
            _.forEach(canesToDelete, function(cane) {
                promises.push(adminService.removeCane(cane.id));
            });

            $q.all(promises).then(function () {
                getCanes();
            });
        }
    };
})();