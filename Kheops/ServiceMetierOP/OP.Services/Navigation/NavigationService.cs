using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.NavigationArbre;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;

namespace OP.Services {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NavigationService : INavigationService {
        private readonly IDbConnection dbConnection;
        private readonly NavigationArbreRepository repository;
        private readonly CourtierRepository courtierRepository;

        private readonly FolderService folderService;
        private readonly IFormule formuleService;

        public NavigationService(IDbConnection dbConnection, NavigationArbreRepository navigationArbreRepository, FolderService folderService, CourtierRepository courtierRepository, IFormule formuleService) {
            this.dbConnection = dbConnection;
            this.repository = navigationArbreRepository;
            this.folderService = folderService;
            this.courtierRepository = courtierRepository;
            this.formuleService = formuleService;
        }

        /// <summary>
        /// Vérifie l'existance de l'étape
        /// dans l'arbre chargé pour l'offre
        /// </summary>
        private static bool ExisteInArbre(List<ArbreDto> arbres, Folder folder, string etape, string acteGestionRegule = null) {
            ArbreDto arbre = null;
            if (etape == ContextStepName.EditionQuittance.AsCode()) {
                if (acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_REGUL) {
                    arbre = arbres.FirstOrDefault(a => a.IsFolder(folder) && a.Etape == etape && a.Perimetre == etape);
                }
                else {
                    arbre = arbres.FirstOrDefault(a => a.IsFolder(folder) && a.Etape == etape && a.Perimetre == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule));
                }
            }
            else {
                arbre = arbres.FirstOrDefault(a => a.IsFolder(folder) && a.Etape == etape);
            }

            return !(arbre is null);
        }

        /// <summary>
        /// Récupère le tag de l'étape
        /// dans l'arbre chargé pour l'offre
        /// </summary>
        private static string GetTagInArbre(List<ArbreDto> arbres, Folder folder, string etape) {
            return arbres.FirstOrDefault(a => a.IsFolder(folder) && a.Etape == etape)?.PassageTag ?? string.Empty;
        }

        /// <summary>
        /// Récupère la liste des risques 
        /// avec les formules liées
        /// pour la navigation
        /// </summary>
        private List<RisqueDto> GetRisquesHierarchy(List<ArbreDto> arbres, Folder folder, ModeConsultation modeNavig) {
            List<RisqueDto> risques = new List<RisqueDto>();
            var lstRsq = arbres.FindAll(a => a.CodeRsq != 0 && a.Perimetre == "RSQ").GroupBy(a => a.CodeRsq).Select(a => a.First());
            if (lstRsq != null) {
                foreach (var rsq in lstRsq) {
                    List<FormuleDto> formules = new List<FormuleDto>();
                    var lstFor = arbres.FindAll(a => a.CodeRsq == rsq.CodeRsq && a.CodeFor != 0).GroupBy(a => a.CodeFor).Select(a => a.First());
                    if (lstFor != null) {
                        foreach (var form in lstFor) {
                            List<OptionDto> options = new List<OptionDto>();
                            OptionDto option = new OptionDto();
                            if (form.CodeFormuleAvt != 0 || modeNavig == ModeConsultation.Historique) {
                                var lstOpt = arbres.FindAll(a => a.CodeRsq == form.CodeRsq && a.CodeFor == form.CodeFor && a.CodeOpt != 0).GroupBy(a => a.CodeOpt).Select(a => a.First());
                                if (lstOpt != null) {
                                    //var isformulesortie = IsFormuleSortie(form);
                                    foreach (var opt in lstOpt) {
                                        if (opt.CodeOpt == 1) {
                                            option = new OptionDto {
                                                Formule = Convert.ToInt32(opt.CodeFor),
                                                Option = Convert.ToInt32(opt.CodeOpt),
                                                Description = opt.DescFor.Trim(),
                                                Risque = Convert.ToInt32(opt.CodeRsq),
                                                TagOption = opt.PassageTag
                                            };
                                        }

                                        options.Add(new OptionDto {
                                            Formule = Convert.ToInt32(opt.CodeFor),
                                            Option = Convert.ToInt32(opt.CodeOpt),
                                            Description = opt.DescFor.Trim(),
                                            Risque = Convert.ToInt32(opt.CodeRsq),
                                            TagOption = opt.PassageTag
                                        });
                                    }
                                }
                            }
                            var datedbavn = ((int)form.DateDebAvn).YMDToDate();
                            if (!datedbavn.HasValue || !this.formuleService.IsSortie(form.CodeOffre, (int)form.CodeRsq, (int)form.CodeObj, datedbavn.Value)) {
                                formules.Add(new FormuleDto {
                                    Alpha = form.LettreFor.Trim(),
                                    CodeFormule = Convert.ToInt32(form.CodeFor),
                                    CodeOption = Convert.ToInt32(form.CodeOpt),
                                    Option = option,
                                    Options = options,
                                    TagFormule = form.PassageTag,
                                    CreateModifAvn = form.CreateAvt == folder.NumeroAvenant || form.ModifAvt == folder.NumeroAvenant || folder.NumeroAvenant == 0
                                });
                            }
                        }
                    }
                    risques.Add(new RisqueDto {
                        Code = Convert.ToInt32(rsq.CodeRsq),
                        Descriptif = rsq.DescRsq.Trim(),
                        Numero = Convert.ToInt32(rsq.ChronoRsq),
                        Formules = formules,
                        TagRisque = rsq.PassageTag,
                        isBadDate = false
                    });
                }
            }

            return risques;
        }

