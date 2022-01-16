using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.Assures
{
    public class AssurePlatDto
    {
        [Column(Name = "CODEASSU")]
        public Int32 Code { get; set; }
        [Column(Name = "NOMASSURE")]
        public string NomAssure { get; set; }
        [Column(Name = "NOMSECONDAIRE")]
        public String NomSecondaire { get; set; }
        [Column(Name = "NUMEROCHRONO")]
        public Int32 NumeroChrono { get; set; }
        public string MatriculeHexavia { get; set; }
        [Column(Name = "BATIMENT")]
        public string Batiment { get; set; }
        [Column(Name = "NUMEROVOIE")]
        public Int16 NumeroVoie { get; set; }
        [Column(Name = "EXTENSIONVOIE")]
        public string ExtensionVoie { get; set; }
        [Column(Name = "NOMVOIE")]
        public string NomVoie { get; set; }
        [Column(Name = "BOITEPOSTALE")]
        public string BoitePostale { get; set; }
        public string CodeInsee { get; set; }
        [Column(Name = "DEPARTEMENT")]
        public string Departement { get; set; }
        [Column(Name = "NOMVILLE")]
        public string NomVille { get; set; }
        [Column(Name = "CODEPOSTAL")]
        public Int16 CodePostal { get; set; }
        [Column(Name = "NOMCEDEX")]
        public string NomCedex { get; set; }
        [Column(Name = "CODEPOSTALCEDEX")]
        public Int16 CodePostalCedex { get; set; }
        [Column(Name = "TYPECEDEX")]
        public string TypeCedex { get; set; }
        [Column(Name = "CODEPAYS")]
        public string CodePays { get; set; }
        [Column(Name = "NOMPAYS")]
        public string NomPays { get; set; }
        [Column(Name = "SIREN")]
        public Int32 Siren { get; set; }
        [Column(Name = "TELEPHONEBUREAU")]
        public string TelephoneBureau { get; set; }
        [Column(Name = "NBSIN")]
        public int NombreSinistres { get; set; }
        public int Retards { get; set; }
        public int Impayes { get; set; }
        public bool EstActif { get; set; }

    }
}
