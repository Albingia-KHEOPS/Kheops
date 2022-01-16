<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Erreur.aspx.cs" Inherits="Albingia.Hexavia.Web.Erreur" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td colspan="2" align="left"><img src="App_Themes/Albingia/albingia.png" alt=""/></td>
            </tr>
            <tr>
                <td></td>
                <td align="center"><b>Une erreur au niveau des paramètres de réception de la page. Veuillez contacter votre administrateur</b>
                <br/>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

                
                </td>
            </tr>
        </table>
        
    </div>
    </form>
</body>
</html>
