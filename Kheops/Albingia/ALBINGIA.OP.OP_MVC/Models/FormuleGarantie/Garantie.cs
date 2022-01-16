using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public class Garantie : OptionItem
    {
        public int Avenant { get; set; }

        public int AvenantInitial { get; set; }

        public int AvenantMAJ { get; set; }

        public string CodeBloc { get; set; }

        public DateTime? DateSortie { get; set; }

        public int Niveau { get; set; }

        public bool IsFlagModifie { get; set; }

        public LabeledValue NatureRetenue { get; set; }
        public LabeledValue Nature { get; set; }

        public bool NatureModifiable { get; set; }

        public bool InventairePossible { get; set; }

        public LabeledValue TypeAlimentation { get; set; }

        public long IdInventaire { get; set; }

        public Inventaires.TypeInventaire TypeInventaire { get; set; }

        public List<Garantie> SousGaranties { get; set; }

        public bool HasPortees { get; set; }

        public bool IsHidden { get; set; }

        public PorteesGarantie Portees { get; set; }

        public IEnumerable<LabeledValue> ActionsPortees { get; set; }

        public bool HasTariffs {
            get {
                return TypeAlimentation?.Code == "B" || TypeAlimentation?.Code == "C";
            }
        }

        public bool MayHavePortee {
            get {
                return HasPortees || HasTariffs;
            }
        }
    }
}