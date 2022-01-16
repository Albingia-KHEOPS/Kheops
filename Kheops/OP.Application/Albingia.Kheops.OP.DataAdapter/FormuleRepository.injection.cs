using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Inventaire;
using Albingia.Kheops.OP.Domain.Extension;

namespace Albingia.Kheops.OP.DataAdapter
{
    public partial class FormuleRepository : IFormuleRepository
    {
        #region Injected Private 
        private readonly IAffaireRepository affaireRepository;
        private readonly IReferentialRepository refRepo;
        private readonly IParamRepository paramRepository;
        private readonly IInventaireRepository inventaireRepository;
        private readonly IRisqueRepository risqueRepository;
        private readonly IDesignationRepository desiRepository;

        private readonly KpCtrlERepository kpctrleRepository;
        private readonly KpIrSGaRepository kpIrSGaRepository;
        private readonly KpGaranRepository kpGaranRepository;
        private readonly KpForRepository kpforRepository;
        private readonly KpGarTarRepository kpGarTarRepository;
        private readonly KpOptDRepository kpOptDRepository;
        private readonly KpOptRepository kpOptRepository;
        private readonly HpgaranRepository hpGaranRepository;
        private readonly HpforRepository hpforRepository;
        private readonly HpgartarRepository hpGarTarRepository;
        private readonly HpoptdRepository hpOptDRepository;
        private readonly HpoptRepository hpOptRepository;
        private readonly KpExpLCIDRepository kpExpLciDRepo;
        private readonly KpExpLCIRepository kpExpLciRepo;
        private readonly KpExpFrhDRepository kpExpFrhDRepo;
        private readonly KpExpFrhRepository kpExpFrhRepo;
        private readonly HpexplcidRepository hpExpLciDRepo;
        private readonly HpexplciRepository hpExpLciRepo;
        private readonly HpexpfrhdRepository hpExpFrhDRepo;
        private readonly HpexpfrhRepository hpExpFrhRepo;
        private readonly KpGarApRepository kpGarApRepository;
        private readonly HpgarapRepository hpGarApRepository;
        private readonly KpInvenRepository kpInvenRepository;
        private readonly KpInvAppRepository kpInvappRepository;
        private readonly KpInveDRepository kpInvedRepository;
        private readonly HpinvenRepository hpInvenRepository;
        private readonly HpinvappRepository hpInvappRepository;
        private readonly HpinvedRepository hpInvedRepository;
        private readonly KpOptApRepository kpOptApRepository;
        private readonly HpoptapRepository hpOptApRepository;
        private readonly YprtForRepository yprtForRepository;
        private readonly YprtGarRepository yprtGarRepository;
        private readonly YhrtforRepository yhrtforRepository;
        private readonly YhrtgarRepository yhrtgarRepository;
        private readonly YprtFooRepository yprtFooRepository;
        private readonly YhrtfooRepository yhrtfooRepository;
        private readonly KpMatFfRepository kpMatffRepository;
        private readonly KpMatFlRepository kpMatflRepository;
        private readonly KpMatFrRepository kpMatfrRepository;
        private readonly KpMatGgRepository kpMatggRepository;
        private readonly KpMatGlRepository kpMatglRepository;
        private readonly KpMatGrRepository kpMatgrRepository;
        private readonly HpmatffRepository hpmatffRepository;
        private readonly HpmatflRepository hpmatflRepository;
        private readonly HpmatfrRepository hpmatfrRepository;
        private readonly HpmatggRepository hpmatggRepository;
        private readonly HpmatglRepository hpmatglRepository;
        private readonly HpmatgrRepository hpmatgrRepository;
        private readonly KJobSortiRepository kJobSortiRepository;
        private readonly HjobsortiRepository hjobsortiRepository;

        #endregion

