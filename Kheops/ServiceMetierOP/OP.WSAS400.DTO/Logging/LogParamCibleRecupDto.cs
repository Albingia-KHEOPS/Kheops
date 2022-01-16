using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Logging
{
    [DataContract]
    public class LogParamCibleRecupDto
    {
        /// <summary>
        /// N° aliment
        /// </summary>
        [DataMember]
        [Column(Name = "NUMALIMENT")]
        public int NumAliment { get; set; }

        /// <summary>
        /// N° avenant
        /// </summary>
        [DataMember]
        [Column(Name = "NUMAVENANT")]
        public int NumAvenant { get; set; }


        /// <summary>
        /// Code situation
        /// </summary>
        [DataMember]
        [Column(Name = "CODESITUATION")]
        public string CodeSituation { get; set; }

        /// <summary>
        /// Date situation
        /// </summary>
        [DataMember]
        [Column(Name = "DATESITUATION")]
        public int DateSituation { get; set; }

        /// <summary>
        /// N° histo
        /// </summary>
        [DataMember]
        [Column(Name = "NUMHISTO")]
        public int NumHisto { get; set; }

        /// <summary>
        /// N° police/offre
        /// </summary>
        [DataMember]
        [Column(Name = "NUMOFFREPOLICE")]
        public string NumOffrePolice { get; set; }

        /// <summary>
        ///N° travail
        /// </summary>
        [DataMember]
        [Column(Name = "NUMTRAVAIL")]
        public int NumTravail { get; set; }

        /// <summary>
        ///Id objet
        /// </summary>
        [DataMember]
        [Column(Name = "IDOBJET")]
        public int IdObjet { get; set; }

        /// <summary>
        ///Option récupération
        /// </summary>
        [DataMember]
        [Column(Name = "OPTIONRECUPERATION")]
        public string OptionRecuperation { get; set; }

        /// <summary>
        /// Id risque
        /// </summary>
        [DataMember]
        [Column(Name = "IDRISQUE")]
        public int IdRisque { get; set; }

        /// <summary>
        /// Commentaire situation
        /// </summary>
        [DataMember]
        [Column(Name = "COMMENTAIRESITUATION")]
        public string CommentaireSituation { get; set; }

        /// <summary>
        /// Type  O Offre  P Police
        /// </summary>
        [DataMember]
        [Column(Name = "TYPEOFFREPOLICE")]
        public string TypeOffrePolice { get; set; }

        /// <summary>
        /// User situation
        /// </summary>
        [DataMember]
        [Column(Name = "USERSITUATION")]
        public string UserSituation { get; set; }

    }
}
