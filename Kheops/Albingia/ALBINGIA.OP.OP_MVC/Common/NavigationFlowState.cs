using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common;

namespace ALBINGIA.OP.OP_MVC.Common {
    public class NavigationFlowState : ICloneable {
        private FlowStep currentFlowState;
        private FlowStep previousFlowState;

        public Folder CurrentFolder { get; set; }
        public Folder SourceFolder { get; set; }

        public FlowName FlowName { get; set; }
        public FlowAccessMode RequestedAccessMode { get; set; }
        public TraitementAffaire ActeGestion { get; set; }
        public FlowStep CurrentFlowStep { get => currentFlowState; set { previousFlowState = value; currentFlowState = value; } }
        public FlowStep PreviousFlowStep { get => previousFlowState; set => previousFlowState = value; }


        public object Clone() {
            var s = (NavigationFlowState)this.MemberwiseClone();
            s.CurrentFolder = (Folder)s.CurrentFolder?.Clone();
            s.SourceFolder = (Folder)s.SourceFolder?.Clone();
            return s;
        }
    }
}