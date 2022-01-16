using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class QuittanceVisualisationLigneDto
    {
        [DataMember]
        [Column(Name = "EMISSIONANNEE")]
        public Int32 EmissionAnnee { get; set; }

        [DataMember]
        [Column(Name = "EMISSIONMOIS")]
        public Int32 EmissionMois { get; set; }

        [DataMember]
        [Column(Name = "EMISSIONJOUR")]
        public Int32 EmissionJour { get; set; }

        [DataMember]
        [Column(Name = "NUMQUITTANCE")]
        public Int32 NumQuittance { get; set; }

        [DataMember]
        [Column(Name = "AVIS")]
        public string Avis { get; set; }

        [DataMember]
        [Column(Name = "DATEDEBANNEE")]
        public Int32 DateDebAnnee { get; set; }

        [DataMember]
        [Column(Name = "DATEDEBMOIS")]
        public Int32 DateDebMois { get; set; }

        [DataMember]
        [Column(Name = "DATEDEBJOURS")]
        public Int32 DateDebJours { get; set; }

        [DataMember]
        [Column(Name = "DATEFINANNEE")]
        public Int32 DateFinAnnee { get; set; }

        [DataMember]
        [Column(Name = "DATEFINMOIS")]
        public Int32 DateFinMois { get; set; }

        [DataMember]
        [Column(Name = "DATEFINJOURS")]
        public Int32 DateFinJours { get; set; }

        [DataMember]
        [Column(Name = "ECHEANCEANNEE")]
        public Int32 EcheanceAnnee { get; set; }

        [DataMember]
        [Column(Name = "ECHEANCEMOIS")]
        public Int32 EcheanceMois { get; set; }

        [DataMember]
        [Column(Name = "ECHEANCEJOUR")]
        public Int32 EcheanceJour { get; set; }

        [DataMember]
        [Column(Name = "AVN")]
        public Int32 Avenant { get; set; }

        [DataMember]
        [Column(Name = "DEMCODE")]
        public string DemCode { get; set; }

        [DataMember]
        [Column(Name = "DEMLIBELLE")]
        public string DemLibelle { get; set; }

        [DataMember]
        [Column(Name = "MVTCODE")]
        public string MvtCode { get; set; }

        [DataMember]
        [Column(Name = "MVTLIBELLE")]
        public string MvtLibelle { get; set; }

        [DataMember]
        [Column(Name = "OPECODE")]
        public int OpeCode { get; set; }

        [DataMember]
        [Column(Name = "OPELIBELLE")]
        public string OpeLibelle { get; set; }

        [DataMember]
        [Column(Name = "SITCODE")]
        public string SitCode { get; set; }

        [DataMember]
        [Column(Name = "SITLIBELLE")]
        public string SitLibelle { get; set; }

        [DataMember]
        [Column(Name = "HT")]
        public double HT { get; set; }

        [DataMember]
        [Column(Name = "TTC")]
        public double TTC { get; set; }

        [DataMember]
        [Column(Name = "REGLE")]
        public double Regle { get; set; }

        [DataMember]
        [Column(Name = "ISANNULE")]
        public string IsAnnule { get; set; }

        [DataMember]
        [Column(Name="ISCHECK")]
        public string IsCheck { get; set; }

    }
}
