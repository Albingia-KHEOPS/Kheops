using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using Albingia.Kheops.OP.Domain.Formule;

namespace OP.WSAS400.DTO.Condition
{

    [DataContract]
    public class LigneGarantieDto
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public string NumeroTarif { get; set; }
        /// <summary>
        /// Gets or sets the LCI valeur.
        /// </summary>
        /// <value>
        /// The LCI valeur.
        /// </value>
        [DataMember]
        public string LCIValeur { get; set; }
        /// <summary>
        /// Gets or sets the LCI unite.
        /// </summary>
        /// <value>
        /// The LCI unite.
        /// </value>
        [DataMember]
        public string LCIUnite { get; set; }
        /// <summary>
        /// Gets or sets the LCI unites.
        /// </summary>
        /// <value>
        /// The LCI unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> LCIUnites { get; set; }
        /// <summary>
        /// Gets or sets the type of the LCI.
        /// </summary>
        /// <value>
        /// The type of the LCI.
        /// </value>
        [DataMember]
        public string LCIType { get; set; }
        /// <summary>
        /// Gets or sets the LCI types.
        /// </summary>
        /// <value>
        /// The LCI types.
        /// </value>
        [DataMember]
        public List<ParametreDto> LCITypes { get; set; }
        /// <summary>
        /// Gets or sets the LCI lecture seule.
        /// </summary>
        /// <value>
        /// The LCI lecture seule.
        /// </value>
        [DataMember]
        public string LCILectureSeule { get; set; }
        /// <summary>
        /// Gets or sets the LCI obligatoire.
        /// </summary>
        /// <value>
        /// The LCI obligatoire.
        /// </value>
        [DataMember]
        public string LCIObligatoire { get; set; }
        /// <summary>
        /// Gets or sets the lien LCI complexe.
        /// </summary>
        /// <value>
        /// The lien LCI complexe.
        /// </value>
        [DataMember]
        public string LienLCIComplexe { get; set; }
        [DataMember]
        public string LibLCIComplexe { get; set; }

        /// <summary>
        /// Gets or sets the franchise valeur.
        /// </summary>
        /// <value>
        /// The franchise valeur.
        /// </value>
        [DataMember]
        public string FranchiseValeur { get; set; }
        /// <summary>
        /// Gets or sets the franchise unite.
        /// </summary>
        /// <value>
        /// The franchise unite.
        /// </value>
        [DataMember]
        public string FranchiseUnite { get; set; }
        /// <summary>
        /// Gets or sets the franchise unites.
        /// </summary>
        /// <value>
        /// The franchise unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> FranchiseUnites { get; set; }
        /// <summary>
        /// Gets or sets the type of the franchise.
        /// </summary>
        /// <value>
        /// The type of the franchise.
        /// </value>
        [DataMember]
        public string FranchiseType { get; set; }
        /// <summary>
        /// Gets or sets the franchise types.
        /// </summary>
        /// <value>
        /// The franchise types.
        /// </value>
        [DataMember]
        public List<ParametreDto> FranchiseTypes { get; set; }
        /// <summary>
        /// Gets or sets the franchise lecture seule.
        /// </summary>
        /// <value>
        /// The franchise lecture seule.
        /// </value>
        [DataMember]
        public string FranchiseLectureSeule { get; set; }
        /// <summary>
        /// Gets or sets the franchise obligatoire.
        /// </summary>
        /// <value>
        /// The franchise obligatoire.
        /// </value>
        [DataMember]
        public string FranchiseObligatoire { get; set; }
        /// <summary>
        /// Gets or sets the franchise mini.
        /// </summary>
        /// <value>
        /// The franchise mini.
        /// </value>
        [DataMember]
        public string FranchiseMini { get; set; }
        /// <summary>
        /// Gets or sets the franchise maxi.
        /// </summary>
        /// <value>
        /// The franchise maxi.
        /// </value>
        [DataMember]
        public string FranchiseMaxi { get; set; }
        /// <summary>
        /// Gets or sets the franchise debut.
        /// </summary>
        /// <value>
        /// The franchise debut.
        /// </value>
        [DataMember]
        public DateTime? FranchiseDebut { get; set; }
        /// <summary>
        /// Gets or sets the franchise fin.
        /// </summary>
        /// <value>
        /// The franchise fin.
        /// </value>
        [DataMember]
        public DateTime? FranchiseFin { get; set; }
        /// <summary>
        /// Gets or sets the lien FRH complexe.
        /// </summary>
        /// <value>
        /// The lien FRH complexe.
        /// </value>
        [DataMember]
        public string LienFRHComplexe { get; set; }
        [DataMember]
        public string LibFRHComplexe { get; set; }

