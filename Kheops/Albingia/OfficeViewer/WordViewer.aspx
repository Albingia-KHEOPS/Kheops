<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WordViewer.aspx.cs" Inherits="OfficeViewer.WordViewer" %>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <link type="text/css" rel="stylesheet" href="Content/OfficeContainer.css?<%=ALBINGIA.Framework.Common.Constants.AlbOpConstants.JsCsVersion %>>">
    <script language="javascript" type="text/javascript" src="Scripts/jquery-1.8.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="Scripts/jquery-ui-1.8.23.min.js"></script>
    <script language="javascript" type="text/javascript" src="Scripts/OfficeEvents.js?<%=ALBINGIA.Framework.Common.Constants.AlbOpConstants.JsCsVersion %>>"></script>
</head>
<body>
    <table border="1" width="100%" height="100%" id="table1" bordercolorlight="#008080" bordercolordark="#008080" cellspacing="1">
        <tr id="trErrorMessage" style="display: none">
            <td  bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF"  width="100%">
                <div class="WordErrorImage"><img src="Content/Images/notif_icn_critical.png" alt="" /></div>
                <div class="WordErrorTitleMessage"><h2>Erreur de visualisation du document Word</h2></div>
                <div class="WordErrorMessage">Le document Word présente des anomalies.</div>
            </td>
        </tr>
        <tr id="trWordViwer">
            <td bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF"  width="100%">
                <input id="fileNamePath" type="hidden" cilentid="fileNamePath" runat="server" />
                <input id="ruleFile" type="hidden" cilentid="ruleFile" runat="server" />
                <input id="jsVersion" type="hidden" cilentid="jsVersion" runat="server" />
                <input id="physicNameDoc" type="hidden" cilentid="fullNameDoc" runat="server" />
                <input id="prefixPathDocuments" type="hidden" cilentid="prefixPathDocuments" runat="server" />
                <%--<SCRIPT LANGUAGE=javascript FOR=OA1 EVENT=NotifyCtrlReady>
                    OA1_NotifyCtrlReady();
                </SCRIPT>--%>
                <object classid="clsid:7677E74E-5831-4C9E-A2DD-9B1EF9DF2DB4" id="OA1" width="1217" height="700" codebase="http://opmvc.albingia.local/OfficeViewer/OfficeViewerDist/officeviewer.cab#version=8,0,0,503">
                    <param name="Toolbars" value="-1">
                    <param name="BorderColor" value="15647136">
                    <param name="BorderStyle" value="2">
                </object>
            </td>
        </tr>
        <tr>
            <td bordercolorlight="#FFFFFF" bordercolordark="#FFFFFF" width="17%">
                 <script language="JavaScript" type="text/javascript" src="Scripts/OfficeViewerActivate.js?<%=ALBINGIA.Framework.Common.Constants.AlbOpConstants.JsCsVersion %>>"></script>
            </td>
        </tr>

    </table>
</body>
</html>