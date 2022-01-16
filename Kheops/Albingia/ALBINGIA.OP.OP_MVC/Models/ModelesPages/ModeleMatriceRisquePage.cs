using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleMatriceFormule;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using EmitMapper;
using OP.WSAS400.DTO.MatriceRisque;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleMatriceRisquePage : MetaModelsBase 
    {
        public List<ModeleRisque> Risques { get; set; }
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public bool FormGen { get; set; }
        public bool HasFormGen { get; set; }
        public ModeleMatriceFormuleForm Formule { get; set; }
        public string StrDateDebutEffetGenerale { get; set; }
        public string StrDateFinEffetGenerale { get; set; }
        public bool AddFormule { get; set; }
        public bool CopyRisque
        {
            get;set;
        }
        public string formuleOption { get; set; }
        public static explicit operator ModeleMatriceRisquePage(MatriceRisqueDto matriceRsqDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<MatriceRisqueDto, ModeleMatriceRisquePage>().Map(matriceRsqDto);
        }

        public static MatriceRisqueDto LoadDto(ModeleMatriceRisquePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleMatriceRisquePage, MatriceRisqueDto>().Map(modele);
        }
    }
}