        public NavigationArbreDto GetTreeHierarchy(Folder folder, bool isModifHorsAvenant, ModeConsultation modeNavig) {
            List<ArbreDto> tree = this.repository.GetFullTree(folder, isModifHorsAvenant, modeNavig);
            NavigationArbreDto navig = new NavigationArbreDto {
                CodeOffre = folder.CodeOffre.ToIPB(),
                Version = folder.Version,
                Type = folder.Type
            };

            if (tree?.Any() == true) {
                navig.InformationsSaisie = true;
                navig.TagSaisie = GetTagInArbre(tree, folder, "SAISIE");
                navig.OffreIdentification = tree.FirstOrDefault().DescOffre.Trim();
                navig.InformationsGenerales = ExisteInArbre(tree, folder, "GEN");
                navig.TagInfoGen = GetTagInArbre(tree, folder, "GEN");
                navig.MatriceRisques = ExisteInArbre(tree, folder, "RSQ");
                navig.TagMatriceRisques = GetTagInArbre(tree, folder, "RSQ");
                navig.MatriceFormules = ExisteInArbre(tree, folder, "OPT");
                navig.TagMatriceFormules = GetTagInArbre(tree, folder, "OPT");
                navig.MatriceGaranties = ExisteInArbre(tree, folder, "OPT");
                navig.TagMatriceGaranties = GetTagInArbre(tree, folder, "OPT");
                navig.Risques = GetRisquesHierarchy(tree, folder, modeNavig);
                navig.Engagement = ExisteInArbre(tree, folder, "ENG");
                navig.TagEngagement = GetTagInArbre(tree, folder, "ENG");
                navig.Evenement = ExisteInArbre(tree, folder, "EVN");
                navig.TagEvenement = GetTagInArbre(tree, folder, "EVN");
                navig.MontantRef = ExisteInArbre(tree, folder, ContextStepName.EditionMontantsReference.AsCode());
                navig.TagMontantRef = GetTagInArbre(tree, folder, ContextStepName.EditionMontantsReference.AsCode());
                navig.Attentat = ExisteInArbre(tree, folder, ContextStepName.Attentat.AsCode());
                navig.TagAttentat = GetTagInArbre(tree, folder, ContextStepName.Attentat.AsCode());
                navig.Cotisation = ExisteInArbre(tree, folder, ContextStepName.EditionQuittance.AsCode());
                navig.TagCotisation = GetTagInArbre(tree, folder, ContextStepName.EditionQuittance.AsCode());
                navig.Fin = ExisteInArbre(tree, folder, "FIN");
                navig.TagFin = GetTagInArbre(tree, folder, "FIN");
                navig.CoAssureurs = this.folderService.AllowCoAssureur(folder, modeNavig);
                navig.CoCourtiers = this.courtierRepository.GetNbCourtrier(folder, modeNavig) > 0;
            }

            return navig;
        }
    }
}
