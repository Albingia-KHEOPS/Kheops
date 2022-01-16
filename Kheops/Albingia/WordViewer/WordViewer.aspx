<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WordViewer.aspx.cs" Inherits="WordViewer.WordViewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <input id="fileNamePath" type="hidden" cilentID="fileNamePath" runat="server" />
     <object classid="clsid:7677E74E-5831-4C9E-A2DD-9B1EF9DF2DB4" id="OA1" width="100%" height="100%" codebase="http://www.edrawsoft.com/download/officeviewer.cab#version=8,0,0,503">
			<!-- NOTE: The officeviewer.cab file in edrawsoft.com is the trial version. If you have the full version, you should upload it to your own site. Then change the codebase url.//-->
			<param name="Toolbars" value="-1">
			<param name="LicenseName" value="30daytrial">
			<param name="LicenseCode" value="EDWD-3333-2222-1111">
			<param name="BorderColor" value="15647136">
			<param name="BorderStyle" value="2">
		</object>
		<script language="JavaScript" type="text/javascript" src="Scripts/OfficeViewerActivate.js"></script>

    </div>
    </form>
</body>
</html>
