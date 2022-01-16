using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.DocumentGestion;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamDocumentChemin
{
    public class ModeleLigneDocumentChemin
    {
        public string IdChemin { get; set; }
        public string LibelleChemin { get; set; }
        public string Chemin { get; set; }
        public string Type { get; set; }
        public string Typologie { get; set; }
        public string Serveur { get; set; }
        public string Racine { get; set; }
        public string Environnement { get; set; } 

        public List<AlbSelectListItem> ListeTypes { get; set; }
        public List<AlbSelectListItem> ListeTypologies { get; set; }

        public static explicit operator ModeleLigneDocumentChemin(LigneCheminDocumentDto data)
        {
            ModeleLigneDocumentChemin toReturn = ObjectMapperManager.DefaultInstance.GetMapper<LigneCheminDocumentDto, ModeleLigneDocumentChemin>().Map(data);
            if (!string.IsNullOrEmpty(toReturn.IdChemin))
            {
                if (toReturn.IdChemin.Split('_').Length > 1 && !string.IsNullOrEmpty(toReturn.IdChemin.Split('_')[1]))
                {
                    toReturn.Typologie = toReturn.IdChemin.Split('_')[1];                
                }
            }
            return toReturn;
        }
    }
}