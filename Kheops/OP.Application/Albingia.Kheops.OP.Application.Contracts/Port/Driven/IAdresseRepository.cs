﻿using Albingia.Kheops.OP.Domain.Affaire;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IAdresseRepository {
        void Reprise(AffaireId id, int? idAdr = null);
    }
}
