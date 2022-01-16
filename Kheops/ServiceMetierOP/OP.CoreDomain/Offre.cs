using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    public class Offre
    {
        public string CodeOffre { get; set; }
        public Branche Branche { get; set; }
        public ElementAssure RisquePrincipal
        {
            get
            {
                if (elementAssures == null)
                    return null;
                //ZBO 14/12/2011 :Modif du return des éléments assurés
                return ElementAssures.Exists(elem => elem.ElementPrincipal) && ElementAssures.Count > 0 ? ElementAssures.FirstOrDefault(x => x.ElementPrincipal) : elementAssures[0];
            }
        }
        private List<ElementAssure> elementAssures;
        public List<ElementAssure> ElementAssures
        {
            get
            {
                List<ElementAssure> result = new List<ElementAssure>();
                if (elementAssures != null)
                {
                    result = elementAssures.ToList();
                }
                return result;
            }
        }
        public string ContratMere { get; set; }
        public int NumAvenant { get; set; }

        public CabinetCourtage CabinetGestionnaire { get; set; }
        public decimal TauxDeComissionGestionnaire { get; set; }
        public CabinetCourtage CabinetApporteur { get; set; }
        public decimal TauxDeComissionApporteur { get; set; }
        public Souscripteur Souscripteur { get; set; }
        public Interlocuteur Interlocuteur { get; set; }
        public Gestionnaire Gestionnaire { get; set; }
        public DateTime? DateSaisie { get; set; }
        public DateTime? DateEffetGarantie { get; set; }
        public DateTime? DateCreation { get; set; }
        public string DateSaisieFormatee
        {
            get
            {
                if (DateSaisie.HasValue)
                {
                    return DateSaisie.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }
            }
        }
        public string HeureSaisieFormatee
        {
            get
            {
                if (DateSaisie.HasValue)
                {
                    return DateSaisie.Value.ToString("HH:mm");
                }
                else
                {
                    return "";
                }
            }
        }
        public DateTime? DateEnregistrement { get; set; }
        public string DateEnregistrementFormatee
        {
            get
            {
                if (DateEnregistrement.HasValue)
                {
                    return DateEnregistrement.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }
            }
        }
        public string HeureEnregistrementFormatee
        {
            get
            {
                if (DateEnregistrement.HasValue)
                {
                    return DateEnregistrement.Value.ToString("HH:mm");
                }
                else
                {
                    return "";
                }
            }
        }
        public Assure PreneurAssurance { get; set; }
        //public SituationOffre Situation { get; set; }

        //ECM 03/04/2012   
        public string Situation { get; set; }       // A = OuvertEtValidable ; V = Validé ; R = Réalisé ; N = OuvertNonValidable ; vide = Indéterminé
        public string Etat { get; set; }            // A = OuvertEtValidable ; V = Validé ; R = Réalisé ; N = OuvertNonValidable ; vide = Indéterminé
        public string Qualite { get; set; }
        public string Type { get; set; }
        public int? Version { get; set; }
        public string Reference { get; set; }
        public string RefCourtier { get; set; }
        public string MotifRefus { get; set; }
        public string CodeMotsClef1 { get; set; }
        public string CodeMotsClef2 { get; set; }
        public string CodeMotsClef3 { get; set; }
        public string Descriptif { get; set; }
        public string Observation { get; set; }

        public Parametre Devise { get; set; }
        public Parametre Periodicite { get; set; }
        public DateTime? EcheancePrincipale { get; set; }
        public DateTime? EffetGarantie { get; set; }
        public DateTime? DateFinEffetGarantie { get; set; }
        public int? DureeGarantie { get; set; }
        public Parametre UniteDeTemps { get; set; }
        public Parametre IndiceReference { get; set; }
        public decimal Valeur { get; set; }
        public Parametre NatureContrat { get; set; }
        public decimal? PartAlbingia { get; set; }
        public Aperiteur Aperiteur { get; set; }
        public decimal? PartAperiteur { get; set; }
        public int? Couverture { get; set; }
        public bool IntercalaireCourtier { get; set; }


        public void AjouterElementAssure(ElementAssure argElementAssure)
        {
            if (elementAssures == null)
            {
                elementAssures = new List<ElementAssure>();
            }

            if (argElementAssure.ElementPrincipal)
            {
                elementAssures.Where(x => x.ElementPrincipal).Select(x => x.ElementPrincipal = false);
            }
            elementAssures.Add(argElementAssure);
        }

        public Bonifications Bonification { get; set; }
        public decimal? FraisAperition { get; set; }
        public Risque Risque { get; set; }

        public List<Risque> Risques { get; set; }

        public List<CabinetAutre> CabinetAutres { get; set; }

        public DateTime? DateMAJ { get; set; }

        public bool CopyMode { get; set; }
        public string CodeOffreCopy { get; set; }
        public string VersionCopy { get; set; }


        public string test { get; set; }
        public DateTime? testDate { get; set; }

        public bool HasDoubleSaisie { get; set; }       
        public string CodeRegime { get; set; }
        //public string LibelleRegime { get; set; }
        public string SoumisCatNat { get; set; }
        //public decimal MontantReference { get; set; }
        //public string Indexation { get; set; }
        //public string LCI { get; set; }
        //public string Assiette { get; set; }
        //public string Franchise { get; set; }
        //public int Preavis { get; set; }
        //public string CodeAction { get; set; }
        //public string LibelleAction { get; set; }
        public string LibelleEtat { get; set; }
        //public string LibelleSituation { get; set; }
        //public int DateSituationJour { get; set; }
        //public int DateSituationMois { get; set; }
        //public int DateSituationAnnee { get; set; }
        //public string CodeUsrCreateur { get; set; }
        //public string NomUsrCreateur { get; set; }
        //public string CodeUsrModificateur { get; set; }
        //public string NomUsrModificateur { get; set; }   
        public int IdAdresseOffre { get; set; }
        public bool IsMonoRisque { get; set; }
        public Adresse AdresseOffre { get; set; }

        public Offre()
        {
            testDate = DateTime.Now;
        }
    }
}
