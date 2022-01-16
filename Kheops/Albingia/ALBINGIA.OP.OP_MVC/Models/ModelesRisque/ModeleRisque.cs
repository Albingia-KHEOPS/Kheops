using ALBINGIA.OP.OP_MVC.Models.ModeleMatriceFormule;
using ALBINGIA.OP.OP_MVC.Models.ModelesObjet;
using EmitMapper;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRisque
{
    [Serializable]
    public class ModeleRisque
    {
        public string Code { get; set; }
        public string Designation { get; set; }
        public List<ModeleObjet> Objets { get; set; }
        public string Flag { get; set; }

        //ajout pour la matrice Risque/Formule
        public List<ModeleMatriceFormuleForm> Formules { get; set; }
        public bool isAffecte { get; set; }
        public bool isCopiable { get; set; }
        public bool isBadDate { get; set; }

        //ajout pour la matrice Risque/Risque
        public string Formule { get; set; }
        public ParametreDto Cible { get; set; }
        public string Valeur { get; set; }
        public ParametreDto Unite { get; set; }
        public ParametreDto Type { get; set; }
        public DateTime? EntreeGarantie { get; set; }
        public string EntreeGarantieStr
        {
            get
            {
                if (EntreeGarantie.HasValue)
                    return EntreeGarantie.Value.Day.ToString().PadLeft(2, '0') + "/" + EntreeGarantie.Value.Month.ToString().PadLeft(2, '0') + "/" + EntreeGarantie.Value.Year;
                else
                    return string.Empty;
            }
        }
        public DateTime? SortieGarantie { get; set; }
        public string SortieGarantieStr
        {
            get
            {
                if (SortieGarantie.HasValue)
                    return SortieGarantie.Value.Day.ToString().PadLeft(2, '0') + "/" + SortieGarantie.Value.Month.ToString().PadLeft(2, '0') + "/" + SortieGarantie.Value.Year;
                else
                    return string.Empty;
            }
        }
        public bool isIndexe { get; set; }
        public bool hasInventaire { get; set; }
        public string CodeAlpha { get; set; }

        public bool IsAlertePeriode { get; set; }
        public bool IsUsed { get; set; }
        public bool IsOut { get; set; }

        public string CodeTypeRegule { get; set; }
        public string LibTypeRegule { get; set; }
        
        public ModeleRisque()
        {
            Code = string.Empty;
            Designation = string.Empty;
            Objets = new List<ModeleObjet>();
        }

        public static explicit operator ModeleRisque(RisqueDto risqueDto)
        {
            ModeleRisque model = ObjectMapperManager.DefaultInstance.GetMapper<RisqueDto, ModeleRisque>().Map(risqueDto);

            if (risqueDto.Cible != null)
            {
                model.Cible = new ParametreDto
                {
                    Id = Convert.ToInt32(risqueDto.Cible.GuidId),
                    Descriptif = risqueDto.Cible.Description
                };
            }

            return model;
        }
        
        public static RisqueDto LoadDto(ModeleRisque modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleRisque, RisqueDto>().Map(modele);
        }

    }
}