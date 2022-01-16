using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Categorie;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Bloc;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Volet
{
    [DataContract]
    public class DtoVolet : _Volet_Base
    {
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
        /// Gets or sets the GUID id.
        /// </summary>
        /// <value>
        /// The GUID id.
        /// </value>
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "GUIDVOLET")]
        public Int64 GuidVolet { get; set; }

        /// <summary>
        /// Gets or sets the code option.
        /// </summary>
        /// <value>
        /// The code option.
        /// </value>
        [DataMember]
        [Column(Name = "CODEOPTION")]
        public string CodeOption { get; set; }

        [Column(Name = "CHECK")]
        public int Check { get; set; }

        [DataMember]
        [Column(Name = "ISVOLETGENERAL")]
        public string IsVoletGeneral { get; set; }

        [DataMember]
        [Column(Name="ISVOLETCOLLAPSE")]
        public string IsVoletCollapse { get; set; }

        [DataMember]
        [Column(Name = "NUMORDRE")]
        public double OrdreVolet { get; set; }

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
        /// Gets or sets the categories volet.
        /// </summary>
        /// <value>
        /// The categories volet.
        /// </value>
        [DataMember]
        public List<CategorieDto> CategoriesVolet { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        [DataMember]
        public List<CategorieDto> Categories { get; set; }
        [DataMember]
        public List<ParametreDto> Branches { get; set; }
        /// <summary>
        /// La branche principale du volet
        /// </summary>
        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }
        /// <summary>
        /// Gets or sets the blocs.
        /// </summary>
        /// <value>
        /// The blocs.
        /// </value>
        [DataMember]
        public List<BlocDto> Blocs { get; set; }

        /// <summary>
        /// récupère des tables finales si le volet est coché ou non pour les formules de garantie
        /// </summary>
        [DataMember]
        public bool isChecked { get; set; }

        [DataMember]
        public bool MAJ { get; set; }

        [DataMember]
        public bool ModifAvt { get; set; }
        [DataMember]
        public bool ModeAvt { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; }

        public DtoVolet()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.DateCreation = DateTime.MinValue;
            this.Caractere = string.Empty;

            this.CategoriesVolet = new List<CategorieDto>();
            this.Categories = new List<CategorieDto>();

            this.Blocs = new List<BlocDto>();

            this.CodeOption = string.Empty;
            this.isChecked = false;
        }
    }
}
