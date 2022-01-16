using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmitMapper;
using ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    [Serializable]
    public class ModeleCabinetCourtage
    {
        public int? Code { get; set; }
        public string Nom { get; set; }
        public string[] NomSecondaires { get; set; }
        public string Rue { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Type { get; set; }
        public string NomInterlocuteur { get; set; }
        public string IdInterlocuteur { get; set; }
        public bool ValideInterlocuteur { get; set; }
        public string Delegation { get; set; }
        public bool EstValide { get; set; }

        public ModeleCabinetCourtage()
        {
            Code = null;
            Nom = String.Empty;
            CodePostal = String.Empty;
            Ville = String.Empty;
            Type = String.Empty;
            NomInterlocuteur = String.Empty;
            IdInterlocuteur = String.Empty;
            EstValide = true;
            Delegation = String.Empty;
        }

        public static explicit operator ModeleCabinetCourtage(CabinetCourtage_JSON_MetaData cabinetCourtageDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CabinetCourtage_JSON_MetaData, ModeleCabinetCourtage>().Map(cabinetCourtageDto);
        }
    }
}