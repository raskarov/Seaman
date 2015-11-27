(function() {
    angular.module("admin.cryobank")
        .controller("CryobankController", cryobankController);

    cryobankController.$inject = ["adminService"];

    function cryobankController(adminService) {
        var vm = this;
        vm.title = "Cryobanks";
        vm.formTitle = "Add Cryobankn";
        vm.cryobank = {};
        vm.cryobanks = [];
        vm.add = add;
        vm.remove = remove;
        vm.select = select;

        activate();

        function activate() {
            adminService.getCryobank().then(recieved);

            function recieved(data) {
                console.log(vm.cryobank);
                vm.cryobanks = data;
                
            };
        };

        function add() {
            adminService.saveCryobank(vm.cryobank).then(added);

            function added(data) {
                clearForm();               
                activate();
            }
        }

        function remove(e, id) {
            e.preventDefault();
            e.stopPropagation();
            adminService.removeCryobank(id).then(removed);
            return false;
            function removed() {
                clearForm();
                activate();
            };
        }

        function select(cryobank) {
            vm.cryobank = cryobank;
        };

        function clearForm() {
            vm.cryobank = {};
            vm.cryobankForm.$setPristine();
        }
    };
})();