(function () {
    angular.module("seaman.sample")
        .controller("SampleController", sampleController);

    sampleController.$inject = ["$scope", "validation", "COLORS", "adminService", 'COMMON', 'sampleService'];

    function sampleController($scope, validation, colors, adminService, consts, sampleService) {
        var vm = this;
        var letters = angular.copy(consts.alphabet);
        $scope.thirdPartyRadio = "";
        vm.title = "Sample";
        vm.sampleModel = { dateStored: moment().format("MM/DD/YYYY") };
        vm.locations = { 0: {} };
        vm.locationsToRemove = [];
        vm.processCane = processCane;
        vm.colors = _.toArray(colors);
        vm.sampleField = {
            depositorLastName: validation.newField('Last Name', { required: true }),
            depositorFirstName: validation.newField('First Name', { required: true }),
            depositorDob: validation.newField('Dob', { required: true, date: true }),
            depositorSsn: validation.newField('Ssn', { required: true }),
            partnerLastName: validation.newField('Last Name', { required: true }),
            partnerFirstName: validation.newField('First Name', { required: true }),
            partnerDob: validation.newField('Dob', { required: true, date: true }),
            partnerSsn: validation.newField('Ssn', { required: true }),
            cryobankName: validation.newField('Cryobank name', { required: true }),
            cryobankVialId: validation.newField('Cryobank’s Vial ID #', { required: true }),
            directedDonorId: validation.newField('Unique donor ID #', { required: true }),
            directedDonorLastName: validation.newField('Last name', { required: true }),
            directedDonorFirstName: validation.newField('First name', { required: true }),
            directedDonorDob: validation.newField('Dob', { required: true, date: true }),
            anonymousDonorId: validation.newField('Unique donor ID #', { required: true }),
            dateStored: validation.newField('Date stored', { required: true, date: true }),
            methodOfCollection: validation.newField('Method of collection', { requiredSelect: true }),
            tank: validation.newField('Tank', { requiredSelect: true }),
            canister: validation.newField('Canister', { requiredSelect: true }),
            caneLetter: validation.newField('Cane letter', { requiredSelect: true }),
            caneColor: validation.newField('Cane color', { requiredSelect: true }),
            position: validation.newField('Position', { requiredSelect: true }),
            physician: validation.newField('Physician of record i.e.', { requiredSelect: true }),
            comment: validation.newField('Comment / Warning', { requiredSelect: true })
        };


        $scope.$watch("thirdPartyRadio", function (value) {
            vm.sampleModel.cryobankPurchased = false;
            vm.sampleModel.directedDonor = false;
            vm.sampleModel.anonymousDonor = false;
            vm.sampleModel[value] = true;
        });

        vm.tanks = [];
        vm.canisters = {};
        vm.canes = {};
        vm.positions = {};
        vm.letters = {};

        vm.addLocation = addLocation;
        vm.removeLocation = removeLocation;

        vm.saveSample = saveSample;

        vm.collectionMethods = [];
        vm.physicians = [];
        vm.comments = [];

        vm.onStorageChange = onStorageChange;
        vm.onTankChange = onTankChange;
        vm.isFormValid = isFormValid;

        activate();

        function activate() {
            adminService.getTanks(true).then(function (data) {
                vm.tanks = data;
            });

            adminService.getCollectionMethods(true).then(function (data) {
                vm.collectionMethods = data;
            });

            adminService.getComments(true).then(function (data) {
                vm.comments = data;
            });

            adminService.getPhysician(true).then(function (data) {
                vm.physicians = data;
            });
        }

        function saveSample() {
            var sample = angular.copy(vm.sampleModel);
            sample.locationsToAdd = _.toArray(vm.locations);
            sample.locationsToRemove = _.toArray(vm.locationsToRemove);
            sampleService.saveSample(sample).then(function(sample) {
                clearForm();
            });
        }

        function onStorageChange(name) {
            var location = vm.locations[name];
            if (!location || !location.tank || !location.canister || !location.cane || !location.position) return false;
            location.filled = true;
            sampleService.checkLocation(location).then(function (res) {
                location.available = res.data == null || res.data.available;
                if (res.data) {
                    angular.extend(location, res.data);
                }
            });
        }

        function processCane(name) {
            var cane = vm.canes[name];
            if (!cane || !cane.name || !cane.color) return false;
            vm.locations[name].cane = cane.name.capitalize() + cane.color.capitalize();
        }

        function generateStorageByTank(tank, name) {
            if (!tank) return false;
            var i;
            vm.canisters[name] = [];
            for (i = 0; i < tank.canistersCount; i++) {
                vm.canisters[name].push(i);
            }

            vm.letters[name] = letters.substring(0, tank.canesCount / vm.colors.length).split("");

            vm.positions[name] = [];
            for (i = 0; i < tank.positionsCount; i++) {
                vm.positions[name].push(i);
            }
        }

        function onTankChange(name) {
            var tank = _.find(vm.tanks, { "name": vm.locations[name].tank });
            generateStorageByTank(tank, name);
        }

        function addLocation(e) {
            e.preventDefault();
            e.stopPropagation();
            var locationLength = _.toArray(vm.locations).length;
            vm.locations[locationLength] = {};
        }

        function removeLocation(e, name) {
            var location = angular.copy(vm.locations[name]);
            if (location.id) {
                vm.locationsToRemove.push(location);
            }
            delete vm.locations[name];
        }

        function hasUsedLocations() {
            return !_.every(vm.locations, function(item) {
                return item.filled && item.available;
            });
        }

        function isFormValid() {
            return vm.sampleForm.$valid && !hasUsedLocations();
        }

        function clearForm() {
            vm.sampleModel = {};
            vm.locations = { 0: {} };
            vm.canisters = {};
            vm.canes = {};
            vm.positions = {};
            vm.letters = {};
            vm.sampleForm.$setPristine();
            vm.sampleModel.$setUntouched();
        }
    }
})();