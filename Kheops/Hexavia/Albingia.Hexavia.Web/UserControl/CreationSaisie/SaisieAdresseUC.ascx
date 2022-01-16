<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SaisieAdresseUC.ascx.cs"
    Inherits="Albingia.Hexavia.Web.UserControl.SaisieAdresseUC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<telerik:RadScriptBlock ID="RadCodeBlock1" runat="server" OnLoad="Page_Load">
    <style type="text/css">
        .RowMouseOver td
        {
            background-color: lightblue !important;
        }
        .RowMouseOut
        {
            /*this style is taken from the corresponding skin's GridRow_[SkinName] class - GridRow_Default in our case*/
            background: #f7f7f7;
        }
    </style>
    <script type="text/javascript">
        val = null;

        function verifieN(object) {
            var valObject = object.value;

            if (valObject == "" || typeof (valObject) == "undefined") {
                valObject = object.val();
            }

            if (valObject && valObject.length > 0) {
                var firstletter = valObject.charAt(0);
                var lastletter = valObject.charAt(valObject.length - 1);
                if (valObject.length > 4) {
                    alert("Le numéro d'extension doit être sur quatre positions");
                    if (object.value == "" || typeof (object.value) == "undefined") {
                        object.val('');
                    } else {
                        object.value = '';
                        
                    }
                    object.focus();
                    return false;
                }
                numcheck = /\d/;
                for (i = 0; i <= valObject.length - 1; i++) {

                    if (!numcheck.test(valObject.charAt(i))) {
                        alert("Le format du numéro n'est pas valide, ce champ doit être numérique");
                        if (object.value == "" || typeof (object.value) == "undefined") {
                            object.val('');
                        } else {
                            object.value = '';
                            
                        }
                        object.focus();
                        return false;
                    }
                } 
                
//                if (!numcheck.test(firstletter) || !numcheck.test(lastletter)) {
//                    alert("Le format du numéro n'est pas valide, ce champ doit commencer et terminer par un chiffre");
//                    object.value = '';
//                    object.focus();
//                    return false;
//                }
            }
            return true;
        }

        function getAdresse() {
            adresse = new Object();
            adresse.batiment = $('#<%= inBatiment.ClientID%>').val();
            adresse.numero = $('#<%= inNumero.ClientID%>').val();
            adresse.extension = $("select[name='<%= ddlExtension.UniqueID%>'] option:selected").html();
            adresse.distribution = $('#<%= inDistribution.ClientID%>').val();
            adresse.ville = $('#<%= inVille.ClientID%>').val();
            adresse.cp = $('#<%= txtCp.ClientID%>').val();
            adresse.pays = new Object();
            adresse.pays.libelle = $('#<%= ddlPays.ClientID%>').val();
            adresse.voie = $('#<%= inVoie.ClientID%>').val();
            var cpville = $('#<%= txtCpVille.ClientID%>').val();
            var villeCedex = $('#<%= inVilleCedex.ClientID%>').val();
            adresse.cpville = '';
            adresse.villeCedex = '';
            adresse.matriculeHexavia = $("#<%= matriculeHexavia.ClientID%>").val();
            if (cpville != null) {
                adresse.cpville = cpville;
            }
            if (villeCedex != null) {
                adresse.villeCedex = villeCedex;
            }
            return adresse;
        }

        // fonction appelé sur le click validation + renvoi de l'adresse à un système externe (GRP)
        function openConfirm() {
            $('#<%= adress.ClientID%>').val('');
            if ($('#<%= ddlPays.ClientID%>').val() == "France") {
                var adresse = null;
                var villeHexavia = $('#<%= inVille.ClientID%>').val();
                var cpHexavia = $('#<%= txtCp.ClientID%>').val();
                
                
                if (!verifieN($('#<%= inNumero.ClientID%>'))) {
                    return false;
                }

                if (villeHexavia == "" && cpHexavia == "") {
                    
                    //test si la ville ou le departement ont été saisie par l'utilisateur
                    if (confirm('Le code département et/ou le nom de la ville ne sont pas renseignés. Continuez tout de même?', null, 330, 100, null, 'Informations incomplètes')) {
                        adresse = confirmCallBackFn(true);
                    }
                }
                else {
                    adresse = confirmCallBackFn(true);
                    //alert("sort de confirmcallbackfn()");
                }

                return ChainageAdresse(adresse);
            }
            else {
                alert("La validation d'adresse n'est possible que sur les adresses en France");
            }
            // dans les autres cas on retourne false
            return false;
        }

        function alimenteAdresse() {
            var adresse = getAdresse();
            var divCedex = $('#divcedex');
            //var statutValidationAdresse = 0;


            /*if (adresse.cpville == null && adresse.villeCedex == null) {
            divCedex.hide();
            }
            else {
            divCedex.show();
            }*/
            return adresse;
            //var matriculeHexavia = $('#<%=matriculeHexavia.ClientID%>');

            /*if (matriculeHexavia.val() != '' && matriculeHexavia.val() != 0) {
            statutValidationAdresse.val("Adresse validée");
            }
            else {
            statutValidationAdresse.val("Adresse non validée");
            }*/
            //RadwindowClose();
        }

        // confirmation de la validité de l'adresse (matricule Hexavia doit avoir un numéro de la base sinon invalide -> valeur 0 ou vide)
        function confirmCallBackFn(arg) {
            var adresse;
            if (arg) {
                var matriculeHexavia = $('#<%=matriculeHexavia.ClientID%>');
                if (matriculeHexavia.val() == '' || matriculeHexavia.val() == 0) {
                    if (confirm('L\'adresse que vous avez saisie n\'est pas validée. Continuez tout de même ?', null, 330, 100, null, 'Adresse non validée')) {
                        adresse = adresseCallBackFn(true);
                    }
                }
                else {
                    adresse = adresseCallBackFn(true);
                }

                return adresse;
            }
        }

        function adresseCallBackFn(arg) {
            if (arg) {
                //alert("passe dans alimenteAdresse()");
                return alimenteAdresse();
            }
            else return false;
        }

        function ChainageAdresse(adresse) {
            if (adresse != null) {
                // si l'adresse est valide on la renvoie
                if (adresse.ville != undefined || adresse.cp != undefined) {
                    var sep = '¤';
                    var adresseValideText = 'NON_VALIDE';
                    if ($('#<%= hfAdresseValide.ClientID%>').val() == 1) {
                        adresseValideText = "VALIDE";
                    }
                    // GRP ajout du cp et ville dans les champs cedex si vide pour la géolocalisation grp
                    /*var villegeoloc = "";
                    var cpgeoloc = "";
                    if (adresse.villeCedex == "" && adresse.cpville == "") {
                    villegeoloc = adresse.ville;
                    cpgeoloc = adresse.cp;
                    }
                    else {
                    villegeoloc = adresse.villeCedex;
                    cpgeoloc = adresse.cpville;
                    }*/
                    //

                    var retourAdresse = adresse.batiment.toUpperCase() + sep + adresse.numero.toUpperCase() + sep + adresse.extension + sep + adresse.voie.toUpperCase() + sep + adresse.distribution.toUpperCase() + sep + adresse.villeCedex.toUpperCase() + sep + adresse.cpville.toUpperCase() + sep + adresse.ville.toUpperCase() + sep + adresse.cp.toUpperCase() + sep + adresse.pays.libelle.toUpperCase() + sep + adresseValideText + sep + adresse.matriculeHexavia;
                    $('#<%= adress.ClientID%>').val(retourAdresse);
                    var retourQuery='?Data=adresse=' + retourAdresse;
                    //window.location = 'http://' + $('#<%= urlExterieur.ClientID%>').val() + '?adresse=' + retourAdresse;
                    window.location = 'http://' + $('#<%= urlExterieur.ClientID%>').val() + retourQuery;

                    //Test parent reussi

                    //val("OUAI");
                    /*try {
                    window.opener.fonctionaappeler(retourAdresse);
                    }
                    catch (e) {
                    alert('Mode Pop-up HEXAVIA non trouvé, erreur technique: ' + e);
                    }*/
                    /*if (val != null) {
                    val(retourAdresse);
                    }*/

                    return retourAdresse;
                }
            }
        }

        function SaisieAdresse() {
            $('#<%= adress.ClientID%>').val('');
            $("#<%= matriculeHexavia.ClientID %>").val('');
            if ($('#<%= ddlPays.ClientID%>').val() == "France") {
                var adresse = null;
                var villeHexavia = $('#<%= inVille.ClientID%>').val();
                var cpHexavia = $('#<%= txtCp.ClientID%>').val();
                if (!verifieN($('#<%= inNumero.ClientID%>'))) {
                    return false;
                }
                adresse = alimenteAdresse();
                return ChainageAdresse(adresse);
            }
            else {
                alert("La validation d'adresse n'est possible que sur les adresses en France");
            }
            // dans les autres cas on retourne false
            return false;
        }


        // fonction qui vide le champ de validation (matriculeHexavia)
        // et qui change le style des champs textes de l'adresse
        $.fn.validHexaviaControls = function (controls) {
            var matriculeHexavia = $('#<%=matriculeHexavia.ClientID%>');
            matriculeHexavia.val('');

            controls.css("border", "2px solid gold");
        }

        $.fn.preloadHexavia = function () {
            //$('#<%=btnValider.ClientID%>').attr('disabled', '');
        }

        // fonction qui détecte le changement d'information d'un champ texte
        $.fn.validHexavia = function () {
            $('input[id$="Cp"], input[id$="inVille"], input[id*="inDistribution"], input[id*="inVoie"]').live('keypress', function (evt) { //, select[id*="ddlPays"] // ToDo ECM --- mise en commentaire du pays ---
                evt = evt || window.event;
                var keyPressed = evt.which || evt.keyCode;

                if (keyPressed != 9) {
                    var adresseValide = $('#<%=hfAdresseValide.ClientID%>').val();
                    var list = $('input[id$="Cp"], input[id$="inVille"], input[id*="inDistribution"], input[id*="inVoie"]'); //, select[id*="ddlPays"] // ToDo ECM --- mise en commentaire du pays ---
                    //jQuery.each(list, function () {
                    if (adresseValide == "1") {
                        $('#<%=hfAdresseValide.ClientID%>').val('0');
                        $('#<%=lblHexaviaErrors.ClientID%>').text("Adresse non validée");
                        // BOUTON VALIDER: adresse invalidée
                        $('#<%=btnValider.ClientID%>').attr('disabled', '');
                        $('#<%=btnSaisie.ClientID%>').removeAttr('disabled');
                        HexaviaDeselectedRowClick();
                    }
                    if (keyPressed != 13) {
                        $.fn.validHexaviaControls(list);
                    } 
                }
            });
        }

        var selectedRowIndex;

        function RowSelected(sender, arg) {
            // BOUTON VALIDER: adresse validée
            selectedRowIndex = sender.Index;
            //$('#<%=btnValider.ClientID%>').removeAttr('disabled');
            $('#<%=btnSaisie.ClientID%>').attr('disabled', '');
        }

        function HexaviaDeselectedRowClick() {
            var rowIndex = selectedRowIndex
            //window["<%= rgHexaviaAdressesGrid.ClientID %>"].rebind();
            //window["<%= rgHexaviaAdressesGrid.ClientID %>"].MasterTableView.DeselectRow(window["<%= rgHexaviaAdressesGrid.ClientID %>"].MasterTableView.Rows[0].Control);
            selectedRowIndex = -1;
            return false;
        }

    </script>
