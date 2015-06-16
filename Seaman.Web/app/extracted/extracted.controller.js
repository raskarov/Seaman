(function() {
    angular.module("seaman.extracted")
        .controller("ExtractedController", extractedController);

    extractedController.$inject = ['$scope', 'uiGridConstants', 'sampleService'];

    function extractedController($scope, uiGridConstants, sampleService) {
        var vm = this;
        vm.title = "Extracted Samples";

        vm.gridOptions = {
            showGridFooter: true,
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableFiltering: true,
            enableSorting: false,
            minRowsToShow: 15,
            rowHeight: 40,
            expandableRowHeight: 150,
            expandableRowTemplate: '/app/samples/expandableRowTemplate.html',
            columnDefs: [
              { name: 'depositorFullName' },
              { name: "depositorDob" },
              { name: 'comment' },
              { name: 'physician' },
              {
                  name: 'consentFormUrl', displayName: "Consent Form", enableFiltering: false,
                  cellTemplate: '<div class="ui-grid-cell-contents" layout="row" layout-align="center center">' +
                      '<md-button type="button" target="_blank" ng-href="{{row.entity.consentFormUrl}}" class="md-raised md-primary">Download</md-button></div>'
              }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = vm.gridApi = gridApi;
            }
        };

        getData();

        function getData() {
            return sampleService.getExtractedSamples(true)
            .then(function (data) {
                vm.gridOptions.totalItems = data.length;
                _.map(data, function (item) {
                    item = initSubGrid(item);
                    item.locationsCount = item.locations.length;
                    return item;
                });
                vm.gridOptions.data = data;
            });
        };

        function initSubGrid(item) {
            if (!item.locations || !item.locations.length) return item;
            var locations = _.map(item.locations, function (loc) {
                loc.sampleId = item.id;
                return loc;
            });
            var subGridOptions = {
                showGridFooter: true,
                columnDefs: [
                    { name: 'uniqName' },
                    { name: 'collectionMethod' },
                    { name: 'dateStored' }
                ],
                enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
                data: locations,
                totalItems: locations.length
            };
            item.subGridOptions = subGridOptions;
            return item;
        }
    }
})();