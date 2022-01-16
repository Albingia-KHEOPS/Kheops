<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Entete.aspx.cs" Inherits="ALBINGIA.OP.OP_MVC.ExcelWeb.InformationsSpecifiques.Entete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                <td colspan="2" class="titre">Bonification</td>
            </tr>
            <tr>
                <td>Bonification en cas de non sinistre</td>
                <td><input id="map_BonificationCasNonSinistre" type="checkbox" checked albrequired="N" /></td>
            </tr>
            <tr>
                <td>Montant (%)</td>
                <td><input id="map_Montant" type="text" albrequired="N"/></td>
            </tr>
            <tr>
                <td>Bonification anticipée</td>
                <td><input id="map_BonificatioonAnticipe" type="checkbox" checked albrequired="N" /></td>
            </tr>
        </table>
    </div>
</body>
</html>
