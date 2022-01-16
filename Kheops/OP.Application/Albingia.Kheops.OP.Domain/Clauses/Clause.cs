using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Referentiel;
using static ALBINGIA.Framework.Common.Constants.AlbConstantesMetiers;

namespace Albingia.Kheops.OP.Domain.Clauses
{
    using Inventaire = Albingia.Kheops.OP.Domain.Inventaire.Inventaire;
    public partial class Clause
    {

        public long IdUnique { get; set; }
        public long IdClause { get; set; }
        public NomClause NomClause { get; set; }
        public AffaireId AffaireId { get; set; }
        public Etapes EtapeDeGeneration { get; set; }
        public Etapes Perimetre { get; set; }
        public int Risque { get; set; }
        public int Objet { get; set; }
        //public int N__de_ligne_inventaire                      {get;set;}
        public int Formule { get; set; }
        public int Option { get; set; }
        public string Garantie { get; set; }
        public Contexte Contexte { get; set; }
        public bool IsAjoutee { get; set; }
        public long IdInventaire { get; set; }
        //public Inventaire Inventaire { get; set; }
        public NatureGeneration Nature { get; set; }
        public long IdDoc { get; set; }
        public long IdClauseMere { get; set; }
        ///public int Document_Impression                         {get;set;}
        public string Chapitre { get; set; }
        public string Souschapitre { get; set; }
        public long NuneroImpression { get; set; }
        public Decimal NumeroOrdonnancement { get; set; }
        public bool IsAnnexe { get; set; }
        public string CodeAnnexe { get; set; }
        public SituationClause Situation { get; set; }
        public DateTime? SituationDate { get; set; }
        public int NumeroAvenantCreation { get; set; }
        public DateTime CreationDate { get; set; }
        public int NumeroAvenantModification { get; set; }
        public DateTime MiseAJourDate { get; set; }
        public bool IsSpecifiqueAvenant { get; set; }
        public TypologieSensibilite TypologieSensibilite { get; set; }
        /// <summary>
        /// Gras/Souligné : "O"/ ""
        /// KCAAIM
        /// </summary>
        public bool AttributImpression { get; set; }

        // UNUSED :  public int Action_enchainée                            {get;set;}

        /// <summary>
        /// ELGO
        /// </summary>
        public OrigineClause OrigineGenerateur { get; set; }
        /// <summary>
        /// ELGI
        /// </summary>
        public long GenerateurId { get; set; }


        public bool IsTexteLibre { get; set; }
        /// <summary>
        /// TYPD : Ajouté / Genéré / Externe
        /// </summary>
        public TypeGenerationDoc TypeGenerationDoc { get; set; }
        /// <summary>
        /// ETAFF
        /// </summary>
        public Etapes EtapeAffichage { get; set; }
        /// <summary>
        /// XTLM
        /// </summary>
        public bool IsTexteLibreModifie { get; set; }
        public ParamClause ParamClause { get; set; }
    }
}
