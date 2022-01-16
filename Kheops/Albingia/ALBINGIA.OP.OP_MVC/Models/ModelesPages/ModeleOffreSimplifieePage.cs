using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleOffreSimplifieePage : MetaModelsBase
    {
      public string SpecificParams { get; set; }
      public string FileName { get; set; }
      public string NouvelleVersion { get; set; }
    }
}