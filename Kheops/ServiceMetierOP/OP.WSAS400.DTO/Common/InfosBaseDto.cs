using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Personnes;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class InfosBaseDto
    {
        [DataMember]
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }

        [DataMember]
        [Column(Name = "VERSION")]
        public int? Version { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public BrancheDto Branche { get; set; }

        [DataMember]
        public CabinetCourtageDto CabinetGestionnaire { get; set; }

        [DataMember]
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }

        [DataMember]
        public InterlocuteurDto Interlocuteur { get; set; }

        [DataMember]
        public AssureDto PreneurAssurance { get; set; }

        [DataMember]
        public List<RisqueDto> Risques { get; set; }

        [DataMember]
        public string Periodicite { get; set; }

        [DataMember]
        [Column(Name = "INDICEREFERENCE")]
        public string IndiceReference { get; set; }

        [DataMember]
        [Column(Name = "DATEEFFETA")]
        public Int16 DateEffetAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETM")]
        public Int16 DateEffetMois { get; set; }
        [DataMember]
        [Column(Name = "DATEEFFETJ")]
        public Int16 DateEffetJour { get; set; }
        [DataMember]
        public Int16 DateEffetHeure { get; set; }

        [DataMember]
        public Int16 FinEffetAnnee { get; set; }
        [DataMember]
        public Int16 FinEffetMois { get; set; }
        [DataMember]
        public Int16 FinEffetJour { get; set; }
        [DataMember]
        public Int16 FinEffetHeure { get; set; }

        [DataMember]
        public Int16 Duree { get; set; }
        [DataMember]
        public string DureeStr { get; set; }

        [DataMember]
        public int DureeGarantie { get; set; }
        [DataMember]
        public string UniteTemps { get; set; }

        [DataMember]
        [Column(Name = "ETATOFFRE")]
        public string Etat { get; set; }

        [DataMember]
        [Column(Name = "DATEAVNA")]
        public Int16 DateAvnAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEAVNM")]
        public Int16 DateAvnMois { get; set; }
        [DataMember]
        [Column(Name = "DATEAVNJ")]
        public Int32 DateAvnJour { get; set; }
        [DataMember]
        [Column(Name = "DATEAVNH")]
        public Int32 DateAvnHeure { get; set; }

        [DataMember]
        public string Situation { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public string LibNature { get; set; }
        [DataMember]
        public string Observation { get; set; }
        [DataMember]
        public Int64 CountRsq { get; set; }
        [DataMember]
        public Int16 NumAvenant { get; set; }

        [DataMember]
        public bool IsTemporaire { get; set; }

        [DataMember]
        public string TypeGestionAvenant { get; set; }

        [DataMember]
        public string TypeTraitement { get; set; }

        public bool IsRemiseEnVigeurAvecModif => TypeTraitement == "AVNRM" && TypeGestionAvenant == "M";
        public bool IsRemiseEnVigeurSansModif => TypeTraitement == "AVNRM" && TypeGestionAvenant != "M";

        public InfosBaseDto Refresh()
        {
            IsTemporaire = Periodicite != "A"
                && Periodicite != "T"
                && Periodicite != "R"
                && Periodicite != "S";

            return this;
        }
    }
}
