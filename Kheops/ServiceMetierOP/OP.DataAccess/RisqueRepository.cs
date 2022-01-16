using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Bloc;
using OP.WSAS400.DTO.ChoixClauses;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.MatriceFormule;
using OP.WSAS400.DTO.MatriceGarantie;
using OP.WSAS400.DTO.MatriceRisque;
using OP.WSAS400.DTO.Modeles;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.Volet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;
using System.Threading.Tasks;

namespace OP.DataAccess
{
    public class RisqueRepository
    {
        public static string GetFirstCodeRsq(string codeOffre, string version, string type)
        {
            var param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            string sql = @"SELECT MIN(JERSQ) INT32RETURNCOL FROM YPRTRSQ WHERE JEIPB = :codeOffre AND JEALX = :version";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            int codeRsq = 1;
            if (result.Any() && result != null)
            {
                codeRsq = result.FirstOrDefault().Int32ReturnCol;
            }

            return codeRsq.ToString();
        }

        public static string GetFirstCodeObjRsq(string codeOffre, string version, string type, string codeRsq)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("codeRsq", DbType.AnsiStringFixedLength);
            param[2].Value = codeRsq;
            string sql = @"SELECT MIN(JGOBJ) INT32RETURNCOL FROM YPRTOBJ WHERE JGIPB = :codeOffre AND JGALX = :version AND JGRSQ = :codeRsq";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            int codeObj = 1;
            if (result.Any() && result != null)
            {
                codeObj = result.FirstOrDefault().Int32ReturnCol;
            }

            return codeObj.ToString();
        }

        #region Matrice Formule

        #region Méthodes Publiques

