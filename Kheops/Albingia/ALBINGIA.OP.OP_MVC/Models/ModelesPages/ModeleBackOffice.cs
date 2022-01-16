using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleBackOffice : MetaModelsBase
    {
        string Id { get; set; }
        string Libelle { get; set; }

        public ModeleBackOffice()
        {
            Id = string.Empty;
            Libelle = string.Empty;
        }
    }
}