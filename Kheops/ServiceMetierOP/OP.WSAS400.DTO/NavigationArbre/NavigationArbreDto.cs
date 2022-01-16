using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.NavigationArbre {
    [DataContract]
    public class NavigationArbreDto : _DTO_Base
    {
        [DataMember]
        public string CodeOffre { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public bool InformationsSaisie { get; set; }
        [DataMember]
        public string TagSaisie { get; set; }

        [DataMember]
        public bool InformationsGenerales { get; set; }
        [DataMember]
        public string TagInfoGen { get; set; }

        [DataMember]
        public bool CoAssureurs { get; set; }
        [DataMember]
        public string TagCoAssureurs { get; set; }

        [DataMember]
        public bool CoCourtiers { get; set; }
        [DataMember]
        public string TagCoCourtiers { get; set; }

        [DataMember]
        public bool MatriceRisques { get; set; }
        [DataMember]
        public string TagMatriceRisques { get; set; }

        [DataMember]
        public bool MatriceFormules { get; set; }
        [DataMember]
        public string TagMatriceFormules { get; set; }

        [DataMember]
        public bool MatriceGaranties { get; set; }
        [DataMember]
        public string TagMatriceGaranties { get; set; }

        [DataMember]
        public List<RisqueDto> Risques { get; set; }

        [DataMember]
        public bool Engagement { get; set; }
        [DataMember]
        public string TagEngagement { get; set; }

        [DataMember]
        public bool Evenement { get; set; }
        [DataMember]
        public string TagEvenement { get; set; }

        [DataMember]
        public bool MontantRef { get; set; }
        [DataMember]
        public string TagMontantRef { get; set; }

        [DataMember]
        public bool Cotisation { get; set; }
        [DataMember]
        public string TagCotisation { get; set; }

        [DataMember]
        public bool Fin { get; set; }
        [DataMember]
        public string TagFin { get; set; }

        [DataMember]
        public bool ListeClauses { get; set; }
        [DataMember]
        public string TagListeClauses { get; set; }

        [DataMember]
        public bool ListeInventaires { get; set; }
        [DataMember]
        public string TagListeInventaires { get; set; }

        [DataMember]
        public bool GestionDocuments { get; set; }
        [DataMember]
        public string TagGestionDocuments { get; set; }

        [DataMember]
        public string OffreIdentification { get; set; }

        [DataMember]
        public long NumAvn { get; set; }
        public bool? Attentat { get; set; }
        public string TagAttentat { get; set; }
    }
}
