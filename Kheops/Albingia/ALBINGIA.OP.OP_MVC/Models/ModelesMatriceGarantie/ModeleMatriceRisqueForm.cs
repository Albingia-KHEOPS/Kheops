using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesMatriceGarantie
{
    [Serializable]
    public class ModeleMatriceRisqueForm
    {
        public string Code { get; set; }
        public string Designation { get; set; }
        public List<ModelesMatriceGarantieOpt> Options { get; set; }
    }
}