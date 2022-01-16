using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using OP.DataAccess;
using OP.WSAS400.DTO.Ecran.ModifierOffre;
using OP.WSAS400.DTO.NavigationArbre;
using OP.WSAS400.DTO.Offres;
using OPServiceContract.ISaisieCreationOffre;
using System.ServiceModel.Activation;
using static DataAccess.Helpers.OutilsHelper;

namespace OP.Services.SaisieCreationOffre
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SaisieOffre : ISaisieOffre
    {
        #region Saisie Offre

        public ModifierOffreGetResultDto ModifierOffreGet(ModifierOffreGetQueryDto query)
        {
            return ModifierOffreGetImplementation(query);
        }

        /// <summary>
        /// Saisies the offre get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="branche"></param>
        /// <param name="cible"></param>
        /// <returns></returns>
        private ModifierOffreGetResultDto ModifierOffreGetImplementation(ModifierOffreGetQueryDto query)
        {
            var toReturn = new ModifierOffreGetResultDto();
            OffrePlatDto offre = new OffrePlatDto();

            string sql = @"SELECT PBIPB CODEOFFRE, PBALX VERSIONOFFRE, PBTYP TYPEOFFRE, PBREF DESCRIPTIF, PBBRA CODEBRANCHE, 
                                KAACIBLE CODECIBLE, KAHDESC NOMCIBLE, PBICT CODECABINETCOURTAGE,PBIAS CODEASSURE,
                                PBMO1 CODEMOTSCLEF1, PBMO2 CODEMOTSCLEF2, PBMO3 CODEMOTSCLEF3, KAJOBSV OBSERVATION, PBDEV DEVISE, PBRGT CODEREGIME,
                                JDCNA SOUMISCATNAT, KAADSTA DATESTATISTIQUE, PBPER PERIODICITE,
                                CASE WHEN PBECJ > 0 AND PBECM > 0 THEN 20120000 + PBECM * 100 + PBECJ ELSE 0 END ECHEANCEPRINCIPALE,
                                (PBEFA * 10000 + PBEFM * 100 + PBEFJ) DATEEFFETGARANTIE, PBEFH DATEEFFETGARANTIEHEURE,
                                (PBFEA * 10000 + PBFEM * 100 + PBFEJ) FINEFFETGARANTIEDATE, PBFEH FINEFFETGARANTIEHEURE,
                                PBCTD DUREEGARANTIE, PBCTU UNITETEMPS,
                                JDPBN PARTBENEF, JDIND INDICEREFERENCE, JDIVA VALEUR, IFNULL(CIICI, '') CODEAPERITEUR, IFNULL(CINOM, '') NOMAPERITEUR,
                                PBNPL CODENATURECONTRAT, PBAPP PARTALBINGIA, IFNULL(PHTXF, CAST ( 0 as DECIMAL ( 5 , 2 ) ) ) FRAISAPERITION, PBPCV COUVERTURE, JDITC INTERCALAIREEXISTE,
                                PBSOU CODESOUSCRIPTEUR, UT1.UTNOM NOMSOUSCRIPTEUR, PBGES CODEGESTIONNAIRE, UT2.UTNOM NOMGESTIONNAIRE,PBETA ETATOFFRE, JDLTA LTA
                            FROM YPOBASE
                                LEFT JOIN YPRTENT ON PBIPB = JDIPB AND PBALX = JDALX
                                LEFT JOIN KPENT ON PBIPB = KAAIPB AND PBALX = KAAALX AND PBTYP = KAATYP
                                LEFT JOIN KCIBLE ON KAHCIBLE = KAACIBLE
                                LEFT JOIN YYYYPAR DEV ON DEV.TCON = 'GENER' AND DEV.TFAM = 'DEVIS' AND DEV.TCOD = PBDEV
                                LEFT JOIN KPOBSV ON KAJCHR = KAAOBSV
                                LEFT JOIN YPOCOAS ON PBIPB = PHIPB AND PBALX = PHALX
                                LEFT JOIN YCOMPA ON PHCIE = CIICI
                                LEFT JOIN YUTILIS UT1 ON PBSOU = UT1.UTIUT
                                LEFT JOIN YUTILIS UT2 ON PBGES = UT2.UTIUT
                                WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";
            var parameters = MakeParams(sql, query.CodeOffre.ToIPB(), query.Version, query.Type);
            toReturn.Offre = PoliceRepository.GetOffre(PoliceRepository.GetOffreInfosGen(sql, query, parameters));


            toReturn.MotsCles = PoliceRepository.ObtenirMotClef(toReturn.Offre.Branche.Code, toReturn.Offre.Branche.Cible.Code);
            toReturn.Devises = CommonRepository.GetParametres(toReturn.Offre.Branche.Code, toReturn.Offre.Branche.Cible.Code, "GENER", "DEVIS");
            toReturn.Periodicites = CommonRepository.GetParametres(toReturn.Offre.Branche.Code, toReturn.Offre.Branche.Cible.Code, "PRODU", "PBPER");
            toReturn.Indices = CommonRepository.GetParametres(toReturn.Offre.Branche.Code, toReturn.Offre.Branche.Cible.Code, "GENER", "INDIC");
            toReturn.NaturesContrat = CommonRepository.GetParametres(toReturn.Offre.Branche.Code, toReturn.Offre.Branche.Cible.Code, "PRODU", "PBNPL");
            toReturn.Durees = CommonRepository.GetParametres(toReturn.Offre.Branche.Code, toReturn.Offre.Branche.Cible.Code, "PRODU", "PBCTU");
            toReturn.RegimesTaxe = CommonRepository.GetParametres(toReturn.Offre.Branche.Code, toReturn.Offre.Branche.Cible.Code, "GENER", "TAXRG");
            toReturn.Antecedents = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBANT");
            toReturn.Stops = CommonRepository.GetParametres(string.Empty, string.Empty, "PRODU", "PBSTP");
            return toReturn;
        }

        public string ModifierOffreSet(ModifierOffreSetQueryDto query, string utilisateur)
        {
            string msgRetour = string.Empty;

            #region Vérification & Préparation des paramètres



            if (query == null || query.Offre == null)
                return "Erreur lors de l'enregistrement, référence nulle.";

            var offre = query.Offre;
            msgRetour = ControleSousGest(offre, utilisateur);
            if (!string.IsNullOrEmpty(msgRetour)) return msgRetour;

            #endregion

            msgRetour = PoliceRepository.SauvegarderModifierOffre(query.Offre, utilisateur);


            #region Arbre de navigation
            NavigationArbreRepository.SetTraceArbre(new TraceDto
            {
                CodeOffre = query.Offre.CodeOffre.ToIPB(),
                Version = query.Offre.Version.Value,
                Type = query.Offre.Type,
                EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                NumeroOrdreDansEtape = 10,
                NumeroOrdreEtape = 1,
                Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale),
                Risque = 0,
                Objet = 0,
                IdInventaire = 0,
                Formule = 0,
                Option = 0,
                Niveau = string.Empty,
                CreationUser = utilisateur,
                PassageTag = "O",
                PassageTagClause = "N"
            });
            #endregion

            //#region Acte de gestion
            //CommonRepository.AjouterActeGestion(query.Offre.CodeOffre, query.Offre.Version.ToString(), query.Offre.Type, 0, AlbConstantesMetiers.ACTEGESTION_GESTION, AlbConstantesMetiers.TRAITEMENT_OFFRE, "", utilisateur);
            //#endregion

            return msgRetour;
        }
        public string ControleSousGest(OffreDto offre, string utilisateur)
        {
            string MessageErr = string.Empty;


            if (offre.Gestionnaire != null && !string.IsNullOrEmpty(offre.Gestionnaire.Id) && !GestionnaireRepository.TesterExistenceGestionnaire(offre.Gestionnaire.Id))
            {

                MessageErr = "Code gestionnaire inconnu";
            }
            if (offre.Souscripteur != null && !string.IsNullOrEmpty(offre.Souscripteur.Code))
            {
                if (!SouscripteurRepository.TesterExistenceSouscripteur(offre.Souscripteur.Code))
                {

                    MessageErr = "Code souscripteur inconnu";
                }
            }
            return MessageErr;
        }

        #endregion
    }
}
