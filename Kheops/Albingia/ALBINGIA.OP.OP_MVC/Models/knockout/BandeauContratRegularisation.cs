using System.Runtime.Serialization;

namespace ALBINGIA.OP
{
    [DataContract]
    public class BandeauContratRegularisation
    {
        public string CodeOffre { get; set; }

        public string Version { get; set; }

        public string TypeContrat { get; set; }

        public string DisplayTypeContrat { get; set; }

        public string Identification { get; set; }

        public string Assure { get; set; }

        public string Souscripteur { get; set; }

        public string DateDebutEffet { get; set; }

        public string DateFinEffet { get; set; }

        public string Periodicite { get; set; }

        public string Echeance { get; set; }

        public string NatureContrat { get; set; }

        public string Courtier { get; set; }

        public string RetourPiece { get; set; }

        public string Observation { get; set; }

        public string Gestionnaire { get; set; }

        public string ContratMere { get; set; }

        public bool IsLightVersion { get; set; }

    }
}