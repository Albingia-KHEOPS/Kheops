using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Albingia.Hexavia.Services;
using Albingia.Hexavia.CoreDomain;
using Albingia.Hexavia.DataAccess;
using Telerik.Web.UI;
using System.Text;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Albingia.Hexavia.Web.UserControl
{
    public partial class SaisieAdresseUC : System.Web.UI.UserControl
    {

        public AdresseServices AdresseServices { get; set; }
        public AdresseRepository AdresseRepository { get; set; }

        //public string AdresseACompleter { get; set; }
        /*public string AdresseUCACompleter
        {
            get
            {
                UCAdresse result = this.Page.Master.FindControl("MainContent").FindControl(this.AdresseACompleter) as UCAdresse;
                result.ClientIDMode = System.Web.UI.ClientIDMode.AutoID;
                return result.ClientID;
            }
        }*/

        public void Reinitialise()
        {
            adresseInput = null;
            txtCp.Text = null;
            inVille.Value = null;
            inBatiment.Value = null;
            inDistribution.Value = null;
            inVoie.Value = null;
            inNumero.Value = null;
            ddlExtension.SelectedIndex = -1;
            txtCpVille.Text = null;
            inVilleCedex.Value = null;
            ddlPays.Items.FindByValue("France").Selected = true;
            matriculeHexavia.Value = null;
            inInsee.Value = null;
            rgHexaviaAdressesGrid.VirtualItemCount = 0;
            rgHexaviaAdressesGrid.DataSource = new List<Adresse>();
            rgHexaviaAdressesGrid.DataBind();
            //divCpVille.Visible = false;
        }

        /*public bool RadWindowVisibleOnPageLoad
        {
            set
            {
                window1.VisibleOnPageLoad = value;
            }
        }*/

        public delegate void HexaviaTermine(Adresse adresse);
        public event HexaviaTermine HexaviaTermineEvent;

        public delegate void HexaviaAnnule();

        private Adresse adresseInput;
        public Adresse AdresseInput
        {
            get
            {
                if (adresseInput == null)
                {
                    adresseInput = new Adresse();
                }
                adresseInput.Batiment = inBatiment.Value;
                adresseInput.BoitePostale = inDistribution.Value;
                adresseInput.NomVoie = inVoie.Value;
                adresseInput.NumeroVoie = inNumero.Value;
                adresseInput.ExtensionVoie = ddlExtension.SelectedValue;
                
                adresseInput.CodePostal = txtCp.Text.Length>5?string.Empty:txtCp.Text;
                Regex supprDeptDansVille = new Regex(@"[^\w]|_|[0-9]");     //permet de supprimer les caractères numériques après des caractères alpha
                adresseInput.Ville = supprDeptDansVille.Replace(inVille.Value, " ").Trim();
                //adresseInput.Ville = inVille.Value;
                adresseInput.Pays = new Pays { Libelle = ddlPays.SelectedItem.Text };
                if (!String.IsNullOrEmpty(matriculeHexavia.Value))
                {
                    adresseInput.MatriculeHexavia = int.Parse(matriculeHexavia.Value);
                }
                adresseInput.INSEE = inInsee.Value;
                return adresseInput;
            }
        }
        public Adresse adresseRecherche;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // BOUTON VALIDER: adresse invalidée
                btnValider.Enabled = false;
                ddlExtension.DataSource = AdresseRepository.Extension;
                ddlExtension.DataTextField = "Libelle";
                ddlExtension.DataValueField = "Code";
                ddlExtension.SelectedIndex = -1;
                ddlExtension.DataBind();

                ddlPays.DataSource = AdresseRepository.Pays;
                ddlPays.DataTextField = "Libelle";
                ddlPays.DataBind();
                ddlPays.Items.FindByValue("France").Selected = true;
                if (!string.IsNullOrEmpty(Request["Data"]))
                {
                    string dataAdresse = Request["Data"].ToString();
                    //if (!string.IsNullOrEmpty(Request["adresse"]))
                    //{
                    string adresse = null;
                    //adresse = Request["adresse"].ToString();
                    adresse = dataAdresse.Replace("adresse=", string.Empty);
                    if (!string.IsNullOrEmpty(Request["host"]))
                    {
                        urlExterieur.Value = Request["host"];
                    }
                    else
                    {
                        RadScriptManager.RegisterStartupScript(this, this.GetType(), "Alerte page de redirection", "alert(\"la page de redirection dans l'url n''est pas présente!\");", true);
                    }
                    string[] adress = adresse.Split('¤');
                    //adresse.Split('�');
                    if (adress.Length >= 10)
                    {
                        inBatiment.Value = adress[0];
                        inNumero.Value = adress[1];
                        string valeurExtension = adress[2];

                        //if (valeurExtension != "bis" && valeurExtension != "ter")
                        if (valeurExtension == "")
                        {
                            valeurExtension = string.Empty;
                        }

                        var lstItems = ddlExtension.Items.FindByText(valeurExtension);
                        if(lstItems!=null)
                            lstItems.Selected = true;
                        inVoie.Value = adress[3];
                        inDistribution.Value = adress[4];
                        if (adress[7] != string.Empty && adress[8] != string.Empty) // mode Cedex
                        {
                            inVille.Value = adress[7];
                            txtCp.Text = adress[8];
                        }
                        else // mode non cedex
                        {
                            inVille.Value = adress[5];
                            txtCp.Text = adress[6];
                        }
                        //ddlPays.Items.FindByValue(adress[9]).Selected = true;

                        if (adress.Length == 11)
                        {
                            matriculeHexavia.Value = adress[10];
                        }

                        if (!string.IsNullOrEmpty(inVille.Value) || !string.IsNullOrEmpty(txtCp.Text))
                        {
                            ChercherProcedure();
                        }
                    }
                    else
                    {
                        RadScriptManager.RegisterStartupScript(this, this.GetType(), "Alerte adresse", "alert(\"l'adresse dans l'url est invalide!\");", true);
                    }
                }
                else
                {
                    RadScriptManager.RegisterStartupScript(this, this.GetType(), "Alerte adresse", "alert(\"l'adresse dans l'url n'est pas présente!\");", true);
                }
            }

            //onrwAdresseNonValide.OuiNonTermineEvent += TraitementAdresseNonValide;
        }

        public void TraitementAdresseNonValide(bool continuer)
        {
            //onrwAdresseNonValide.Visible = false;
            if (continuer)
            {
                SortirHexavia();
            }
        }

        private void SortirHexavia()
        {
            if (HexaviaTermineEvent != null)
            {
                HexaviaTermineEvent(AdresseInput);
            }
        }

        public void SortieDepVilleNonRenseigne(bool continuerValidation)
        {
            if (continuerValidation)
            {
                if (VerifieAdresseValide())
                {
                    SortirHexavia();
                }
            }
        }

        public void RechercheEtAffiche()
        {
            //try
            //{
                ViewState["InfosRecherche"] = AdresseInput;
            //}
            //catch (Exception e)
            //{
            //    throw new Exception(e.Message,e.InnerException);
            //}
            AdressesWrapper adressesWrapper = AdresseServices.RechercheAdresse(AdresseInput, 1, 10);

            if (adressesWrapper.Adresses.Count() == 0)
            {
                if (adressesWrapper.Overflow)
                {
                    AfficheErreur("Trop de résultats, veuillez préciser l'adresse.");
                }
                else
                {
                    AfficheErreur("Ville non trouvée");
                }
            }
            else
            {
                if (adressesWrapper.AucuneVoieNeConvient)
                {
                    AfficheVille(adressesWrapper, "Aucune voie ne convient");
                }
                else
                {
                    AfficheVille(adressesWrapper);
                }
            }
        }

        protected void Chercher_Click(object sender, EventArgs e)
        {
            //ECM 03/04/2012    Recherche si les premiers caractères du nom de la voie sont des numériques
            //                  si oui, renseigne le numéro de voie avec les caractères numériques
            string strVoie = inVoie.Value.Replace(",", " ");

            int numVoie = 0;
            string[] tabVoie = strVoie.Split(' ');
            if (int.TryParse(tabVoie[0], out numVoie))
            {
                inNumero.Value = numVoie.ToString();
                inVoie.Value = strVoie.Substring(inNumero.Value.Length + 1).Trim();
            }
            ChercherProcedure();
        }

        private void ChercherProcedure()
        {
            AfficheInfo("");
            if (ddlPays.SelectedItem.Text != "France")
            {
                AfficheErreur("La recherche d'adresse n'est possible que sur les adresses en France");
            }
            else
            {
                RechercheEtAffiche();
                btnValider.Enabled = false;
                btnSaisie.Enabled = true;
            }
        }

        private bool VerifieAdresseValide()
        {
            bool result = false;
            if (String.IsNullOrEmpty(matriculeHexavia.Value))
            {
                //onrwAdresseNonValide.Titre = "Adresse non validée";
                //onrwAdresseNonValide.Label =
                //    "L'adresse que vous avez saisie n'est pas validée. Continuez tout de même ?";
                //onrwAdresseNonValide.RadWindowVisibleOnPageLoad = true;
                //onrwAdresseNonValide.Visible = true;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private void AfficheErreur(string erreur)
        {
            lblHexaviaErrors.InnerText = erreur;
            rgHexaviaAdressesGrid.DataSource = null;
            rgHexaviaAdressesGrid.DataBind();
        }

        private void AfficheInfo(string msg)
        {
            lblHexaviaMsg.InnerText = msg;
        }

        private void AfficheVille(AdressesWrapper adressesWrapper, string message = "Veuillez selectionner l'adresse qui convient")
        {
            rgHexaviaAdressesGrid.CurrentPageIndex = 0;
            rgHexaviaAdressesGrid.VirtualItemCount = adressesWrapper.Count;
            rgHexaviaAdressesGrid.DataSource = adressesWrapper.Adresses;

            if (adressesWrapper.HasCedex)
            {
                AfficheInfo("CEDEX Détecté.");
            }
            divCpVille.Visible = true;

            // vide les champs + lecture seule pour les champs cedex
            inVilleCedex.Value = string.Empty;
            txtCpVille.Text = string.Empty;

            inVilleCedex.Disabled = true;
            txtCpVille.Enabled = false;
            //}
            /*else
            {
                inVilleCedex.Value = string.Empty;
                txtCpVille.Text = string.Empty;

                //divCpVille.Visible = false;
            }*/

            rgHexaviaAdressesGrid.DataBind();
            lblHexaviaErrors.InnerText = message;
        }

        protected void AdressesGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (ViewState["InfosRecherche"] != null)
            {
                adresseRecherche = (Adresse)ViewState["InfosRecherche"];
            }

            if (e.RebindReason != GridRebindReason.InitialLoad && adresseRecherche != null && (!String.IsNullOrEmpty(adresseRecherche.CodePostal) || !String.IsNullOrEmpty(adresseRecherche.Ville)))
            {
                int startRow = rgHexaviaAdressesGrid.CurrentPageIndex * rgHexaviaAdressesGrid.PageSize + 1;
                int endRow = (rgHexaviaAdressesGrid.CurrentPageIndex + 1) * rgHexaviaAdressesGrid.PageSize;
                rgHexaviaAdressesGrid.DataSource = AdresseServices.RechercheAdresse(adresseRecherche, startRow, endRow).Adresses;
            }
        }

        protected void AdressesGrid_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "RowClick")
            {
                CorrigerAdresse();
                hfAdresseValide.Value = "1";
                btnValider.Enabled = true;
                btnSaisie.Enabled = false;
            }
        }

        private void CorrigerAdresse()
        {
            Adresse adresseGrid = AdresseGridSelectionnee();
            if (adresseGrid != null)
            {
                lblHexaviaErrors.InnerText = "";    //ECM 03/04/12  

                if (AdresseInput.IsCedex.Value)
                {
                    if (!String.IsNullOrEmpty(txtCp.Text) && txtCp.Text.Substring(0, 2) != adresseGrid.Departement)
                    {
                        AfficheInfo("Différence de département entre votre critère et votre sélection.");
                    }

                    txtCpVille.Text = adresseGrid.CodePostal;
                    inVilleCedex.Value = adresseGrid.Ville;

                    /*if (String.IsNullOrEmpty(txtCpVille.Text) && String.IsNullOrEmpty(inVilleCedex.Value))
                    {
                        //2 lignes cedex ancienne règle
                        txtCpVille.Text = txtCp.Text;
                        inVilleCedex.Value = inVille.Value.ToUpper();

                        //lecture seule pour les champs non cedex
                        //txtCp.Enabled = false;
                        //inVille.Disabled = true;

                        //edition recherche pour les champs cedex
                        //txtCpVille.Enabled = true;
                        //inVilleCedex.Disabled = false;
                    }*/
                }
                else
                {
                    if (!String.IsNullOrEmpty(txtCp.Text) && txtCp.Text != adresseGrid.CodePostal)
                    {
                        AfficheInfo("Différence de Code Postal entre votre critère et votre sélection.");
                    }
                    txtCp.Text = adresseGrid.CodePostal;
                    inVille.Value = adresseGrid.Ville;

                    txtCpVille.Text = adresseGrid.CodePostal;
                    inVilleCedex.Value = adresseGrid.Ville;

                    //txtCp.Enabled = true;
                    //inVille.Disabled = false;
                }
                inVoie.Value = adresseGrid.NomVoie;
                //txtCp.Text = adresseGrid.CodePostal;
                //inVille.Value = adresseGrid.Ville;
                matriculeHexavia.Value = adresseGrid.MatriculeHexavia.ToString();
                inInsee.Value = adresseGrid.INSEE;
            }
        }

        private Adresse AdresseGridSelectionnee()
        {
            Adresse result = null;
            if (rgHexaviaAdressesGrid.SelectedItems.Count > 0)
            {
                result = new Adresse();
                GridDataItem item = (GridDataItem)rgHexaviaAdressesGrid.SelectedItems[0];
                result.CodePostal = item["CodePostal"].Text;
                result.Ville = item["Ville"].Text;
                result.NomVoie = item["NomVoie"].Text;
                result.MatriculeHexavia = int.Parse(item["MatriculeHexavia"].Text);
                result.INSEE = item["INSEE"].Text;
            }
            return result;
        }

        /*protected void btnRechSaisie_Click(object sender, EventArgs e)
        {
            rgHexaviaAdressesGrid.DataSource = null;
            rgHexaviaAdressesGrid.DataBind();
            lblHexaviaMsg.InnerText = "";
            lblHexaviaErrors.InnerText = "";
            inBatiment.Focus();
            window1.VisibleOnPageLoad = true;
            divHexavia.Visible = true;
        }*/
    }
}