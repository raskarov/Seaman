(function() {
    angular.module("admin.collectionMethod")
        .controller("CollectionMethodController", collectionMethodController);

    collectionMethodController.$inject = ["adminService"];

    function collectionMethodController(adminService) {
        var vm = this;
        vm.title = "Methods Of Collection";
        vm.formTitle = "Add Method Of Collection";
        vm.method = {};
        vm.collectionMethods = [];
        vm.save = save;
        vm.remove = remove;
        vm.select = select;

        activate();

        function activate() {
            adminService.getCollectionMethods().then(methodsRecieved);

            function methodsRecieved(data) {
                vm.collectionMethods = data;
            };
        };

        function save() {
            adminService.saveCollectionMethod(vm.method).then(methodAdded);

            function methodAdded(data) {
                clearForm();
                activate();
            }
        }

        function remove(e, id) {
            e.preventDefault();
            e.stopPropagation();
            adminService.removeCollectionMethod(id).then(methodRemoved);

            function methodRemoved() {
                clearForm();
                activate();
            };
        }

        function select(method) {
            vm.method = method;
        };

        function clearForm() {
            vm.method = {};
            vm.methodForm.$setPristine();
        }
    }
})();