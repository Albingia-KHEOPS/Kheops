using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ControleFin
{
    [DataContract]
    public class ControleFinControleDto
    {
        /// <summary>
        /// Etape de génération
        /// </summary>
        [DataMember]
        [Column(Name = "ETAPE")]
        public string Etape { get; set; }
        /// <summary>
        /// Risque
        /// </summary>
        [DataMember]
        [Column(Name = "RISQUE")]
        public Int32 Risque { get; set; }
        /// <summary>
        /// Objet
        /// </summary>
        [DataMember]
        [Column(Name = "OBJET")]
        public Int32 Objet { get; set; }
        /// <summary>
        /// ID KPINVEN
        /// </summary>
        [DataMember]
        [Column(Name = "IDINVENTAIRE")]
        public Int64 IdInventaire { get; set; }
        /// <summary>
        /// Numéro de ligne inventaire
        /// </summary>
        [DataMember]
        [Column(Name = "NUMEROLIGNEINVENTAIRE")]
        public Int32 NumeroLigneInventaire { get; set; }
        /// <summary>
        /// Formule
        /// </summary>
        [DataMember]
        [Column(Name = "FORMULE")]
        public Int32 Formule { get; set; }
        /// <summary>
        /// Option
        /// </summary>
        [DataMember]
        [Column(Name = "OPTION")]
        public Int32 Option { get; set; }
        /// <summary>
        /// Garantie
        /// </summary>
        [DataMember]
        [Column(Name = "GARANTIE")]
        public string Garantie { get; set; }
        /// <summary>
        /// Texte du message
        /// </summary>
        [DataMember]
        [Column(Name = "MESSAGE")]
        public string Message { get; set; }
        /// <summary>
        /// Niv classement Bloquant/NonVal/Avert
        /// </summary>
        [DataMember]
        [Column(Name = "NIVEAU")]
        public string Niveau { get; set; }
        /// <summary>
        /// Reference
        /// </summary>
        [DataMember]
        [Column(Name = "REFERENCE")]
        public string Reference { get; set; }
        /// <summary>
        /// Lien référence
        /// </summary>
        [DataMember]
        [Column(Name = "LIENREFERENCE")]
        public string LienReference { get; set; }

        [DataMember]
        [Column(Name = "LETTREFORMULE")]
        public string LettreFormule { get; set; }

        [DataMember]
        [Column(Name = "ACTEGESTION")]
        public string ActeGestion { get; set; }


        public ControleFinControleDto()
        {
            Etape = string.Empty;
            //Risque = string.Empty;
            //Objet = string.Empty;
            //IdInventaire = string.Empty;
            //NumeroLigneInventaire = string.Empty;
            //Formule = string.Empty;
            //Option = string.Empty;
            Garantie = string.Empty;
            Message = string.Empty;
            Niveau = string.Empty;
            Reference = string.Empty;
            LienReference = string.Empty;
        }
    }
}
