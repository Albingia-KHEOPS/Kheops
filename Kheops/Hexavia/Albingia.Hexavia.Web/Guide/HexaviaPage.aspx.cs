using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Albingia.Hexavia.Services;
using Albingia.Hexavia.CoreDomain;

namespace Albingia.Hexavia.Web
{
    public partial class HexaviaPage : System.Web.UI.Page
    {
        public AdresseServices AdresseServices { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnValider_Click(object sender, EventArgs e)
        {
            ResultatRechercheAdresse resultatRechercheAdresse = AdresseServices.VerifieAdresse(ObtenirAdresseEcran());
            switch (resultatRechercheAdresse)
            {
                case ResultatRechercheAdresse.OK:
                    lblStatusValeur.Text = "Valide";
                    break;
                default:
                    lblStatusValeur.Text = "Invalide";
                    break;
            }
        }


        #region binding
        public Adresse ObtenirAdresseEcran()
        {
            Adresse result = new Adresse();
            result.Batiment = txtBatiment.Text;
            result.NumeroVoie = txtNumeroVoie.Text;
            result.ExtensionVoie = txtExtensionVoie.Text;
            result.NomVoie = txtNomVoie.Text;
            result.BoitePostale = txtBoitePostale.Text;
            result.CodePostal = txtCodePostal.Text;
            result.Ville = txtVille.Text;
            result.Pays = new Pays{ Libelle = txtPays.Text};
            return result;
        }

        #endregion
    }
}