(function() {
    angular.module("admin.extractReason")
        .controller("ExtractReasonController", ExtractReasonController);

    ExtractReasonController.$inject = ["adminService"];

    function ExtractReasonController(adminService) {
        var vm = this;
        vm.title = "Extract Reason";
        vm.formTitle = "Add Extract Reason";
        vm.reason = {};
        vm.reasons = [];
        vm.add = add;
        vm.remove = remove;
        vm.select = select;

        activate();

        function activate() {
            adminService.getReasons().then(recieved);

            function recieved(data) {
                vm.reasons = data;
            };
        };

        function add() {
            adminService.saveReason(vm.reason).then(added);

            function added(data) {
                clearForm();
                activate();
            }
        }

        function remove(e, id) {
            e.preventDefault();
            e.stopPropagation();
            adminService.removeReason(id).then(removed);
            return false;
            function removed() {
                clearForm();
                activate();
            };
        }

        function select(reason) {
            vm.reason = reason;
        };

        function clearForm() {
            vm.reason = {};
            vm.reasonForm.$setPristine();
        }
    }
})();