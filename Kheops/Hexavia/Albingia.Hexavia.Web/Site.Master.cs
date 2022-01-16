using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Albingia.Hexavia.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateMenu();
        }

        protected void CreateMenu()
        {
            AlbingiaMenu.Items.Clear();
            MenuItem l_oMenu;

            // Onglet Menu Accueil
            l_oMenu = new MenuItem("Accueil");
            l_oMenu.NavigateUrl = "~/Default.aspx";
            l_oMenu.Selectable = true;
            AlbingiaMenu.Items.Add(l_oMenu);

            // Onglet Création Saisie
            l_oMenu = new MenuItem("Création Saisie");
            l_oMenu.NavigateUrl = "~/Offres/CreationSaisie.aspx";
            l_oMenu.Selectable = true;
            AlbingiaMenu.Items.Add(l_oMenu);

            // Onglet Gestion des offres
            l_oMenu = new MenuItem("Gestion des offres");
            l_oMenu.Selectable = false;
            AjouterItemDansMenu("Recherche de saisies", "~/Offres/RechercheSaisie.aspx", l_oMenu);

            if (l_oMenu.ChildItems.Count > 0)
            {
                AlbingiaMenu.Items.Add(l_oMenu);
            }
        }

        /// <summary>
        /// Ajouter une page dans le menu parent
        /// </summary>
        /// <param name="nomMenuItem">Nom de la page dans le menu</param>
        /// <param name="url">Url de la page</param>
        /// <param name="menuParent">Menu parent auquel ajouter la page</param>
        private static void AjouterItemDansMenu(string nomMenuItem, string url, MenuItem menuParent)
        {
            MenuItem sousMenu;
            string[] urlSplitte = url.Split(new char[] { '/' });
            string nomPage = urlSplitte[urlSplitte.Length - 1].ToLower();
            
            sousMenu = new MenuItem(nomMenuItem);
            sousMenu.NavigateUrl = url;
            menuParent.ChildItems.Add(sousMenu);
        }
    }
}
