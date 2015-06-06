(function () {
    angular.module("seaman.samples")
        .controller("SamplesListController", samplesListController);

    samplesListController.$inject = ['$scope', 'uiGridConstants', 'sampleService', '$q'];

    function samplesListController($scope, uiGridConstants, sampleService, $q) {
        var vm = this;
        vm.title = "Samples";
     
        var paginationOptions = {
            sort: null
        };

        $scope.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableGridMenu: true,
            enableFiltering: true,
            enableSorting: false,
            exporterMenuCsv: false,
            gridMenuCustomItems: [
            {
                title: 'Extract',
                action: function ($event) {
                    extractSamples(_.filter(this.grid.rows, "isSelected"));
                },
                shown: function($event) {
                    return this.grid.selection.selectedCount > 0;
                },
                order: 210
            }
            ],
            columnDefs: [
              { name: 'depositorFullName'},
              { name: 'locations' },
              { name: 'comment' },
              { name: 'physician' },
              { name: 'collectionMethod' },
              { name: 'dateStored', type: 'date', cellFilter: 'date:"MM/dd/yyyy"' }
            ],
            exporterPdfDefaultStyle: { fontSize: 9 },
            //exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
            exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
            exporterPdfHeader: { text: "Report", style: 'headerStyle' },
            exporterPdfFooter: function (currentPage, pageCount) {
                return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
            },
            exporterPdfCustomFormatter: function (docDefinition) {
                docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
                docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
                return docDefinition;
            },
            exporterPdfOrientation: 'portrait',
            exporterPdfPageSize: 'LETTER',
            exporterPdfMaxGridWidth: 500,
            exporterCsvLinkElement: angular.element(document.querySelectorAll(".custom-csv-link-location")),
            exporterFieldCallback: function (grid, row, col, input) {
                if (col.name === 'dateStored') {
                    return moment(input).format("MM/DD/YYYY");
                } else {
                    return input;
                }
            },
            exporterAllDataFn: function () {
                return getPage(1, $scope.gridOptions.totalItems, paginationOptions.sort)
                .then(function () {
                    $scope.gridOptions.useExternalPagination = false;
                    $scope.gridOptions.useExternalSorting = false;
                    getPage = null;
                });
            },
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
            return sampleService.getSamples()
            .then(function (data) {
                var rows = data.data;
                var firstRow = (curPage - 1) * pageSize;
                $scope.gridOptions.totalItems = data.total < pageSize ? 1 : data.total / pageSize;
                $scope.gridOptions.data = rows.slice(firstRow, firstRow + pageSize);
            });
        };

        getPage(1, $scope.gridOptions.paginationPageSize);

        function extractSamples(samples) {
            if (!angular.isArray(samples)) return false;
            var promises = []
            _.forEach(samples, function(item) {
                promises.push(sampleService.removeSample(item.entity.id));
            });

            $q.all(promises).then(function() {
                getPage(1, $scope.gridOptions.paginationPageSize);
            });
        }
    };
})();