using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Risque;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;

namespace Albingia.Kheops.OP.DataAdapter {
    public partial class RisqueRepository : IRisqueRepository
    {
        internal static Objet BuildSimpleObjet(ObjetId identifier, KpObj kpobj, YprtObj yobj, IReferentialRepository referential)
        {
            return new Objet {
                Id = identifier,
                Designation = kpobj == null ? 0 : kpobj.Kacdesi,
                Description = kpobj == null ? string.Empty : kpobj.Kacdesc,
                Type = yobj.Jgvat,
                Unite = yobj.Jgvau,
                Valeur = yobj.Jgval,
                DateDebut = MakeNullableDateTime(yobj.Jgvda, yobj.Jgvdm, yobj.Jgvdj, yobj.Jgvdh),
                DateFin = MakeNullableDateTime(yobj.Jgvfa, yobj.Jgvfm, yobj.Jgvfj, yobj.Jgvfh),
                NumeroAvenantCreation = yobj.Jgave,
                NumeroAvenantModification = yobj.Jgavf,
                DateEffetAvenant = MakeNullableDateTime(yobj.Jgava, yobj.Jgavm, yobj.Jgavj),
                Cible = kpobj.Kaccible.IsEmptyOrNull() ? null : referential.GetCible(kpobj.Kaccible),
                Nomenclature01 = kpobj.Kacnmc01,
                TarifLCI = new Domain.TarifGeneral {
                    Base = new BaseDeCalcul { Code = kpobj.Kaclcibase },
                    IdExpCpx = kpobj.Kackdiid,
                    Unite = new Unite { Code = kpobj.Kaclciunit },
                    ValeurActualisee = kpobj.Kaclcivala,
                    ValeurOrigine = kpobj.Kaclcivalo
                },
                TarifFranchise = new Domain.TarifGeneral {
                    Base = new BaseDeCalcul { Code = kpobj.Kacfrhbase },
                    IdExpCpx = kpobj.Kackdkid,
                    Unite = new Unite { Code = kpobj.Kacfrhunit },
                    ValeurActualisee = kpobj.Kacfrhvala,
                    ValeurOrigine = kpobj.Kacfrhvalo
                }
            };
        }

        internal static Risque BuildRisque(AffaireId affaire, YprtRsq yrsq, KpRsq kprsq, IEnumerable<Objet> objets, IReferentialRepository referential) {
            return new Risque {
                AffaireId = affaire,
                Designation = kprsq is null ? string.Empty : kprsq.Kabdesc,
                Numero = yrsq.Jersq,
                AllowCANAT = yrsq.Jecna.AsBool(),
                Cible = kprsq is null ? null : (referential?.GetCible(kprsq.Kabcible) ?? new Cible { Code = kprsq.Kabcible }),
                Branche = referential?.GetValue<Branche>(yrsq.Jebra) ?? new Branche { Code = yrsq.Jebra },
                SousBranche = yrsq.Jesbr,
                NumeroAvenantCreation = yrsq.Jeave,
                NumeroAvenantModification = yrsq.Jeavf,
                DateEffetAvenant = MakeNullableDateTime(yrsq.Jeava, yrsq.Jeavm, yrsq.Jeavj),
                Objets = new List<Objet>(objets == null ? new Objet[0] : objets),
                DateDebut = MakeNullableDateTime(yrsq.Jevda, yrsq.Jevdm, yrsq.Jevdj, yrsq.Jevdh),
                DateFin = MakeNullableDateTime(yrsq.Jevfa, yrsq.Jevfm, yrsq.Jevfj, yrsq.Jevfh),
                IsTemporaire = yrsq.Jetem == Booleen.Oui.AsCode(),
                ARegulariser = yrsq.Jerul == Booleen.Oui.AsCode(),
                ParticipationBeneficiaire = yrsq.Jepbn == Booleen.Oui.AsCode(),
                BonnificationNonSinistre = yrsq.Jepbn == "B",
                BonnificationNonSinistreIncendie = yrsq.Jepbn == "U",
                Nomenclature01 = kprsq.Kabnmc01,
                TarifLCI = new Domain.TarifGeneral {
                    Base = new BaseDeCalcul { Code = kprsq.Kablcibase },
                    IdExpCpx = kprsq.Kabkdiid,
                    Unite = new Unite { Code = kprsq.Kablciunit },
                    ValeurActualisee = kprsq.Kablcivala,
                    ValeurOrigine = kprsq.Kablcivalo
                },
                TarifFranchise = new Domain.TarifGeneral {
                    Base = new BaseDeCalcul { Code = kprsq.Kabfrhbase },
                    IdExpCpx = kprsq.Kabkdkid,
                    Unite = new Unite { Code = kprsq.Kabfrhunit },
                    ValeurActualisee = kprsq.Kabfrhvala,
                    ValeurOrigine = kprsq.Kabfrhvalo
                },
                RegimeTaxe = referential?.GetValue<RegimeTaxe>(yrsq.Jergt) ?? new RegimeTaxe { Code = yrsq.Jergt }
            };
        }

