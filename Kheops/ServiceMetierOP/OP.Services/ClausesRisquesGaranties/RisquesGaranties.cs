using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.DataAccess;
using OP.IOWebService.BLServices;
using OP.Services.BLServices;
using OP.Services.SaisieCreationOffre;
using OP.Services.WSKheoBridge;
using OP.WSAS400.DTO.ChoixClauses;
using OP.WSAS400.DTO.Clause;
using OP.WSAS400.DTO.Clausier;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Concerts.Clause;
using OP.WSAS400.DTO.Condition;
using OP.WSAS400.DTO.Ecran.ConditionRisqueGarantie;
using OP.WSAS400.DTO.Ecran.DetailsInventaire;
using OP.WSAS400.DTO.Ecran.DetailsObjetRisque;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.MatriceFormule;
using OP.WSAS400.DTO.MatriceGarantie;
using OP.WSAS400.DTO.MatriceRisque;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.RefExprComplexe;
using OPServiceContract.IClausesRisquesGaranties;
using OP.WSAS400.DTO.LibelleClauses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EasycomClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Activation;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Domain.Affaire;
using OP.WSAS400.DTO.Engagement;

namespace OP.Services.ClausesRisquesGaranties {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RisquesGaranties : IRisquesGaranties
    {
        #region Méthodes transverses
        public OffreDto OffreGetDto(string id, int version, string type)
        {
            return new PoliceServices().OffreGetDto(id, version, type, ModeConsultation.Standard);
        }

        public decimal GetSumPrimeGarantie(string codeAff, long codeFromule)
        {
            return FormuleRepository.GetSumPrimeGarantie(codeAff, codeFromule);
        }
        public List<FormuleDto> GetFormuleIdByRsq(string codeOffre, int numRsq)
        {
            return FormuleRepository.GetFormuleIdByRsq(codeOffre, numRsq);
        }
        public List<FormuleDto> GetIdGar(string codeOffre,long codeFormule)
        {
            return FormuleRepository.GetIdGar(codeOffre,codeFormule);
        }
        public void UpdateKpgartar(long id, decimal? PrimeGaranties)
        {
            FormuleRepository.UpdateKpgartar(id, PrimeGaranties);
        }
        
        public int GetNewInventaireId(string codeOffre, string version, string type, string perimetre, string codeRsq, string codeObj, string codeFor, string codeGaran)
        {
            return BLInventaire.GetNewInventaireId(codeOffre, version, type, perimetre, codeRsq, codeObj, codeFor, codeGaran);
        }

        public string GetInfoInventGarantie(string idGarantie, string codeOffre, int version, string type, int codeFormule, int codeOption, ModeConsultation modeNavig, FormuleGarantieSaveDto formulesGarantiesSave, string user, string codeObjetRisque, string codeAvenant)
        {
            var idGar = BLCommon.InitFormuleIfNotExistsForGar(codeOffre, version.ToString(), type, codeFormule.ToString(), codeOption.ToString(), codeAvenant, codeObjetRisque, user, idGarantie, formulesGarantiesSave);
            return FormuleRepository.GetInfoInventGarantie(idGarantie, codeOffre, version, type, codeFormule, codeOption, modeNavig);
        }

        #endregion

