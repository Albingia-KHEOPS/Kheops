
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;

namespace Albingia.Kheops.OP.DataAdapter {
    public partial class RisqueRepository {
        readonly YprtRsqRepository yprtRsqRepository;
        readonly YhrtrsqRepository yhrtrsqRepository;
        readonly KpRsqRepository kpRsqRepository;
        readonly HprsqRepository hprsqRepository;
        readonly YprtObjRepository yprtObjRepository;
        readonly YhrtobjRepository yhrtObjRepository;
        readonly KpObjRepository kpObjRepository;
        readonly HpobjRepository hpobjRepository;
        readonly IReferentialRepository refRepo;
        readonly IAffaireRepository affaireRepository;
        readonly YprtObtRepository yprtObtRepository;
        readonly YhrtobtRepository yhrtobtRepository;
        readonly KJobSortiRepository kJobSortiRepository;
        readonly HjobsortiRepository hjobsortiRepository;

        public RisqueRepository(
            YprtRsqRepository yprtRsqRepository,
            YhrtrsqRepository yhrtrsqRepository,
            KpRsqRepository kpRsqRepository,
            HprsqRepository hprsqRepository,
            YprtObjRepository yprtObjRepository,
            YhrtobjRepository yhrtObjRepository,
            KpObjRepository kpObjRepository,
            HpobjRepository hpobjRepository,
            YprtObtRepository yprtObtRepository,
            YhrtobtRepository yhrtobtRepository,
            KJobSortiRepository kJobSortiRepository,
            HjobsortiRepository hjobsortiRepository,
            IReferentialRepository referentialRepository,
            IAffaireRepository affaireRepository) {

            this.yprtRsqRepository = yprtRsqRepository;
            this.yhrtrsqRepository = yhrtrsqRepository;
            this.kpRsqRepository = kpRsqRepository;
            this.hprsqRepository = hprsqRepository;
            this.yprtObjRepository = yprtObjRepository;
            this.yhrtObjRepository = yhrtObjRepository;
            this.kpObjRepository = kpObjRepository;
            this.hpobjRepository = hpobjRepository;
            this.refRepo = referentialRepository;
            this.yprtObtRepository = yprtObtRepository;
            this.yhrtobtRepository = yhrtobtRepository;
            this.kJobSortiRepository = kJobSortiRepository;
            this.hjobsortiRepository = hjobsortiRepository;
            this.affaireRepository = affaireRepository;
        }
    }
}
