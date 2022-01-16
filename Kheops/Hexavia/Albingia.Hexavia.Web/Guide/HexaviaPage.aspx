<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="HexaviaPage.aspx.cs" Inherits="Albingia.Hexavia.Web.HexaviaPage" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainPlaceHolder" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblBatiment" runat="server" Text="Batiment / ZI"></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtBatiment" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblVoie" runat="server" Text="N°/Ext./Voie"></asp:Label>
            </td>
            <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="btnValider">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="lblStatusValeur" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManagerProxy>
            <td>
                <asp:TextBox ID="txtNumeroVoie" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtExtensionVoie" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtNomVoie" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBoitePostale" runat="server" Text="Boite Postale"></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtBoitePostale" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCpVille" runat="server" Text="CP/Ville"></asp:Label>
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtCodePostal" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:TextBox ID="txtVille" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPays" runat="server" Text="Pays"></asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtPays" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
            </td>
            <td colspan="3">
                <asp:Label ID="lblStatusValeur" runat="server" Text="Non Validée"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnValider" runat="server" Text="Valider" 
    onclick="btnValider_Click" />
</asp:Content>
