(function () {
    angular.module("seaman.reports")
        .controller("ReportsController", reportsController);

    reportsController.$inject = ["$scope", "adminService", 'sampleService', 'uiGridExporterConstants', "$timeout"];

    function reportsController($scope, adminService, sampleService, uiGridExporterConstants, $timeout) {
        $scope.toggleMin = function () {
            $scope.maxDate = $scope.maxDate ? null : new Date();
        };
        $scope.toggleMin();

        var vm = this;
        var nullOpt = [{ name: "Empty" }];
        vm.title = "Reports";
        vm.tanks = [];
        vm.physicians = [];
        vm.collectionMethods = [];
        vm.types = ["Existing", "Extracted", "Missing Data", "All"];
        vm.reportModel = {};
        vm.reportModel.type = "Existing";
        vm.reportModel.startDate = moment().subtract(100, 'day').format("MM/DD/YYYY");
        vm.startDateDatepickerOpened = false;
        vm.openStartDateDatepicker = openStartDateDatepicker;
        vm.endDateDatepickerOpened = false;
        vm.openEndDateDatepicker = openEndDateDatepicker;
        vm.frozenStartDateDatepickerOpened = false;
        vm.openFrozenStartDateDatepicker = openFrozenStartDateDatepicker;
        vm.frozeEendDateDatepickerOpened = false;
        vm.openFrozenEndDateDatepicker = openFrozenEndDateDatepicker;
        vm.generateReport = generateReport;
        vm.randomReport = randomReport;
        vm.onTankChange = onTankChange;
        vm.gridOptions = {
            exporterMenuCsv: false,
            minRowsToShow: 20,
            columnDefs: [
                { name: "uniqName" },
                { name: "dateStored" },
                { name: "dateFrozen" },
                { name: "collectionMethod" },
                { name: 'depositorFullName' },
                { name: "depositorDob" },
                { name: 'physician' },
                { name: 'extracted' }
            ],
            exporterPdfDefaultStyle: { fontSize: 10 },
            exporterPdfTableStyle: { margin: [0, 60, 30, 0] },
            exporterPdfTableHeaderStyle: { fontSize: 12, bold: true },
            exporterPdfHeader: { text: "Guided report", style: 'headerStyle' },
            exporterPdfFooter: {
                columns: [{ text: 'Date: ' + moment().format("MM/DD/YYYY"), style: 'footerStyle' }, { text: 'Signature: ______________', alignment: 'right', style: 'footerStyle' }]
            },
            exporterPdfCustomFormatter: function (docDefinition) {
                docDefinition.styles.headerStyle = { fontSize: 22, bold: true, margin: [35, 10, 0, 0] };
                docDefinition.styles.footerStyle = { fontSize: 16, bold: true, margin: [35, 0, 35, 0] };
                return docDefinition;
            },
            exporterPdfOrientation: 'landscape',
            onRegisterApi: function (gridApi) {
                vm.gridApi = gridApi;
            }
        };
        activate();

        function activate() {
            adminService.getTanks(true).then(function (data) {
                vm.tanks = nullOpt.concat(data);
            });

            adminService.getPhysician(true).then(function (data) {
                vm.physicians = nullOpt.concat(data);
            });

            adminService.getCollectionMethods(true).then(function(data) {
                vm.collectionMethods = data;
            });
        }

        function openStartDateDatepicker(e) {
            e.preventDefault();
            e.stopPropagation();
            vm.startDateDatepickerOpened = true;
        }

        function openEndDateDatepicker(e) {
            e.preventDefault();
            e.stopPropagation();
            vm.endDateDatepickerOpened = true;
        }

        function openFrozenStartDateDatepicker(e) {
            e.preventDefault();
            e.stopPropagation();
            vm.frozenStartDateDatepickerOpened = true;
        }

        function openFrozenEndDateDatepicker(e) {
            e.preventDefault();
            e.stopPropagation();
            vm.frozenEndDateDatepickerOpened = true;
        }

        function onTankChange() {
            getCanisters();
        }

        function getCanisters() {
            if (!vm.reportModel.tankId) return false;
            var tank = _.find(vm.tanks, { "id": +vm.reportModel.tankId });
            if (!tank) return false;
            vm.canisters = [];
            for (var i = 1; i <= tank.canistersCount; i++) {
                vm.canisters.push(i);
            }
        }

        function generateReport() {
            return sampleService.generateReport(vm.reportModel).then(recieved);

            function recieved(data) {
                vm.gridOptions.exporterPdfHeader.text = "Guided report";

                if (data.length > 400) {
                    alert("Too many records. Only first 400 records are shown. Please refine your filters and try again.");
                    data = data.slice(0, 400);
                }
                vm.gridOptions.data = data;                
                
                $timeout(function() {
                    vm.gridApi.exporter.pdfExport(uiGridExporterConstants.ALL, uiGridExporterConstants.ALL);
                    vm.gridOptions.data = [];
                });
            }
        }

        function randomReport() {
            return sampleService.generateRandomReport(10).then(recieved);

            function recieved(data) {
                vm.gridOptions.exporterPdfHeader.text = "Random Audit Report";
                vm.gridOptions.data = data;
                $timeout(function () {
                    vm.gridApi.exporter.pdfExport(uiGridExporterConstants.ALL, uiGridExporterConstants.ALL);
                    vm.gridOptions.data = [];
                });
            }
        }
    }
})();