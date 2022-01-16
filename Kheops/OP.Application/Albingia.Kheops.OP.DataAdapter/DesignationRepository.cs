using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.DataAdapter {
    public class DesignationRepository : IDesignationRepository
    {

        private readonly KpDesiRepository kpDesiRepository;
        private readonly HpdesiRepository hpDesiRepository;

        public DesignationRepository(

            KpDesiRepository kpDesiRepository,
            HpdesiRepository hpDesiRepository
            )
        {

            this.kpDesiRepository = kpDesiRepository;
            this.hpDesiRepository = hpDesiRepository;

        }
        public string GetDesignation(long x)
        {
            if (x == 0) {
                return null;
            }
            return this.kpDesiRepository.Get((int)x)?.Kaddesi;
        }

        public string GetDesignation(long x, int avenant)
        {
            if (x == 0) {
                return null;
            }
            return this.hpDesiRepository.Get((int)x, avenant)?.Kaddesi;
        }

        public IDictionary<int, string> GetDesignationsByAffaire(AffaireId id)
        {
            if (id.IsHisto) {
                return this.hpDesiRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).ToDictionary(x => x.Kadchr, x => x.Kaddesi);
            }
            return this.kpDesiRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment).ToDictionary(x => x.Kadchr, x => x.Kaddesi);

        }

        public int UpdateDesi(string value, AffaireId affId, long id, string type)
        {
            if (affId.IsHisto) {
                throw new InvalidOperationException("On ne peut pas mettre à jour l'historique");
            }
            var desi = default(KpDesi);
            if (id > 0) {
                desi = kpDesiRepository.Get((int)id);
            }
            if (desi == null) {
                desi = new KpDesi() {
                    Kaddesi = String.Empty,
                    Kadipb = affId.CodeAffaire,
                    Kadtyp = affId.TypeAffaire.AsCode(),
                    Kadalx = affId.NumeroAliment
                };
                desi.Kadperi = type;
            }
            if (desi.Kaddesi != value) {
                desi.Kaddesi = value ?? string.Empty;
                if (desi.Kadchr == default(int)) {
                    desi.Kadchr = kpDesiRepository.NewId();
                    kpDesiRepository.Insert(desi);
                } else {
                    kpDesiRepository.Update(desi);
                }
            }
            return desi.Kadchr;
        }
        public void DeleteForInventaire(long inventaireId)
        {
            this.kpDesiRepository.DeleteForInventaire(inventaireId);
        }
        public void Delete(long id)
        {
            this.kpDesiRepository.Delete(new KpDesi { Kadchr = (int)id });
        }

        public void Reprise(AffaireId id) {
            var list = this.kpDesiRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            list.ForEach(x => this.kpDesiRepository.Delete(x));
            var histo = this.hpDesiRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histo.ForEach(x => this.hpDesiRepository.Delete(x));
            histo.ForEach(x => this.kpDesiRepository.Insert(x));
        }
    }
}
