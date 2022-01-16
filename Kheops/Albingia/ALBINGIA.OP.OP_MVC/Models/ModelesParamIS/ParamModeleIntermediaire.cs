using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamIS
{
    public class ParamModeleIntermediaire
    {
        //Liste lignes info
        public List<ParamModeleLigneInfo> ListLignesInfo { get; set; }
        //Ligne vide (en cas d'ajout)
        public ParamModeleLigneInfo LigneInfoVide { get; set; }

        //Types requêtes
        public ParamModeleSqlRequest SqlRequest { get; set; }

    }
}