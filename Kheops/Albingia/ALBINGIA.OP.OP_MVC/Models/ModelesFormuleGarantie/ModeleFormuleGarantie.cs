using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using EmitMapper;
using OP.WSAS400.DTO.FormuleGarantie;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    [Serializable]
    public class ModeleFormuleGarantie
    {
        public bool FormuleChecked { get; set; }
        public List<ModeleVolet> Volets { get; set; }

        public string CodeOption { get; set; }

        public bool FullScreen { get; set; }
        public bool IsReadOnly { get; set; }

        public List<AlbSelectListItem> ParamNatMods { get; set; }
        
        public ModeleFormuleGarantie()
        {
            FormuleChecked = false;
            Volets = new List<ModeleVolet>();
            ParamNatMods = new List<AlbSelectListItem>();
        }
        public static explicit operator ModeleFormuleGarantie(FormuleGarantieDto formuleGarantieDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FormuleGarantieDto, ModeleFormuleGarantie>().Map(formuleGarantieDto);
        }

        public static FormuleGarantieDto LoadDto(ModeleFormuleGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormuleGarantie, FormuleGarantieDto>().Map(modele);
        }


    }
}