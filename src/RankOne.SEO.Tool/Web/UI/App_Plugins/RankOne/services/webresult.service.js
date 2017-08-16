angular.module('umbraco')
    .service('webresultService',
        function ($q, $http, localizationService) {
            this.GetResult = function (url) {
                var deferred = $q.defer();
                $http({ method: 'GET', url: url })
                    .then(function (response) {
                        if (response.data && response.status === 200) {
                            deferred.resolve(response.data);
                        } else {
                            deferred.reject(localizationService
                                .localize("error_page_error", [response.status]));
                        }
                    },
                    function (response) {
                        deferred.reject(response.data);
                    });

                return deferred.promise;
            },
            this.GetResultFromEditorState = function (editorState, url) {
                var deferred = $q.defer();

                url = url.replace("{id}", editorState.id);

                this.GetResult(url)
                    .then(function (response) {
                        deferred.resolve(response);
                    },
                    function (response) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            };
        });