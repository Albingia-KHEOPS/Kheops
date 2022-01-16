using System;
using System.Collections.Generic;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Document;
using Albingia.Kheops.OP.Domain.Referentiel;

namespace Albingia.Kheops.OP.Application.Port.Driver
{
    public interface IDocumentPort
    {
        void GenerateDocuments(
            AffaireId affaire,
            //string codeOffre, 
            //int version, 
            //string type, 
            //int codeAvn, 
            string service, 
            string acteGes, 
            string user, 
            DateTime now, 
            bool init, 
            long[] docsId, 
            int attesId, 
            int regulId);

        IEnumerable<LotDocument> GetLots(AffaireId affaireId, bool work);
    }
}