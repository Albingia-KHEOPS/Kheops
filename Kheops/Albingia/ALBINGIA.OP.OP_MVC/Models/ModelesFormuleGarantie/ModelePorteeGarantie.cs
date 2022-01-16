using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using EmitMapper;
using OP.WSAS400.DTO.FormuleGarantie;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    [Serializable]
    public class ModelePorteeGarantie
    {
        public bool isReadOnly { get; set; }
        public Int64 IdGarantie { get; set; }
        public Int64 SequenceGarantie { get; set; }
        public string CodeGarantie { get; set; }
        public string Garantie { get; set; }
        public string LibelleGarantie { get; set; }

        public Int64 IdPortee { get; set; }
        public string Action { get; set; }
        [Display(Name = "Action")]
        public List<AlbSelectListItem> Actions { get; set; }

        public ModeleRisque Risque { get; set; }

        public string AlimAssiette { get; set; }

        public List<AlbSelectListItem> UnitesTaux { get; set; }
        public List<AlbSelectListItem> TypesCalTaux { get; set; }

        public bool ReportCal { get; set; }

        public static explicit operator ModelePorteeGarantie(FormuleGarantiePorteeDto infoDto)
        {
            var model = ObjectMapperManager.DefaultInstance.GetMapper<FormuleGarantiePorteeDto, ModelePorteeGarantie>().Map(infoDto);

            model.Actions.Clear();
            infoDto.Actions.ToList().ForEach(
                elem => model.Actions.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );

            model.UnitesTaux = new List<AlbSelectListItem>();
            infoDto.UnitesTaux.ForEach(elm => model.UnitesTaux.Add(AlbSelectListItem.ConvertToAlbSelect(elm.Code, elm.Libelle, elm.Libelle)));

            model.TypesCalTaux = new List<AlbSelectListItem>();
            infoDto.TypesCalTaux.ForEach(elm => model.TypesCalTaux.Add(AlbSelectListItem.ConvertToAlbSelect(elm.Code, elm.Libelle, elm.Libelle)));

            return model;
        }

        public static FormuleGarantiePorteeDto LoadDto(ModelePorteeGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModelePorteeGarantie, FormuleGarantiePorteeDto>().Map(modele);
        }

    }
}