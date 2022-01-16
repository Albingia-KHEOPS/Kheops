using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using EmitMapper;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.NavigationArbre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre {
    [Serializable]
    public class ModeleNavigationArbre {
        public ModeleNavigationArbre() {
            Links = new SortedDictionary<(string, int), LienArbre>();
            IsStandardRegul = true;
            RegularisationStepList = new List<RegularisationStateModel>();
        }

        public IDictionary<(string, int), LienArbre> Links { get; }

        public string CodeOffre { get; set; }

        public string Type { get; set; }

        public int Version { get; set; }

        public bool IsAffaireNouvelle => Type == AlbConstantesMetiers.TYPE_CONTRAT && NumAvn < 1;

        public bool IsEtapeRegul => Etape == GetEtapeCode(ContextStepName.EditionRegularisation);

        public string Etape { get; set; }

        public int CodeRisque { get; set; }

        public int CodeFormule { get; set; }
        //public string ActeGestion { get; set; }
        //public string TabGuid { get; set; }

        public bool InformationsSaisie { get; set; }
        public string TagSaisie { get; set; }

        public bool InformationsGenerales { get; set; }
        public string TagInfoGen { get; set; }
        public bool CoAssureurs { get; set; }
        public string TagCoAssureurs { get; set; }

        public bool CoCourtiers { get; set; }
        public string TagCoCourtiers { get; set; }

        public bool MatriceRisques;
        public string TagMatriceRisques { get; set; }

        public bool MatriceFormules;
        public string TagMatriceFormules { get; set; }

        public bool MatriceGaranties;
        public string TagMatriceGaranties { get; set; }

        public List<RisqueDto> Risques;

        public bool Engagement;
        public string TagEngagement { get; set; }

        public bool Evenement;
        public string TagEvenement { get; set; }

        public bool MontantRef { get; set; }
        public string TagMontantRef { get; set; }

        public bool? Attentat { get; set; }
        public string TagAttentat { get; set; }

        public bool Cotisation;
        public string TagCotisation { get; set; }

        public bool Fin;
        public string TagFin { get; set; }

        public bool Documents { get; set; }
        public string TagDocuments { get; set; }

        public bool ListeClauses;
        public string TagListeClauses { get; set; }

        public bool ListeInventaires;
        public string TagListeInventaires { get; set; }

        public bool GestionDocuments;
        public string TagGestionDocuments { get; set; }

        public string OffreIdentification;

        public string ModeNavig { get; set; }
        public bool IsReadOnly { get; set; }

        public string ScreenType { get; set; }
        public bool IsValidation { get; set; }

        public bool IsStandardRegul { get; set; }

        public bool IsEmptyRequested { get; set; }
        public bool IsTransverseAllowed { get; set; }
        public bool IsRegule { get; set; }

        public bool IsReguleValidated { get; set; }

        public List<ModeleAvenantAlerte> AlertesAvenant { get; set; }

        public bool IsCheckedEcheance { get; set; }
        public int NumAvn { get; set; }
        public bool IsScreenResil => ScreenType == AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
        public bool IsScreenResilPartial => ScreenType == AlbConstantesMetiers.SCREEN_TYPE_AVNRS && IsCheckedEcheance && !IsReadOnly;
        public bool IsScreenAvnModif => IsScreenResil || ScreenType.IsIn(
            AlbConstantesMetiers.SCREEN_TYPE_AVNMD,
            AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF,
            AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR);
        public bool IsScreenAvenant => IsScreenAvnModif
            || ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REGUL && IsReadOnly
            || ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF;
        public bool IsScreenRmVgReadonly => ScreenType == AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF;
        public bool IsScreenNoResilNoRMV => !IsScreenResil && !IsScreenRmVgReadonly;

        public List<RegularisationStateModel> RegularisationStepList { get; set; }

        public bool IsMonoRisque { get; set; }

        public bool IsMonoGarantie { get; set; }

        public static explicit operator ModeleNavigationArbre(NavigationArbreDto navigationArbreDto) {
            return ObjectMapperManager.DefaultInstance.GetMapper<NavigationArbreDto, ModeleNavigationArbre>().Map(navigationArbreDto);
        }

        public static NavigationArbreDto LoadDto(ModeleNavigationArbre modele) {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleNavigationArbre, NavigationArbreDto>().Map(modele);
        }
        public string LastValidNumAvt { get; set; }

        public long LastValidRgId { get; set; }

        public string LastNumAvt { get; set; }

        public int CodeOption { get; set; }

        public string CodeAffaire => CodeOffre + "_" + Version.ToString() + "_" + Type;

        internal void BuildLinks(NavigationArbreDto arbreDto, bool isRegule = false) {
            Links.Clear();
            if (CodeOffre.IsEmptyOrNull() || IsEmptyRequested) {
                return;
            }
            if (IsValidation) {
                AddControleFinLink(arbreDto);
                return;
            }

            AddInfosLink(arbreDto);

            var target = ContextStepName.EditionCoAssureurs;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagCoAssureurs.AsBoolean() ?? false,
                IsDisabled = !arbreDto.CoAssureurs,
                IsNew = !arbreDto.TagCoAssureurs.AsBoolean() ?? true,
                IsVisible = Type != AffaireType.Offre.AsCode() && (IsScreenNoResilNoRMV || IsReadOnly),
                Name = GetEtapeName(target),
                Target = target,
                Url = $"AnCoAssureurs/Index/{CodeAffaire}"
            });
            target = ContextStepName.EditionCommissions;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagCoCourtiers.AsBoolean() ?? false,
                IsDisabled = !arbreDto.CoCourtiers,
                IsNew = !arbreDto.TagCoCourtiers.AsBoolean() ?? true,
                IsVisible = Type != AffaireType.Offre.AsCode() && (IsScreenNoResilNoRMV || IsReadOnly),
                Name = GetEtapeName(target),
                Target = target,
                Url = $"AnCourtier/Index/{CodeAffaire}"
            });

            if ((arbreDto.Risques?.Any() ?? false) || IsScreenAvenant) {
                AddLinksRisques(arbreDto);
            }
            if (Type == AffaireType.Contrat.AsCode() && IsScreenAvenant && !IsEtapeRegul) {
                AddAvenantLink(arbreDto);
            }
            if (IsRegule || isRegule) {
                AddRegulLink(arbreDto);
            }

            if (!IsScreenResilPartial) {
                target = ContextStepName.EditionEngagements;
                Links.Add(GetLinkOrder(target), new LienArbre(this) {
                    Code = GetEtapeCode(target),
                    IsActive = arbreDto.TagEngagement.AsBoolean() ?? false,
                    IsDisabled = !arbreDto.Engagement,
                    IsNew = !arbreDto.TagEngagement.AsBoolean() ?? true,
                    IsVisible = Type == AffaireType.Offre.AsCode() || IsReadOnly || !IsScreenRmVgReadonly,
                    Name = GetEtapeName(target),
                    Target = target,
                    Url = $"{(Type == AffaireType.Contrat.AsCode() && IsScreenAvenant ? "EngagementPeriodes" : "Engagements")}/Index/{CodeAffaire}"
                });

                target = ContextStepName.Attentat;
                Links.Add(GetLinkOrder(target), new LienArbre(this) {
                    Code = GetEtapeCode(target),
                    IsActive = arbreDto.TagAttentat.AsBoolean() ?? false,
                    IsDisabled = !arbreDto.Attentat ?? true,
                    IsNew = !arbreDto.TagAttentat.AsBoolean() ?? true,
                    IsVisible = arbreDto.Attentat.HasValue && (Type == AffaireType.Offre.AsCode() || IsReadOnly || !IsScreenRmVgReadonly),
                    Name = GetEtapeName(target),
                    Target = target,
                    Url = $"AttentatGareat/Index/{CodeAffaire}"
                });

                if (Type == AffaireType.Offre.AsCode() || !IsScreenResil || IsReadOnly) {
                    AddMontantRefLink(arbreDto);
                }
            }
            AddQuittanceLink(arbreDto);
            AddControleFinLink(arbreDto);
        }

        private void AddInfosLink(NavigationArbreDto arbreDto) {
            var target = ContextStepName.EditionInfosBase;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagSaisie.AsBoolean() ?? false,
                IsDisabled = false,
                IsNew = !arbreDto.TagSaisie.AsBoolean() ?? true,
                IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                Name = GetEtapeName(target),
                Target = target,
                Url = $"{(Type == AffaireType.Offre.AsCode() ? "CreationSaisie" : "AnCreationContrat")}/Index/{CodeAffaire}",
                IsNavigationStep = true
            });
            target = ContextStepName.EditionInfosGenerales;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagInfoGen.AsBoolean() ?? false,
                IsDisabled = !arbreDto.InformationsGenerales,
                IsNew = !arbreDto.TagInfoGen.AsBoolean() ?? true,
                IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                Name = GetEtapeName(target),
                Target = target,
                Url = $"{(Type == AffaireType.Offre.AsCode() ? "ModifierOffre" : IsScreenAvnModif || arbreDto.NumAvn > 0 ? "AvenantInfoGenerales" : "AnInformationsGenerales")}/Index/{CodeAffaire}",
                IsNavigationStep = true
            });
        }

        private void AddLinksRisques(NavigationArbreDto arbreDto) {
            ContextStepName target = 0;
            var matricesNode = new LienArbre(this, true) {
                Code = string.Empty,
                IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                Name = "Matrices"
            };
            target = ContextStepName.MatriceRisque;
            Links.Add((string.Empty, GetLinkOrder(target).order), matricesNode);
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagMatriceRisques.AsBoolean() ?? false,
                IsDisabled = !arbreDto.MatriceRisques,
                IsNew = !arbreDto.TagMatriceRisques.AsBoolean() ?? true,
                IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                Name = GetEtapeName(target),
                Target = target,
                Url = $"{target}/Index/{CodeAffaire}",
                ParentLink = matricesNode,
                IsNavigationStep = true
            });

            if (arbreDto.Risques.Any(x => x.Formules.Any())) {
                target = ContextStepName.MatriceFormule;
                Links.Add(GetLinkOrder(target), new LienArbre(this) {
                    Code = GetEtapeCode(target),
                    IsActive = arbreDto.TagMatriceFormules.AsBoolean() ?? false,
                    IsDisabled = !arbreDto.MatriceFormules,
                    IsNew = !arbreDto.TagMatriceFormules.AsBoolean() ?? true,
                    IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                    Name = GetEtapeName(target),
                    Target = target,
                    Url = $"{target}/Index/{CodeAffaire}",
                    ParentLink = matricesNode,
                    IsNavigationStep = true
                });
                target = ContextStepName.MatriceGarantie;
                Links.Add(GetLinkOrder(target), new LienArbre(this) {
                    Code = GetEtapeCode(target),
                    IsActive = arbreDto.TagMatriceGaranties.AsBoolean() ?? false,
                    IsDisabled = !arbreDto.MatriceGaranties,
                    IsNew = !arbreDto.TagMatriceGaranties.AsBoolean() ?? true,
                    IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                    Name = GetEtapeName(target),
                    Target = target,
                    Url = $"{target}/Index/{CodeAffaire}",
                    ParentLink = matricesNode,
                    IsNavigationStep = true
                });
            }

            target = ContextStepName.DetailsRisque;
            var risquesNode = new LienArbre(this, true) {
                Code = string.Empty,
                IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                Name = "Risques"
            };
            Links.Add(GetLinkOrder(target), risquesNode);
            arbreDto.Risques.ForEach(risque => {
                target = ContextStepName.DetailsRisque;
                int rsq = risque.Code;
                var link = new LienArbre(this, risque.Formules.Any()) {
                    ParentLink = risquesNode,
                    Code = GetEtapeCode(target),
                    Name = GetEtapeName(target) + " " + rsq,
                    IsActive = risque.TagRisque.AsBoolean() ?? false,
                    IsDisabled = false,
                    IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                    IsNew = !risque.TagRisque.AsBoolean() ?? true,
                    Target = target,
                    Url = $"DetailsRisque/Index/{CodeAffaire}_{rsq}",
                    FilterCurrentStep = () => CodeRisque == rsq,
                    Description = risque.Descriptif,
                    IsNavigationStep = true
                };
                link.TargetData.Add("risque", risque.Numero.ToString());
                Links.Add(("Risque", rsq), link);
                foreach (var fo in risque.Formules) {
                    target = ContextStepName.EditionOption;
                    int num = fo.CodeFormule;
                    var foLink = new LienArbre(this, true) {
                        Code = GetEtapeCode(target),
                        Name = $"Formule {fo.Alpha}",
                        ParentLink = link,
                        IsVisible = IsScreenNoResilNoRMV || IsReadOnly
                    };
                    bool addConditions = Type == AffaireType.Offre.AsCode() || IsAffaireNouvelle || fo.CreateModifAvn;
                    if (Type == AffaireType.Offre.AsCode()) {
                        fo.Options.ForEach(o => {
                            int opt = o.Option;
                            var optLink = new LienArbre(this, true) {
                                Code = GetEtapeCode(target),
                                IsActive = o.TagOption.AsBoolean() ?? false,
                                FilterCurrentStep = () => CodeFormule == num && CodeOption == opt,
                                IsDisabled = false,
                                IsNew = !o.TagOption.AsBoolean() ?? true,
                                IsVisible = IsScreenNoResilNoRMV || IsReadOnly,
                                Name = $"Option {opt}",
                                ParentLink = foLink,
                                Target = target,
                                Url = $"FormuleGarantie/Index/{CodeAffaire}_{num}_{opt}",
                                IsNavigationStep = true,
                                Description = $"Formule {o.Description}"
                            };
                            optLink.TargetData.Add("formule", num.ToString());
                            optLink.TargetData.Add("option", opt.ToString());
                            Links.Add(("Option", int.Parse($"{num}{opt}")), optLink);
                            if (addConditions) {
                                AddLinkConditions(num, risque, optLink);
                            }
                        });
                    }
                    else {
                        int opt = fo.CodeOption;
                        foLink.IsNew = !fo.TagFormule.AsBoolean() ?? true;
                        foLink.IsActive = fo.TagFormule.AsBoolean() ?? false;
                        foLink.IsDisabled = false;
                        foLink.Target = target;
                        foLink.Url = $"FormuleGarantie/Index/{CodeAffaire}_{num}_{opt}";
                        foLink.FilterCurrentStep = () => CodeFormule == num && CodeOption == opt;
                        foLink.TargetData.Add("formule", num.ToString());
                        foLink.TargetData.Add("option", opt.ToString());
                        foLink.IsNavigationStep = true;
                        foLink.Description = fo.Option.Description;
                        if (addConditions) {
                            AddLinkConditions(num, risque, foLink);
                        }
                    }
                    Links.Add(("Formule", num), foLink);
                }
            });
        }

        private void AddMontantRefLink(NavigationArbreDto arbreDto) {
            var target = ContextStepName.EditionMontantsReference;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagMontantRef.AsBoolean() ?? false,
                IsDisabled = !arbreDto.MontantRef,
                IsNew = !arbreDto.TagMontantRef.AsBoolean() ?? true,
                IsVisible = Type == AffaireType.Offre.AsCode() || IsReadOnly || IsScreenNoResilNoRMV,
                Name = GetEtapeName(target),
                Target = target,
                Url = $"AnMontantReference/Index/{CodeAffaire}"
            });
        }

        private void AddAvenantLink(NavigationArbreDto arbreDto) {
            var target = ContextStepName.EditionInfosAvenant;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsNew = true,
                IsVisible = true,
                Name = GetEtapeName(target),
                Target = target,
                Url = $"CreationAvenant/Index/{CodeAffaire}"
            });
        }

        private void AddRegulLink(NavigationArbreDto arbreDto) {
            var target = ContextStepName.ConsulterRegule;
            var link = new LienArbre(this) {
                Code = GetEtapeCode(target),
                Description = "Infos de régularisation",
                IsVisible = true,
                Name = GetEtapeName(target),
                Target = target,
                Url = $"CreationRegularisation/Step1_ChoixPeriode_FromNavig_Consult/{CodeAffaire}",
                IsActive = TagSaisie.AsBoolean() ?? false,
                IsNew = !TagSaisie.AsBoolean() ?? true
            };
            link.ParamsArbre.Add(ArbreParam.ActeGestionRegule, AlbConstantesMetiers.TYPE_AVENANT_MODIF);
            link.ParamsArbre.Add(ArbreParam.NewWindow, true.ToString().ToLower());
            link.ParamsArbre.Add(ArbreParam.ConsultOnly, true.ToString().ToLower());
            Links.Add(GetLinkOrder(target), link);
        }

        private bool TryBuildValidation(NavigationArbreDto arbreDto) {
            if (IsValidation && (Type == AffaireType.Offre.AsCode() || !IsEtapeRegul)) {
                AddControleFinLink(arbreDto);
            }
            return IsValidation;
        }

        private void AddControleFinLink(NavigationArbreDto arbreDto) {
            var target = ContextStepName.ControleFin;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagFin.AsBoolean() ?? false,
                IsDisabled = !arbreDto.Fin || !arbreDto.Cotisation || ModeNavig == ModeConsultation.Historique.AsCode(),
                IsNew = !arbreDto.TagFin.AsBoolean() ?? true,
                IsVisible = true,
                Name = (Type == AffaireType.Offre.AsCode() || !IsReadOnly) ? GetEtapeName(target) : "Gestion doc.",
                Target = target,
                Url = $"ControleFin/Index/{CodeAffaire}",
                HtmlId = "ControleFinMenuArbreLI"
            });
        }

        private void AddLinkConditions(int num, RisqueDto risqueDto, LienArbre parent) {
            var target = ContextStepName.ConditionsGarantie;
            var urlParts = parent.Url.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            urlParts[0] = target.ToString();
            urlParts[2] = urlParts[2] + "_" + risqueDto.Code;
            var cndLink = new LienArbre(this) {
                Target = target,
                Code = GetEtapeCode(target),
                IsActive = parent.IsActive,
                IsDisabled = parent.IsDisabled,
                FilterCurrentStep = parent.FilterCurrentStep,
                IsNew = parent.IsNew,
                IsVisible = parent.IsVisible,
                Name = GetEtapeName(target),
                Description = "Conditions tarifaires",
                Url = string.Join("/", urlParts),
                ParentLink = parent
            };
            Links.Add(("Conditions", num), cndLink);
        }

        private void AddQuittanceLink(NavigationArbreDto arbreDto) {
            var target = ContextStepName.EditionQuittance;
            Links.Add(GetLinkOrder(target), new LienArbre(this) {
                Code = GetEtapeCode(target),
                IsActive = arbreDto.TagCotisation.AsBoolean() ?? false,
                IsDisabled = !arbreDto.Cotisation,
                IsNew = !arbreDto.TagCotisation.AsBoolean() ?? true,
                IsVisible = true,
                Name = GetEtapeName(target),
                Target = target,
                Url = $"{(Type == AffaireType.Offre.AsCode() ? "Cotisations" : "Quittance")}/Index/{CodeAffaire}",
                HtmlId = "CotisationMenuArbreLI"
            });
        }

        private (string type, int order) GetLinkOrder(ContextStepName target) {
            switch (target) {
                case ContextStepName.ConsulterRegule:
                    return (string.Empty, 1);
                case ContextStepName.EditionInfosBase:
                    return (string.Empty, 3);
                case ContextStepName.EditionInfosGenerales:
                    return (string.Empty, 4);
                case ContextStepName.EditionCoAssureurs:
                    return (string.Empty, 5);
                case ContextStepName.EditionCommissions:
                    return (string.Empty, 6);
                case ContextStepName.EditionInfosAvenant:
                    return (string.Empty, 2);
                case ContextStepName.MatriceRisque:
                    return ("Matrice", 7);
                case ContextStepName.MatriceFormule:
                    return ("Matrice", 8);
                case ContextStepName.MatriceGarantie:
                    return ("Matrice", 9);
                case ContextStepName.DetailsRisque:
                    return (string.Empty, 10);
                case ContextStepName.EditionOption:
                    return (string.Empty, 11);
                case ContextStepName.ConditionsGarantie:
                    return (string.Empty, 12);
                case ContextStepName.EditionEngagements:
                case ContextStepName.EngagementPeriodes:
                    return (string.Empty, 13);
                case ContextStepName.Attentat:
                    return (string.Empty, 14);
                case ContextStepName.EditionMontantsReference:
                    return (string.Empty, 15);
                case ContextStepName.EditionQuittance:
                case ContextStepName.AnnulationQuittances:
                    return (string.Empty, 16);
                case ContextStepName.ControleFin:
                    return (string.Empty, 17);
                default:
                    return (string.Empty, -1);
            }
        }

        private static string GetEtapeCode(ContextStepName stepName) {
            switch (stepName) {
                case ContextStepName.ConsulterRegule:
                case ContextStepName.EditionRegularisation:
                case ContextStepName.EditionRegularisationEtAvenant:
                    return "Regule";
                case ContextStepName.EditionInfosBase:
                    return "InfoSaisie";
                case ContextStepName.EditionInfosGenerales:
                    return "InfoGen";
                case ContextStepName.EditionCoAssureurs:
                    return "CoAssureurs";
                case ContextStepName.EditionCommissions:
                    return "CoCourtiers";
                case ContextStepName.EditionInfosAvenant:
                    return "";
                case ContextStepName.MatriceRisque:
                    return "MatriceRisques";
                case ContextStepName.MatriceFormule:
                    return "MatriceFormules";
                case ContextStepName.MatriceGarantie:
                    return "MatriceGaranties";
                case ContextStepName.DetailsRisque:
                    return "Risque";
                case ContextStepName.EditionOption:
                    return "Formule";
                case ContextStepName.ConditionsGarantie:
                    return "Condition";
                case ContextStepName.EditionEngagements:
                case ContextStepName.EngagementPeriodes:
                    return "Engagement";
                case ContextStepName.Attentat:
                    return "Attentat";
                case ContextStepName.EditionMontantsReference:
                    return "MontantRef";
                case ContextStepName.EditionQuittance:
                case ContextStepName.AnnulationQuittances:
                    return "Cotisation";
                case ContextStepName.ControleFin:
                    return "Fin";
                default:
                    return string.Empty;
            }
        }

        private static string GetEtapeName(ContextStepName stepName) {
            switch (stepName) {
                case ContextStepName.ConsulterRegule:
                    return "Regularisation";
                case ContextStepName.EditionInfosBase:
                    return "Infos. de base";
                case ContextStepName.EditionInfosGenerales:
                    return "Infos. Générales";
                case ContextStepName.EditionCoAssureurs:
                    return "Co-assureurs";
                case ContextStepName.EditionCommissions:
                    return "Commissions";
                case ContextStepName.EditionInfosAvenant:
                    return "Infos. Avenant";
                case ContextStepName.MatriceRisque:
                    return "Matrice Risques";
                case ContextStepName.MatriceFormule:
                    return "Matrice Formules";
                case ContextStepName.MatriceGarantie:
                    return "Matrice Garanties";
                case ContextStepName.DetailsRisque:
                    return "Risque";
                case ContextStepName.EditionOption:
                    return "";
                case ContextStepName.ConditionsGarantie:
                    return "Conditions";
                case ContextStepName.EditionEngagements:
                case ContextStepName.EngagementPeriodes:
                    return "Engagements";
                case ContextStepName.Attentat:
                    return "Attentat";
                case ContextStepName.EditionMontantsReference:
                    return "Montant Réf";
                case ContextStepName.EditionQuittance:
                case ContextStepName.AnnulationQuittances:
                    return "Cotisations";
                case ContextStepName.ControleFin:
                    return "Contrôle & Fin";
                default:
                    return string.Empty;
            }
        }
    }

    public enum RegulSteps {
        Step1_ChoixPeriode = 1,
        Step2_ChoixRisque = 2,
        Step3_ChoixGarantie = 3,
        Step4_ChoixPeriodeGarantie = 4,
        Step5_RegulContrat = 5,
        Step6_Cotisations = 6,
        Step7_Fin = 7,
    }

}
