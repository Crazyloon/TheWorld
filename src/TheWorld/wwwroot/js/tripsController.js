// tripsController.js
(function () {
    "use strict";

    // Get existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    // much like constructor DI angular allows DI by adding dependancies to the controller functions input ($http)
    function tripsController($http) {
        var vm = this; // View Model

        vm.trips = [];

        vm.newTrip = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips") //returns a promise object
            .then(function (response) { 
                // Success
                // copy the data returned the api/trips into our vm.trips object
                // we could also use a for each but .copy is quick and easy for us
                angular.copy(response.data, vm.trips); // response.data already converted from JSON to an object graph for us
            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to load data: " + error;
            })
        .finally(function () {
            vm.isBusy = false;
        });

        vm.addTrip = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
                .then(function (response) {
                    // Success
                    vm.trips.push(response.data);
                    vm.newTrip = {};
                }, function (error) {
                    // Failure
                    vm.errorMessage = "Failed to save new trip";
                })
                .finally(function () {
                    vm.isBusy = false;
                });

        };
    }


})();