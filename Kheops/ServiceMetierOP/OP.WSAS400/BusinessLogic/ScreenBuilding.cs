using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OP.WSAS400.DTO.Offres.Parametres;
using EmitMapper;
using OP.WSAS400.DataLogic;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.BusinessLogic
{
    public class ScreenBuilding
    {

        internal static List<ParametreDto> MotifsRefusGet(IOAS400 _dataContext)
        {
            List<ParametreDto> toReturn = new List<ParametreDto>
            (
                ObjectMapperManager.DefaultInstance.GetMapper
                <
                    List<OP.CoreDomain.Parametre>,
                    List<ParametreDto>
                >()
                .Map(_dataContext.MotifsRefusGet())
                .AsEnumerable()
            );
            return toReturn;
        }

        internal static DTO.Offres.OffreDto OffreGet(IOAS400 _dataContext, string codeOffre, Nullable<int> version)
        {
            OffreDto toReturn = ObjectMapperManager.DefaultInstance.GetMapper
            <
                OP.CoreDomain.Offre,
                OffreDto
            >()
            .Map(_dataContext.OffreGet(codeOffre,version));
            return toReturn;
        }
    }
}