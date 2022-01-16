using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocs;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleBlocPage: MetaModelsBase
    {
        public ModeleRechercheBlocs RechercheBlocs { get; set; }
        public List<ModeleBloc> Blocs { get; set; }
        public ModeleBloc Bloc { get; set; }

        public ModeleBlocPage()
        {
            this.RechercheBlocs = new ModeleRechercheBlocs();
            this.Blocs = new List<ModeleBloc>();
            this.Bloc = new ModeleBloc();
        }
    }
}