        #region Condition
        /// <summary>
        /// ObtenirFullConditions
        /// Test de parallélisation des opérations
        /// </summary>
        /// <param name="query"></param>
        /// <param name="codeAvn"></param>
        /// <param name="modeNavig"></param>
        /// <param name="isReadOnly"></param>
        /// <param name="loadFormule"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public ConditionRisqueGarantieGetResultDto ObtenirFullConditions(
            ConditionRisqueGarantieGetQueryDto query,
            string codeAvn, ModeConsultation modeNavig,
            bool isReadOnly,
            bool loadFormule,
            string user
        )
        {
            const string Query = @"SELECT COUNT(*) NBLIGN FROM KPGARAN WHERE KDEIPB = :ipb AND KDEALX = :alx AND KDETYP = :typ AND KDEFOR = :formule AND KDEOPT = :option";
            var parameters = new List<EacParameter> {
                new EacParameter("ipb", DbType.AnsiStringFixedLength, 9) { Value = query.NumeroOffre.PadLeft(9, ' ') },
                new EacParameter("alx", DbType.Int32) { Value = query.version.IsEmptyOrNull() ? 0 : int.Parse(query.version) },
                new EacParameter("typ", DbType.AnsiStringFixedLength, 1) { Value = query.Type },
                new EacParameter("formule", DbType.Int32) { Value = int.Parse(query.CodeFormule) },
                new EacParameter("option", DbType.Int32) { Value = int.Parse(query.CodeOption) }
            };
            int numAvn = int.TryParse(codeAvn, out int a) ? a : 0;
            if (!CommonRepository.ExistRow(Query, parameters.ToArray()) && !isReadOnly && modeNavig != ModeConsultation.Historique)
            {
                var formuleGarantie = InitFormuleGarantie(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, "0", modeNavig, isReadOnly, user);
                if (!isReadOnly)
                {
                    SaveFormuleFromCondition(query.NumeroOffre, query.version, query.Type, query.CodeFormule, query.CodeOption, formuleGarantie.Libelle);
                    NavigationArbreRepository.SetTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
                    {
                        CodeOffre = query.NumeroOffre.PadLeft(9, ' '),
                        Version = Convert.ToInt32(query.version),
                        Type = query.Type,
                        EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                        NumeroOrdreDansEtape = 50,
                        NumeroOrdreEtape = 1,
                        Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option),
                        Risque = Convert.ToInt32(formuleGarantie.ObjetRisqueCode.Split(';')[0]),
                        Objet = 0,
                        IdInventaire = 0,
                        Formule = Convert.ToInt32(query.CodeFormule),
                        Option = Convert.ToInt32(query.CodeOption),
                        Niveau = string.Empty,
                        CreationUser = user,
                        PassageTag = "O",
                        PassageTagClause = "N"
                    });
                }
            }

            var result = new ConditionRisqueGarantieGetResultDto();

            if (!string.IsNullOrEmpty(query.NumeroOffre) && !string.IsNullOrEmpty(query.version))
            {
                string rsqobjApplique = ConditionRepository.ObtenirRisqueObjetFormule(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig);
                if (!string.IsNullOrEmpty(rsqobjApplique))
                {
                    string codeRisque = string.Empty;
                    if (!string.IsNullOrEmpty(rsqobjApplique) && rsqobjApplique.Split(';').Length > 0)
                    {
                        codeRisque = rsqobjApplique.Split(';')[0];
                    }
                    BrancheDto branche = CommonRepository.GetBrancheCibleFormule(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, modeNavig);
                    ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = -1 }; // 1 to disable threads, -1 to enable them
                    result.CodeBranche = branche.Code;
                    result.CodeCible = branche.Cible;
                    Parallel.Invoke(parallelOptions,
                        () =>
                        {
                            result.Formule = ConditionRepository.GetLibelleFormule(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, modeNavig);
                            //Perf conditions de garantie
                            result.AppliqueA = ConditionRepository.GetAppliqueA(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig);
                            List<ParametreDto> lstGaranties = new List<ParametreDto>();
                            lstGaranties.Add(new ParametreDto { Code = "", Libelle = "Toutes" });
                            lstGaranties.Add(new ParametreDto { Code = "M", Libelle = "Modifiables" });
                            result.Garanties = lstGaranties;
                            result.Niveaux = ConditionRepository.GetFiltreCondition(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, "N", modeNavig);
                            result.VoletsBlocs = ConditionRepository.GetFiltreCondition(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, "V", modeNavig);

                            result.UnitesLCI = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNLCI");
                            result.TypesLCI = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BALCI");

                            result.UnitesFranchise = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNFRA");
                            result.TypesFranchise = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BAFRA");
                            result.IsAvnDisabled = false;
                            if (!isReadOnly && modeNavig != ModeConsultation.Historique && numAvn > 0)
                            {
                                var x = ConditionRepository.GetOptionStateAvn(query.NumeroOffre, int.Parse(query.version), int.Parse(query.CodeOption), int.Parse(query.CodeFormule));
                                result.IsAvnDisabled = x.Value.avnOptCreate != numAvn && x.Value.avnOptModif != numAvn;
                            }
                        },
                        () =>
                        {
                            var lciGenerale = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.LCI, AlbConstantesMetiers.TypeAppel.Generale, modeNavig);
                            if (lciGenerale != null)
                            {
                                result.LCI = lciGenerale.Valeur.ToString("N", CultureInfo.GetCultureInfo("fr-FR"));
                                result.UniteLCI = lciGenerale.Unite;
                                result.TypeLCI = lciGenerale.Type;
                                result.IsIndexeLCI = lciGenerale.IsIndexe == "O" ? true : false; ;
                                result.LienComplexeLCIGenerale = lciGenerale.LienComplexe.ToString();
                                result.LibComplexeLCIGenerale = lciGenerale.LibComplexe;
                                result.CodeComplexeLCIGenerale = lciGenerale.CodeComplexe;
                            }
                        },
                        () =>
                        {
                            var franchiseGenerale = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.Franchise, AlbConstantesMetiers.TypeAppel.Generale, modeNavig);
                            if (franchiseGenerale != null)
                            {
                                result.Franchise = franchiseGenerale.Valeur.ToString("N", CultureInfo.GetCultureInfo("fr-FR"));
                                result.UniteFranchise = franchiseGenerale.Unite;
                                result.TypeFranchise = franchiseGenerale.Type;
                                result.IsIndexeFranchise = franchiseGenerale.IsIndexe == "O" ? true : false;
                                result.LienComplexeFranchiseGenerale = franchiseGenerale.LienComplexe.ToString();
                                result.LibComplexeFranchiseGenerale = franchiseGenerale.LibComplexe;
                                result.CodeComplexeFranchiseGenerale = franchiseGenerale.CodeComplexe;
                            }
                        },
                        () =>
                        {
                            var lciRisque = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.LCI, AlbConstantesMetiers.TypeAppel.Risque, modeNavig);
                            if (lciRisque != null)
                            {
                                result.LCIRisque = lciRisque.Valeur.ToString("N", CultureInfo.GetCultureInfo("fr-FR"));
                                result.UniteLCIRisque = lciRisque.Unite;
                                result.TypeLCIRisque = lciRisque.Type;
                                result.LienComplexeLCIRisque = lciRisque.LienComplexe.ToString();
                                result.LibComplexeLCIRisque = lciRisque.LibComplexe;
                                result.CodeComplexeLCIRisque = lciRisque.CodeComplexe;

                            }
                        },
                        () =>
                        {
                            var franchiseRisque = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.Franchise, AlbConstantesMetiers.TypeAppel.Risque, modeNavig);
                            if (franchiseRisque != null)
                            {
                                result.FranchiseRisque = franchiseRisque.Valeur.ToString("N", CultureInfo.GetCultureInfo("fr-FR"));
                                result.UniteFranchiseRisque = franchiseRisque.Unite;
                                result.TypeFranchiseRisque = franchiseRisque.Type;
                                result.LienComplexeFranchiseRisque = franchiseRisque.LienComplexe.ToString();
                                result.LibComplexeFranchiseRisque = franchiseRisque.LibComplexe;
                                result.CodeComplexeFranchiseRisque = franchiseRisque.CodeComplexe;
                            }
                        },
                        () =>
                        {
                            result.ExpAssiette = ConditionRepository.AssietteGet(query.NumeroOffre, query.version);

                            result.AssietteUnites = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNCAP");
                            result.AssietteTypes = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BACAP");
                            result.TauxForfaitHTUnites = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNPRI");

                            result.LstRisque = ConditionRepository.GetListRisque(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig);

                            //Perf conditions de garantie
                            result.LstEnsembleLigne = ConditionRepository.LstEnsembleLigne(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig, isReadOnly);

                        });


                    result.LstEnsembleLigne.ForEach(elem =>
                    {
                        elem.LCIUnites = result.UnitesLCI;
                        elem.LCITypes = result.TypesLCI;
                        elem.FranchiseUnites = result.UnitesFranchise;
                        elem.FranchiseTypes = result.TypesFranchise;
                        elem.AssietteUnites = result.AssietteUnites;
                        elem.AssietteTypes = result.AssietteTypes;
                        elem.TauxForfaitHTUnites = result.TauxForfaitHTUnites;
                        elem.ReadOnly = isReadOnly;
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Obtenirs the conditions.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public ConditionRisqueGarantieGetResultDto ObtenirConditions(ConditionRisqueGarantieGetQueryDto query, string codeAvn, ModeConsultation modeNavig, bool isReadOnly)
        {
            var result = new ConditionRisqueGarantieGetResultDto();

            if (!string.IsNullOrEmpty(query.NumeroOffre) && !string.IsNullOrEmpty(query.version))
            {
                string rsqobjApplique = ConditionRepository.ObtenirRisqueObjetFormule(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig);
                if (!string.IsNullOrEmpty(rsqobjApplique))
                {
                    string codeRisque = string.Empty;
                    if (!string.IsNullOrEmpty(rsqobjApplique) && rsqobjApplique.Split(';').Length > 0)
                        codeRisque = rsqobjApplique.Split(';')[0];

                    BrancheDto branche = CommonRepository.GetBrancheCibleFormule(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, modeNavig);

                    result.Formule = ConditionRepository.GetLibelleFormule(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, modeNavig);
                    result.AppliqueA = ConditionRepository.GetAppliqueA(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig);
                    List<ParametreDto> lstGaranties = new List<ParametreDto>();
                    lstGaranties.Add(new ParametreDto { Code = "", Libelle = "Toutes" });
                    lstGaranties.Add(new ParametreDto { Code = "M", Libelle = "Modifiables" });
                    result.Garanties = lstGaranties;
                    result.Niveaux = ConditionRepository.GetFiltreCondition(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, "N", modeNavig);
                    result.VoletsBlocs = ConditionRepository.GetFiltreCondition(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, "V", modeNavig);

                    result.UnitesLCI = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNLCI");
                    result.TypesLCI = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BALCI");

                    result.UnitesFranchise = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNFRA");
                    result.TypesFranchise = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BAFRA");

                    var lciGenerale = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.LCI, AlbConstantesMetiers.TypeAppel.Generale, modeNavig);
                    if (lciGenerale != null)
                    {
                        result.LCI = lciGenerale.Valeur.ToString();
                        result.UniteLCI = lciGenerale.Unite;
                        result.TypeLCI = lciGenerale.Type;
                        result.IsIndexeLCI = lciGenerale.IsIndexe == "O" ? true : false; ;
                        result.LienComplexeLCIGenerale = lciGenerale.LienComplexe.ToString();
                        result.LibComplexeLCIGenerale = lciGenerale.LibComplexe;
                        result.CodeComplexeLCIGenerale = lciGenerale.CodeComplexe;
                    }

                    var franchiseGenerale = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.Franchise, AlbConstantesMetiers.TypeAppel.Generale, modeNavig);
                    if (franchiseGenerale != null)
                    {
                        result.Franchise = franchiseGenerale.Valeur.ToString();
                        result.UniteFranchise = franchiseGenerale.Unite;
                        result.TypeFranchise = franchiseGenerale.Type;
                        result.IsIndexeFranchise = franchiseGenerale.IsIndexe == "O" ? true : false;
                        result.LienComplexeFranchiseGenerale = franchiseGenerale.LienComplexe.ToString();
                        result.LibComplexeFranchiseGenerale = franchiseGenerale.LibComplexe;
                        result.CodeComplexeFranchiseGenerale = franchiseGenerale.CodeComplexe;
                    }

                    var lciRisque = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.LCI, AlbConstantesMetiers.TypeAppel.Risque, modeNavig);
                    if (lciRisque != null)
                    {
                        result.LCIRisque = lciRisque.Valeur.ToString();
                        result.UniteLCIRisque = lciRisque.Unite;
                        result.TypeLCIRisque = lciRisque.Type;
                        result.LienComplexeLCIRisque = lciRisque.LienComplexe.ToString();
                        result.LibComplexeLCIRisque = lciRisque.LibComplexe;
                        result.CodeComplexeLCIRisque = lciRisque.CodeComplexe;

                    }

                    var franchiseRisque = ConditionRepository.GetLCIFranchise(query.NumeroOffre, query.version, query.Type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.Franchise, AlbConstantesMetiers.TypeAppel.Risque, modeNavig);
                    if (franchiseRisque != null)
                    {
                        result.FranchiseRisque = franchiseRisque.Valeur.ToString();
                        result.UniteFranchiseRisque = franchiseRisque.Unite;
                        result.TypeFranchiseRisque = franchiseRisque.Type;
                        result.LienComplexeFranchiseRisque = franchiseRisque.LienComplexe.ToString();
                        result.LibComplexeFranchiseRisque = franchiseRisque.LibComplexe;
                        result.CodeComplexeFranchiseRisque = franchiseRisque.CodeComplexe;
                    }
                    result.ExpAssiette = ConditionRepository.AssietteGet(query.NumeroOffre, query.version);

                    result.AssietteUnites = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNCAP");
                    result.AssietteTypes = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BACAP");
                    result.TauxForfaitHTUnites = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNPRI");

                    result.LstRisque = ConditionRepository.GetListRisque(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig);

                    result.LstEnsembleLigne = ConditionRepository.LstEnsembleLigne(query.NumeroOffre, query.version, query.Type, codeAvn, query.CodeFormule, query.CodeOption, modeNavig, isReadOnly);

                    result.LstEnsembleLigne.ForEach(elem =>
                    {
                        elem.LCIUnites = result.UnitesLCI;

                        elem.LCITypes = result.TypesLCI;
                        elem.FranchiseUnites = result.UnitesFranchise;
                        elem.FranchiseTypes = result.TypesFranchise;
                        elem.AssietteUnites = result.AssietteUnites;
                        elem.AssietteTypes = result.AssietteTypes;
                        elem.TauxForfaitHTUnites = result.TauxForfaitHTUnites;
                        elem.ReadOnly = isReadOnly;
                    });
                }
            }

            return result;
        }

        public bool DeleteCondition(string codeCondition)
        {

            return ConditionRepository.DeleteCondition(codeCondition);

        }

        /// <summary>
        /// Sauvegarde la ligne de condition
        /// </summary>
        /// <param name="conditionDto">Ligne de condition.</param>
        /// <returns></returns>
        public int SaveCondition(LigneGarantieDto conditionDto, string codeAvn)
        {
            if (conditionDto != null)
                return ConditionRepository.SaveCondition(conditionDto, codeAvn);
            return 0;
        }

        /// <summary>
        /// Recharge la ligne de condition après l'annulation
        /// </summary>
        public EnsembleGarantieDto CancelGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, string codeCondition, string oldFRHExpr, string oldLCIExpr, ModeConsultation modeNavig, bool isReadOnly)
        {
            ConditionRepository.ResetExprCompCondition(codeCondition, oldFRHExpr, oldLCIExpr);
            var lstCondition = ConditionRepository.LstEnsembleLigne(codeOffre, version, type, codeAvn, codeFormule, codeOption, modeNavig, isReadOnly, codeCondition);
            return lstCondition != null && lstCondition.Any() ? lstCondition.FirstOrDefault() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argCodeOffre"></param>
        /// <param name="argType"></param>
        /// <returns></returns>
        public ConditionComplexeDto RecuperationConditionComplexe(string argCodeOffre, string argVersion, string argTypeOffre, string argType)
        {
            ConditionComplexeDto condition = null;
            if (!string.IsNullOrEmpty(argCodeOffre) && !string.IsNullOrEmpty(argType))
            {
                condition = new ConditionComplexeDto
                {
                    Type = argType,
                    Expressions = ConditionRepository.RecupConditionComplexe(
                                               argCodeOffre, argVersion, argTypeOffre, argType)
                };
            }

            return condition;
        }

        /// <summary>
        /// Recuperations the detail complexe.
        /// </summary>
        /// <param name="codeOffre">The arg code offre.</param>
        /// <param name="codeExpr">The arg code.</param>
        /// <param name="typeExpr">Type of the arg.</param>
        /// <returns></returns>
        public ConditionComplexeDto RecuperationDetailComplexe(string codeOffre, string version, string type, string codeAvn, string codeExpr, string typeExpr, ModeConsultation modeNavig)
        {
            ConditionComplexeDto condition = null;

            if (!string.IsNullOrEmpty(codeOffre) && !string.IsNullOrEmpty(typeExpr))
            {
                condition = ConditionRepository.RecupDetailComplexe(codeOffre, version, type, codeExpr, typeExpr);
                condition.Type = typeExpr;
                BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);
                if (typeExpr == "LCI")
                {
                    condition.UnitesNew = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNLCI");
                    condition.TypesNew = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BALCI");
                }
                else if (typeExpr == "Franchise")
                {
                    condition.UnitesNew = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "UNFRA");
                    condition.TypesNew = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, "ALSPK", "BAFRA");
                }
            }

            return condition;
        }

        /// <summary>
        /// Affects the expression condition.
        /// </summary>
        /// <param name="argType">Type of the arg.</param>
        /// <param name="argCodeCondition">The arg code condition.</param>
        /// <param name="argCodeExpression">The arg code expression.</param>
        public void AffectExpressionCondition(string argType, string argCodeCondition, string argCodeExpression)
        {


            ConditionRepository.EnregistrementExpCompPourCond(argType, argCodeCondition, argCodeExpression);

        }

        /// <summary>
        /// Enregistrements the condition complexe.
        /// </summary>
        /// <param name="argExpComp">The arg exp comp.</param>
        /// <param name="argTypeOffre">The arg type offre.</param>
        /// <param name="argType">Type of the arg.</param>
        /// <param name="argCodeOffre">The arg code offre.</param>
        /// <param name="argVersion">The arg version.</param>
        /// <param name="argIdExpression">The arg id expression.</param>
        /// <param name="argLibelle">The arg libelle.</param>
        /// <param name="argDescriptif">The arg descriptif.</param>
        public string EnregistrementConditionComplexe(ConditionComplexeDto argExpComp, string argTypeOffre, string argType, string argCodeOffre, string argVersion, int? argIdExpression, string argLibelle, string argDescriptif)
        {
            return ConditionRepository.EnregistreExpressionDetail(argExpComp, argTypeOffre, argType, argCodeOffre, argVersion, argIdExpression, argLibelle, argDescriptif);
        }

        /// <summary>
        /// Supressions the detail.
        /// </summary>
        /// <param name="argType">Type of the arg.</param>
        /// <param name="argId">The arg id.</param>
        public void SupressionDetail(string argType, string argId)
        {


            if (argType == "LCI")
            {
                ConditionRepository.SuppressionDetailLCI(argId);
            }
            else
            {
                ConditionRepository.SuppressionDetailFranchise(argId);
            }

        }

        /// <summary>
        /// Suppressions the expression.
        /// </summary>
        /// <param name="argType">Type of the arg.</param>
        /// <param name="argIdExpression">The arg id expression.</param>
        /// <param name="argIdCondition">The arg id condition.</param>
        public void SuppressionExpression(string argType, string argTypeAppel, string argIdExpression, string argIdCondition)
        {

            //AccesDataManager._connectionHelper = easyComConnectionHelper;

            ConditionRepository.SuppressionExpression(argType, argTypeAppel, argIdExpression, argIdCondition);

        }

        public List<ParametreDto> GetListeFiltre(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption,
            string typeFiltre, string garantie, string voletbloc, string niveau, ModeConsultation modeNavig)
        {
            string[] codeVoletBloc = voletbloc.Split('_');
            string codeVolet = codeVoletBloc[0];
            string codeBloc = (codeVoletBloc.Length > 1 ? codeVoletBloc[1] : "0");
            return ConditionRepository.GetFiltreCondition(codeOffre, version, type, codeAvn, codeFormule, codeOption, typeFiltre, modeNavig, garantie, codeVolet, codeBloc, niveau);
        }

        public string GetFormuleGarantieBranche(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            return FormuleRepository.GetFormuleGarantieBranche(codeOffre, version, type, codeFormule, codeOption);
        }
        public List<EngagementPeriodeDto> IsInHpeng(string codeOffre)
        {
            return ConditionRepository.IsInHpeng(codeOffre);
        }

        #endregion

        #region Garantie Modele

        /// <summary>
        /// Garanties the modeles get by bloc.
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public List<GarantieModeleDto> GarantieModelesGetByBloc(string codeId)
        {
            return new Administration.Administration(null).GarantieModelesGetByBloc(codeId);
        }

        /// <summary>
        /// Garanties the modele get.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public List<GarantieModeleDto> GarantieModeleGet(string code, string description)
        {
            List<GarantieModeleDto> garantieModeleDto = null;

            //AccesDataManager._connectionHelper = easyComConnectionHelper;

            garantieModeleDto = GarantieModeleRepository.RechercherGarantieModele(code, description);

            return garantieModeleDto;
        }

        /// <summary>
        /// Modeleses the get list.
        /// </summary>
        /// <returns></returns>
        public List<GarantieModeleDto> ModelesGetList()
        {
            return new Administration.Administration(null).ModelesGetList();
        }

        /// <summary>
        /// Enregistrers the modele by categorie.
        /// </summary>
        /// <param name="codeId">The code id.</param>
        /// <param name="codeIdBloc">The code id bloc.</param>
        /// <param name="dateApp">The date app.</param>
        /// <param name="codeTypo">The code typo.</param>
        /// <param name="codeModele">The code modele.</param>
        /// <param name="user">The user.</param>
        //public void EnregistrerModeleByCategorie(string codeId, string codeIdBloc, string dateApp, string codeTypo, string codeModele, string user)
        //{
        //    new Administration.Administration().EnregistrerModeleByCategorie(codeId, codeIdBloc, dateApp, codeTypo,
        //                                                                     codeModele, user);
        //}

        /// <summary>
        /// Supprimers the modele by categorie.
        /// </summary>
        /// <param name="codeId">The code id.</param>
        public void SupprimerModeleByCategorie(string codeId, string infoUser)
        {
            new Administration.Administration(null).SupprimerModeleByCategorie(codeId, infoUser);
        }

        /// <summary>
        /// Garanties the modele info get.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public GarantieModeleDto GarantieModeleInfoGet(string code)
        {
            return GarantieModeleRepository.GetGarantieModele(code);
        }

        /// <summary>
        /// Enregistrer the garantie modele.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="codeOffre"> </param>
        /// <param name="version"> </param>
        public void EnregistrerGarantieModele(string code, string description, bool isNew, out string msgRetour)
        {
            msgRetour = "";
            if (isNew)
            {
                if (!GarantieModeleRepository.ExistCodeModele(code))
                {
                    GarantieModeleRepository.InsertGarantieModele(code, description);
                }
                else
                {
                    msgRetour = string.Format(@"Le code {0} est déjà utilisé par un autre modèle de garantie", code);
                }
            }
            else
            {
                GarantieModeleRepository.UpdateGarantieModele(code, description);
            }
        }

        /// <summary>
        /// Copier the garantie modele.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="codeOffre"> </param>
        /// <param name="version"> </param>
        public void CopierGarantieModele(string code, string codeCopie)
        {
            GarantieModeleRepository.CopierGarantieModele(code, codeCopie);
        }

        /// <summary>
        /// Supprimers the garantie modele.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="codeOffre"> </param>
        /// <param name="version"> </param>
        public void SupprimerGarantieModele(string code, out string msgRetour)
        {
            GarantieModeleRepository.SupprimerGarantieModele(code, out msgRetour);
        }

        /// <summary>
        /// Vérifie si la garantie modele est utilisé dans un contrat.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="codeOffre"> </param>
        /// <param name="version"> </param>
        public bool ExistGarantieModeleDansContrat(string code)
        {
            return GarantieModeleRepository.ExistDansContrat(code);
        }

        #endregion

        #region Formule
        public List<ParamNatGarDto> GetParamNatGar()
        {
            return FormuleRepository.GetParamNatGar().ToList();
        }

        public List<ParametreDto> ObtenirTypesInventaire()
        {
            return ReferenceRepository.ObtenirTypeInventaire();
        }

        public List<RisqueObjetPlatDto> GetEarliestRisqueObjetFormule(string codeOffre, string version, string type)
        {
            return FormuleRepository.GetEarliestRisqueObjetFormule(codeOffre, version, type);
        }

        public List<RisqueObjetPlatDto> GetRisqueObjetFormule(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            return FormuleRepository.GetRisqueObjetFormule(codeOffre, version, type, codeAvn, codeFormule, codeOption, modeNavig);
        }

        public string ObtenirFormuleOptionByOffre(string codeOffre, string version, string codeAvn, ModeConsultation modeNavig)
        {
            return FormuleRepository.ObtenirFormuleOptionByOffre(codeOffre, version, codeAvn, modeNavig);
        }

        public FormuleDto InitFormuleGarantie(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption, string formGen, ModeConsultation modeNavig, bool readOnly, string user)
        {
            var toReturn = FormuleRepository.InitFormuleGarantie(codeOffre, version, type, avenant, codeFormule, codeOption, formGen, modeNavig, readOnly, user);

            return toReturn;
        }

        public string FormulesGarantiesSaveSet(string codeOffre, string version, string type, string codeAvenant, string dateAvenant, string codeFormule, string codeOption, string formGen, string libelle, string codeObjetRisque, FormuleGarantieSaveDto formuleGarantie, string user)
        {
            return BLCommon.FormulesGarantiesSet(codeOffre, version, type, codeAvenant, dateAvenant, codeFormule, codeOption, formGen, libelle, formuleGarantie, codeObjetRisque, user);
        }

        //TODO AMO/SAB
        public void RegenerateCanevas(string user, bool totalRegeneration)
        {
            FormuleRepository.RegenerateCanevas(user, totalRegeneration);
        }

        public FormGarDto FormulesGarantiesGet(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string formGen, string codeCategorie, string codeAlpha, string branche, string libFormule, string user, int appliqueA, bool isReadOnly, ModeConsultation modeNavig)
        {
            return FormuleRepository.FormulesGarantiesGet(codeOffre, Convert.ToInt32(version), type, codeAvn, Convert.ToInt32(codeFormule), Convert.ToInt32(codeOption), Convert.ToInt32(formGen), Convert.ToInt32(codeCategorie), codeAlpha, branche, libFormule, user, appliqueA, isReadOnly, modeNavig);
        }

        public void DeleteFormuleGarantieHisto(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            FormuleRepository.DeleteFormuleGarantieHistorique(codeOffre, version, type, codeFormule, codeOption);
        }

        public string EnregistrerFormuleGarantie(string codeOffre, string version, string type, string codeAvt, string codeFormule, string codeOption, string formGen, string codeAlpha, string branche, string codeCible, string libFormule, string codeObjetRisque, string user)
        {
            return FormuleRepository.EnregistrerFormuleGarantie(codeOffre, version, type, codeAvt, codeFormule, codeOption, formGen, codeAlpha, branche, codeCible, libFormule, codeObjetRisque, user);
        }


        public FormuleGarantieDetailsDto ObtenirInfosDetailsFormuleGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string id, string codeObjetRisque, FormuleGarantieSaveDto volets, DateTime? dateEffAvnModifLocale, string user, bool isReadonly, ModeConsultation modeNavig)
        {
            return BLCommon.ObtenirGarantieDetails(codeOffre, version, type, codeAvn, codeFormule, codeOption, id, codeObjetRisque, volets, dateEffAvnModifLocale, user, isReadonly, modeNavig);
        }

        public List<GarantiePeriodeDto> GetDateDebByGaranties(int[] ids, int codeAvn)
        {
            return FormuleRepository.GetDateDebByGaranties(ids, codeAvn);
        }

        public FormuleGarantiePorteeDto ObtenirInfosPorteeFormuleGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, ModeConsultation modeNavig, string alimAssiette, string branche, string cible, string modifAvn, string codeObjetRisque)
        {
            return FormuleRepository.ObtenirGarantiePortee(codeOffre, version, type, codeAvn, codeFormule, codeOption, codeGarantie, modeNavig, alimAssiette, branche, cible, codeObjetRisque, modifAvn);
        }

        public void SavePorteeGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string idGarantie, string codeGarantie, string nature, string codeRsq, string codesObj, string codeObjetRisque, FormuleGarantieSaveDto formulesGarantiesSave, string user, RisqueDto rsq, string alimAssiette, bool reportCal)
        {
            var idGar = BLCommon.InitFormuleIfNotExistsForGar(codeOffre, version, type, codeFormule, codeOption, codeAvn, codeObjetRisque, user, idGarantie, formulesGarantiesSave);
            BLFormuleGarantie.SavePorteeGarantie(codeOffre, version, type, codeAvn, codeFormule, codeOption, idGar.ToString(CultureInfo.InvariantCulture), codeGarantie, nature, codeRsq, codesObj, user, rsq, alimAssiette, reportCal);
        }

        public string SauverDetailsFormuleGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, string codeAvenant, string codeObjetRisque, string user, FormuleGarantieDetailsDto garantieDetails, FormuleGarantieSaveDto formulesGarantiesSave)
        {
            var idGar = BLCommon.InitFormuleIfNotExistsForGar(codeOffre, version, type, codeFormule, codeOption, codeAvenant, codeObjetRisque, user, garantieDetails.CodeGarantie, formulesGarantiesSave);

            garantieDetails.CodeGarantie = idGar.ToString(CultureInfo.InvariantCulture);
            if (FormuleRepository.CheckDatesGarantie(garantieDetails))
                return FormuleRepository.SaveDetailsGarantie(garantieDetails, codeAvenant);

            return "ERREUR_DATE";
        }

        public List<RisqueDto> GetFormuleRisquesApplicables(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            return BLFormuleGarantie.GetFormuleRisquesApplicables(codeOffre, version, type, avenant, codeFormule, codeOption, modeNavig);
        }

        public string GetLettreLibFormuleGarantie(IdContratDto contrat)
        {
            return FormuleRepository.GetLettreLibFormuleGarantie(contrat.CodeOffre, contrat.Version.ToString(), contrat.Type);
        }

        public FormuleDto GetFormuleGarantieInfo(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            var formule = codeFormule.IsEmptyOrNull() || codeFormule == "0" ?
                new FormuleDto()
                : FormuleRepository.GetFormuleGarantieInfo(codeOffre, version, type, codeAvn, codeFormule, modeNavig);

            formule.ObjetRisqueCode = FormuleRepository.GetRisqueObjetFormuleString(codeOffre, version, type, codeAvn, codeFormule, codeOption, modeNavig);

            if (formule.ObjetRisqueCode.IsEmptyOrNull())
            {
                formule.ObjetRisqueCode = FormuleRepository.GetFirstRsqObj(codeOffre, version, modeNavig);
            }

            if (formule.ObjetRisqueCode.Split(';').Length <= 1)
            {
                formule.ObjetRisqueCode = FormuleRepository.GetAllRsqObj(codeOffre, version, formule.ObjetRisqueCode, modeNavig);
            }

            formule.OffreAppliqueA = FormuleRepository.IsOffreAppliqueA(
                new IdContratDto
                {
                    CodeOffre = codeOffre,
                    Version = int.Parse(version),
                    Type = type
                },
                int.Parse(codeAvn),
                modeNavig == ModeConsultation.Historique);

            return formule;
        }

        private void InitFormuleIfNotExistsForOption(string codeOffre, string version, string type, string codeFormule, string codeOption,
                                                      string codeAvenant, string codeObjetRisque, string user, long guidVolet, long guidBloc)
        {

            var brancheCible = CommonRepository.GetBrancheCibleFormule(codeOffre, version, type, codeAvenant, codeFormule, ModeConsultation.Standard);
            var codeCible = (int)brancheCible.Cible.GuidId;
            var branche = brancheCible.Code;
            var kdbid = FormuleRepository.GetLienKpOpt(codeOffre, version, type, codeFormule, codeOption);
            var dateNow = (long)AlbConvert.ConvertDateToInt(DateTime.Now);

            var codeRisque = codeObjetRisque.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[0];
            var codeObjets =
                codeObjetRisque.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new[] { '_' },
                                                                                                   StringSplitOptions.
                                                                                                       RemoveEmptyEntries).
                    ToList();

            var nbObj = FormuleRepository.GetNbrObj(codeOffre, version, codeRisque);
            var kddid = 0;
            if (nbObj == codeObjets.Count)
            {
                kddid = CommonRepository.GetAS400Id("KDDID");
                FormuleRepository.PopulateKpoptap(codeOffre, version, type, new KpoptapDto
                {
                    KDDID = kddid,
                    KDDKDBID = kdbid,
                    KDDIPB = codeOffre,
                    KDDALX =
                        string.IsNullOrEmpty(version)
                            ? 0
                            : Convert.ToInt32(version),
                    KDDTYP = type,
                    KDDFOR =
                        string.IsNullOrEmpty(codeFormule)
                            ? 0
                            : Convert.ToInt32(codeFormule),
                    KDDOPT =
                        string.IsNullOrEmpty(codeOption)
                            ? 0
                            : Convert.ToInt32(codeOption),
                    KDDRSQ =
                        Convert.ToInt32(
                            codeObjetRisque.Split(';')[0]),
                    KDDOBJ = 0,
                    KDDCRD = dateNow,
                    KDDCRU = user,
                    KDDMAJD = dateNow,
                    KDDMAJU = user,
                    KDDPERI = "RQ",
                    KDDINVEN = 0,
                    KDDINVEP = 0
                });
            }
            else
            {
                codeObjets.ForEach(i =>
                {
                    kddid = CommonRepository.GetAS400Id("KDDID");
                    FormuleRepository.PopulateKpoptap(codeOffre, version, type, new KpoptapDto
                    {
                        KDDID = kddid,
                        KDDKDBID =
                            kdbid,
                        KDDIPB =
                            codeOffre,
                        KDDALX =
                            string.
                                IsNullOrEmpty
                                (version)
                                ? 0
                                : Convert
                                      .
                                      ToInt32
                                      (version),
                        KDDTYP = type,
                        KDDFOR =
                            string.
                                IsNullOrEmpty
                                (codeFormule)
                                ? 0
                                : Convert
                                      .
                                      ToInt32
                                      (codeFormule),
                        KDDOPT =
                            string.
                                IsNullOrEmpty
                                (codeOption)
                                ? 0
                                : Convert
                                      .
                                      ToInt32
                                      (codeOption),
                        KDDRSQ =
                            Convert.
                            ToInt32(
                                codeObjetRisque
                                    .
                                   Split
                                    (';')
                                    [
                                        0
                                   ]),
                        KDDOBJ =
                            Convert.
                            ToInt32(i),
                        KDDCRD =
                            dateNow,
                        KDDCRU = user,
                        KDDMAJD =
                            dateNow,
                        KDDMAJU =
                            user,
                        KDDPERI =
                            "OB",
                        KDDINVEN = 0,
                        KDDINVEP = 0
                    });
                });
            }


            var paramsVoletBlocs = FormuleRepository.LoadVoletBlocParameters(codeOffre, Convert.ToInt32(version), type, codeCible, branche);

            var v = paramsVoletBlocs.FirstOrDefault(i => i.Guidv == guidVolet);
            var b = paramsVoletBlocs.FirstOrDefault(i => i.Guidb == guidBloc);


            var idVolet = FormuleRepository.PopulateKpoptd(codeOffre, Convert.ToInt32(version), type, new KpoptdDto
            {
                KDCIPB =
                    codeOffre,
                KDCALX =
                    Convert.
                    ToInt32(
                        version),
                KDCTYP = type,
                KDCFOR =
                    Convert.
                    ToInt32(
                        codeFormule),
                KDCOPT =
                    Convert.
                    ToInt32(
                        codeOption),
                KDCFLAG = 1,
                KDCKAEID = 0,
                KDCKAKID =
                    v.
                    GuidVolet,
                KDCKAQID = 0,
                KDCKARID =
                    v.GuidM,
                KDCKDBID =
                    kdbid,
                KDCMAJD =
                    dateNow,
                KDCMAJU = user,
                KDCMODELE =
                    v.
                    LibModele,
                KDCTENG = "V",
                KDCCRD =
                    dateNow,
                KDCCRU = user,
                KDCORDRE = v.VoletOrdre
            }, true);

            if (guidBloc > 0)
            {
                var idBloc = FormuleRepository.PopulateKpoptd(codeOffre, Convert.ToInt32(version), type, new KpoptdDto
                {
                    KDCIPB =
                        codeOffre,
                    KDCALX =
                        Convert.
                        ToInt32(
                            version),
                    KDCTYP = type,
                    KDCFOR =
                        Convert.
                        ToInt32(
                            codeFormule),
                    KDCOPT =
                        Convert.
                        ToInt32(
                            codeOption),
                    KDCFLAG = 1,
                    KDCKAEID =
                        b.GuidBloc,
                    KDCKAKID =
                        b.GuidVolet,
                    KDCKAQID =
                        b.Guidb,
                    KDCKARID =
                        b.GuidM,
                    KDCKDBID =
                        kdbid,
                    KDCMAJD =
                        dateNow,
                    KDCMAJU = user,
                    KDCMODELE =
                        b.LibModele,
                    KDCTENG = "B",
                    KDCCRD =
                        dateNow,
                    KDCCRU = user,
                    KDCORDRE = b.BlocOrdre
                }, true);
            }
        }


        public void SaveAppliqueA(string codeOffre, string version, string type, string codeFormule, string codeOption, string cible, string formGen, string objetRisqueCode, string user)
        {
            FormuleRepository.SaveAppliqueA(codeOffre, version, type, codeFormule, codeOption, cible, formGen, objetRisqueCode, user);
        }

        public GarantieDetailInfoDto GetInfoDetailsGarantie(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption)
        {
            if (!string.IsNullOrEmpty(codeFormule) && !string.IsNullOrEmpty(codeOption))
                return FormuleRepository.GetInfoDetailsGarantie(codeOffre, version, type, codeGarantie, codeFormule, codeOption);

            return FormuleRepository.GetInfoDetailsGarantie(codeOffre, version, type, codeGarantie);
        }

        public string GetLibCible(string codeCible)
        {
            return FormuleRepository.GetLibCible(codeCible);
        }

        public void SaveFormuleFromCondition(string codeOffre, string version, string type, string codeFormule, string codeOption, string libelle)
        {
            FormuleRepository.SaveFormuleFromCondition(codeOffre, version, type, codeFormule, codeOption, libelle);
        }

        public FormuleDto GetCibleInfoFormule(string codeOffre, string version, string type, string codeFormule)
        {
            return FormuleRepository.GetCibleInfoFormule(codeOffre, version, type, codeFormule);
        }

        public void UpdateDateForcee(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption, string codeAvn, string codeObjetRisque, string niveauLib, string guidV, string guidB, string guidG, int? dateModifAvt, bool isChecked, string user, FormuleGarantieSaveDto volets)
        {
            if (string.IsNullOrEmpty(codeGarantie) || codeGarantie == "0")
                InitFormuleIfNotExistsForOption(codeOffre, version, type, codeFormule, codeOption, codeAvn, codeObjetRisque, user, Convert.ToInt64(guidV), Convert.ToInt64(guidB));
            else
                BLCommon.InitFormuleIfNotExistsForGar(codeOffre, version, type, codeFormule, codeOption, codeAvn, codeObjetRisque, user, codeGarantie, volets);

            if (isChecked)
                FormuleRepository.UpdateDateDebForcee(codeOffre, version, type, codeAvn, niveauLib, guidV, guidB, guidG, dateModifAvt, user);
            else
                FormuleRepository.UpdateDateFinForcee(codeAvn, niveauLib, guidV, guidB, guidG, dateModifAvt, user);
        }

        public void InitParfaitAchevement(string codeOffre, string version, string type, string codeGarantie, string codeFormule, string codeOption, string codeAvenant, string codeObjetRisque, string user, FormuleGarantieSaveDto volets)
        {
            BLCommon.InitParfaitAchevement(codeOffre, version, type, codeGarantie, codeFormule, codeOption, codeAvenant, codeObjetRisque, user, volets);
        }

        public bool IsTraceAvnExist(string codeAffaire, string version, string type, string codeFormule, string codeOption)
        {
            return FormuleRepository.IsTraceAvnExist(codeAffaire, version, type, codeFormule, codeOption);
        }

        #endregion

        #region Garantie

        List<GarantieModeleDto> IRisquesGaranties.GarantieModeleGet(string code, string description)
        {
            List<GarantieModeleDto> garantieModeleDto;

            //AccesDataManager._connectionHelper = easyComConnectionHelper;

            garantieModeleDto = GarantieModeleRepository.RechercherGarantieModele(code, description);

            return garantieModeleDto;
        }

        List<GarantieTypeDto> IRisquesGaranties.GarantieTypeGet(string codeModele)
        {
            List<GarantieTypeDto> garantieTypeDto = null;

            //garantieTypeDto = GarantieModeleRepository.RechercherGarantieType(codeModele);

            return garantieTypeDto;
        }

        /// <summary>
        /// Garanties the type info get.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public GarantieTypeDto GarantieTypeInfoGet(int seq)
        {
            return null;// GarantieModeleRepository.GetGarantieType(seq);
        }

        /// <summary>
        /// Lien Garanties info get.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        public GarantieTypeDto GarantieTypeLienInfoGet(int seq)
        {
            return null; // GarantieModeleRepository.GetGarantieTypeLien(seq);
        }

        /// <summary>
        /// Enregistrer the garantie type.
        /// </summary>
        /// <param name="garType">The garantie type.</param>
        public void EnregistrerGarantieType(GarantieTypeDto garType, bool isNew, out string msgRetour)
        {
            msgRetour = "";

            //if (!GarantieModeleRepository.ExistTri(garType.NumeroSeq, garType.CodeModele, garType.Tri))
            //{
            //    if (isNew)
            //    {
            //        if (!GarantieModeleRepository.ExistCodeGarantie(garType.CodeModele, garType.CodeGarantie, garType.NumeroSeqM))
            //        {
            //            GarantieModeleRepository.InsertGarantieType(garType, out msgRetour);
            //        }
            //        else
            //        {
            //            if (garType.Niveau == 1) { msgRetour = string.Format("La garantie {0} est déjà présente dans le modèle", garType.CodeGarantie); }
            //            else { msgRetour = string.Format("La sous-garantie {0} est déjà présente", garType.CodeGarantie); }
            //        }

            //    }
            //    else
            //    {
            //        GarantieModeleRepository.UpdateGarantieType(garType);
            //    }
            //}
            //else
            //{
            //    msgRetour = "Le numéro d'ordre est déjà utilisé par une autre garantie de même niveau";
            //}
        }

        /// <summary>
        /// Supprimers the garantie type.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="codeOffre"> </param>
        /// <param name="version"> </param>
        public void SupprimerGarantieType(int seq, out string msgRetour)
        {
            msgRetour = "";
            //GarantieModeleRepository.SupprimerGarantieType(seq, out msgRetour);
        }

        #endregion

        #region Risque

        //public RisqueDto ObtenirRisqueApplique(string codeOffre, string version, string codeRisque, string codeOption)
        //{

        //    return ObjectMapperManager.DefaultInstance.GetMapper<Risque, RisqueDto>().Map((BLFormuleGarantie.ObtenirInfosRisque(codeOffre, version, codeRisque, codeOption)));
        //}
        //public string ObtenirLibGarantie(string codeGarantie)
        //{


        //    return BLFormuleGarantie.ObtenirLibGarantie(codeGarantie);



        //}
        //public string ObtenirRisqueObjetApplique(string codeOption, string[] perimetres)
        //{
        //    //AccesDataManager._connectionHelper = easyComConnectionHelper;

        //    return BLFormuleGarantie.ObtenirRisqueObjetApplique(codeOption, perimetres);





        // }
        //public int GetNextRisque(OffreDto offre)
        //{
        //    int toReturn;

        //    //AccesDataManager._connectionHelper = easyComConnectionHelper;
        //    toReturn = PoliceRepository.GetNextCodeRisque_YprtRsq(ObjectMapperManager.DefaultInstance.GetMapper<OffreDto, Offre>().Map(offre));

        //    return toReturn;
        //}
        //public List<RisqueDto> ListeRisquesGetByTerm(string codeOffre, string version)
        //{
        //    var toReturn = new List<RisqueDto>();

        //    //AccesDataManager._connectionHelper = easyComConnectionHelper;
        //    toReturn = ObjectMapperManager.DefaultInstance.GetMapper<List<Risque>, List<RisqueDto>>().Map(PoliceRepository.ObtenirRisquesByTerm(codeOffre, version));

        //    return toReturn;
        //}

        public string GetFirstCodeRsq(string codeOffre, string version, string type)
        {
            return RisqueRepository.GetFirstCodeRsq(codeOffre, version, type);
        }

        public string GetFirstCodeObjRsq(string codeOffre, string version, string type, string codeRsq)
        {
            return RisqueRepository.GetFirstCodeObjRsq(codeOffre, version, type, codeRsq);
        }

        public List<RisqueDto> ListRisquesObjet(string codeOffre, string version, string type, string avenant, ModeConsultation modeNavig)
        {
            if (modeNavig == ModeConsultation.Historique)
                return RisqueRepository.ObtenirRisquesAvenant(codeOffre, version, type, avenant, modeNavig);
            return RisqueRepository.ObtenirRisques(codeOffre, version, type, avenant, modeNavig);
        }

        #endregion

        #region Details Risque

        public DetailsRisqueGetResultDto DetailsRisqueGet(DetailsRisqueGetQueryDto query, string codeAvn, string branche, string cible, ModeConsultation modeNavig, bool isAdmin, bool isUserHorse)
        {
            var toReturn = new DetailsRisqueGetResultDto();

            //AccesDataManager._connectionHelper = easyComConnectionHelper;
            toReturn.HasFormules = RisqueRepository.RisquesHasFormules(query.CodeOffre, query.Version, query.Type, codeAvn, query.CodeRisque, modeNavig);
            toReturn.Cibles = BrancheRepository.ObtenirCibles(query.CodeBranche, isAdmin, isUserHorse);
            toReturn.Unites = CommonRepository.GetParametres(branche, cible, "PRODU", "QCVAU");
            //toReturn.Unites = ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.ObtenirUniteValeur());
            toReturn.Types = CommonRepository.GetParametres(branche, cible, "PRODU", "QCVAT");
            //toReturn.Types = ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.ObtenirTypeValeur());
            toReturn.TypesInventaire = ReferenceRepository.ObtenirTypeInventaire();
            toReturn.RegimesTaxe = CommonRepository.GetParametres(branche, cible, "GENER", "TAXRG");
            //toReturn.RegimesTaxe = ObjectMapperManager.DefaultInstance.GetMapper<List<Parametre>, List<ParametreDto>>().Map(ReferenceRepository.ObtenirRegimeTaxe());
            //Nomenclature de risques
            toReturn.CodesApe = new List<ParametreDto>();
            toReturn.CodesTre = CommonRepository.GetParametres(branche, cible, "KHEOP", "TREAC");
            toReturn.Territorialites = CommonRepository.GetParametres(branche, cible, "PRODU", "QATRR");


            //Récupération de l'ensemble des combos
            var resultCombo = RisqueRepository.GetComboNomenclatures(query.CodeOffre, Convert.ToInt32(query.Version), query.Type, Convert.ToInt32(query.CodeRisque), 0, cible);
            if (resultCombo != null && resultCombo.Count > 0)
            {
                toReturn.Nomenclatures1 = resultCombo.FindAll(elm => elm.NumeroCombo == 1);
                toReturn.Nomenclatures2 = resultCombo.FindAll(elm => elm.NumeroCombo == 2);
                toReturn.Nomenclatures3 = resultCombo.FindAll(elm => elm.NumeroCombo == 3);
                toReturn.Nomenclatures4 = resultCombo.FindAll(elm => elm.NumeroCombo == 4);
                toReturn.Nomenclatures5 = resultCombo.FindAll(elm => elm.NumeroCombo == 5);

                if (toReturn.Nomenclatures1 != null)
                    toReturn.Nomenclatures1 = toReturn.Nomenclatures1.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures2 != null)
                    toReturn.Nomenclatures2 = toReturn.Nomenclatures2.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures3 != null)
                    toReturn.Nomenclatures3 = toReturn.Nomenclatures3.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures4 != null)
                    toReturn.Nomenclatures4 = toReturn.Nomenclatures4.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures5 != null)
                    toReturn.Nomenclatures5 = toReturn.Nomenclatures5.OrderBy(elm => elm.OrdreNomenclature).ToList();
            }
            else
            {
                toReturn.Nomenclatures1 = new List<NomenclatureDto>();
                toReturn.Nomenclatures2 = new List<NomenclatureDto>();
                toReturn.Nomenclatures3 = new List<NomenclatureDto>();
                toReturn.Nomenclatures4 = new List<NomenclatureDto>();
                toReturn.Nomenclatures5 = new List<NomenclatureDto>();
            }


            toReturn.CodesClasse = new List<ParametreDto>();

            toReturn.TypesRisque = CommonRepository.GetParametres(branche, cible, "KHEOP", "RISRS");
            toReturn.TypesMateriel = CommonRepository.GetParametres(branche, cible, "KHEOP", "MATRS");
            toReturn.NaturesLieux = CommonRepository.GetParametres(branche, cible, "ALSPK", "NLOC");
            toReturn.IsExistLoupe = PoliceRepository.ChekConceptFamille(cible);

            toReturn.DateDebHisto = PoliceRepository.GetDateDebRsqHisto(query.CodeOffre, query.Version.ToString(), query.CodeRisque, codeAvn);

            return toReturn;
        }

        public DetailsRisqueSetResultDto DetailsRisqueSet(DetailsRisqueSetQueryDto query, string user)
        {
            var toReturn = new DetailsRisqueSetResultDto();
            if (query != null && query.Offre != null)
            {
                var offre = query.Offre;
                toReturn.Code = PoliceRepository.SauvegarderDetailsRisque(offre, user);
            }
            return toReturn;
        }

        public DetailsRisqueDelResultDto DetailsRisqueDel(DetailsRisqueDelQueryDto query)
        {
            var toReturn = new DetailsRisqueDelResultDto();
            if (query != null && query.offre != null)
            {
                PoliceRepository.DeleteDetailsRisque(query.offre);
                #region Arbre de navigation
                //NavigationArbreRepository.DelTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
                //{
                //    CodeOffre = query.offre.CodeOffre.PadLeft(9, ' '),
                //    Version = query.offre.Version.Value,
                //    Type = query.offre.Type,
                //    EtapeGeneration = "RSQ",
                //    Risque = query.offre.Risques[0].Code
                //});
                #endregion
            }
            return toReturn;
        }

        public string LoadComplementNum1(string concept, string famille, string code)
        {
            //return RisqueRepository.LoadComplementNum1(concept, famille, code);
            return RisqueRepository.LoadComplementAlpha2(concept, famille, code);
        }

        public List<ParametreDto> GetListeTypesRegularisation(string branche, string cible)
        {
            return RisqueRepository.GetListeTypesRegularisation(branche, cible);
        }

        public bool CheckObjetSorit(string codeOffre, int? version, string type, string codeAvn, string openObj)
        {
            return RisqueRepository.CheckObjetSorit(codeOffre, version, type, codeAvn, openObj);
        }

        #endregion

        #region Details Objet Risque

        public DetailsObjetRisqueGetResultDto DetailsObjetRisqueGet(DetailsObjetRisqueGetQueryDto query, string codeAvn, string branche, string cible, ModeConsultation modeNavig, bool isAdmin, bool isUserHorse)
        {
            var toReturn = new DetailsObjetRisqueGetResultDto();

            toReturn.HasFormules = RisqueRepository.RisquesHasFormules(query.CodeOffre, Convert.ToInt32(query.Version), query.Type, codeAvn, Convert.ToInt32(query.CodeRisque), modeNavig);
            toReturn.Cibles = BrancheRepository.ObtenirCibles(query.CodeBranche, isAdmin, isUserHorse);
            toReturn.Unites = CommonRepository.GetParametres(branche, cible, "PRODU", "QCVAU");
            toReturn.Types = CommonRepository.GetParametres(branche, cible, "PRODU", "QCVAT");
            toReturn.TypesInventaire = ReferenceRepository.ObtenirTypeInventaire(branche, cible);
            //Nomenclature d'objets
            toReturn.CodesApe = new List<ParametreDto>();
            toReturn.CodesTre = CommonRepository.GetParametres(branche, cible, "KHEOP", "TREAC");
            toReturn.Territorialites = CommonRepository.GetParametres(branche, cible, "PRODU", "QATRR");
            //Sauvegarde pour la demo du 15/11
            //toReturn.Nomenclatures1 = CommonRepository.GetParametres(branche, cible, "KHEOP", "NCRS1");

            //Récupération de l'ensemble des combos
            int version = 0, codeRisque = 0, codeObjet = 0;
            Int32.TryParse(query.Version, out version);
            Int32.TryParse(query.CodeRisque, out codeRisque);
            Int32.TryParse(query.CodeObjet, out codeObjet);

            var resultCombo = RisqueRepository.GetComboNomenclatures(query.CodeOffre, version, query.Type, codeRisque, codeObjet, cible);
            if (resultCombo != null && resultCombo.Count > 0)
            {
                toReturn.Nomenclatures1 = resultCombo.FindAll(elm => elm.NumeroCombo == 1);
                toReturn.Nomenclatures2 = resultCombo.FindAll(elm => elm.NumeroCombo == 2);
                toReturn.Nomenclatures3 = resultCombo.FindAll(elm => elm.NumeroCombo == 3);
                toReturn.Nomenclatures4 = resultCombo.FindAll(elm => elm.NumeroCombo == 4);
                toReturn.Nomenclatures5 = resultCombo.FindAll(elm => elm.NumeroCombo == 5);

                if (toReturn.Nomenclatures1 != null)
                    toReturn.Nomenclatures1 = toReturn.Nomenclatures1.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures2 != null)
                    toReturn.Nomenclatures2 = toReturn.Nomenclatures2.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures3 != null)
                    toReturn.Nomenclatures3 = toReturn.Nomenclatures3.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures4 != null)
                    toReturn.Nomenclatures4 = toReturn.Nomenclatures4.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures5 != null)
                    toReturn.Nomenclatures5 = toReturn.Nomenclatures5.OrderBy(elm => elm.OrdreNomenclature).ToList();
            }
            else
            {
                toReturn.Nomenclatures1 = new List<NomenclatureDto>();
                toReturn.Nomenclatures2 = new List<NomenclatureDto>();
                toReturn.Nomenclatures3 = new List<NomenclatureDto>();
                toReturn.Nomenclatures4 = new List<NomenclatureDto>();
                toReturn.Nomenclatures5 = new List<NomenclatureDto>();
            }


            toReturn.CodesClasse = new List<ParametreDto>();

            toReturn.TypesRisque = CommonRepository.GetParametres(branche, cible, "KHEOP", "RISRS");
            toReturn.TypesMateriel = CommonRepository.GetParametres(branche, cible, "KHEOP", "MATRS");
            toReturn.NaturesLieux = CommonRepository.GetParametres(branche, cible, "ALSPK", "NLOC");
            toReturn.IsExistLoupe = PoliceRepository.ChekConceptFamille(cible);
            toReturn.DateDebHisto = PoliceRepository.GetDateDebObjHisto(query.CodeOffre, query.Version, codeRisque, codeObjet, codeAvn);

            return toReturn;
        }

        public string DetailsObjetRisqueSet(DetailsObjetRisqueSetQueryDto query, string user)
        {
            string toReturn = string.Empty;

            if (query != null && query.Offre != null)
            {
                OffreDto offre = query.Offre;
                toReturn = PoliceRepository.SauvegarderDetailsObjetRisque(offre, user);
            }
            return toReturn;
        }

        public DetailsObjetRisqueDelResultDto DetailsObjetRisqueDel(DetailsObjetRisqueDelQueryDto query)
        {
            var toReturn = new DetailsObjetRisqueDelResultDto();
            PoliceRepository.DeleteDetailsObjet(query.offre/*ObjectMapperManager.DefaultInstance.GetMapper<OffreDto, Offre>().Map(query.offre)*/);
            #region Arbre de navigation
            NavigationArbreRepository.DelTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = query.offre.CodeOffre.PadLeft(9, ' '),
                Version = query.offre.Version.Value,
                Type = query.offre.Type,
                EtapeGeneration = "OBJ",
                Risque = query.offre.Risques[0].Code,
                Objet = query.offre.Risques[0].Objets[0].Code
            });
            #endregion

            return toReturn;
        }

        public void SaveValeurModeAvn(string codeOffre, string version, string type, string valeur)
        {
            PoliceRepository.SaveValeurModeAvn(codeOffre, version, type, valeur);
        }

        public string GetQuestionMedical(string codeAffaire, string version, string type, string codeRsq, string codeObj, string oldValue, bool controlAssiette, string user)
        {
            return RisqueRepository.GetQuestionMedical(codeAffaire, version, type, codeRsq, codeObj, oldValue, controlAssiette, user);
        }

        #endregion

        #region recherche  Activites


        public RechercheActiviteDto GetActivites(string code, string branche, string cible, string nom, int startLigne, int endLigne)
        {

            return PoliceRepository.GetActivites(code, branche, cible, nom, startLigne, endLigne);

        }

        public string LoadCodeClassByCible(string codeCible, string codeActivite)
        {
            return PoliceRepository.LoadCodeClassByCible(codeCible, codeActivite);
        }


        #endregion
        #region Informations Spécifiques Risques

        public string InfoSpecRisqueSet(OffreDto offreDto, string codeAvn, ModeConsultation modeNavig)
        {
            if (!PoliceRepository.ValiderCatnat(offreDto, codeAvn, modeNavig)) return "CATNAT non soumise sur ce risque";
            PoliceRepository.UpdateDetailsRisque_YPRTRSQ(offreDto, true);
            return string.Empty;
        }

        #endregion

        #region Clauses

        /// <summary>
        /// Initialise les listes
        /// de l'écran clausier
        /// </summary>
        public ClausierPageDto InitClausier()
        {
            return ClauseRepository.InitClausier();
        }
        
        /// <summary>
        /// Ajout  Libelle 
        /// Clause
        /// </summary>
        public void SaveClauseLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle)
        {
            ClauseRepository.AddClauseLibelle(branche, cible, nm1, nm2, nm3, libelle);
        }
        /// <summary>
        /// Modifier le Libelle 
        /// de Clause
        /// </summary>
        public void UpdateClauseLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle)
        {
            ClauseRepository.UpdateClauseLibelle(branche, cible, nm1, nm2, nm3, libelle);
        }

        /// <summary>
        /// Supprimer le Libelle 
        /// de Clause
        /// </summary>
        public void DeleteClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3, string libelle)
        {
            ClauseRepository.DeleteClausesLibelle(branche, cible, nm1, nm2, nm3, libelle);
        }
        /// <summary>
        /// Retourne la liste des clauses
        /// de l'écran
        /// </summary>
        public List<LibelleClauseDto> GetClausesLibelle(string branche, string cible, string nm1, string nm2, string nm3)
        {
            return ClauseRepository.GetClausesLibelle(branche, cible, nm1,nm2,nm3);
        }

        /// <summary>
        /// Retourne la liste des Branche
        /// Pour les clauses
        /// </summary>
        public List<ClauseBrancheDto> GetClausesBranches()
        {
            return ClauseRepository.GetClausesBranches();
        }

        /// <summary>
        /// Recherche et retourne une liste de clauses
        /// suivant des critères
        /// </summary>
        public List<ClausierDto> SearchClause(string libelle, string motcle1, string motcle2, string motcle3, string sequence, string rubrique, string sousrubrique, string modaliteAffichage, int date)
        {
            return ClauseRepository.SearchClause(libelle, motcle1, motcle2, motcle3, sequence, rubrique, sousrubrique, modaliteAffichage, date);
        }

        /// <summary>
        /// Récupère la liste de l'historique
        /// d'une clause
        /// </summary>
        public List<ClausierDto> GetHistoClause(string rubrique, string sousrubrique, string sequence)
        {
            return ClauseRepository.GetHistoClause(rubrique, sousrubrique, sequence);
        }

        /// <summary>
        /// Sauvegarde la clause sélectionnée
        /// </summary>
        public string SaveClause(string codeOffre, string version, string type, string etape, string perimetre, string codeRsq, string codeObj,
            string codeFor, string codeOpt, string contexte, string codeClause, string rubrique, string sousRubrique, string sequence, string versionClause, string numAvenant)
        {
            return ClauseRepository.SaveClause(codeOffre, version, type, etape, perimetre, codeRsq, codeObj, codeFor, codeOpt, contexte, codeClause, rubrique, sousRubrique, sequence, versionClause, numAvenant);
        }

        /// <summary>
        /// Récupère la liste des sous-rubriques
        /// </summary>
        public List<ParametreDto> GetListSousRubriques(string codeRubrique)
        {
            return ClauseRepository.GetListSousRubriques(codeRubrique);
        }

        /// <summary>
        /// Récupère la liste des séquences
        /// </summary>
        public List<ParametreDto> GetListSequences(string codeRubrique, string codeSousRubrique)
        {
            return ClauseRepository.GetListSequences(codeRubrique, codeSousRubrique);
        }
        //SAB24042016: Pagination clause

        public List<ClauseDto> ClausesGet(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousRubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig)
        {
            return ClauseRepository.ObtenirBaseInfosClauses(type, numeroOffre, version, codeAvn, etape, contexte, rubrique, sousRubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig);
        }
        public ClauseDto DetailsClauses(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousRubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig)
        {
            return ClauseRepository.DetailsClauses(type, numeroOffre, version, codeAvn, etape, contexte, rubrique, sousRubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig);
        }
        //public List<ClauseDto> ClausesGet(string type, string numeroOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousRubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig, string coltri, string triimg, int StartLine, int EndLine)
        //{
        //    return ClauseRepository.ObtenirClauses(type, numeroOffre, version, codeAvn, etape, contexte, rubrique, sousRubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, coltri, triimg, StartLine,  EndLine, modeNavig);
        //}

        //SAB24042016: Pagination clause

        public ClauseVisualisationDto ClausesGet2(string type, string codeOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig)
        {
            return RisqueRepository.ClausesGet(type, codeOffre, version, codeAvn, etape, contexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig);
        }

        //public ClauseVisualisationDto ClausesGet2(string type, string codeOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig, string coltri, string triimg, int StartLine, int EndLine)
        //{
        //    return RisqueRepository.ClausesGet(type, codeOffre, version, codeAvn, etape, contexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig, coltri, triimg, StartLine,  EndLine);
        //}

        //public ChoixClausesSetResultDto ClausesSet(ChoixClausesSetQueryDto query)
        //{
        //    var result = new ChoixClausesSetResultDto();


        //    //AccesDataManager._connectionHelper = easyComConnectionHelper;
        //    //var clauses = ObjectMapperManager.DefaultInstance.GetMapper<List<ClauseDto>, List<Clause>>().Map(query.Clauses);

        //    ClauseRepository.SauvegarderEtatTitre(query.Clauses);

        //    return result;
        //}

        //public void EnregistreClauseUnique(string type, string numeroOffre, string version, string natureClause, string codeClause, string versionClause, string actionEnchaine, string contexte, string utilisateur, string etape)
        //{
        //    var offre = new Offre { Type = type, CodeOffre = numeroOffre, Version = int.Parse(version) };

        //    //AccesDataManager._connectionHelper = easyComConnectionHelper;
        //    PoliceRepository.EnregistrerClause(offre, natureClause, codeClause, versionClause, actionEnchaine, contexte, utilisateur, etape);

        //}
        public void SupprimeClauseUnique(string id)
        {
            ClauseRepository.SupprimerClause(id);
        }
        //SAB24042016: Pagination clause
        public ChoixClausesInfoDto SupprimeClauseUnique2(string id, OrigineAppel origine,
                       string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
                       string codeEtape, string contexte, string famille)
        {
            ClauseRepository.SupprimerClause(id);

            return RisqueRepository.GetInfoChoixClauses(origine, type, codeOffre, version, codeAvn, etape, filtreContexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig, codeEtape, contexte, famille);
        }
        //public ChoixClausesInfoDto SupprimeClauseUnique2(string id, OrigineAppel origine,
        //                string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
        //                string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine)
        //{
        //    ClauseRepository.SupprimerClause(id);

        //    return RisqueRepository.GetInfoChoixClauses(origine, type, codeOffre, version, codeAvn, etape, filtreContexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig, codeEtape, contexte, famille, coltri, triimg, StartLine,  EndLine);
        //}

        public string EnregistreClauseLibre(string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string contexte, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre)
        {
            return ClauseRepository.EnregistrerClauseLibre(codeOffreContrat, versionOffreContrat, typeOffreContrat, contexte, etape, codeRisque, codeFormule, codeOption, codeObj, libelleClauseLibre, texteClauseLibre);
        }

        //SAB24042016: Pagination clause

        public EnregistrementClauseLibreDto EnregistreClauseLibre2(string codeOffre, string version, string type, string codeAvn, string contexteClause, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre,
                  OrigineAppel origine,
                      string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
                      string codeEtape, string contexte, string famille)
        {
            return RisqueRepository.EnregistrerClauseLibre(codeOffre, version, type, codeAvn, contexteClause, etape, codeRisque, codeFormule, codeOption, codeObj, libelleClauseLibre, texteClauseLibre,
                            origine, filtreContexte, rubrique, sousrubrique, sequence, idClause, filtre, modeNavig, codeEtape, contexte, famille);
        }
        //public EnregistrementClauseLibreDto EnregistreClauseLibre2(string codeOffre, string version, string type, string codeAvn, string contexteClause, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre,
        //            OrigineAppel origine,
        //                string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
        //                string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine)
        //{
        //    return RisqueRepository.EnregistrerClauseLibre(codeOffre, version, type, codeAvn, contexteClause, etape, codeRisque, codeFormule, codeOption, codeObj, libelleClauseLibre, texteClauseLibre,
        //                    origine, filtreContexte, rubrique, sousrubrique, sequence, idClause, filtre, modeNavig, codeEtape, contexte, famille, coltri ,  triimg, StartLine,  EndLine );
        //}

        public string UpdateEtatTitre(string clauseId, string etatTitre)
        {
            return ClauseRepository.UpdateEtatTitre(clauseId, etatTitre);
        }

        public void UpdateTextClauseLibre(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj)
        {
            ClauseRepository.UpdateTextClauseLibre(clauseId, titreClauseLibre, texteClauseLibre, codeObj);
        }
        //SAB24042016: Pagination clause
        public EnregistrementClauseLibreDto UpdateTextClauseLibre2(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj,
                       string codeOffre, string version, string type, string codeAvn, string etape, string codeRisque, string codeFormule, string codeOption, OrigineAppel origine,
                       string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
                       string codeEtape, string contexte, string famille)
        {
            return RisqueRepository.UpdateClauseLibre(clauseId, titreClauseLibre, texteClauseLibre, codeObj, codeOffre, version, type, codeAvn, etape, codeRisque, codeFormule, codeOption, origine, filtreContexte, rubrique, sousrubrique, sequence, idClause, filtre, modeNavig, codeEtape, contexte, famille);
        }

        //public EnregistrementClauseLibreDto UpdateTextClauseLibre2(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj,
        //                string codeOffre, string version, string type, string codeAvn, string etape, string codeRisque, string codeFormule, string codeOption, OrigineAppel origine,
        //                string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
        //                string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine)
        //{
        //    return RisqueRepository.UpdateClauseLibre(clauseId, titreClauseLibre, texteClauseLibre, codeObj, codeOffre, version, type, codeAvn, etape, codeRisque, codeFormule, codeOption, origine, filtreContexte, rubrique, sousrubrique, sequence, idClause, filtre, modeNavig, codeEtape, contexte, famille,coltri ,triimg, StartLine,  EndLine );
        //}

        public void UpdateTextClauseLibreOffreSimp(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeRisque, string codeObj, string codeFormule, string codeOption)
        {
            ClauseRepository.UpdateTextClauseLibreOffreSimp(clauseId, titreClauseLibre, texteClauseLibre, codeRisque, codeObj, codeFormule, codeOption);
        }

        public ClauseLibreViewerDto GetInfoClauseLibreViewer(string codeOffre, string version, string type, string codeRsq, string clauseId, string etape, string contexte)
        {
            return ClauseRepository.GetInfoClauseLibreViewer(codeOffre, version, type, codeRsq, clauseId, etape, contexte);
        }

        public string VerifAjout(string etape, string contexte, string typeAjt)
        {
            return ClauseRepository.VerifAjout(etape, contexte, typeAjt);
        }

        public string SaveClauseLibre(string codeOffre, string version, string type, string codeAvt, string contexte, string etape, string codeRsq, string codeObj, string codeFor, string codeOpt, string emplacement, string sousemplacement, string ordre)
        {
            return ClauseRepository.SaveClauseLibre(codeOffre, version, type, codeAvt, contexte, etape, codeRsq, codeObj, codeFor, codeOpt, emplacement, sousemplacement, ordre);
        }

        public void UpdateDocumentLibre(string idClause, string idDoc, string codeObj, string codeAvn)
        {
            ClauseRepository.UpdateDocumentLibre(idClause, idDoc, codeObj, codeAvn);
        }

        public string CreateDocumentLibre(string codeOffre, string version, string type, string etape, string idClause, string pathDoc, string nameDoc, string createDoc)
        {
            return ClauseRepository.CreateDocumentLibre(codeOffre, version, type, etape, idClause, pathDoc, nameDoc, createDoc);
        }

        public string GetClauseFilePath(string clauseId, ModeConsultation modeNavig)
        {
            return ClauseRepository.GetClauseFilePath(clauseId, modeNavig);
        }

        public ChoixClausePieceJointeDto GetListPiecesJointes(string codeOffre, string version, string type, string codeRisque, string codeObjet, string etape, string contexte)
        {
            return ClauseRepository.GetListPiecesJointes(codeOffre, version, type, codeRisque, codeObjet, etape, contexte);
        }

        public void SavePiecesJointes(string codeOffre, string version, string type, string contexte, string etape, string codeRsq, string codeObj,
                string codeFor, string codeOpt, string emplacement, string sousemplacement, string ordre, string piecesjointesid)
        {
            ClauseRepository.SavePiecesJointes(codeOffre, version, type, contexte, etape, codeRsq, codeObj,
                codeFor, codeOpt, emplacement, sousemplacement, ordre, piecesjointesid);
        }

        public void SaveClauseEntete(string idClause, string emplacement, string sousemplacement, string ordre)
        {
            ClauseRepository.SaveClauseEntete(idClause, emplacement, sousemplacement, ordre);
        }

        public void SaveClauseMagnetic(string codeAffaire, string version, string type, int idDoc, string acteGes, string etape, string nameClause, string fileName,
            int idClause, string emplacement, string sousemplacement, string ordre, string contexte)
        {
            ClauseRepository.SaveClauseMagnetic(codeAffaire, version, type, idDoc, acteGes, etape, nameClause, fileName, idClause, emplacement, sousemplacement, ordre, contexte, true);
        }

        #endregion

        #region Details Inventaire

        //public DetailsInventaireDelResultDto DetailsInventaireDel(DetailsInventaireDelQueryDto query)
        //{
        //    var toReturn = new DetailsInventaireDelResultDto();



        //    //toReturn.Inventaires = ObjectMapperManager.DefaultInstance.GetMapper<List<Inventaire>, List<InventaireDto>>().Map(PoliceRepository.SupprimerRisqueInventaire(query.CodeOffre, query.Version, query.CodeRisque, query.CodeObjet, query.CodeInventaire, query.NumDescription));
        //    toReturn.Inventaires = PoliceRepository.SupprimerRisqueInventaire(query.CodeOffre, query.Version,query.Type, query.CodeRisque, query.CodeObjet, query.CodeInventaire, query.NumDescription);


        //    return toReturn;
        //}

        public void DetailsInventaireDel(DetailsInventaireDelQueryDto query)
        {
            PoliceRepository.SupprimerRisqueInventaire(query.CodeOffre, query.Version, query.Type, query.CodeRisque, query.CodeObjet, query.CodeInventaire, query.NumDescription);
        }



        //public DetailsInventaireSetResultDto DetailsInventaireSet(DetailsInventaireSetQueryDto query)
        //{
        //    var toReturn = new DetailsInventaireSetResultDto();

        //    //AccesDataManager._connectionHelper = easyComConnectionHelper;
        //    Offre offre = ObjectMapperManager.DefaultInstance.GetMapper<OffreDto, Offre>().Map(query.Offre);

        //    PoliceRepository.SauvegarderDetailsInventaire(offre);

        //    return toReturn;
        //}






        public string DetailsInventaireGetSumBudget(string codeInventaire, string typeInventaire)
        {
            return InventaireRepository.GetSumBugdetInventaire(codeInventaire, typeInventaire).ToString(CultureInfo.CurrentCulture);

            //return BLInventaire.GetSumBugdetInventaire(codeInventaire).ToString(CultureInfo.CurrentCulture);
        }

        #endregion

        #region Matrice Formule

        #region Méthodes Publiques

        public MatriceFormuleDto InitMatriceFormule(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly)
        {
            return RisqueRepository.InitMatriceFormule(codeOffre, version, type, codeAvn, modeNavig, user, acteGestion, isReadonly);
        }

        public void DeleteFormuleGarantie(string codeOffre, string version, string type, string codeFormule, string typeDel)
        {
            FormuleRepository.DeleteFormuleGarantie(codeOffre, version, type, codeFormule, typeDel);
        }
        public void DeleteFormuleGarantieRsq(string codeOffre, string version, string type, string codeRisque, string typeDel)
        {
            FormuleRepository.DeleteFormuleGarantieRsq(codeOffre, version, type, codeRisque, typeDel);
        }

        /// <summary>
        /// Supprime une formule de garantie
        /// </summary>
        /// <param name="codeOffre">Code Offre</param>
        /// <param name="version">Version</param>
        /// <param name="type">Type</param>
        /// <param name="codeFormule">Code Formule</param>
        public void DeleteFormule(string codeOffre, string version, string type, string codeFormule, string typeDel)
        {
            FormuleRepository.DeleteFormule(codeOffre, version, type, codeFormule, typeDel);
            #region Arbre de navigation
            NavigationArbreRepository.DelTraceArbre(new OP.WSAS400.DTO.NavigationArbre.TraceDto
            {
                CodeOffre = codeOffre.PadLeft(9, ' '),
                Version = Convert.ToInt32(version),
                Type = type,
                EtapeGeneration = "FOR",
                Formule = Convert.ToInt32(codeFormule)
            });
            #endregion
        }

        /// <summary>
        /// Vérifie qu'une formule de garantie est renseignée
        /// La supprime si ce n'est pas le cas
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        public void CheckFormule(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            FormuleRepository.CheckFormule(codeOffre, version, type, codeFormule);
            if (!string.IsNullOrEmpty(codeFormule))
                FormuleRepository.BackwardFormule(codeOffre, version, type, codeFormule, codeOption);
        }

        /// <summary>
        /// Duplique une formule de garantie
        /// </summary>
        public string DuplicateFormule(string codeOffre, string version, string type, string codeFormule, string user)
        {
            return FormuleRepository.DuplicateFormule(codeOffre, version, type, codeFormule, user);
        }

        public string GetLibFormule(int codeFormule, string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            return FormuleRepository.GetLibFormule(codeFormule, codeOffre, version, type, codeAvn, modeNavig);
        }
       
        public string GetValidRsq(string codeOffre, string version, string type, string codeRsq)
        {
            return RisqueRepository.GetValidRsq(codeOffre, version, type, codeRsq);
        }

        #endregion

        #region Méthodes Privées

        #endregion

        #endregion

        #region Matrice Risque

        #region Méthode Publique

        public MatriceRisqueDto InitMatriceRisque(string codeOffre, string version, string type, string codeAvenant, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly)
        {
            return RisqueRepository.InitMatriceRisque(codeOffre, version, type, codeAvenant, modeNavig, user, acteGestion, isReadonly);
        }

        #endregion

        #endregion

        #region Matrice Garantie

        #region Methode public

        public MatriceGarantieDto InitMatriceGarantie(string argCodeOffre, string argVersion, string argType, string argCodeAvn, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly)
        {
            return RisqueRepository.InitMatriceGarantie(argCodeOffre, argVersion, argType, argCodeAvn, modeNavig, user, acteGestion, isReadonly);
        }

        #endregion
        #endregion

        #region Inventaires

        public InventaireDto GetInventaire(string codeOffre, int version, string type, string codeAvn, string ecranProvenance, int codeRisque, int codeObjet, int codeFormule, string codeGarantie, string typeInventaire, Int64 codeInven, string branche, string cible, ModeConsultation modeNavig)
        {
            return InventaireRepository.LoadInventaire(codeOffre, version, type, codeAvn, ecranProvenance, codeRisque, codeObjet, codeFormule, codeGarantie, typeInventaire, codeInven, branche, cible, modeNavig);

            //return BLInventaire.LoadInventaire(codeOffre, version, type, codeAvn, ecranProvenance, codeRisque, codeObjet, codeFormule, codeGarantie, typeInventaire, codeInven, branche, cible, modeNavig);
        }

        /// <summary>
        /// Sauvegarde la ligne d'inventaire et retourne 
        /// uniquement la ligne sauvegardée
        /// </summary>
        /// <param name="inventaireLigne">Ligne d'inventaire à sauvegarder</param>
        /// <returns></returns>
        public InventaireGridRowDto SaveLineInventaire(string codeOffre, int version, string type, int codeInven, int typeInven, InventaireGridRowDto inventaireLigne)
        {
            return InventaireRepository.SaveLineInventaire(codeOffre, version, type, codeInven, typeInven, inventaireLigne);

            //return BLInventaire.SaveLineInventaire(codeOffre, version, type, codeInven, typeInven, inventaireLigne);
        }

        public void SaveInventaire(string codeOffre, string version, string type, string ecranProvenance, string codeRisque, string codeObjet, string codeInven, string descriptif, string description, string valReport, string unitReport, string typeReport, string taxeReport, bool activeReport, string typeAlim, string garantie, string codeFormule, string codeOption)
        {
            InventaireRepository.SaveInventaire(codeOffre, version, type, ecranProvenance, codeRisque, codeObjet, codeInven, descriptif, description, valReport, unitReport, typeReport, taxeReport, activeReport, typeAlim, garantie, codeFormule, codeOption);

            //BLInventaire.SaveInventaire(codeOffre, version, type, ecranProvenance, codeRisque, codeObjet, codeInven, descriptif, description, valReport, unitReport, typeReport, taxeReport, activeReport, typeAlim, garantie);
        }

        public void DeleteLineInventaire(string codeInven)
        {
            InventaireRepository.DeleteLineInventaire(codeInven);

            //BLInventaire.DeleteLineInventaire(codeInven);
        }

        #endregion

        #region TestProd
        public bool TestSrtoredProc()
        {
            //FormuleRepository.CallStoredProcedure();
            return true;
        }
        #endregion

        #region Oppositions

        public List<OppositionDto> ObtenirListeOppositions(string idOffre, string versionOffre, string typeOffre, string idRisque, ModeConsultation modeNavig)
        {
            return RisqueRepository.ObtenirListeOppositions(idOffre, versionOffre, typeOffre, idRisque);
        }

        public OppositionDto ObtenirDetailOpposition(string idOffre, string versionOffre, string typeOffre, string codeAvn, string idRisque, string idOpposition, string mode, ModeConsultation modeNavig, string typeDest)
        {
            return RisqueRepository.ObtenirDetailOpposition(idOffre, versionOffre, typeOffre, codeAvn, idRisque, idOpposition, mode, modeNavig, typeDest);
        }

        public int MiseAJourOpposition(string idOffre, string versionOffre, string typeOffre, string idRisque, OppositionDto opposition, string objets, string user)
        {
            return RisqueRepository.MiseAJourOpposition(idOffre, versionOffre, typeOffre, idRisque, opposition, objets, user);
        }

        public List<OrganismeOppDto> OrganismesGet(string value, string mode, string typeOppBenef)
        {
            return RisqueRepository.OrganismesGet(value, mode, typeOppBenef);
        }


        #endregion

        #region Clauses

        public RisqueDto GetRisque(string codeOffre, string version, string type, string codeRsq)
        {
            return RisqueRepository.GetRisque(codeOffre, version, type, codeRsq);
        }

        #endregion

        #region Inventaire de garantie
        public string SupprimerGarantieInventaire(string codeOffre, string version, string type, string codeFormule, string codeGarantie, string codeInventaire)
        {
            return InventaireRepository.SupprimerGarantieInventaire(codeOffre, version, type, codeFormule, codeGarantie, codeInventaire);

            //return BLInventaire.SupprimerGarantieInventaire(codeOffre, version, type, codeFormule, codeGarantie, codeInventaire);
        }
        public string SupprimerGarantieListInventaires(string codeOffre, string version, string type, string codeFormule, string codesGaranties, string codesInventaires)
        {
            return InventaireRepository.SupprimerGarantieListInventaires(codeOffre, version, type, codeFormule, codesGaranties, codesInventaires);

            //return BLInventaire.SupprimerGarantieListInventaires(codeOffre, version, type, codeFormule, codesGaranties, codesInventaires);
        }
        public string SupprimerListInventairesByCodeInventaire(string codesInventaires)
        {
            return InventaireRepository.SupprimerListInventairesByCodeInventaire(codesInventaires);

            //return BLInventaire.SupprimerListInventairesByCodeInventaire(codesInventaires);
        }
        #endregion
        #region LCIFranchise
        public void EnregistrementExpCompGeneraleRisque(string codeOffre, string version, string typeOffre, string codeAvn, string codeFormule, string codeOption, string codeRisque, string codeExpression, string unite, AlbConstantesMetiers.ExpressionComplexe typeVue, AlbConstantesMetiers.TypeAppel typeAppel, ModeConsultation modeNavig)
        {
            ConditionRepository.EnregistrementExpCompGeneraleRisque(codeOffre, version, typeOffre, codeAvn, codeFormule, codeOption, codeRisque, codeExpression, unite, typeVue, typeAppel, modeNavig);
        }
        public LCIFranchiseDto GetLCIFranchise(string codeOffre, string version, string typeOffre, string codeAvn, string codeRisque, AlbConstantesMetiers.ExpressionComplexe typeVue, AlbConstantesMetiers.TypeAppel typeAppel, ModeConsultation modeNavig)
        {
            return ConditionRepository.GetLCIFranchise(codeOffre, version, typeOffre, codeAvn, codeRisque, typeVue, typeAppel, modeNavig);
        }
        public string InfoSpecRisqueLCIFranchiseSet(OffreDto offreDto, string codeAvn
                                , string argValeurLCIRisque, string argUniteLCIRisque, string argTypeLCIRisque, string argLienCpxLCIRisque
                                , string argValeurFranchiseRisque, string argUniteFranchiseRisque, string argTypeFranchiseRisque, string argLienCpxFranchiseRisque, ModeConsultation modeNavig)
        {
            var offre = offreDto;
            if (!PoliceRepository.ValiderCatnat(offre, codeAvn, modeNavig)) return "CATNAT non soumise sur ce risque";
            PoliceRepository.UpdateDetailsRisque_YPRTRSQ(offre, true);

            string codeRisque = "0";
            if (offre.Risques != null && offre.Risques.Count > 0)
                codeRisque = offre.Risques[0].Code.ToString();
            ConditionRepository.InfoSpecRisqueLCIFranchiseSet(
                offre.CodeOffre, offre.Version.ToString(), offre.Type, codeRisque
                , argValeurLCIRisque, argUniteLCIRisque, argTypeLCIRisque, argLienCpxLCIRisque
                , argValeurFranchiseRisque, argUniteFranchiseRisque, argTypeFranchiseRisque, argLienCpxFranchiseRisque);
            return string.Empty;

        }
        #endregion
        #region Generation Clauses
        public RetGenClauseDto GenerateClause(string type, string codeDossier, int version,
                                              ParametreGenClauseDto parmClause)
        {
            //Mise à jour du tag de génération des clauses dans KPCTRLE
            //NavigationArbreRepository.SetTagGenerationClause(codeDossier, version, type, parmClause.Letape, parmClause.NuRisque, parmClause.NuObjet, parmClause.NuFormule, parmClause.NuOption);


            //// ZBO : Test sans appel KheoBridge : génèration d eclause
            //return null;
            var result = new RetGenClauseDto();
            try
            {
                if (parmClause.Letape == "OPT")
                {
                    GenererClauses(type, codeDossier, version, parmClause);
                }
                else
                {
                    using (var serviceContext = new KheoBridge())
                    {
                        var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                        if (!string.IsNullOrEmpty(kheoBridgeUrl))
                            serviceContext.Url = kheoBridgeUrl;

                        var clauseParKheo = ObjectMapperManager.DefaultInstance.GetMapper<ParametreGenClauseDto, WSKheoBridge.KpClausePar>().Map(parmClause);

                        var clauseRetKheo = serviceContext.GenererClauses(type, codeDossier, version, clauseParKheo);

                        result = ObjectMapperManager.DefaultInstance.GetMapper<WSKheoBridge.KpClauseRet, RetGenClauseDto>().Map(clauseRetKheo);
                        var lstChoixClauses = result.ListChoixClauses.Select(
                            m => new ClauseOpChoixDto
                            {
                                Avenant = m.Avenant,
                                IdLot = m.IdLot,
                                Idunique = m.Idunique,
                                Origine = m.Origine,
                                OrigineClause = m.OrigineClause,
                                Retenue = m.Retenue,
                                Rub = m.Rub,
                                Seq = m.Seq,
                                SRub = m.SRub,
                                Version = m.Version,
                                IdClause = m.IdClause,
                                Libclause = m.Libclause
                            }).ToList();
                        result.ListChoixClauses = lstChoixClauses;
                    }
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        public string ValiderChoixClause(string type, string codeAffaire, int version, int codeAvn, int idClause, string idLot, List<ClauseOpChoixDto> lstChoixClause, string user)
        {
            using (var serviceContext = new KheoBridge())
            {
                var kheoChoixClauses = lstChoixClause.Select(
                    m => new ClauseOpChoix
                    {
                        Avenant = m.Avenant,
                        IdLot = m.IdLot,
                        Idunique = m.Idunique,
                        Origine = m.Origine,
                        OrigineClause = m.OrigineClause,
                        Retenue = m.Retenue,
                        Rub = m.Rub,
                        Seq = m.Seq,
                        SRub = m.SRub,
                        Version = m.Version,
                        IdClause = m.IdClause,
                        Libclause = m.Libclause
                    }).ToArray();

                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;
                var ret = serviceContext.ValiderChoixClause(type, codeAffaire, version, codeAvn, idLot, kheoChoixClauses, user);
                return ret.ToString();
            }
        }

        public async void GenererClauses(string type, string codeDossier, int version, ParametreGenClauseDto parmClause)
        {
            Task<RetGenClauseDto> result = GenererClausesAsyn(type, codeDossier, version, parmClause);
            RetGenClauseDto res = await result;
        }
        public async Task<RetGenClauseDto> GenererClausesAsyn(string type, string codeDossier, int version, ParametreGenClauseDto parmClause)
        {
            var result = new RetGenClauseDto();
            await Task.Run(() =>
            {
                using (var serviceContext = new KheoBridge())
                {
                    var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                    if (!string.IsNullOrEmpty(kheoBridgeUrl))
                        serviceContext.Url = kheoBridgeUrl;

                    var clauseParKheo = ObjectMapperManager.DefaultInstance.GetMapper<ParametreGenClauseDto, OP.Services.WSKheoBridge.KpClausePar>().Map(parmClause);

                    var res = serviceContext.GenererClauses(type, codeDossier, version, clauseParKheo);

                    result = ObjectMapperManager.DefaultInstance.GetMapper<OP.Services.WSKheoBridge.KpClauseRet, RetGenClauseDto>().Map(res);
                }
            });
            return result;
        }

        public string VerifierContraintesClauses(string codeOffre, string version, string type, string perimetre, string acteGestion, string etape, string risque, string objet, string formule, string option, string contexte)
        {
            var checkClause = ClauseRepository.CheckClauseLibre(codeOffre, version, type, etape, perimetre, risque, objet, formule, option);
            if (!string.IsNullOrEmpty(checkClause))
                return checkClause;

            try
            {
                using (var serviceContext = new KheoBridge())
                {
                    var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                    if (!string.IsNullOrEmpty(kheoBridgeUrl))
                        serviceContext.Url = kheoBridgeUrl;
                    string attestation = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation);
                    bool isAttestation = etape == attestation;
                    var clauseParKheo = ObjectMapperManager.DefaultInstance.GetMapper<ParametreGenClauseDto, WSKheoBridge.KpClausePar>().Map(
                            new ParametreGenClauseDto
                            {
                                ActeGestion = isAttestation ? etape : "**",
                                Letape = etape,
                                NuRisque = isAttestation ? 0 : !string.IsNullOrEmpty(risque) ? Convert.ToInt32(risque) : 0,
                                NuObjet = isAttestation ? 0 : !string.IsNullOrEmpty(objet) ? Convert.ToInt32(objet) : 0,
                                NuFormule = isAttestation ? 0 : !string.IsNullOrEmpty(formule) ? Convert.ToInt32(formule) : 0,
                                NuOption = isAttestation ? 0 : !string.IsNullOrEmpty(option) ? Convert.ToInt32(option) : 0
                            }
                        );
                    var message = serviceContext.VerifierContraintesClauses(type, codeOffre, !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0, clauseParKheo);
                    if (!string.IsNullOrEmpty(message))
                        return message;

                    clauseParKheo = ObjectMapperManager.DefaultInstance.GetMapper<ParametreGenClauseDto, WSKheoBridge.KpClausePar>().Map(
                           new ParametreGenClauseDto
                           {
                               ActeGestion = "**",
                               Letape = etape,
                               NuRisque = 0,
                               NuObjet = 0,
                               NuFormule = !string.IsNullOrEmpty(formule) ? Convert.ToInt32(formule) : 0,
                               NuOption = !string.IsNullOrEmpty(option) ? Convert.ToInt32(option) : 0,
                               LeContexte = "ANEXINDISP"
                           }
                       );

                    var errMsg = serviceContext.VerifierContraintesClauses(type, codeOffre, !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0, clauseParKheo);

                    if (string.IsNullOrEmpty(errMsg))
                        NavigationArbreRepository.SetTagGenerationClause(codeOffre, !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0, type, etape, !string.IsNullOrEmpty(risque) ? Convert.ToInt32(risque) : 0, !string.IsNullOrEmpty(objet) ? Convert.ToInt32(objet) : 0, !string.IsNullOrEmpty(formule) ? Convert.ToInt32(formule) : 0, !string.IsNullOrEmpty(option) ? Convert.ToInt32(option) : 0);

                    return errMsg;

                }
            }
            catch
            {

                return "Erreur de traitement des contraintes de clauses.";
            }
        }


        #endregion

        #region Étapes choix clause
        //public List<ParametreDto> GetListEtapes()
        //{
        //    return CommonRepository.GetParametres(string.Empty, string.Empty, "KHEOP", "ETAPE");
        //}
        public List<ParametreDto> GetListEtapes(OrigineAppel origine)
        {
            return RisqueRepository.GetListEtapes(origine);
        }

        public List<ParametreDto> GetListContextes(string codeEtape, string codeOffre, string version, string type, string codeAvn, string contexte, string famille, ModeConsultation modeNavig)
        {
            return RisqueRepository.GetListContextes(codeEtape, codeOffre, version, type, codeAvn, contexte, famille, modeNavig);
        }


        //SAB24042016: Pagination clause
        public ChoixClausesInfoDto GetInfoChoixClause(OrigineAppel origine,
                       string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
                       string codeEtape, string contexte, string famille)
        {
            return RisqueRepository.GetInfoChoixClauses(origine, type, codeOffre, version, codeAvn, etape, filtreContexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig, codeEtape, contexte, famille);
        }

        //public ChoixClausesInfoDto GetInfoChoixClause(OrigineAppel origine,
        //                string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
        //                string codeEtape, string contexte, string famille, string coltri, string triimg, int StartLine, int EndLine)
        //{
        //    return RisqueRepository.GetInfoChoixClauses(origine, type, codeOffre, version, codeAvn, etape, filtreContexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig, codeEtape, contexte, famille, coltri, triimg, StartLine,  EndLine);
        //}

        public string CheckSessionClause(int idSessionClause, string ipStation, string userAD)
        {
            using (var serviceContext = new KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;

                var pushDto = new KheoPushDto
                {
                    Fonction = PushFonction.TRAITEMENT_ACTIF,
                    Adresse_IP = ipStation,
                    UserAD = userAD,
                    ID = idSessionClause
                };

                return serviceContext.TraitementActif(pushDto);
            }
        }

        public int GetFullPathDocument(string documentFullPath, string ipStation, string userAD)
        {
            var idSessionClause = CommonRepository.GetAS400Id("IdTrt");
            using (var serviceContext = new KheoBridge())
            {
                var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
                if (!string.IsNullOrEmpty(kheoBridgeUrl))
                    serviceContext.Url = kheoBridgeUrl;

                var pushDto = new KheoPushDto
                {
                    Fonction = PushFonction.OUVRIR_DOC_EN_PDF,
                    Adresse_IP = ipStation,
                    UserAD = userAD,
                    NomFichier = documentFullPath
                };
                serviceContext.ExecuterPush(pushDto);
            }
            return idSessionClause;
        }


        #endregion


        #region Expressions Complexes

        public ListRefExprComplexeDto LoadListesExprComplexe()
        {
            return RefExpressionComplexeRepository.LoadListesExprComplexe();
            //return BLCommonOffre.LoadListesExprComplexe();
        }

        public List<ParametreDto> LoadListExprComplexe(string typeExpr)
        {
            return RefExpressionComplexeRepository.LoadListExprComplexe(typeExpr);
            //return BLCommonOffre.LoadListExprComplexe(typeExpr);
        }

        public ConditionComplexeDto GetInfoExpComplexe(string typeExpr, string codeExpr)
        {
            return RefExpressionComplexeRepository.GetInfoExprComplexe(typeExpr, codeExpr);
            //return BLCommonOffre.GetInfoExprComplexe(typeExpr, codeExpr);
        }

        public Int32 SaveDetailExpr(string idExpr, string typeExpr, string codeExpr, string libExpr, bool modifExpr, string descrExpr)
        {
            return RefExpressionComplexeRepository.SaveDetailExpr(idExpr, typeExpr, codeExpr, libExpr, modifExpr, descrExpr);
            //return BLCommonOffre.SaveDetailExpr(idExpr, typeExpr, codeExpr, libExpr, modifExpr, descrExpr);
        }

        public void DeleteExprComp(string idExpr, string typeExpr)
        {
            RefExpressionComplexeRepository.DeleteExprComp(idExpr, typeExpr);
            //BLCommonOffre.DeleteExprComp(idExpr, typeExpr);
        }

        public void SaveRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr,
            string valExpr, string unitExpr, string typeExpr, string concuValExpr, string concuUnitExpr, string concuTypeExpr,
            string valMinFrh, string valMaxFrh, string debFrh, string finFrh)
        {
            RefExpressionComplexeRepository.SaveRowExprComplexe(idExpr, typeExpComp, idRowExpr,
                         valExpr, unitExpr, typeExpr, concuValExpr, concuUnitExpr, concuTypeExpr,
                         valMinFrh, valMaxFrh, debFrh, finFrh);
            //BLCommonOffre.SaveRowExprComplexe(idExpr, typeExpComp, idRowExpr,
            // valExpr, unitExpr, typeExpr, concuValExpr, concuUnitExpr, concuTypeExpr,
            // valMinFrh, valMaxFrh, debFrh, finFrh);
        }

        public void DelRowExprComplexe(string idExpr, string typeExpComp, string idRowExpr)
        {
            RefExpressionComplexeRepository.DelRowExprComplexe(idExpr, typeExpComp, idRowExpr);
            //BLCommonOffre.DelRowExprComplexe(idExpr, typeExpComp, idRowExpr);
        }

        public ConditionComplexeDto LoadRowsExprComplexe(string typeExpr, string idExpr)
        {
            return RefExpressionComplexeRepository.LoadRowsExprComplexe(typeExpr, idExpr);
            //return BLCommonOffre.LoadRowsExprComplexe(typeExpr, idExpr);
        }

        public List<ConditionComplexeDto> LoadListExprComplexeReferentiel(string typeExpr, string codeExpr)
        {
            return RefExpressionComplexeRepository.LoadListExprComplexeReferentiel(typeExpr, codeExpr);
            //return BLCommonOffre.LoadListExprComplexeReferentiel(typeExpr, codeExpr);
        }

        public string ValidSelExprReferentiel(string codeOffre, string version, string type, string mode, string typeExpr, string idExpr, string splitCharHtml)
        {
            return RefExpressionComplexeRepository.ValidSelExprReferentiel(codeOffre, version, type, mode, typeExpr, idExpr, splitCharHtml);
            //return BLCommonOffre.ValidSelExprReferentiel(codeOffre, version, type, mode, typeExpr, idExpr, splitCharHtml);
        }

        public string DuplicateExpr(string codeOffre, string version, string type, string codeAvn, string typeExpr, string codeExpr)
        {
            return RefExpressionComplexeRepository.DuplicateExpr(codeOffre, version, type, codeAvn, typeExpr, codeExpr);
        }

        public List<ParametreDto> SearchExprComp(string typeExpr, string strSearch)
        {
            return RefExpressionComplexeRepository.SearchExprComp(typeExpr, strSearch);
        }

        #endregion

        #region Nomenclatures

        public DetailsObjetRisqueGetResultDto GetListesNomenclatureOnly(string codeOffre, string version, string type, string codeRisque, string codeObjet, string cible)
        {
            DetailsObjetRisqueGetResultDto toReturn = new DetailsObjetRisqueGetResultDto();
            //Récupération de l'ensemble des combos
            var resultCombo = RisqueRepository.GetComboNomenclatures(codeOffre, Convert.ToInt32(version), type, Convert.ToInt32(codeRisque), Convert.ToInt32(codeObjet), cible);
            if (resultCombo != null && resultCombo.Count > 0)
            {
                toReturn.Nomenclatures1 = resultCombo.FindAll(elm => elm.NumeroCombo == 1);
                toReturn.Nomenclatures2 = resultCombo.FindAll(elm => elm.NumeroCombo == 2);
                toReturn.Nomenclatures3 = resultCombo.FindAll(elm => elm.NumeroCombo == 3);
                toReturn.Nomenclatures4 = resultCombo.FindAll(elm => elm.NumeroCombo == 4);
                toReturn.Nomenclatures5 = resultCombo.FindAll(elm => elm.NumeroCombo == 5);

                if (toReturn.Nomenclatures1 != null)
                    toReturn.Nomenclatures1 = toReturn.Nomenclatures1.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures2 != null)
                    toReturn.Nomenclatures2 = toReturn.Nomenclatures2.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures3 != null)
                    toReturn.Nomenclatures3 = toReturn.Nomenclatures3.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures4 != null)
                    toReturn.Nomenclatures4 = toReturn.Nomenclatures4.OrderBy(elm => elm.OrdreNomenclature).ToList();
                if (toReturn.Nomenclatures5 != null)
                    toReturn.Nomenclatures5 = toReturn.Nomenclatures5.OrderBy(elm => elm.OrdreNomenclature).ToList();
            }
            else
            {
                toReturn.Nomenclatures1 = new List<NomenclatureDto>();
                toReturn.Nomenclatures2 = new List<NomenclatureDto>();
                toReturn.Nomenclatures3 = new List<NomenclatureDto>();
                toReturn.Nomenclatures4 = new List<NomenclatureDto>();
                toReturn.Nomenclatures5 = new List<NomenclatureDto>();
            }
            return toReturn;
        }

        public List<NomenclatureDto> GetSpecificListeNomenclature(Int64 IdNomenclatureParent, int NumeroCombo, string cible, string idNom1, string idNom2, string idNom3, string idNom4, string idNom5)
        {
            return RisqueRepository.GetSpecificListeNomenclature(IdNomenclatureParent, NumeroCombo, cible, idNom1, idNom2, idNom3, idNom4, idNom5);
        }

        #endregion

        #region Trace régularisation

        /// <summary>
        /// B3203
        /// Vérification du trace de la régularisation
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HaveTraceRegularisation(string codeContrat, string codeRisque, string version, string type, string numAvn)
        {
            var result = false;
            try
            {
                result = RisqueRepository.HaveTraceRegularisation(codeContrat, codeRisque, version, type, numAvn);
            }
            catch (Exception)
            {

                throw;
            }
            return result;


        }
        /// <summary>
        ///  B3203
        ///  Avoir dérniere version de risque
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="codeRisque"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool? GetIsRegularisation(string codeContrat, string codeRisque, string version)
        {
            bool? result = null;
            try
            {
                result = RisqueRepository.GetIsRegularisation(codeContrat, codeRisque, version);
            }
            catch (Exception)
            {

                throw;
            }
            return result;

        }
        #endregion

    }
}
