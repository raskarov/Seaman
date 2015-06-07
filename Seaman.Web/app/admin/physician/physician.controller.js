(function() {
    angular.module("admin.physician")
        .controller("PhysicianController", physicianController);

    physicianController.$inject = ["adminService"];

    function physicianController(adminService) {
        var vm = this;
        vm.title = "Physicians";
        vm.formTitle = "Add Physician";
        vm.physician = {};
        vm.physicians = [];
        vm.add = add;
        vm.remove = remove;
        vm.select = select;

        activate();

        function activate() {
            adminService.getPhysician().then(recieved);

            function recieved(data) {
                vm.physicians = data;
            };
        };

        function add() {
            adminService.savePhysician(vm.physician).then(added);

            function added(data) {
                clearForm();
                activate();
            }
        }

        function remove(e, id) {
            e.preventDefault();
            e.stopPropagation();
            adminService.removePhysician(id).then(removed);
            return false;
            function removed() {
                clearForm();
                activate();
            };
        }

        function select(physician) {
            vm.physician = physician;
        };

        function clearForm() {
            vm.physician = {};
            vm.physicianForm.$setPristine();
        }
    };
})();