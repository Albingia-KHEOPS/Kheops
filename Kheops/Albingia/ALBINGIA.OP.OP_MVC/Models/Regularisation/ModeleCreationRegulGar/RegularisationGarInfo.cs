using EmitMapper;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegulGar
{
    public class RegularisationGarInfo
    {
        public long GrId { get; set; }

        public LigneGarantieRegul RegulPeriodDetail { get; set; }
        public RisqueDto AppliqueRegule { get; set; }
        public List<Mouvement> ListMvtPeriod { get; set; }
        public List<PerioderegulGarantie> ListPeriodRegulGar { get; set; }
        public string idLot { get; set; }
        public string  idregul { get; set; }
        public Int64 MouvementPeriodeDebMin { get; set; }
        public Int64 MouvementPeriodeFinMax { get; set; }
        
        public static explicit operator RegularisationGarInfo(RegularisationGarInfoDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RegularisationGarInfoDto, RegularisationGarInfo>().Map(modeleDto);
        }
    }

}