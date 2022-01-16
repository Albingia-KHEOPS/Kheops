var files = [];

$(document).ready(function () {
    // Init autocomplete 
    MapCommonAutoCompCourtierGestion();
    MapCommonAutoCompCourtierPayeur();
    MapCommonAutoCompAssure();
    
    if ($("#messageErreur").val() != "" && $("#messageErreur").val() != null) {
        ShowMessage($("#messageErreur").val());
    }

    $("#Offre, #NumAliment").change(function () {
        ShowLoading();
        var offreId = $("#Offre").val();
        var version = $("#NumAliment").val() == "" ? "0" : $("#NumAliment").val();
        $.ajax({
            type: "GET",
            url: "/ExcelContrat/GetOffre",
            data: { offreId: offreId, version: version },
            success: function (dataReturn) {
                if (dataReturn.Offre != null) {
                    $("#Designation").val(dataReturn.Offre.Descriptif);
                    $("#drlBranches").val(dataReturn.Offre.Branche).change();
                    setTimeout(function () { $("#drlCibles").val(dataReturn.Offre.Cible) }, 500);
                    $("#SouscripteurCode").val(dataReturn.Offre.SouscripteurCode);
                    $("#SouscripteurNom").val(dataReturn.Offre.SouscripteurCode + " - " + dataReturn.Offre.SouscripteurNom);
                    $("#SouscripteurActive").val(dataReturn.SouscripteurActive);
                    $("#GestionnaireCode").val(dataReturn.Offre.GestionnaireCode);
                    $("#GestionnaireNom").val(dataReturn.Offre.GestionnaireCode + " - " + dataReturn.Offre.GestionnaireNom);
                    $("#GestionnaireActive").val(dataReturn.GestionnaireActive);
                    if (dataReturn.Offre.DateEffetJour != 0 && dataReturn.OffreDateEffetMois != 0 && dataReturn.Offre.DateEffetAnnee != 0) {
                        $("#DateEffetDebut").val(dataReturn.Offre.DateEffetJour.toString().padStart(2, '0') + "/"
                            + dataReturn.Offre.DateEffetMois.toString().padStart(2, '0') + "/"
                            + dataReturn.Offre.DateEffetAnnee.toString().padStart(4, '0'));
                    } else {
                        $("#DateEffetDebut").val("");
                    }
                    if (dataReturn.Offre.FinEffetJour != 0 && dataReturn.Offre.FinEffetMois != 0 && dataReturn.Offre.FinEffetAnnee != 0) {
                        $("#DateEffetFin").val(dataReturn.Offre.FinEffetJour.toString().padStart(2, '0') + "/"
                            + dataReturn.Offre.FinEffetMois.toString().padStart(2, '0') + "/"
                            + dataReturn.Offre.FinEffetAnnee.toString().padStart(4, '0'));
                    } else {
                        $("#DateEffetFin").val("");
                    }
                    $("#Periodicite").val(dataReturn.Offre.PeriodiciteCode + " - " + dataReturn.Offre.PeriodiciteNom);
                    if (dataReturn.Offre.Jour != 0 && dataReturn.Offre.Mois != 0) {
                        $("#DateEcheance").val(dataReturn.Offre.Jour.toString().padStart(2, '0') + "/"
                            + dataReturn.Offre.Mois.toString().padStart(2, '0'));
                    } else {
                        $("#DateEcheance").val("");
                    }
                    if (dataReturn.Offre.CourtierGestionnaire != 0) {
                        $("#CourtierGestionnaire_Courtier_CodeCourtier").val(dataReturn.Offre.CourtierGestionnaire).change();
                        //$("#CourtierGestionnaire_Courtier_NomCourtier").val(dataReturn.Offre.NomCourtierGest);
                    } else {
                        $("#CourtierGestionnaire_Courtier_CodeCourtier").val("").change();
                        //$("#CourtierGestionnaire_Courtier_NomCourtier").val("");
                    }
                    $("#GpIdentique").prop("checked", false).change();
                    if (dataReturn.Offre.CourtierApporteur != 0) {
                        $("#CourtierApporteur_Courtier_CodeCourtier").val(dataReturn.Offre.CourtierApporteur).change();
                        //$("#CourtierApporteur_Courtier_NomCourtier").val(dataReturn.Offre.NomCourtierAppo);
                    } else {
                        $("#CourtierApporteur_Courtier_CodeCourtier").val("").change();
                        //$("#CourtierApporteur_Courtier_NomCourtier").val("");
                    }
                    if (dataReturn.Offre.CourtierPayeur != 0) {
                        $("#CourtierPayeur_Courtier_CodeCourtier").val(dataReturn.Offre.CourtierPayeur).change();
                        //$("#CourtierPayeur_Courtier_NomCourtier").val(dataReturn.Offre.NomCourtierPayeur);
                    } else {
                        $("#CourtierPayeur_Courtier_CodeCourtier").val("").change();
                        //$("#CourtierPayeur_Courtier_NomCourtier").val("");
                    }
                    if (dataReturn.Offre.CodePreneurAssurance != 0) {
                        $("#PreneurAssurance_Numero").val(dataReturn.Offre.CodePreneurAssurance);
                        $("#PreneurAssurance_Nom").val(dataReturn.Offre.NomPreneurAssurance);
                    } else {
                        $("#PreneurAssurance_Numero").val("");
                        $("#PreneurAssurance_Nom").val("");
                    }

                    if (dataReturn.Offre.AdresseContrat != null) {
                        $("#NumeroVoie").val(dataReturn.Offre.AdresseContrat.NumeroVoie);
                        $("#ExtensionVoie").val(dataReturn.Offre.AdresseContrat.ExtensionVoie);
                        $("#NomVoie").val(dataReturn.Offre.AdresseContrat.NomVoie);
                        $("#CodePostal").val(dataReturn.Offre.AdresseContrat.Departement.toString().padStart(2, '0') + dataReturn.Offre.AdresseContrat.CodePostal.toString().padStart(3, '0'));
                        $("#Ville").val(dataReturn.Offre.AdresseContrat.NomVille);
                    }
                    else {
                        $(".adresseInput").val("");
                    }
                } else {
                    $("#btnInitialize").click();
                }
                CloseLoading();
            },
            error: function (request) {
                common.error.showXhr(request);
                CloseLoading();
            }
        });
    });

    $("#drlBranches").on("change", function () {
        var branche = $(this).val();
        $.ajax({
            type: "GET",
            url: "/ExcelContrat/GetCibles",
            data: { branche: branche },
            success: function (dataReturn) {
                var cibleHtml = ""
                for (var i = 0; i < dataReturn.length; i++) {
                    cibleHtml += "<option value='" + dataReturn[i].Value + "'>" + dataReturn[i].Text + "</option>"
                }
                $("#drlCibles").html(cibleHtml);
            },
            error: function (request) {
                common.error.showXhr(request);
            }
        });
    });

    $("#GpIdentique").change(function () {
        var checked = $(this).is(':checked');
        if (checked) {
            var code = $("#CourtierGestionnaire_Courtier_CodeCourtier").val();
            var nom = $("#CourtierGestionnaire_Courtier_NomCourtier").val();
            var classAttr = $("#CourtierInvalideGestionnaireDiv").attr("class");
            var text = $("#CourtierInvalideGestionnaireDiv").text();
            var invalidValue = $("#inInvalidCourtierGest").val();

            $("#CourtierApporteur_Courtier_CodeCourtier").val(code).attr('readonly', 'readonly').addClass('readonly');
            $("#CourtierApporteur_Courtier_NomCourtier").val(nom).attr('readonly', 'readonly').addClass('readonly');
            $("#CourtierInvalideDiv").attr("class", classAttr);
            $("#CourtierInvalideDiv").text(text);
            $("#inInvalidCourtierApp").val(invalidValue);
            $("#CourtierPayeur_Courtier_CodeCourtier").val(code).attr('readonly', 'readonly').addClass('readonly');
            $("#CourtierPayeur_Courtier_NomCourtier").val(nom).attr('readonly', 'readonly').addClass('readonly');
            $("#CourtierInvalidePayeurDiv").attr("class", classAttr);
            $("#CourtierInvalidePayeurDiv").text(text);
            $("#inInvalidCourtierPay").val(invalidValue);
        }
        else {
            $("#CourtierApporteur_Courtier_CodeCourtier").removeAttr('readonly');
            $("#CourtierApporteur_Courtier_CodeCourtier").removeClass('readonly');
            $("#CourtierApporteur_Courtier_NomCourtier").removeAttr('readonly');
            $("#CourtierApporteur_Courtier_NomCourtier").removeClass('readonly');
            $("#CourtierPayeur_Courtier_CodeCourtier").removeAttr('readonly');
            $("#CourtierPayeur_Courtier_CodeCourtier").removeClass('readonly');
            $("#CourtierPayeur_Courtier_NomCourtier").removeAttr('readonly');
            $("#CourtierPayeur_Courtier_NomCourtier").removeClass('readonly');
        }
    });

    $("#btnInitialize").click(function () {
        $("#drlCibles").html("");
        $("#formExcelContrat").find("input, select").each(function () {
            $(this).val("");
        });
    });

    $("#btnLoad").click(function () {
        ShowLoading();
        $.ajax({
            url: '/ExcelContrat/LoadExcelContrat',
            type: 'POST',
            success: function (data) {
                CloseLoading();
                ShowMessage(data);
            },
            error: function (error) {
                CloseLoading();
                ShowMessage("Une erreur est survenue.");
            }
        });
    });

    //------------------------------------------------------------------------------
    //Initialisation dropzone fichier
    //------------------------------------------------------------------------------
    
    $(".dropzone").on("dragover", function (event) {
        event.preventDefault();
        event.stopPropagation();
        $(this).addClass("dragover");
    });
    $(".dropzone").on("dragleave", function (event) {
        event.preventDefault();
        event.stopPropagation();
        $(this).removeClass("dragover");
    });
    $(".dropzone").bind("drop", function (event) {
        event.preventDefault();
        event.stopPropagation();
        $(this).removeClass("dragover");

        for (var i = 0; i < event.originalEvent.dataTransfer.files.length; i++) {
            var formData = new FormData();
            formData.append("file", event.originalEvent.dataTransfer.files[i]);
            var filename = event.originalEvent.dataTransfer.files[i].name;
            $.ajax({
                type: "POST",
                url: "/ExcelContrat/UploadTempFiles",
                data: formData,
                async: false,
                processData: false,
                contentType: false,
                success: function () {
                    // Use DataTransfer interface to access the file(s)
                    if (files.indexOf(filename) < 0) {
                        files.push(filename);
                        $(".dropzone_list_file").append("<div><strong class='FloatLeft' title='" + filename + "'>" + filename + "</strong> <img class='FloatRight' src='/Content/Images/cancel.png'></div>")
                    }
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        $(".dropzone_nb_file strong").text($(".dropzone_list_file").find("div").length);
    });

    $("#file").change(function (event) {
        for (var i = 0; i < this.files.length; i++) {
            var formData = new FormData();
            formData.append("file", this.files[i]);
            var filename = this.files[i].name;
            $.ajax({
                type: "POST",
                url: "/ExcelContrat/UploadTempFiles",
                data: formData,
                async: false,
                processData: false,
                contentType: false,
                success: function () {
                    // Use DataTransfer interface to access the file(s)
                    if (files.indexOf(filename) < 0) {
                        files.push(filename);
                        $(".dropzone_list_file").append("<div><strong class='FloatLeft' title='" + filename + "'>" + filename + "</strong> <img class='FloatRight' src='/Content/Images/cancel.png'></div>")
                    }
                },
                error: function (request) {
                    common.error.showXhr(request);
                }
            });
        }
        $(".dropzone_nb_file strong").text($(".dropzone_list_file").find("div").length);
    });

    $(".dropzone_list_file").on("click", "img", function () {
        var file = $(this).closest("div");
        var filename = file.find("strong").text();
        files.splice(files.indexOf(filename), 1);
        file.remove();
        $(".dropzone_nb_file strong").text($(".dropzone_list_file").find("div").length);
    });
    //------------------------------------------------------------------------------
    // END Initialisation dropzone fichier
    //------------------------------------------------------------------------------
});

function ShowMessage(data) {
    var type = data.toLowerCase().indexOf("erreur") < 0 ? "Info" : "Error";
    $("#msg" + type).html(data);
    $.fancybox($("#fancy" + type).html(), {
        'autoDimensions': true,
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedin': 400,
        'speedOut': 400,
        'easingOut': 'easeInBack',
        'modal': true,
    });
}

function validateField() {
    ShowLoading();
    var valid = true;
    if ($("#Designation").val() == "") {
        $("#Designation").addClass("requiredField");
        valid = false;
    } else {
        $("#Designation").removeClass("requiredField");
    }
    $(".inputNomCourtier, .inputAnCodeCourtier").each(function () {
        if ($(this).val() == "" || $(this).closest(".HeightRow").prev(".isInvalidCourtier").val() == 1) {
            $(this).addClass("requiredField");
            valid = false;
        } else {
            $(this).removeClass("requiredField");
        }
    });
    if ($("#drlBranches").val() == "") {
        $("#drlBranches").addClass("requiredField");
        valid = false;
    } else {
        $("#drlBranches").removeClass("requiredField");
    }
    if ($("#drlCibles").val() == "" || $("#drlCibles").val() == null) {
        $("#drlCibles").addClass("requiredField");
        valid = false;
    } else {
        $("#drlCibles").removeClass("requiredField");
    }

    if ($("#SouscripteurNom").val() == "" || $("#SouscripteurActive").val() == "N") {
        $("#SouscripteurNom").addClass("requiredField");
        valid = false;
    } else {
        $("#SouscripteurNom").removeClass("requiredField");
    }
    if ($("#GestionnaireNom").val() == "" || $("#GestionnaireActive").val() == "N") {
        $("#GestionnaireNom").addClass("requiredField");
        valid = false;
    } else {
        $("#GestionnaireNom").removeClass("requiredField");
    }
    if ($("#DateEffetDebut").val() == "") {
        $("#DateEffetDebut").addClass("requiredField");
        valid = false;
    } else {
        $("#DateEffetDebut").removeClass("requiredField");
    }

    if (["U", "E"].indexOf($("#Periodicite").val().split(" - ")[0]) > -1) {
        $("#DateEcheance").removeClass("requiredField");
        if ($("#DateEffetFin").val() == "") {
            $("#DateEffetFin").addClass("requiredField");
            valid = false;
        } else {
            $("#DateEffetFin").removeClass("requiredField");
        }
    } else {
        $("#DateEffetFin").removeClass("requiredField");
        if ($("#DateEcheance").val() == "") {
            $("#DateEcheance").addClass("requiredField");
            valid = false;
        } else {
            $("#DateEcheance").removeClass("requiredField");
        }
    }

    if ($("#DateEcheance").val() != "") {
        if (!isDate($("#DateEcheance").val() + "/2012") || $("#DateEcheance").val().length != 5) {
            $("#DateEcheance").addClass('requiredField');
            valid = false;
        } else {
            $("#DateEcheance").removeClass("requiredField");
        }
    }

    if (!valid) CloseLoading();
    $("#Files").val(files.join("*"));
    return valid;
}

function UpdateCourtiersIfChecked() {
    if ($("#GpIdentique").is(":checked")) {
        var code = $("#CourtierGestionnaire_Courtier_CodeCourtier").val();
        var nom = $("#CourtierGestionnaire_Courtier_NomCourtier").val();
        var classAttr = $("#CourtierInvalideGestionnaireDiv").attr("class");
        var text = $("#CourtierInvalideGestionnaireDiv").text();
        var invalidValue = $("#inInvalidCourtierGest").val();

        $("#CourtierApporteur_Courtier_CodeCourtier").val(code);
        $("#CourtierApporteur_Courtier_NomCourtier").val(nom);
        $("#CourtierInvalideDiv").attr("class", classAttr);
        $("#CourtierInvalideDiv").text(text);
        $("#inInvalidCourtierApp").val(invalidValue);

        $("#CourtierPayeur_Courtier_CodeCourtier").val(code);
        $("#CourtierPayeur_Courtier_NomCourtier").val(nom);
        $("#CourtierInvalidePayeurDiv").attr("class", classAttr);
        $("#CourtierInvalidePayeurDiv").text(text);
        $("#inInvalidCourtierPay").val(invalidValue);
    }
}