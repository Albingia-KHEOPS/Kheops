using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleMatriceFormule;
using ALBINGIA.OP.OP_MVC.Models.ModelesMatriceGarantie;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using EmitMapper;
using OP.WSAS400.DTO.MatriceGarantie;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleMatriceGarantiePage : MetaModelsBase
    {
        public bool FormuleChecked { get; set; }
        public List<ModeleVolet> Volets { get; set; }
        public List<ModeleMatriceRisqueForm> Risques { get; set; }

        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }

        public bool FormGen { get; set; }
        public bool HasFormGen { get; set; }
        public ModeleMatriceFormuleForm Formule { get; set; }
        public bool AddFormule { get; set; }
        public string formuleOption { get; set; }
        public ModeleMatriceGarantiePage()
        {
            FormuleChecked = false;
            Volets = new List<ModeleVolet>();
        }
        public static explicit operator ModeleMatriceGarantiePage(MatriceGarantieDto formuleGarantieDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<MatriceGarantieDto, ModeleMatriceGarantiePage>().Map(formuleGarantieDto);
        }
    }
}