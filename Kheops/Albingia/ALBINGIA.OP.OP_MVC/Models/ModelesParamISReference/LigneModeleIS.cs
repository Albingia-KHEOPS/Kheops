using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParamIS;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamISReference
{
    public class LigneModeleIS
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string LibelleAffiche { get; set; }
        public string TypeZone { get; set; }
        public string LongueurZone { get; set; }
        public string Mappage { get; set; }
        public string Conversion { get; set; }
        public int Presentation { get; set; }
        public string PresentationLabel { get; set; }
        public string TypeUI { get; set; }
        public string Obligatoire { get; set; }
        public string Affichage { get; set; }
        public string Controle { get; set; }
        public string Observation { get; set; }
        public string ValeurDefaut { get; set; }
        public string Tcon { get; set; }
        public string Tfam { get; set; }

        public List<AlbSelectListItem> ListMappage { get; set; }
        public List<AlbSelectListItem> ListConversion { get; set; }
        public List<AlbSelectListItem> ListPresentation { get; set; }
        public List<AlbSelectListItem> ListTypesUI { get; set; }
        public List<AlbSelectListItem> ListObligatoire { get; set; }
        public List<AlbSelectListItem> ListAffichage { get; set; }
        public List<AlbSelectListItem> ListControle { get; set; }
        public List<AlbSelectListItem> ListTypeZone { get; set; }

        public static explicit operator LigneModeleIS(LigneModeleISDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneModeleISDto, LigneModeleIS>().Map(data);
        }
    }
}