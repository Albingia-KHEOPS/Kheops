using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven {
    public interface IPrimeRepository {
        PagingList<(Affaire affaire, Prime prime)> GetImpayes(string[] branches, int page = 0, int codeAssure = 0);
        PagingList<(Affaire affaire, Prime prime)> GetRetardsPaiement(string[] branches, int page = 0, int codeAssure = 0);
        IEnumerable<PrimeGarantie> GetPrimesGaranties(AffaireId affaireId, bool isReadonly = false);
    }
}