        public IEnumerable<Risque> GetRisquesByAffaire(AffaireId affaire) {
            IEnumerable<YprtRsq> yList = null;
            IEnumerable<KpRsq> kList = null;
            if (affaire.IsHisto) {
                yList = yhrtrsqRepository.GetByAffaire(affaire.CodeAffaire, affaire.NumeroAliment, affaire.NumeroAvenant.GetValueOrDefault(), 1);
                kList = hprsqRepository.GetAllByIpbAlx(affaire.CodeAffaire, affaire.NumeroAliment).Where(x => x.Kabavn == affaire.NumeroAvenant.GetValueOrDefault());
            }
            else {
                yList = yprtRsqRepository.Liste(affaire.CodeAffaire, affaire.NumeroAliment);
                kList = kpRsqRepository.Liste(affaire.TypeAffaire.AsCode(), affaire.CodeAffaire, affaire.NumeroAliment);

                // ensure num avn
                var currentId = this.affaireRepository.GetCurrentId(affaire.CodeAffaire, affaire.NumeroAliment);
                currentId.Adapt(affaire);
            }
            IEnumerable<Objet> objets = GetMinimalObjetsByAffaire(affaire);

            return yList.Select(yrsq => {
                var kprsq = kList.FirstOrDefault(x => x.Kabipb == yrsq.Jeipb && x.Kabalx == yrsq.Jealx && x.Kabrsq == yrsq.Jersq);
                return BuildRisque(affaire, yrsq, kprsq, objets.Where(o => o.Id.NumRisque == yrsq.Jersq), refRepo);
            }).ToList();
        }

        /// <summary>
        /// Retrieves all risques-objets from the beginning AFFNOUV till now
        /// </summary>
        /// <param name="codeOffre">IPB code</param>
        /// <param name="version">ALX code</param>
        /// <returns></returns>
        public IEnumerable<Risque> GetAllRisquesByAffaire(string codeOffre, int version) {
            var risques = GetRisquesByAffaire(new AffaireId { CodeAffaire = codeOffre, NumeroAliment = version, TypeAffaire = AffaireType.Contrat }).ToList();
            var hpRsqList = this.hprsqRepository.GetAllByIpbAlx(codeOffre, version);
            var hpObjList = this.hpobjRepository.GetAllByIpbAlx(codeOffre, version);
            var yhObjList = this.yhrtObjRepository.GetAllByIpbAlx(codeOffre, version);
            var test = this.yhrtrsqRepository.GetAllByIpbAlx(codeOffre, version);
            var oldRisques = this.yhrtrsqRepository.GetAllByIpbAlx(codeOffre, version)
                .Select(r => {
                    var id = new AffaireId { CodeAffaire = codeOffre, TypeAffaire = AffaireType.Contrat, NumeroAliment = version, IsHisto = true, NumeroAvenant = r.Jeavn };
                    return BuildRisque(
                        id,
                        r,
                        hpRsqList.First(x => x.Kabrsq == r.Jersq && x.Kabavn == r.Jeavn),
                        hpObjList.Where(x => x.Kacrsq == r.Jersq && x.Kacavn == r.Jeavn).Select(x => BuildSimpleObjet(new ObjetId { AffaireId = id, NumObjet = x.Kacobj, NumRisque = x.Kacrsq }, x, yhObjList.First(y => y.Jgrsq == r.Jersq && y.Jgavn == r.Jeavn && x.Kacobj == y.Jgobj), this.refRepo)),
                        this.refRepo);
                });

            risques.AddRange(oldRisques);
            return risques;
        }

