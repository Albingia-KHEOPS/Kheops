using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationComputeData
    {
        [Column(Name = "SIT")]
        [DataMember]
        public string Etat { get; set; }

        [Column(Name ="RGT")]
        [DataMember]
        public string RegimeTaxe { get; set; }

        [Column(Name = "LBL")]
        [DataMember]
        public string Label { get; set; }

        [Column(Name = "PBT")]
        [DataMember]
        public int TauxAppel { get; set; }

        [Column(Name = "PBTR")]
        [DataMember]
        public int TauxAppelRetenu { get; set; }

        [Column(Name = "PBA")]
        [DataMember]
        public int NbAnnees { get; set; }

        [Column(Name = "PBR")]
        [DataMember]
        public int Ristourne { get; set; }

        [Column(Name = "RIA")]
        [DataMember]
        public int RistourneAnticipee { get; set; }

        [Column(Name = "SRIM")]
        [DataMember]
        public decimal MontantRistourneAnticipee { get; set; }

        [Column(Name = "PBS")]
        [DataMember]
        public int PrcSeuilSP { get; set; }

        [Column(Name = "SPC")]
        [DataMember]
        public double SeuilSPRetenu { get; set; }

        [Column(Name = "PBP")]
        [DataMember]
        public int PrcCotisationsRetenues { get; set; }

        [Column(Name = "EMD")]
        [DataMember]
        public decimal CotisationPeriode { get; set; }


        [Column(Name = "COT")]
        [DataMember]
        public decimal CotisationsRetenues { get; set; }

        [Column(Name = "SCHG")]
        [DataMember]
        public decimal ChargementSinistres { get; set; }

        [Column(Name = "XXX")]
        [DataMember]
        public decimal PrimeCalculee { get; set; }

        [DataMember]
        public string LibelleMontant { get; set; }

        [DataMember]
        public decimal MontantComptant { get; set; }

        [DataMember]
        public bool IsAnticipee { get; set; }

        [DataMember]
        public string SigneMontant { get; set; }

        [Column(Name = "MHC")]
        [DataMember]
        public decimal MontantCalcule { get; set; }

        [DataMember]
        public decimal MontantAffiche { get; set; }

        [Column(Name = "RGID")]
        [DataMember]
        public long RgId { get; set; }

        [Column(Name = "RSQID")]
        [DataMember]
        public int RsqId { get; set; }

        [Column(Name = "FORM")]
        [DataMember]
        public string Formule { get; set; }

        [Column(Name = "CODETAXE")]
        [DataMember]
        public string CodeTaxe { get; set; }

        [Column(Name ="BRNT")]
        [DataMember]
        public double TauxMaxi { get; set; }

        [Column(Name = "BRNC")]
        [DataMember]
        public decimal PrimeMaxi { get; set; }

        [Column(Name = "ASV")]
        [DataMember]
        public decimal Assiette { get; set; }

        [Column(Name = "MRG")]
        [DataMember]
        public RegularisationMode ReguleMode { get; set; }

        [Column(Name = "SIDF")]
        [DataMember]
        public decimal IndemnitesFrais { get; set; }

        [Column(Name = "SREC")]
        [DataMember]
        public decimal Recours { get; set; }

        [Column(Name = "SPRO")]
        [DataMember]
        public decimal Provisions { get; set; }

        [Column(Name = "SPRE")]
        [DataMember]
        public decimal Previsions { get; set; }

        [Column(Name = "SREP")]
        [DataMember]
        public decimal ReportChargesTrouve { get; set; }

        [Column(Name = "STD")]
        [DataMember]
        public int ReportChargeDateSituation { get; set; }

        [Column(Name = "RCR")]
        [DataMember]
        public decimal ReportChargesRetenu { get; set; }

        [Column(Name = "SRCN")]
        [DataMember]
        public decimal ReportChargesNouveau { get; set; }

        [Column(Name = "IPB")]
        [DataMember]
        public string IdContrat { get; set; }

        public void ComputeCotisationsRetenues()
        {
            switch (ReguleMode)
            {
                case RegularisationMode.BNS:

                    break;

                case RegularisationMode.Burner:

                    break;

                default:
                    CotisationsRetenues = (CotisationPeriode / 100M) * PrcCotisationsRetenues;
                    break;
            }
            
        }

        //private void ComputeMontantPB()
        //{
        //    var prcR = Convert.ToDecimal(Ristourne) / 100;
        //    var prcC = Convert.ToDecimal(PrcCotisationsRetenues) / 100;
        //    var prcS = Convert.ToDecimal(PrcSeuilSP) / 100;
        //    var prcT = TauxAppel == 0 ? 1 : Convert.ToDecimal(TauxAppel) / 100;

        //    CotisationsRetenues = prcT==1 ? prcC * CotisationPeriode : CotisationPeriode / (1 - prcR) * prcC;
        //    //CotisationsRetenues = prcC * CotisationPeriode;

        //    if (!string.IsNullOrEmpty(IdContrat) && IdContrat.StartsWith("TR"))
        //    {
        //        if (ChargementSinistres != IndemnitesFrais + Provisions - (Recours + Previsions))
        //        {
        //            ChargementSinistres = IndemnitesFrais + Provisions - (Recours + Previsions);
        //        }
               
        //        var P = CotisationPeriode;
        //        var O = ReportChargesRetenu;
        //        var S = ChargementSinistres + O;
        //        var A = MontantRistourneAnticipee;

                
        //        var total = prcR * (prcC * P - S) - A;
        //        MontantCalcule = total;
        //    }
        //    else
        //    {
        //        //MontantCalcule = prcR*(CotisationsRetenues - ChargementSinistres) - MontantRistourneAnticipee;
        //        // ARA - 3244
        //        if (ChargementSinistres <= CotisationPeriode * prcS)
        //        {
        //            MontantCalcule = -(prcR * (CotisationsRetenues - ChargementSinistres));
        //        }
        //        else
        //        {
        //            if (ChargementSinistres < 0)
        //            {
        //                MontantCalcule = prcR * (prcR*CotisationsRetenues);
        //            }
        //            else
        //            {
        //                MontantCalcule = 0;
        //            }

        //        }
        //        var cotisationEmise = prcT * CotisationPeriode;
        //        //MontantRistourneAnticipee = cotisationEmise - CotisationPeriode;
        //        MontantRistourneAnticipee = prcT == 1 ? cotisationEmise - CotisationPeriode : - (CotisationPeriode / (1 - prcR) - CotisationPeriode);
        //        MontantComptant = MontantCalcule - MontantRistourneAnticipee;
        //        IsAnticipee = TauxAppel != 100 && TauxAppel !=0;
        //    }
        //}


        private void ComputeMontantPB()
        {
            var prcR = Convert.ToDecimal(Ristourne) / 100;
            var prcC = Convert.ToDecimal(PrcCotisationsRetenues) / 100;
            var prcS = Convert.ToDecimal(PrcSeuilSP) / 100;
            var prcT = TauxAppel == 0 ? 1 : Convert.ToDecimal(TauxAppel) / 100;

            CotisationsRetenues = prcT == 1 ? prcC * CotisationPeriode : decimal.Round((CotisationPeriode / prcT), 2) * prcC;

            if (!string.IsNullOrEmpty(IdContrat) && IdContrat.StartsWith("TR"))
            {
                if (ChargementSinistres != IndemnitesFrais + Provisions - (Recours + Previsions))
                {
                    ChargementSinistres = IndemnitesFrais + Provisions - (Recours + Previsions);
                }

                var P = CotisationPeriode;
                var O = ReportChargesRetenu;
                var S = ChargementSinistres + O;
                var A = MontantRistourneAnticipee;


                var total = prcR * (prcC * P - S) - A;
                MontantCalcule = total;
            }
            else
            {
                // ARA - 3244
                if (ChargementSinistres <= (CotisationPeriode / prcT) * prcS)
                {
                    MontantCalcule = -(prcR * (CotisationsRetenues - ChargementSinistres));
                }
                else
                {
                    if (ChargementSinistres < 0)
                    {
                        MontantCalcule = prcR * (prcR * CotisationsRetenues);
                    }
                    else
                    {
                        MontantCalcule = 0;
                    }

                }
                var cotisationEmise = CotisationPeriode;
                MontantRistourneAnticipee = cotisationEmise - decimal.Round((CotisationPeriode / prcT), 2); 
                MontantComptant = MontantCalcule - MontantRistourneAnticipee;
                IsAnticipee = TauxAppel != 100 && TauxAppel != 0;
            }
        }

        private void ComputeMontantBNS()
        {
            IsAnticipee = TauxAppel != 100 && TauxAppel != 0;

            if (IsAnticipee)
            {
                CotisationPeriode = 100 * CotisationPeriode / Convert.ToDecimal(TauxAppel);
            }

            int displayTauxAppelRetenu = (TauxAppelRetenu == 0) ? 100 : TauxAppelRetenu;
            
            int displayTauxAppel = (TauxAppel == 0) ? 100 : TauxAppel;
            int taux = displayTauxAppelRetenu - displayTauxAppel;
            Ristourne = IsAnticipee ? 100 - TauxAppel : Ristourne;
            CotisationsRetenues = 100 - Ristourne;
            var prcT = IsAnticipee ? (100 - Convert.ToDecimal(TauxAppel)) / 100 : ((100 - Convert.ToDecimal(Ristourne)) - 100) / 100;

            PrimeCalculee = CotisationPeriode * CotisationsRetenues / 100;

            MontantCalcule = prcT * CotisationPeriode;
        }

        private void ComputeMontantBurner()
        {
            if (Assiette != 0)
            {
                PrimeMaxi = Assiette * (decimal)TauxMaxi / 100;
            }

            if (SeuilSPRetenu != 0)
            {
                PrimeCalculee = ChargementSinistres / ((decimal)SeuilSPRetenu / 100) * 100;
            }

            if (PrimeCalculee > PrimeMaxi)
            {
                MontantCalcule = PrimeMaxi - CotisationPeriode;
            }
            else
            {
                MontantCalcule = PrimeCalculee - CotisationPeriode;
            }

        }

        public void ComputeMontant()
        {
            switch (ReguleMode)
            {
                case RegularisationMode.BNS:
                    ComputeMontantBNS();
                    break;

                case RegularisationMode.Burner:
                    ComputeMontantBurner();
                    break;

                case RegularisationMode.PB:
                    ComputeMontantPB();
                    break;                
            }
            
            MontantAffiche = Math.Abs(MontantCalcule);
            
        }

        public void UpdateLabelMontant()
        {
            LibelleMontant = MontantCalcule < 0 ? "Montant calculé" : "Montant dû";
            SigneMontant = MontantCalcule < 0 ? "-" : string.Empty;
        }

        public RegularisationComputeData Refresh()
        {
            ComputeCotisationsRetenues();
            ComputeMontant();
            UpdateLabelMontant();

            return this;
        }
    }
}
