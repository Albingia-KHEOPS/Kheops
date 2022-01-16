using OP.WSAS400.DTO.Regularisation;
using System;

namespace ALBINGIA.OP.OP_MVC.Models
{
    [Serializable]
    public class RegularisationStateModel : BasicStateModel
    {
        public RegularisationStateModel() : base() { }

        public RegularisationStep Step { get; set; }

        public int TreeLevel { get; set; }
    }
}