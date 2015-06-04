(function() {
    angular.module("admin.comment")
        .controller("CommentController", commentController);

    commentController.$inject = ["adminService"];

    function commentController(adminService) {
        var vm = this;
        vm.title = "Comments";
        vm.formTitle = "Add Comment";
        vm.comment = {};
        vm.comments = [];
        vm.add = add;
        vm.remove = remove;
        vm.select = select;

        activate();

        function activate() {
            adminService.getComments().then(commentsRecieved);

            function commentsRecieved(data) {
                vm.comments = data;
            };
        };

        function add() {
            adminService.saveComment(vm.comment).then(commentAdded);

            function commentAdded(data) {
                clearForm();
                activate();
            }
        }

        function remove(e, id) {
            e.preventDefault();
            e.stopPropagation();
            adminService.removeComment(id).then(commentRemoved);

            function commentRemoved() {
                clearForm();
                activate();
            };
        }

        function select(comment) {
            vm.comment = comment;
        };

        function clearForm() {
            vm.comment = {};
            vm.commentForm.$setPristine();
        }
    }
})();