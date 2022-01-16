using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using Albingia.Kheops.OP.Domain.Risque;
using ALBINGIA.Framework.Common.Extensions;
using Mapster;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Activation;
using Albingia.Kheops.OP.Application.Contracts;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class InfosSpecifiquesService : IInfosSpecifiquesPort
    {
        private const string MandatoryMessage = "Champ obligatoire";
        private readonly ISessionContext context;
        private readonly IAffaireRepository affaireRepository;
        private readonly IInfosSpecifiquesRepository infosSpecifiquesRepository;
        private readonly ITraceRepository traceRepository;

        public InfosSpecifiquesService(ISessionContext context, IAffaireRepository affaireRepository, IInfosSpecifiquesRepository infosSpecifiquesRepository, ITraceRepository traceRepository)
        {
            this.context = context;
            this.affaireRepository = affaireRepository;
            this.infosSpecifiquesRepository = infosSpecifiquesRepository;
            this.traceRepository = traceRepository;
        }

        public InfosSpecifiquesDto GetByAffaireAndSection(AffaireId affaireId, SectionISDto section)
        {
            affaireId = this.affaireRepository.ValidateId(affaireId);
            var (modeles, affaire, risques) = this.infosSpecifiquesRepository.GetAllAffaireModeles(affaireId);
            ModeleIS modele = modeles.FirstOrDefault(x => x.Code == $"{affaire.Branche.Code}{(affaire.IsKheops ? string.Empty : "-Recup")}-{section.Type}");
            if (modele is null)
            {
                return new InfosSpecifiquesDto
                {
                    Modele = null,
                    Infos = Enumerable.Empty<InformationSpecifiqueDto>()
                };
            }
            IEnumerable<InformationSpecifique> infos;
            Risque risque = risques.FirstOrDefault(x => x.Numero == section.NumeroRisque);
            switch (section.Type)
            {
                case TypeSection.Risques:
                    infos = this.infosSpecifiquesRepository.GetModeleInfos(modele, affaire, risque, null).ToList();
                    break;
                case TypeSection.Objets:
                    infos = this.infosSpecifiquesRepository.GetModeleInfos(
                        modele,
                        affaire,
                        risque,
                        risque.Objets.First(x => x.Id.NumObjet == section.NumeroObjet)).ToList();
                    break;
                case TypeSection.Garanties:
                    infos = this.infosSpecifiquesRepository.GetModeleInfos(modele, affaire, risque, null, section.NumeroOption, section.NumeroFormule).ToList();
                    break;
                case TypeSection.Entete:
                    infos = this.infosSpecifiquesRepository.GetModeleInfos(modele, affaire, null, null, section.NumeroOption, section.NumeroFormule).ToList();
                    break;
                default:
                    return new InfosSpecifiquesDto
                    {
                        Modele = null,
                        Infos = Enumerable.Empty<InformationSpecifiqueDto>()
                    };
            }

            return new InfosSpecifiquesDto
            {
                Modele = modele.Adapt<ModeleISDto>(),
                Infos = infos.Select(i =>
                {
                    var dto = i.Adapt<InformationSpecifiqueDto>();
                    if (i.Valeur is null)
                    {
                        dto.Valeur = new InfoSpeValeurDto { Val1 = string.Empty, Val2 = string.Empty };
                    }
                    else
                    {
                        dto.Valeur = new InfoSpeValeurDto { Unite = i.Valeur.Unite.code.IsEmptyOrNull(false) ? string.Empty : i.Valeur.Unite.code };
                        (dto.Valeur.Val1, dto.Valeur.Val2) = i.Valeur.ToVal1Val2(i.ModeleLigne);
                    }
                    return dto;
                }).OrderBy(i => i.ModeleLigne.Ordre),
                ShouldRestoreOldValues = infos.Any() && !infos.Any(x => x.Valeur != null)
            };
        }

        public bool HasModeleLignes(AffaireId affaireId, SectionISDto section)
        {
            affaireId = this.affaireRepository.ValidateId(affaireId);
            var (modeles, affaire, risques) = this.infosSpecifiquesRepository.GetAllAffaireModeles(affaireId);
            ModeleIS modele = modeles.FirstOrDefault(x => x.Code == $"{affaire.Branche.Code}-{section.Type}");
            if (modele is null)
            {
                return false;
            }
            IEnumerable<LigneModeleIS> lignes;
            Risque risque = risques.FirstOrDefault(x => x.Numero == section.NumeroRisque);
            switch (section.Type)
            {
                case TypeSection.Risques:
                    lignes = this.infosSpecifiquesRepository.GetModeleLignes(modele, affaire, risque, null).ToList();
                    break;
                case TypeSection.Objets:
                    lignes = this.infosSpecifiquesRepository.GetModeleLignes(
                        modele,
                        affaire,
                        risque,
                        risque.Objets.First(x => x.Id.NumObjet == section.NumeroObjet)).ToList();
                    break;
                case TypeSection.Garanties:
                    lignes = this.infosSpecifiquesRepository.GetModeleLignes(modele, affaire, risque, null, section.NumeroOption, section.NumeroFormule).ToList();
                    break;
                case TypeSection.Entete:
                    lignes = this.infosSpecifiquesRepository.GetModeleLignes(modele, affaire, null).ToList();
                    break;
                default:
                    return false;
            }

            return lignes.Any();
        }

        public IEnumerable<(AffaireId affaire, SectionISDto section)> GetExistingOldIS(int maxResults = 1000, bool fromHisto = false)
        {
            var oldIS = this.infosSpecifiquesRepository.GetKPIRSKeys(maxResults, fromHisto);
            return oldIS.Select(i => (
                new AffaireId
                {
                    CodeAffaire = i.Ipb.ToIPB(),
                    NumeroAliment = i.Alx,
                    TypeAffaire = i.Typ.ParseCode<AffaireType>(),
                    NumeroAvenant = i.Avn,
                    IsHisto = fromHisto
                },
                new SectionISDto
                {
                    Type = (TypeSection)Enum.Parse(typeof(TypeSection), i.Section, true),
                    NumeroFormule = i.Formule,
                    NumeroObjet = i.Obj,
                    NumeroOption = i.Option,
                    NumeroRisque = i.Rsq,
                    Branche = i.Branche
                })).ToList();
        }

        public void TraceOldISTransfert(AffaireId affaireId, SectionISDto section, string commentaires)
        {
            var affaire = this.affaireRepository.GetLightByIpbAlx(affaireId.CodeAffaire, affaireId.NumeroAliment);
            if (affaire is null)
            {
                return;
            }
            this.traceRepository.Log(new Domain.SimpleTrace
            {
                AffaireId = affaireId,
                CodeGarantie = string.Empty,
                Commentaire = commentaires ?? string.Empty,
                DateCreation = DateTime.Now,
                Etape = "IS",
                Formule = section.NumeroFormule,
                Objet = section.NumeroObjet,
                Option = section.NumeroOption,
                Perimetre = section.Type.ToString(),
                Risque = section.NumeroRisque,
                Situation = affaire.SituationAffaire?.Code.ParseCode<SituationAffaire>() ?? 0,
                User = this.context.UserId ?? "ASYNC"
            });
        }

        public bool HasOldISTransfertLogId(Guid guid)
        {
            var logs = this.traceRepository.FindLogsByCommentEndsWith(DateTime.Now, guid.ToString());
            return logs.Any();
        }

        public void CancelOldISTransfert(Guid initGuid)
        {
            this.traceRepository.DeleteTraceOldISTransfert(DateTime.Now, initGuid.ToString());
        }

        public void SaveIS(InfosSpecifiquesDto infos)
        {
            var affaireId = infos.Infos.First().AffaireId;
            if (!ConfigurationManager.AppSettings["AllowHPISVALInsert"].AsBoolean() ?? true)
            {
                affaireId = this.affaireRepository.ValidateId(affaireId);
                if (affaireId.IsHisto)
                {
                    return;
                }
            }
            var (modeles, affaire, risques) = this.infosSpecifiquesRepository.GetAllAffaireModeles(affaireId);
            ModeleIS modele = modeles.FirstOrDefault(x => x.Code == $"{affaire.Branche.Code}-{infos.Modele.Section}");
            var i = infos.Infos.First();
            var risque = risques.FirstOrDefault(x => x.Numero == i.NumeroRisque);

            var lignes = risque != null ? this.infosSpecifiquesRepository.GetModeleLignes(
                modele, affaire,
                risque, risque.Objets.FirstOrDefault(x => x.Id.NumObjet == i.NumeroObjet),
                i.NumeroOption, i.NumeroFormule): this.infosSpecifiquesRepository.GetModeleLignes(
                modele, affaire);

            
            var allowedInfos = infos.Infos.Where(x =>
            {
                var p = lignes.FirstOrDefault(y => y.Code == x.Cle);
                return !p?.IsReadonly ?? false;
            });
            CheckISBeforeSave(modele, allowedInfos.ToList());
            var infosValeurs = allowedInfos.Select(x =>
            {
                var info = x.Adapt<InformationSpecifique>();
                info.ModeleLigne = modele.Proprietes.First(p => p.Code == x.Cle);
                info.Valeur.SetVals(info.ModeleLigne, x.Valeur.Val1, x.Valeur.Val2, x.Valeur.Unite);
                return info;
            }).ToList();
            this.infosSpecifiquesRepository.SaveISVals(affaireId, infosValeurs, modele.Code, this.context.UserId);
            Trace.WriteLine($"IS created for {affaireId.AsAffaireKey()} Affaire");
        }

        private void CheckISBeforeSave(ModeleIS modele, IEnumerable<InformationSpecifiqueDto> infosValeurs)
        {
            var errors = new List<ValidationError>();
            if (modele is null)
            {
                errors.Add("Modèle introuvable");
            }
            else
            {
                foreach (var p in modele.Proprietes.Where(x => x.Code.ContainsChars()))
                {
                    var info = infosValeurs.FirstOrDefault(x => x.Cle == p.Code);
                    string valeur = info?.Valeur?.Val1?.Trim() ?? string.Empty;
                    string valeur2 = info?.Valeur?.Val2?.Trim() ?? string.Empty;
                    if (p.IsMandatory)
                    {
                        var errorList = CheckISValeurObligatoire(p, valeur, valeur2);
                        if (errorList.Any() && (ConfigurationManager.AppSettings["KISVALBypassMandatory"].AsBoolean() ?? false))
                        {
                            SetDefaultValue(errorList.First().FieldName, p, info);
                        }
                        else
                        {
                            errors.AddRange(errorList);
                        }
                    }
                    if (valeur.Length > 0)
                    {
                        CheckISValeurRenseignee(errors, p, valeur, valeur2);
                    }
                }
            }
            if (errors.Any())
            {
                throw new BusinessValidationException(errors);
            }
        }

        private static void SetDefaultValue(string fieldName, LigneModeleIS p, InformationSpecifiqueDto info)
        {
            if (info is null)
            {
                return;
            }
            if (info.Valeur is null)
            {
                info.Valeur = new InfoSpeValeurDto();
            }
            switch (p.Propriete.TypeUIControl)
            {
                case TypeAffichage.Text:
                    if (p.Propriete.Type.ToUpper().IsIn(TypeCode.Double.ToString().ToUpper(), TypeCode.Int64.ToString().ToUpper()))
                    {
                        info.Valeur.Val1 = 0.ToString();
                    }
                    else
                    {
                        info.Valeur.Val1 = "-";
                    }
                    break;
                case TypeAffichage.CheckBox:
                    info.Valeur.Val1 = false.ToYesNo();
                    break;
                case TypeAffichage.Select:
                    info.Valeur.Val1 = p.ListeValeurs?.FirstOrDefault().Key ?? string.Empty;
                    break;
                case TypeAffichage.TextArea:
                    info.Valeur.Val1 = "-";
                    break;
                case TypeAffichage.Date:
                    info.Valeur.Val1 = new DateTime(2000, 1, 1).ToShortDateString();
                    break;
            }
        }

        private static IEnumerable<ValidationError> CheckISValeurObligatoire(LigneModeleIS p, string valeur, string valeur2)
        {
            var errors = new List<ValidationError>();
            (string fieldName, string error) = (p.Code, MandatoryMessage);
            var mdError = (fieldName, error);
            if (valeur.Length == 0 && !p.Propriete.TypeUIControl.IsIn(TypeAffichage.Periode, TypeAffichage.PeriodeHeure))
            {
                yield return mdError;
            }
            else
            {
                switch (p.Propriete.TypeUIControl)
                {
                    case TypeAffichage.Text:
                        if (p.Propriete.Type.ToUpper().IsIn(TypeCode.Double.ToString().ToUpper(), TypeCode.Int64.ToString().ToUpper())
                            && decimal.TryParse(valeur, out var d) && d == decimal.Zero)
                        {
                            yield return mdError;
                        }
                        break;
                    case TypeAffichage.Periode:
                    case TypeAffichage.PeriodeHeure:
                        if (valeur.Length == 0)
                        {
                            yield return mdError;
                        }
                        if (valeur2.Length == 0)
                        {
                            yield return new ValidationError("", "", $"{p.Code}_d2", mdError.fieldName, mdError.error);
                        }
                        break;
                }
            }
        }

        private static void CheckISValeurRenseignee(List<ValidationError> errors, LigneModeleIS p, string valeur, string valeur2)
        {
            switch (p.Propriete.TypeUIControl)
            {
                case TypeAffichage.CheckBox:
                    if (!valeur.AsBoolean(false).HasValue)
                    {
                        errors.Add((p.Code, "Valeur choix O/N invalide"));
                    }
                    break;
                case TypeAffichage.Date:
                    if (!DateTime.TryParse(valeur, out var d))
                    {
                        errors.Add((p.Code, "Date invalide"));
                    }
                    break;
                case TypeAffichage.Heure:
                    if (!DateTime.TryParse($"{DateTime.Now.ToShortDateString()} {valeur}", out d))
                    {
                        errors.Add((p.Code, "Heure invalide"));
                    }
                    break;
                case TypeAffichage.Text:
                    if (p.Propriete.Type.ToUpper() == TypeCode.Double.ToString().ToUpper())
                    {
                        valeur = valeur.Replace(".", ",");
                        if (!decimal.TryParse(valeur, out var dc))
                        {
                            errors.Add((p.Code, "Nombre décimale invalide"));
                        }
                        else
                        {
                            if (((int)dc).ToString().Length > (p.Propriete.Longueur - p.Propriete.NbDecimales))
                            {
                                errors.Add((p.Code, $"Nombre décimale trop grand ({new string('9', p.Propriete.Longueur - p.Propriete.NbDecimales)}.{new string('9', p.Propriete.NbDecimales)} max)"));
                            }
                            if ((dc - (int)dc).ToString().Length - 2 > p.Propriete.NbDecimales)
                            {
                                errors.Add((p.Code, $"Nombre décimale non autorisé ({p.Propriete.NbDecimales} max de décimales)"));
                            }
                        }
                    }
                    else if (p.Propriete.Type.ToUpper() == TypeCode.Int64.ToString().ToUpper())
                    {
                        if (!int.TryParse(valeur, out var i))
                        {
                            errors.Add((p.Code, "Nombre invalide"));
                        }
                        else if (i.ToString().Length > p.Propriete.Longueur)
                        {
                            errors.Add((p.Code, $"Nombre trop grand ({new string('9', p.Propriete.Longueur)} max)"));
                        }
                    }
                    break;
                case TypeAffichage.Periode:
                case TypeAffichage.PeriodeHeure:
                    bool valid = true;
                    if (!DateTime.TryParse(valeur, out d))
                    {
                        errors.Add((p.Code, "Date début invalide"));
                        valid = false;
                    }
                    if (!DateTime.TryParse(valeur2, out d))
                    {
                        errors.Add((p.Code, "Date fin invalide"));
                        valid = false;
                    }
                    if (valid)
                    {
                        var dateMin = DateTime.Parse(valeur);
                        var dateMax = DateTime.Parse(valeur2);
                        if (dateMin > dateMax)
                        {
                            errors.Add(new ValidationError(string.Empty, string.Empty, string.Empty, p.Code, "Dates de périodes incohérentes"));
                        }
                    }
                    break;
            }
        }
    }
}
