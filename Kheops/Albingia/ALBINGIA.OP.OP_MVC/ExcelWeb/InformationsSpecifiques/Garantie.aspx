<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Garantie.aspx.cs" Inherits="ALBINGIA.OP.OP_MVC.ExcelWeb.InformationsSpecifiques.Garantie" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" media="screen" href="../../Content/_Site.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/FormCommon.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/vInformationsSpecifiques.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/themes/base/jquery.ui.all.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/themes/base/jquery-ui.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/themes/base/jquery.ui.dialog.css" />
    <script language="javascript" type="text/javascript" src="../../Scripts/Jquery/jquery-1.4.4.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/Jquery/jquery-ui-1.8.16.min.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/Jquery/date-fr-FR.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/AlbingiaJS/albInformationsSpecifiques.js"></script>
</head>
<body>
    <input type="hidden" id="fileName" clientidmode="Static" runat="server" />
    <input type="hidden" id="jsSplitChar" clientidmode="Static" runat="server" />
    <input type="hidden" id="branche" clientidmode="Static" runat="server" />
    <input type="hidden" id="section" clientidmode="Static" runat="server" />
    <input type="hidden" id="parameters" clientidmode="Static" runat="server" />
    <input type="hidden" id="cellsMap" clientidmode="Static" runat="server" />
    <input id="split_const_html" type="hidden" value="<%= ALBINGIA.OP.OP_MVC.MvcApplication.SPLIT_CONST_HTML %>" />
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td colspan="2" class="titre">Garantie ANNULATION</td>
            </tr>
            <tr>
                <td colspan="2" class="titre2">ANNULATION : </td>
            </tr>
            <tr>
                <td>Tout seul / Périls dénommés</td>
                <td>
                    <select id="map_ToutSaufPerilDenomme" albrequired="N">
                        <option value="TS">Tout sauf</option>
                        <option value="PD">Périls dénommés</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Attentats : durée 45 jours</td>
                <td><input id="map_Attantat45j" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td class="sub">Autre durée (jours)</td>
                <td><input id="map_AutreDuree" class="datepicker" data-val="true" data-val-regex="La date doit suivre la forme 24/11/2030" data-val-regex-pattern="^\d{2}/\d{2}/\d{4}$" type="text" value="31/10/2012" albrequired="O" /></td>
            </tr>
            <tr>
                <td>Durée de couverture</td>
                <td><input id="map_DureeCouverture" type="text" value="TRUE" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Nature des montants assurés</td>
                <td><textarea id="map_NatureMontanAssuree" albrequired="N"></textarea></td>
            </tr>
            <tr>
                <td>Définitions des pertes prévues</td>
                <td><input id="map_DefinitionPertesPrevues" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Exclusion pandémie</td>
                <td><input id="map_ExclusionPandemie" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Retransmission en direct</td>
                <td><input id="map_RetransmissionDirect" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rupture de faisceau</td>
                <td><input id="map_RuptureFaisceau" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Clause de sortie TV</td>
                <td><input id="map_CaluseSortieTV" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Pénurie forcée du public</td>
                <td><input id="map_PenurieForceePublic" type="checkbox" checked albrequired="N" /></td>
            </tr>
            <tr>
                <td>Annulation suite accident endeuillant</td>
                <td><input id="map_AnnulationSuiteAccidentEndeuillant" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Plein air (sans intempéries)</td>
                <td><input id="map_PlainAirSansIntemperies" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Frais supplémentaires</td>
                <td><input id="map_FraisSupplementaire" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Type de garantie</td>
                <td>
                    <select id="map_TypeGarantie" albrequired="N">
                        <option value="P">Plus</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="titre2">SPECIFIQUE AUDIOVISUEL : </td>
            </tr>
            <tr>
                <td>Rachat des stop dates</td>
                <td><input id="map_RachatStopDates" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Participation à des scènes dangereuses</td>
                <td><input id="map_ParticipationScenesDangereuses" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat exclusion pilotage</td>
                <td><input id="map_RachatExclusionPilotage" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat exclusion compétition</td>
                <td><input id="map_RachatExclusionCompetition" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat exclusion sport aérien</td>
                <td><input id="map_RachatExclusionSportAerien" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td colspan="2" class="titre2">INDISPONIBILITE : </td>
            </tr>
            <tr>
                <td>Deuil familial</td>
                <td><input id="map_DeuilFamilial" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td class="sub">Deuil familial étendu</td>
                <td><input id="map_DeuilFamilialEtendue" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Indisponibilité simultanée</td>
                <td><input id="map_IndisponibiliteSimultannee" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Indisponibilité suite matériel volé</td>
                <td><input id="map_IndisponibiliteMaterielVole" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Nature de l'indisponibilité de</td>
                <td><textarea id="map_NatureIndisponibilitePersonne" albrequired="N"></textarea></td>
            </tr>
            <tr>
                <td>Indisponibilité suite retard transport</td>
                <td><input id="map_IndisponibiliteRetardTransport" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Contrôle médical non reçu</td>
                <td><input id="map_ControlMedicalNonRecu" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Clause 30 jours si indisponibilité</td>
                <td><input id="map_Clause30jIndisp" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Attestation de bonne santé</td>
                <td><input id="map_AttestationBonneSante" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Personnes de + de 65 ans</td>
                <td><input id="map_PersonnePlus65" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Personnes de - de 16 ans</td>
                <td><input id="map_PersonneMoins16" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat excl malad. infantiles</td>
                <td><input id="map_RachatExclMaldInfantiles" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Indisponbilité équipe technique</td>
                <td><input id="map_IndiponibiliteEquipeTechnique" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td colspan="2" class="titre2">INTEMPERIES : </td>
            </tr>
            <tr>
                <td>Intempéries étendues</td>
                <td><input id="map_Intemperiesetendues" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Intempéries tournées batiment</td>
                <td><input id="map_IntemperiesTourneesBatiment" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Intempéries sans huissier</td>
                <td><input id="map_IntemperiesSansHuissier" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Intempéries feu artifice</td>
                <td><input id="map_IntemperieFeuArtifice" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Intempéries régates</td>
                <td><input id="map_IntemperiesRegates" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Intempéries croisière</td>
                <td><input id="map_IntemperiesCroisiere" type="checkbox" albrequired="N" /></td>
            </tr>
        </table>
    </div>
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td colspan="2" class="titre">Bonification</td>
            </tr>
            <tr>
                <td>Bonification en cas de non sinistre</td>
                <td><input id="map_BonificationCasNonSinistre" type="checkbox" checked albrequired="N" /></td>
            </tr>
            <tr>
                <td>Montant (%)</td>
                <td><input id="map_MontantBon" type="text" value="10" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Bonification anticipée</td>
                <td><input id="map_BonificationAnticipe" type="checkbox" checked albrequired="N" /></td>
            </tr>
        </table>
    </div>
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td colspan="2" class="titre">Garantie DOMMAGE</td>
            </tr>
            <tr>
                <td colspan="2" class="titre2">RACHATS : </td>
            </tr>
            <tr>
                <td>Rachat bijoux</td>
                <td><input id="map_RachatBijoux" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td class="sub">Montant rachat bijoux (U.M.)</td>
                <td><input id="map_MontantRachatBijoux" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat fourrures</td>
                <td><input id="map_RachatFourrure" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td class="sub">Montant rachat fourrures (U.M.)</td>
                <td><input id="map_MontantRachatFourrure" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat bijoux et fourrures</td>
                <td><input id="map_RachatBijouxFourrure" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Ecrancs, plasma, LCD</td>
                <td><input id="map_EcranPlasmaLcd" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat exclusion bris fragiles</td>
                <td><input id="map_RachatExclusionBrisFragiles" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Chapiteaux</td>
                <td><input id="map_Chapitaux" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat dommages électriques</td>
                <td><input id="map_RachatDommageElec" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat bris fonctionnel</td>
                <td><input id="map_RachatBrisFonctionnel" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Rachat exclusion bris</td>
                <td><input id="map_RachatExclusionBris" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Intempéries</td>
                <td><input id="map_RachatIntemperies" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Transport</td>
                <td><input id="map_RachatTransport" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td colspan="2" class="titre2">PREVENTIONS : </td>
            </tr>
            <tr>
                <td>Sécurité des batiments</td>
                <td><input id="map_SecuriteBatiments" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Prévention montage/démontage</td>
                <td><input id="map_PreventionMonDemont" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Prévention vêtements cuirs et peaux</td>
                <td><input id="map_PreventionVetmCuirPeaux" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Prévention ordinateurs portables</td>
                <td><input id="map_PrevOrdiPort" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Gardiennage</td>
                <td><input id="map_Gardiennages" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Prévention instruments de musique</td>
                <td><input id="map_PreventionInstrumentsMusiques" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Clause de surveillance</td>
                <td><input id="map_ClauseSurveillance" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Surveillance petits objets</td>
                <td><input id="map_SurveillancePetitsObjets" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Surveillance objets de valeur</td>
                <td><input id="map_SurveillanceObjetsValeurs" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Surv. exclusion perte / disparition</td>
                <td><input id="map_SurvExclusionPerteDisp" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Surv. manque inventaire</td>
                <td><input id="map_SurvManqueInvent" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td colspan="2" class="titre2">EXCLUSIONS : </td>
            </tr>
            <tr>
                <td>Exclusion vol véhicule stationné</td>
                <td><input id="map_ExclusionVolVehiculeStaionne" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Exclusion vol ouverture</td>
                <td><input id="map_ExclusionVolOuverture" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Exclusion instruments de musique</td>
                <td><input id="map_ExclusionInstrumentMusique" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Exclusion vélos</td>
                <td><input id="map_ExclusionVelo" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Exclusion matériel photo, ciné, son</td>
                <td><input id="map_ExclusionMatMultimedia" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Exclusion ordinateurs portables</td>
                <td><input id="map_ExclusionOrdinateurPortable" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Exclusion structures légères</td>
                <td><input id="map_ExclusionStructureLegere" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Clause Canon & exclusion</td>
                <td><input id="map_ClauseCanonExclusion" type="checkbox" albrequired="N" /></td>
            </tr>
        </table>
    </div>
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td colspan="2" class="titre">Garantie INDIVIDUELLE ACCIDENT</td>
            </tr>
            <tr>
                <td>Garantie étendue</td>
                <td><input id="map_GarantieEtendueIndAcc" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Désignation</td>
                <td><textarea id="map_DesignationIndivAccident" albrequired="N">Tests</textarea></td>
            </tr>
            <tr>
                <td>Capital décès (U.M.)</td>
                <td><input id="map_CapitalDeces" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Capital IP (U.M.)</td>
                <td><input id="map_CapitalIP" type="text" albrequired="N" /></td>
            </tr>
        </table>
    </div>
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td colspan="2" class="titre">Garantie RESPONSABILITE CIVILE</td>
            </tr>
            <tr>
                <td>Garantie étendue</td>
                <td><input id="map_GarantieEtendueRespCiv" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Désignation</td>
                <td><input id="map_DesignationRespCivile" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Tribunes démontables</td>
                <td><input id="map_TribunesDemontables" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Prises de vue à risque</td>
                <td><input id="map_PriseVueRisque" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Scènes dangereuses</td>
                <td><input id="map_SceneDangereuse" type="checkbox" albrequired="N" /></td>
            </tr>
        </table>
    </div>
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td>Montant par élève</td>
                <td><input id="map_MontantParEleve" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Seuil interruption de scolarité (mois)</td>
                <td><input id="map_SeuilInterruptionScolarite" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Seuil redoublement (mois)</td>
                <td><input id="map_SeuilRedoublement" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Seuil cours particuliers (mois)</td>
                <td><input id="map_SeuilsCoursParticuliers" type="text" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Perte de revenus</td>
                <td><input id="map_PerteRevenue" type="checkbox" albrequired="N" /></td>
            </tr>
        </table>
    </div>
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td>Renonciation à recours réciproque</td>
                <td><input id="map_RenonciaRecourRecip" type="checkbox" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Renonciation à recours</td>
                <td><input id="map_RenonciaRecour" type="checkbox" albrequired="N" /></td>
            </tr>
        </table>
    </div>
</body>
</html>