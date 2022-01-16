using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
namespace OP.WSAS400.DTO.ExcelDto.RS
{
    //Class Dto générée 
    [KnownType(typeof(Entete))]
    [DataContract]
    public class Entete : BaseExcelObjets
    {

        [DataMember]
        [Column(Name = "KAABONI")]
        public string BonificationCasNonSinistreEntete { get; set; }



        [DataMember]
        [Column(Name = "KAABONT")]
        public Single MontantEntete { get; set; }



        [DataMember]
        [Column(Name = "KAAANTI")]
        public string BonificatioonAnticipeEntete { get; set; }

        [DataMember]
        [Column(Name = "KAAIMED")]
        public Int64 MedecinConseil { get; set; }

        [DataMember]
        [Column(Name = "KAAIMDA")]
        public Int64 MedecinAviser { get; set; }

        [DataMember]
        [Column(Name = "KAAISIN")]
        public string CorrespondantSinistre { get; set; }

        [DataMember]
        public string CellsBonificationCasNonSinistreEntete { get; set; }

        [DataMember]
        public string CellsMontantEntete { get; set; }

        [DataMember]
        public string CellsBonificatioonAnticipeEntete { get; set; }

        [DataMember]
        public string CellsMedecinConseil { get; set; }

        [DataMember]
        public string CellsMedecinAviser { get; set; }

        [DataMember]
        public string CellsCorrespondantSinistre { get; set; }

    }
}