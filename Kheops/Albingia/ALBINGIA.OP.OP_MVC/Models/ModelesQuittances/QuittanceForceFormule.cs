using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class QuittanceForceFormule
    {
        public string TypeHTTC { get; set; }
        public List<QuittanceForceInfoFormule> ListFormule { get; set; }
    }
}