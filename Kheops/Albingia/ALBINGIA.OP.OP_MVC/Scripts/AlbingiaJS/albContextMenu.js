
var ContextMenu = function () {
    //----------Créer le menu nouveau--------------
    this.createMenuNouveau = function (type, etat, brche) {
        if ($("#btnActionNouveau").length > 0) {
            $("#btnActionNouveau").attr("albcontextmenu", "N");
            $.contextMenu('destroy', '#btnActionNouveau');

            $.ajax({
                type: "POST",
                url: "/ContextMenu/GetMenuNouveau",
                async: false,
                data: { type: type == "P" ? "C" : type, etat: etat, brche: brche },
                success: function (data) {
                    if (data != undefined && data != "") {
                        $("#btnActionNouveau").attr("albcontextmenu", "O");
                        common.$initContextMenu("#btnActionNouveau", "", data);
                    }
                    else {
                        $("#btnActionNouveau").attr("disabled", "disabled").hide();
                    }
                },
                error: function (request) {
                    common.error.showXhr(request, true);
                }
            });
        }
    };

    //----------Créer le menu avenant--------------
    this.createMenuAvenant = function (type, etat, brche, offrePeriodicite, offreSituation, typeAccord, numAvt, typeAvt) {
        if ($("#btnAvenant").exists()) {
            $("#btnAvenant").attr("albcontextmenu", "N");
            $.contextMenu('destroy', '#btnAvenant');
            $.ajax({
                type: "POST",
                url: "/ContextMenu/GetMenuAvenant",
                async: false,
                data: {
                    type: type == $("#TypeMenuPolice").val() ? $("#TypeMenuContrat").val() : type, etat: etat, brche: brche, situation: offreSituation,
                    periodicite: offrePeriodicite, typeAccord: typeAccord, numAvt: numAvt, typeAvt: typeAvt
                },
                success: function (data) {
                    if (data && data !== "{}") {
                        $("#btnAvenant").attr("albcontextmenu", "O");
                        common.$initContextMenu("#btnAvenant", "", data);
                    }
                    else {
                        $("#btnAvenant").attr("disabled", "disabled").hide();
                    }
                },
                error: function (request) {
                    common.error.showXhr(request, true);
                }
            });
        }
    };

    //------------Créer le menu sur le bouton "menu"
    this.createMenuPrincipalOnButton = function (val, offreType, offreEtat, offreSituation, offrePeriodicite, offreCopyOffre, modeNavig, brche, typeAccord, generdoc, typeAvt, kheopsStatut) {
        if ($("#btnActionMenu").length > 0) {
            $("#btnActionMenu").attr("albcontextmenu", "N");
            $("tr[albcontextmenu]").attr("albcontextmenu", "N");
            $.contextMenu('destroy', '#btnActionMenu');

            var isBlackedList = false;
            if ($('#PreneurAssurance_Numero').val() != undefined || $('#PreneurAssurance_CodePreneurAssurance').val() != undefined) {
                isBlackedList = true;
            }

            $.ajax({
                type: "POST",
                url: "/ContextMenu/GetContextMenu",
                async: false,
                data: {
                    type: offreType, etat: offreEtat, situation: offreSituation, periodicite: offrePeriodicite, copyOffre: offreCopyOffre,
                    modeNavig: modeNavig, brche: brche, typeAccord: typeAccord, generdoc: generdoc, typeAvt: typeAvt, numAvt: val.split('_')[3], isBlackListedSeach: isBlackedList
                },
                success: function (data) {
                    if (data) {

                        if (kheopsStatut == "KHE") {
                            $("tr[albcontext=" + val + "]").attr("albcontextmenu", "O");
                            common.$initContextMenu("tr[albcontextmenu=O]", "right", data, true);
                        }

                        if (offreEtat == 'V' || data.toLowerCase().indexOf("modifier") >= 0) {
                            $("#btnCreerOffre").removeAttr("disabled");
                        }
                        $("#btnActionMenu").attr("albcontextmenu", "O");
                        common.$initContextMenu("#btnActionMenu", "", data);
                    }
                },
                error: function (request) {
                    common.error.showXhr(request, true);
                }
            });
        }
    };

    this.triggerMenuAction = function (action, options) {   
        switch (action) {
            case "creersaisie":
                CreerSaisie();
                break;
            case "creercontrat":
                CreerContrat();
                break;
            case "consulter":
                recherche.affaires.results.consultOrEdit("btnFolderReadOnly");
                break;
            case "openoffre":
                recherche.affaires.results.consultOrEdit("btnCreerOffre");
                break;
            case "doublesaisie":
                DoubleSaisie(options);
                break;
            case "etabliran":
                if ($("input[type='radio'][name='RadioRow']:checked").attr("albkheopsstatut") === "KHE") {
                    EtablirAN();
                } else {
                    common.dialog.error("Vous ne pouvez pas établir d'affaire nouvelle à partir d'une offre CITRIX.");
                }
                break;
            case "valideroffre":
                recherche.affaires.results.edit("Validation");
                break;
            case "copyoffre":
                CopyOffre();
                break;
            case "refuser":
                Refuser();
                break;
            case "engagementperiode":
                OpenEngagementPeriode();
                break;
            case "blocdebloc1":
            case "blocdebloc2":
            case "blocdebloc3":
                recherche.affaires.results.edit(action);
                break;
            case "historique":
                OpenVisuHistorique($('input[type=radio][name=RadioRow]:checked').val());
                //OpenHistorique();
                break;
            case "AVNMD":
                CreateAvntModif();
                break;
            case "AVNRS":
                CreateAvntResil();
                break;
            case "AVNRM":
                CreateAvntRemiseEnVigueur();
                break;
            case "AVNPG":
                //TODO ECM : avenant de prorogation
                common.dialog.show("En cours de réalisation");
                break;
            case "AVNSP":
                //TODO ECM : avenant de suspension
                common.dialog.show("En cours de réalisation");
                break;
            case "clausier":
                AddClause();
                break;
            case "txtlibre":
                AddClauseLibre();
                break;
            case "piecejointe":
                AddPieceJointe();
                break;
            case "updateformule":
                //MenuUpdateFormule();
                matricesAffaire.matriceFormule.menuActions.updateFormule();
                break;
            case "consultformule":
                //MenuConsultFormule();
                matricesAffaire.matriceFormule.menuActions.consultFormule();
                break;
            case "conditionformule":
                //MenuOpenCondition();
                matricesAffaire.matriceFormule.menuActions.consultConditions();
                break;
            case "copyFormule":
                ////MenuDuplicateFormule();
                matricesAffaire.matriceFormule.menuActions.duplicateFormule();
                break;
            case "deleteFormule":
                //MenuDeleteFormule();
                matricesAffaire.matriceFormule.menuActions.deleteFormule();
                break;
            case "createOption":
                matricesAffaire.matriceFormule.menuActions.addOption();
                break;
            case "updateOption":
                matricesAffaire.matriceFormule.menuActions.updateOption();
                break;
            case "duplicateOption":
                matricesAffaire.matriceFormule.menuActions.duplicateOption();
                break;
            case "deleteOption":
                matricesAffaire.matriceFormule.menuActions.deleteOption();
                break;
            case "QUITTVISU":
                OpenVisulisationQuittances('Toutes', isEntete = false);
                break;
            case "REPVAL":
                RepriseContrat();
                break;
            case "suividoc":
                OpenGestionDoc('AFF');
                break;
            case "PRENDREPOSITION":
                recherche.affaires.results.consultOrEdit("PrisePosition");
                break;
            case "saveInven":
                ClicHeaderInven();
                break;
            case "delInven":
                DeleteLineInventaire($("#InvenContextMenuId").val());
                break;
            case "addInven":
                $("#addInventaire").trigger('click');
                break;
            case "HORSAVENANT":
                recherche.affaires.results.consultOrEdit("btnMHA");
                break;
            case "REGUL":
                OpenRegulPage();
                break;
            case "PB":
                OpenPBPage();
                break;
            case "BNS":
                OpenBNSPage();
                break;
            case "connexites":
                contextMenu.showConnexites();
                break;
            case "retourpieces":
                OpenRetoursPieces();
                break;
            default:
                common.dialog.info("En cours de développement.");
                break;
        }
    };

    //------------Créer le menu contextuel pour les clauses-----------
    this.createContextMenuClause = function () {
        //if ($("#btnCtxtMenuClause").attr("id") != undefined) {
        $.contextMenu('destroy', '#btnCtxtMenuClause');
        $.ajax({
            type: "POST",
            url: "/ContextMenu/GetMenuClause",
            async: false,
            data: { type: $("#Offre_Type").val(), user: $("#NameUser").val() },
            success: function (data) {
                if (data != undefined && data != "") {
                    //$("#btnCtxtMenuClause").attr("albcontextmenu", "O");
                    common.$initContextMenu("img[id=btnCtxtMenuClause]", "", data);
                }
            },
            error: function (request) {
                common.error.showXhr(request, true);
            }
        });
        //}
    };
    //---------Créer le menu contextuel pour les formules------
    this.createContextMenuFormule = function (codeFormule, blockConditions, suppFormule, isSorti) {
        var readOnly = $("#OffreReadOnly").val();
        if ($("#IsModifHorsAvn").val() == "True") {
            readOnly = $("#IsModifHorsAvn").val();
            blockConditions = "True";
        }
        $("td[id='formuleHover" + codeFormule + "']").attr("albcontextmenu", "N");
        $.contextMenu("destroy", "tr[albcontextmenu=O]");
        $.ajax({
            type: "POST",
            url: "/ContextMenu/GetMenuFormule",
            async: false,
            data: { type: $("#Offre_Type").val(), user: $("#NameUser").val(), readOnly: readOnly, blockConditions: blockConditions, suppFormule: suppFormule, isSorti: isSorti },
            success: function (data) {
                if (data != undefined && data != "") {
                    $("td[id='formuleHover" + codeFormule + "']").attr("albcontextmenu", "O");
                    common.$initContextMenu("td[id=formuleHover" + codeFormule + "]", "", data);
                }
            },
            error: function (request) {
                common.error.showXhr(request, true);
            }
        });

    };
    //------------Créer le menu contextuel pourles inventaires------------
    this.createContextMenuInventaire = function (invenId) {//, isOpen) {
        $("tr[albNameContext='inventaire']").attr("albcontextmenu", "N");
        $.contextMenu("destroy", "tr[albcontextmenu=O]");
        $.ajax({
            type: "POST",
            url: "/ContextMenu/GetMenuInventaire",
            async: false,
            data: { type: $("#Offre_Type").val(), user: $("#NameUser").val(), invenId: invenId }, //isOpen: isOpen },
            success: function (data) {
                //$("#EditInventaireId").val(invenId);
                $("tr[albNameContext='inventaire']").attr("albcontextmenu", "O");
                common.$initContextMenu("tr[albNameContext=inventaire]", "right", data);

            },
            error: function (request) {
                common.error.showXhr(request, true);
            }
        });
    };
    //--------Créer le menu contextuel pour le header de l'inventaire------
    this.createContextMenuHeaderInven = function () {
        $("tr[albNameContext='headerinven']").attr("albcontextmenu", "N");
        $.contextMenu("destroy", "tr[albcontextmenu=O]");
        $.ajax({
            type: "POST",
            url: "/ContextMenu/GetMenuHeaderInventaire",
            async: false,
            data: { type: $("#Offre_Type").val(), user: $("#NameUser").val() },
            success: function (data) {
                $("tr[albNameContext='headerinven']").attr("albcontextmenu", "O");
                common.$initContextMenu("tr[albNameContext='headerinven']", "right", data);
            },
            error: function (request) {
                common.error.showXhr(request, true);
            }
        });
    };

    this.showConnexites = function () {
        let selectRadio = $("input[type='radio'][name='RadioRow']:checked");
        if (!selectRadio.val()) {
            return false;
        }
        let infoRows = $(selectRadio).val().split("_");
        let offreNum = infoRows[0];
        let offreVersion = infoRows[1];
        let offreType = infoRows[2];
        let numAvenant = infoRows[3];
        //const numAvnExt = selectRadio.attr("albnumavnext");
        //let addParam = "";
        //if (numAvenant != "0")
        //    addParam = "addParam" + $("#globalTypeAddParamAvn").val() + "|||" + $("#globalTypeAddParamAvn").val() + "ID|" + numAvenant + "||AVNTYPE|" + selectRadio.attr("albtypeavt") + "||AVNIDEXTERNE|" + numAvnExt + "addParam";
        //var valRedirect = offreNum + "_" + offreVersion + "_" + offreType + $("#homeTabGuid", window.parent.document).val() + addParam + "modeNavig" + $("#ModeNavig").val() + "modeNavigConsultOnly";


        affaire.showConnexites(
            "Connexités",
            {
                modeNavig: 'S', isReadonly: false, affair: { codeOffre: offreNum, version: offreVersion, type: offreType, tabGuid: "" }
            });
    };
};

var contextMenu = new ContextMenu();