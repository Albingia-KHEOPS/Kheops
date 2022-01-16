using OP.WSAS400.DTO.Offres.Parametres;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.Risque
{
    [DataContract]
    public class OppositionDto
    {
        //GuidId
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        //Type Financement
        [DataMember]
        [Column(Name = "TYPE")]
        public string TypeFinancement { get; set; }
        [DataMember]
        [Column(Name = "TYPELABEL")]
        public string TypeFinancementLabel { get; set; }

        //Organisme de l'opposition
        [DataMember]
        [Column(Name = "CODEORGANISME")]
        public int CodeOrganisme { get; set; }
        [DataMember]
        [Column(Name = "ORGANISME")]
        public string NomOrganisme { get; set; }
        [DataMember]
        [Column(Name = "CPORGANISME")]
        public string CPOrganisme { get; set; }
        [DataMember]
        [Column(Name = "VILLEORGANISME")]
        public string VilleOrganisme { get; set; }
        [DataMember]
        [Column(Name = "PAYSORGANISME")]
        public string PaysOrganisme { get; set; }
        [DataMember]
        [Column(Name = "NOMPAYS")]
        public string NomPays { get; set; }

        [DataMember]
        [Column(Name = "ADRESSE1")]
        public string Adresse1 { get; set; }
        [DataMember]
        [Column(Name = "ADRESSE2")]
        public string Adresse2 { get; set; }
        //Opposition
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "REFERENCE")]
        public string Reference { get; set; }
        [DataMember]
        [Column(Name = "ECHEANCE")]
        public int iEcheance { get; set; }
        [DataMember]
        [Column(Name = "MONTANT")]
        public Double Montant { get; set; }
        [DataMember]
        [Column(Name = "KDESIREF")]
        public Int64 KDESIRef { get; set; }

        //Informations complémentaires
        [DataMember]
        public string Mode { get; set; }
        [DataMember]
        public List<ParametreDto> TypesFinancement { get; set; }
        [DataMember]
        public List<ParametreDto> ObjetsRisque { get; set; }
        [DataMember]
        public DateTime? Echeance { get; set; }
        [DataMember]
        public bool AppliqueAuRisqueEntier { get; set; }

        [DataMember]
        [Column(Name = "LIENFICHIERORIGINE")]
        public Int64 LienFichierOrigine { get; set; }
        [DataMember]
        [Column(Name = "ORGANISMEWARNING")]
        public string NomOrganismeWarning { get; set; }
        [DataMember]
        [Column(Name = "ADRESSE1WARNING")]
        public string Adresse1Warning { get; set; }
        [DataMember]
        [Column(Name = "ADRESSE2WARNING")]
        public string Adresse2Warning { get; set; }
        [DataMember]
        [Column(Name = "CPWARNING")]
        public string CPOrganismeWarning { get; set; }
        [DataMember]
        [Column(Name = "VILLEWARNING")]
        public string VilleWarning { get; set; }
        [DataMember]
        [Column(Name = "CODEPAYSWARNING")]
        public string CodePaysWarning { get; set; }
        [DataMember]
        [Column(Name = "NOMPAYSWARNING")]
        public string NomPaysWarning { get; set; }

        [DataMember]
        [Column(Name = "TYPEDEST")]
        public string TypeDest { get; set; }
        [DataMember]
        [Column(Name = "TYPEINTERV")]
        public string TypeInterv { get; set; }

    }
}
