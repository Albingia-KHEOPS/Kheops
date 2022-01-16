using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCotisations;
using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleCotisationsPage : MetaModelsBase
    {
        public ModeleInfoGarantie GarantiesInfo { get; set; }

        [Display(Name = "Commentaires")]
        public string CommentForce { get; set; }
        public bool MontantForce { get; set; }
        public bool TraceCC { get; set; }
        public bool isChecked { get; set; }

        public ModeleCotisationsPage()
        {
            this.GarantiesInfo = new ModeleInfoGarantie();
        }

        public static explicit operator ModeleCotisationsPage(CotisationsDto CotisationsDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CotisationsDto, ModeleCotisationsPage>().Map(CotisationsDto);
        }

        public static CotisationsDto LoadDto(ModeleCotisationsPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleCotisationsPage, CotisationsDto>().Map(modele);
        }

    }
}