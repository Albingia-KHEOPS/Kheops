
(function () {
    var viewModelCustomLoader = {
        loadViewModel: function (name, viewModelConfig, callback) {
            var skipLoader = true;

            if (viewModelConfig.vmCtor) {
                skipLoader = false;
                var ViewModelFactory = viewModelConfig.vmCtor;
                var useDefaultLoader = true;
                if (viewModelConfig.loadEmptyObject || viewModelConfig.loadPrototypes) {
                    if (typeof viewModelConfig.ajaxUrl === "string") {
                        useDefaultLoader = false;
                        $.get(viewModelConfig.ajaxUrl).done(function (data) {
                            callback(function (p, info) {
                                return new ViewModelFactory(p, data);
                            });
                        });
                    }
                }

                if (useDefaultLoader) {
                    ko.components.defaultLoader.loadViewModel(name, ViewModelFactory, callback);
                }
            }

            if (skipLoader) {
                // Unrecognized config format. Let another loader handle it.
                callback(null);
            }
        }
    };

    // Register it
    ko.components.loaders.unshift(viewModelCustomLoader);
}) ();
