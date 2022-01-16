using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class CreationAffaireNouvelleContratDto
    {
        [DataMember]
        [Column(Name = "CODECONTRAT")]
        public string CodeContrat { get; set; }
        [DataMember]
        [Column(Name = "VERSCONTRAT")]
        public Int64 VersionContrat { get; set; }
        [DataMember]
        [Column(Name = "TYPECONTRAT")]
        public string TypeContrat { get; set; }
        [DataMember]
        [Column(Name = "CODEAVN")]
        public Int64 CodeAvn { get; set; }
        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }
        [DataMember]
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
        [DataMember]
        [Column(Name = "LIBTYPECONTRAT")]
        public string LibTypeContrat { get; set; }
        [Column(Name = "DATEEFFET")]
        public string DateEffetDB { get; set; }
        [Column(Name = "DATEACCORD")]
        public string DateAccordDB { get; set; }
        [DataMember]
        [Column(Name = "CODECONTRATREMP")]
        public string CodeContratRemplace { get; set; }
        [DataMember]
        [Column(Name = "VERSCONTRATREMP")]
        public Int64 ContratRemplaceAliment { get; set; }
        [DataMember]
        [Column(Name = "SOUSCRIPTEUR")]
        public string Souscripteur { get; set; }
        [DataMember]
        [Column(Name = "GESTIONNAIRE")]
        public string Gestionnaire { get; set; }
        [DataMember]
        public string GestionnaireCode { get; set; }
        [DataMember]
        [Column(Name = "ETAT")]
        public string Etat { get; set; }
        [DataMember]
        public TimeSpan? HeureEffet { get; set; }
        [DataMember]
        public bool EstContratRemplace { get; set; }
        [DataMember]
        public string ContratMere { get; set; }
        [DataMember]
        public string Aliment { get; set; }

        [DataMember]
        public List<ParametreDto> Branches { get; set; }

        [DataMember]
        public DateTime? DateEffet { get; set; }

        [DataMember]
        public DateTime DateAccord { get; set; }
        [DataMember]
        public List<ParametreDto> TypesContrat { get; set; }

        [DataMember]
        public List<ParametreDto> Souscripteurs { get; set; }
        [DataMember]
        public List<ParametreDto> Gestionnaires { get; set; }

        [Column(Name = "HEUREEFFETINT")]
        public short? HeureEffetInt { get; set; }

       [Column(Name = "DATEEFFETDATE")]
        public long? DateEffetDate { get; set; }
    }
}
