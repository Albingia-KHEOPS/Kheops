using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleDocumentGestionPage : MetaModelsBase, IRegulModel
    {
        public List<DocumentGestionDoc> ListeDocuments { get; set; }
        public bool IsErrorOccured { get; set; }
        public bool IsChekedEcheance { get; set; }

        public RegularisationContext Context { get; set; }

        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public long RgId
        {
            get
            {
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.REGULEID), out long id);
                return id;
            }
        }

        public long LotId
        {
            get
            {
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.LOTID), out long id);
                return id;
            }
        }
    }
}