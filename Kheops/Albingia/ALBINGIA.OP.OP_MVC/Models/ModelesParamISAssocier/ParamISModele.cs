using EmitMapper;
using OP.WSAS400.DTO.ParamIS;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamISAssocier
{
    public class ParamISModele
    {
        public string NomModele { get; set; }
        public string Description { get; set; }
        public int DateDebut { get; set; }
        public int DateFin { get; set; }
        public string SqlSelect { get; set; }
        public string SqlInsert { get; set; }
        public string SqlUpdate { get; set; }
        public string SqlExist { get; set; }
        public string ScriptAffichage { get; set; }
        public string ScriptControle { get; set; }
        public List<ParamISLigneModele> ListeLignesModele { get; set; }
        public ParamISLigneModele LigneVide { get; set; }
        public bool ModeAssociation { get; set; }

        public static explicit operator ParamISModele(ModeleISDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleISDto, ParamISModele>().Map(data);
        }

    }
}