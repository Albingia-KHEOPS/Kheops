using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParamIS;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamISAssocier
{
    public class ParamISLigneModele
    {
        public int GuidId { get; set; }
        public string ModeleIS { get; set; }
        public string Referentiel { get; set; }
        public string ReferentielId { get; set; }
        public string Libelle { get; set; }
        public float NumOrdreAffichage { get; set; }
        public int SautDeLigne { get; set; }
        public string Modifiable { get; set; }
        public string Obligatoire { get; set; }
        public string Tcon { get; set; }
        public string Tfam { get; set; }
        public string Affichage { get; set; }
        public string Controle { get; set; }
        public int LienParent { get; set; }
        public string ParentComportement { get; set; }
        public string ParentEvenement { get; set; }
        public string Presentation { get; set; }
        public string Mappage { get; set; }

        public string TypeUI { get; set; }
        public string LienParentLibelle { get; set; }
        public string ScriptAffichage { get; set; }
        public string ScriptControle { get; set; }


        public List<AlbSelectListItem> ListObligatoire { get; set; }
        public List<AlbSelectListItem> ListModifiable { get; set; }
        public List<AlbSelectListItem> ListAffichage { get; set; }
        public List<AlbSelectListItem> ListControle { get; set; }
        //public List<AlbSelectListItem> ListReferentiel { get; set; }
        public List<AlbSelectListItem> ListPresentation { get; set; }
        //public List<AlbSelectListItem> ListLinkBehaviour { get; set; }     

        public static explicit operator ParamISLigneModele(ParamISLigneModeleDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamISLigneModeleDto, ParamISLigneModele>().Map(data);
        }

    }
}