using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class AffaireDto
    {
        virtual public DateTime? DateSaisie { get; set; }
        virtual public DateTime? DateEffet { get; set; }
        virtual public DateTime? DateFin { get; set; }
        virtual public DateTime? DateEffetAvenant { get; set; }
        virtual public DateTime? DateAccord { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime DateModification { get; set; }
        virtual public string CodeUserCreate { get; set; }
        virtual public string CodeUserUpdate { get; set; }
        virtual public DateTime? DateValidation { get; set; }
        virtual public DateTime? DateMajTraitement { get; set; }
        virtual public string CodeInterlocuteur { get; set; }
        virtual public int DelaisRelanceJours { get; set; }
        virtual public MotifSituation MotifSituation { get; set; }
        virtual public MotifAvenant MotifAvenant { get; set; }
        virtual public MotifResiliation MotifResiliation { get; set; }
        virtual public MotifRegularisation MotifRegularisation { get; set; }
        virtual public ModeRegularisation ModeRegularisation { get; set; }
        virtual public Situation SituationAffaire { get; set; }
        virtual public RegimeTaxe RegimeTaxe { get; set; }
        virtual public CodeSTOP CodeSTOP { get; set; }
        virtual public string Descriptif { get; set; }
        virtual public string CodeAffaire { get; set; }
        virtual public int NumeroAliment { get; set; }
        virtual public int NumeroAvenant { get; set; }
        virtual public AffaireType TypeAffaire { get; set; }
        /// <summary>
        /// ' ', 'O', 'N'
        /// </summary>
        virtual public Utilisateur Souscripteur { get; set; }
        virtual public Utilisateur Gestionnaire { get; set; }
        /// <summary>
        /// (V validé / A / N / R)
        /// </summary>
        virtual public string Etat { get; set; }
        virtual public Etat EtatAffaire { get; set; }
        virtual public TypeTraitement TypeTraitement { get; set; }
        virtual public TypeTraitement DernierTraitement { get; set; }
        virtual public NatureTravauxAffaire NatureTravaux { get; set; }
        virtual public string Observations { get; set; }
        virtual public Int64 NumChronoOsbv { get; set; }

        virtual public SituationAffaire Situation { get; set; }

        virtual public decimal PartAlbingia { get; set; }
        virtual public TypePolice TypePolice { get; set; }
        virtual public Cible Cible { get; set; }
        virtual public Branche Branche { get; set; }
        virtual public string SousBranche { get; set; }
        virtual public bool IsKheops { get; set; }
        virtual public Encaissement Encaissement { get; set; }
        virtual public string Categorie { get; set; }
        virtual public Devise Devise { get; set; }
        virtual public Intervenant Interlocuteur { get; set; }
        virtual public AffaireMetadata Metadata { get; set; }
        virtual public Assure Preneur { get; set; }
        virtual public Courtier CourtierGestionnaire { get; set; }
        virtual public Courtier CourtierApporteur { get; set; }
        virtual public Courtier CourtierPayeur { get; set; }
        virtual public string ReferenceCourtier { get; set; }

        virtual public bool IsHisto { get; set; }
        virtual public int? NumAvenant { get; set; }
        virtual public int PreavisMois { get; set; }
        virtual public Indice IndiceReference { get; set; }
        virtual public decimal Valeur { get; set; }
        virtual public bool SoumisCatNat { get; set; }
        virtual public bool IntercalaireExiste { get; set; }
        virtual public decimal TauxCommission { get; set; }
        virtual public decimal TauxCommissionCATNAT { get; set; }
        virtual public decimal BaseCATNATCalculee { get; set; }
        virtual public AffaireId OffreOrigine { get; set; }
        public Periode Periode { get; set; }
        public Echeance ProchaineEcheance { get; set; }
        public string DescriptionAvenant { get; set; }
        public DateTime? DateFinCalculee { get; set; }
        public TypeAccord TypeAccord { get; set; }
        public string DesignationAvenant { get; set; }
        virtual public int FraisAccessoires { get; set; }
        virtual public ApplicationFraisAccessoire ApplicationFraisAccessoire { get; set; }
        virtual public Periodicite Periodicite { get; set; }
        virtual public DateTime? Echeance { get; set; }
        public string Numero => $"{CodeAffaire} - {NumeroAliment}";
        virtual public decimal MontantReference { get; set; }
        virtual public NatureAffaire Nature { get; set; }
        virtual public bool? IsAttenteDocumentsCouriter { get; set; }
        virtual public TarifAffaireDto TarifAffaireLCI { get; set; }
        virtual public string TypeRemiseEnVigueur { get; set; }
    }

}

