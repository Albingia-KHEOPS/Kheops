using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.FormuleGarantie
{
    [DataContract]
    public class FormuleGarantieDetailsDto : _FormuleGarantie_Base
    {
        [DataMember]
        public string CodeGarantie { get; set; }
        [DataMember]
        public string LibelleGarantie { get; set; }
        [DataMember]
        public string Garantie { get; set; }

        [DataMember]
        public string Caractere { get; set; }
        [DataMember]
        public string NatureStd { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public List<ParametreDto> Natures { get; set; }
        [DataMember]
        public DateTime? DateDebStd { get; set; }
        [DataMember]
        public TimeSpan? HeureDebStd { get; set; }
        [DataMember]
        public DateTime? DateFinStd { get; set; }
        [DataMember]
        public TimeSpan? HeureFinStd { get; set; }
        [DataMember]
        public DateTime? DateDeb { get; set; }
        [DataMember]
        public TimeSpan? HeureDeb { get; set; }
        [DataMember]
        public DateTime? DateFin { get; set; }
        [DataMember]
        public TimeSpan? HeureFin { get; set; }
        [DataMember]
        public string Duree { get; set; }
        [DataMember]
        public string DureeUnite { get; set; }
        [DataMember]
        public List<ParametreDto> DureeUnites { get; set; }
        [DataMember]
        public bool GarantieIndexe { get; set; }
        [DataMember]
        public bool LCI { get; set; }
        [DataMember]
        public bool Franchise { get; set; }
        [DataMember]
        public bool Assiette { get; set; }
        [DataMember]
        public bool CATNAT { get; set; }
        [DataMember]
        public bool InclusMontant { get; set; }
        [DataMember]
        public string Application { get; set; }
        [DataMember]
        public List<ParametreDto> Applications { get; set; }
        [DataMember]
        public string TypeEmission { get; set; }
        [DataMember]
        public List<ParametreDto> TypesEmission { get; set; }
        [DataMember]
        public string CodeTaxe { get; set; }
        [DataMember]
        public List<ParametreDto> CodesTaxe { get; set; }
        [DataMember]
        public string DefGarantie { get; set; }
        [DataMember]
        public string AlimAssiette { get; set; }
        [DataMember]
        public List<ParametreDto> AlimAssiettes { get; set; }

        [DataMember]
        public string GarantieIndexeW { get; set; }
        [DataMember]
        public string LCIW { get; set; }
        [DataMember]
        public string FranchiseW { get; set; }
        [DataMember]
        public string AssietteW { get; set; }
        [DataMember]
        public string CATNATW { get; set; }
        [DataMember]
        public string TypeControleDate { get; set; }
        [DataMember]
        public bool AvnReadOnly { get; set; }
        [DataMember]
        public Int64 AvnCreation { get; set; }
        [DataMember]
        public string GarTemp { get; set; }
        [DataMember]
        public string IsRegul { get; set; }
        [DataMember]
        public string LibGrilleRegul { get; set; }


        public FormuleGarantieDetailsDto()
        {
            this.CodeGarantie = string.Empty;
            this.LibelleGarantie = string.Empty;
            this.Caractere = string.Empty;
            this.NatureStd = string.Empty;
            this.Nature = string.Empty;
            this.Natures = new List<ParametreDto>();
            this.DateDebStd = null;
            this.HeureDebStd = null;
            this.DateFinStd = null;
            this.HeureFinStd = null;
            this.DateDeb = null;
            this.HeureDeb = null;
            this.DateFin = null;
            this.HeureFin = null;
            this.Duree = string.Empty;
            this.DureeUnite = string.Empty;
            this.DureeUnites = new List<ParametreDto>();
            this.GarantieIndexe = false;
            this.LCI = false;
            this.Franchise = false;
            this.Assiette = false;
            this.CATNAT = false;
            this.InclusMontant = false;
            this.Application = string.Empty;
            this.Applications = new List<ParametreDto>();
            this.TypeEmission = string.Empty;
            this.TypesEmission = new List<ParametreDto>();
            this.CodeTaxe = string.Empty;
            this.CodesTaxe = new List<ParametreDto>();
            this.DefGarantie = string.Empty;

            this.GarantieIndexeW = string.Empty;
            this.LCIW = string.Empty;
            this.FranchiseW = string.Empty;
            this.AssietteW = string.Empty;
            this.CATNATW = string.Empty;
        }
    }
}

