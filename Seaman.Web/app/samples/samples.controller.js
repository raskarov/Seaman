(function () {
    angular.module("seaman.samples")
        .controller("SamplesListController", samplesListController);

    samplesListController.$inject = ['$scope', 'uiGridConstants', 'sampleService', '$q', '$state', '$mdDialog', "adminService", '$timeout'];

    function samplesListController($scope, uiGridConstants, sampleService, $q, $state, $mdDialog, adminService, $timeout) {
        var vm = this;
        vm.title = "Records";

        var selectedSubrows = [];
        vm.gridApi = {};
        vm.isRowSelected = isRowSelected;
        vm.editSample = editSample;
        vm.newSample = newSample;
        vm.extract = extractSamples;
        vm.isRowsSelected = false;
        vm.isSubRowsSelected = false;
        vm.printSample = printSample;
        vm.import = importSamples;

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
              { name: "locationsCount", maxWidth: 120, displayName: "No. of Samples", enableFiltering: false },
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
                columns: [{ text: 'Date: ____/_______/2015', style: 'footerStyle' }, { text: 'Signature: ______________', alignment: 'right', style: 'footerStyle' }]
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
            return sampleService.getSamples(true)
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
                    { name: 'uniqName', displayName: "Stored Location" },
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
    
        function printSample(ev) {
            $scope.sample = getSelectedRows()[0];
            $timeout(function() {
                $scope.$root.print();
                $scope.sample = {};
            }, 100);
        }

        function extractSamples(ev) {
            $mdDialog.show({
                controller: ["$scope", "$mdDialog", DialogController],
                templateUrl: '/app/samples/extract.dialog.html',
                parent: angular.element(document.body),
                targetEvent: ev
            })
            .then(function (result) {
                var locations = selectedSubrows;
                if (!locations.length) return false;
                var sample = _.find(vm.gridApi.grid.rows, function (item) {
                    return item.entity.id === locations[0].sampleId;
                });
                if (!sample) return false;
                sample = angular.copy(sample.entity);
                sample.reason = result.reason.name;
                sample.locations = locations;

                $scope.sample = sample;
                $scope.sample.printTitle = "Extract";

                $timeout(function () {
                    $scope.$root.print();
                    var model = {
                        locationIds: _.map(locations, "id"),
                        sampleId: sample.id,
                        consentFormName: result.uploadedFile,
                        reasonId: result.reason.id
                    };
                    sampleService.extract(model).then(function() {
                        getData();
                    });
                });

                
            }, function () { });
        }


        function DialogController(scope, $mdDialog) {
            scope.reasons = [];
            scope.model = {};
            adminService.getReasons().then(function (data) {
                scope.reasons = data;
            });
            scope.$on("cfpLoadingBar:started", function () {
                scope.showProgress = true;
            });

            scope.$on("cfpLoadingBar:completed", function () {
                scope.showProgress = false;
            });

            scope.upload = upload;

            scope.cancel = function () {
                $mdDialog.cancel();
            };
            scope.extract = function () {
                var reason = _.find(scope.reasons, { 'id': +scope.model.reason });
                var result = {
                    reason: reason,
                    uploadedFile: scope.uploadedFile
                };
                $mdDialog.hide(result);
            };

            function upload(e, files) {
                if (!files.length) return false;
                if (!selectedSubrows.length) return false;
                var sampleId = selectedSubrows[0].sampleId;
                sampleService.uploadConsentForm(files, sampleId).success(function(data) {
                    scope.uploadedFile = data;
                });
            }
        }

        function importSamples(ev) {
            $mdDialog.show({
                controller: ["$scope", "$mdDialog", ImportController],
                templateUrl: '/app/samples/import.dialog.html',
                parent: angular.element(document.body),
                targetEvent: ev
            }).then(function() {
                getData();
            });
        }

        function ImportController(scope, $mdDialog) {
            scope.$on("cfpLoadingBar:started", function () {
                scope.showProgress = true;
            });

            scope.$on("cfpLoadingBar:completed", function () {
                scope.showProgress = false;
            });

            scope.upload = upload;

            scope.cancel = cancel;

            function upload(e, files) {
                if (!files.length) return false;
                sampleService.importSamples(files).success(function (data) {
                    scope.uploadedFile = data;
                });
            }

            function cancel() {
                $mdDialog.cancel();
            };
        }
    };
})();