using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GestionIntervenants
{
    [DataContract]
    public class IntervenantDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "TYPEINTERVENANT")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "CODEINTERVENANT")]
        public int CodeIntervenant { get; set; }

        [DataMember]
        [Column(Name = "NOMINTERVENANT")]
        public string Denomination { get; set; }

        [DataMember]
        [Column(Name = "CODEPOSTAL")]
        public string CodePostal { get; set; }

        [DataMember]
        [Column(Name = "VILLE")]
        public string Ville { get; set; }

        [DataMember]
        [Column(Name = "CODEINTERLO")]
        public int CodeInterlocuteur { get; set; }

        [DataMember]
        [Column(Name = "NOMINTERLO")]
        public string Interlocuteur { get; set; }

        [DataMember]
        [Column(Name = "REFERENCE")]
        public string Reference { get; set; }

        [DataMember]
        [Column(Name = "ISMEDECINCONSEIL")]
        public string IsMedecinConseil { get; set; }

        [DataMember]
        [Column(Name = "ISPRINCIPAL")]
        public string IsPrincipal { get; set; }

        [DataMember]
        [Column(Name = "ISFINVALIDITE")]
        public string IsFinValidite { get; set; }


        [DataMember]
        [Column(Name = "ANNEEFINVALIDITE")]
        public int AnneeFinValidite { get; set; }

        [DataMember]
        [Column(Name = "MOISFINVALIDITE")]
        public int MoisFinValidite { get; set; }

        [DataMember]
        [Column(Name = "JOURFINVALIDITE")]
        public int JourFinValidite { get; set; }

        [DataMember]
        [Column(Name = "ADRESSE1")]
        public string Adresse1 { get; set; }

        [DataMember]
        [Column(Name = "ADRESSE2")]
        public string Adresse2 { get; set; }

        [DataMember]
        [Column(Name = "TELEPHONE")]
        public string Telephone { get; set; }

        [DataMember]
        [Column(Name = "EMAIL")]
        public string Email { get; set; }


    }
}