        public Objet GetMinimalObjetById(ObjetId identifier) {
            string codeAffaire = identifier.AffaireId.CodeAffaire.PadLeft(9, ' ');
            var kpobj = kpObjRepository.Get(
                identifier.AffaireId.TypeAffaire.AsCode(),
                codeAffaire,
                identifier.AffaireId.NumeroAliment,
                identifier.NumRisque,
                identifier.NumObjet);

            var yobj = yprtObjRepository.Get(
                codeAffaire,
                identifier.AffaireId.NumeroAliment,
                identifier.NumRisque,
                identifier.NumObjet);

            return BuildSimpleObjet(identifier, kpobj, yobj, this.refRepo);
        }

        public IEnumerable<Objet> GetMinimalObjetsByAffaire(AffaireId identifier) {
            var kpobjList = kpObjRepository.GetByAffaire(
                identifier.TypeAffaire.AsCode(),
                identifier.CodeAffaire,
                identifier.NumeroAliment);

            var yobjList = yprtObjRepository.GetByAffaire(identifier.CodeAffaire, identifier.NumeroAliment);

            return yobjList.Select(y => {
                var kp = kpobjList.FirstOrDefault(x => x.Kacipb == y.Jgipb && x.Kacalx == y.Jgalx && x.Kacrsq == y.Jgrsq && x.Kacobj == y.Jgobj);
                return BuildSimpleObjet(
                    new ObjetId { AffaireId = identifier, NumObjet = y.Jgobj, NumRisque = y.Jgrsq },
                    kp,
                    y,
                    this.refRepo);
            }).ToList();
        }

        public void Reprise(AffaireId id) {
            var kobjList = this.kpObjRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            kobjList.ForEach(x => this.kpObjRepository.Delete(x));
            var khobjList = this.hpobjRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            khobjList.ForEach(x => this.hpobjRepository.Delete(x));
            khobjList.ForEach(x => this.kpObjRepository.Insert(x));
            var yobjList = this.yprtObjRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            yobjList.ForEach(x => this.yprtObjRepository.Delete(x));
            var hobjList = this.yhrtObjRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hobjList.ForEach(x => this.yhrtObjRepository.Delete(x));
            hobjList.ForEach(x => this.yprtObjRepository.Insert(x));
            var krsqList = this.kpRsqRepository.Liste(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            krsqList.ForEach(x => this.kpRsqRepository.Delete(x));
            var khrsqList = this.hprsqRepository.Liste(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            khrsqList.ForEach(x => this.hprsqRepository.Delete(x));
            khrsqList.ForEach(x => this.kpRsqRepository.Insert(x));
            var yrsqList = this.yprtRsqRepository.Liste(id.CodeAffaire, id.NumeroAliment).ToList();
            yrsqList.ForEach(x => this.yprtRsqRepository.Delete(x));
            var hrsqList = this.yhrtrsqRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1, 1).ToList();
            hrsqList.ForEach(x => this.yhrtrsqRepository.Delete(x));
            hrsqList.ForEach(x => this.yprtRsqRepository.Insert(x));
            var yobtList = this.yprtObtRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            yobtList.ForEach(x => this.yprtObtRepository.Delete(x));
            var hobtList = this.yhrtobtRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hobtList.ForEach(x => this.yhrtobtRepository.Delete(x));
            hobtList.ForEach(x => this.yprtObtRepository.Insert(x));
            var rsqObjSortis = this.kJobSortiRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, AlbConstantesMetiers.TYPE_CONTRAT).ToList();
            foreach (var group in rsqObjSortis.Where(x => x.Avn == id.NumeroAvenant && x.Rsq > 1).GroupBy(x => x.Rsq)) {
                this.kJobSortiRepository.DeleteByRisque(id.CodeAffaire, id.NumeroAliment, AlbConstantesMetiers.TYPE_CONTRAT, id.NumeroAvenant.Value, group.Key);
            }
            var hrsqObjSortis = this.hjobsortiRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            foreach (var group in hrsqObjSortis.Where(x => x.Rsq > 1).GroupBy(x => x.Rsq)) {
                this.hjobsortiRepository.DeleteByRisque(id.CodeAffaire, id.NumeroAliment, group.First().Avn, group.Key);
            }
            this.kJobSortiRepository.InsertMultiple(hrsqObjSortis.Where(x => x.Rsq > 1));
        }

