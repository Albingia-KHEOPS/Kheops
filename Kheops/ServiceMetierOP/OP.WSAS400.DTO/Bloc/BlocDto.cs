using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.Modeles;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Bloc
{
    [DataContract]
    public class BlocDto : _Bloc_Base
    {
        /// <summary>
        /// Gets or sets the GUID id.
        /// </summary>
        /// <value>
        /// The GUID id.
        /// </value>
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "GUIDBLOC")]
        public Int64 GuidBloc { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "NUMORDRE")]
        public double OrdreBloc { get; set; }
        /// <summary>
        /// Gets or sets the date creation.
        /// </summary>
        /// <value>
        /// The date creation.
        /// </value>
        [DataMember]
        public DateTime? DateCreation { get; set; }

        [Column(Name = "DATECREATION")]
        public int iDateCreation { get; set; }

        [Column(Name = "HEURECREATION")]
        public int iHeureCreation { get; set; }

        /// <summary>
        /// Gets or sets the caractere.
        /// </summary>
        /// <value>
        /// The caractere.
        /// </value>
        [DataMember]
        [Column(Name = "CARACTERE")]
        public string Caractere { get; set; }

        /// <summary>
        /// Gets or sets the modeles.
        /// </summary>
        /// <value>
        /// The modeles.
        /// </value>
        [DataMember]
        public List<ModeleDto> Modeles { get; set; }

        /// <summary>
        /// Gets or sets the code option.
        /// </summary>
        /// <value>
        /// The code option.
        /// </value>
        [DataMember]
        public string CodeOption { get; set; }

        /// <summary>
        /// récupère des tables finales si le volet est coché ou non pour les formules de garantie
        /// </summary>
        [DataMember]
        public bool isChecked { get; set; }

        [DataMember]
        public bool MAJ { get; set; }

        [DataMember]
        public string BlocIncompatible { get; set; }
        [DataMember]
        public string BlocAssocie { get; set; }

        [DataMember]
        public bool ModifAvt { get; set; }
        [DataMember]
        public bool ModeAvt { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; }

        ///// <summary>
        ///// Gets or sets the modeles.
        ///// </summary>
        ///// <value>
        ///// The modeles.
        ///// </value>
        //[DataMember]
        //public List<GarantieModeleNiveau1Dto> Modeles { get; set; }

        public BlocDto()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.DateCreation = DateTime.MinValue;
            this.Caractere = string.Empty;

            this.Modeles = new List<ModeleDto>();
            //this.Modeles = new List<GarantieModeleNiveau1Dto>();

            this.CodeOption = string.Empty;
            this.isChecked = false;
        }
    }
}
