using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModeleModeles;
using EmitMapper;
using OP.WSAS400.DTO.Bloc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OP.WSAS400.DTO.LibelleClauses;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleLibellesClauses
{
    [Serializable]
    public class ModeleClause
    {
        public string GuidId { get; set; }

        public String Branche { get; set; }

        public String Cible { get; set; }

        public String Nomenclature1 { get; set; }

        public String Nomenclature2 { get; set; }

        public int? Nomenclature3 { get; set; }

        public String Libellespecifique { get; set; }



        public ModeleClause()
        {
            this.GuidId = string.Empty;
            this.Branche = string.Empty;
            this.Cible = string.Empty;
            this.Nomenclature1 = string.Empty;
            this.Nomenclature2 = string.Empty;
            this.Nomenclature3 = 0;
            this.Libellespecifique = string.Empty;

        }
        public static explicit operator ModeleClause(LibelleClauseDto LibelleClauseDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LibelleClauseDto, ModeleClause>().Map(LibelleClauseDto);
        }
    }
}