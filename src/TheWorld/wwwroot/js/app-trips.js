// app-trip.js
(function () {
    // Create the app-trips module
    angular.module("app-trips", ["simpleControls", "ngRoute"])
          .config(function ($routeProvider) {

              $routeProvider.when("/", {
                  controller: "tripsController",
                  controllerAs: "vm",
                  templateUrl: "/views/tripsView.html"
              });

              // A route to handle a single trip based on which trip is selected
              $routeProvider.when("/editor/:tripName", {
                  controller: "tripEditorController",
                  controllerAs: "vm",
                  templateUrl: "/views/tripEditorView.html"
              });

              $routeProvider.otherwise({ redirectTo: "/" });
        });
})();