        public FormuleRepository(
        #region injected values
            IReferentialRepository refRepo,
            IAffaireRepository affaireRepository,
            IParamRepository paramRepository,
            IInventaireRepository inventaireRepository,
            IDesignationRepository desiRepository,
            IRisqueRepository risqueRepository,

            KpCtrlERepository kpctrleRepository,
            KpIrSGaRepository kpIrSGaRepository,

            KpGaranRepository KpGaranRepository,
            KpForRepository kpforRepository,
            KpGarTarRepository KpGarTarRepository,
            KpOptDRepository kpOptDRepository,
            KpOptRepository kpOptRepository,
            KpExpLCIDRepository kpExpLciDRepo,
            KpExpLCIRepository kpExpLciRepo,
            KpExpFrhDRepository kpExpFrhDRepo,
            KpExpFrhRepository kpExpFrhRepo,
            KpGarApRepository kpGarApRepo,
            KpInvenRepository kpInvenRepo,
            KpInvAppRepository kpInvappRepo,
            KpInveDRepository kpInvedRepo,

            HpgaranRepository hpGaranRepository,
            HpforRepository hpforRepository,
            HpgartarRepository hpGarTarRepository,
            HpoptdRepository hpOptDRepository,
            HpoptRepository hpOptRepository,
            HpexplcidRepository hpExpLciDRepo,
            HpexplciRepository hpExpLciRepo,
            HpexpfrhdRepository hpExpFrhDRepo,
            HpexpfrhRepository hpExpFrhRepo,
            HpgarapRepository hpGarApRepo,
            HpinvenRepository hpInvenRepo,
            HpinvappRepository hpInvappRepo,
            HpinvedRepository hpInvedRepo,
            KpOptApRepository kpOptApRepository,
            HpoptapRepository hpOptApRepository,
            YprtForRepository yprtForRepository,
            YhrtforRepository yhrtforRepository,
            YhrtfooRepository yhrtfooRepository,
            YprtGarRepository yprtGarRepository,
            YprtFooRepository yprtFooRepository,
            KpMatFfRepository kpMatffRepository,
            KpMatFlRepository kpMatflRepository,
            KpMatFrRepository kpMatfrRepository,
            KpMatGgRepository kpMatggRepository,
            KpMatGlRepository kpMatglRepository,
            KpMatGrRepository kpMatgrRepository,
            HpmatffRepository hpmatffRepository,
            HpmatflRepository hpmatflRepository,
            HpmatfrRepository hpmatfrRepository,
            HpmatggRepository hpmatggRepository,
            HpmatglRepository hpmatglRepository,
            HpmatgrRepository hpmatgrRepository,
            YhrtgarRepository yhrtgarRepository,
            KJobSortiRepository kJobSortiRepository,
            HjobsortiRepository hjobsortiRepository
        #endregion
            ) {
            this.refRepo = refRepo;
            this.affaireRepository = affaireRepository;
            this.paramRepository = paramRepository;
            this.inventaireRepository = inventaireRepository;
            this.desiRepository = desiRepository;
            this.risqueRepository = risqueRepository;

            this.kpIrSGaRepository = kpIrSGaRepository;
            this.kpGaranRepository = KpGaranRepository;
            this.kpforRepository = kpforRepository;
            this.kpGarTarRepository = KpGarTarRepository;
            this.kpOptDRepository = kpOptDRepository;
            this.kpOptRepository = kpOptRepository;
            this.kpGarApRepository = kpGarApRepo;
            this.kpExpFrhDRepo = kpExpFrhDRepo;
            this.kpExpFrhRepo = kpExpFrhRepo;
            this.kpExpLciDRepo = kpExpLciDRepo;
            this.kpExpLciRepo = kpExpLciRepo;
            this.kpInvenRepository = kpInvenRepo;
            this.kpInvappRepository = kpInvappRepo;
            this.kpInvedRepository = kpInvedRepo;
            this.kpctrleRepository = kpctrleRepository;

            this.hpforRepository = hpforRepository;
            this.hpOptRepository = hpOptRepository;
            this.hpOptDRepository = hpOptDRepository;
            this.hpGaranRepository = hpGaranRepository;
            this.hpGarTarRepository = hpGarTarRepository;
            this.hpExpLciDRepo = hpExpLciDRepo;
            this.hpExpLciRepo = hpExpLciRepo;
            this.hpExpFrhDRepo = hpExpFrhDRepo;
            this.hpExpFrhRepo = hpExpFrhRepo;
            this.hpGarApRepository = hpGarApRepo;

            this.hpInvenRepository = hpInvenRepo;
            this.hpInvappRepository = hpInvappRepo;
            this.hpInvedRepository = hpInvedRepo;


            this.kpOptApRepository = kpOptApRepository;
            this.hpOptApRepository = hpOptApRepository;

            this.yprtForRepository = yprtForRepository;
            this.yprtGarRepository = yprtGarRepository;
            this.yprtFooRepository = yprtFooRepository;
            this.kpMatffRepository = kpMatffRepository;
            this.kpMatflRepository = kpMatflRepository;
            this.kpMatfrRepository = kpMatfrRepository;
            this.kpMatggRepository = kpMatggRepository;
            this.kpMatglRepository = kpMatglRepository;
            this.kpMatgrRepository = kpMatgrRepository;
            this.yhrtfooRepository = yhrtfooRepository;
            this.yhrtforRepository = yhrtforRepository;
            this.hpmatffRepository = hpmatffRepository;
            this.hpmatflRepository = hpmatflRepository;
            this.hpmatfrRepository = hpmatfrRepository;
            this.hpmatggRepository = hpmatggRepository;
            this.hpmatglRepository = hpmatglRepository;
            this.hpmatgrRepository = hpmatgrRepository;
            this.yhrtgarRepository = yhrtgarRepository;
            this.kJobSortiRepository = kJobSortiRepository;
            this.hjobsortiRepository = hjobsortiRepository;
        }
    }
}
