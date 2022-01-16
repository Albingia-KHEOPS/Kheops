using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleGestionDocument;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleGestionDocumentPage : MetaModelsBase
    {
        public List<ModeleGestionDistribution> Distributions { get; set; }

        public long RgId
        {
            get
            {
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.REGULEID), out long id);
                return id;
            }
        }

        //public static explicit operator ModeleGestionDocumentPage(GestionDocumentDto gestionDocumentDto)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<GestionDocumentDto, ModeleGestionDocumentPage>().Map(gestionDocumentDto);
        //}

        //public static GestionDocumentDto LoadDto(ModeleGestionDocumentPage modele)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<ModeleGestionDocumentPage, GestionDocumentDto>().Map(modele);
        //}

    }
}