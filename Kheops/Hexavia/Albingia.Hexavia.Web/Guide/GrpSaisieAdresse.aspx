<%@ Page Title="" Language="C#" MasterPageFile="~/SiteHexavia.Master" AutoEventWireup="true" CodeBehind="GrpSaisieAdresse.aspx.cs" Inherits="Albingia.Hexavia.Web.Guide.GrpSaisieAdresse" EnableEventValidation="true"  %>
<%@ Register Assembly="Albingia.Hexavia.Web" Namespace="Albingia.Hexavia.Web.UserControl"
    TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControl/CreationSaisie/SaisieAdresseUC.ascx" TagName="Hexavia"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<uc1:Hexavia ID="SaisieAdresseUC" runat="server" ></uc1:Hexavia>
</asp:Content>
