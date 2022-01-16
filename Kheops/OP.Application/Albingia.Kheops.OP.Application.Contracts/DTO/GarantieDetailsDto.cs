
using System;

namespace Albingia.Kheops.DTO
{
    public class GarantieDetailsDto
    {
        public long Sequence { get; set; }

        public string CodeBloc { get; set; }

        public string Code { get; set; }

        public string Libelle { get; set; }

        public string CodeCaractere { get; set; }

        public string Definition { get; set; }

        public string CodeNature { get; set; }

        public OP.Domain.Referentiel.NatureValue NatureRetenue { get; set; }

        public string LabelNatureRetenue { get; set; }

        public DateTime DateDebutStd { get; set; }

        public DateTime? DateFinStd { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public bool IsDuree { get; set; }

        public int? Duree { get; set; }

        public string CodeDureeUnite { get; set; }

        public bool IsGarantieIndexee { get; set; }

        public bool HasLCI { get; set; }

        public bool HasFranchise { get; set; }

        public bool HasAssiette { get; set; }

        public bool HasCATNAT { get; set; }

        public bool IsTemporaire { get; set; }

        public string CodeTypeApplication { get; set; }

        public bool InclusMontant { get; set; }

        public string CodeTypeEmission { get; set; }

        public bool Regularisable { get; set; }

        public string CodeTaxe { get; set; }

        public string CodeAlimentationAssiette { get; set; }

        public string CodeGrilleRegul { get; internal set; }

        public string LabelGrilleRegul { get; internal set; }

        public int NumeroAvenantCreation { get; set; }
    }
}
