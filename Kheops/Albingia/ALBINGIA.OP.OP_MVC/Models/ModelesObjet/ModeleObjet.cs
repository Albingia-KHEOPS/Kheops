using ALBINGIA.OP.OP_MVC.Models.ModeleMatriceFormule;
//using EmitMapper;
using ALBINGIA.OP.OP_MVC.Models.ModelesInventaire;
using EmitMapper;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using System;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesObjet
{
    [Serializable]
    public class ModeleObjet
    {
        public string Code { get; set; }
        public string Designation { get; set; }
        public List<ModeleInventaire> Inventaires { get; set; }

        //ajout pour la matrice Risque/Formule
        public bool hasInventaires { get; set; }
        public List<ModeleMatriceFormuleForm> Formules { get; set; }
        public bool isAffecte { get; set; }
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
        public string CodeAlpha { get; set; }

        public string Action { get; set; }

        public bool IsAlertePeriode { get; set; }
        public bool IsUsed { get; set; }
        public bool IsOut { get; set; }

        public string CodeTypeRegule { get; set; }
        public string LibTypeRegule { get; set; }

        public decimal ValPorteeObj { get; set; }
        public string UnitPorteeObj { get; set; }
        public string TypePorteeCal { get; set; }
        public double PrimeMntCal { get; set; }


        public ModeleObjet()
        {
            this.Code = string.Empty;
            this.Designation = string.Empty;
            this.Inventaires = new List<ModeleInventaire>();
        }

        public static explicit operator ModeleObjet(ObjetDto objetDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ObjetDto, ModeleObjet>().Map(objetDto);
        }


    }
}
