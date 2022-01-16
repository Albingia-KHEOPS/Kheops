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
using ALBINGIA.Framework.Common.Extensions;

namespace Albingia.Kheops.OP.DataAdapter
{
    public partial class FormuleRepository
    {
        private void CleanUp(Formule formule, Option opt, bool force = true)
        {
            opt.OptionVolets.ForEach(x => CleanUp(x, force));
            opt.Applications.Where(x => x.Id > 0).ToList().ForEach(a => this.kpOptApRepository.Delete(new KpOptAp() { Kddid = a.Id }));
            this.kpOptRepository.Delete(new KpOpt() { Kdbid = opt.Id });
            this.kpctrleRepository.DeleteOptionCtrl(new KpCtrlE {
                Kevipb = formule.AffaireId.CodeAffaire.ToIPB(),
                Kevalx = formule.AffaireId.NumeroAliment,
                Kevtyp = formule.AffaireId.TypeAffaire.AsCode(),
                Kevopt = opt.OptionNumber,
                Kevfor = formule.FormuleNumber
            });
        }

        private void CleanUp(OptionVolet opt, bool force = true)
        {
            opt.Blocs.ForEach(x => CleanUp(x, force));
            this.kpOptDRepository.Delete(new KpOptD() { Kdcid = opt.Id });
        }

        private void CleanUp(OptionBloc opt, bool force = true)
        {
            opt.Garanties.ForEach(x => CleanUp(x, force));
            this.kpOptDRepository.Delete(new KpOptD() { Kdcid = opt.Id });
        }

        private void CleanUp(Garantie opt, bool force = true)
        {
            opt.SousGaranties.ForEach(x => CleanUp(x, force));
            if (force || !(opt.IsChecked || opt.Caractere == CaractereSelection.Propose)) {
                CleanUp(opt.Inventaire);
                CleanUp(opt.Tarif);
                this.kpGarApRepository.Delete(new KpGarAp() { Kdfid = opt.LienKPGARAP });
                this.kpGaranRepository.Delete(new KpGaran() { Kdeid = opt.Id });
            }
        }

        private void CleanUp(TarifGarantie tarif)
        {
            //CleanUp(tarif.Franchise.ExpressionComplexe);
            //CleanUp(tarif.LCI.ExpressionComplexe);
            this.kpGarTarRepository.Delete(new KpGarTar() { Kdgid = tarif.Id });
        }

        private void CleanUp(ExpressionComplexeBase cpx)
        {
            if (cpx != null && cpx.DesiId != 0) {
                this.desiRepository.Delete(cpx.DesiId );
            }
            if (cpx is ExpressionComplexeFranchise) {
                this.kpExpFrhRepo.DeleteWithCascade(cpx.Id);
            } else if (cpx is ExpressionComplexeLCI) {
                this.kpExpLciRepo.DeleteWithCascade(cpx.Id);
            }
        }

        private void CleanUp(Inventaire inventaire)
        {
            if (inventaire != null) {
                this.inventaireRepository.DeleteInventaire(inventaireId: inventaire.Id);
            }
        }

    }

}
