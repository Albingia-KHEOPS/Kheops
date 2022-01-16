using ALBINGIA.Framework.Common.Constants;

namespace OP.DataAccess.Data
{
    public class NouvelleAffaireData : AffaireBaseData {
        public override string Typ { get => AlbConstantesMetiers.TYPE_CONTRAT; set { } }
        public override int Avn { get => 0; set => base.Avn = 0; }
        public string OIpb { get; set; }
        public int OAlx { get; set; }
        public string Sit { get; set; }
        public string Typo { get; set; }
    }
}
