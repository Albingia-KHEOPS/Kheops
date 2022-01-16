using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreCibles;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class DetailsCible
    {
        public string Mode { get; set; }
        public string GuidId { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string DateCreation { get; set; }
        public string Grille { get; set; }
        public string CodeConcept { get; set; }
        //public string LibelleConcept { get; set; }
        public string CodeFamilleRecherche { get; set; }
        public List<AlbSelectListItem> Grilles { get; set; }

        public List<LigneBSC> ListeBSC { get; set; }

        public DropListBranches ListeBranches { get; set; }
        public DropListSousBranches ListeSousBranches { get; set; }
        public DropListCategories ListeCategories { get; set; }

        public static explicit operator DetailsCible(ParamCiblesDto paramCiblesDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamCiblesDto, DetailsCible>().Map(paramCiblesDto);
        }

    }
}