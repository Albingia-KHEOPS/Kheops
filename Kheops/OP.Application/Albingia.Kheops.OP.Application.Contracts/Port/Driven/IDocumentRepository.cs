using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Document;
using Albingia.Kheops.OP.Domain.Referentiel;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Application.Port.Driven
{
    public interface IDocumentRepository
    {
        IEnumerable<LotDocument> GetByAffaireId(AffaireId id, bool work);
        IEnumerable<DocumentGenere> GetDocGenByAffaireId(AffaireId id, bool work);
        IEnumerable<DocumentExterne> GetDocExtByAffaireId(AffaireId id);
        void SaveLot(LotDocument lot, string userName);
        void DeleteLot(LotDocument lot);

        IEnumerable<Chemin> GetChemins();
        Chemin GetChemin(string cle);
        void AddCopyDoc(IEnumerable<CopyDoc> docsToCopy);
        IEnumerable<CopyDoc> GetCopyDoc(AffaireId id);
        void ClearCopyDoc(AffaireId id);
    }
}