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
    [DataContract]
    public class SaisieGarInfoDto
    {
        [Column(Name = "MOTIF_INF")]
        [DataMember]
        public Int32 Motif_Inf { get; set; }

        [Column(Name = "TITREGAR_STD")]
        [DataMember]
        public string TitreGar_STD { get; set; }
        [Column(Name = "DATEDEB_STD")]
        [DataMember]
        public Int32 DateDeb_STD { get; set; }
        [Column(Name = "DATEFIN_STD")]
        [DataMember]
        public Int32 DateFin_STD { get; set; }
        [Column(Name = "CODERSQ_STD")]
        [DataMember]
        public Int32 CodeRsq_STD { get; set; }
        [Column(Name = "LIBRSQ_STD")]
        [DataMember]
        public string LibRsq_STD { get; set; }
        [Column(Name = "CODERGT_STD")]
        [DataMember]
        public string CodeRgt_STD { get; set; }
        [Column(Name = "LIBRGT_STD")]
        [DataMember]
        public string LibRgt_STD { get; set; }
        [Column(Name = "CODEFOR_STD")]
        [DataMember]
        public Int32 CodeFor_STD { get; set; }

        [Column(Name = "LIBFOR_STD")]
        [DataMember]
        public string LibFor_STD { get; set; }
        [Column(Name = "LETTREFOR_STD")]
        [DataMember]
        public string LettreFor_STD { get; set; }
        [Column(Name = "LIBTAXE_STD")]
        [DataMember]
        public string LibTaxe_STD { get; set; }
        [Column(Name = "CODETAXE_STD")]
        [DataMember]
        public string CodeTaxe_STD { get; set; }
        [Column(Name = "TYPEGRILLE")]
        [DataMember]
        public string TypeGrille { get; set; }
        [Column(Name = "TYPEREGULGAR_STD")]
        [DataMember]
        public string TypeReguleGar_STD { get; set; }
        [Column(Name = "LIBREGULGAR_STD")]
        [DataMember]
        public string LibReguleGar_STD { get; set; }
        [Column(Name = "GARAUTO_STD")]
        [DataMember]
        public string GarAuto_STD { get; set; }
        [Column(Name = "VALGARAUTO_STD")]
        [DataMember]
        public Int32 ValGarAuto_STD { get; set; }
        [Column(Name = "UNITGARAUTO_STD")]
        [DataMember]
        public string UnitGarAuto_STD { get; set; }
        [Column(Name = "TXAPPEL_STD")]
        [DataMember]
        public double TxAppel_STD { get; set; }
        [Column(Name = "TXATTENTAT_STD")]
        [DataMember]
        public Single TxAttentat_STD { get; set; }
        [Column(Name = "MNTCOTISPROV_STD")]
        [DataMember]
        public double MntCotisProv_STD { get; set; }
        [Column(Name = "MNTCOTISACQUIS_STD")]
        [DataMember]
        public double MntCotisAquis_STD { get; set; }
        [Column(Name = "PREVASSIETTE_STD")]
        [DataMember]
        public double PrevAssiette_STD { get; set; }
        [Column(Name = "PREVTAUX_STD")]
        [DataMember]
        public double PrevTaux_STD { get; set; }
        [Column(Name = "PREVUNITE_STD")]
        [DataMember]
        public string PrevUnite_STD { get; set; }
        [Column(Name = "PREVCODTAXE_STD")]
        [DataMember]
        public string PrevCodTaxe_STD { get; set; }
        [Column(Name = "PREVMONTHT_STD")]
        [DataMember]
        public double PrevMntHt_STD { get; set; }
        [Column(Name = "PREVTAX_STD")]
        [DataMember]
        public double PrevTax_STD { get; set; }
        [Column(Name = "DEFASSIETTE_STD")]
        [DataMember]
        public double DefAssiette_STD { get; set; }
        [Column(Name = "DEFTAUX_STD")]
        [DataMember]
        public double DefTaux_STD { get; set; }
        [Column(Name = "DEFUNITE_STD")]
        [DataMember]
        public string DefUnite_STD { get; set; }
        [Column(Name = "DEFCODTAXE_STD")]
        [DataMember]
        public string DefCodTaxe_STD { get; set; }
        [Column(Name = "DEFVMONTHT_STD")]
        [DataMember]
        public double DefVmntHt_STD { get; set; }
        [Column(Name = "DEFVTAX_STD")]
        [DataMember]
        public double DefVtax_STD { get; set; }
        [Column(Name = "MNTCOTISEMIS_STD")]
        [DataMember]
        public double MntCotisEmis_STD { get; set; }
        [Column(Name = "MNTTXEMIS_STD")]
        [DataMember]
        public double MntTxEmis_STD { get; set; }
        [Column(Name = "MNTFORCEEMIS_STD")]
        [DataMember]
        public double MntForceEmis_STD { get; set; }
        [Column(Name = "MNTFORCETX_STD")]
        [DataMember]
        public double MntForceTx_STD { get; set; }
        [Column(Name = "COEFF_STD")]
        [DataMember]
        public double Coef_STD { get; set; }
        [Column(Name = "MNTREGULHT_STD")]
        [DataMember]
        public double MntRegulHt_STD { get; set; }
        [Column(Name = "ATTENTAT_STD")]
        [DataMember]
        public double Attentat_STD { get; set; }
        [Column(Name = "MNTFORCECALC_STD")]
        [DataMember]
        public double MntForceCalc_STD { get; set; }
        [Column(Name = "FORCE0_STD")]
        [DataMember]
        public string Force0_STD { get; set; }
        [Column(Name = "NBYEARRSQ_PB")]
        [DataMember]
        public Int32 NbYearRsq_PB { get; set; }
        [Column(Name = "TXRISTRSQ_PB")]
        [DataMember]
        public double TxRistRsq_PB { get; set; }
        [Column(Name = "SEUILSP_PB")]
        [DataMember]
        public double TSeuilSp_PB { get; set; }
        [Column(Name = "TXCOTISRETRSQ_PB")]
        [DataMember]
        public double TxCotisRetRsq_PB { get; set; }
        [Column(Name = "RISTANTICIPEE_PB")]
        [DataMember]
        public decimal RistAnticipee_PB { get; set; }
        [Column(Name = "COTISEMISE_PB")]
        [DataMember]
        public double CotisEmise_PB { get; set; }
        [Column(Name = "TXAPPELPBNS_PB")]
        [DataMember]
        public double TxAppelPbns_PB { get; set; }
        [Column(Name = "COTISDUE_PB")]
        [DataMember]
        public double CotisDue_PB { get; set; }
        [Column(Name = "NBYEARREGUL_PB")]
        [DataMember]
        public Int32 NbYearRegul_PB { get; set; }
        [Column(Name = "COTISRETENUE_PB")]
        [DataMember]
        public double CotisRetenue_PB { get; set; }
        [Column(Name = "TXCOTISRET_PB")]
        [DataMember]
        public double TxCotisRet_PB { get; set; }
        [Column(Name = "CHARGESIN_PB")]
        [DataMember]
        public double ChargeSin_PB { get; set; }
        [Column(Name = "PBNS_PB")]
        [DataMember]
        public double Pbns_PB { get; set; }
        //[Column(Name = "COTISANTICIPEE_PB")]
        //[DataMember]
        //public Int32 CotisAnticipee_PB { get; set; }
        [Column(Name = "TXRISTREGUL_PB")]
        [DataMember]
        public double TxRistRegul_PB { get; set; }
        [Column(Name = "RISTANTICIPEEREGUL_PB")]
        [DataMember]
        public double RistAnticipeeReguL_PB { get; set; }
        [Column(Name = "DATEDEBREGUL")]
        [DataMember]
        public Int32 DateDebRegul { get; set; }
        [Column(Name = "DATEFINREGUL")]
        [DataMember]
        public Int32 DateFinRegul { get; set; }

        [Column(Name = "CODECONTRAT")]
        [DataMember]
        public string codeContrat { get; set; }
        [Column(Name = "TYPE")]
        [DataMember]
        public string type { get; set; }
        [Column(Name = "VERSION")]
        [DataMember]
        public Int16 version { get; set; }
        [Column(Name = "CODEAVN")]
        [DataMember]
        public string codeAvn { get; set; }
        [Column(Name = "CODERSQ")]
        [DataMember]
        public Int32 codeRsq { get; set; }
        [Column(Name = "ASSIETTE")]
        [DataMember]
        public double assiette { get; set; }
        [Column(Name = "VALEUR")]
        [DataMember]
        public double val { get; set; }
        [Column(Name = "UNITE")]
        [DataMember]
        public string unite { get; set; }
        [Column(Name = "CODETAXE")]
        [DataMember]
        public string codeTaxe { get; set; }
        [Column(Name = "TOPFRC")]
        [DataMember]
        public string topFrc { get; set; }
        [Column(Name = "PRIMEPRO")]
        [DataMember]
        public string PrimePro { get; set; }
        [DataMember]
        public string ErrorStr { get; set; }

        [Column(Name = "MOTIF")]
        [DataMember]
        public string motif { get; set; }
        [DataMember]
        public bool SuivantDesactiv { get; set; }
        
        [Column(Name = "MNTCALCULREF")]
        [DataMember]
        public double MntCalculRef { get; set; }

        [DataMember]
        [Column(Name = "CODEFORMULE")]
        public int CodeFormule { get; set; }

        [DataMember]
        [Column(Name = "CODEOPTION")]
        public int CodeOption { get; set; }

        [DataMember]
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }

        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }

    }
}
