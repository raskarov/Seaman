(function() {
    angular.module("seaman")
        .config(config);

    config.$inject = ["flowFactoryProvider"];

    function config(flowFactoryProvider) {
        flowFactoryProvider.defaults = {
            permanentErrors: [404, 500, 501],
            maxChunkRetries: 1,
            chunkRetryInterval: 5000,
            simultaneousUploads: 4,
            singleFile:true,
            testChunks: false
        };
    };
})();