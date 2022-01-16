using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Condition;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Branches;

namespace OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie
{
    /// <summary>
    /// Classe de retour pour l'initialisation de l'écran : ConditionRisqueGarantie
    /// </summary>
    [DataContract]
    public class ConditionRisqueGarantieGetResultDto // _ConditionRisqueGarantie_Base, IResult
    {
        [DataMember]
        public string CodeBranche { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is LCI.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is LCI; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsLCI { get; set; }
        /// <summary>
        /// Gets or sets the LCI.
        /// </summary>
        /// <value>
        /// The LCI.
        /// </value>
        [DataMember]
        public string LCI { get; set; }
        /// <summary>
        /// Gets or sets the unite LCI.
        /// </summary>
        /// <value>
        /// The unite LCI.
        /// </value>
        [DataMember]
        public string UniteLCI { get; set; }
        /// <summary>
        /// Gets or sets the unites LCI.
        /// </summary>
        /// <value>
        /// The unites LCI.
        /// </value>
        [DataMember]
        public List<ParametreDto> UnitesLCI { get; set; }
        /// <summary>
        /// Gets or sets the type LCI.
        /// </summary>
        /// <value>
        /// The type LCI.
        /// </value>
        [DataMember]
        public string TypeLCI { get; set; }
        /// <summary>
        /// Gets or sets the types LCI.
        /// </summary>
        /// <value>
        /// The types LCI.
        /// </value>
        [DataMember]
        public List<ParametreDto> TypesLCI { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is indexe LCI.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is indexe LCI; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsIndexeLCI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is franchise.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is franchise; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsFranchise { get; set; }
        /// <summary>
        /// Gets or sets the franchise.
        /// </summary>
        /// <value>
        /// The franchise.
        /// </value>
        [DataMember]
        public string Franchise { get; set; }
        /// <summary>
        /// Gets or sets the unite franchise.
        /// </summary>
        /// <value>
        /// The unite franchise.
        /// </value>
        [DataMember]
        public string UniteFranchise { get; set; }
        /// <summary>
        /// Gets or sets the unites franchise.
        /// </summary>
        /// <value>
        /// The unites franchise.
        /// </value>
        [DataMember]
        public List<ParametreDto> UnitesFranchise { get; set; }
        /// <summary>
        /// Gets or sets the type franchise.
        /// </summary>
        /// <value>
        /// The type franchise.
        /// </value>
        [DataMember]
        public string TypeFranchise { get; set; }
        /// <summary>
        /// Gets or sets the types franchise.
        /// </summary>
        /// <value>
        /// The types franchise.
        /// </value>
        [DataMember]
        public List<ParametreDto> TypesFranchise { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is indexe franchise.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is indexe franchise; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsIndexeFranchise { get; set; }

        /// <summary>
        /// Gets or sets the exp assiette.
        /// </summary>
        /// <value>
        /// The exp assiette.
        /// </value>
        [DataMember]
        public string ExpAssiette { get; set; }

        /// <summary>
        /// Gets or sets the formule.
        /// </summary>
        /// <value>
        /// The formule.
        /// </value>
        [DataMember]
        public string Formule { get; set; }
        /// <summary>
        /// Gets or sets the formules.
        /// </summary>
        /// <value>
        /// The formules.
        /// </value>
        [DataMember]
        public List<ParametreDto> Formules { get; set; }
        /// <summary>
        /// Gets or sets the garantie.
        /// </summary>
        /// <value>
        /// The garantie.
        /// </value>
        [DataMember]
        public string Garantie { get; set; }
        /// <summary>
        /// Gets or sets the garanties.
        /// </summary>
        /// <value>
        /// The garanties.
        /// </value>
        [DataMember]
        public List<ParametreDto> Garanties { get; set; }
        [DataMember]
        public List<ParametreDto> Niveaux { get; set; }
        [DataMember]
        public List<ParametreDto> VoletsBlocs { get; set; }

        /// <summary>
        /// Gets or sets the LCI unites.
        /// </summary>
        /// <value>
        /// The LCI unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> LCIUnites { get; set; }
        /// <summary>
        /// Gets or sets the LCI types.
        /// </summary>
        /// <value>
        /// The LCI types.
        /// </value>
        [DataMember]
        public List<ParametreDto> LCITypes { get; set; }
        /// <summary>
        /// Gets or sets the franchise unites.
        /// </summary>
        /// <value>
        /// The franchise unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> FranchiseUnites { get; set; }
        /// <summary>
        /// Gets or sets the franchise types.
        /// </summary>
        /// <value>
        /// The franchise types.
        /// </value>
        [DataMember]
        public List<ParametreDto> FranchiseTypes { get; set; }
        /// <summary>
        /// Gets or sets the assiette unites.
        /// </summary>
        /// <value>
        /// The assiette unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> AssietteUnites { get; set; }
        /// <summary>
        /// Gets or sets the assiette types.
        /// </summary>
        /// <value>
        /// The assiette types.
        /// </value>
        [DataMember]
        public List<ParametreDto> AssietteTypes { get; set; }
        /// <summary>
        /// Gets or sets the taux forfait HT unites.
        /// </summary>
        /// <value>
        /// The taux forfait HT unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> TauxForfaitHTUnites { get; set; }

        [DataMember]
        public string AppliqueA { get; set; }


        /// <summary>
        /// Gets or sets the LST ensemble ligne.
        /// </summary>
        /// <value>
        /// The LST ensemble ligne.
        /// </value>
        [DataMember]
        public List<EnsembleGarantieDto> LstEnsembleLigne { get; set; }

        [DataMember]
        public RisqueDto LstRisque { get; set; }

        //, query.LCI, query.UniteLCI, query.TypeLCI, query.IsIndexeLCI
        //                            , query.Franchise, query.UniteFranchise, query.TypeFranchise, query.IsIndexeFranchise

        [DataMember]
        public string FranchiseRisque { get; set; }
        [DataMember]
        public string UniteFranchiseRisque { get; set; }
        [DataMember]
        public string TypeFranchiseRisque { get; set; }
        [DataMember]
        public string LienComplexeFranchiseRisque { get; set; }
        [DataMember]
        public string LibComplexeFranchiseRisque { get; set; }
        [DataMember]
        public string LienComplexeFranchiseGenerale{ get; set; }
        [DataMember]
        public string CodeComplexeFranchiseRisque { get; set; }
        [DataMember]
        public string LibComplexeFranchiseGenerale { get; set; }
        [DataMember]
        public string CodeComplexeFranchiseGenerale { get; set; }

        [DataMember]
        public string LCIRisque { get; set; }
        [DataMember]
        public string UniteLCIRisque { get; set; }
        [DataMember]
        public string TypeLCIRisque { get; set; }
        [DataMember]
        public string LienComplexeLCIRisque { get; set; }
        [DataMember]
        public string LibComplexeLCIRisque { get; set; }
        [DataMember]
        public string CodeComplexeLCIRisque { get; set; }
        [DataMember]
        public string LienComplexeLCIGenerale { get; set; }
        [DataMember]
        public string LibComplexeLCIGenerale { get; set; }
        [DataMember]
        public string CodeComplexeLCIGenerale { get; set; }
        [DataMember]
        public bool IsAvnDisabled { get; set; }
        [DataMember]
        public CibleDto CodeCible { get; set; }

        [DataMember]
        public decimal TauxFraisGenerauxGareat { get; set; }
        [DataMember]
        public decimal TauxCommissionGareat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionRisqueGarantieGetResultDto"/> class.
        /// </summary>
        public ConditionRisqueGarantieGetResultDto()
        {
            UnitesLCI = new List<ParametreDto>();
            TypesLCI = new List<ParametreDto>();
            UnitesFranchise = new List<ParametreDto>();
            TypesFranchise = new List<ParametreDto>();
            Formules = new List<ParametreDto>();
            Garanties = new List<ParametreDto>();

            AssietteUnites = new List<ParametreDto>();
            AssietteTypes = new List<ParametreDto>();
            TauxForfaitHTUnites = new List<ParametreDto>();

            LstEnsembleLigne = new List<EnsembleGarantieDto>();
        }
    }

    
}
