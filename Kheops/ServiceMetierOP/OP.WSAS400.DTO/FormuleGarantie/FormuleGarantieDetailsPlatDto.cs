using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class FormuleGarantieDetailsPlatDto
    {
        [Column(Name = "CODEGARANTIE")]
        public Int64 CodeGarantie { get; set; }
        [Column(Name = "LIENDTLBLOC")]
        public Int64 LienDtlBloc { get; set; }
        [Column(Name = "CARACTERE")]
        public String Caractere { get; set; }
        [Column(Name = "NATURE")]
        public String NatureStd { get; set; }
        [Column(Name = "NATRETENUE")]
        public String Nature { get; set; }
        [Column(Name = "DATEDEBW")]
        public Int64 DateDebW { get; set; }
        [Column(Name = "HEUREDEBW")]
        public Int32 HeureDebW { get; set; }
        [Column(Name = "DATEFINW")]
        public Int64 DateFinW { get; set; }
        [Column(Name = "HEUREFINW")]
        public Int32 HeureFinW { get; set; }
        [Column(Name = "DATEDEB")]
        public Int32 DateDeb { get; set; }
        [Column(Name = "HEUREDEB")]
        public Int32 HeureDeb { get; set; }
        [Column(Name = "DATEFIN")]
        public Int32 DateFin { get; set; }
        [Column(Name = "HEUREFIN")]
        public Int32 HeureFin { get; set; }
        [Column(Name = "DUREE")]
        public Int32 Duree { get; set; }
        [Column(Name = "DUREEUNITE")]
        public String DureeUnite { get; set; }
        [Column(Name = "GARANTIEINDEXE")]
        public String GarantieIndexe { get; set; }
        [Column(Name = "SOUMISCATNAT")]
        public String CATNAT { get; set; }
        [Column(Name = "MONTANTREF")]
        public String InclusMontant { get; set; }
        [Column(Name = "APPLICATION")]
        public String Application { get; set; }
        [Column(Name = "TYPEMISSION")]
        public String TypeEmission { get; set; }
        [Column(Name = "CODETAXE")]
        public String CodeTaxe { get; set; }
        [Column(Name = "DEFGARANTIE")]
        public String DefGarantie { get; set; }
        [Column(Name = "GARANTIE")]
        public String Garantie { get; set; }
        [Column(Name = "LIBGARANTIE")]
        public String LibelleGarantie { get; set; }
        [Column(Name = "TYPECONTROLEDATE")]
        public String TypeControleDate { get; set; }
        [Column(Name = "LCI")]
        public string LCIW { get; set; }
        [Column(Name = "FRANCHISE")]
        public string FranchiseW { get; set; }
        [Column(Name = "ASSIETTE")]
        public string AssietteW { get; set; }
        [Column(Name = "CATNAT")]
        public string CATNATW { get; set; }
        [Column(Name = "CODEOBJET")]
        public Int32 CodeObjet { get; set; }
        [Column(Name = "LONGDATEDEB")]
        public Int64 LongDateDeb { get; set; }
        [Column(Name = "LONGDATEFIN")]
        public Int64 LongDateFin { get; set; }
        [Column(Name = "PARENTNIV")]
        public Int64 ParentNiv { get; set; }
        [Column(Name = "ALIMASSIETTE")]
        public string AlimAssiette { get; set; }
        [Column(Name = "ISREGUL")]
        public string IsRegul { get; set; }
        [Column(Name = "LIBGRILLEREGUL")]
        public string LibGrilleRegul { get; set; }

        [Column(Name = "GARANTIEINDEX")]
        public string GarantieIndexeW { get; set; }
        [Column(Name="CREATENUMAVN")]
        public Int64 CreateNumAvn { get; set; }
        [Column(Name="GARTEMP")]
        public string GarTemp { get; set; }

    }
}
