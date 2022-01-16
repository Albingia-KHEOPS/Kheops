using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleMatriceFormule;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using EmitMapper;
using OP.WSAS400.DTO.MatriceFormule;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleMatriceFormulePage : MetaModelsBase
    {
        public List<ModeleRisque> Risques { get; set; }
        public List<ModeleMatriceFormuleForm> Formules { get; set; }
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }

        public bool FormGen { get; set; }
        public bool HasFormGen { get; set; }
        public ModeleMatriceFormuleForm Formule { get; set; }

        public bool IsModeAvenant { get; set; }
        public bool AddFormule { get; set; }
        
        public static explicit operator ModeleMatriceFormulePage(MatriceFormuleDto matriceFormDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<MatriceFormuleDto, ModeleMatriceFormulePage>().Map(matriceFormDto);
        }

        public static MatriceFormuleDto LoadDto(ModeleMatriceFormulePage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleMatriceFormulePage, MatriceFormuleDto>().Map(modele);
        }

    }
}