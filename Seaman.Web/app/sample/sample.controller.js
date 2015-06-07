(function () {
    angular.module("seaman.sample")
        .controller("SampleController", sampleController);

    sampleController.$inject = ["$scope", "validation", "COLORS", "adminService", 'COMMON', 'sampleService', "$state"];

    function sampleController($scope, validation, colors, adminService, consts, sampleService, $state) {
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
            partnerLastName: validation.newField('Last Name', { required: false }),
            partnerFirstName: validation.newField('First Name', { required: false }),
            partnerDob: validation.newField('Dob', { required: false, date: true }),
            partnerSsn: validation.newField('Ssn', { required: false }),
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
            if ($state.params && $state.params.id) {
                sampleService.getSample($state.params.id).then(function (data) {
                    data.depositorDob = moment(data.depositorDob).format("MM/DD/YYYY");
                    data.partnerDob = data.partnerDob ? moment(data.partnerDob).format("MM/DD/YYYY") : null;
                    data.directedDonorDob = data.directedDonorDob ? moment(data.directedDonorDob).format("MM/DD/YYYY") : null;
                    data.dateStored = data.dateStored ? moment(data.dateStored).format("MM/DD/YYYY") : null;
                    vm.sampleModel = data;
                    _.forEach(data.locations, function (item, i) {
                        vm.locations[i] = item;
                        onTankChange(i);
                        vm.canes[i] = {
                            name: item.cane.substring(0, 1),
                            color: item.cane.substring(1, item.cane.length)
                        };
                    });
                    activateThirdPartyRadio();
                    delete vm.sampleModel.locations;
                });
            }

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

        function activateThirdPartyRadio() {
            if (vm.sampleModel.cryobankPurchased) {
                $scope.thirdPartyRadio = "cryobankPurchased";
            } else if (vm.sampleModel.directedDonor) {
                $scope.thirdPartyRadio = "directedDonor";
            } else if (vm.sampleModel.anonymousDonor) {
                $scope.thirdPartyRadio = "anonymousDonor";
            }
            
        }

        function saveSample() {
            var sample = angular.copy(vm.sampleModel);
            sample.locationsToAdd = _.toArray(vm.locations);
            sample.locationsToRemove = _.toArray(vm.locationsToRemove);
            sampleService.saveSample(sample).then(function(sample) {
                clearForm();
                $scope.$root.printSample(sample.id, function() {
                    $state.go("samples");
                });
            });
        }

        function onStorageChange(name) {
            var location = vm.locations[name];
            if (!location || !location.tank || !location.canister || !location.cane || !location.position) return false;
            location.filled = true;
            sampleService.checkLocation(location).then(function (res) {
                if (res.data) {
                    angular.extend(location, res.data);
                }
                location.available = res.data == null || res.data.available || vm.sampleModel.id && !res.data.available && res.data.sampleId === vm.sampleModel.id;
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
            for (i = 1; i <= tank.canistersCount; i++) {
                vm.canisters[name].push(i);
            }

            vm.letters[name] = letters.substring(0, tank.canesCount / vm.colors.length).toUpperCase().split("");

            vm.positions[name] = [];
            for (i = 1; i <= tank.positionsCount; i++) {
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
            return _.some(vm.locations, function(item) {
                return item.filled && !item.available;
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
        }
    }
})();