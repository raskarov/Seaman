(function () {
    angular.module("seaman.sample")
        .controller("SampleController", sampleController);

    sampleController.$inject = ["$scope", "validation"];

    function sampleController($scope, validation) {
        var vm = this;
        $scope.thirdPartyRadio = "";
        vm.title = "Sample";
        vm.sampleModel = {};
        vm.sampleField = {
            depositorLastName: validation.newField('Last Name', { required: true }),
            depositorFirstName: validation.newField('First Name', { required: true }),
            depositorDob: validation.newField('Dob', { required: true }),
            depositorSsn: validation.newField('Ssn', { required: true }),
            partnerLastName: validation.newField('Last Name', { required: true }),
            partnerFirstName: validation.newField('First Name', { required: true }),
            partnerDob: validation.newField('Dob', { required: true }),
            partnerSsn: validation.newField('Ssn', { required: true }),
            cryobankName: validation.newField('Cryobank name', { required: true }),
            cryobankVialId: validation.newField('Cryobank’s Vial ID #', { required: true }),
            directedDonorId: validation.newField('Unique donor ID #', { required: true }),
            directedDonorLastName: validation.newField('Last name', { required: true }),
            directedDonorFirstName: validation.newField('First name', { required: true }),
            directedDonorDob: validation.newField('Dob', { required: true }),
            anonymousDonorId: validation.newField('Unique donor ID #', { required: true }),
            date: validation.newField('Date stored', { required: true, date: true }),
            methodOfCollection: validation.newField('Method of collection', { requiredSelect: true }),
            tank: validation.newField('Tank', { requiredSelect: true }),
            canister: validation.newField('Canister', { requiredSelect: true }),
            caneLetter: validation.newField('Cane letter', { requiredSelect: true }),
            caneColor: validation.newField('Cane color', { requiredSelect: true }),
            position: validation.newField('Position', { requiredSelect: true }),
            physician: validation.newField('Physician of record i.e.', { requiredSelect: true }),
            comments: validation.newField('Comments / Warnings', { requiredSelect: true })
        };


        $scope.$watch("thirdPartyRadio", function (value) {
            vm.sampleModel.cryobankPurchased = false;
            vm.sampleModel.directedDonor = false;
            vm.sampleModel.anonymousDonor = false;
            vm.sampleModel[value] = true;
        });
    }
})();