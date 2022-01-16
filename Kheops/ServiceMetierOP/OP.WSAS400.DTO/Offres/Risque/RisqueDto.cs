using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.MatriceFormule;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Risque
{
    [DataContract]
    public class RisqueDto //: _Risque_Base
    {
        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        /// <value>
        /// The Code.
        /// </value>
        [DataMember]
        [Column(Name = "CODE")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [DataMember]
        public String Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the cible.
        /// </summary>
        /// <value>
        /// The cible.
        /// </value>
        [DataMember]
        public CibleDto Cible { get; set; }

        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        /// <value>
        /// The designation.
        /// </value>
        [DataMember]
        [Column(Name = "LIBELLERISQUE")]
        public String Designation { get; set; }

        /// <summary>
        /// Gets or sets the entree garantie.
        /// </summary>
        /// <value>
        /// The entree garantie.
        /// </value>
        [DataMember]
        public DateTime? EntreeGarantie { get; set; }

        /// <summary>
        /// Gets or sets the heure entree.
        /// </summary>
        /// <value>
        /// The heure entree.
        /// </value>
        [DataMember]
        public TimeSpan? HeureEntree { get; set; }

        /// <summary>
        /// Gets or sets the sortie garantie.
        /// </summary>
        /// <value>
        /// The sortie garantie.
        /// </value>
        [DataMember]
        public DateTime? SortieGarantie { get; set; }

        [DataMember]
        public DateTime? SortieGarantieHisto { get; set; }
        /// <summary>
        /// Gets or sets the heure sortie.
        /// </summary>
        /// <value>
        /// The heure sortie.
        /// </value>
        [DataMember]
        public TimeSpan? HeureSortie { get; set; }

        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        [DataMember]
        public Int64? Valeur { get; set; }

        /// <summary>
        /// Gets or sets the unite.
        /// </summary>
        /// <value>
        /// The unite.
        /// </value>
        [DataMember]
        public ParametreDto Unite { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public ParametreDto Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [valeur HT].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [valeur HT]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public string ValeurHT { get; set; }

        /// <summary>
        /// Gets or sets the cout m2.
        /// </summary>
        /// <value>
        ///   cout M2
        /// </value>
        [DataMember]
        public Int64? CoutM2 { get; set; }

        /// <summary>
        /// Gets or sets the code objet.
        /// </summary>
        /// <value>
        /// The code objet.
        /// </value>
        [DataMember]
        [Column(Name = "CODEOBJET")]
        public int CodeObjet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [risque indexe].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [risque indexe]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RisqueIndexe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RisqueDto"/> is LCI.
        /// </summary>
        /// <value>
        ///   <c>true</c> if LCI; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool LCI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RisqueDto"/> is franchise.
        /// </summary>
        /// <value>
        ///   <c>true</c> if franchise; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Franchise { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RisqueDto"/> is assiette.
        /// </summary>
        /// <value>
        ///   <c>true</c> if assiette; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Assiette { get; set; }

        /// <summary>
        /// Gets or sets the regime taxe.
        /// </summary>
        /// <value>
        /// The regime taxe.
        /// </value>
        [DataMember]
        public string RegimeTaxe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RisqueDto"/> is CATNAT.
        /// </summary>
        /// <value>
        ///   <c>true</c> if CATNAT; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CATNAT { get; set; }

        [DataMember]
        public List<ObjetDto> Objets { get; set; }

        /// <summary>
        /// Gets or sets the chrono desi.
        /// </summary>
        /// <value>
        /// The chrono desi.
        /// </value>
        [DataMember]
        public int ChronoDesi { get; set; }

        [DataMember]
        public string ReportValeur { get; set; }
        [DataMember]
        public string ReportObligatoire { get; set; }

        //ToDo ECM ajout pour la matrice Risque/Formule
        [DataMember]
        public List<MatriceFormuleFormDto> Formules { get; set; }
        [DataMember]
        public Int32 NumeroChronoMatrice { get; set; }

        //ToDo ECM ajout pour la matrice Risque/Risque
        [DataMember]
        public string Formule { get; set; }
        [DataMember]
        public bool isIndexe { get; set; }
        [DataMember]
        public bool hasInventaire { get; set; }
        [DataMember]
        public bool isAffecte { get; set; }
        [DataMember]
        public bool isCopiable { get; set; }
        [DataMember]
        public bool isBadDate { get; set; }
        [DataMember]
        public string CodeAlpha { get; set; }
        [DataMember]
        public string Flag { get; set; }
        [DataMember]
        public AdressePlatDto AdresseRisque { get; set; }
        [DataMember]
        public int IdAdresseRisque { get; set; }
        [DataMember]
        public string CodeApe { get; set; }
        [DataMember]
        public string Nomenclature1 { get; set; }
        [DataMember]
        public string Nomenclature2 { get; set; }
        [DataMember]
        public string Nomenclature3 { get; set; }
        [DataMember]
        public string Nomenclature4 { get; set; }
        [DataMember]
        public string Nomenclature5 { get; set; }
        [DataMember]
        public string CodeTre { get; set; }
        [DataMember]
        public string LibTre { get; set; }
        [DataMember]
        public string CodeClasse { get; set; }
        [DataMember]
        public string Territorialite { get; set; }

        [DataMember]
        public string TypeRisque { get; set; }
        [DataMember]
        public string TypeMateriel { get; set; }
        [DataMember]
        public string NatureLieux { get; set; }
        [DataMember]
        public DateTime? DateEntreeDescr { get; set; }
        [DataMember]
        public TimeSpan? HeureEntreeDescr { get; set; }
        [DataMember]
        public DateTime? DateSortieDescr { get; set; }
        [DataMember]
        public TimeSpan? HeureSortieDescr { get; set; }

        [DataMember]
        public bool IsAlertePeriode { get; set; }
        [DataMember]
        public bool IsTraceAvnExist { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
        [DataMember]
        public int AvnCreationRsq { get; set; }
        [DataMember]
        public int AvnModifRsq { get; set; }

        [DataMember]
        public int TauxAppel { get; set; }
        [DataMember]
        public string IsRegularisable { get; set; }
        [DataMember]
        public string TypeRegularisation { get; set; }
        [DataMember]
        public bool IsUsed { get; set; }
        [DataMember]
        public bool IsOut { get; set; }

        [DataMember]
        public string PartBenef { get; set; }
        [DataMember]
        public int NbYear { get; set; }
        [DataMember]
        public int Ristourne { get; set; }
        [DataMember]
        public int CotisRetenue { get; set; }
        [DataMember]
        public int Seuil { get; set; }   
        [DataMember]
        public int TauxComp { get; set; }
        [DataMember]
        public bool IsRisqueTemporaire { get; set; }
        [DataMember]
        public Single TauxMaxi { get; set; }
        [DataMember]
        public double PrimeMaxi { get; set; }

        [DataMember]
        public string CodeTypeRegule { get; set; }
        [DataMember]
        public string LibTypeRegule { get; set; }

        [DataMember]
        public int EchAnnee { get; set; }
        [DataMember]
        public int EchMois { get; set; }
        [DataMember]
        public int EchJour { get; set; }


        public RisqueDto()//TODO SLA : à enlever
        {
            this.Descriptif = string.Empty;
            this.Cible = new CibleDto();
            this.Designation = string.Empty;
            this.EntreeGarantie = null;
            this.HeureEntree = null;
            this.SortieGarantie = null;
            this.HeureSortie = null;
            this.Valeur = null;
            this.Unite = new ParametreDto();
            this.Type = new ParametreDto();
            this.ValeurHT = string.Empty;
            this.RegimeTaxe = string.Empty;
            this.Objets = new List<ObjetDto>();
        }
    }
}
