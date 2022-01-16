using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;

namespace OP.Services.SaisieCreationOffre
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RechercheSaisie : IRechercheSaisie {
        private static readonly Dictionary<AlbConstantesMetiers.TypeDateRecherche, string> dateFieldNames = new Dictionary<AlbConstantesMetiers.TypeDateRecherche, string> {
            [AlbConstantesMetiers.TypeDateRecherche.Saisie] = "$datesaisie",
            [AlbConstantesMetiers.TypeDateRecherche.Creation] = "$datecreation",
            [AlbConstantesMetiers.TypeDateRecherche.MAJ] = "$datemaj",
            [AlbConstantesMetiers.TypeDateRecherche.Effet] = "$dateeffet",
        };

        #region Recherche Saisie
        public string DernierNumeroVersionOffreMotifSituation(string codeOffre, string type, string version) {
            return RechercheRepository.DernierNumeroVersionOffreMotifSituation(codeOffre, type, version);
        }
        public bool CreationNouvelleVersionOffre(string codeOffre, string version, string type, string utilisateur, string traitement) {
            return TraitementVarianteOffreRepository.CreationNouvelleVersionOffre(codeOffre, version, type, utilisateur, traitement);
        }

        public RechercheSaisieGetResultDto RechercheSaisieGet(RechercheSaisieGetQueryDto query) {
            var toReturn = new RechercheSaisieGetResultDto {
                Branches =
                    CommonRepository.GetParametres(string.Empty, string.Empty, "GENER", "BRCHE", tCod: new List<string> { "PP", "ZZ" }, notIn: true, tPcn2: "1"),
                Etats = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBETA"),
                Situation = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBSIT"),
                ListRefus = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBSTF"),
                Cibles = BrancheRepository.GetCibles(string.Empty, true, query.IsAdmin, query.IsUserHorse),
                Qualites = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBSTQ")
            };

            //toReturn.Branches = SaisiOffreEntities.BrancheRepo.ObtenirBranches().Select(x => x.Code).ToList();

            return toReturn;
        }

        public string GetEtatOffre(string codeOffre, string version, string type) {
            if (RechercheRepository.IsOffreCitrix(codeOffre, version, type)) {
                return "C";
            }

            return RechercheRepository.GetEtatOffre(codeOffre, version, type);
        }



        public string CheckOffreCreate(string codeOffreCopy, string versionCopy, string type) {
            var toReturn = string.Empty;
            var countOffre = RechercheRepository.GetCountOffreByCode(codeOffreCopy, versionCopy, type);

            if (countOffre <= 0) {
                if (type == AlbConstantesMetiers.TYPE_OFFRE) {
                    toReturn = "Offre inexistante.";
                }

                if (type == AlbConstantesMetiers.TYPE_CONTRAT) {
                    toReturn = "Contrat inexistant.";
                }
            }
            else {
                var branche = RechercheRepository.GetBrancheOffreByCode(codeOffreCopy, versionCopy, type);
                var cible = RechercheRepository.GetCibleOffreByCode(codeOffreCopy, versionCopy, type);

                if ((string.IsNullOrEmpty(branche) || string.IsNullOrEmpty(cible)) && (type == AlbConstantesMetiers.TYPE_OFFRE)) {
                    toReturn = "Cette offre ne peut être copiée : branche ou cible non trouvée(s).";
                }

                if ((string.IsNullOrEmpty(branche) || string.IsNullOrEmpty(cible)) && (type == AlbConstantesMetiers.TYPE_CONTRAT)) {
                    toReturn = "Ce contrat ne peut être copiée : branche ou cible non trouvée(s).";
                }
            }

            return toReturn;
        }

        public string CheckOffreCopy(string codeOffre, string version, string codeOffreCopy, string versionCopy, string type) {
            var toReturn = string.Empty;
            var countOffre = RechercheRepository.GetCountOffreByCode(codeOffreCopy, versionCopy, type);

            if (countOffre <= 0) {
                toReturn = "Offre inexistante.";
            }
            else {
                var branche = RechercheRepository.CheckBrancheOffres(codeOffre, version, codeOffreCopy, versionCopy, type);
                var cible = RechercheRepository.CheckCibleOffres(codeOffre, version, codeOffreCopy, versionCopy, type);

                if (!branche || !cible) {
                    toReturn = "Cette offre ne peut être copiée : branche ou cible incompatible(s).";
                }
            }

            return toReturn;
        }

        public string GetOffreMere(string param) {
            var tParam = param.Split('_');
            return RechercheRepository.GetOffreMere(tParam[0], tParam[1]);
        }

        public RechercheOffresGetResultDto RechercherOffresContrat(ModeleParametresRechercheDto paramRecherche, ModeConsultation modeNavig) {
            var toReturn = new RechercheOffresGetResultDto();

            //using (var kBridge1 = new KheoBridge())
            //{
            //    var kheoBridgeUrl = ConfigurationManager.AppSettings["KheoBridgeUrl"];
            //    if (!string.IsNullOrEmpty(kheoBridgeUrl))
            //        kBridge1.Url = kheoBridgeUrl;
            List<RechPolDto> listCriteres = MakeCriteria(paramRecherche);
            var lpol = RechercheRepository.RecherchePolices(listCriteres, modeNavig.AsCode());

            for (int i = 0; i < lpol.Count; i++) {
                OffreRechPlatDto pol = lpol[i];
                toReturn.LstOffres.Add(new OffreDto {
                    CodeOffre = pol.CodeOffre,
                    Version = pol.Version,
                    Type = pol.Type,
                    NumAvenant = pol.CodeAvn,
                    DateSaisie = AlbConvert.ConvertIntToDateHour(pol.DateSaisie),
                    Branche = new BrancheDto { Code = pol.CodeBranche, Nom = pol.LibBranche, Cible = new CibleDto { Code = pol.CodeCible, Nom = pol.LibCible } },
                    Etat = pol.CodeEtat,
                    CodeSousBranche = pol.CodeSousBranche,
                    EtatLib = pol.LibEtat,
                    Situation = pol.CodeSit,
                    SituationLib = pol.LibSit,
                    Qualite = pol.CodeQualite,
                    QualiteLib = pol.LibQualite,
                    Descriptif = pol.Descriptif,
                    CodeCategorie = pol.CodeCategorie,
                    PreneurAssurance = new AssureDto {
                        Code = pol.CodeAss.ToString(),
                        NomAssure = pol.NomAss,
                        Adresse = new AdressePlatDto {
                            // CodePostal = cpAssur,
                            NomVille = pol.VilleAss,
                            CodePostalString = pol.CpAss
                        }
                    },
                    CabinetGestionnaire = new CabinetCourtageDto {
                        Code = pol.CodeCourt,
                        NomCabinet = pol.NomCourt,
                        Adresse = new AdressePlatDto {
                            //CodePostal = cpCourt,
                            NomVille = pol.VilleCourt,
                            CodePostalString = pol.CpCourt
                        }
                    },
                    TypeAvt = pol.TypeTraitement,
                    TypeAccord = pol.TypeAccord,
                    KheopsStatut = pol.StatutKheops,
                    NumAvnExterne = pol.AvnExt,
                    GenerDoc = pol.TrtLot == "W" ? 1 : 0,
                    MotifRefus = pol.MotifRefus.Trim() != "-" ? pol.MotifRefus : string.Empty,
                    Periodicite = new ParametreDto { Code = pol.CodePeriodicite },
                    ContratMere = pol.TypePolice,
                    DateFinEffetGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(pol.DateFinEffet)),
                    DateEffetGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(pol.DateEffet)),
                    DateCreation = AlbConvert.ConvertIntToDate(Convert.ToInt32(pol.DateCreation)),
                    DateMAJ = AlbConvert.ConvertIntToDate(Convert.ToInt32(pol.DateMaj / 10000)),
                    HeureFin = pol.HeureFinEffet < 2400 ? AlbConvert.ConvertIntToTimeMinute(pol.HeureFinEffet) : null,
                    HasSusp = pol.HasSusp > 0,
                    DateFinSusp = AlbConvert.ConvertIntToDate(pol.DtFinSusp),
                    HasDoubleSaisie = pol.DoubleSaisie == "O",
                    RegulId = pol.MaxIdRegul
                });
            }

            toReturn.NbCount = toReturn.LstOffres.Count;

            return toReturn;
        }

        private static List<RechPolDto> MakeCriteria(ModeleParametresRechercheDto paramRecherche) {
            var listCriteres = new List<RechPolDto>();

            #region Alimentation des paramètres de recherche

            if (!string.IsNullOrEmpty(paramRecherche.CodeOffre)) {
                listCriteres.Add(new RechPolDto<string> {
                    Champ = nameof(OffreRechPlatDto.CodeOffre)
                    ,
                    Operateur = "LIKE",
                    Expression = "%" + paramRecherche.CodeOffre.Trim() + "%"
                });
                if (!string.IsNullOrEmpty(paramRecherche.NumAliment)) {
                    listCriteres.Add(new RechPolDto<int> {
                        Champ = nameof(OffreRechPlatDto.Version),
                        Expression = int.TryParse(paramRecherche.NumAliment, out int alx) ? alx : 0
                    });
                }
                if (!string.IsNullOrEmpty(paramRecherche.Type)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.Type), Expression = paramRecherche.Type });
                }
            }
            else {
                if (paramRecherche.ExcludedCodeOffres?.Any() ?? false) {
                    listCriteres.Add(new RechPolDto<string[]> {
                        Champ = nameof(OffreRechPlatDto.CodeOffreVersion),
                        Operateur = "NOT IN",
                        Expression = paramRecherche.ExcludedCodeOffres.Select(x => $"{x.ipb.Trim()},{x.alx}").ToArray()
                    });
                }
                if (!string.IsNullOrEmpty(paramRecherche.Type)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.Type), Expression = paramRecherche.Type });
                }

                if (!string.IsNullOrEmpty(paramRecherche.Branche)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.CodeBranche), Expression = paramRecherche.Branche });
                }

                if (!string.IsNullOrEmpty(paramRecherche.Cible)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.CodeCible), Expression = paramRecherche.Cible });
                }

                listCriteres.Add(new GroupingRechPolDto("("));

                if (paramRecherche.CabinetCourtageId > 0) {
                    if (paramRecherche.CabinetCourtageIsApporteur) {
                        listCriteres.Add(new RechPolDto<long> { Champ = nameof(OffreRechPlatDto.CodeCourt), Liaison = "OR", Expression = paramRecherche.CabinetCourtageId });
                    }

                    if (paramRecherche.CabinetCourtageIsGestionnaire) {
                        listCriteres.Add(new RechPolDto<long> { Champ = nameof(OffreRechPlatDto.CodeCourtierApporteur), Liaison = "OR", Expression = paramRecherche.CabinetCourtageId });
                    }
                }

                listCriteres.Add(new GroupingRechPolDto(")"));

                listCriteres.AddRange(MakeCriteriaPreneur(paramRecherche));

                if (int.TryParse(paramRecherche.AdresseRisqueCP, out var cpCodeRisque)) {
                    if (!string.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5)) {
                        listCriteres.Add(new RechPolDto<long> { Champ = "ABPCP6", Expression = cpCodeRisque % 1000 });
                    }

                    if (!string.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && (paramRecherche.AdresseRisqueCP.Length == 5)) {
                        listCriteres.Add(new RechPolDto<string> { Champ = "ABPDP6", Expression = (cpCodeRisque / 1000).ToString("00") });
                    }
                    else if (!string.IsNullOrEmpty(paramRecherche.AdresseRisqueCP)) {
                        listCriteres.Add(new RechPolDto<string> { Champ = "ABPDP6", Expression = cpCodeRisque.ToString("00") });
                    }
                } else if (!string.IsNullOrEmpty(paramRecherche.AdresseRisqueCP) && paramRecherche.AdresseRisqueCP.Length < 3)
                {
                    listCriteres.Add(new RechPolDto<string> { Champ = "ABPDP6", Expression = paramRecherche.AdresseRisqueCP.ToUpper() });

                }
                if (!string.IsNullOrEmpty(paramRecherche.AdresseRisqueVille)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = "ABPVI6", Operateur = "LIKE", Expression = "%" + paramRecherche.AdresseRisqueVille + "%" });
                }
                if (!string.IsNullOrEmpty(paramRecherche.AdresseRisqueVoie)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = "$ADRPRINCIPALE", Operateur = "LIKE", Expression = "%" + paramRecherche.AdresseRisqueVoie + "%" });

                }

                if (!string.IsNullOrEmpty(paramRecherche.SouscripteurCode)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = "PBSOU", Expression = paramRecherche.SouscripteurCode });
                }

                if (!string.IsNullOrEmpty(paramRecherche.GestionnaireNom)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = "PBGES", Expression = paramRecherche.GestionnaireCode });
                }

                if (!string.IsNullOrEmpty(paramRecherche.Etat)) {
                    var op = paramRecherche.SaufEtat ? "<>" : "=";
                    listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.CodeEtat), Operateur = op, Expression = paramRecherche.Etat });
                }
                if (!string.IsNullOrEmpty(paramRecherche.Situation)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.CodeSit), Expression = paramRecherche.Situation });
                }
                else if (paramRecherche.IsActif ^ paramRecherche.IsInactif) {
                    string op = paramRecherche.IsActif ? "NOT IN" : "IN";
                    listCriteres.Add(new RechPolDto<IEnumerable<string>> { Champ = "PBSIT", Operateur = op, Expression = new[] { "X", "W", "N" } });
                }

                AddRechercheDate(listCriteres, paramRecherche);

                if (!string.IsNullOrEmpty(paramRecherche.MotsClefs)) {
                    listCriteres.Add(new RechPolDto<string> { Champ = "$motscle", Operateur = "LIKE", Expression = "%" + paramRecherche.MotsClefs + "%" });
                }
            }
            
            if (!string.IsNullOrEmpty(paramRecherche.SortingName)) {
                if (paramRecherche.SortingName.Trim().ToLower() == "datesaisie" || paramRecherche.SortingName.Trim().ToLower() == "date") {
                    paramRecherche.SortingName = "datesaisie";
                }
                listCriteres.Add(new RechPolDto<string> { Champ = "$orderby", Expression = paramRecherche.SortingName + " " + paramRecherche.SortingOrder });
            }

            #endregion
            return listCriteres;
        }

        private static List<RechPolDto> MakeCriteriaPreneur(ModeleParametresRechercheDto paramRecherche) {
            var listCriteres = new List<RechPolDto>();

            listCriteres.Add(new GroupingRechPolDto("("));
            if (paramRecherche.PreneurAssuranceCode > 0) {
                listCriteres.Add(new RechPolDto<long> { Champ = nameof(OffreRechPlatDto.CodeAss), Liaison = "OR", Expression = paramRecherche.PreneurAssuranceCode });
            }

            if (paramRecherche.PreneurAssuranceSIREN > 0) {
                listCriteres.Add(new RechPolDto<long> { Champ = "ASSIR", Liaison = "OR", Expression = paramRecherche.PreneurAssuranceSIREN });
            }


            if (!string.IsNullOrEmpty(paramRecherche.PreneurAssuranceNom))
            {
                listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.NomAss), Liaison = "OR", Operateur = "LIKE", Expression = "%" + paramRecherche.PreneurAssuranceNom + "%" });
            }

            if (int.TryParse(paramRecherche.PreneurAssuranceCP, out var cpCode))
            {
                if (!string.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP) && (paramRecherche.PreneurAssuranceCP.Length == 5))
                {
                    listCriteres.Add(new GroupingRechPolDto("(") { Liaison = "OR" });
                    listCriteres.Add(new RechPolDto<string> { Champ = "ASCPO", Expression = int.Parse(paramRecherche.PreneurAssuranceCP.Substring(2, 3)).ToString("000") });
                    listCriteres.Add(new RechPolDto<string> { Champ = "ASDEP", Expression = int.Parse(paramRecherche.PreneurAssuranceCP.Substring(0, 2)).ToString("00") });
                    listCriteres.Add(new GroupingRechPolDto(")"));
                }
                else if (!string.IsNullOrEmpty(paramRecherche.PreneurAssuranceCP))
                {
                    listCriteres.Add(new RechPolDto<string> { Champ = "ASDEP", Liaison = "OR", Expression = int.Parse(paramRecherche.PreneurAssuranceCP).ToString("00") });
                }
            } else if (!string.IsNullOrWhiteSpace( paramRecherche.PreneurAssuranceCP) && paramRecherche.PreneurAssuranceCP.Length < 3){
                listCriteres.Add(new RechPolDto<string> { Champ = "ASDEP", Expression = paramRecherche.PreneurAssuranceCP.ToUpper() });
            }
            //if (!string.IsNullOrEmpty(paramRecherche.PreneurAssuranceVille))
            //    listCriteres.Add(new RechPolDto<string> { Champ = nameof(OffreRechPlatDto.VilleAss), Operateur = "LIKE", Expression = "%" + paramRecherche.PreneurAssuranceVille + "%" });

            listCriteres.Add(new GroupingRechPolDto(")"));
            return listCriteres;
        }


        private static void AddRechercheDate(List<RechPolDto> listCriteres, ModeleParametresRechercheDto paramRecherche) {
            if (!dateFieldNames.ContainsKey(paramRecherche.TypeDateRecherche)) {
                return;
            }
            var fieldName = dateFieldNames[paramRecherche.TypeDateRecherche];

            RechPolDto dto = MakeRechercheDate(paramRecherche, fieldName);

            if (dto != default(RechPolDto)) {
                listCriteres.Add(dto);
            }
        }



        private static RechPolDto MakeRechercheDate(ModeleParametresRechercheDto paramRecherche, string fieldName) {
            DateTime? dateDebut = paramRecherche.DDateDebut;
            DateTime? dateFin = paramRecherche.DDateFin;
            var dto = default(RechPolDto);
            if (dateDebut.HasValue && dateFin.HasValue) {
                dto = new RechPolDto<(long, long)> {
                    Champ = fieldName,
                    Operateur = "ENTRE",
                    Expression = ((long)AlbConvert.ConvertDateToInt(dateDebut).Value, (long)AlbConvert.ConvertDateToInt(dateFin).Value)
                };
            }

            if (dateDebut.HasValue && !dateFin.HasValue) {
                dto = new RechPolDto<long> {
                    Champ = fieldName,
                    Operateur = ">=",
                    Expression = AlbConvert.ConvertDateToInt(dateDebut).Value
                };
            }

            if (!dateDebut.HasValue && dateFin.HasValue) {
                dto = new RechPolDto<long> {
                    Champ = fieldName,
                    Operateur = "<=",
                    Expression = AlbConvert.ConvertDateToInt(dateFin).Value
                };
            }

            return dto;
        }

        public string GetTypeAvenant(string paramOffre) {
            return RechercheRepository.GetTypeAvenant(paramOffre);
        }

        public void ConfirmReprise(string codeOffre, string version, string type, string codeAvt, string typeAvt, string user) {
            RechercheRepository.ConfirmReprise(codeOffre, version, type, codeAvt, typeAvt, user);
        }

        public bool GetHasPrimeSoldee(string codeAffaire, string version, string type, string codeAvn) {
            return RechercheRepository.GetHasPrimeSoldee(codeAffaire, Convert.ToInt32(version), type, Convert.ToInt32(codeAvn));
        }

        public string CheckPrimeAvt(string codeContrat, string version, string type, string codeAvn) {
            return RechercheRepository.CheckPrimeAvt(codeContrat, version, type, codeAvn);
        }

        public long GetNumRegule(string codeAffaire, string version, string type, string codeAvn) {
            return RegularisationRepository.GetNumRegule(codeAffaire, version, type, codeAvn);
        }

        public BlocageSaisieDto GetBlocageSaisieAS400()
        {
            return CommonRepository.GetBlocageSaisieAS400();
        }

        #endregion
    }
}
