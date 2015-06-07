(function () {
    angular.module("seaman.samples")
        .controller("SamplesListController", samplesListController);

    samplesListController.$inject = ['$scope', 'uiGridConstants', 'sampleService', '$q', '$state', '$mdDialog', "adminService"];

    function samplesListController($scope, uiGridConstants, sampleService, $q, $state, $mdDialog, adminService) {
        var vm = this;
        vm.title = "Samples";

        vm.isRowSelected = isRowSelected;
        vm.editSample = editSample;
        vm.newSample = newSample;
        vm.extract = extractSamples;
        vm.isRowsSelected = isRowsSelected;
        var paginationOptions = {
            sort: null
        };

        $scope.gridOptions = {
            paginationPageSizes: [50, 100, 200],
            paginationPageSize: 100,
            useExternalPagination: true,
            useExternalSorting: true,
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableFiltering: true,
            enableSorting: false,
            exporterMenuCsv: false,
            columnDefs: [
              { name: 'depositorFullName' },
              { name: "depositorDob", type: 'date', cellFilter: 'date:"MM/dd/yyyy"' },
              { name: 'locations' },
              { name: 'comment' },
              { name: 'physician' },
              { name: 'collectionMethod' },
              { name: 'dateStored', type: 'date', cellFilter: 'date:"MM/dd/yyyy"' }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (getPage) {
                        if (sortColumns.length > 0) {
                            paginationOptions.sort = sortColumns[0].sort.direction;
                        } else {
                            paginationOptions.sort = null;
                        }
                        getPage(grid.options.paginationCurrentPage, grid.options.paginationPageSize, paginationOptions.sort);
                    }
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    if (getPage) {
                        getPage(newPage, pageSize, paginationOptions.sort);
                    }
                });
            }
        };

        var getPage = function (curPage, pageSize, sort) {
            return sampleService.getSamples(true)
            .then(function (data) {
                var rows = data.data;
                var firstRow = (curPage - 1) * pageSize;
                $scope.gridOptions.totalItems = data.total < pageSize ? 1 : data.total / pageSize;
                $scope.gridOptions.data = rows.slice(firstRow, firstRow + pageSize);
            });
        };

        getPage(1, $scope.gridOptions.paginationPageSize, null, true);

        function extractSamples(ev) {
            $mdDialog.show({
                controller: DialogController,
                templateUrl: '/app/samples/extract.dialog.html',
                parent: angular.element(document.body),
                targetEvent: ev
            })
            .then(function (answer) {
                $scope.gridApi.selection.clearSelectedRows();
                getPage(1, $scope.gridOptions.paginationPageSize);
            }, function () {

            });
        }

        function isRowSelected() {
            return $scope.gridApi.grid.selection.selectedCount === 1;
        }

        function isRowsSelected() {
            return $scope.gridApi.grid.selection.selectedCount > 0;
        }

        function editSample() {
            var selectedRow = getSelectedRows()[0];
            $state.go("sample.edit", { id: selectedRow.entity.id });
        }

        function newSample() {
            $state.go("sample");
        }

        function getSelectedRows() {
            return $scope.gridApi.selection.getSelectedRows();
        }

        function DialogController($scope, $mdDialog, $timeout) {
            var samples = getSelectedRows();
            var ids = _.map(samples, "id");
            var promises = [];
            $scope.reasons = [];
            $scope.reason = {};
            adminService.getReasons().then(function (data) {
                $scope.reasons = data;
            });

            $scope.cancel = function () {
                $mdDialog.cancel();
            };
            $scope.extract = function () {
                sampleService.getSamplesReport(ids).then(function (data) {
                    data = _.map(data, function (item) {
                        item.reason = $scope.reason;
                        return item;
                    });
                    $scope.samples = data;
                    $timeout(function () {
                        $scope.$root.print();
                        _.forEach(samples, function (item) {
                            promises.push(sampleService.removeSample(item.id));
                        });

                        $q.all(promises).then(function () {
                            $mdDialog.hide($scope.reason);
                        });
                    });
                });
            };
        }
    };
})();