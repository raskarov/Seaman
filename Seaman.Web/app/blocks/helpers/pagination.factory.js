(function () {
    angular.module("blocks.helpers")
        .factory("Pagination", initPagination);

    initPagination.$inject = ["$http", "helper", "$q"];

    function initPagination($http, helper, $q) {
        var Pagination = Pagination;

        Pagination.prototype.nextPage = nextPage;
        Pagination.prototype.reload = reload;

        return Pagination;

        function Pagination(options) {
            this.request = options && options.request;
            this.items = [];
            this.loading = false;            
            this.take = options && options.take || 20;
        };

        function nextPage() {
            var that = this;
            var deferred = $q.defer();
            if (that.loading || !that.request || that.total == that.items.length) {
                deferred.resolve();
            } else {
                that.loading = true;
                that.request(that.items.length, that.take).then(onRequestSuccess).finally(onFinally);
            }
            return deferred.promise;

            function onRequestSuccess(data) {
                that.total = data.total;
                if (!data.length) return false;
                that.items = _.union(that.items, data);                
            };

            function onFinally() {
                that.loading = false;
                deferred.resolve();
            };
        };

        function reload() {
            var that = this;            
            this.items = [];
            return this.nextPage();
        };
    };
})();