        /// <summary>
        /// Gets or sets the assiette valeur.
        /// </summary>
        /// <value>
        /// The assiette valeur.
        /// </value>
        [DataMember]
        public string AssietteValeur { get; set; }
        /// <summary>
        /// Gets or sets the assiette unite.
        /// </summary>
        /// <value>
        /// The assiette unite.
        /// </value>
        [DataMember]
        public string AssietteUnite { get; set; }
        /// <summary>
        /// Gets or sets the assiette unites.
        /// </summary>
        /// <value>
        /// The assiette unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> AssietteUnites { get; set; }
        /// <summary>
        /// Gets or sets the type of the assiette.
        /// </summary>
        /// <value>
        /// The type of the assiette.
        /// </value>
        [DataMember]
        public string AssietteType { get; set; }
        /// <summary>
        /// Gets or sets the assiette types.
        /// </summary>
        /// <value>
        /// The assiette types.
        /// </value>
        [DataMember]
        public List<ParametreDto> AssietteTypes { get; set; }
        /// <summary>
        /// Gets or sets the assiette lecture seule.
        /// </summary>
        /// <value>
        /// The assiette lecture seule.
        /// </value>
        [DataMember]
        public string AssietteLectureSeule { get; set; }
        /// <summary>
        /// Gets or sets the assiette obligatoire.
        /// </summary>
        /// <value>
        /// The assiette obligatoire.
        /// </value>
        [DataMember]
        public string AssietteObligatoire { get; set; }

        [DataMember]
        public string TauxForfaitHTValeurOrigine { get; set; }

        /// <summary>
        /// Gets or sets the taux forfait HT valeur.
        /// </summary>
        /// <value>
        /// The taux forfait HT valeur.
        /// </value>
        [DataMember]
        public string TauxForfaitHTValeur { get; set; }
        /// <summary>
        /// Gets or sets the taux forfait HT unite.
        /// </summary>
        /// <value>
        /// The taux forfait HT unite.
        /// </value>
        [DataMember]
        public string TauxForfaitHTUnite { get; set; }
        /// <summary>
        /// Gets or sets the taux forfait HT unites.
        /// </summary>
        /// <value>
        /// The taux forfait HT unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> TauxForfaitHTUnites { get; set; }
        /// <summary>
        /// Gets or sets the taux forfait HT minimum.
        /// </summary>
        /// <value>
        /// The taux forfait HT minimum.
        /// </value>
        [DataMember]
        public string TauxForfaitHTMinimum { get; set; }
        /// <summary>
        /// Gets or sets the taux forfait HT lecture seule.
        /// </summary>
        /// <value>
        /// The taux forfait HT lecture seule.
        /// </value>
        [DataMember]
        public string TauxForfaitHTLectureSeule { get; set; }
        /// <summary>
        /// Gets or sets the taux forfait HT obligatoire.
        /// </summary>
        /// <value>
        /// The taux forfait HT obligatoire.
        /// </value>
        [DataMember]
        public string TauxForfaitHTObligatoire { get; set; }

        [DataMember]
        public Valeurs PrimeValeur { get; set; }

        [DataMember]
        public bool TauxPrimeAlim { get; set; }

        [DataMember]
        public string ConcurrenceValeur { get; set; }
        [DataMember]
        public string ConcurrenceUnite { get; set; }
        [DataMember]
        public List<ParametreDto> ConcurrenceUnites { get; set; }
        [DataMember]
        public string ConcurrenceType { get; set; }
        [DataMember]
        public List<ParametreDto> ConcurrenceTypes { get; set; }

        [DataMember]
        public string UniteLCINew { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesLCINew { get; set; }
        [DataMember]
        public string TypeLCINew { get; set; }
        [DataMember]
        public List<ParametreDto> TypesLCINew { get; set; }
        [DataMember]
        public string UniteFranchiseNew { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesFranchiseNew { get; set; }
        [DataMember]
        public string TypeFranchiseNew { get; set; }
        [DataMember]
        public List<ParametreDto> TypesFranchiseNew { get; set; }
        [DataMember]
        public string UniteConcurrence { get; set; }
        [DataMember]
        public List<ParametreDto> UnitesConcurrence { get; set; }
        [DataMember]
        public string TypeConcurrence { get; set; }
        [DataMember]
        public List<ParametreDto> TypesConcurrence { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Niveau { get; set; }

        [DataMember]
        public string CVolet { get; set; }
        [DataMember]
        public string CBloc { get; set; }

        [DataMember]
        public string MAJ { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LigneGarantieDto"/> class.
        /// </summary>
        public LigneGarantieDto()
        {
            LCIUnites = new List<ParametreDto>();
            LCITypes = new List<ParametreDto>();
            FranchiseUnites = new List<ParametreDto>();
            FranchiseTypes = new List<ParametreDto>();
            AssietteUnites = new List<ParametreDto>();
            AssietteTypes = new List<ParametreDto>();
            TauxForfaitHTUnites = new List<ParametreDto>();

            ConcurrenceUnites = new List<ParametreDto>();
            ConcurrenceTypes = new List<ParametreDto>();

            UnitesLCINew = new List<ParametreDto>();
            TypesLCINew = new List<ParametreDto>();
            UnitesFranchiseNew = new List<ParametreDto>();
            TypesFranchiseNew = new List<ParametreDto>();
            UnitesConcurrence = new List<ParametreDto>();
            TypesConcurrence = new List<ParametreDto>();
        }
    }
}