        public void SaveConditions(Risque risque, string user) {
            var rsq = this.kpRsqRepository.Get(risque.AffaireId.TypeAffaire.AsCode(), risque.AffaireId.CodeAffaire, risque.AffaireId.NumeroAliment, risque.Numero);
            var objs = this.kpObjRepository.GetByAffaire(risque.AffaireId.TypeAffaire.AsCode(), risque.AffaireId.CodeAffaire, risque.AffaireId.NumeroAliment).Where(o => o.Kacrsq == risque.Numero).ToList();
            bool isMonoObj = objs.Count == 1;
            objs.ForEach(o => {
                o.Kaclcivalo = isMonoObj ? (risque.TarifLCI?.ValeurOrigine ?? default) : default;
                o.Kaclcivala = isMonoObj ? (risque.TarifLCI?.ValeurActualisee ?? default) : default;
                o.Kaclciunit = isMonoObj ? (risque.TarifLCI?.Unite) : string.Empty;
                o.Kaclcibase = isMonoObj ? (risque.TarifLCI?.Base?.Code ?? string.Empty) : string.Empty;
                o.Kackdiid = isMonoObj ? (risque.TarifLCI?.IdExpCpx ?? default) : default;
                o.Kacfrhvalo = isMonoObj ? (risque.TarifFranchise?.ValeurOrigine ?? default) : default;
                o.Kacfrhvala = isMonoObj ? (risque.TarifFranchise?.ValeurActualisee ?? default) : default;
                o.Kacfrhunit = isMonoObj ? (risque.TarifFranchise?.Unite) : string.Empty;
                o.Kacfrhbase = isMonoObj ? (risque.TarifFranchise?.Base?.Code ?? string.Empty) : string.Empty;
                o.Kackdkid = isMonoObj ? (risque.TarifFranchise?.IdExpCpx ?? default) : default;
                this.kpObjRepository.Update(o);
            });
            rsq.Kablcivalo = risque.TarifLCI?.ValeurOrigine ?? default;
            rsq.Kablcivala = risque.TarifLCI?.ValeurActualisee ?? default;
            rsq.Kablciunit = risque.TarifLCI?.Unite;
            rsq.Kablcibase = risque.TarifLCI?.Base?.Code ?? string.Empty;
            rsq.Kabkdiid = risque.TarifLCI?.IdExpCpx ?? default;
            rsq.Kabfrhvalo = risque.TarifFranchise?.ValeurOrigine ?? default;
            rsq.Kabfrhvala = risque.TarifFranchise?.ValeurActualisee ?? default;
            rsq.Kabfrhunit = risque.TarifFranchise?.Unite;
            rsq.Kabfrhbase = risque.TarifFranchise?.Base?.Code ?? string.Empty;
            rsq.Kabkdkid = risque.TarifFranchise?.IdExpCpx ?? default;
            this.kpRsqRepository.Update(rsq);
        }

        public void ToggleCanatFlag(Risque risque) {
            var rsq = this.yprtRsqRepository.Get(risque.AffaireId.CodeAffaire, risque.AffaireId.NumeroAliment, risque.Numero);
            rsq.Jecna = risque.AllowCANAT.ToYesNo();
            this.yprtRsqRepository.Update(rsq);
        }
    }
}
