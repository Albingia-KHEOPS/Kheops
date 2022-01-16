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
    public class TransverseRepository : ITransverseRepository {
        private readonly YpoCoasRepository ypoCoasRepository;
        private readonly YhpcoasRepository yhpcoasRepository;
        private readonly YpoAssuRepository ypoAssuRepository;
        private readonly YhpassuRepository yhpassuRepository;
        private readonly YpoAssxRepository ypoAssxRepository;
        private readonly YhpassxRepository yhpassxRepository;
        private readonly YpoEcheRepository ypoEcheRepository;
        private readonly YhpecheRepository yhpecheRepository;
        private readonly YpoInteRepository ypoInteRepository;
        private readonly YhpinteRepository yhpinteRepository;
        private readonly YpoConxRepository ypoConxRepository;
        private readonly YhpconxRepository yhpconxRepository;
        private readonly YpoCourRepository ypoCourRepository;
        private readonly YhpcourRepository yhpcourRepository;

        public TransverseRepository(
            YpoCoasRepository ypoCoasRepository,
            YhpcoasRepository yhpcoasRepository,
            YpoAssuRepository ypoAssuRepository,
            YhpassuRepository yhpassuRepository,
            YpoAssxRepository ypoAssxRepository,
            YhpassxRepository yhpassxRepository,
            YpoEcheRepository ypoEcheRepository,
            YhpecheRepository yhpecheRepository,
            YpoInteRepository ypoInteRepository,
            YhpinteRepository yhpinteRepository,
            YpoConxRepository ypoConxRepository,
            YhpconxRepository yhpconxRepository,
            YpoCourRepository ypoCourRepository,
            YhpcourRepository yhpcourRepository) {

            this.ypoCoasRepository = ypoCoasRepository;
            this.yhpcoasRepository = yhpcoasRepository;
            this.ypoAssuRepository = ypoAssuRepository;
            this.yhpassuRepository = yhpassuRepository;
            this.ypoAssxRepository = ypoAssxRepository;
            this.yhpassxRepository = yhpassxRepository;
            this.ypoEcheRepository = ypoEcheRepository;
            this.yhpecheRepository = yhpecheRepository;
            this.ypoInteRepository = ypoInteRepository;
            this.yhpinteRepository = yhpinteRepository;
            this.ypoConxRepository = ypoConxRepository;
            this.yhpconxRepository = yhpconxRepository;
            this.ypoCourRepository = ypoCourRepository;
            this.yhpcourRepository = yhpcourRepository;

        }

        public void Reprise(AffaireId id) {
            this.ypoCoasRepository.DeleteByAffaire(id.CodeAffaire, id.NumeroAliment, AlbConstantesMetiers.TYPE_CONTRAT);
            var hcoasList = this.yhpcoasRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            this.yhpcoasRepository.DeleteByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            this.ypoCoasRepository.InsertMultiple(hcoasList);
            var assuList = this.ypoAssuRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            assuList.ForEach(x => this.ypoAssuRepository.Delete(x));
            var hassuList = this.yhpassuRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            hassuList.ForEach(x => this.yhpassuRepository.Delete(x));
            hassuList.ForEach(x => this.ypoAssuRepository.Insert(x));
            this.ypoAssxRepository.DeleteByAffaire(id.CodeAffaire, id.NumeroAliment, AlbConstantesMetiers.TYPE_CONTRAT);
            var hassxList = this.yhpassxRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            this.yhpassxRepository.DeleteByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            this.ypoAssxRepository.InsertMultiple(hassxList);
            var echList = this.ypoEcheRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            echList.ForEach(x => this.ypoEcheRepository.Delete(x));
            var hechList = this.yhpecheRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            hechList.ForEach(x => this.yhpecheRepository.Delete(x));
            hechList.ForEach(x => this.ypoEcheRepository.Insert(x));
            var inteList = this.ypoInteRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            inteList.ForEach(x => this.ypoInteRepository.Delete(x));
            var hinteList = this.yhpinteRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hinteList.ForEach(x => this.yhpinteRepository.Delete(x));
            hinteList.ForEach(x => this.ypoInteRepository.Insert(x));
            var conxList = this.ypoConxRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            conxList.ForEach(x => this.ypoConxRepository.Delete(x));
            var hconxList = this.yhpconxRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            this.yhpconxRepository.DeleteByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1);
            hconxList.ForEach(x => this.ypoConxRepository.Insert(x));
            var crtList = this.ypoCourRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            crtList.ForEach(x => this.ypoCourRepository.Delete(x));
            var hcrtList = this.yhpcourRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            hcrtList.ForEach(x => this.yhpcourRepository.Delete(x));
            hcrtList.ForEach(x => this.ypoCourRepository.Insert(x));
        }
    }
}
