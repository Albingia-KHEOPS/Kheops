using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.Offres;
using System;
using System.Globalization;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    public class ModeleListResultRecherche
    {
        #region Grille Principale

        public int i { get; set; }
        public string Type { get; set; }
        public string OffreContratNum { get; set; }
        public string Version { get; set; }
        public string VersionLabel { get; set; }
        public DateTime? DateSaisie { get; set; }
        public DateTime? DateEffet { get; set; }
        public DateTime? DateFinEffet { get; set; }
        public string DateFinEffetStr
        {
            get
            {
                if (DateFinEffet.HasValue)
                    return DateFinEffet.Value.Day.ToString().PadLeft(2, '0') + "/" +
                           DateFinEffet.Value.Month.ToString().PadLeft(2, '0') + "/" + DateFinEffet.Value.Year;
                else
                    return string.Empty;
            }
        }
        public TimeSpan? HeureFinEffet { get; set; }
        public string HeureFinEffetStr
        {
            get
            {
                if (HeureFinEffet.HasValue)
                    return HeureFinEffet.Value.Hours.ToString().PadLeft(2, '0') + ":" +
                           HeureFinEffet.Value.Minutes.ToString().PadLeft(2, '0');
                else
                    return string.Empty;
            }
        }
        public DateTime? DateMaj { get; set; }
        public DateTime? DateCreation { get; set; }
        public AlbConstantesMetiers.TypeDateRecherche TypeDate { get; set; }
        public string TypeAvt { get; set; }
        public string CodeCategorie { get; set; }
        public string Branche { get; set; }
        public string BrancheLib { get; set; }
        public string SousBranche { get; set; }
        public string PreneurAssuranceNom { get; set; }
        public string PreneurAssuranceCP { get; set; }
        public string Etat { get; set; }
        public string EtatLib { get; set; }
        public string Situation { get; set; }
        public string SituationLib { get; set; }
        public string TypeAccord { get; set; }
        public string Qualite { get; set; }
        public string QualiteLib { get; set; }
        public string CourtierGestionnaireNom { get; set; }
        public string CourtierGestionnaireCP { get; set; }
        public string DescriptifRisque { get; set; }
        public string ContratMere { get; set; }
        public string Cible { get; set; }
        public string CibleLib { get; set; }
        public int NumAvenant { get; set; }
        public string Periodicite { get; set; }
        public string KheopsStatut { get; set; }
        public Int32 GenerDoc { get; set; }
        public Int64 NumAvnExterne { get; set; }
        public bool HasSusp { get; set; }
        public DateTime? DateFinSusp { get; set; }
        public bool HasDoubleSaisie { get; set; }

        public long RegulId { get; set; }

        #endregion

        #region Sous-Grille

        public string Souscripteur { get; set; }
        public string PreneurAssuranceNum { get; set; }
        public string PreneurAssuranceVille { get; set; }
        public string CourtierGestionnaireNum { get; set; }
        public string CourtierGestionnaireVille { get; set; }
        public string MotifRefus { get; set; }

        #endregion

        public static explicit operator ModeleListResultRecherche(OffreDto offre)
        {
            var recherche = new ModeleListResultRecherche {
                i = offre.Compteur,
                Type = offre.Type,
                OffreContratNum = offre.CodeOffre,
                Version =
                    (offre.Version.ToString().Length == 0
                            ? " "
                            : offre.Version.ToString()),
                VersionLabel =
                    (offre.Version.ToString().Length == 0
                            ? " "
                            : offre.Version.Value.ToString("D4")),
                DateSaisie = offre.DateSaisie,
                DateMaj = offre.DateMAJ,
                DateEffet = offre.DateEffetGarantie,
                DateCreation = offre.DateCreation,
                TypeAvt = offre.TypeAvt,
                KheopsStatut = offre.KheopsStatut,
                DateFinEffet = offre.DateFinEffetGarantie,
                HeureFinEffet = offre.HeureFin,
                CodeCategorie = offre.CodeCategorie,
                SousBranche = offre.CodeSousBranche
            };
            if (offre.Branche != null)
            {
                recherche.Branche = offre.Branche.Code;
                recherche.BrancheLib = offre.Branche.Nom;

                if (offre.Branche.Cible != null)
                {
                    recherche.Cible = offre.Branche.Cible.Code;
                    recherche.CibleLib = offre.Branche.Cible.Nom;
                }
            }

            if (offre.PreneurAssurance != null)
            {
                recherche.PreneurAssuranceNom = offre.PreneurAssurance.NomAssure;
                recherche.PreneurAssuranceNum = offre.PreneurAssurance.Code;
                if (offre.PreneurAssurance.Adresse != null)
                {
                    recherche.PreneurAssuranceCP = offre.PreneurAssurance.Adresse.CodePostalString;
                    // ZBO-22/02/2016 : Remplacer le code postal numérique par de l'alphanumérique
                    //recherche.PreneurAssuranceCP = offre.PreneurAssurance.Adresse.CodePostal.ToString();
                    recherche.PreneurAssuranceVille = offre.PreneurAssurance.Adresse.NomVille;
                }
            }
            if (offre.CabinetGestionnaire != null)
            {
                recherche.CourtierGestionnaireNom = offre.CabinetGestionnaire.NomCabinet;
                recherche.CourtierGestionnaireNum = offre.CabinetGestionnaire.Code.ToString(CultureInfo.CurrentCulture);
                if (offre.CabinetGestionnaire.Adresse != null)
                {

                    recherche.CourtierGestionnaireCP = offre.CabinetGestionnaire.Adresse.CodePostalString;
                    // ZBO-22/02/2016 : Remplacer le code postal numérique par de l'alphanumérique
                    //recherche.CourtierGestionnaireCP = offre.CabinetGestionnaire.Adresse.CodePostal.ToString();
                    recherche.CourtierGestionnaireVille = offre.CabinetGestionnaire.Adresse.NomVille;
                }
            }
            else
            {
                recherche.CourtierGestionnaireNom = " ";
            }
            if (offre.Souscripteur != null)
            {
                recherche.Souscripteur = offre.Souscripteur.Nom;
            }
            if (offre.Periodicite != null)
            {
                recherche.Periodicite = offre.Periodicite.Code;
            }

            recherche.MotifRefus = offre.MotifRefus;
            recherche.DescriptifRisque = offre.Descriptif;

            recherche.Etat = offre.Etat;
            recherche.EtatLib = offre.EtatLib;

            recherche.Situation = offre.Situation;
            recherche.SituationLib = offre.SituationLib;

            recherche.Qualite = offre.Qualite.ToString(CultureInfo.CurrentCulture);
            recherche.QualiteLib = offre.QualiteLib;

            recherche.ContratMere = offre.ContratMere;
            recherche.NumAvenant = offre.NumAvenant;

            recherche.TypeAccord = offre.TypeAccord;
            recherche.GenerDoc = offre.GenerDoc;

            recherche.NumAvnExterne = offre.NumAvnExterne;

            recherche.HasSusp = offre.HasSusp;
            recherche.DateFinSusp = offre.DateFinSusp;

            recherche.HasDoubleSaisie = offre.HasDoubleSaisie;
            recherche.RegulId = offre.RegulId;

            return recherche;
        }
    }
}