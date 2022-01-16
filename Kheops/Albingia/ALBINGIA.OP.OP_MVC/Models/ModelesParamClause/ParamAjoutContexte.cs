using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using EmitMapper;
using OP.WSAS400.DTO.ParametreClauses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamClause
{
    public class ParamAjoutContexte
    {
        public Int64 IdContexte { get; set; }
        public string CodeService { get; set; }
        [Display(Name = "Service")]
        public string LibelleService { get; set; }
        public string CodeActGes { get; set; }
        [Display(Name = "Acte de gestion")]
        public string LibelleActGes { get; set; }
        public string CodeEtape { get; set; }
        [Display(Name = "Etape")]
        public string LibelleEtape { get; set; }
        public string CodeContexte { get; set; }
        public string LibelleContexte { get; set; }
        [Display(Name = "Contexte")]
        public string Contexte { get; set; }
        public List<AlbSelectListItem> Contextes { get; set; }
        public string CodeSpecificite { get; set; }
        public string LibelleSpecificite { get; set; }
        [Display(Name = "Specificite")]
        public string Specificite { get; set; }
        public List<AlbSelectListItem> Specificites { get; set; }
        [Display(Name="Emplacement modifiable")]
        public bool EmplacementModif { get; set; }
        [Display(Name="Ajout Clausier")]
        public bool AjoutClausier { get; set; }
        [Display(Name="Ajout Libre")]
        public bool AjoutLibre { get; set; }
        [Display(Name="Script controle")]
        public string ScriptControle { get; set; }
        public List<AlbSelectListItem> ScriptsControle { get; set; }
        [Display(Name="Clause modèle")]
        public string Rubrique { get; set; }
        public List<AlbSelectListItem> Rubriques { get; set; }
        public ModeleDDLSousRubrique ModeleDDLSousRubrique { get; set; }
        public ModeleDDLSequence ModeleDDLSequence { get; set; }

        [Display(Name="Emplacement")]
        public string Emplacement { get; set; }
        public List<AlbSelectListItem> Emplacements { get; set; }
        [Display(Name = "Sous emplacement")]
        public string SousEmplacement { get; set; }
        public List<AlbSelectListItem> SousEmplacements { get; set; }
        [Display(Name = "Num Ord.")]
        public string NumOrdonnancement { get; set; }
        public List<AlbSelectListItem> NumOrdonnancements { get; set; }

        public static explicit operator ParamAjoutContexte(ParamAjoutContexteDto paramAjoutContexteDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutContexteDto, ParamAjoutContexte>().Map(paramAjoutContexteDto);
        }

        public static ParamAjoutContexteDto LoadDto(ParamAjoutContexte modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ParamAjoutContexte, ParamAjoutContexteDto>().Map(modele);
        }
    }
}