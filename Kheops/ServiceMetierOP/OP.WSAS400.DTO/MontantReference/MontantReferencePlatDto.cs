using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.MontantReference
{
    public class MontantReferencePlatDto
    {
        [Column(Name = "CODEPERIODICITE")]
        public string CodePeriodicite { get; set; }
        [Column(Name = "LIBPERIODICITE")]
        public string LibPeriodicite { get; set; }
        [Column(Name = "ECHEANCEJOUR")]
        public Int32 EcheanceJour { get; set; }
        [Column(Name = "ECHEANCEMOIS")]
        public Int32 EcheanceMois { get; set; }
        [Column(Name = "NEXTECHEANCEJOUR")]
        public Int32 NextEcheanceJour { get; set; }
        [Column(Name = "NEXTECHEANCEMOIS")]
        public Int32 NextEcheanceMois { get; set; }
        [Column(Name = "NEXTECHEANCEANNEE")]
        public Int32 NextEcheanceAnnee { get; set; }
        [Column(Name = "PERIODEDEBJOUR")]
        public Int32 PeriodeDebJour { get; set; }
        [Column(Name = "PERIODEDEBMOIS")]
        public Int32 PeriodeDebMois { get; set; }
        [Column(Name = "PERIODEDEBANNEE")]
        public Int32 PeriodeDebAnnee { get; set; }
        [Column(Name = "PERIODEFINJOUR")]
        public Int32 PeriodeFinJour { get; set; }
        [Column(Name = "PERIODEFINMOIS")]
        public Int32 PeriodeFinMois { get; set; }
        [Column(Name = "PERIODEFINANNEE")]
        public Int32 PeriodeFinAnnee { get; set; }
        [Column(Name = "FRAISACC")]
        public string FraisAccessoires { get; set; }
        [Column(Name = "MNTFRAISACC")]
        public Int64 MntFraisAccessoires { get; set; }
        [Column(Name = "TAXEATTENTAT")]
        public string TaxeAttentat { get; set; }
        [Column(Name = "LETTREFORM")]
        public string LettreFormule { get; set; }
        [Column(Name = "CODEFORM")]
        public Int64 CodeFormule { get; set; }
        [Column(Name = "LIBFORM")]
        public string LibFormule { get; set; }
        [Column(Name = "CODERSQ")]
        public Int64 CodeRsq { get; set; }
        [Column(Name = "LIBRSQ")]
        public string LibRisque { get; set; }
        [Column(Name = "MNTCALCULE")]
        public double MntCalcule { get; set; }
        [Column(Name = "MNTFORCE")]
        public double MntForce { get; set; }
        [Column(Name = "MNTACQUIS")]
        public double MntAcquis { get; set; }
        [Column(Name = "MNTPROVI")]
        public string MontantProvisionnel { get; set; }
        [Column(Name = "TOTALMNTCALCULE")]
        public double TotalMntCalcule { get; set; }
        [Column(Name = "TOTALMNTFORCE")]
        public double TotalMntForce { get; set; }
        [Column(Name = "TOTALMNTACQUIS")]
        public double TotalMntAcquis { get; set; }
        [Column(Name = "TOTALMNTPROVI")]
        public string TotalMntProvi { get; set; }
        [Column(Name = "CODEOBSV")]
        public Int64 CodeObservation { get; set; }
        [Column(Name = "OBSV")]
        public string Observation { get; set; }
    }
}
