(function () {
    angular.module("seaman.samples")
        .controller("SamplesListController", samplesListController);

    samplesListController.$inject = ['$scope', '$compile', 'uiGridConstants', 'sampleService', '$q', '$state', '$mdDialog', "adminService", '$timeout', 'uiGridExporterConstants'];

    function samplesListController($scope, $compile, uiGridConstants, sampleService, $q, $state, $mdDialog, adminService, $timeout, uiGridExporterConstants) {
        var vm = this;
        vm.title = "Samples";

        var selectedSubrows = [];
        vm.gridApi = {};
        vm.printVisibleColumns = true;
        vm.isRowSelected = isRowSelected;
        vm.editSample = editSample;
        vm.newSample = newSample;
        vm.extract = extractSamples;
        vm.isRowsSelected = false;
        vm.isSubRowsSelected = false;
        vm.printSample = printSample;
        vm.randomReport = randomReport;
        vm.printAllRows = printAllRows;
        vm.printSelectedRows = printSelectedRows;
        vm.printVisibleRows = printVisibleRows;

        $scope.gridOptions = {
            showGridFooter: true,
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableFiltering: true,
            enableSorting: false,
            exporterMenuCsv: false,
            minRowsToShow: 20,
            expandableRowHeight: 150,
            expandableRowTemplate: '/app/samples/expandableRowTemplate.html',
            columnDefs: [
              { name: "locationsCount", maxWidth: 100, displayName: "Locations", enableFiltering: false },
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
              { name: 'anonymousDonorId', visible: false }
            ],
            enableGridMenu: true,
            enableSelectAll: true,
            exporterPdfDefaultStyle: { fontSize: 10 },
            exporterPdfTableStyle: { margin: [0, 60, 30, 0] },
            exporterPdfTableHeaderStyle: { fontSize: 12, bold: true },
            exporterPdfHeader: { text: "Audit Report", style: 'headerStyle' },
            exporterPdfFooter: {
                columns: [{ text: 'Date: ____/_______/2015' }, { text: 'Signature: ______________', alignment: 'right' }]
            },
            exporterPdfCustomFormatter: function (docDefinition) {
                docDefinition.styles.headerStyle = { fontSize: 22, bold: true, margin: [35, 10, 0, 0] };
                docDefinition.styles.footerStyle = { fontSize: 10, bold: true, margin: [35, 0, 35, 0] };
                return docDefinition;
            },
            exporterPdfOrientation: 'landscape',
            exporterFieldCallback: function (grid, row, col, input) {
                if (col.name === 'dateStored' || col.name === "depositorDob") {
                    return moment(input).format("MM/DD/YYYY");
                } else {
                    return input;
                }
            },
            exporterAllDataFn: function () {

            },
            onRegisterApi: function (gridApi) {
                $scope.gridApi = vm.gridApi = gridApi;
                gridApi.expandable.on.rowExpandedStateChanged($scope, function (row) {
                    if (!row.isExpanded) {
                        _.remove(selectedSubrows, function (item) {
                            return item.sampleId === row.entity.id;
                        });
                        isSubRowSelected();
                    } else if (!row.entity.subGridOptions) {
                        row.isExpanded = false;
                    }
                });
            }
        };

        getData();

        function getData() {
            return sampleService.getAllReportSamples(true)
            .then(function (data) {
                $scope.gridOptions.totalItems = data.length;
                _.map(data, function (item) {
                    item = initSubGrid(item);
                    item.locationsCount = item.locations.length;
                    return item;
                });
                $scope.gridOptions.data = data;
            });
        };

        function initSubGrid(item) {
            if (!item.locations || !item.locations.length) return item;
            var locations = _.map(item.locations, function (loc) {
                loc.sampleId = item.id;
                return loc;
            });
            var subGridOptions = {
                columnDefs: [
                    { name: 'uniqName' },
                    { name: 'collectionMethod' },
                    { name: 'dateStored' }
                ],
                enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
                enableGridMenu: true,
                data: locations,
                onRegisterApi: function (gridApi) {
                    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                        if (row.isSelected) {
                            selectedSubrows.push(row.entity);
                        } else {
                            _.remove(selectedSubrows, row.entity);
                        }
                        isSubRowSelected();
                    });
                }
            };
            item.subGridOptions = subGridOptions;
            return item;
        }


        function randomReport() {
            var itemsToSelect = _.sample($scope.gridOptions.data, 10);
            _.forEach(itemsToSelect, $scope.gridApi.selection.selectRow);
            $scope.gridApi.exporter.pdfExport(uiGridExporterConstants.SELECTED, uiGridExporterConstants.VISIBLE);
            $timeout(function () {
                _.forEach(itemsToSelect, function (item) {
                    $scope.gridApi.selection.unSelectRow(item);
                });
            }, 100);
        }

        function printAllRows() {
            var columnState = getColumnsState();
            $scope.gridApi.exporter.pdfExport(uiGridExporterConstants.ALL, columnState);
        }

        function printSelectedRows() {
            var columnState = getColumnsState();
            $scope.gridApi.exporter.pdfExport(uiGridExporterConstants.SELECTED, columnState);
        }

        function printVisibleRows() {
            var columnState = getColumnsState();
            $scope.gridApi.exporter.pdfExport(uiGridExporterConstants.VISIBLE, columnState);
        }

        function getColumnsState() {
            return vm.printVisibleColumns ? uiGridExporterConstants.VISIBLE : uiGridExporterConstants.ALL;
        }

        function isRowSelected() {
            return $scope.gridApi.grid.selection.selectedCount === 1;
        }

        function isSubRowSelected() {
            if (!selectedSubrows.length) {
                vm.isSubRowsSelected = false;
                return false;
            }
            var sampleId = selectedSubrows[0].sampleId;
            vm.isSubRowsSelected = _.all(selectedSubrows, { "sampleId": sampleId });
        }

        function editSample() {
            var selectedRow = getSelectedRows()[0];
            $state.go("sample.edit", { id: selectedRow.id });
        }

        function newSample() {
            $state.go("sample");
        }

        function getSelectedRows() {
            return $scope.gridApi.selection.getSelectedRows();
        }

        function getSelectedSubRows() {
            return $scope.subGridApi && $scope.subGridApi.selection.getSelectedRows();
        }

        function printSample(ev) {
            $scope.sample = getSelectedRows()[0];
            $timeout(function() {
                $scope.$root.print();
                $scope.sample = {};
            }, 100);
        }

        function extractSamples(ev) {
            $mdDialog.show({
                controller: DialogController,
                templateUrl: '/app/samples/extract.dialog.html',
                parent: angular.element(document.body),
                targetEvent: ev
            })
            .then(function (reason) {
                var promises = [];
                var locations = selectedSubrows;
                if (!locations.length) return false;
                var sample = _.find(vm.gridApi.grid.rows, function (item) {
                    return item.entity.id === locations[0].sampleId;
                });
                if (!sample) return false;
                sample = angular.copy(sample.entity);
                sample.reason = reason;
                sample.locations = locations;

                $scope.sample = sample;
                $scope.sample.printTitle = "Extract";

                $timeout(function () {
                    $scope.$root.print();
                    _.forEach(locations, function (item) {
                        promises.push(sampleService.removeLocation(item.id));
                    });

                    $q.all(promises).then(function () {
                        $scope.gridApi.selection.clearSelectedRows();
                        getData(1, $scope.gridOptions.paginationPageSize);
                    });
                });

                
            }, function () { });
        }


        function DialogController($scope, $mdDialog) {
            $scope.reasons = [];
            $scope.reason = {};
            adminService.getReasons().then(function (data) {
                $scope.reasons = data;
            });

            $scope.cancel = function () {
                $mdDialog.cancel();
            };
            $scope.extract = function () {
                $mdDialog.hide($scope.reason);
            };
        }
    };
})();