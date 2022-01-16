
(function () {
    if (!ko) return;

    let buildUrl = function (name, configuration, filename) {
        if (typeof name !== "string") {
            throw "name: invalid type";
        }
        if (!name) {
            throw "name must contain characters";
        }
        if (typeof configuration !== "object" || configuration == null) {
            throw "configuration is invalid";
        }

        if (filename) {
            return "/knockout/components/" + name + "/" + filename + "?version=" + configuration.version;
        }
        else {
            return configuration.fileUrl
                || ("/knockout/components/" + name + "/" +
                    (configuration.filename || ((configuration.fullQualifiedFilenames ? (name + ".") : "") + "template.html")) +
                    "?version=" + configuration.version);
        }
    };

    let templateFromUrlLoader = {
        loadTemplate: function (name, templateConfig, callback) {
            if (templateConfig.version) {
                // Uses jQuery's ajax facility to load the markup from a file
                var fullUrl = buildUrl(name, templateConfig);
                
                if (Array.isArray(templateConfig.extraFiles) || templateConfig.hasItsOwnCss) {
                    let templates = [];
                    let urls = [fullUrl];
                    if (templateConfig.hasItsOwnCss) {
                        urls.push(name + ".css");
                    }
                    $.when.apply(
                        null,
                        urls.concat(templateConfig.extraFiles || []).map(function (file, index) {
                            let url = index == 0 ? file : buildUrl(name, templateConfig, file);
                            return $.get(url, function (markupString) {
                                if (file.match(/\.css$/ig)) {
                                    templates[0] = ('<style type="text/css">' + markupString + "</style>") + templates[0];
                                }
                                else {
                                    templates.push(markupString);
                                }
                            });
                        })).done(function () {
                            ko.components.defaultLoader.loadTemplate(name, templates.join("").replace(/>\s+</g, "><"), callback);
                        });
                }
                else {
                    $.get(fullUrl, function (markupString) {
                        // We need an array of DOM nodes, not a string.
                        // We can use the default loader to convert to the
                        // required format.
                        ko.components.defaultLoader.loadTemplate(name, markupString.replace(/>\s+</g, "><"), callback);
                    });
                }
            } else {
                // Unrecognized config format. Let another loader handle it.
                callback(null);
            }
        }
    };

    // Register it
    ko.components.loaders.unshift(templateFromUrlLoader);
})();
