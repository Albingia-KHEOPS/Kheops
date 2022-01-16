using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using EmitMapper;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.ControleFin;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleControleFinPage : MetaModelsBase, IRegulModel
    {
        public List<ModeleControleFinControle> ModeleControleFinControles { get; set; }

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

        public ModeleControleFinPage()
        {
            ModeleControleFinControles = new List<ModeleControleFinControle>();
        }

        public static explicit operator ModeleControleFinPage(ControleFinDto controleFinDto)
        {
            ModeleControleFinPage modeleControleFinPage = new ModeleControleFinPage();
            foreach (ControleFinControleDto controleFinControleDto in controleFinDto.ControleFinListeControleDto)
            {
                modeleControleFinPage.ModeleControleFinControles.Add(new ModeleControleFinControle() {
                    Etape = controleFinControleDto.Etape,
                    Risque = controleFinControleDto.Risque.ToString(),
                    Objet = controleFinControleDto.Objet.ToString(),
                    IdInventaire = controleFinControleDto.IdInventaire.ToString(),
                    NumeroLigneInventaire = controleFinControleDto.NumeroLigneInventaire.ToString(),
                    Formule = controleFinControleDto.Formule.ToString(),
                    Option = controleFinControleDto.Option.ToString(),
                    Garantie = controleFinControleDto.Garantie,
                    Message = controleFinControleDto.Message,
                    Niveau = controleFinControleDto.Niveau,
                    Reference = controleFinControleDto.Reference,
                    LienReference = controleFinControleDto.LienReference
                });
            }
            return modeleControleFinPage;
        }

        public static ControleFinDto LoadDto(ModeleControleFinPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleControleFinPage, ControleFinDto>().Map(modele);
        }
    }
}