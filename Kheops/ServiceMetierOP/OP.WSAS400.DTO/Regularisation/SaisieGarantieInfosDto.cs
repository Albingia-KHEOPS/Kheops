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
    public class SaisieGarantieInfosDto
    {
        [Column(Name = "ID")]
        public long Id { get; set; }

        [Column(Name = "MOTIF_INF")]
        public int MotifInferieur { get; set; }

        [Column(Name = "TITREGAR")]
        public string TitreGar { get; set; }

        [Column(Name = "DATEDEB")]
        public int DateDeb { get; set; }

        [Column(Name = "DATEFIN")]
        public int DateFin { get; set; }

        [Column(Name = "CODERSQ")]
        public int CodeRsq { get; set; }

        [Column(Name = "LIBRSQ")]
        public string LibRsq { get; set; }

        [Column(Name = "CODERGT")]
        public string CodeRgt { get; set; }

        [Column(Name = "LIBRGT")]
        public string LibRgt { get; set; }

        [Column(Name = "CODEFOR")]
        public int CodeFor { get; set; }

        [Column(Name = "LIBFOR")]
        public string LibFor { get; set; }

        [Column(Name = "LETTREFOR")]
        public string LettreFor { get; set; }

        [Column(Name = "LIBTAXE")]
        public string LibTaxe { get; set; }

        [Column(Name = "CODETAXE")]
        public string CodeTaxe { get; set; }

        [Column(Name = "TYPEGRILLE")]
        public string TypeGrille { get; set; }

        [Column(Name = "TYPEREGULGAR")]
        public string TypeReguleGar { get; set; }

        [Column(Name = "ISREADONLYRCUS")]
        public bool IsReadOnlyRCUS { get; set; }

        public bool IsGarantieAuto
        {
            get { return TypeReguleGar == "A"; }
            set { }
        }

        [Column(Name = "LIBREGULGAR")]
        public string LibReguleGar { get; set; }

        [Column(Name = "GARAUTO")]
        public string GarAuto { get; set; }

        [Column(Name = "VALGARAUTO")]
        public Int32 ValGarAuto { get; set; }

        [Column(Name = "UNITGARAUTO")]
        public string UnitGarAuto { get; set; }

        [Column(Name = "TXAPPEL")]
        public double TxAppel { get; set; }

        [Column(Name = "TXATTENTAT")]
        public Single TxAttentat { get; set; }

        [Column(Name = "MNTCOTISPROV")]
        public double MntCotisProv { get; set; }

        public bool HasCotisationPro
        {
            get { return PrimePro == "O"; }
            set { }
        }

        [Column(Name = "MNTCOTISACQUIS")]
        public double MntCotisAquis { get; set; }

        public bool HasCotisationAquis
        {
            get { return MntCotisAquis != 0; }
            set { }
        }

        [Column(Name = "PREVASSIETTE")]
        public double PrevAssiette { get; set; }

        [Column(Name = "PREVTAUX")]
        public double PrevTaux { get; set; }

        [Column(Name = "PREVUNITE")]
        public string PrevUnite { get; set; }

        [Column(Name = "PREVCODTAXE")]
        public string PrevCodTaxe { get; set; }

        [Column(Name = "PREVMONTHT")]
        public double PrevMntHt { get; set; }

        [Column(Name = "PREVTAX")]
        public double PrevTax { get; set; }

        [Column(Name = "DEFASSIETTE")]
        public double DefAssiette { get; set; }

        [Column(Name = "DEFTAUX")]
        public double DefTaux { get; set; }

        [Column(Name = "DEFUNITE")]
        public string DefUnite { get; set; }

        [Column(Name = "DEFCODTAXE")]
        public string DefCodTaxe { get; set; }

        [Column(Name = "DEFVMONTHT")]
        public double DefVmntHt { get; set; }

        [Column(Name = "DEFVTAX")]
        public double DefVtax { get; set; }

        [Column(Name = "MNTCOTISEMIS")]
        public double MntCotisEmis { get; set; }

        [Column(Name = "MNTTXEMIS")]
        public double MntTxEmis { get; set; }

        [Column(Name = "MNTFORCEEMIS")]
        public double MntForceEmis { get; set; }

        [Column(Name = "MNTFORCETX")]
        public double MntForceTx { get; set; }

        [Column(Name = "COEFF")]
        public double Coef { get; set; }

        [Column(Name = "MNTREGULHT")]
        public double MntRegulHt { get; set; }

        [Column(Name = "ATTENTAT")]
        public double Attentat { get; set; }

        [Column(Name = "MNTFORCECALC")]
        public double MntForceCalc { get; set; }

        [Column(Name = "FORCE0")]
        public string Force0 { get; set; }

        [Column(Name = "DATEDEBREGUL")]
        public Int32 DateDebRegul { get; set; }

        [Column(Name = "DATEFINREGUL")]
        public Int32 DateFinRegul { get; set; }

        [Column(Name = "CODECONTRAT")]
        public string codeContrat { get; set; }

        [Column(Name = "TYPE")]
        public string type { get; set; }

        [Column(Name = "VERSION")]
        public Int16 version { get; set; }

        [Column(Name = "CODEAVN")]
        public string codeAvn { get; set; }

        [Column(Name = "CODERSQ")]
        public Int32 codeRsq { get; set; }

        [Column(Name = "ASSIETTE")]
        public double assiette { get; set; }

        [Column(Name = "VALEUR")]
        public double val { get; set; }

        [Column(Name = "UNITE")]
        public string unite { get; set; }

        [Column(Name = "CODETAXE")]
        public string codeTaxe { get; set; }

        [Column(Name = "TOPFRC")]
        public string topFrc { get; set; }

        [Column(Name = "PRIMEPRO")]
        public string PrimePro { get; set; }

        public string ErrorStr { get; set; }

        [Column(Name = "MOTIF")]
        public string motif { get; set; }

        [Column(Name = "CODEFORMULE")]
        public int CodeFormule { get; set; }

        [Column(Name = "CODEOPTION")]
        public int CodeOption { get; set; }

        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }

        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
    }
}
