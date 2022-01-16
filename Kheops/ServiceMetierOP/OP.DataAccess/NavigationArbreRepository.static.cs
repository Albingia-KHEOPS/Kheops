using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.NavigationArbre;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess
{
    public partial class NavigationArbreRepository : RepositoryBase
    {
        #region Méthodes publiques

        /// <summary>
        /// Récupère l'arbre de navigation pour une offre
        /// </summary>
        public static NavigationArbreDto GetHierarchy(string codeOffre, int version, string type, string codeAvn, ModeConsultation modeNavig, string acteGestionRegule)
        {
            codeOffre = codeOffre.PadLeft(9, ' ');

            long numInterne = 0;
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sqlInterne = @"SELECT PBAVN INT32RETURNCOL FROM YPOBASE WHERE PBIPB = :codeContrat AND PBALX = :version AND PBTYP = :type";
            var resultInterne = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInterne, param);
            if (resultInterne != null && resultInterne.Any())
            {
                numInterne = resultInterne.FirstOrDefault().Int32ReturnCol;
            }
            if (numInterne.ToString() != codeAvn)
            {
                modeNavig = ModeConsultation.Historique;
            }
            else
            {
                modeNavig = ModeConsultation.Standard;
            }


            //Récupérations des traces
            List<ArbreDto> arbres = GetArbre(codeOffre, version, type, codeAvn, modeNavig);
            var navig = new NavigationArbreDto
            {
                CodeOffre = codeOffre,
                Version = version,
                Type = type
            };

            if (arbres?.Any() ?? false)
            {
                var firstArbre = arbres.First();
                navig.InformationsSaisie = true;
                navig.TagSaisie = GetTagInArbre(arbres, codeOffre, version, type, "SAISIE");
                navig.OffreIdentification = firstArbre.DescOffre.Trim();
                navig.NumAvn = firstArbre.NumAvn;
                navig.InformationsGenerales = ExisteInArbre(arbres, codeOffre, version, type, "GEN");
                navig.TagInfoGen = GetTagInArbre(arbres, codeOffre, version, type, "GEN");
                navig.MatriceRisques = ExisteInArbre(arbres, codeOffre, version, type, "RSQ");
                navig.TagMatriceRisques = GetTagInArbre(arbres, codeOffre, version, type, "RSQ");
                navig.MatriceFormules = ExisteInArbre(arbres, codeOffre, version, type, "OPT");
                navig.TagMatriceFormules = GetTagInArbre(arbres, codeOffre, version, type, "OPT");
                navig.MatriceGaranties = ExisteInArbre(arbres, codeOffre, version, type, "OPT");
                navig.TagMatriceGaranties = GetTagInArbre(arbres, codeOffre, version, type, "OPT");
                navig.Risques = GetRisquesHierarchy(arbres, codeOffre, version, type, codeAvn, modeNavig);
                navig.Engagement = ExisteInArbre(arbres, codeOffre, version, type, "ENG");
                navig.TagEngagement = GetTagInArbre(arbres, codeOffre, version, type, "ENG");
                navig.Evenement = ExisteInArbre(arbres, codeOffre, version, type, "EVN");
                navig.TagEvenement = GetTagInArbre(arbres, codeOffre, version, type, "EVN");
                navig.MontantRef = ExisteInArbre(arbres, codeOffre, version, type, "ATT");
                navig.TagMontantRef = GetTagInArbre(arbres, codeOffre, version, type, "ATT");
                navig.Cotisation = ExisteInArbre(arbres, codeOffre, version, type, "COT");
                navig.TagCotisation = GetTagInArbre(arbres, codeOffre, version, type, "COT");
                navig.Fin = ExisteInArbre(arbres, codeOffre, version, type, "FIN");
                navig.TagFin = GetTagInArbre(arbres, codeOffre, version, type, "FIN");

                navig.CoAssureurs = AffaireNouvelleRepository.ExistCoAs(codeOffre, version.ToString(), type, modeNavig);
                if (modeNavig == ModeConsultation.Historique) {
                    navig.CoCourtiers = CommonRepository.ExistRow(string.Format("SELECT COUNT(*) NBLIGN FROM YHPCOUR WHERE PFIPB = '{0}' AND PFALX = {1}", codeOffre, version));
                }
                else {
                    navig.CoCourtiers = CommonRepository.ExistRow(string.Format("SELECT COUNT(*) NBLIGN FROM YPOCOUR WHERE PFTYP = '{0}' AND PFIPB = '{1}' AND PFALX = {2}", type, codeOffre, version));
                }
            }

            return navig;
        }

        public static void SetTraceArbre(TraceDto trace)
        {
            DateTime dateNow = DateTime.Now;

            DbParameter[] param = new DbParameter[18];
            param[0] = new EacParameter("P_TYPE", trace.Type);
            param[1] = new EacParameter("P_CODEOFFRE", trace.CodeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("P_VERSION", 0);
            param[2].Value = trace.Version;
            param[3] = new EacParameter("P_ETAPE", 0);
            param[3].Value = trace.EtapeGeneration;
            param[4] = new EacParameter("P_NUMETAPE", 0);
            param[4].Value = trace.NumeroOrdreDansEtape;
            param[5] = new EacParameter("P_ORDRE", 0);
            param[5].Value = trace.NumeroOrdreEtape;
            param[6] = new EacParameter("P_PERIMETRE", trace.Perimetre);
            param[7] = new EacParameter("P_CODERSQ", 0);
            param[7].Value = trace.Risque;
            param[8] = new EacParameter("P_CODEOBJ", 0);
            param[8].Value = trace.Objet;
            param[9] = new EacParameter("P_CODEINVEN", 0);
            param[9].Value = trace.IdInventaire;
            param[10] = new EacParameter("P_CODEFOR", 0);
            param[10].Value = trace.Formule;
            param[11] = new EacParameter("P_CODEOPT", 0);
            param[11].Value = trace.Option;
            param[12] = new EacParameter("P_NIVCLSST", trace.Niveau);
            param[13] = new EacParameter("P_USER", trace.CreationUser);
            param[14] = new EacParameter("P_DATESYSTEME", 0);
            param[14].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[15] = new EacParameter("P_HEURESYSTEME", 0);
            param[15].Value = AlbConvert.ConvertTimeToInt(AlbConvert.GetTimeFromDate(dateNow));
            param[16] = new EacParameter("P_PASSTAG", trace.PassageTag);
            param[17] = new EacParameter("P_PASSTAGCLAUSE", trace.PassageTagClause);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ARBRESV", param);
        }

        public static void DelTraceArbre(TraceDto trace)
        {
            string sql = string.Format(@"DELETE FROM KPCTRLE WHERE KEVTYP = '{0}' AND KEVIPB = '{1}' AND KEVALX = {2}",
                                trace.Type, trace.CodeOffre.PadLeft(9, ' '), trace.Version);

            if (trace.EtapeGeneration == "RSQ")
                sql = string.Format("{0} AND KEVRSQ = {1}", sql, trace.Risque);
            if (trace.EtapeGeneration == "OBJ")
                sql = string.Format("{0} AND KEVRSQ = {1} AND KEVOBJ = {2}", sql, trace.Risque, trace.Objet);
            if (trace.EtapeGeneration == "FOR")
                sql = string.Format("{0} AND KEVFOR = {1}", sql, trace.Formule);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static void SetTagGenerationClause(string code, int version, string type, string etape, int risque, int objet, int formule, int option)
        {
            //Gestion de l'étape base
            if (etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Saisie))
                etape = "GEN";
            //if (etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option))
            //    etape = "GAR";
            string sql = string.Format(@"UPDATE KPCTRLE SET KEVTAGC = 'O'
                                         WHERE KEVTYP = '{0}' AND KEVIPB = '{1}' AND KEVALX = {2} 
                                         AND KEVETAPE = '{3}' {4} 
                                         AND KEVFOR = {5} AND KEVOPT = {6}",
                            type, code.PadLeft(9, ' '), version, etape,
                            etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option) ? string.Format(" AND KEVRSQ = {0} AND KEVOBJ = {1}", risque, objet) : string.Empty, 
                            formule, option);
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Récupère la liste des pages
        /// présentes dans KPCTRLE pour une offre donnée
        /// pour afficher la navigation
        /// </summary>
        private static List<ArbreDto> GetArbre(string codeOffre, int version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            //DbParameter[] param = new DbParameter[3];
            //if (modeNavig == ModeConsultation.Historique)
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("P_CODEOFFRE", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = version;
            param[2] = new EacParameter("P_TYPE", type);

            param[3] = new EacParameter("P_CODEAVN", 0);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            if (modeNavig == ModeConsultation.Historique)
                return DbBase.Settings.ExecuteList<ArbreDto>(CommandType.StoredProcedure, "SP_ARBREHIST", param);

            return DbBase.Settings.ExecuteList<ArbreDto>(CommandType.StoredProcedure, "SP_ARBRE", param);


        }

        private static bool IsFormuleSortie(ArbreDto form)
        {
            var param = new List<DbParameter> {
                new EacParameter("P_CODEOFFRE", form.CodeOffre.PadLeft(9, ' ')),
                new EacParameter("P_CODERSQ", form.CodeRsq),
                new EacParameter("P_CODEFOR", form.CodeFor)
            }; ;

            string query = @"select MAX(JGVFA * 10000 + JGVFM * 100 + JGVFJ) DATEFINRSQ from kpoptap
                            inner join yprtobj on jgipb = kddipb and jgrsq = kddrsq and jgobj = kddobj
                            where kddipb = :P_CODEOFFRE AND kddrsq = :P_CODERSQ AND kddfor = :P_CODEFOR";

            var res = DbBase.Settings.ExecuteList<ArbreDto>(CommandType.Text, query, param).FirstOrDefault();

            return res.DateFinRsq != 0 && res.DateFinRsq < form.DateDebAvn; // la formule s'applique à l'objet              
//            return form.DateFinRsq < form.DateDebAvn;
        }

        /// <summary>
        /// Récupère la liste des risques 
        /// avec les formules liées
        /// pour la navigation
        /// </summary>
        private static List<RisqueDto> GetRisquesHierarchy(List<ArbreDto> arbres, string codeOffre, int version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            List<RisqueDto> risques = new List<RisqueDto>();
            var lstRsq = arbres.FindAll(a => a.CodeRsq != 0 && a.Perimetre == "RSQ").GroupBy(a => a.CodeRsq).Select(a => a.First());
            if (lstRsq != null)
            {
                foreach (var rsq in lstRsq)
                {
                    List<FormuleDto> formules = new List<FormuleDto>();
                    var lstFor = arbres.FindAll(a => a.CodeRsq == rsq.CodeRsq && a.CodeFor != 0).GroupBy(a => a.CodeFor).Select(a => a.First());
                    if (lstFor != null)
                    {
                        foreach (var form in lstFor)
                        {
                            List<OptionDto> options = new List<OptionDto>();
                            OptionDto option = new OptionDto();
                            if (form.CodeFormuleAvt != 0 || modeNavig == ModeConsultation.Historique)
                            {
                                var lstOpt = arbres.FindAll(a => a.CodeRsq == form.CodeRsq && a.CodeFor == form.CodeFor && a.CodeOpt != 0).GroupBy(a => a.CodeOpt).Select(a => a.First());
                                if (lstOpt != null)
                                {
                                    //var isformulesortie = IsFormuleSortie(form);
                                    foreach (var opt in lstOpt)
                                    {
                                        if (opt.CodeOpt == 1)
                                        {
                                            option = new OptionDto
                                            {
                                                Formule = Convert.ToInt32(opt.CodeFor),
                                                Option = Convert.ToInt32(opt.CodeOpt),
                                                Description = opt.DescFor.Trim(),
                                                Risque = Convert.ToInt32(opt.CodeRsq),
                                                TagOption = opt.PassageTag
                                            };
                                        }

                                        options.Add(new OptionDto
                                        {
                                            Formule = Convert.ToInt32(opt.CodeFor),
                                            Option = Convert.ToInt32(opt.CodeOpt),
                                            Description = opt.DescFor.Trim(),
                                            Risque = Convert.ToInt32(opt.CodeRsq),
                                            TagOption = opt.PassageTag
                                        });
                                    }
                                }
                            }
                            if (!IsFormuleSortie(form))
                                formules.Add(new FormuleDto
                                {
                                    Alpha = form.LettreFor.Trim(),
                                    CodeFormule = Convert.ToInt32(form.CodeFor),
                                    CodeOption = Convert.ToInt32(form.CodeOpt),
                                    Option = option,
                                    Options = options,
                                    TagFormule = form.PassageTag,
                                    CreateModifAvn = int.TryParse(codeAvn, out int i) && i > 0 && (form.CreateAvt == i || form.ModifAvt == i)
                                });
                        }
                    }
                    risques.Add(new RisqueDto
                    {
                        Code = Convert.ToInt32(rsq.CodeRsq),
                        Descriptif = rsq.DescRsq.Trim(),
                        Numero = Convert.ToInt32(rsq.ChronoRsq),
                        Formules = formules,
                        TagRisque = rsq.PassageTag,
                        isBadDate = false//rsq.DateFinRsq == 0 ? false : rsq.DateFinRsq < rsq.DateDebAvn
                    });
                }
            }

            return risques;
        }

        /// <summary>
        /// Vérifie l'existance de l'étape
        /// dans l'arbre chargé pour l'offre
        /// </summary>
        private static bool ExisteInArbre(List<ArbreDto> arbres, string codeOffre, int version, string type, string etape, string acteGestionRegule = null)
        {
            ArbreDto arbre = null;
            if (etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation))
            {
                if (acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_REGUL)
                    arbre = arbres.FirstOrDefault(a => a.CodeOffre == codeOffre && a.Version == version && a.Type == type && a.Etape == etape && a.Perimetre == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation));
                else
                    arbre = arbres.FirstOrDefault(a => a.CodeOffre == codeOffre && a.Version == version && a.Type == type && a.Etape == etape && a.Perimetre == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule));
            }
            else
                arbre = arbres.FirstOrDefault(a => a.CodeOffre == codeOffre && a.Version == version && a.Type == type && a.Etape == etape);

            if (arbre != null)
                return true;
            return false;
        }

        /// <summary>
        /// Récupère le tag de l'étape
        /// dans l'arbre chargé pour l'offre
        /// </summary>
        private static string GetTagInArbre(List<ArbreDto> arbres, string codeOffre, int version, string type, string etape)
        {
            ArbreDto arbre = arbres.FirstOrDefault(a => a.CodeOffre == codeOffre && a.Version == version && a.Type == type && a.Etape == etape);
            if (arbre != null)
                return arbre.PassageTag;
            return string.Empty;
        }

        private static bool ExisteTrace(TraceDto trace)
        {
            DbParameter[] param = new DbParameter[14];
            string sql = @"SELECT KEVTYP, KEVIPB, KEVALX, KEVETAPE, KEVETORD, KEVORDR, KEVPERI, KEVRSQ, KEVOBJ, KEVKBEID, KEVFOR, KEVOPT, KEVNIVM, KEVCRU, KEVCRD, KEVCRH, KEVMAJU, KEVMAJD, KEVMAJH FROM KPCTRLE 
WHERE KEVTYP = :KEVTYP 
AND KEVIPB = :KEVIPB 
AND KEVALX = :KEVALX 
AND KEVETAPE = :KEVETAPE 
AND KEVETORD = :KEVETORD 
AND KEVORDR = :KEVORDR 
AND KEVPERI = :KEVPERI 
AND KEVRSQ = :KEVRSQ 
AND KEVOBJ = :KEVOBJ 
AND KEVKBEID = :KEVKBEID 
AND KEVFOR = :KEVFOR 
AND KEVOPT = :KEVOPT 
AND KEVNIVM = :KEVNIVM
AND KEVCRU = :KEVCRU";
            param[0] = new EacParameter("KEVTYP", trace.Type);
            param[1] = new EacParameter("KEVIPB", trace.CodeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KEVALX", trace.Version);
            param[3] = new EacParameter("KEVETAPE", trace.EtapeGeneration);
            param[4] = new EacParameter("KEVETORD", trace.NumeroOrdreEtape);
            param[5] = new EacParameter("KEVORDR", trace.NumeroOrdreDansEtape);
            param[6] = new EacParameter("KEVPERI", trace.Perimetre);
            param[7] = new EacParameter("KEVRSQ", trace.Risque);
            param[8] = new EacParameter("KEVOBJ", trace.Objet);
            param[9] = new EacParameter("KEVKBEID", trace.IdInventaire);
            param[10] = new EacParameter("KEVFOR", trace.Formule);
            param[11] = new EacParameter("KEVOPT", trace.Option);
            param[12] = new EacParameter("KEVNIVM", trace.Niveau);
            param[13] = new EacParameter("KEVCRU", trace.CreationUser);
            List<TraceDto> traces = DbBase.Settings.ExecuteList<TraceDto>(CommandType.Text, sql, param);
            if (traces.Count > 0)
            {
                return true;
            }
            return false;
        }

        private static List<RisqueDto> GetRisquesHierarchy(string codeOffre, int version, string type, ModeConsultation navig)
        {
            List<RisqueDto> result = new List<RisqueDto>();
            //Récupération de toutes les traces de l'offre
            List<TraceDto> traces = GetTraces(codeOffre, version, type);
            //Récupération de tous les risques de l'offre
            List<RisqueDto> risques = GetRisques(codeOffre, version, type).Distinct().ToList();
            //Pour chacun des risques
            foreach (RisqueDto risqueDto in risques)
            {
                //Récupération de la trace qui correspond au risque en cours
                TraceDto risqueTrace = traces.FirstOrDefault(x => x.Type.Equals(type) && x.CodeOffre.Equals(codeOffre) && x.Version.Equals(version) && x.EtapeGeneration.Equals("RSQ") && x.Risque.Equals(risqueDto.Numero));
                //S'il existe une trace qui correspond au risque en cours
                if (risqueTrace != null)
                {
                    //Récupération de la liste des options de l'offre
                    List<OptionDto> options = GetOptions(codeOffre, version, type, risqueDto.Numero).GroupBy(x => new { x.Risque, x.Formule, x.Option }).Select(y => y.First()).ToList();
                    //Pour chacune des forumules
                    foreach (OptionDto optionDto in options.FindAll(x => x.Option == 1))
                    {
                        TraceDto formuleTrace = traces.FirstOrDefault(x => x.Type.Equals(type) && x.CodeOffre.Equals(codeOffre) && x.Version.Equals(version) && x.EtapeGeneration.Equals("FOR") && x.Risque.Equals(optionDto.Risque) && x.Formule.Equals(optionDto.Formule) && x.Option.Equals(optionDto.Option));
                        if (formuleTrace != null)
                        {
                            //Création de l'objet Formule
                            FormuleDto formule = new FormuleDto();
                            //Affectation de l'option à la formule
                            formule.Option = optionDto;
                            //Pour chacune des formules/options
                            foreach (OptionDto option in options)
                            {
                                //Récupération de la trace qui correspond à la option en cours
                                TraceDto optionTrace = traces.FirstOrDefault(x => x.Type.Equals(type) && x.CodeOffre.Equals(codeOffre) && x.Version.Equals(version) && x.EtapeGeneration.Equals("FOR") && x.Risque.Equals(option.Risque) && x.Formule.Equals(option.Formule) && x.Option.Equals(option.Option));
                                //S'il existe une trace qui correspond à la option en cours
                                if (optionTrace != null)
                                {
                                    formule.Options.Add(option);
                                }
                            }
                            formule.Alpha = GetFormuleAlpha(codeOffre, version, type, formule.Option);
                            risqueDto.Formules.Add(formule);
                        }
                    }
                    result.Add(risqueDto);
                }
            }
            return result;
        }

        private static string GetFormuleAlpha(string codeOffre, int version, string type, OptionDto option)
        {
            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT KDAALPHA FROM KPFOR WHERE KDATYP = :KDATYP AND KDAIPB = :KDAIPB AND KDAALX = :KDAALX AND KDAFOR = :KDAFOR";
            param[0] = new EacParameter("KDATYP", type);
            param[1] = new EacParameter("KDAIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KDAALX", Convert.ToInt32(version));
            param[3] = new EacParameter("KDAFOR", option.Formule);
            return DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param).ToString();
        }

        private static string GetOffreIdentification(string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT PBREF FROM YPOBASE WHERE PBTYP = :PBTYP AND PBIPB = :PBIPB AND PBALX = :PBALX";
            param[0] = new EacParameter("PBTYP", type);
            param[1] = new EacParameter("PBIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("PBALX", Convert.ToInt32(version));
            return DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param).ToString();
        }

        private static List<OptionDto> GetOptions(string codeOffre, int version, string type, int risque)
        {
            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT KDDRSQ, KDDFOR, KDDOPT, KDADESC DESCRIPTION FROM KPOPTAP
                    INNER JOIN KPFOR ON KDDFOR = KDAFOR AND KDDIPB = KDAIPB AND KDDALX = KDAALX
                        WHERE KDDTYP = :KDDTYP AND KDDIPB = :KDDIPB AND KDDALX = :KDDALX AND KDDRSQ = :KDDRSQ AND (KDDPERI = 'RQ' OR KDDPERI = 'OB')
                        ORDER BY KDDFOR, KDDOPT";
            param[0] = new EacParameter("KDDTYP", type);
            param[1] = new EacParameter("KDDIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KDDALX", Convert.ToInt32(version));
            param[3] = new EacParameter("KDDRSQ", risque);

            return DbBase.Settings.ExecuteList<OptionDto>(CommandType.Text, sql, param);
        }

        private static List<TraceDto> GetTraces(string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[4];
            string sql = @"SELECT KEVTYP, KEVIPB, KEVALX, KEVETAPE, KEVETORD, KEVORDR, KEVPERI, KEVRSQ, KEVOBJ, KEVKBEID, KEVFOR, KEVOPT, KEVNIVM, KEVCRU, KEVCRD, KEVCRH, KEVMAJU, KEVMAJD, KEVMAJH  FROM KPCTRLE
                        WHERE KEVTYP = :KEVTYP AND KEVIPB = :KEVIPB AND KEVALX = :KEVALX";
            param[0] = new EacParameter("KEVTYP", type);
            param[1] = new EacParameter("KEVIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KEVALX", Convert.ToInt32(version));

            return DbBase.Settings.ExecuteList<TraceDto>(CommandType.Text, sql, param);
        }

        private static List<RisqueDto> GetRisques(string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[2];
            string sql = @"SELECT JERSQ, JECCH, KABDESC FROM YPRTRSQ
                            LEFT JOIN KPRSQ ON KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ
                            WHERE JEIPB = :JEIPB AND JEALX = :JEALX
                            ORDER BY JECCH";
            param[0] = new EacParameter("JEIPB", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("JEALX", Convert.ToInt32(version));

            return DbBase.Settings.ExecuteList<RisqueDto>(CommandType.Text, sql, param);
        }

        public static bool ExisteKPCTRL(string codeOffre, int version, string type)
        {
            DbParameter[] param = new DbParameter[3];
            string sql = @"SELECT * FROM KPCTRL
                            WHERE KEUTYP = :KEUTYP AND KEUIPB = :KEUIPB AND KEUALX = :KEUALX";
            param[0] = new EacParameter("KEUTYP", type);
            param[1] = new EacParameter("KEUIPB", codeOffre.PadLeft(9, ' '));
            param[2] = new EacParameter("KEUALX", Convert.ToInt32(version));

            if (DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param) > 0)
            {
                return true;
            }
            return false;
        }


        private static bool ExisteDansTraces(List<TraceDto> traces, string codeOffre, int version, string type, string etapeGeneration)
        {
            TraceDto trace = traces.FirstOrDefault(x => x.Type.Equals(type) && x.CodeOffre.Equals(codeOffre) && x.Version.Equals(version) && x.EtapeGeneration.Equals(etapeGeneration));
            if (trace != null)
            {
                return true;
            }
            return false;
        }

        public static bool ExisteTrace(string codeOffre, int version, string type, string etapeGeneration)
        {
            List<TraceDto> traces = GetTraces(codeOffre, version, type);
            return ExisteDansTraces(traces, codeOffre, version, type, etapeGeneration);
        }

        #endregion
    }
}