</telerik:RadScriptBlock>
<telerik:RadAjaxManagerProxy runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="btnChercher">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="divHexavia" LoadingPanelID="RadAjaxLoadingPanel1" />
                <telerik:AjaxUpdatedControl ControlID="rgHexaviaAdressesGrid" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="rgHexaviaAdressesGrid">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgHexaviaAdressesGrid" />
                <telerik:AjaxUpdatedControl ControlID="hfAdresseValide" />
                <telerik:AjaxUpdatedControl ControlID="hfAdresseValideLettre" />
                <telerik:AjaxUpdatedControl ControlID="btnValider" />
                <telerik:AjaxUpdatedControl ControlID="rapAdresse" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
<div id="divHexavia" runat="server">
    <fieldset id="fsHexavia" runat="server">
        <!--onkeypress="$.fn.validHexavia();"-->
        <legend>Données d'adresse</legend>
        <telerik:RadAjaxPanel ID="rapAdresse" runat="server" EnableAJAX="true">
            <div>
                <div id="divHexaviaColonneInfo">
                    <div>
                        <label for="inBatiment" class="decalage">
                            Bâtiment/ZI</label>
                        <input id="inBatiment" name="inBatiment" runat="server" type="text" tabindex="1"
                            size="32" maxlength="32" />
                    </div>
                    <div>
                        <label for="inNumero">
                            N°/Ext./Voie</label>
                        <%--<input id="inNumero" name="inNumero" runat="server" type="text" tabindex="2" size="4"
                            onblur="verifieN(this)" />
                           --%>
                           <input id="inNumero" name="inNumero" runat="server" type="text" tabindex="2" size="4"/>
                        <asp:DropDownList ID="ddlExtension" runat="server" SkinID="DropDown" TabIndex="3" />
                        <input id="inVoie" name="inVoie" runat="server" type="text" tabindex="4" size="32" maxlength="32"/>
                    </div>
                    <div>
                        <label for="inDistribution" class="decalage">
                            Distribution(BP...)
                        </label>
                        <input id="inDistribution" name="inDistribution" runat="server" type="text" tabindex="5"
                            size="32" maxlength="32" />
                    </div>
                    <div>
                        <label for="txtCp">
                            CP/Ville (Cedex)</label>
                        <asp:TextBox ID="txtCp" runat="server" Width="60px" TabIndex="6" MaxLength="5" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="RegulCP" runat="server" TargetControlID="txtCp"
                            FilterType="Numbers">
                        </ajaxToolkit:FilteredTextBoxExtender>
                        <input id="inVille" runat="server" type="text" tabindex="7" size="32" maxlength="26" />
                    </div>
                    <div runat="server" id="divCpVille">
                        <label for="txtCpVille">
                            &nbsp;</label>
                        <asp:TextBox ID="txtCpVille" runat="server" Width="60px" TabIndex="8" MaxLength="5"
                            Enabled="false" />
                        <ajaxToolkit:FilteredTextBoxExtender ID="RegulCPVille" runat="server" TargetControlID="txtCpVille"
                            FilterType="Numbers">
                        </ajaxToolkit:FilteredTextBoxExtender>
                        <input id="inVilleCedex" runat="server" type="text" tabindex="9" disabled size="32" maxlength="26" />
                    </div>
                    <div style="margin-top: 10px">
                        <label for="ddlPays">
                            Pays</label>
                        <asp:DropDownList ID="ddlPays" SkinID="DropDown" TabIndex="10" runat="server" Enabled="false" />
                        <asp:HiddenField runat="server" ID="matriculeHexavia" />
                        <asp:HiddenField runat="server" ID="urlExterieur" />
                        <asp:HiddenField runat="server" ID="adress" />
                        <input runat="server" id="inInsee" visible="false" />
                    </div>
                    <div>
                        <label runat="server" id="lblHexaviaMsg" class="infoMessage HexaviaErrors" />
                    </div>
                    <div>
                        <label runat="server" id="lblHexaviaErrors" class="errorMessage HexaviaErrors" />
                    </div>
                </div>
                <div id="divHexaviaColonneBouton">
                    <asp:Button ID="btnChercher" TabIndex="11" runat="server" Text="Chercher" OnClick="Chercher_Click" />
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </fieldset>
    <div id="divHexaviaAdresse">
        <telerik:RadGrid ID="rgHexaviaAdressesGrid" runat="server" AllowPaging="true" AllowCustomPaging="true"
            PageSize="10" OnNeedDataSource="AdressesGrid_NeedDataSource" OnItemCommand="AdressesGrid_ItemCommand"
            Culture="fr-FR" ShowStatusBar="true">
            <MasterTableView AutoGenerateColumns="false">
                <Columns>
                    <telerik:GridBoundColumn DataField="NomVoie" HeaderText="Libellé voie" />
                    <telerik:GridBoundColumn DataField="CodePostal" HeaderText="Code postal" />
                    <telerik:GridBoundColumn DataField="Ville" HeaderText="Ville" />
                    <telerik:GridBoundColumn Visible="false" DataField="MatriculeHexavia" />
                    <telerik:GridBoundColumn Visible="false" DataField="INSEE" />
                </Columns>
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" ClientEvents-OnRowSelected="RowSelected">
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </div>
    <div id="divHexaviaButton" style="float: right">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnValider" TabIndex="12" Enabled="false" runat="server" Text="Valider"
                        OnClientClick="return openConfirm()" />
                </td>
                <td>
                    <asp:Button ID="btnSaisie" TabIndex="13" runat="server" Text="Forcer" OnClientClick="return SaisieAdresse()" />
                </td>
                <td>
                    <asp:Button ID="btnAnnuler" TabIndex="14" runat="server" Text="Annuler" OnClientClick="return window.close()" />
                    <%--<uc2:OuiNonRadWindow ID="onrwAdresseNonValide" runat="server" Visible="false" />--%>
                </td>
            </tr>
        </table>
    </div>
</div>
<asp:HiddenField ID="hfAdresseValide" runat="server" Value="0" />
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        //$.fn.preloadHexavia();
        $.fn.validHexavia();
    });
</script>
