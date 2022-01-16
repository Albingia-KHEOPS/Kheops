using ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre;
using OP.WSAS400.DTO.Regularisation;
using System;

namespace ALBINGIA.OP.OP_MVC.Models.Regularisation
{
    [Serializable]
    public class NavTreeModel
    {
        public ModeleNavigationArbre NavTree { get; set; }

        public RegularisationStep Step { get; set; }

        public int RsqId { get; set; }

        public long GrId { get; set; }

        public long RgGrId { get; set; }
    }
}