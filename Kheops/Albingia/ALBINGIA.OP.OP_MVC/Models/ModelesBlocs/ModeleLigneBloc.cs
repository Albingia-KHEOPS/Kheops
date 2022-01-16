using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Bloc;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesBlocs
{
    public class ModeleLigneBloc
    {
        public string GuidId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public List<AlbSelectListItem> ListeReferentielBlocsIncompatibles { get; set; }
        public List<AlbSelectListItem> ListeReferentielBlocsAssocies { get; set; }

        public static explicit operator ModeleLigneBloc(BlocDto BlocDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<BlocDto, ModeleLigneBloc>().Map(BlocDto);
        }
    }
}