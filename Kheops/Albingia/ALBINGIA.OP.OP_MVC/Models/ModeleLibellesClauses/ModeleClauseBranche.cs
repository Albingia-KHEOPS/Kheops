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
    public class ModeleClauseBranche
    {

       
        public String Code { get; set; }

        
        public String Libelle { get; set; }

        public ModeleClauseBranche()
        {
            this.Code = string.Empty;
            this.Libelle = string.Empty;

        }



       public static explicit operator ModeleClauseBranche(ClauseBrancheDto ClauseBrancheDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ClauseBrancheDto, ModeleClauseBranche>().Map(ClauseBrancheDto);
        }


    }
}