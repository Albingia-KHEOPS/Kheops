using EmitMapper;
using OP.WSAS400.DTO.Engagement;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleConnexite
    {
        public Int64 NumConnexite { get; set; }

        public string CodeAffaire1 { get; set; }
        public Int32 Version1 { get; set; }
        public string Type1 { get; set; }
        public string CodeBranche1 { get; set; }
        public string LibBranche1 { get; set; }
        public string CodeCible1 { get; set; }
        public string LibCible1 { get; set; }
        public Int32 CodeAss1 { get; set; }
        public string NomAss1 { get; set; }
        public string Ad1_1 { get; set; }
        public string Ad2_1 { get; set; }
        public string Dep1 { get; set; }
        public string Cp1 { get; set; }
        public string Ville1 { get; set; }

        public string CodeAffaire2 { get; set; }
        public Int32 Version2 { get; set; }
        public string Type2 { get; set; }
        public string CodeBranche2 { get; set; }
        public string LibBranche2 { get; set; }
        public string CodeCible2 { get; set; }
        public string LibCible2 { get; set; }
        public Int32 CodeAss2 { get; set; }
        public string NomAss2 { get; set; }
        public string Ad1_2 { get; set; }
        public string Ad2_2 { get; set; }
        public string Dep2 { get; set; }
        public string Cp2 { get; set; }
        public string Ville2 { get; set; }

        public Int64 CodeObsv { get; set; }
        public string Observations { get; set; }

        public static explicit operator ModeleConnexite(ConnexiteDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ConnexiteDto, ModeleConnexite>().Map(modeleDto);
        }

        public static ConnexiteDto LoadDto(ModeleConnexite modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleConnexite, ConnexiteDto>().Map(modele);
        }
    }
}