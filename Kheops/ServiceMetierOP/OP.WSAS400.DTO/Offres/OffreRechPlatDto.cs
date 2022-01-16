using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres
{
    public class OffreRechPlatDto
    {

        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [Column(Name = "CODEOFFREVERSION")]
        public string CodeOffreVersion { get; set; }
        [Column(Name = "TYPEOFFRE")]
        public string Type { get; set; }
        [Column(Name = "VERSION")]
        public Int32 Version { get; set; }
        [Column(Name = "CODEAVN")]
        public Int32 CodeAvn { get; set; }
        [Column(Name = "DATESAISIE")]
        public Int64 DateSaisie { get; set; } // 201704011100
        [Column(Name = "CODEBRANCHE")]
        public string CodeBranche { get; set; }
        [Column(Name = "CODESOUSBRANCHE")]
        public string CodeSousBranche { get; set; }
        [Column(Name = "LIBBRANCHE")]
        public string LibBranche { get; set; }
        [Column(Name = "CODECIBLE")]
        public string CodeCible { get; set; }
        [Column(Name = "LIBCIBLE")]
        public string LibCible { get; set; }
        [Column(Name = "CODECATEGORIE")]
        public string CodeCategorie { get; set; }
        [Column(Name = "CODEETAT")]
        public string CodeEtat { get; set; }
        [Column(Name = "LIBETAT")]
        public string LibEtat { get; set; }
        [Column(Name = "CODESIT")]
        public string CodeSit { get; set; }
        [Column(Name = "LIBSIT")]
        public string LibSit { get; set; }
        [Column(Name = "CODEQUALITE")]
        public string CodeQualite { get; set; }
        [Column(Name = "LIBQUALITE")]
        public string LibQualite { get; set; }
        [Column(Name = "DESCRIPTIF")]
        public string Descriptif { get; set; }
        [Column(Name = "CODEASS")]
        public Int32 CodeAss { get; set; }
        [Column(Name = "NOMASS")]
        public string NomAss { get; set; }
        [Column(Name = "CPASS")]
        public string CpAss { get; set; }
        [Column(Name = "VILASS")]
        public string VilleAss { get; set; }
        [Column(Name = "CODECOURT")]
        public Int32 CodeCourt { get; set; }
        [Column(Name = "CODECOURTAPPORT")]
        public Int32 CodeCourtierApporteur { get; set; }
        [Column(Name = "NOMCOURT")]
        public string NomCourt { get; set; }
        [Column(Name = "CPCOURT")]
        public string CpCourt { get; set; }
        [Column(Name = "VILCOURT")]
        public string VilleCourt { get; set; }
        [Column(Name = "TYPETRAITEMENT")]
        public string TypeTraitement { get; set; }
        [Column(Name = "TYPEACCORD")]
        public string TypeAccord { get; set; }
        [Column(Name = "STATUTKHEOPS")]
        public string StatutKheops { get; set; }
        [Column(Name = "AVNEXT")]
        public Int64 AvnExt { get; set; }   // PBAVK
        [Column(Name = "GENERDOC")]
        public Int64 GenerDoc { get; set; }
        [Column(Name = "TYPEPOLICE")]
        public string TypePolice { get; set; }
        [Column(Name = "MOTIFREFUS")]
        public string MotifRefus { get; set; }
        [Column(Name = "CODEPERIODICITE")]
        public string CodePeriodicite { get; set; }
        [Column(Name = "LINE")]
        public Int64 NbLine { get; set; }
        [Column(Name = "DATEEFFET")]
        public Int64 DateEffet { get; set; } // Pbefa/m/j
        [Column(Name = "DATECREATION")]
        public Int64 DateCreation { get; set; } // Pbefa/m/j        [Column(Name = "DATEFINEFFET")]
        [Column(Name = "DATEFINEFFET")]
        public Int64 DateFinEffet { get; set; } // Pbefa/m/j
        [Column(Name = "HEUREFINEFFET")] // 2359  Si = 24 = 23h59
        public Int32 HeureFinEffet { get; set; }
        [Column(Name = "HASSUSP")]
        public Int32 HasSusp { get; set; }
        [Column(Name = "DTFINSUSP")]
        public Int32 DtFinSusp { get; set; }
        [Column(Name = "DOUBLESAISIE")]
        public string DoubleSaisie { get; set; }
        [Column(Name = "MAXIDREGUL")]
        public Int64 MaxIdRegul { get; set; }
        [Column(Name = "TRTLOT")]
        public string TrtLot { get; set; }
        [Column(Name = "DATEMAJ")]
        public Int64 DateMaj { get; set; } // 201704011100

        public OffreRechPlatDto()
        {
            CodeOffre = "";
            Type = "";
            Version = 0;
            CodeAvn = 0;
            DateSaisie = 0;
            CodeBranche = "";
            LibBranche = "";
            CodeCible = "";
            LibCible = "";
            CodeEtat = "";
            LibEtat = "";
            CodeSit = "";
            LibSit = "";
            CodeQualite = "";
            LibQualite = "";
            Descriptif = "";
            CodeAss = 0;
            NomAss = "";
            CpAss = "";
            VilleAss = "";
            CodeCourt = 0;
            NomCourt = "";
            CpCourt = "";
            VilleCourt = "";
            TypeTraitement = "";
            TypeAccord = "";
            StatutKheops = "";
            AvnExt = 0;
            GenerDoc = 0;
            TypePolice = "";
            MotifRefus = "";
            CodePeriodicite = "";
            NbLine = 0;
            DateFinEffet = 0;
            HeureFinEffet = 0;
            HasSusp = 0;
            DoubleSaisie = "N";
            MaxIdRegul = 0;
            //TrtLot = "";
            DateMaj = 0;
        }
    }
}
