using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class MaterielAssure : Materiel
    {
        public string Reference { get; set; }

        public LabeledValue Code { get; set; }
    }
}