using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using Albingia.Kheops.OP.Domain.Risque;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter
{
    public class InfosSpecifiquesRepository : IInfosSpecifiquesRepository
    {
        private readonly IGenericCache cache;
        private readonly KISRefRepository kISRefRepository;
        private readonly KISModRepository kISModRepository;
        private readonly KISModlRepository kISModlRepository;
        private readonly KPISValRepository kpISValRepository;
        private readonly HpisvalRepository hpISValRepository;
        private readonly KpIrsObRepository kpIrsObRepository;
        private readonly HpirsobRepository hpirsobRepository;
        //private readonly RefRepository refRepository;
        private readonly IControleRepository controleRepository;
        private readonly IAffaireRepository affaireRepository;
        private readonly IRisqueRepository risqueRepository;
        private readonly IFormuleRepository formuleRepository;
        private readonly IParamRepository parameterRepository;
        private readonly IReferentialRepository referentialRepository;

        public InfosSpecifiquesRepository(IGenericCache genericCache, KISRefRepository kISRefRepository, KISModRepository kISModRepository, KISModlRepository kISModlRepository, KPISValRepository kPISValRepository, HpisvalRepository hpISValRepository, HpirsobRepository hpirsobRepository, KpIrsObRepository kpIrsObRepository/*, RefRepository refRepository*/, IControleRepository controleRepository, IAffaireRepository affaireRepository, IRisqueRepository risqueRepository, IFormuleRepository formuleRepository, IParamRepository parameterRepository, IReferentialRepository referentialRepository)
        {
            this.cache = genericCache;
            this.kISRefRepository = kISRefRepository;
            this.kISModRepository = kISModRepository;
            this.kISModlRepository = kISModlRepository;
            this.affaireRepository = affaireRepository;
            this.kpISValRepository = kPISValRepository;
            this.kpIrsObRepository = kpIrsObRepository;
            this.hpirsobRepository = hpirsobRepository;
            //this.refRepository = refRepository;
            this.controleRepository = controleRepository;
            this.risqueRepository = risqueRepository;
            this.hpISValRepository = hpISValRepository;
            this.referentialRepository = referentialRepository;
            this.formuleRepository = formuleRepository;
            this.parameterRepository = parameterRepository;
        }

        public IEnumerable<ModeleIS> GetModeles(bool resetCache = false)
        {
            if (resetCache)
            {
                this.cache.Invalidate<IEnumerable<ModeleIS>>("/ALL/");
            }
            return CacheGetModeles();
        }

        public void InitCache()
        {
            _ = GetModeles();
        }

        public void ResetCache()
        {
            _ = GetModeles(true);
        }

        public IEnumerable<LigneModeleIS> GetModeleLignes(ModeleIS modele, Affaire affaire, Risque risque = null, Objet objet = null, int numOption = 0, int numFormule = 0)
        {
            if (modele is null) throw new ArgumentException(nameof(modele));

            var affaireId = affaire.Adapt<AffaireId>();
            var filtre = new FiltreLignesIS { Affaire = affaire, Risque = risque, Objet = objet };
            if (modele.Section == TypeSection.Garanties)
            {
                var formules = this.formuleRepository.GetFormulesForAffaire(affaireId);
                var formule = formules.First(f => f.FormuleNumber == numFormule);
                formule.ApplyParameters(affaire,
                    this.parameterRepository.GetParamVolets(),
                    this.parameterRepository.GetParamNatures(),
                    true,
                    affaire.Typologie,
                    affaire.DateModeleApplicable);
                formule.SetDates(affaire, risque);
                filtre.Option = formule.Options.First(o => o.OptionNumber == numOption);
            }

            var lignes = filtre.Filtrer(modele.Proprietes).ToList();

            // link parent to each child
            var parentIds = lignes.Where(x => x.ParentId > 0).Select(x => x.Id).ToList();
            var parents = lignes.Where(x => parentIds.Contains(x.Id));
            lignes.ForEach(x => x.Parent = x.ParentId < 1 ? null : parents.FirstOrDefault(p => p.Id == x.ParentId));
            return lignes;
        }

        public IEnumerable<InformationSpecifique> GetModeleInfos(ModeleIS modele, Affaire affaire, Risque risque = null, Objet objet = null, int numOption = 0, int numFormule = 0)
        {
            if (modele is null) throw new ArgumentException(nameof(modele));

            AffaireId affaireId = affaire.Adapt<AffaireId>();
            var currentId = this.affaireRepository.GetCurrentId(affaire.CodeAffaire, affaire.NumeroAliment);
            if (!affaireId.NumeroAvenant.HasValue || affaireId.NumeroAvenant == currentId.NumeroAvenant)
            {
                affaireId = currentId;
            }
            else
            {
                affaireId.IsHisto = true;
            }

            var lignes = GetModeleLignes(modele, affaire, risque, objet, numOption, numFormule).ToList();
            if (!lignes.Any())
            {
                yield break;
            }
            var infos = (affaireId.IsHisto
                    ? this.hpISValRepository.GetByAffaire(affaire.TypeAffaire.AsCode(), affaire.CodeAffaire, affaire.NumeroAliment, affaire.NumeroAvenant)
                    : this.kpISValRepository.GetByAffaire(affaire.CodeAffaire, affaire.NumeroAliment)
                )
                .Where(x => lignes.Any(k => k.Code == x.Kkckgbnmid && x.Kkcrsq == (risque?.Numero ?? 0) && x.Kkcobj == (objet?.Id.NumObjet ?? 0) && x.Kkcfor == numFormule && x.Kkcopt == numOption))
                .Select(v =>
                {
                    var mdlLigne = lignes.FirstOrDefault(p => p.Code == v.Kkckgbnmid);
                    DateTime? dt1 = null;
                    if (mdlLigne.Propriete?.TypeUIControl == TypeAffichage.Heure)
                    {
                        dt1 = Tools.MakeNullableDateTime(20000101, v.Kkcvheud, true);
                    }
                    else
                    {
                        dt1 = Tools.MakeNullableDateTime(v.Kkcvdatd, v.Kkcvheud, true);
                    }
                    return new InformationSpecifique
                    {
                        Cle = v.Kkckgbnmid,
                        NumeroFormule = v.Kkcfor,
                        NumeroObjet = v.Kkcobj,
                        NumeroOption = v.Kkcopt,
                        NumeroRisque = v.Kkcrsq,
                        ModeleLigne = lignes.FirstOrDefault(p => p.Code == v.Kkckgbnmid),
                        Valeur = new ValeurInformationSpecifique
                        {
                            Nombre = v.Kkcvdec,
                            Texte = v.Kkcvtxt,
                            Unite = mdlLigne.HasUnite
                                ? mdlLigne.ListeUnites.FirstOrDefault(x => x.code == v.Kkcvun)
                                : (mdlLigne.Propriete?.HasUnite ?? false) ? (mdlLigne.Propriete?.ListeUnites.FirstOrDefault(x => x.code == v.Kkcvun) ?? default) : default,
                            DateMin = dt1,
                            DateMax = Tools.MakeNullableDateTime(v.Kkcvdatf, v.Kkcvheuf, true)
                        }
                    };
                });

            // return titles
            lignes = FormatModeleLignesByOrder(lignes) as List<LigneModeleIS>;
            if (!lignes.Any())
            {
                // invalid Modele
                throw new Exception($"Modèle {modele.Code} invalide");
            }
            foreach (var ligne in lignes.Where(x => x.Code.IsEmptyOrNull()))
            {
                yield return new InformationSpecifique { ModeleLigne = ligne };
            }

            //  return values
            if (infos.Any())
            {
                foreach (var info in infos)
                {
                    info.AffaireId = affaireId;
                    yield return info;
                }
            }
            else
            {
                foreach (var ligne in lignes.Where(x => x.Code.ContainsChars()))
                {
                    yield return new InformationSpecifique
                    {
                        Cle = ligne.Code,
                        NumeroFormule = numFormule,
                        NumeroObjet = objet?.Id.NumObjet ?? 0,
                        NumeroOption = numOption,
                        NumeroRisque = risque?.Numero ?? 0,
                        ModeleLigne = ligne
                    };
                }
            }
        }

        public (IEnumerable<ModeleIS> modeles, Affaire affaire, IEnumerable<Risque> risques) GetAllAffaireModeles(AffaireId affaireId)
        {
            var affaire = this.affaireRepository.GetById(affaireId);
            var risques = this.risqueRepository.GetRisquesByAffaire(affaireId) ?? null;
            var date = affaire.DateEffetAvenant ?? affaire.DateEffet;
            if (!date.HasValue && affaire.TypeAffaire == AffaireType.Offre)
            {
                date = affaire.DateSaisie;
            }
            var modeles = CacheGetModeles().Where(x => (!affaire.DateFinCalculee.HasValue || x.DateDebut <= affaire.DateFinCalculee) && x.DateFin >= date && x.CodeBranche == affaire.Branche.Code).ToList();
            return (modeles, affaire, risques);
        }

        public IEnumerable<SectionIS> GetKPIRSKeys(int maxResults, bool fromHisto = false)
        {
            if (fromHisto)
            {
                return this.hpirsobRepository.SelectHPIR();
            }
            return this.kpIrsObRepository.SelectKPIR(maxResults);
        }

        public void SaveISVals(AffaireId affaireId, IEnumerable<InformationSpecifique> infos, string codeModele, string user)
        {
            var valeurs = affaireId.IsHisto
                ? this.hpISValRepository.GetByAffaire(affaireId.TypeAffaire.AsCode(), affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant.Value)
                : this.kpISValRepository.GetByAffaire(affaireId.CodeAffaire, affaireId.NumeroAliment);
            foreach (var info in infos)
            {
                var v = valeurs.FirstOrDefault(x => x.Kkckgbnmid == info.Cle && x.Kkcrsq == info.NumeroRisque && x.Kkcobj == info.NumeroObjet
                                                && x.Kkcfor == info.NumeroFormule && x.Kkcopt == info.NumeroOption);
                bool isNew = v is null;
                bool? qmChanged = null;
                if (info.Cle == ProprieteIS.CleQuestionMedical)
                {
                    qmChanged = false;
                }
                if (isNew)
                {
                    if (!affaireId.IsHisto && qmChanged.HasValue)
                    {
                        qmChanged = true;
                    }
                    v = new KPISVal
                    {
                        Kkcalx = affaireId.NumeroAliment,
                        Kkcavn = affaireId.NumeroAvenant.Value,
                        Kkcfor = info.NumeroFormule,
                        Kkchin = 1,
                        Kkcipb = affaireId.CodeAffaire,
                        Kkckgbnmid = info.Cle,
                        Kkcobj = info.NumeroObjet,
                        Kkcopt = info.NumeroOption,
                        Kkcrsq = info.NumeroRisque,
                        Kkctyp = affaireId.TypeAffaire.AsCode()
                    };
                }
                else if (!affaireId.IsHisto)
                {
                    if (qmChanged.HasValue && info.Valeur.Texte.AsBool() != v.Kkcvtxt.AsBool())
                    {
                        qmChanged = true;
                    }
                }
                v.Kkcvdec = info.Valeur.Nombre;
                v.Kkcvtxt = info.Valeur.Texte;
                v.Kkcvun = info.Valeur.Unite.code ?? string.Empty;
                v.Kkcvdatd = info.Valeur.DateMin.AsDate();
                v.Kkcvdatf = info.Valeur.DateMax.AsDate();
                v.Kkcvheud = info.Valeur.DateMin.AsTime6();
                v.Kkcvheuf = info.Valeur.DateMax.AsTime6();
                v.Kkcisval = FormatISVal(v, codeModele);

                if (isNew)
                {
                    if (affaireId.IsHisto)
                    {
                        this.hpISValRepository.Insert(v);
                    }
                    else
                    {
                        this.kpISValRepository.Insert(v);
                    }
                }
                else if (!affaireId.IsHisto)
                {
                    this.kpISValRepository.Update(v);
                }
                if (qmChanged ?? false)
                {
                    // add control trace Assiette
                    this.controleRepository.InsertControlAssiette(affaireId, user, "IS Rsq/Obj", "Q.M.");
                }
            }
        }

        public void Reprise(AffaireId id)
        {
            var listVals = this.kpISValRepository.GetByAffaire(id.CodeAffaire, id.NumeroAliment).ToList();
            listVals.ForEach(x => this.kpISValRepository.Delete(x));
            var histoVals = this.hpISValRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histoVals.ForEach(x => this.hpISValRepository.Delete(x));
            histoVals.ForEach(x => this.kpISValRepository.Insert(x));
        }

        private string FormatISVal(KPISVal v, string codeModele)
        {
            string isVal = string.Empty;
            if (v.Kkcvdatd > 0)
            {
                isVal += $" {(v.Kkcvdatf > 0 ? "du " : "")} {AlbConvert.ConvertIntToDate(v.Kkcvdatd)}";
            }
            if (v.Kkcvheud > 0)
            {
                isVal += $" {AlbConvert.ConvertIntToTimeMinute(v.Kkcvheud)}";
            }
            if (v.Kkcvdatf > 0)
            {
                isVal += $" {(v.Kkcvdatd > 0 ? "au " : "")} {AlbConvert.ConvertIntToDate(v.Kkcvdatf)}";
            }
            if (v.Kkcvheuf > 0)
            {
                isVal += $" {AlbConvert.ConvertIntToTimeMinute(v.Kkcvheuf)}";
            }

            KISRef iSRefs = this.kISRefRepository.Get(v.Kkckgbnmid);
            if ((iSRefs.Kgbtypz == "Int64" || iSRefs.Kgbtypz == "Double") && iSRefs.Kgbtypu == "Text")
            {
                isVal += $" {v.Kkcvdec}";
            }

            isVal += $" {(!v.Kkcvun.IsEmptyOrNull() ? v.Kkcvun : string.Empty)}";

            isVal += $" {(!v.Kkcvtxt.IsEmptyOrNull() ? GetLibISVal(v.Kkckgbnmid, codeModele, v.Kkcvtxt, iSRefs) : string.Empty)}";

            return isVal.Trim();
        }

        private string GetLibISVal(string keyIS, string codeModele, string valCode, KISRef iSRefs)
        {
            string lib = valCode;

            KISModl iSModl = this.kISModlRepository.SelectByModel(codeModele).FirstOrDefault(x => x.Kgdnmid == keyIS);

            if (!iSModl.Kgdvucon.IsEmptyOrNull() || !iSRefs.Kgbvucon.IsEmptyOrNull())
            {
                lib = referentialRepository.GetValues(iSModl.Kgdvucon ?? iSRefs.Kgbvucon, iSModl.Kgdvufam ?? iSRefs.Kgbvufam).FirstOrDefault(x => x.Code == valCode)?.LibelleLong;
            }

            return lib;
        }

        private static IEnumerable<LigneModeleIS> FormatModeleLignesByOrder(IEnumerable<LigneModeleIS> lignes)
        {
            bool continueCheck = true;
            int index;
            var sortedLines = new List<LigneModeleIS>(lignes);
            while (continueCheck)
            {
                continueCheck = false;
                sortedLines = lignes.OrderBy(x => x.Ordre).ToList();
                for (index = 0; index < sortedLines.Count; index++)
                {
                    if (sortedLines[index].TypePresentation == 3)
                    {
                        if (index < 2)
                        {
                            continueCheck = true;
                            break;
                        }
                        int x = index - 1;
                        while (x >= 0 && sortedLines[x].TypePresentation == 3) { x--; }
                        if (sortedLines[x].TypePresentation != 2 || x == 0)
                        {
                            continueCheck = true;
                            break;
                        }
                        if (sortedLines[x - 1].TypePresentation == 2)
                        {
                            continueCheck = true;
                            break;
                        }
                    }
                    else if (sortedLines[index].TypePresentation == 2)
                    {
                        if (index == 0
                            || !sortedLines[index - 1].TypePresentation.IsIn(1, 3)
                            || index + 1 == sortedLines.Count
                            || sortedLines[index + 1].TypePresentation != 3)
                        {
                            continueCheck = true;
                            break;
                        }
                    }
                    else if (sortedLines[index].TypePresentation == 1)
                    {
                        if (index.IsIn(1, 2)
                            || index > 0 && sortedLines[index - 1].TypePresentation != 3)
                        {
                            continueCheck = true;
                            break;
                        }
                    }
                }
                if (continueCheck)
                {
                    sortedLines.RemoveAt(index);
                }
            }
            return sortedLines;
        }

        private IEnumerable<ModeleIS> CacheGetModeles()
        {
            return this.cache.Get(DBGetModeles);
        }

        private List<ModeleIS> DBGetModeles()
        {
            var refProps = this.kISRefRepository.GetAll();
            var mdls = this.kISModRepository.GetAll();
            var result = new List<ModeleIS>();
            foreach (var md in mdls)
            {
                var codes = md.Kgcmodid.Split('-');
                bool isRecup = false;
                if (codes.Length == 3 && codes[1] == "Recup")
                {
                    codes = new[] { codes[0], codes[2] };
                    isRecup = true;
                }
                result.Add(new ModeleIS
                {
                    Code = md.Kgcmodid,
                    DateDebut = Tools.MakeDateTime(md.Kgcdatd),
                    DateFin = Tools.MakeDateTime(md.Kgcdatf),
                    CodeBranche = codes.First().Trim(),
                    CodeCible = ModeleIS.ToutesCibles,
                    Section = Enum.TryParse(codes.Last().Trim(), true, out TypeSection s) ? s : 0,
                    IsRecup = isRecup,
                    Proprietes = this.kISModlRepository.SelectByModel(md.Kgcmodid).Select(x =>
                    {
                        var prop = refProps.FirstOrDefault(p => p.Kgbnmid == x.Kgdnmid);
                        var lgth = prop?.Kgblngz.Split(':');
                        return new LigneModeleIS
                        {
                            Code = x.Kgdnmid,
                            Id = x.Kgdid,
                            IsMandatory = x.Kgdobli.AsBool(),
                            IsReadonly = !x.Kgdmodi.AsBool(),
                            Libelle = x.Kgdlib,
                            Ordre = Convert.ToInt32(x.Kgdnumaff),
                            ParentId = x.Kgdparenid,
                            ComportementSiParent = x.Kgdparenc,
                            EvenementParent = x.Kgdparene,
                            SQLSelectParametres = x.Kgdsql,
                            NombreSautsLignes = x.Kgdsautl,
                            HasScriptAffichage = x.Kgdscraffs.AsBool(),
                            HasScriptControle = x.Kgdscrctrs.AsBool(),
                            CodeConditions = x.Kgdsaid2,
                            TypePresentation = x.Kgdpres,
                            ListeUnites = x.Kgdvucon.IsEmptyOrNull() ? null
                                : this.referentialRepository.GetValues(x.Kgdvucon, x.Kgdvufam).Select(v => (v.Code, v.Libelle)).ToList(),
                            ListeValeurs = this.kISModlRepository.GetListeValeurs(x),
                            Propriete = prop is null ? null : new ProprieteIS
                            {
                                Code = prop.Kgbnmid,
                                Description = prop.Kgbdesc,
                                Libelle = prop.Kgblib,
                                Type = prop.Kgbtypz,
                                Observations = prop.Kgbobsv,
                                IsMapped = prop.Kgbmapp.AsBool(),
                                TypeConvertion = prop.Kgbtypc,
                                TypePresentation = prop.Kgbpres,
                                Longueur = int.Parse(lgth.First()),
                                NbDecimales = lgth.Length > 1 ? int.Parse(lgth[1]) : 0,
                                IsMandatory = prop.Kgbobli.AsBool(),
                                TypeUIControl = Enum.TryParse(prop.Kgbtypu.Trim(), true, out TypeAffichage t) ? t : default,
                                HasScriptAffichage = prop.Kgbscraff.AsBool(),
                                HasScriptControle = prop.Kgbscrctr.AsBool(),
                                ListeUnites = prop.Kgbvucon.IsEmptyOrNull() ? null
                                    : this.referentialRepository.GetValues(prop.Kgbvucon, prop.Kgbvufam).Select(v => (v.Code, v.Libelle)).ToList()
                            }
                        };
                    }).ToList()
                });
            }
            return result;
        }
    }
}
