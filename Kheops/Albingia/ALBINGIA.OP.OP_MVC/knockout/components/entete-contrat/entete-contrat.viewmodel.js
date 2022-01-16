(function () {
    let viewModelEnteteContrat = function (parameters, emptyEntete) {
        let vm = this;
        try {
            ko.mapping.fromJS(emptyEntete, {}, this);
            $(document).trigger(customEvents.entete.initializing);
            this.isReadonly = ko.observable(true);
            this.showError = function (messages) {
                if (messages) {
                    common.dialog.bigError($.isArray(messages) ? messages.join('\n') : messages, true);
                }
            };
            this.changeAttestationTabStyle = function (isCollapsed) {
                let alertAttestationTab = $(".dvAlertAttes");
                let garantieTab = $(".divGarantie");
                let rsqObjTab = $(".divRsqObj");
                if (isCollapsed) {
                    alertAttestationTab.css("height", alertAttestationTab.data("max-height") + "px");
                    garantieTab.css("max-height", garantieTab.data("max-height") + "px");
                    rsqObjTab.css("max-height", rsqObjTab.data("max-height") + "px");
                }
                else {
                    alertAttestationTab.css("height", alertAttestationTab.data("min-height") + "px");
                    garantieTab.css("max-height", garantieTab.data("min-height") + "px");
                    rsqObjTab.css("max-height", rsqObjTab.data("min-height") + "px");
                }
            };
            this.displayOrNotDetailedInfo = function (vm, event) {
                let content = document.getElementById("contentDetailedContratInfo");
                let infoContratDiv = $("#infosContrat")[0];
                let divHeight = 0;
                let isAttestation = $("div.albAttestationInfos").length != 0;
                if (content.style.display === "block") {
                    infoContratDiv.className = infoContratDiv.className.replace("divInfoContratDetailedDisplay", "divInfoContratRegularisation");
                    content.style.display = "none";
                    let parent = $(event.target).parents("entete-contrat").parent().get(0);
                    divHeight = parent.clientHeight - $("#infosContrat").outerHeight();
                    parent.lastElementChild.style.height = divHeight + "px";
                    $("#imgExpandInfo").attr("src", "/Content/Images/expand.png");
                    if (isAttestation) {
                        vm.changeAttestationTabStyle(true);
                    }
                }
                else {
                    infoContratDiv.className = infoContratDiv.className.replace("divInfoContratRegularisation", "divInfoContratDetailedDisplay");
                    content.style.display = "block";
                    let parent = $(event.target).parents("entete-contrat").parent().get(0);
                    divHeight = parent.clientHeight - $("#infosContrat").outerHeight() - 12;
                    parent.lastElementChild.style.height = divHeight + "px";
                    $("#imgExpandInfo").attr("src", "/Content/Images/collapse.png");
                    if (isAttestation) {
                        vm.changeAttestationTabStyle(false);
                    }
                }
            };
            this.init = function () {
                common.$postJson("/Folder/GetContratBandeau", { contrat: currentAffair, isHisto: false })
                    .fail(function (x, s, e) {
                        console.error(e);
                        common.error.show(x);
                        $(document).trigger(customEvents.entete.error);
                    })
                    .done(function (data) {
                        try {
                            ko.mapping.fromJS(data, {}, vm);
                            $(document).trigger(customEvents.entete.loaded);
                        }
                        catch (mapError) {
                            console.error(mapError);
                            $(document).trigger(customEvents.entete.error);
                        }
                    });
            };

            this.init();
        } catch (e) {
            console.error(e);
            common.error.showMessage(e.message);
            $(document).trigger(customEvents.entete.error);
        }
    };

    affaire.contrat.entete.ViewModel = viewModelEnteteContrat;
})();
