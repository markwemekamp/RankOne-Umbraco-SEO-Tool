angular.module('umbraco')
    .service('urlService', ['$location',
        function ($location) {

            this.GetUrl = function (relativeUrl) {
                var url = $location.protocol() + "://" + $location.host();

                if ($location.port() != 80) {
                    url = url + ":" + $location.port();
                }

                url += relativeUrl;

                return url;
            };
        }]);