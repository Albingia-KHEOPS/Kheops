using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using System;

namespace Albingia.Kheops.DTO
{
    public class FolderLockDto : FolderDto {
        public FolderLockDto() {
            Now = DateTime.Now;
        }

        public Guid? TabGuid { get; set; }
        public string ActeGestion { get; set; }
        public DateTime Now { get; }

        public string Name {
            get {
                if (TypeAffaire == AffaireType.Offre) {
                    return "offre";
                }
                else {
                    if (ActeGestion == AlbConstantesMetiers.TRAITEMENT_AFFNV) {
                        return "contrat";
                    }
                    return "avenant";
                }
            }
        }

        public string BuildId(string separator = " ") {
            return $"{CodeAffaire.Trim()}{separator}({NumeroAliment}){(NumeroAvenant > 0 ? $"{separator}AVN{NumeroAvenant}" : string.Empty)}";
        }
    }
}
