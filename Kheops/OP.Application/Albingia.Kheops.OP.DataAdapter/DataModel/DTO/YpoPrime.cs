using System;

namespace Albingia.Kheops.OP.DataAdapter.DataModel.DTO {
    public class YpoPrime : YPrime {
        public string Pbbra { get; set; }
        public int Pbavn { get; set; }
        public DateTime DateValidation { get; set; }
        public DateTime DateEcheance { get; set; }
    }
}
