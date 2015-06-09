(function () {
    angular.module("seaman.samples")
        .controller("SamplesListController", samplesListController);

    samplesListController.$inject = ['$scope', 'uiGridConstants', 'sampleService', '$q', '$state', '$mdDialog', "adminService", '$timeout', 'uiGridExporterConstants'];

    function samplesListController($scope, uiGridConstants, sampleService, $q, $state, $mdDialog, adminService, $timeout, uiGridExporterConstants) {
        var vm = this;
        vm.title = "Samples";

        vm.printVisibleColumns = true;
        vm.isRowSelected = isRowSelected;
        vm.editSample = editSample;
        vm.newSample = newSample;
        vm.extract = extractSamples;
        vm.isRowsSelected = isRowsSelected;
        vm.printSample = printSample;
        vm.randomReport = randomReport;
        vm.printAllRows = printAllRows;
        vm.printSelectedRows = printSelectedRows;
        vm.printVisibleRows = printVisibleRows;

        $scope.gridOptions = {
            showGridFooter: true,
            useExternalSorting: true,
            enableHorizontalScrollbar: uiGridConstants.scrollbars.NEVER,
            enableFiltering: true,
            enableSorting: false,
            exporterMenuCsv: false,
            minRowsToShow: 20,
            columnDefs: [
              { name: 'depositorFullName' },
              { name: "depositorDob" },
              { name: 'locations' },
              { name: 'comment' },
              { name: 'physician' },
              { name: 'collectionMethod' },
              { name: 'dateStored' },
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
                $scope.gridApi = gridApi;
            }
        };

        var getData = function () {
            return sampleService.getAllReportSamples(true)
            .then(function (data) {
                $scope.gridOptions.totalItems = data.length;
                $scope.gridOptions.data = data;
            });
        };
        getData();

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
            var columns = getCoumnsState();
            $scope.gridApi.exporter.pdfExport(uiGridExporterConstants.ALL, columns);
        }

        function printSelectedRows() {
            var columns = getCoumnsState();
            $scope.gridApi.exporter.pdfExport(uiGridExporterConstants.SELECTED, columns);
        }

        function printVisibleRows() {
            var columns = getCoumnsState();
            $scope.gridApi.exporter.pdfExport(uiGridExporterConstants.VISIBLE, columns);
        }

        function getCoumnsState() {
            return vm.printVisibleColumns ? uiGridExporterConstants.VISIBLE : uiGridExporterConstants.ALL;
        }

        function isRowSelected() {
            return $scope.gridApi.grid.selection.selectedCount === 1;
        }

        function isRowsSelected() {
            return $scope.gridApi.grid.selection.selectedCount > 0;
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

        function printSample() {
            var selectedRow = getSelectedRows()[0];
            $scope.$root.printSample(selectedRow.id, function () {
                $state.go("samples");
            });
        }

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