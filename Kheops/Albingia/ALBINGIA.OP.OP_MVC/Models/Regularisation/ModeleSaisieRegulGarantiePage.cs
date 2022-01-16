using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleSaisieRegulGarantie;
using OP.WSAS400.DTO.Offres.Risque;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation
{
    public class ModeleSaisieRegulGarantiePage : MetaModelsBase
    {
        public SaisieInfoRegulPeriod modelInfoInitregul { get; set; }
        
        public RisqueDto AppliqueA { get; set; }
        public string CodeGar { get; set; }
        public string lotId { get; set; }
        public string codeOpt { get; set; }
        public string idGar { get; set; }
        public string reguleId { get; set; }
        public string codeFor { get; set; }
        public string codeRsq { get; set; }
        public string libGar { get; set; }
        public string idregulgar { get; set; }
       

        

       
    }
}