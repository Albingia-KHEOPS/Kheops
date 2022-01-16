using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FraisAccessoires
{
    [DataContract]
    public class FraisAccessoiresEngDto
    {
        /// <summary>
        /// Branche
        /// </summary>
        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }

        /// <summary>
        /// Sous branche
        /// </summary>
        [DataMember]
        [Column(Name = "SBRANCHE")]
        public string SousBranche { get; set; }


        /// <summary>
        /// Catégorie
        /// </summary>
        [DataMember]
        [Column(Name = "CATEGORIE")]
        public string Categorie { get; set; }

        /// <summary>
        /// Année
        /// </summary>
        [DataMember]
        [Column(Name = "ANNEE")]
        public int Annee { get; set; }

        /// <summary>
        /// Montant (on a decider de le passer en int)
        /// </summary>
        [DataMember]
        [Column(Name = "MONTANT")]
        public int Montant { get; set; }

        /// <summary>
        /// Frais accessoires en dessous du montant
        /// </summary>
        [DataMember]
        [Column(Name = "FRAISACCMIN")]
        public int FRaiSACCMIN { get; set; }

        /// <summary>
        ///Frais accessoires au dessu du montant
        /// </summary>
        [DataMember]
        [Column(Name = "FRAISACCMAX")]
        public int FRaiSACCMAX { get; set; }       

    }
}
