using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.MatriceFormule;

namespace OP.WSAS400.DTO.Offres.Risque.Objet
{
    [DataContract]
    public class ObjetDto : _Objet_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public int Code { get; set; }
        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [DataMember]
        public string Descriptif { get; set; }
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
        public string Designation { get; set; }
        /// <summary>
        /// Gets or sets the entree garantie.
        /// </summary>
        /// <value>
        /// The entree garantie.
        /// </value>
        [DataMember]
        public DateTime? EntreeGarantie { get; set; }
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
        /// Gets or sets the heure entree.
        /// </summary>
        /// <value>
        /// The heure entree.
        /// </value>
        [DataMember]
        public TimeSpan? HeureEntree { get; set; }
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
        [DataMember]
        public Int64? CoutM2 { get; set; }
        [DataMember]
        public string EnsembleType { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [valeur HT].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [valeur HT]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public string ValeurHT { get; set; }
        /// <summary>
        /// Gets or sets the adresse.
        /// </summary>
        /// <value>
        /// The adresse.
        /// </value>
        [DataMember]
        public AdressePlatDto AdresseObjet { get; set; }
        [DataMember]
        public int IdAdresseObjet { get; set; }
        /// <summary>
        /// Gets or sets the inventaires.
        /// </summary>
        /// <value>
        /// The inventaires.
        /// </value>
        [DataMember]
        public List<InventaireDto> Inventaires { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [risque indexe].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [risque indexe]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RisqueIndexe { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ObjetDto"/> is LCI.
        /// </summary>
        /// <value>
        ///   <c>true</c> if LCI; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool LCI { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ObjetDto"/> is franchise.
        /// </summary>
        /// <value>
        ///   <c>true</c> if franchise; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Franchise { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ObjetDto"/> is assiette.
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
        /// Gets or sets a value indicating whether this <see cref="ObjetDto"/> is CATNAT.
        /// </summary>
        /// <value>
        ///   <c>true</c> if CATNAT; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CATNAT { get; set; }
        /// <summary>
        /// Gets or sets the chrono desi.
        /// </summary>
        /// <value>
        /// The chrono desi.
        /// </value>
        [DataMember]
        public int ChronoDesi { get; set; }

        //ToDo ECM ajout pour la matrice Risque/Formule
        [DataMember]
        public bool hasInventaires { get; set; }
        [DataMember]
        public List<MatriceFormuleFormDto> Formules { get; set; }
        [DataMember]
        public Int32 NumeroChronoMatrice { get; set; }
        [DataMember]
        public bool isAffecte { get; set; }
        [DataMember]
        public bool isBadDate { get; set; }
        //ToDo ECM ajout pour la matrice Risque/Risque
        [DataMember]
        public string Formule { get; set; }
        [DataMember]
        public string CodeAlpha { get; set; }

        [DataMember]
        public string Action { get; set; }

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
        public DateTime? DateSortieDescr { get; set; }
        [DataMember]
        public TimeSpan? HeureEntreeDescr { get; set; }
        [DataMember]
        public TimeSpan? HeureSortieDescr { get; set; }
        [DataMember]
        public bool IsAlertePeriode { get; set; }
        [DataMember]
        public bool IsSortiAvenant { get; set; }
        [DataMember]
        public bool IsAfficheAvenant { get; set; }
        [DataMember]
        public double IndiceOrigine { get; set; }
        [DataMember]
        public double IndiceActualise { get; set; }
        [DataMember]
        public bool IsAvenantModificationLocale { get; set; }
        [DataMember]
        public bool IsReportvaleur { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantModificationLocale { get; set; }

        [DataMember]
        public DateTime? DateModifAvenantModificationLocale { get; set; }

        [DataMember]
        public DateTime? DateModificationObjetRisque { get; set; }
        [DataMember]
        public int AvnCreationObj { get; set; }
        [DataMember]
        public bool IsUsed { get; set; }
        [DataMember]
        public bool IsOut { get; set; }
        [DataMember]
        public bool IsRisqueTemporaire { get; set; }
        [DataMember]
        public DateTime? DateEffetAvenantOBJ { get; set; }

        [DataMember]
        public string CodeTypeRegule { get; set; }
        [DataMember]
        public string LibTypeRegule { get; set; }

        [DataMember]
        public decimal ValPorteeObj { get; set; }
        [DataMember]
        public string UnitPorteeObj { get; set; }
        [DataMember]
        public string TypePorteeCal { get; set; }
        [DataMember]
        public double PrimeMntCal { get; set; }
        

        public ObjetDto()
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
            this.AdresseObjet = new AdressePlatDto();
            this.Inventaires = new List<InventaireDto>();
            this.RegimeTaxe = string.Empty;
        }
    }
}