        public static MatriceFormuleDto InitMatriceFormule(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            var sql = @"SELECT PBAVA * 10000 + PBAVM * 100 + PBAVJ DATEDEBRETURNCOL,
                                PBPER STRRETURNCOL,
                                PBFEA * 10000 + PBFEM * 100 + PBFEJ DATEFINRETURNCOL,
                                JDPEA * 10000 + JDPEM * 100 + JDPEJ INT64RETURNCOL
                            FROM YPOBASE
                                INNER JOIN YPRTENT ON JDIPB = PBIPB AND JDALX = PBALX
                            WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";
            var resultPer = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            DtoCommon periodeAvt = new DtoCommon();
            if (resultPer != null && resultPer.Any())
            {
                periodeAvt = resultPer.FirstOrDefault();
            }

            MatriceFormuleDto toReturn = new MatriceFormuleDto();

            string result = "";
            if (modeNavig == ModeConsultation.Standard && !isReadonly)
            {
                result = CommonRepository.LoadAS400Matrice(codeOffre, version, type, codeAvn, user, acteGestion);
            }
            if (result == "ERREUR")
            {
                //erreur
            }

            toReturn.FormGen = FormGenPossible(codeOffre, version, type, codeAvn, modeNavig);
            toReturn.Formule = GetFormuleGenerale(codeOffre, version, type, codeAvn, modeNavig);
            toReturn.HasFormGen = toReturn.Formule != null;
            toReturn.AddFormule = AddFormule(codeOffre, version, type);

            List<MatriceFormuleLineDto> rows = GetMatriceFormuleLineM(codeOffre, version, type, codeAvn, modeNavig);
            List<MatriceFormuleColumnDto> columns = GetMatriceFormuleColumn(codeOffre, version, type, codeAvn, modeNavig);
            List<MatriceFormuleCellDto> cells = GetMatriceFormuleCell(codeOffre, version, type, codeAvn, modeNavig);

            List<MatriceFormuleFormDto> formules = new List<MatriceFormuleFormDto>();

            //récupération des formules
            toReturn.Formules = InitFormules(columns, periodeAvt, codeAvn);

            List<RisqueDto> risques = new List<RisqueDto>();
            //récupération des risques
            var listRows = rows.GroupBy(r => r.CodeRisque).Select(r => r.First());
            if (listRows != null)
            {
                listRows.ToList().FindAll(el => el.TypeEnregistrement == "R").ForEach(el =>
                {
                    RisqueDto risque = new RisqueDto
                    {
                        Code = Convert.ToInt32(el.CodeRisque),
                        Designation = el.Description,
                        isIndexe = el.isAffecte != "O",
                        hasInventaire = el.hasInventaire == "O" ? true : false,
                        NumeroChronoMatrice = el.NumChronoLine,
                        isAffecte = el.isAffecte == "O" ? false : true,
                        Formules = InitFormules(columns, periodeAvt, codeAvn),
                        AvnCreationRsq = el.CreateAvt,
                        AvnModifRsq = el.ModifAvt,
                        DateEffetAvenantModificationLocale = el.DateAvt > 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateAvt)) : null,
                        EntreeGarantie = el.DateAvt > 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateAvt)) : null,
                        SortieGarantie = el.DateFinAvt > 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateFinAvt)) : null
                    };
                    risque.IsAlertePeriode = CommonRepository.GetIsAlertePeriode(risque.EntreeGarantie, risque.SortieGarantie,
                                                        AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.DateDebReturnCol)), AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.DateFinReturnCol)),
                                                        periodeAvt.StrReturnCol, AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.Int64ReturnCol)));

                    //affectation des icônes pour les risques
                    risque.Formules.ForEach(frs =>
                    {
                        frs.Options.ForEach(opts =>
                        {
                            var cls = cells.FindAll(cel => cel.NumChronoColumn == opts.NumeroChrono && cel.NumChronoLine == risque.NumeroChronoMatrice);
                            if (cls != null)
                            {
                                if (cls.Count >= 1)
                                {
                                    if (cls.Count > 1)
                                    {
                                        opts.Anomalie = "Doublon";
                                    }

                                    opts.Icone = cls.First().Icone;
                                }
                            }
                        });
                    });

                    //récupération des objets du risque
                    var objRisques = rows.FindAll(obj => obj.CodeRisque == el.CodeRisque && obj.TypeEnregistrement == "O");
                    if (objRisques != null)
                    {
                        risque.Objets = new List<ObjetDto>();
                        objRisques.ForEach(objRsq =>
                        {
                            ObjetDto objet = new ObjetDto
                            {
                                Code = Convert.ToInt32(objRsq.CodeObjet),
                                Designation = objRsq.Description,
                                hasInventaires = objRsq.hasInventaire == "O" ? true : false,
                                NumeroChronoMatrice = objRsq.NumChronoLine,
                                isAffecte = objRsq.isAffecte != "O",
                                Formules = InitFormules(columns, periodeAvt, codeAvn),
                                AvnCreationObj = objRsq.CreateAvt,
                                DateEffetAvenantModificationLocale = objRsq.DateAvt > 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(objRsq.DateAvt)) : null,
                                EntreeGarantie = objRsq.DateAvt > 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(objRsq.DateAvt)) : null,
                                SortieGarantie = objRsq.DateFinAvt > 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(objRsq.DateFinAvt)) : null
                            };
                            objet.IsAlertePeriode = CommonRepository.GetIsAlertePeriode(objet.EntreeGarantie, objet.SortieGarantie,
                                                                AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.DateDebReturnCol)), AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.DateFinReturnCol)),
                                                                periodeAvt.StrReturnCol, AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.Int64ReturnCol)));

                            //affectation des icônes pour les risques
                            objet.Formules.ForEach(frs =>
                            {
                                frs.Options.ForEach(opts =>
                                {
                                    var cls = cells.FindAll(cel => cel.NumChronoColumn == opts.NumeroChrono && cel.NumChronoLine == objet.NumeroChronoMatrice);
                                    if (cls != null)
                                    {
                                        if (cls.Count >= 1)
                                        {
                                            if (cls.Count > 1)
                                            {
                                                opts.Anomalie = "Doublon";
                                            }

                                            opts.Icone = cls.First().Icone;
                                        }
                                    }
                                });
                            });

                            risque.Objets.Add(objet);
                        });
                    }

                    risques.Add(risque);
                });
            }
            toReturn.Risques = risques;

            return toReturn;
        }


        public static string GetValidRsq(string codeOffre, string version, string type, string codeRsq)
        {
            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM KPCTRLE WHERE KEVIPB = '{0}' AND KEVALX = {1} AND KEVTYP = '{2}' AND KEVRSQ = {3} AND KEVETAPE = 'RSQ'",
                    codeOffre.PadLeft(9, ' '), version, type, codeRsq);
            return CommonRepository.ExistRow(sql) ? "1" : string.Empty;
        }

        #endregion

        #region Méthodes Privées

        private static List<MatriceFormuleFormDto> InitFormules(List<MatriceFormuleColumnDto> columns, DtoCommon periodeAvt, string codeAvn)
        {
            List<MatriceFormuleFormDto> formules = new List<MatriceFormuleFormDto>();

            var listColumns = columns.GroupBy(f => f.CodeFormule).Select(f => f.First());

            if (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0")
            {
                listColumns.ToList().ForEach(el =>
                {
                    var minDate = columns.FindAll(f => f.CodeFormule == el.CodeFormule).Min(f => f.DateAvt);
                    el.DateAvt = minDate;
                    var maxDate = columns.FindAll(f => f.CodeFormule == el.CodeFormule).Min(f => f.DateFinAvt);
                    if (maxDate > 0)
                    {
                        maxDate = columns.FindAll(f => f.CodeFormule == el.CodeFormule).Max(f => f.DateFinAvt);
                    }

                    el.DateFinAvt = maxDate;
                });
            }

            if (listColumns != null)
            {
                listColumns.ToList().ForEach(el =>
                {
                    MatriceFormuleFormDto formule = new MatriceFormuleFormDto
                    {
                        Code = el.CodeFormule.ToString(),
                        Libelle = "Formule " + el.LibFormule,
                        Designation = el.NomFormule,
                        AvnCreationFor = el.CreateAvt,
                        DateEffetAvenantModificationLocale = el.DateAvt > 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateAvt)) : null,
                        CodeRsq = el.CodeRsq,
                        BlockFormConditions = el.CodeFormuleAvt == 0,
                        SupprForm = el.CreateAvt.ToString() == codeAvn || string.IsNullOrEmpty(codeAvn)

                    };
                    formule.IsAlertePeriode = CommonRepository.GetIsAlertePeriode(AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateAvt)), AlbConvert.ConvertIntToDate(Convert.ToInt32(el.DateFinAvt)),
                                      AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.DateDebReturnCol)), AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.DateFinReturnCol)),
                                      periodeAvt.StrReturnCol, AlbConvert.ConvertIntToDate(Convert.ToInt32(periodeAvt.Int64ReturnCol)));

                    var optsFormules = columns.FindAll(co => co.CodeFormule == el.CodeFormule);
                    if (optsFormules != null)
                    {
                        formule.Options = new List<MatriceFormuleOptDto>();
                        optsFormules.ForEach(optFor =>
                              formule.Options.Add(new MatriceFormuleOptDto
                              {
                                  Code = optFor.CodeOption.ToString(),
                                  Designation = "Opt " + optFor.CodeOption,
                                  NumeroChrono = optFor.NumChronoColumn
                              }));
                    }
                    formules.Add(formule);
                });
            }

            return formules;
        }

        private static List<MatriceFormuleLineDto> GetMatriceFormuleLineM(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {

            string sql = string.Format(@"SELECT KEBTYE TYPEENREGISTREMENT, KEBRSQ CODERISQUE, KEBOBJ CODEOBJET,
                    KEBINV HASINVENTAIRE, KEBVID ISAFFECTE, KABDESC DESCRIPTION, JEOBJ MONOOBJET, KEBCHR NUMCHRONO,
                    JEAVE CREATEAVT, JEAVF MODIFAVT, JEVDA * 10000 + JEVDM * 100 + JEVDJ DATEAVT,
                    JEVFA * 10000 + JEVFM * 100 + JEVFJ DATEFINAVT
                FROM {5}
                    INNER JOIN {6} ON JEIPB = KEBIPB AND JEALX = KEBALX AND JERSQ = KEBRSQ {11}
                    INNER JOIN {7} ON KABTYP = KEBTYP AND KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ {12}
                WHERE KEBTYP = '{0}' AND KEBALX = {1} AND KEBIPB = '{2}' AND KEBTYE = '{3}' {10}
                UNION
                SELECT KEBTYE TYPEENREGISTREMENT, KEBRSQ CODERISQUE, KEBOBJ CODEOBJET,
                    KEBINV HASINVENTAIRE, KEBVID ISAFFECTE, KACDESC DESCRIPTION, -1 MONOOBJET, KEBCHR NUMCHRONO,
                    JGAVE CREATEAVT, JGAVF MODIFAVT, JGVDA * 10000 + JGVDM * 100 + JGVDJ DATEAVT,
                    JGVFA * 10000 + JGVFM * 100 + JGVFJ DATEFINAVT
                FROM {5}
                    INNER JOIN {8} ON JGIPB = KEBIPB AND JGALX = KEBALX AND JGRSQ = KEBRSQ AND JGOBJ = KEBOBJ {13}
                    INNER JOIN {9} ON KACTYP = KEBTYP AND KACIPB = JGIPB AND KACALX = JGALX AND KACRSQ = JGRSQ AND KACOBJ = JGOBJ {14}
                WHERE KEBTYP = '{0}' AND KEBALX = {1} AND KEBIPB = '{2}' AND KEBTYE = '{4}' {10}
                ORDER BY NUMCHRONO",
                type, Convert.ToInt32(version), codeOffre.PadLeft(9, ' '), "R", "O",
                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFR"),
                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                modeNavig == ModeConsultation.Historique ? string.Format(" AND KEBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                modeNavig == ModeConsultation.Historique ? " AND JEAVN = KEBAVN" : string.Empty,
                modeNavig == ModeConsultation.Historique ? " AND JEAVN = KABAVN" : string.Empty,
                modeNavig == ModeConsultation.Historique ? " AND JGAVN = KEBAVN" : string.Empty,
                modeNavig == ModeConsultation.Historique ? " AND JGAVN = KACAVN" : string.Empty);

            return DbBase.Settings.ExecuteList<MatriceFormuleLineDto>(CommandType.Text, sql);
        }
        private static List<MatriceFormuleColumnDto> GetMatriceFormuleColumn(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            //TODO ECM : utilité des champs KDBAVG et JEAVF/JGAVF (MODIFAVT), propriété non utilisée
            string sql = string.Format(@"SELECT KEAFOR CODEFORMULE, KEAOPT CODEOPTION, KDAALPHA LIBFORMULE, KDADESC NOMFORMULE, KEACHR NUMCHRONO,
                                            KDBAVE CREATEAVT, KDBAVG MODIFAVT, JEAVF MODIFAVT {15}, KDDRSQ CODERSQ {14}
                                        FROM {12}
                                            INNER JOIN {4} ON KDAID = KEAKDAID AND KDAFGEN <> 'O'
                                            INNER JOIN {5} ON KDBKDAID = KDAID
                                            INNER JOIN {6} ON KDDKDBID = KDBID
                                            INNER JOIN {7} ON JEIPB = KDDIPB AND JEALX = KDDALX AND JERSQ = KDDRSQ {10}
                                            {13}
                                        WHERE KEATYP='{0}' AND KEAALX='{1}' AND KEAIPB='{2}' AND KDDPERI = 'RQ' {9}
                                        UNION
                                        SELECT KEAFOR CODEFORMULE, KEAOPT CODEOPTION, KDAALPHA LIBFORMULE, KDADESC NOMFORMULE, KEACHR NUMCHRONO,
                                            KDBAVE CREATEAVT, KDBAVG MODIFAVT, JGAVF MODIFAVT {16}, KDDRSQ CODERSQ {14}
                                        FROM {12}
                                            INNER JOIN {4} ON KDAID = KEAKDAID AND KDAFGEN <> 'O'
                                            INNER JOIN {5} ON KDBKDAID = KDAID
                                            INNER JOIN {6} ON KDDKDBID = KDBID
                                            INNER JOIN {8} ON JGIPB = KDDIPB AND JGALX = KDDALX AND JGRSQ = KDDRSQ AND JGOBJ = KDDOBJ {11}
                                            {13}
                                        WHERE KEATYP='{0}' AND KEAALX='{1}' AND KEAIPB='{2}' AND KDDPERI = 'OB' {9}
                                        ORDER BY NUMCHRONO",
                                    type,
                                    Convert.ToInt32(version),
                                    codeOffre.PadLeft(9, ' '),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFF"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPOPT"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                                    CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                                    modeNavig == ModeConsultation.Historique ? string.Format(" AND KEAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                    modeNavig == ModeConsultation.Historique ? " AND JEAVN = KDDAVN" : string.Empty,
                                    modeNavig == ModeConsultation.Historique ? " AND JGAVN = KDDAVN" : string.Empty,
                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFF"),
                                    !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? "LEFT JOIN KPAVTRC ON KHOIPB = KDBIPB AND KHOALX = KDBALX AND KHOTYP = KDBTYP AND KHOFOR = KDBFOR AND TRIM(KHOPERI) = 'OPT'" : "",
                                    !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? ", IFNULL(KHOFOR, 0) AVTFOR" : ", -1 AVTFOR",
                                    !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? ", JEVDA * 10000 + JEVDM * 100 + JEVDJ DATEAVT, JEVFA * 10000 + JEVFM * 100 + JEVFJ DATEFINAVT" : ", 0 DATEAVT, 0 DATEFINAVT",
                                    !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? ", JGVDA * 10000 + JGVDM * 100 + JGVDJ DATEAVT, JGVFA * 10000 + JGVFM * 100 + JGVFJ DATEFINAVT" : ", 0 DATEAVT, 0 DATEFINAVT");



            var toReturn = DbBase.Settings.ExecuteList<MatriceFormuleColumnDto>(CommandType.Text, sql);
            return toReturn;
        }
        private static List<MatriceFormuleCellDto> GetMatriceFormuleCell(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Format(@"SELECT KECKEACHR NUMCHRONOCOLUMN, KECKEBCHR NUMCHRONOLINE, KECICO ICONE
                        FROM {3} WHERE KECTYP='{0}' AND KECALX='{1}' AND KECIPB='{2}' {4}", type, Convert.ToInt32(version), codeOffre.PadLeft(9, ' '),
                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFL"),
                modeNavig == ModeConsultation.Historique ? string.Format(" AND KECAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);

            var toReturn = DbBase.Settings.ExecuteList<MatriceFormuleCellDto>(CommandType.Text, sql);
            return toReturn;
        }

        #endregion

        #endregion

        #region Matrice Garantie

        #region Méthodes Publiques

        public static MatriceGarantieDto InitMatriceGarantie(string argCodeOffre, string argVersion, string argType, string argCodeAvn, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly)
        {
            return RemplirMatriceGarantieDto(argCodeOffre, argVersion, argType, argCodeAvn, modeNavig, user, acteGestion, isReadonly);
        }

        #endregion

        #region Méthodes Privées

        private static MatriceGarantieDto RemplirMatriceGarantieDto(string argCodeOffre, string argVersion, string argType, string argCodeAvn, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly)
        {
            MatriceGarantieDto toReturn = new MatriceGarantieDto();

            string result = "";
            if (modeNavig == ModeConsultation.Standard && !isReadonly)
            {
                result = CommonRepository.LoadAS400Matrice(argCodeOffre, argVersion, argType, argCodeAvn, user, acteGestion);
            }
            if (result == "ERREUR")
            {
                toReturn.CodeError = 1;
            }

            toReturn.FormGen = FormGenPossible(argCodeOffre, argVersion, argType, argCodeAvn, modeNavig);
            toReturn.Formule = GetFormuleGenerale(argCodeOffre, argVersion, argType, argCodeAvn, modeNavig);
            toReturn.HasFormGen = toReturn.Formule != null;
            toReturn.AddFormule = AddFormule(argCodeOffre, argVersion, argType);

            //----------- Recuperation des risques          
            string sql = string.Empty;

            var param = new List<DbParameter>
            {
                new EacParameter("codeOffre", argCodeOffre.PadLeft(9, ' ')),
                new EacParameter("version", int.Parse(argVersion)),
                new EacParameter("type", argType)
            };

            switch (modeNavig)
            {
                case ModeConsultation.Standard:
                    sql =
                        $@"SELECT KABDESC LibelleRisque, KEDRSQ Code 
                        FROM { CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ")}
                        INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ")} 
                            ON JEIPB = KABIPB AND JEALX = KABALX AND JERSQ = KABRSQ 
                        INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPMATGR")} 
                            ON KEDRSQ = JERSQ AND KEDIPB = JEIPB AND KEDALX = JEALX 
                        WHERE KEDIPB=:codeOffre AND KEDALX=:version AND KEDTYP=:type 
                        ORDER BY KEDCHR";
                    break;
                case ModeConsultation.Historique:
                    param.Add(new EacParameter("avn", !string.IsNullOrEmpty(argCodeAvn) ? Convert.ToInt32(argCodeAvn) : 0));
                    sql =
                        $@"SELECT KABDESC LibelleRisque, KEDRSQ Code 
                        FROM {CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ")}
                        INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ")} 
                            ON JEIPB = KABIPB AND JEALX = KABALX AND JERSQ = KABRSQ AND JEAVN = KABAVN AND JEAVN = KABAVN
                        INNER JOIN {CommonRepository.GetPrefixeHisto(modeNavig, "KPMATGR")} 
                            ON KEDRSQ = JERSQ AND KEDIPB = JEIPB AND KEDALX = JEALX AND JEAVN = KEDAVN AND JEAVN = KEDAVN
                        WHERE KEDIPB=:codeOffre AND KEDALX=:version AND KEDTYP=:type AND KEDAVN = :avn
                        ORDER BY KEDCHR";
                    break;
            }

            toReturn.Risques = DbBase.Settings.ExecuteList<RisqueDto>(CommandType.Text, sql, param);
            toReturn.Volets = GetVoletsMatriceGarantie(argCodeOffre, argVersion, argType, argCodeAvn, modeNavig);
            //SAB :08/08/2017 #EPURATIONREQUETE
            toReturn.formuleOption = FormuleRepository.ObtenirFormuleOptionByOffre(argCodeOffre, argVersion, argCodeAvn, modeNavig);
            return toReturn;
        }
        /// <summary>
        /// Récupère les risques pour une formule donnée
        /// </summary>
        private static List<RisqueDto> GetRisquesByGarantie(List<MatriceGarantiePlatDto> garanties, long codeVolet, long codeBloc, long codeGarantie, long nivGarantie)
        {
            List<RisqueDto> risques = new List<RisqueDto>();
            var lstRsq1 = garanties.FindAll(r1 => r1.IdVolet == codeVolet && r1.IdBloc == codeBloc && r1.NiveauGarantie == nivGarantie && r1.IdSequence == codeGarantie);
            if (lstRsq1 != null)
            {
                foreach (var rsq in lstRsq1)
                {
                    risques.Add(new RisqueDto
                    {
                        Code = Convert.ToInt32(rsq.IdRisque),
                        Flag = rsq.Icon2
                    });
                }
            }
            return risques;
        }

        /// <summary>
        /// Récupère la liste des volets/blocs/garanties pour une offre
        /// </summary>
        private static List<DtoVolet> GetVoletsMatriceGarantie(string argCodeOffre, string argVersion, string argType, string codeAvn, ModeConsultation modeNavig)
        {
            List<DtoVolet> toReturn = new List<DtoVolet>();

            var paramMat = new List<DbParameter>
            {
                new EacParameter("codeOffre", argCodeOffre.PadLeft(9, ' ')),
                new EacParameter("version", int.Parse(argVersion)),
                new EacParameter("type", argType)
            };

            string sql = string.Format(@"SELECT 
                                optdV.KDCKAKID IDVOLET,
                                KEEVOLET LIBELLEVOLET1,
                                KAKDESC LIBELLEVOLET2,
                                KEEKAEID IDBLOC,
                                KEEBLOC LIBELLEBLOC1,
                                KAEDESC LIBELLEBLOC2,
                                KEEGARAN LIBELLEGARANTIE1,
                                GADEA LIBELLEGARANTIE2,
                                KEENIV NIVEAUGARANTIE,
                                KEESEQ IDSEQUENCE,
                                KDESEM NIVEAUSUP,
                                KDESE1 NIVEAUORIGINE,
                                KEEVID ICON1,
                                KEFICO ICON2,
                                KEDRSQ IDRISQUE
                            FROM {0}
                                INNER JOIN KVOLET ON KEEKAKID = KAKID
                                INNER JOIN KBLOC ON KEEKAEID = KAEID
                                INNER JOIN KGARAN ON KEEGARAN = GAGAR
                                INNER JOIN {1} ON KEETYP = KDETYP AND KEEIPB = KDEIPB AND KEEALX = KDEALX AND KEEGARAN = KDEGARAN AND KEEKDCID = KDEKDCID AND KEEGARAN = GAGAR AND KEESEQ = KDESEQ {5}
                                LEFT JOIN {2} ON KEECHR = KEFKEECHR AND KEETYP = KEFTYP AND KEEIPB = KEFIPB AND KEEALX = KEFALX {6}
                                LEFT JOIN {3} ON KEFKEDCHR = KEDCHR AND KEFIPB = KEDIPB AND KEFALX = KEDALX AND KEFTYP = KEDTYP {7}

                                INNER JOIN {8} optdV
                                    ON optdV.KDCIPB = KDEIPB
                                    AND optdV.KDCTYP = KDETYP
                                    AND optdV.KDCALX = KDEALX
                                    AND optdV.KDCFOR = KDEFOR
                                    AND optdV.KDCOPT = KDEOPT
   		                            AND optdV.KDCTENG = 'V'
                  
                                INNER JOIN {8} optdB
   	 	                            ON optdB.KDCID = KDEKDCID
   	 	                            AND optdB.KDCKAKID = optdV.KDCKAKID
   		                            AND optdB.KDCTENG = 'B'

                                WHERE KEEIPB = :codeOffre AND KEEALX = :version AND KEETYP = :type AND KEEGARAN <> '' {4} AND KEEVOLET <> 'GAREAT'
                                ORDER BY optdV.kdcordre, optdB.kdcordre, kdetri",
                /*0*/                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATGG"),
                /*1*/                CommonRepository.GetPrefixeHisto(modeNavig, "KPGARAN"),
                /*2*/                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATGL"),
                /*3*/                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATGR"),
                /*4*/                modeNavig == ModeConsultation.Historique ? " AND KEEAVN = :avn" : string.Empty,
                /*5*/                modeNavig == ModeConsultation.Historique ? " AND KEEAVN = KDEAVN" : string.Empty,
                /*6*/                modeNavig == ModeConsultation.Historique ? " AND KEEAVN = KEFAVN" : string.Empty,
                /*7*/                modeNavig == ModeConsultation.Historique ? " AND KEFAVN = KEDAVN" : string.Empty,
                /*8*/                CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTD"));

            if (modeNavig == ModeConsultation.Historique)//|| (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0"))
            {
                paramMat.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            List<MatriceGarantiePlatDto> resultSql = DbBase.Settings.ExecuteList<MatriceGarantiePlatDto>(CommandType.Text, sql, paramMat);

            if (resultSql != null && resultSql.Count > 0)
            {
                var lstVolets = resultSql.GroupBy(v => v.IdVolet).Select(v => v.First());
                foreach (var vol in lstVolets)
                {
                    List<BlocDto> blocs = new List<BlocDto>();
                    var lstBloc = resultSql.FindAll(b => b.IdVolet == vol.IdVolet).GroupBy(b => b.IdBloc).Select(b => b.First()); //OrderBy(i=>i.IdBloc).
                    if (lstBloc != null)
                    {
                        foreach (var bloc in lstBloc)
                        {
                            List<ModeleDto> modeles = new List<ModeleDto>();
                            List<GarantieModeleNiveau1Dto> garantiesNiv1 = new List<GarantieModeleNiveau1Dto>();
                            var lstGarNiv1 = resultSql.FindAll(gn1 => gn1.IdVolet == bloc.IdVolet && gn1.IdBloc == bloc.IdBloc && gn1.NiveauGarantie == 1).GroupBy(gn1 => gn1.IdSequence).Select(gn1 => gn1.First());
                            if (lstGarNiv1 != null)
                            {
                                foreach (var garNiv1 in lstGarNiv1)
                                {
                                    List<GarantieModeleSousNiveauDto> garantiesNiv2 = new List<GarantieModeleSousNiveauDto>();
                                    var lstGarNiv2 = resultSql.FindAll(gn2 => gn2.IdVolet == garNiv1.IdVolet && gn2.IdBloc == garNiv1.IdBloc && gn2.NiveauSup == garNiv1.IdSequence && gn2.NiveauGarantie == 2).GroupBy(gn2 => gn2.IdSequence).Select(gn2 => gn2.First());
                                    if (lstGarNiv2 != null)
                                    {
                                        foreach (var garNiv2 in lstGarNiv2)
                                        {
                                            List<GarantieModeleSousNiveauDto> garantiesNiv3 = new List<GarantieModeleSousNiveauDto>();
                                            var lstGarNiv3 = resultSql.FindAll(gn3 => gn3.IdVolet == garNiv2.IdVolet && gn3.IdBloc == garNiv2.IdBloc && gn3.NiveauSup == garNiv2.IdSequence && gn3.NiveauGarantie == 3).GroupBy(gn3 => gn3.IdSequence).Select(gn3 => gn3.First());
                                            if (lstGarNiv3 != null)
                                            {
                                                foreach (var garNiv3 in lstGarNiv3)
                                                {
                                                    List<GarantieModeleSousNiveauDto> garantiesNiv4 = new List<GarantieModeleSousNiveauDto>();
                                                    var lstGarNiv4 = resultSql.FindAll(gn4 => gn4.IdVolet == garNiv3.IdVolet && gn4.IdBloc == garNiv3.IdBloc && gn4.NiveauSup == garNiv3.IdSequence && gn4.NiveauGarantie == 4).GroupBy(gn4 => gn4.IdSequence).Select(gn4 => gn4.First());
                                                    if (lstGarNiv4 != null)
                                                    {
                                                        foreach (var garNiv4 in lstGarNiv4)
                                                        {
                                                            garantiesNiv4.Add(new GarantieModeleSousNiveauDto
                                                            {
                                                                Code = Convert.ToInt32(garNiv4.IdSequence),
                                                                Description = string.Format("{0} {1}", garNiv4.LibelleGarantie1, garNiv4.LibelleGarantie2),
                                                                Risques = GetRisquesByGarantie(resultSql, garNiv4.IdVolet, garNiv4.IdBloc, garNiv4.IdSequence, 4)
                                                            });
                                                        }
                                                    }

                                                    garantiesNiv3.Add(new GarantieModeleSousNiveauDto
                                                    {
                                                        Code = Convert.ToInt32(garNiv3.IdSequence),
                                                        Description = string.Format("{0} {1}", garNiv3.LibelleGarantie1, garNiv3.LibelleGarantie2),
                                                        Modeles = garantiesNiv4,
                                                        Risques = GetRisquesByGarantie(resultSql, garNiv3.IdVolet, garNiv3.IdBloc, garNiv3.IdSequence, 3)
                                                    });
                                                }
                                            }

                                            garantiesNiv2.Add(new GarantieModeleSousNiveauDto
                                            {
                                                Code = Convert.ToInt32(garNiv2.IdSequence),
                                                Description = string.Format("{0} {1}", garNiv2.LibelleGarantie1, garNiv2.LibelleGarantie2),
                                                Modeles = garantiesNiv3,
                                                Risques = GetRisquesByGarantie(resultSql, garNiv2.IdVolet, garNiv2.IdBloc, garNiv2.IdSequence, 2)
                                            });
                                        }
                                    }

                                    garantiesNiv1.Add(new GarantieModeleNiveau1Dto
                                    {
                                        Code = Convert.ToInt32(garNiv1.IdSequence),
                                        Description = string.Format("{0} {1}", garNiv1.LibelleGarantie1, garNiv1.LibelleGarantie2),
                                        Modeles = garantiesNiv2,
                                        Risques = GetRisquesByGarantie(resultSql, garNiv1.IdVolet, garNiv1.IdBloc, garNiv1.IdSequence, 1)
                                    });

                                }
                                modeles.Add(new ModeleDto
                                {
                                    Code = string.Format("{0}{1}", bloc.IdBloc, bloc.NiveauOrigine),
                                    Modeles = garantiesNiv1
                                });
                            }


                            blocs.Add(new BlocDto
                            {
                                Code = bloc.IdBloc.ToString(),
                                Description = string.Format("{0} {1}", bloc.LibelleBloc1, bloc.LibelleBloc2),
                                Modeles = modeles
                            });
                        }
                    }

                    toReturn.Add(new DtoVolet
                    {
                        Code = vol.IdVolet.ToString(),
                        Description = string.Format("{0} {1}", vol.LibelleVolet1, vol.LibelleVolet2),
                        Blocs = blocs
                    });
                }
            }

            return toReturn;
        }

        #endregion

        #endregion

        #region Matrice Risque

        #region Méthodes Publiques

        public static MatriceRisqueDto InitMatriceRisque(string codeOffre, string version, string type, string codeAvenant, ModeConsultation modeNavig, string user, string acteGestion, bool isReadonly)
        {
            MatriceRisqueDto toReturn = new MatriceRisqueDto();

            string result = "";
            if (modeNavig == ModeConsultation.Standard && !isReadonly)
            {
                result = CommonRepository.LoadAS400Matrice(codeOffre, version, type, codeAvenant, user, acteGestion);
            }
            if (result == "ERREUR")
            {
                //erreur
            }
            toReturn.FormGen = FormGenPossible(codeOffre, version, type, codeAvenant, modeNavig);
            toReturn.Formule = GetFormuleGenerale(codeOffre, version, type, codeAvenant, modeNavig);
            toReturn.HasFormGen = toReturn.Formule != null;
            List<MatriceRisqueLineDto> rows = GetMatriceRisqueLine(codeOffre, version, type, codeAvenant, modeNavig);
            List<MatriceRisqueFormuleDto> forms = GetMatriceRisqueFormule(codeOffre, version, type, codeAvenant, modeNavig);

            toReturn.AddFormule = AddFormule(codeOffre, version, type);

            if (forms != null)
            {
                forms.ForEach(el =>
                {
                    var selRow = rows.FirstOrDefault(r => r.NumChrono == el.NumChrono);
                    if (selRow != null)
                    {
                        if (!string.IsNullOrEmpty(selRow.CodeAlpha))
                        {
                            selRow.CodeAlpha = selRow.CodeAlpha + "/" + el.CodeAlpha;
                        }
                        else
                        {
                            selRow.CodeAlpha = el.CodeAlpha;
                        }
                    }
                });
            }

            List<RisqueDto> risques = new List<RisqueDto>();
            //récupération des risques
            var listRows = rows.GroupBy(r => r.CodeRsq).Select(r => r.First());
            if (listRows != null)
            {
                var rsqs = listRows.ToList().FindAll(el => el.TypeEnregistrement == "R");
                if (rsqs != null)
                {
                    rsqs.ForEach(el =>
                    {
                        RisqueDto risque = new RisqueDto
                        {
                            Code = Convert.ToInt32(el.CodeRsq),
                            Designation = el.LibelleRsqObj,
                            Cible = new CibleDto { Code = el.Cible },
                            Valeur = el.Valeur,
                            Unite = new ParametreDto { Code = el.Unite },
                            Type = new ParametreDto { Code = el.Type },
                            EntreeGarantie = el.DateEntree,
                            SortieGarantie = el.DateSortie,
                            isIndexe = el.isIndexe,
                            hasInventaire = el.hasInventaire,
                            CodeAlpha = el.CodeAlpha,
                            isBadDate = el.isBadDate,
                            isAffecte = el.IsAffecte.Equals("O", StringComparison.InvariantCulture)
                        };
                        //récupération des objets du risque
                        var objRisques = rows.FindAll(obj => obj.CodeRsq == el.CodeRsq && obj.TypeEnregistrement == "O");
                        if (objRisques != null)
                        {
                            risque.Objets = new List<ObjetDto>();
                            objRisques.ForEach(objRsq =>
                            {
                                objRsq.DateDebAvn = AlbConvert.ConvertIntToDate(objRsq.DebAvnAnnee * 10000 + objRsq.DebAvnMois * 100 + objRsq.DebAvnJour);
                                ObjetDto objet = new ObjetDto
                                {
                                    Code = Convert.ToInt32(objRsq.CodeObj),
                                    Designation = objRsq.LibelleRsqObj,
                                    Cible = new CibleDto { Code = objRsq.Cible },
                                    Valeur = objRsq.Valeur,
                                    Unite = new ParametreDto { Code = objRsq.Unite },
                                    Type = new ParametreDto { Code = objRsq.Type },
                                    EntreeGarantie = objRsq.DateEntree,
                                    SortieGarantie = objRsq.DateSortie,
                                    hasInventaires = objRsq.hasInventaire,
                                    CodeAlpha = objRsq.CodeAlpha,
                                    isBadDate = objRsq.isBadDate//AlbConvert.ConvertIntToDate(objRsq.DateFin) < objRsq.DateDebAvn ? true : false,
                                    ,
                                    isAffecte = objRsq.IsAffecte.Equals("O", StringComparison.InvariantCulture)
                                };
                                risque.Objets.Add(objet);
                            });
                        }
                        risque.isCopiable = !string.IsNullOrEmpty(risque.CodeAlpha) || risque.Objets.All(obj => !string.IsNullOrEmpty(obj.CodeAlpha));
                        risques.Add(risque);
                    });
                }

            }

            toReturn.Risques = risques;
            toReturn.CopyRisque = toReturn.Risques.Any(rsq => rsq.isCopiable);
            //SAB :07/08/2017 #EPURATIONREQUETE
            toReturn.formuleOption = FormuleRepository.ObtenirFormuleOptionByOffre(codeOffre, version, codeAvenant, modeNavig);

            return toReturn;
        }

        public static List<MatriceRisqueFormuleCodeDto> GetLienRisqueFormule(string codeOffre, string version, string type, string codeRsq)
        {
            string sql = string.Format(@"SELECT DISTINCT KDDRSQ CODERISQUE, KDDFOR CODEFORMULE FROM KPOPTAP WHERE KDDIPB = '{0}' AND KDDALX = {1} AND KDDTYP = '{2}'",
                codeOffre.PadLeft(9, ' '),
                Convert.ToInt32(version),
                type);

            if (!string.IsNullOrEmpty(codeRsq))
            {
                sql += " AND KDDRSQ = " + Convert.ToInt32(codeRsq);
            }

            return DbBase.Settings.ExecuteList<MatriceRisqueFormuleCodeDto>(CommandType.Text, sql);
        }
        #endregion

        #region Méthodes Privées
        private static List<MatriceRisqueLineDto> GetMatriceRisqueLine(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Format(@"SELECT JERSQ CODERSQ, 0 CODEOBJ, KABDESC LIBELLE, KABCIBLE CIBLE, KEBCHR NUMCHRONO, 
                    JEVAL VALEUR, JEVAU UNITE, JEVAT TYPE, JEVDJ JOURDEB, JEVDM MOISDEB, JEVDA ANNEEDEB, 
                    {24}, JEINA INDEXEE, JDINA INDEXEENTETE, KEBINV INVENTAIRE, 'R' TYPEEENREGISTREMENT, 
                    PBEFJ JOURDEBO, PBEFM MOISDEBO, PBEFA ANNEEDEBO, PBFEJ JOURFINO, PBFEM MOISFINO, PBFEA ANNEEFINO,
                    PBAVA DEBAVNANNEE, PBAVM DEBAVNMOIS, PBAVJ DEBAVNJOUR, JEVFJ JOURFIN, JEVFM MOISFIN, JEVFA ANNEEFIN,KEBVID ISAFFECTE
                FROM {0}
                    INNER JOIN {1} ON PBIPB = KEBIPB AND PBALX = KEBALX AND {14} = KEBTYP {16}
                    INNER JOIN {2} ON JEIPB = KEBIPB AND JEALX = KEBALX AND JERSQ = KEBRSQ {17}
                    INNER JOIN {3} ON KABTYP = KEBTYP AND KABIPB = JEIPB AND KABALX = JEALX AND KABRSQ = JERSQ {18}
                    INNER JOIN {6} ON JDIPB = JEIPB AND JDALX = JEALX {21}
                    {7}
                    WHERE KEBTYP = '{8}'  AND KEBALX = '{9}' AND KEBIPB = '{10}' AND KEBTYE = '{12}' {15}
                UNION
                SELECT JGRSQ CODERSQ, JGOBJ CODEOBJ, KACDESC LIBELLE, KACCIBLE CIBLE, KEBCHR NUMCHRONO, 
                    JGVAL VALEUR, JGVAU UNITE, JGVAT TYPE, JGVDJ JOURDEB, JGVDM MOISDEB, JGVDA ANNEEDEB, 
                    {24}, JGINA INDEXEE, JDINA INDEXEENTETE, KEBINV INVENTAIRE, 'O' TYPEEENREGISTREMENT, 
                    PBEFJ JOURDEBO, PBEFM MOISDEBO, PBEFA ANNEEDEBO, PBFEJ JOURFINO, PBFEM MOISFINO, PBFEA ANNEEFINO,
                    PBAVA DEBAVNANNEE, PBAVM DEBAVNMOIS, PBAVJ DEBAVNJOUR, JGVFJ JOURFIN, JGVFM MOISFIN, JGVFA ANNEEFIN,KEBVID ISAFFECTE
                FROM {0} 
                    INNER JOIN {1} ON PBIPB = KEBIPB AND PBALX = KEBALX AND {14} = KEBTYP {16}
                    INNER JOIN {4} ON JGIPB = KEBIPB AND JGALX = KEBALX AND JGRSQ = KEBRSQ AND JGOBJ = KEBOBJ {19}
                    INNER JOIN {5} ON KACTYP = KEBTYP AND KACIPB = JGIPB AND KACALX = JGALX AND KACRSQ = JGRSQ AND KACOBJ = JGOBJ {20}
                    INNER JOIN {6} ON JDIPB = JGIPB AND JDALX = JGALX {22}
                    {23}
                    WHERE KEBTYP = '{8}' AND KEBALX ='{9}' AND KEBIPB = '{10}' AND KEBTYE = '{13}' {15} ORDER BY NUMCHRONO",
                /*0*/CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFR"),
                /*1*/CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                /*2*/CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                /*3*/CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                /*4*/CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                /*5*/CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                /*6*/CommonRepository.GetPrefixeHisto(modeNavig, "YPRTENT"),
                /*7*/modeNavig == ModeConsultation.Historique || (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0") ? "LEFT JOIN KJOBSORTI ON IPB = KEBIPB AND ALX = KEBALX AND TYP = KEBTYP AND RSQ = KEBRSQ AND AVN = PBAVN" : string.Empty,
                /*8*/type,
                /*9*/Convert.ToInt32(version),
                /*10*/codeOffre.PadLeft(9, ' '),
                /*11*/Convert.ToInt32(codeAvn),
                /*12*/"R",
                /*13*/"O",
                /*14*/modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                /*15*/modeNavig == ModeConsultation.Historique ? string.Format(" AND KEBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                /*16*/modeNavig == ModeConsultation.Historique ? " AND PBAVN = KEBAVN" : string.Empty,
                /*17*/modeNavig == ModeConsultation.Historique ? " AND JEAVN = KEBAVN" : string.Empty,
                /*18*/modeNavig == ModeConsultation.Historique ? " AND JEAVN = KABAVN" : string.Empty,
                /*19*/modeNavig == ModeConsultation.Historique ? " AND JGAVN = KEBAVN" : string.Empty,
                /*20*/modeNavig == ModeConsultation.Historique ? " AND JGAVN = KACAVN" : string.Empty,
                /*21*/modeNavig == ModeConsultation.Historique ? " AND JEAVN = JDAVN" : string.Empty,
                /*22*/modeNavig == ModeConsultation.Historique ? " AND JGAVN = JDAVN" : string.Empty,
                /*23*/modeNavig == ModeConsultation.Historique || (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0") ? "LEFT JOIN KJOBSORTI ON IPB = KEBIPB AND ALX = KEBALX AND TYP = KEBTYP AND RSQ = KEBRSQ AND OBJ = KEBOBJ AND AVN = PBAVN" : string.Empty,
                /*24*/modeNavig == ModeConsultation.Historique || (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0") ? "IFNULL(DATEFIN, 0) DATEFIN" : "0");
            var toReturn = DbBase.Settings.ExecuteList<MatriceRisqueLineDto>(CommandType.Text, sql);
            if (toReturn.Any())
            {
                foreach (var matriceRisqueLine in toReturn)
                {
                    if (matriceRisqueLine.DateEntreeAnnee != 0 && matriceRisqueLine.DateEntreeMois != 0 && matriceRisqueLine.DateEntreeJour != 0)
                    {
                        matriceRisqueLine.DateEntree = new DateTime(matriceRisqueLine.DateEntreeAnnee, matriceRisqueLine.DateEntreeMois, matriceRisqueLine.DateEntreeJour);
                    }

                    if (matriceRisqueLine.DateSortieAnnee != 0 && matriceRisqueLine.DateSortieMois != 0 && matriceRisqueLine.DateSortieJour != 0)
                    {
                        matriceRisqueLine.DateSortie = new DateTime(matriceRisqueLine.DateSortieAnnee, matriceRisqueLine.DateSortieMois, matriceRisqueLine.DateSortieJour);
                    }

                    matriceRisqueLine.isIndexe = matriceRisqueLine.Indexee == "O";
                    if (!matriceRisqueLine.isIndexe)
                    {
                        matriceRisqueLine.isIndexe = matriceRisqueLine.IndexeEntete == "O";
                    }

                    matriceRisqueLine.hasInventaire = matriceRisqueLine.Inventaire == "O";
                    //matriceRisqueLine.hasBadDate == 1 ? matriceRisqueLine.isBadDate = true : matriceRisqueLine.isBadDate = false;
                    if (matriceRisqueLine.DebAvnAnnee != 0 && matriceRisqueLine.DebAvnMois != 0 && matriceRisqueLine.DebAvnJour != 0)
                    {
                        //matriceRisqueLine.DateDebAvn = new DateTime(matriceRisqueLine.DebAvnAnnee, matriceRisqueLine.DebAvnMois, matriceRisqueLine.DebAvnJour);
                        //matriceRisqueLine.DateSortie = new DateTime(matriceRisqueLine.DateFin);
                        var datedebavn = matriceRisqueLine.DebAvnAnnee * 10000 + matriceRisqueLine.DebAvnMois * 100 + matriceRisqueLine.DebAvnJour;

                        if (matriceRisqueLine.DateSortie != null)
                        {
                            if (matriceRisqueLine.DateSortie < AlbConvert.ConvertIntToDate(datedebavn))
                            {
                                matriceRisqueLine.isBadDate = true;
                            }
                            else
                            {
                                matriceRisqueLine.isBadDate = false;
                            }
                        }
                        else
                        {
                            if (matriceRisqueLine.DateFin != 0 && matriceRisqueLine.DateFin < datedebavn)
                            {
                                matriceRisqueLine.isBadDate = true;
                            }
                            else
                            {
                                matriceRisqueLine.isBadDate = false;
                            }
                        }
                    }
                }
            }
            return toReturn;
        }

        private static List<MatriceRisqueFormuleDto> GetMatriceRisqueFormule(string codeOffre, string version, string type, string codeAvenant, ModeConsultation modeNavig)
        {
            string sql = string.Empty;

            var param = new List<DbParameter> {
            new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
            new EacParameter("version", 0) {Value = Convert.ToInt32(version)},
            new EacParameter("type", type)
            };


            switch (modeNavig)
            {
                case ModeConsultation.Standard:
                    sql = string.Format(@"SELECT KECKEBCHR NUMCHRONO, KDAALPHA CODEALPHA
                        	FROM {0}
                            INNER JOIN {1} ON KEACHR = KECKEACHR AND KEAIPB = KECIPB AND KEATYP = KECTYP AND KEAALX = KECALX 
                            INNER JOIN {2} ON KDAID = KEAKDAID AND KDAIPB = KEAIPB AND KDATYP = KEATYP AND KDAALX = KEAALX 
                            WHERE KEAIPB = :codeOffre AND KEAALX = :version AND KEATYP = :type",
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFL"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFF"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                                codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), type);
                    break;
                case ModeConsultation.Historique:
                    param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) : 0));
                    sql = string.Format(@"SELECT KECKEBCHR NUMCHRONO, KDAALPHA CODEALPHA
                        	FROM {0}
                            INNER JOIN {1} ON KEACHR = KECKEACHR AND KEAIPB = KECIPB AND KEATYP = KECTYP AND KEAALX = KECALX AND KEAAVN = KECAVN
                            INNER JOIN {2} ON KDAID = KEAKDAID AND KDAIPB = KEAIPB AND KDATYP = KEATYP AND KDAALX = KEAALX AND KDAAVN = KEAAVN
                            WHERE  KEAIPB = :codeOffre AND KEAALX = :version AND KEATYP = :type AND KEAAVN = :avn",
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFL"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPMATFF"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"));
                    break;
            }


            return DbBase.Settings.ExecuteList<MatriceRisqueFormuleDto>(CommandType.Text, sql, param);
        }

        private static bool FormGenPossible(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            string sql = string.Empty;

            if (modeNavig == ModeConsultation.Historique)
            {
                return false;
            }
            else
            {
                sql = string.Format(@"SELECT COUNT(*) NBLIGN 
	                        FROM {0}
		                        INNER JOIN {1} ON KAAIPB = PBIPB AND KAAALX = PBALX AND KAATYP = PBTYP {6}
		                        INNER JOIN KCATVOLET ON KAPCIBLE = KAACIBLE
		                        INNER JOIN KVOLET ON KAPKAKID = KAKID AND KAKFGEN = 'O'
	                        WHERE PBIPB = '{2}' AND PBALX = {3} AND PBTYP = '{4}' {5}",
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                                codeOffre.PadLeft(9, ' '), Convert.ToInt32(version), type,
                                modeNavig == ModeConsultation.Historique ? string.Format(" AND PBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND KAAAVN = PBAVN" : string.Empty);
                return CommonRepository.ExistRow(sql);
            }
        }

        private static FormuleDto GetFormuleGenerale(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter> {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", 0) {Value = Convert.ToInt32(version)},
                new EacParameter("type", type)
            };

            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT KDAFOR CODE, KDADESC LIBELLE
                            FROM {0} 
                            WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KDAFGEN = 'O' {1}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                            modeNavig == ModeConsultation.Historique ? " AND KDAAVN = :avn" : string.Empty);
            var result = DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param);


            return result != null && result.Count > 0 ? result.FirstOrDefault() : null;
        }

        private static bool AddFormule(string codeOffre, string version, string type)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_ADDFORMULE", DbType.Int32);
            param[3].Direction = ParameterDirection.InputOutput;
            param[3].Value = 0;
            param[3].DbType = DbType.Int32;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDFORMULE", param);

            return param[3].Value.ToString() == "1";
        }

        #endregion

        #endregion

        #region Formule Garantie

        #region Méthodes Publiques

        public static bool RisquesHasFormules(string codeOffre, int? version, string type, string codeAvn, int codeRisque, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter> {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", 0) {Value = Convert.ToInt32(version)},
                new EacParameter("type", type),
                new EacParameter("codeRisque", codeRisque)
            };

            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM {0} WHERE KDDIPB = :codeOffre  AND KDDALX = :version AND KDDTYP = :type AND KDDRSQ = :codeRisque {1}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                            modeNavig == ModeConsultation.Historique ? " AND KDDAVN = :avn" : string.Empty
                            );
            return CommonRepository.ExistRowParam(sql, param);
        }

        public static List<RisqueDto> ObtenirRisques(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<DbParameter> {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", 0) {Value = Convert.ToInt32(version)},
                new EacParameter("type", type)
            };
            //Voir les périodes définies dans la spec
            string sql = string.Format(@"SELECT KABRSQ CODERSQ, KABDESC DESCRSQ, KABCIBLE CIBLERSQ, 
                            KAIID CODECIBLE, KABCIBLE DESCCIBLE, 
                            KACOBJ CODEOBJ, KACDESC DESCOBJ,
                            KBEID CODEINVEN, KBEDESC DESCINVEN,
                            IFNULL(APRSQ.KDDID, 0) RSQUSED, IFNULL(APOBJ.KDDID, 0) OBJUSED
                        FROM {0}
                            INNER JOIN {9} ON JEIPB = PBIPB AND JEALX = PBALX {10}
                        	INNER JOIN {1} ON KABIPB = JEIPB AND KABALX = JEALX AND KABTYP = {4} {6} AND KABRSQ = JERSQ 
                            INNER JOIN KCIBLEF ON KAICIBLE = KABCIBLE AND KAIBRA = PBBRA
                            INNER JOIN {11} ON JGIPB = KABIPB AND JGALX = KABALX AND JGRSQ = KABRSQ {12}
                            INNER JOIN {2} ON KACIPB = JGIPB AND KACALX = JGALX AND KACTYP = KABTYP AND KACRSQ = JGRSQ AND KACOBJ = JGOBJ {7}
                            LEFT JOIN {3} ON KBEIPB = KACIPB AND KBEALX = KACALX AND KBETYP = KACTYP AND KBEID = KACINVEN {8}
                            LEFT JOIN {13} APRSQ ON APRSQ.KDDIPB = KACIPB AND APRSQ.KDDALX = KACALX AND APRSQ.KDDTYP = KACTYP AND APRSQ.KDDPERI = 'RQ' AND APRSQ.KDDRSQ = KACRSQ
                            LEFT JOIN {13} APOBJ ON APOBJ.KDDIPB = KACIPB AND APOBJ.KDDALX = KACALX AND APOBJ.KDDTYP = KACTYP AND APOBJ.KDDPERI = 'OB' AND APOBJ.KDDRSQ = KACRSQ AND APOBJ.KDDOBJ = KACOBJ 
                        WHERE PBIPB = :codeOffre AND PBALX = :version AND {4} = :type {5}
                        ORDER BY KABRSQ, KACOBJ",
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                                modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto),
                                modeNavig == ModeConsultation.Historique ? " AND PBAVN = :avn" : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND PBAVN = KABAVN" : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND KACAVN = KABAVN" : string.Empty,
                                modeNavig == ModeConsultation.Historique ? " AND KACAVN = KBEAVN" : string.Empty,
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                                !string.IsNullOrEmpty(codeAvn) ? " AND (JEAVE = :avn OR JEAVF = :avn OR (JEVDA * 10000 + JEVDM * 100 + JEVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ)) " : string.Empty,
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                                !string.IsNullOrEmpty(codeAvn) ? " AND (JGAVE = :avn OR JGAVF = :avn OR (JGVDA * 10000 + JGVDM * 100 + JGVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ))" : string.Empty,
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP"),
                                modeNavig == ModeConsultation.Historique ? " AND KDDAVN = KACAVN" : string.Empty);
            if (modeNavig == ModeConsultation.Historique || !string.IsNullOrEmpty(codeAvn))
            {
                param.Insert(0, new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }
            var result = DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.Text, sql, param);

            return TransformRsqObjPlat(result);
        }

        public static List<RisqueDto> ObtenirRisquesAvenant(string codeOffre, string version, string type, string avenant, ModeConsultation modeNavig)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", type);
            param[3] = new EacParameter("avenant", 0);
            param[3].Value = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0;

            string sql = string.Format(@"SELECT KABRSQ CODERSQ, KABDESC DESCRSQ, KABCIBLE CIBLERSQ, 
                            KAIID CODECIBLE, KABCIBLE DESCCIBLE, 
                            KACOBJ CODEOBJ, KACDESC DESCOBJ,
                            KBEID CODEINVEN, KBEDESC DESCINVEN 
                        FROM {0}
	                        INNER JOIN {1} ON JEIPB = PBIPB AND JEALX = PBALX AND (JEAVE = PBAVN OR JEAVF = PBAVN) AND (JEVDA * 10000 + JEVDM * 100 + JEVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ)
	                        INNER JOIN {2} ON KABIPB = JEIPB AND KABALX = JEALX AND KABTYP = PBTYP AND KABRSQ = JERSQ AND KABAVN = JEAVN
                            INNER JOIN KCIBLEF ON KAICIBLE = KABCIBLE AND KAIBRA = PBBRA
                            INNER JOIN {3} ON JGIPB = JEIPB AND JGALX = JEALX AND JGRSQ = JERSQ AND (JGAVE = PBAVN OR JGAVF = PBAVN) AND (JGVDA * 10000 + JGVDM * 100 + JGVDJ) >= (PBAVA * 10000 + PBAVM * 100 + PBAVJ)
                            INNER JOIN {4} ON KACIPB = KABIPB AND KACALX = KABALX AND KACTYP = KABTYP AND KACRSQ = JGRSQ AND KACOBJ = JGOBJ AND KACAVN = JGAVN
                            LEFT JOIN {5} ON KBEIPB = KACIPB AND KBEALX = KACALX AND KBETYP = KACTYP AND KBEID = KACINVEN AND KBEAVN = KACAVN
                        WHERE PBIPB = :codeOffre AND PBALX = :version AND {6} = :type AND PBAVN = :avenant
                        ORDER BY KABRSQ, KACOBJ",
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPOBASE"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTRSQ"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPOBJ"),
                                CommonRepository.GetPrefixeHisto(modeNavig, "KPINVEN"),
                                modeNavig == ModeConsultation.Standard ? "PBTYP" : string.Format("'{0}'", AlbConstantesMetiers.TypeHisto));

            var result = DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.Text, sql, param);

            return TransformRsqObjPlat(result);
        }

        public static List<RisqueDto> ObtenirRisquesSP(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEAVT", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            param[4] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
            param[5] = new EacParameter("P_CODEOPTION", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;

            var result = DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.StoredProcedure, modeNavig == ModeConsultation.Historique ? "SP_GETLISTRSQAPPLIQUE_HIST" : "SP_GETLISTRSQAPPLIQUE", param);

            return TransformRsqObjPlat(result);
        }

        #endregion

        #region Méthodes Privées

        private static List<RisqueDto> TransformRsqObjPlat(List<RisqueObjetPlatDto> lstRsqObj)
        {
            List<RisqueDto> risqueDto = new List<RisqueDto>();
            if (lstRsqObj != null)
            {
                var lstRsq = lstRsqObj.GroupBy(el => el.CodeRsq).Select(r => r.First()).ToList();
                lstRsq.ForEach(rsq =>
                {
                    var lstObj = lstRsqObj.FindAll(obj => obj.CodeRsq == rsq.CodeRsq).ToList();
                    List<ObjetDto> objetDto = new List<ObjetDto>();
                    lstObj.ForEach(obj =>
                    {
                        var lstInven = lstRsqObj.FindAll(inv => inv.CodeRsq == obj.CodeRsq && inv.CodeObj == obj.CodeObj).ToList();
                        List<InventaireDto> invenDto = new List<InventaireDto>();
                        lstInven.ForEach(inv =>
                        {
                            invenDto.Add(new InventaireDto
                            {
                                Code = inv.CodeInven,
                                Descriptif = inv.DescInven
                            });
                        });

                        //Ajout du 20151005 : ajout de l'objet dans liste si celui-ci n'existe pas déjà
                        if (objetDto.FindAll(el => el.Code == Convert.ToInt32(obj.CodeObj)).ToList().Count == 0)
                        {
                            objetDto.Add(new ObjetDto
                            {
                                Code = Convert.ToInt32(obj.CodeObj),
                                Designation = obj.DescObj,
                                Inventaires = invenDto,
                                IsUsed = obj.ObjUsed > 0 || obj.RsqUsed > 0,
                                IsOut = obj.ObjOut > 0 || obj.RsqOut > 0
                            });
                        }
                    });

                    risqueDto.Add(new RisqueDto
                    {
                        Code = Convert.ToInt32(rsq.CodeRsq),
                        Designation = rsq.DescRsq,
                        Cible = new CibleDto { GuidId = rsq.CodeCible, Description = rsq.DescCible },
                        Objets = objetDto,
                        IsUsed = rsq.RsqUsed > 0,
                        IsOut = rsq.RsqOut > 0
                    });
                });
            }

            return risqueDto;
        }

        #endregion

        #endregion

        #region Oppositions
        #region Méthodes Publiques

        /// <summary>
        /// retourne la liste des oppositions de l'offre/contrat risque en paramètre
        /// </summary>
        /// <param name="idOffre"></param>
        /// <param name="versionOffre"></param>
        /// <param name="typeOffre"></param>
        /// <param name="idRisque"></param>
        /// <returns></returns>
        public static List<OppositionDto> ObtenirListeOppositions(string idOffre, string versionOffre, string typeOffre, string idRisque)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("typeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = typeOffre;
            param[1] = new EacParameter("idOffre", DbType.AnsiStringFixedLength);
            param[1].Value = idOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("versionOffre", DbType.AnsiStringFixedLength);
            param[2].Value = versionOffre;
            param[3] = new EacParameter("idRisque", DbType.AnsiStringFixedLength);
            param[3].Value = idRisque;

            string sql = @"SELECT OPP.KFPID GUIDID, OPP.KFPTFI TYPE, TYPEFINANCEMENT.TPLIB TYPELABEL, 
		                                    CASE WHEN OPP.KFPTDS = 'AS' THEN ASSUREGEN.ANIAS ELSE ORGANISMEGEN.IMIIN END CODEORGANISME, 
		                                    CASE WHEN OPP.KFPTDS = 'AS' THEN ASSUREGEN.ANNOM ELSE ORGANISMEGEN.IMNOM END ORGANISME, 
		                                    CASE WHEN OPP.KFPTDS = 'AS' THEN ASSUREDET.ASVIL ELSE ORGANISMEDET.INVIL END VILLEORGANISME, 
		                                    CASE WHEN OPP.KFPTDS = 'AS' THEN ASSUREDET.ASDEP CONCAT ASSUREDET.ASCPO ELSE ORGANISMEDET.INCOM END CPORGANISME, 
		                                    CASE WHEN OPP.KFPTDS = 'AS' THEN ASSUREDET.ASPAY ELSE ORGANISMEDET.INPAY END PAYSORGANISME, PAYS.TPLIB NOMPAYS, 
		                                    OPPDESCR.KADDESI DESCRIPTION, OPP.KFPREF REFERENCE, OPP.KFPDECH ECHEANCE, OPP.KFPMNT MONTANT, 
		                                    IFNULL(KWD.W3IDORI,0) LIENFICHIERORIGINE, KWD.W3NOM ORGANISMEWARNING,
		                                    KWD.W3COM CPWARNING, KWD.W3VIL VILLEWARNING, KWD.W3PAY CODEPAYSWARNING, PAYSWRN.TPLIB NOMPAYSWARNING,
		                                    OPP.KFPTDS TYPEDEST
	                                    FROM KPOPP OPP		
		                                    LEFT JOIN ( 
			                                    SELECT KFQKFPID, MAX(KFQTYP) KFQTYP, MAX(KFQIPB) KFQIPB, MAX(KFQALX) KFQALX, MAX(KFQRSQ) KFQRSQ 
                                                    FROM KPOPPAP GROUP BY KFQKFPID 
				                                    ) PERIMETRE ON OPP.KFPTYP = PERIMETRE.KFQTYP AND OPP.KFPIPB = PERIMETRE.KFQIPB AND OPP.KFPALX = PERIMETRE.KFQALX AND OPP.KFPID = PERIMETRE.KFQKFPID 
                                            LEFT JOIN YINTNOM ORGANISMEGEN ON OPP.KFPIDCB = ORGANISMEGEN.IMIIN AND ORGANISMEGEN.IMINL = 0 AND ORGANISMEGEN.IMTNM = 'A' AND OPP.KFPTDS != 'AS'
		                                    LEFT JOIN YINTERV ORGANISMEDET ON ORGANISMEGEN.IMIIN = ORGANISMEDET.INIIN
		                                    LEFT JOIN YASSNOM ASSUREGEN ON OPP.KFPIDCB = ASSUREGEN.ANIAS AND ASSUREGEN.ANINL = 0 AND ASSUREGEN.ANTNM = 'A' AND OPP.KFPTDS = 'AS'
		                                    LEFT JOIN YASSURE ASSUREDET ON ASSUREGEN.ANIAS = ASSUREDET.ASIAS
		                                    INNER JOIN YYYYPAR TYPEFINANCEMENT ON TYPEFINANCEMENT.TCON = 'PRODU' AND TYPEFINANCEMENT.TFAM = 'QCMFI' AND TYPEFINANCEMENT.TCOD = OPP.KFPTFI 
		                                    LEFT JOIN KPDESI OPPDESCR ON OPP.KFPDESI = OPPDESCR.KADCHR
		                                    LEFT JOIN KWADR KWD ON KWD.W3ORI = 'KPOPP' AND KWD.W3IDORI = OPP.KFPID AND OPP.KFPIDCB=0
		                                    LEFT JOIN YYYYPAR PAYS ON PAYS.TCON = 'GENER' AND PAYS.TFAM = 'CPAYS'  
			                                    AND PAYS.TCOD = CASE WHEN OPP.KFPTDS = 'AS' THEN ASSUREDET.ASPAY ELSE ORGANISMEDET.INPAY END
		                                    LEFT JOIN YYYYPAR PAYSWRN ON PAYSWRN.TCON = 'GENER' AND PAYSWRN.TFAM = 'CPAYS'  AND PAYSWRN.TCOD = KWD.W3PAY
                            WHERE Opp.KFPTYP = :typeOffre AND Opp.KFPIPB = :idOffre AND Opp.KFPALX = :versionOffre AND Perimetre.KFQRSQ = :idRisque
                            ORDER BY Opp.KFPID";


            var lstOpposition = DbBase.Settings.ExecuteList<OppositionDto>(CommandType.Text, sql, param);
            if (lstOpposition != null)
            {
                lstOpposition.ForEach(c =>
                {
                    c.Echeance = AlbConvert.ConvertIntToDate(c.iEcheance);
                });
            }

            //Récupération des "s'applique à"
            foreach (OppositionDto opposition in lstOpposition)
            {
                var lst = ObtenirListeObjetsConcernesParOpposition(idOffre, versionOffre, typeOffre, idRisque, opposition.GuidId.ToString(), "RQ");
                if ((lst != null) && (lst.Count > 0))
                {
                    opposition.AppliqueAuRisqueEntier = true;
                    opposition.ObjetsRisque = null;
                }
                else
                {
                    opposition.AppliqueAuRisqueEntier = false;
                    opposition.ObjetsRisque = ObtenirListeObjetsConcernesParOpposition(idOffre, versionOffre, typeOffre, idRisque, opposition.GuidId.ToString(), "OB");
                }
            }

            return lstOpposition;
        }

        /// <summary>
        /// Obtient les informations complètes de l'opposition en paramètre
        /// </summary>
        public static OppositionDto ObtenirDetailOpposition(string idOffre, string versionOffre, string typeOffre, string codeAvn, string idRisque, string idOpposition, string mode, ModeConsultation modeNavig, string typeDest)
        {
            OppositionDto toReturn = new OppositionDto();

            if (mode != "I")
            {
                var sql = string.Empty;
                EacParameter[] param = new EacParameter[] {
                    new EacParameter("typeOffre", DbType.AnsiStringFixedLength) {Value = typeOffre},
                    new EacParameter("idOffre", DbType.AnsiStringFixedLength) {Value = idOffre.PadLeft(9, ' ')},
                    new EacParameter("versionOffre", DbType.AnsiStringFixedLength) {Value = versionOffre},
                    new EacParameter("idRisque", DbType.AnsiStringFixedLength) {Value = idRisque},
                    new EacParameter("idOpposition", DbType.AnsiStringFixedLength) {Value = idOpposition}
                };
                if (typeDest == "AS")
                {


                    sql = @"SELECT OPP.KFPID GUIDID, OPP.KFPTFI TYPE, ASSUREGEN.ANIAS CODEORGANISME, ASSUREGEN.ANNOM ORGANISME, ASSUREDET.ASVIL VILLEORGANISME, (ASSUREDET.ASDEP CONCAT ASSUREDET.ASCPO) CPORGANISME, ASSUREDET.ASPAY PAYSORGANISME, PAYS.TPLIB NOMPAYS, OPPDESCR.KADDESI DESCRIPTION, OPP.KFPDESI KDESIREF, OPP.KFPREF REFERENCE, OPP.KFPDECH ECHEANCE, OPP.KFPMNT MONTANT, ASSUREDET.ASAD1 ADRESSE1, ASSUREDET.ASAD2 ADRESSE2,
                                            IFNULL(KWD.W3IDORI,0) LIENFICHIERORIGINE, KWD.W3NOM ORGANISMEWARNING, KWD.W3AD1 ADRESSE1WARNING, KWD.W3AD2 ADRESSE2WARNING, KWD.W3COM CPWARNING, KWD.W3VIL VILLEWARNING, KWD.W3PAY CODEPAYSWARNING, PAYSWRN.TPLIB NOMPAYSWARNING	      
	                                    FROM KPOPP OPP		
		                                    LEFT JOIN KPOPPAP PERIMETRE ON OPP.KFPTYP = PERIMETRE.KFQTYP AND OPP.KFPIPB = PERIMETRE.KFQIPB AND OPP.KFPALX = PERIMETRE.KFQALX AND OPP.KFPID = PERIMETRE.KFQKFPID  
		                                    LEFT JOIN YASSNOM ASSUREGEN ON OPP.KFPIDCB = ASSUREGEN.ANIAS AND ANINL = 0 AND ANTNM = 'A'
		                                    LEFT JOIN YASSURE ASSUREDET ON ASSUREGEN.ANIAS = ASSUREDET.ASIAS
		                                    LEFT JOIN KPDESI OPPDESCR ON OPP.KFPDESI = OPPDESCR.KADCHR
		                                    LEFT JOIN KWADR KWD ON KWD.W3ORI = 'KPOPP' AND KWD.W3IDORI = OPP.KFPID AND OPP.KFPIDCB = 0
		                                    LEFT JOIN YYYYPAR PAYS ON PAYS.TCON = 'GENER' AND PAYS.TFAM = 'CPAYS'  AND PAYS.TCOD = ASSUREDET.ASPAY 
		                                    LEFT JOIN YYYYPAR PAYSWRN ON PAYSWRN.TCON = 'GENER' AND PAYSWRN.TFAM = 'CPAYS'  AND PAYSWRN.TCOD = KWD.W3PAY 
                                        WHERE OPP.KFPTYP = :typeOffre AND OPP.KFPIPB = :idOffre AND OPP.KFPALX = :versionOffre AND PERIMETRE.KFQRSQ = :idRisque AND OPP.KFPID = :idOpposition";
                }
                else
                {
                    sql = @"SELECT OPP.KFPID GUIDID, OPP.KFPTFI TYPE, ORGANISMEGEN.IMIIN CODEORGANISME, ORGANISMEGEN.IMNOM ORGANISME, ORGANISMEDET.INVIL VILLEORGANISME, ORGANISMEDET.INCOM CPORGANISME, ORGANISMEDET.INPAY PAYSORGANISME, PAYS.TPLIB NOMPAYS, OPPDESCR.KADDESI DESCRIPTION, OPP.KFPDESI KDESIREF, OPP.KFPREF REFERENCE, OPP.KFPDECH ECHEANCE, OPP.KFPMNT MONTANT, ORGANISMEDET.INAD1 ADRESSE1, ORGANISMEDET.INAD2 ADRESSE2,
                                            IFNULL(KWD.W3IDORI,0) LIENFICHIERORIGINE, KWD.W3NOM ORGANISMEWARNING, KWD.W3AD1 ADRESSE1WARNING, KWD.W3AD2 ADRESSE2WARNING, KWD.W3COM CPWARNING, KWD.W3VIL VILLEWARNING, KWD.W3PAY CODEPAYSWARNING, PAYSWRN.TPLIB NOMPAYSWARNING	      
	                                    FROM KPOPP OPP		
		                                    LEFT JOIN KPOPPAP PERIMETRE ON OPP.KFPTYP = PERIMETRE.KFQTYP AND OPP.KFPIPB = PERIMETRE.KFQIPB AND OPP.KFPALX = PERIMETRE.KFQALX AND OPP.KFPID = PERIMETRE.KFQKFPID  
		                                    LEFT JOIN YINTNOM ORGANISMEGEN ON OPP.KFPIDCB = ORGANISMEGEN.IMIIN AND IMINL = 0 AND IMTNM = 'A'
		                                    LEFT JOIN YINTERV ORGANISMEDET ON ORGANISMEGEN.IMIIN = ORGANISMEDET.INIIN
		                                    LEFT JOIN KPDESI OPPDESCR ON OPP.KFPDESI = OPPDESCR.KADCHR
		                                    LEFT JOIN KWADR KWD ON KWD.W3ORI = 'KPOPP' AND KWD.W3IDORI = OPP.KFPID AND OPP.KFPIDCB = 0
		                                    LEFT JOIN YYYYPAR PAYS ON PAYS.TCON = 'GENER' AND PAYS.TFAM = 'CPAYS'  AND PAYS.TCOD = ORGANISMEDET.INPAY 
		                                    LEFT JOIN YYYYPAR PAYSWRN ON PAYSWRN.TCON = 'GENER' AND PAYSWRN.TFAM = 'CPAYS'  AND PAYSWRN.TCOD = KWD.W3PAY
                                        WHERE OPP.KFPTYP = :typeOffre AND OPP.KFPIPB = :idOffre AND OPP.KFPALX = :versionOffre AND PERIMETRE.KFQRSQ = :idRisque AND OPP.KFPID = :idOpposition";
                }

                toReturn = DbBase.Settings.ExecuteList<OppositionDto>(CommandType.Text, sql, param).FirstOrDefault();
                if (toReturn == null)
                {
                    return null;
                }

                toReturn.Echeance = AlbConvert.ConvertIntToDate(toReturn.iEcheance);
                toReturn.ObjetsRisque = ObtenirListeObjetsConcernesParOpposition(idOffre, versionOffre, typeOffre, idRisque, idOpposition, "OB");
                var lst = ObtenirListeObjetsConcernesParOpposition(idOffre, versionOffre, typeOffre, idRisque, idOpposition, "RQ");
                if ((lst != null) && (lst.Count > 0))
                {
                    toReturn.AppliqueAuRisqueEntier = true;
                }
                else
                {
                    toReturn.AppliqueAuRisqueEntier = false;
                }
            }
            else//Mode I
            {
                toReturn.AppliqueAuRisqueEntier = true;
            }

            BrancheDto branche = CommonRepository.GetBrancheCible(idOffre, versionOffre, typeOffre, codeAvn, modeNavig);

            toReturn.TypesFinancement = ObtenirTypesFinancement(branche.Code, branche.Cible.Code);

            return toReturn;
        }

        /// <summary>
        /// Met à jour l'opposition en paramètre (création, modification, suppression)
        /// </summary>
        /// <param name="idOffre"></param>
        /// <param name="versionOffre"></param>
        /// <param name="typeOffre"></param>
        /// <param name="idRisque"></param>
        /// <param name="opposition"></param>
        /// <param name="objets"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int MiseAJourOpposition(string idOffre, string versionOffre, string typeOffre, string idRisque, OppositionDto opposition, string objets, string user)
        {
            //définition du périmètre de l'opposition
            string perimetre = string.Empty;
            List<string> listObjets = objets.Split(';').ToList();
            if (listObjets.Count == 0)
            {
                return 0;
            }

            if ((listObjets.Count == 2) && (listObjets.FirstOrDefault() == "0"))
            {
                perimetre = "RQ";
            }
            else
            {
                perimetre = "OB";
            }

            EacParameter[] param = new EacParameter[22];
            param[0] = new EacParameter("P_TYPE_OFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = typeOffre;
            param[1] = new EacParameter("P_ID_OFFRE", DbType.AnsiStringFixedLength);
            param[1].Value = idOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("P_VERSION_OFFRE", DbType.AnsiStringFixedLength);
            param[2].Value = versionOffre;
            param[3] = new EacParameter("P_CODE_RISQUE", DbType.Int32);
            param[3].Value = int.Parse(idRisque);
            param[4] = new EacParameter("P_OPP_KPOPP_REF", DbType.Int64);
            param[4].Value = opposition.GuidId;
            param[5] = new EacParameter("P_OPP_DESCRIPTION", DbType.AnsiStringFixedLength);
            param[5].Value = opposition.Description;
            param[6] = new EacParameter("P_OPP_KPDESI_REF", DbType.Int64);
            param[6].Value = opposition.KDESIRef;
            param[7] = new EacParameter("P_OPP_CODE_ORGANISME", DbType.Int32);
            param[7].Value = opposition.CodeOrganisme;
            param[8] = new EacParameter("P_OPP_TYPEFINANCEMENT", DbType.AnsiStringFixedLength);
            param[8].Value = opposition.TypeFinancement;
            param[9] = new EacParameter("P_OPP_REFERENCE", DbType.AnsiStringFixedLength);
            param[9].Value = opposition.Reference;
            param[10] = new EacParameter("P_OPP_ECHEANCE", DbType.Int32);
            param[10].Value = opposition.Echeance == null ? 0 : int.Parse(opposition.Echeance.Value.ToString("yyyyMMdd"));
            param[11] = new EacParameter("P_OPP_MONTANT", DbType.Double);
            param[11].Value = opposition.Montant;
            param[12] = new EacParameter("P_OPP_LISTOBJ", DbType.AnsiStringFixedLength);
            param[12].Value = objets;
            param[13] = new EacParameter("P_OPP_PERI", DbType.AnsiStringFixedLength);
            param[13].Value = perimetre;
            param[14] = new EacParameter("P_CREA_USER", DbType.AnsiStringFixedLength);
            param[14].Value = user;
            param[15] = new EacParameter("P_CREA_DATE", DbType.Int32);
            param[15].Value = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            param[16] = new EacParameter("P_CREA_HEURE", DbType.Int32);
            param[16].Value = int.Parse(DateTime.Now.ToString("HHmmss"));
            param[17] = new EacParameter("P_MAJ_USER", DbType.AnsiStringFixedLength);
            param[17].Value = user;
            param[18] = new EacParameter("P_MAJ_DATE", DbType.Int32);
            param[18].Value = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            param[19] = new EacParameter("P_MAJ_HEURE", DbType.Int32);
            param[19].Value = int.Parse(DateTime.Now.ToString("HHmmss"));
            param[20] = new EacParameter("P_TYPE_OPERATION", DbType.AnsiStringFixedLength);
            param[20].Value = opposition.Mode;
            param[21] = new EacParameter("P_TYPEDEST", DbType.AnsiStringFixedLength);
            param[21].Value = opposition.TypeDest;

            return DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_ADDOPP", param);
        }

        /// <summary>
        /// Obtient les organismes d'opposition correspondant au critère en paramètre
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mode"></param>
        /// <param name="typeOppBenef"></param>
        /// <returns></returns>
        public static List<OrganismeOppDto> OrganismesGet(string value, string mode, string typeOppBenef)
        {
            var whereQuery = string.Empty;
            int val;
            string sql;

            if (typeOppBenef == "AS")
            {
                #region Type Opposition/Beneficiaire : ASSURE
                if (mode == "Name" && !string.IsNullOrEmpty(value))
                {
                    whereQuery = string.Format(@"  WHERE  TRIM(LOWER(ANNOM))like '%{0}%' FETCH FIRST 10 ROWS ONLY",
                        value.Trim().ToLower());
                }

                if (mode == "Code" && int.TryParse(value, out val))
                {
                    whereQuery = string.Format(@"  WHERE  ANIAS = {0}", val);
                }

                sql = string.Format(@"SELECT 
	                                    ANIAS CODEORGANISME,
	                                    ANNOM ORGANISME, 
	                                    (asdep concat ascpo) CPORGANISME, 
	                                    asvil VILLEORGANISME, 
	                                    asPAY PAYSORGANISME,
                                        asad1 ADRESSE1,
                                        asad2 ADRESSE2,
                                        PAYS.TPLIB NOMPAYS
                                    FROM yassnom 
                                        INNER JOIN yassure ON ANIAS = ASIAS AND ANINL = 0 AND ANTNM = 'A'
                                        {1}
                                   {0}", whereQuery,
                    CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "CPAYS", "PAYS", " AND PAYS.TCOD = asPAY"));

                #endregion
            }
            else
            {
                #region Type Opposition/Beneficiaire : INTERVENANT
                if (mode == "Name" && !string.IsNullOrEmpty(value))
                {
                    whereQuery = string.Format(@"  WHERE detail.INTYI = 'CB' AND TRIM(LOWER(base.IMNOM))like '%{0}%' FETCH FIRST 10 ROWS ONLY",
                        value.Trim().ToLower());
                }

                if (mode == "Code" && int.TryParse(value, out val))
                {
                    whereQuery = string.Format(@"  WHERE detail.INTYI = 'CB' AND base.IMIIN = {0}", val);
                }

                if (String.IsNullOrEmpty(mode))
                {
                    whereQuery = string.Format(@"  WHERE detail.INTYI = 'CB'");
                }
                sql = string.Format(@"SELECT 
	                                    base.IMIIN CODEORGANISME,
	                                    base.IMNOM ORGANISME, 
	                                    detail.INCOM CPORGANISME, 
	                                    detail.INVIL VILLEORGANISME, 
	                                    detail.INPAY PAYSORGANISME,
                                        detail.INAD1 ADRESSE1,
                                        detail.INAD2 ADRESSE2,
                                        PAYS.TPLIB NOMPAYS
                                    FROM 
	                                    YINTNOM Base INNER JOIN 
	                                    YINTERV detail ON                                         
		                                    Base.IMIIN = detail.INIIN 
		                                    AND Base.IMINL = 0 
		                                    AND Base.IMTNM = 'A'
                                        {1}
                                   {0}", whereQuery,
                    CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER",
                        "CPAYS",
                        "PAYS", " AND PAYS.TCOD = detail.INPAY"));

                #endregion
            }

            return !string.IsNullOrEmpty(sql) ? DbBase.Settings.ExecuteList<OrganismeOppDto>(CommandType.Text, sql) : null;
        }

        #endregion

        #region Méthodes privées

        private static List<ParametreDto> ObtenirListeObjetsConcernesParOpposition(string idOffre, string versionOffre, string typeOffre, string idRisque, string idOpposition, string perimetre)
        {
            EacParameter[] param = new EacParameter[] {
                new EacParameter("typeOffre", DbType.AnsiStringFixedLength) {Value = typeOffre},
                new EacParameter("idOffre", DbType.AnsiStringFixedLength) {Value = idOffre.PadLeft(9, ' ')},
                new EacParameter("versionOffre", DbType.Int32) {Value = versionOffre},
                new EacParameter("idOpposition", DbType.Int32) {Value = idOpposition},
                new EacParameter("idRisque", DbType.Int32) {Value = idRisque},
                new EacParameter("perimetre", DbType.AnsiStringFixedLength) {Value = perimetre}
            };
            var sql = @"SELECT CAST(KFQOBJ AS CHAR(15)) CODE, '' LIBELLE from KPOPPAP Perimetre
	                                        WHERE 
		                                        Perimetre.KFQTYP = :typeOffre                                                
		                                        AND Perimetre.KFQIPB = :idOffre
		                                        AND Perimetre.KFQALX = :versionOffre
		                                        AND Perimetre.KFQKFPID = :idOpposition
		                                        AND Perimetre.KFQRSQ = :idRisque
                                                AND Perimetre.KFQPERI = :perimetre";

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param).ToList();
        }

        private static List<ParametreDto> ObtenirTypesFinancement(string branche, string cible)
        {
            string sql = CommonRepository.BuildSelectYYYYPAR(branche, cible, string.Empty, "base.TCOD CODE, base.TPLIB LIBELLE", "PRODU", "QCMFI", "base");

            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql).OrderBy(m => m.Code).ToList();
        }

        #endregion

        #endregion

        #region Clauses

        public static RisqueDto GetRisque(string codeOffre, string version, string type, string codeRsq)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeRsq", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0;

            string sql = @"SELECT KABRSQ CODERSQ, KABDESC DESCRSQ,
                            KACOBJ CODEOBJ, KACDESC DESCOBJ
                        FROM KPRSQ
                            INNER JOIN KPOBJ ON KACIPB = KABIPB AND KACALX = KABALX AND KACTYP = KABTYP AND KACRSQ = KABRSQ
                        WHERE KABIPB = :codeOffre AND KABALX = :version AND KABTYP = :type AND KABRSQ = :codeRsq
                        ORDER BY KABRSQ, KACOBJ";

            var result = DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.Text, sql, param);

            return TransformRsqObjPlat(result).FirstOrDefault();
        }
        public static List<ParametreDto> GetListEtapes(OrigineAppel origine)
        {
            string sql = string.Empty;
            switch (origine)
            {
                case OrigineAppel.OffreSimp:
                    sql = @"SELECT TCOD CODE, TPLIL LIBELLE
                            FROM YYYYPAR
                            INNER JOIN KALETAP ON KEHETAPE=TCOD AND KEHSERV = 'PRODU' AND KEHACTG = '**'  AND KEHSAIT = 'O' AND KEHGCLA='O'
                            WHERE TCON='KHEOP' AND TFAM='ETAPE'";
                    break;
                case OrigineAppel.Generale:
                    sql = @"SELECT TCOD CODE, TPLIL LIBELLE
                            FROM YYYYPAR
                            INNER JOIN KALETAP ON KEHETAPE=TCOD AND KEHSERV = 'PRODU' AND KEHACTG = '**' AND KEHGCLA = 'O'
                            WHERE TCON='KHEOP' AND TFAM='ETAPE'";
                    break;
                case OrigineAppel.Attestation:
                    sql = @"SELECT TCOD CODE, TPLIL LIBELLE
                            FROM YYYYPAR
                            INNER JOIN KALETAP ON KEHETAPE=TCOD AND KEHSERV = 'PRODU' AND KEHACTG = 'ATTES' 
                            WHERE TCON='KHEOP' AND TFAM='ETAPE'";
                    break;

                case OrigineAppel.Regule:
                    sql = @"SELECT TCOD CODE, TPLIL LIBELLE
                            FROM YYYYPAR
                            INNER JOIN KALETAP ON KEHETAPE=TCOD AND KEHSERV = 'PRODU' AND KEHACTG = 'REGUL' 
                            WHERE TCON='KHEOP' AND TFAM='ETAPE'";
                    break;
                default:
                    sql = @"SELECT TCOD CODE, TPLIL LIBELLE
                            FROM YYYYPAR
                            INNER JOIN KALETAP ON KEHETAPE=TCOD AND KEHSERV = 'PRODU' AND KEHACTG = '**' AND KEHGCLA = 'O'
                            WHERE TCON='KHEOP' AND TFAM='ETAPE'";
                    break;
            }
            return DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql);
        }
        public static List<ParametreDto> GetListContextes(string codeEtape, string codeOffre, string version, string type, string codeAvn, string contexte, string famille, ModeConsultation modeNavig)
        {
            var toReturn = new List<ParametreDto>();
            if (string.IsNullOrEmpty(codeEtape))
            {
                return toReturn;
            }

            BrancheDto branche = CommonRepository.GetBrancheCible(codeOffre, version, type, codeAvn, modeNavig);

            var listContextesParam = CommonRepository.GetParametres(branche.Code, branche.Cible.Code, contexte, famille);
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("typeAttestation", DbType.AnsiStringFixedLength);
            param[0].Value = (codeEtape != AlbConstantesMetiers.TYPE_ATTESTATION) ? "**" : codeEtape;
            param[1] = new EacParameter("codeEtape", DbType.AnsiStringFixedLength);
            param[1].Value = codeEtape;

            string sql = @"SELECT KEICTX CODE, KEIID LONGID FROM KALCONT                                     
                                     WHERE KEISERV='PRODU' AND KEIACTG = :typeAttestation AND KEIETAPE=:codeEtape
                                     ORDER BY KEICTX";
            var listCodesContextes = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);
            if (listContextesParam != null && listCodesContextes != null)
            {
                foreach (var cp in listContextesParam)
                {
                    foreach (var c in listCodesContextes)
                    {
                        if (c.Code == cp.Code)
                        {
                            toReturn.Add(new ParametreDto { Code = cp.Code, Libelle = cp.Libelle, LongId = c.LongId });
                            break;
                        }
                    }
                }
            }
            return toReturn;
        }
        //SAB24042016: Pagination clause     
        public static ChoixClausesInfoDto GetInfoChoixClauses(OrigineAppel origine,
                      string type, string codeOffre, string version, string codeAvn, string etape, string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig,
                      string codeEtape, string contexte, string famille)
        {
            ChoixClausesInfoDto toReturn = new ChoixClausesInfoDto();
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = -1 };
            Parallel.Invoke(parallelOptions,
                () =>
                {
                    toReturn.Etapes = GetListEtapes(origine);
                },
                () =>
                {
                    toReturn.Clauses = ClauseRepository.ObtenirBaseInfosClauses(type, codeOffre, version, codeAvn, etape, filtreContexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig);
                },
                () =>
                {
                    toReturn.ContextesCibles = GetListContextes(codeEtape, codeOffre, version, type, codeAvn, contexte, famille, modeNavig);
                });
            return toReturn;
        }


        public static EnregistrementClauseLibreDto EnregistrerClauseLibre(string codeOffre, string version, string type, string codeAvn, string contexteClause, string etape, string codeRisque, string codeFormule, string codeOption, string codeObj, string libelleClauseLibre, string texteClauseLibre,
                         OrigineAppel origine,
                             string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
                             string codeEtape, string contexte, string famille)
        {
            EnregistrementClauseLibreDto toReturn = new EnregistrementClauseLibreDto();
            toReturn.retourMsg = ClauseRepository.EnregistrerClauseLibre(codeOffre, version, type, contexteClause, etape, codeRisque, codeFormule, codeOption, codeObj, libelleClauseLibre, texteClauseLibre);
            toReturn.ChoixClausesInfo = GetInfoChoixClauses(origine, type, codeOffre, version, codeAvn, etape, filtreContexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig, codeEtape, contexte, famille);
            return toReturn;
        }

        public static EnregistrementClauseLibreDto UpdateClauseLibre(string clauseId, string titreClauseLibre, string texteClauseLibre, string codeObj,
                              string codeOffre, string version, string type, string codeAvn, string etape, string codeRisque, string codeFormule, string codeOption, OrigineAppel origine,
                              string filtreContexte, string rubrique, string sousrubrique, string sequence, string idClause, string filtre, ModeConsultation modeNavig,
                              string codeEtape, string contexte, string famille)
        {
            ClauseRepository.UpdateTextClauseLibre(clauseId, titreClauseLibre, texteClauseLibre, codeObj);
            EnregistrementClauseLibreDto toReturn = new EnregistrementClauseLibreDto();
            toReturn.retourMsg = string.Empty;
            toReturn.ChoixClausesInfo = GetInfoChoixClauses(origine, type, codeOffre, version, codeAvn, etape, filtreContexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig, codeEtape, contexte, famille);
            return toReturn;
        }


        public static ClauseVisualisationDto ClausesGet(string type, string codeOffre, string version, string codeAvn, string etape, string contexte, string rubrique, string sousrubrique, string sequence, string idClause, string codeRisque, string codeFormule, string codeOption, string filtre, ModeConsultation modeNavig)
        {
            ClauseVisualisationDto toReturn = new ClauseVisualisationDto();
            toReturn.Clauses = ClauseRepository.ObtenirClauses(type, codeOffre, version, codeAvn, etape, contexte, rubrique, sousrubrique, sequence, idClause, codeRisque, codeFormule, codeOption, filtre, modeNavig);
            if (!string.IsNullOrEmpty(codeRisque))
            {
                toReturn.Risques = RisqueRepository.GetRisque(codeOffre, version, type, codeRisque);
            }
            return toReturn;
        }



        #endregion


        #region Details Risque

        public static string LoadComplementNum1(string concept, string famille, string code)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("concept", DbType.AnsiStringFixedLength);
            param[0].Value = concept;
            param[1] = new EacParameter("famille", DbType.AnsiStringFixedLength);
            param[1].Value = famille;
            param[2] = new EacParameter("code", DbType.AnsiStringFixedLength);
            param[2].Value = code;
            string sql = string.Format(@"SELECT TPCN1 DECRETURNCOL FROM YYYYPAR 
                                            WHERE TCON = :concept AND TFAM = :famille AND TCOD = :code",
                                        concept, famille, code);

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                return result.FirstOrDefault().DecReturnCol.ToString();
            }

            return string.Empty;
        }

        public static string LoadComplementAlpha2(string concept, string famille, string code)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("concept", DbType.AnsiStringFixedLength);
            param[0].Value = concept;
            param[1] = new EacParameter("famille", DbType.AnsiStringFixedLength);
            param[1].Value = famille;
            param[2] = new EacParameter("code", DbType.AnsiStringFixedLength);
            param[2].Value = code;

            string sql = @"SELECT TPCA1 STRRETURNCOL FROM YYYYPAR 
                                            WHERE TCON = :concept AND TFAM = :famille AND TCOD = :code";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                return result.FirstOrDefault().StrReturnCol;
            }

            return string.Empty;
        }

        public static List<ParametreDto> GetListeTypesRegularisation(string branche, string cible)
        {
            return CommonRepository.GetParametres(branche, cible, "PRODU", "JERUT");
        }

        public static bool CheckObjetSorit(string codeOffre, int? version, string type, string codeAvn, string openObj)
        {

            string coderisque = openObj.Split('_')[0];
            string codeobjet = openObj.Split('_')[1];

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            //param[2] = new EacParameter("type", type);
            param[2] = new EacParameter("codersq", DbType.Int32);
            param[2].Value = coderisque;
            param[3] = new EacParameter("codeobj", DbType.Int32);
            param[3].Value = codeobjet;

            string sql = @"SELECT JGVFA ENTREEGARANTIEANNEE, JGVFM ENTREEGARANTIEMOIS, JGVFJ ENTREEGARANTIEJOUR, JGVFH ENTREEGARANTIEHEURE, 
                                    JEAVA SORTIEGARANTIEANNEE, JEAVM SORTIEGARANTIEMOIS, JEAVJ SORTIEGARANTIEJOUR, JGAVE AVNCREATION
                        FROM YPRTOBJ 
                        LEFT JOIN YPRTRSQ ON JEIPB=JGIPB AND JEALX= JGALX AND JERSQ = JGRSQ
                      WHERE JGIPB = :codeoffre AND JGALX = :version AND JGRSQ= :codersq AND JGOBJ = :codeobj";

            var result = DbBase.Settings.ExecuteList<ObjetPlatDto>(CommandType.Text, sql, param);


            if (result != null && result.Any())
            {
                var frstRes = result.FirstOrDefault();
                DateTime? idatesortiegarantie = AlbConvert.GetDate(frstRes.EntreeGarantieAnnee, frstRes.EntreeGarantieMois, frstRes.EntreeGarantieJour, frstRes.EntreeGarantieHeure);
                DateTime? idateeffetavenant = AlbConvert.GetDate(frstRes.SortieGarantieAnnee, frstRes.SortieGarantieMois, frstRes.SortieGarantieJour, 0);

                if (idatesortiegarantie != null && idateeffetavenant != null && frstRes.AvnCreationObj.ToString() != codeAvn)
                {
                    if (idatesortiegarantie.Value < idateeffetavenant.Value.AddMinutes(-1))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        //Retourne la valeur du questionnaire médical
        public static string GetQuestionMedical(string codeAffaire, string version, string type, string codeRsq, string codeObj, string oldValue, bool controlAssiette, string user)
        {
            string toReturn = string.Empty;

            var param = new List<EacParameter> {
                new EacParameter("codeAffaire", DbType.AnsiStringFixedLength) {Value = codeAffaire.PadLeft(9, ' ')},
                new EacParameter("version", DbType.Int32) {Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0},
                new EacParameter("type", DbType.Int32) {Value = type},
                new EacParameter("codeRsq", DbType.Int32) {Value = !string.IsNullOrEmpty(codeRsq) ? Convert.ToInt32(codeRsq) : 0}

            };

            string sql = @"SELECT DISTINCT KFAQMD STRRETURNCOL FROM KPIRSOB WHERE KFAIPB = :codeAffaire AND KFAALX = :version AND KFATYP = :type AND KFARSQ = :codeRsq";
            if (!string.IsNullOrEmpty(codeObj) && codeObj != "0")
            {
                sql += " AND KFAOBJ = :codeObj";
                param.Add(new EacParameter("codeObj", DbType.Int32) { Value = !string.IsNullOrEmpty(codeObj) ? Convert.ToInt32(codeObj) : 0 });
            }

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                toReturn = result.FirstOrDefault().StrReturnCol;
            }

            if (controlAssiette && toReturn != oldValue)
            {
                param = new List<EacParameter> {
                new EacParameter("codeAffaire", DbType.AnsiStringFixedLength) {Value = codeAffaire.PadLeft(9, ' ')},
                new EacParameter("version", DbType.Int32) {Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0},
                new EacParameter("type", DbType.Int32) {Value = type}
               };
                if (!CommonRepository.ExistRowParam("SELECT COUNT ( * ) NBLIGN FROM KPCTRLA WHERE KGTIPB = :codeAffaire AND KGTALX = :version AND KGTTYP = :type", param))
                {
                    CommonRepository.InsertControlAssiette(codeAffaire, version, type, "IS Rsq/Obj", "Q.M.", user);
                }
            }

            return toReturn;
        }


        #endregion

        #region Nomenclatures

        public static List<NomenclatureDto> GetComboNomenclatures(string codeOffre, int version, string type, int codeRisque, int codeObj, string cible)
        {

            if (!string.IsNullOrEmpty(cible))
            {
                EacParameter[] param = new EacParameter[6];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = version;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = type;
                param[3] = new EacParameter("P_CODERSQ", DbType.Int32);
                param[3].Value = codeRisque;
                param[4] = new EacParameter("P_CODEOBJ", DbType.Int32);
                param[4].Value = codeObj;
                param[5] = new EacParameter("P_CIBLE", DbType.AnsiStringFixedLength);
                param[5].Value = cible;

                var result = DbBase.Settings.ExecuteList<NomenclatureDto>(CommandType.StoredProcedure, "SP_GETCOMBONOMENCLATURE", param);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static List<NomenclatureDto> GetSpecificListeNomenclature(long idNomenclatureParent, int numeroCombo, string cible, string idNom1, string idNom2, string idNom3, string idNom4, string idNom5)
        {
            var parameters = new List<EacParameter>
            {
                //new EacParameter("cible", cible),
                new EacParameter("niv1", idNom1)
            };

            string sql = string.Format(@"SELECT {0} NUMCOMBO, IFNULL(K{1}.KHIID, 0) IDVALEUR, G.KHJLIEN{0} NIVCOMBO, G.KHJLIB{0} LIBELLECOMBO, 
IFNULL(K{1}.KHINMC, '') CODENOMEN, IFNULL(K{1}.KHIDESI, '') LIBNOMEN, IFNULL(K{1}.KHINORD, 0) ORDREVAL
FROM KCIBLE C 
	INNER JOIN KNMGRI G ON (C.KAHNMG) = (G.KHJNMG) 
	INNER JOIN KNMVALF K ON (K.KHKNMG, K.KHKNIV, K.KHKTYPO) = (G.KHJNMG, 1, G.KHJTYPO1)
	INNER JOIN KNMREF K2 ON (K2.KHIID, K2.KHIID) = (K.KHKKHIID, :niv1)
	INNER JOIN KNMVALF k3 ON (k3.KHKMER, k3.KHKNIV) = (k.KHKID, 2)
	INNER JOIN KNMREF k4 ON k4.KHIID = k3.KHKKHIID ", numeroCombo, numeroCombo * 2);

            if (numeroCombo >= 3)
            {
                parameters.Add(new EacParameter("niv2", idNom2));

                sql += @" AND k4.KHIID = :niv2
	INNER JOIN KNMVALF k5 ON (k5.KHKMER, k5.KHKNIV) = (k3.KHKID, 3)
	INNER JOIN KNMREF k6 ON k6.KHIID = k5.KHKKHIID ";
            }

            if (numeroCombo >= 4)
            {
                parameters.Add(new EacParameter("niv3", idNom3));

                sql += @" AND k6.KHIID = :niv3
	INNER JOIN KNMVALF k7 ON k7.KHKMER = k5.KHKID 
	INNER JOIN KNMREF k8 ON k8.KHIID = k7.KHKKHIID ";
            }

            if (numeroCombo >= 5)
            {
                parameters.Add(new EacParameter("niv4", idNom4));

                sql += @" AND k8.KHIID = :niv4
	INNER JOIN KNMVALF k9 ON k9.KHKMER = k7.KHKID 
	INNER JOIN KNMREF k10 ON k10.KHIID = k9.KHKKHIID ";
            }

            sql += @"WHERE C.KAHCIBLE = :cible
ORDER BY ORDREVAL";


            //            var parameters = new List<EacParameter> {
            //                new EacParameter("idNom", idNomenclatureParent),
            //                new EacParameter("cible", cible)
            //            };

            //            string sql = string.Format(@"SELECT DISTINCT {0} NUMCOMBO, IFNULL(KHIID, 0) IDVALEUR, KHJLIEN{0} NIVCOMBO, KHJLIB{0} LIBELLECOMBO, IFNULL(KHINMC, '') CODENOMEN, IFNULL(KHIDESI, '') LIBNOMEN, IFNULL(KHINORD, 0) ORDREVAL
            //FROM KNMGRI
            //LEFT JOIN KNMVALF ON KHKNMG = KHJNMG AND KHKTYPO = KHJTYPO{0} AND KHKNIV = {0}
            //LEFT JOIN KNMREF ON KHKKHIID = KHIID
            //WHERE KHKMER IN (SELECT KHKID FROM KCIBLE 
            //INNER JOIN KNMGRI ON KHJNMG = KAHNMG 
            //INNER JOIN KNMVALF ON (KHKTYPO, KHKNMG, KHKKHIID) = (KHJTYPO{1}, KHJNMG, :idNom)
            //WHERE KAHCIBLE = :cible)
            //ORDER BY ORDREVAL", numeroCombo, (numeroCombo - 1));

            parameters.Add(new EacParameter("cible", cible));

            var result = DbBase.Settings.ExecuteList<NomenclatureDto>(CommandType.Text, sql, parameters.ToArray());
            return result;
        }

        #endregion


        #region Vérification trace régularisation
        /// <summary>
        /// B3101
        /// Vérification trace régularisation
        /// </summary>
        /// <param name="codeContrat"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HaveTraceRegularisation(string codeContrat, string codeRisque, string version, string type, string numAvn)
        {
            var result = false;
            try
            {
                var param = new EacParameter[5];

                param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength)
                {
                    Value = codeContrat.PadLeft(9, ' ')
                };
                param[1] = new EacParameter("codeRisque", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 1
                };
                param[2] = new EacParameter("version", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                };
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength)
                {
                    Value = type
                };
                param[4] = new EacParameter("numAvn", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(numAvn) ? Convert.ToInt32(numAvn) : 0
                };

                string sql = @"SELECT  COUNT ( * ) NBLIGN FROM KPHAVH WHERE KIGIPB =:codeContrat AND KIGRSQ =:codeRisque AND KIGALX =:version AND KIGTYP =:type AND KIGAVN =:numAvn AND KIGRUL='O'";

                result = CommonRepository.ExistRowParam(sql, param);

            }
            catch (Exception)
            {

                throw;
            }
            return result;

        }


        /// <summary>
        /// Recuperation date fin effet 
        /// </summary>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeAvn"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        public static bool? GetIsRegularisation(string codeContrat, string codeRisque, string version)
        {
            bool? result = null;
            try
            {
                var param = new EacParameter[3];

                param[0] = new EacParameter("codeContrat", DbType.AnsiStringFixedLength)
                {
                    Value = codeContrat.PadLeft(9, ' ')
                };
                param[1] = new EacParameter("version", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0
                };
                param[2] = new EacParameter("codeRisque", DbType.Int32)
                {
                    Value = !string.IsNullOrEmpty(codeRisque) ? Convert.ToInt32(codeRisque) : 1
                };

                string sql = @"SELECT JERUL STRRETURNCOL FROM YPRTRSQ WHERE JEIPB =:codeContrat AND JEALX =:version AND JERSQ=:codeRisque";
                result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault()?.StrReturnCol == "O";


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

