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
            enableGridMenu: true,
            columnDefs: [
              { name: 'depositorFullName' },
              { name: "depositorDob" },
              { name: 'comment' },
              { name: 'physician' },
              { name: 'depositorFirstName', visible: false },
              { name: 'depositorLastName', visible: false },
              { name: 'depositorSsn', visible: false },
              { name: 'partnerFirstName', visible: false },
              { name: 'partnerLastName', visible: false },
              { name: 'partnerDob', visible: false },
              { name: 'partnerSsn', visible: false },
              { name: 'autologous', visible: false },
              { name: 'refreeze', visible: false },
              { name: 'testingOnFile', visible: false },
              { name: 'cryobankName', visible: false },
              { name: 'cryobankVialId', visible: false },
              { name: 'directedDonorId', visible: false },
              { name: 'directedDonorLastName', visible: false },
              { name: 'directedDonorFirstName', visible: false },
              { name: 'directedDonorDob', visible: false },
              { name: 'anonymousDonorId', visible: false },
              {
                  name: 'consentFormUrl', displayName: "Consent Form", enableFiltering: false,
                  cellTemplate: '<div class="ui-grid-cell-contents" layout="row" layout-align="center center">' +
                      '<md-button type="button" target="_blank" ng-show="row.entity.consentFormUrl" ng-href="{{row.entity.consentFormUrl}}" class="md-raised md-primary">Download</md-button></div>'
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
                data = _.map(data, function (item) {
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
                    { name: 'uniqName', displayName: "Stored Location" },
                    { name: 'collectionMethod' },
                    { name: 'dateStored' },
                    { name: 'dateFrozen' },
                    { name: 'dateExtracted' },
                    {name: 'reasonForExtraction'}
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