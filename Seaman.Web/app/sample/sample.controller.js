(function () {
    angular.module("seaman.sample")
        .controller("SampleController", sampleController);

    sampleController.$inject = ["$scope", "validation", "COLORS", "adminService"];

    function sampleController($scope, validation, colors, adminService) {
        var vm = this;
        $scope.thirdPartyRadio = "";
        vm.title = "Sample";
        vm.sampleModel = {};
        vm.location = {};
        vm.locations = [];
        vm.cane = {};
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
        vm.canisters = [];
        vm.canes = [];
        vm.positions = [];

        vm.collectionMethods = [];
        vm.physicians = [];
        vm.comments = [];

        activate();

        function activate() {
            adminService.getTanks(true).then(function(data) {
                vm.tanks = data;
            });

            adminService.getCanisters(true).then(function(data) {
                vm.canisters = data;
            });

            adminService.getCanes(true).then(function(data) {
                vm.canes = data;
            });

            adminService.getPositions(true).then(function(data) {
                vm.positions = data;
            });

            adminService.getCollectionMethods(true).then(function(data) {
                vm.collectionMethods = data;
            });

            adminService.getComments(true).then(function(data) {
                vm.comments = data;
            });

            adminService.getPhysician(true).then(function(data) {
                vm.physicians = data;
            });
        }


        function processCane() {
            if (!vm.cane.name || !vm.cane.color) return false;
            var cane = _.find(vm.canes, { 'name': vm.cane.name, 'color': vm.name.color });
            vm.location.caneId = cane.id;
            vm.cane = {};
        }
    }
})();