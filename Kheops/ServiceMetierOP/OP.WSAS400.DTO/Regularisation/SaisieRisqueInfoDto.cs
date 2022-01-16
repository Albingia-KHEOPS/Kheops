using OP.WSAS400.DTO.Offres.Risque;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    public class SaisieRisqueInfoDto
    {
        [Column(Name = "SEUILSP")]
        [DataMember]
        public double SeuilSp { get; set; }

        [Column(Name = "NBYEARRSQ")]
        [DataMember]
        public Int32 NbYears { get; set; }

        [Column(Name = "TXCOTISRETRSQ")]
        [DataMember]
        public double CotisationRetenue { get; set; }

        [Column(Name = "TXRISTRSQ")]
        [DataMember]
        public double Ristourne { get; set; }

        [Column(Name = "TAUXRSQ")]
        [DataMember]
        public double TauxAppel { get; set; }

        [Column(Name = "TAUX_MAXI")]
        [DataMember]
        public double TauxMaxi { get; set; }

        [Column(Name = "PRIME_MAXI")]
        [DataMember]
        public double PrimeMaxi { get; set; }


        [Column(Name = "CODERSQ")]
        [DataMember]
        public int CodeRsq { get; set; }

        [Column(Name = "CIBLE")]
        [DataMember]
        public string Cible { get; set; }

        [Column(Name = "LIBELLE")]
        [DataMember]
        public string Libelle { get; set; }

        [Column(Name = "ISCHECKED")]
        [DataMember]
        public bool IsProcessed { get; set; }

        [Column(Name = "DEBUT_RSQ")]
        [DataMember]
        public string DateDebutRsq { get; set; }

        [Column(Name = "FIN_RSQ")]
        [DataMember]
        public string DateFinRsq { get; set; }

    }
}
