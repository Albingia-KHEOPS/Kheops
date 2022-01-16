using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public class ObservationRepository : IObservationRepository {
        private readonly KpObsvRepository kpObsvRepository;
        private readonly HpobsvRepository hpobsvRepository;

        public ObservationRepository(
            KpObsvRepository kpObsvRepository,
            HpobsvRepository hpobsvRepository) {
            this.kpObsvRepository = kpObsvRepository;
            this.hpobsvRepository = hpobsvRepository;
        }

        public ALBINGIA.Framework.Business.Observation GetObservation(int id, int? avenant = null) {
            var o = this.kpObsvRepository.Get(id);
            if (o is null && avenant.HasValue && avenant.Value >= 0) {
                o = this.hpobsvRepository.Get(id, avenant.Value, 1);
            }
            else {
                return null;
            }
            return new ALBINGIA.Framework.Business.Observation {
                Folder = new ALBINGIA.Framework.Common.Folder(o.Kajipb, o.Kajalx, o.Kajtyp[0], o.Kajavn.GetValueOrDefault()),
                Id = id,
                Texte = o.Kajobsv
            };
        }

        public void AddOrUpdate(ALBINGIA.Framework.Business.Observation observation) {
            var o = this.kpObsvRepository.Get(observation.Id ?? 0);
            if (o is null) {
                this.kpObsvRepository.Insert(new DataModel.DTO.KpObsv {
                    Kajipb = observation.Folder.CodeOffre.ToIPB(),
                    Kajalx = observation.Folder.Version,
                    Kajobsv = observation.Texte,
                    Kajtyp = observation.Folder.Type,
                    Kajtypobs = observation.Type
                });
            }
            else {
                o.Kajobsv = observation.Texte;
                this.kpObsvRepository.Update(o);
            }
        }

        public void Reprise(AffaireId id) {
            var obList = this.kpObsvRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            obList.ForEach(x => this.kpObsvRepository.Delete(x));
            var hobList = this.hpobsvRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hobList.ForEach(x => this.hpobsvRepository.Delete(x));
            hobList.ForEach(x => this.kpObsvRepository.Insert(x));
        }
    }
}
