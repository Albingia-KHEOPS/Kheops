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
    public class AdresseRepository : IAdresseRepository {

        private readonly YAdressRepository yAdressRepository;
        private readonly YprtAdrRepository yprtAdrRepository;
        private readonly YhrtadrRepository yhrtadrRepository;
        
        public AdresseRepository(YAdressRepository yAdressRepository, YprtAdrRepository yprtAdrRepository, YhrtadrRepository yhrtadrRepository) {
            this.yAdressRepository = yAdressRepository;
            this.yprtAdrRepository = yprtAdrRepository;
            this.yhrtadrRepository = yhrtadrRepository;
        }

        public void Reprise(AffaireId id, int? idAdr = null) {
            var list = this.yprtAdrRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            list.ForEach(x => this.yprtAdrRepository.Delete(x));
            var histo = this.yhrtadrRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histo.ForEach(x => this.yhrtadrRepository.Delete(x));
            histo.ForEach(x => this.yprtAdrRepository.Insert(x));
            var oldIds = list.Where(x => x.Jfadh > 0).Select(x => x.Jfadh).ToList();
            if (idAdr.GetValueOrDefault() > 0) {
                oldIds.Add(idAdr.Value);
            }
            if (oldIds.Any()) {
                this.yAdressRepository.DeleteMultiple(new List<int>(list.Select(x => x.Jfadh)) { idAdr.Value });
            }
        }
    }
}
