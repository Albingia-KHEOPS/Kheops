using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class InfoCompQuittanceDto
    {
        [DataMember]       
        public string CommentaireForce { get; set; }

        [DataMember]
        public bool IsEcheancierExist { get; set; }

        [DataMember]
        public string TypeCalcul { get; set; }

        [DataMember]
        public bool IsEcheanceNonTraite { get; set; }

        [DataMember]
        public string Periodicite { get; set; }

        [DataMember]
        public double PourcentRepartition { get; set; } 

        [DataMember]
        public double PourcentRepartitionCalc { get; set; }

        [DataMember]
        public bool DisplayEmission { get; set; }
        [DataMember]
        public bool AEmission { get; set; }

        [DataMember]
        public DateTime? DateAvenant { get; set; }

        [DataMember]
        public bool ModifPeriode { get; set; }

        [DataMember]
        public bool AuMoinsRisqueTempExiste { get; set; }

        [DataMember]
        public bool TraceCC { get; set; }
        
        [DataMember]
        public DateTime? DateDebutEffetContrat { get; set; }

        [DataMember]
        public DateTime? DateFinEffetContrat { get; set; }

    }
}
