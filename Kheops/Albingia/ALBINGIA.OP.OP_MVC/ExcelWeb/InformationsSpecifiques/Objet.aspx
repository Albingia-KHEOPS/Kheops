<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Objet.aspx.cs" Inherits="ALBINGIA.OP.OP_MVC.ExcelWeb.InformationsSpecifiques.Objet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="Stylesheet" media="screen" href="../../Content/_Site.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/FormCommon.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/vInformationsSpecifiques.css" />
    <link rel="Stylesheet" media="screen" href="../../Content/themes/base/jquery.ui.all.css" />
    <script language="javascript" type="text/javascript" src="../../Scripts/Jquery/jquery-1.8.2.min.js"></script>
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
                <td colspan="2" class="titre">Informations sur l'événement</td>
            </tr>
            <tr>
                <td>Nature des lieux</td>
                <td>
                    <select id="map_NatureLieux" albrequired="N">
                        <option value="SC">Scène couverte</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Nombre de participants</td>
                <td><input id="map_NombreParticipants" type="text" value="200" albrequired="O" /></td>
            </tr>
            <tr>
                <td>Nombre d'exposants</td>
                <td><input id="map_NombreExposants" type="text" value="13" albrequired="O" /></td>
            </tr>
        </table>
    </div>
    <div class="GradientSection FloatLeft Padding">
        <table>
            <tr>
                <td colspan="2" class="titre">Informations sur les supports d'enregistrement</td>
            </tr>
            <tr>
                <td>Nature du support</td>
                <td>
                    <select id="map_NatureSupport" albrequired="N">
                        <option value="INFO">Informatique</option>
                        <option value="NEGA">Négatif</option>
                        <option value="NGVD">Négatif et vidéo</option>
                        <option value="Vide">Vidéo</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Autre support</td>
                <td><input id="map_AutreSupport" type="text" value="true" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Type de négatif</td>
                <td>
                    <select id="map_TypeNegatif" albrequired="N">
                    
                    </select>
                </td>
            </tr>
            <tr>
                <td>Labo de développement</td>
                <td><input id="map_LaboDeveloppement" type="text" value="photolab paris" albrequired="N" /></td>
            </tr>
            <tr>
                <td>Fréquence d'envoi des rushs</td>
                <td>
                    <select id="map_FrequenceEnvoieRushs" albrequired="N">
                        <option value="1">1 semaine</option>
                        <option value="2">2 semaines</option>
                    </select>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
