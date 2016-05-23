angular.module('umbraco')
    .service('webresultService',
        function ($q, $http, localizationService, urlService) {
            this.GetResult = function (editorState, url) {
                var deferred = $q.defer();

                if (!editorState.template) {
                    deferred.reject(localizationService.localize("error_no_template"));
                } else {
                    var relativeUrl = editorState.urls[0];

                    if (relativeUrl == "This item is not published") {
                        deferred.reject(localizationService.localize("error_not_published"));
                    } else {

                        var fullUrl = urlService.GetUrl(relativeUrl);

                        url = url.replace("{url}", fullUrl);

                        $http({ method: 'GET', url: url })
                            .then(function (response) {
                                    if (response.data && response.status == 200) {
                                        deferred.resolve(response.data);
                                    } else {
                                        deferred.reject(localizationService.localize("error_page_error", [response.status]));
                                    }
                                },
                            function (response) {
                                deferred.reject(response.data.Message);
                            });
                    }
                    return deferred.promise;
                };
            }
        });