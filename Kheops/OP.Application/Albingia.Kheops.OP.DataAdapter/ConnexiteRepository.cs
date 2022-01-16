using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class ConnexiteRepository : IConnexiteRepository {
        private readonly YpoConxRepository ypoConxRepository;
        public ConnexiteRepository(YpoConxRepository ypoConxRepository) {
            this.ypoConxRepository = ypoConxRepository;
        }
        public Connexite GetByAffaire(ConnexiteId connexiteId) {
            var poconx = this.ypoConxRepository
                .GetByAffaire(connexiteId.AffaireId.TypeAffaire.AsCode(), connexiteId.AffaireId.CodeAffaire, connexiteId.AffaireId.NumeroAliment)
                .FirstOrDefault(x => x.Pjcnx == connexiteId.Numero || int.Parse(x.Pjccx) == connexiteId.Type);

            if (poconx is null) {
                return null;
            }

            return new Connexite {
                Id = new ConnexiteId {
                    AffaireId = connexiteId.AffaireId,
                    Numero = poconx.Pjcnx,
                    Type = int.Parse(poconx.Pjccx)
                },
                CodeObservation = poconx.Pjobs
            };
        }
    }
}
