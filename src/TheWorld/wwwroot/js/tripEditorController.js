// tripEditorController.js
(function () {
    "use strict";
    
    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http){
        var vm = this;

        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newStop = {};

        var stopsUrl = "/api/trips/" + vm.tripName + "/stops"

        $http.get(stopsUrl)
            .then(function (response) {
                //success
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);
            }, function (err) {
                //failure
                vm.errorMessage = "Failed to load stops";

            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addStop = function () {
            vm.isBusy = true;


            $http.post(stopsUrl, vm.newStop)
                .then(function (response) {
                    //success
                    vm.stops.push(response.data);
                    _showMap(vm.stops);
                    vm.newStop = {};
                },
                function (err) {
                    //failure
                    vm.errorMessage = "Failed to add new stop";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        }
    }

    // private function for use within this file only
    function _showMap(stops) {
        if (stops && stops.length > 0) {
            var mapStops = _.map(stops, function (item) {

                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });

            // Show the map
            travelMap.createMap({
                stops: mapStops,
                selector: "#map", // the css selector to look for for placing the map on the page
                currentStop: 1,
                initialZoom: 3
            });
        }
    }

})();