using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie.GarantieModele
{
    public class GarantieTypePlatDto
    {
        public Int64 C2seq { get; set; }
        public string C2mga { get; set; }
        public string C2obe { get; set; }
        public int C2niv { get; set; }
        public string C2gar { get; set; }
        public int C2ord { get; set; }
        public string C2lib { get; set; }
        public Int64 C2sem { get; set; }
        public string C2car { get; set; }
        public string C2nat { get; set; }
        public string C2ina { get; set; }
        public string C2cna { get; set; }
        public string C2tax { get; set; }
        public int C2alt { get; set; }
        public string C2tri { get; set; }
        public Int64 C2se1 { get; set; }
        public string C2scr { get; set; }
        public string C2prp { get; set; }
        public string C2tcd { get; set; }
        public string C2mrf { get; set; }
        public string C2ntm { get; set; }
        public string C2mas { get; set; }

        public string Gades { get; set; }
        public string C2natlib { get; set; }
        public string C2carlib { get; set; }
        public bool C2inaBool => C2ina == "O";
        public bool C2cnaBool => C2cna == "O";
        public bool C2mrfBool => C2mrf == "O";
        public bool C2ntmBool => C2ntm == "O";
        public bool C2masBool => C2mas == "O";


        public string C4typ { get; set; }
        public string C4bas { get; set; }
        public Decimal C4val { get; set; }
        public string C4unt { get; set; }
        public string C4maj { get; set; }
        public string C4obl { get; set; }
        public bool C4oblBool => C4obl == "O";
        public string C4ala { get; set; }

        public string C5typ { get; set; }
        public Int64 C5seq { get; set; }
        public Int64 C5sem { get; set; }
        public string Garlieenom { get; set; }
        public string Garlieemodele { get; set; }
        public string Garlieeniv { get; set; }

        #region old properties
        //[Column(Name = "C2SEQ")]
        //public long NumeroSeq { get; set; }
        //[Column(Name = "C2MGA")]
        //public string CodeModele { get; set; }
        //[Column(Name = "C2OBE")]
        //public string NomModele { get; set; }
        //[Column(Name = "C2NIV")]
        //public int Niveau { get; set; }
        //[Column(Name = "C2GAR")]
        //public string CodeGarantie { get; set; }
        //[Column(Name = "C2ORD")]
        //public int Ordre { get; set; }
        //[Column(Name = "C2TRI")]
        //public string Tri { get; set; }
        //[Column(Name = "DESCRIPTION")]
        //public string Description { get; set; }
        //[Column(Name = "C2SEM")]
        //public long NumeroSeqM { get; set; }
        //[Column(Name = "C2SE1")]
        //public long NumeroSeq1 { get; set; }
        //[Column(Name = "C2CAR")]
        //public string Caractere { get; set; }
        //[Column(Name = "C2CARLIB")]
        //public string CaractereLib { get; set; }
        //[Column(Name = "C2NAT")]
        //public string Nature { get; set; }
        //[Column(Name = "C2NATLIB")]
        //public string NatureLib { get; set; }
        //[Column(Name = "C2INA")]
        //public string isIndexee { get; set; }
        //public bool IsIndexee => isIndexee == "O";
        //[Column(Name = "C2CNA")]
        //public string soumisCATNAT { get; set; }
        //public bool SoumisCATNAT => soumisCATNAT == "O";
        //[Column(Name = "C2TAX")]
        //public string CodeTaxe { get; set; }
        //[Column(Name = "C2ALT")]
        //public int GroupeAlternative { get; set; }
        //[Column(Name = "C2SCR")]
        //public string Conditionnement { get; set; }
        //[Column(Name = "C2PRP")]
        //public string TypePrime { get; set; }
        //[Column(Name = "C2TCD")]
        //public string TypeControleDate { get; set; }
        //[Column(Name = "C2MRF")]
        //public string isMontantRef { get; set; }
        //public bool IsMontantRef => isMontantRef == "O";
        //[Column(Name = "C2NTM")]
        //public string isNatureModifiable { get; set; }
        //public bool IsNatureModifiable => isNatureModifiable == "O";
        //[Column(Name = "C2MAS")]
        //public string isMasquerCP { get; set; }
        //public bool IsMasquerCP => isMasquerCP == "O";

        //[Column(Name = "C4TYP")]
        //public string Type { get; set; }
        //[Column(Name = "C4BAS")]
        //public string Base { get; set; }
        //[Column(Name = "C4UNT")]
        //public string Unite { get; set; }
        //[Column(Name = "C4VAL")]
        //public decimal Valeur { get; set; }
        //[Column(Name = "C4MAJ")]
        //public string Modi { get; set; }
        //[Column(Name = "C4OBL")]
        //public string obl { get; set; }
        //public bool Obl => obl == "O";
        //[Column(Name = "C4ALA")]
        //public string Alim { get; set; }

        //[Column(Name = "C5TYP")]
        //public string TypeLien { get; set; }
        //[Column(Name = "C5SEQ")]
        //public int GarantieA { get; set; }
        //[Column(Name = "C5SEM")]
        //public int GarantieB { get; set; }
        //[Column(Name = "GARLIEE")]
        //public string NomGarantieLiee { get; set; }
        #endregion
    }
}

