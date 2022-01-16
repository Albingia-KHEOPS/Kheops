using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Bloc;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.FormuleGarantie;
using OP.WSAS400.DTO.FormuleGarantie.GarantieModele;
using OP.WSAS400.DTO.GarantieModele;
using OP.WSAS400.DTO.MatriceFormule;
using OP.WSAS400.DTO.Modeles;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.Volet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace OP.DataAccess
{
    public partial class FormuleRepository
    {
        #region Ecran Formule de Garantie

        #region Méthodes Publiques

        public static List<ParamNatGarDto> GetParamNatGar()
        {
            var sql = "SELECT KAUCAR CARACTERE, KAUNAT NATURE, KAUGANC NATUREPARAMCHECKED, KAUGANNC NATUREPARAMNONCHECKED FROM KGANPAR";
            return DbBase.Settings.ExecuteList<ParamNatGarDto>(CommandType.Text, sql);
        }

        

      public static decimal GetSumPrimeGarantie(string codeAff, long codeFromule)
        {
            decimal sum = 0;
            var param = new List<EacParameter>{
                 new EacParameter("codeOffre", codeAff.PadLeft(9, ' ')),
                 new EacParameter("codeFromule", Convert.ToInt32(codeFromule)),

            };

            string sql = @"SELECT SUM(KDGPRIVALO) FROM KPGARAN  
                                    INNER JOIN KGARAN  ON KDEGARAN = GAGAR
                                    INNER JOIN KPGARTAR   ON KDEID = KDGKDEID
                           WHERE KDEIPB=:codeOffre AND KDEFOR = :codeFromule AND   GAATG  = 'O' AND KDGPRIVALO > 0 AND KDGPRIUNIT ='D'";
            var result = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);
            if (!DBNull.Value.Equals(result))
            {
                sum = Convert.ToDecimal(result.ToString().Replace('.', ','));
            }

            return sum;
        
        }
        public static List<FormuleDto> GetFormuleIdByRsq(string codeOffre, int numRsq)
        {
            var param = new List<EacParameter>{
                 new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                 new EacParameter("numRsq", Convert.ToInt32(numRsq)),

            };

            string sql = @"SELECT KDDFOR CODE FROM KPOPTAP WHERE KDDIPB = :codeOffre AND KDDRSQ = :numRsq";


            return DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param);
        }
        public static List<FormuleDto> GetIdGar (string codeOffre, long codeFormule)
        {
            var param = new List<EacParameter>{
                 new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                 new EacParameter ("codeFormule", Convert.ToInt64(codeFormule)),
            
        };

            string sql = @"SELECT KDEID CODE FROM KPGARAN 
                                  INNER JOIN KPOPTD ON KDEKDCID = KDCID
                           WHERE KDEIPB =:codeOffre AND KDEFOR =:codeFormule AND KDEGARAN = 'ATTENT'";


            return DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param);
        }

        public static void UpdateKpgartar(long id,decimal? PrimeGaranties)
        {
            var param = new List<EacParameter>{
                 new EacParameter ("PrimeGaranties", Convert.ToDecimal(PrimeGaranties)),
                 new EacParameter ("id", Convert.ToInt64(id)),

            };

        
            string sql = @"UPDATE KPGARTAR 
                                    SET KDGPRIVALO = :PrimeGaranties,KDGPRIVALA  = :PrimeGaranties , KDGPRIUNIT  = 'D'  
                            WHERE KDGKDEID =:id ";

          DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }
        
        private static void CleanInfosCanevas(DtoCommon item, bool totalRegeneration)
        {
            var parameters = new List<EacParameter> {
                new EacParameter("ipb", DbType.AnsiStringFixedLength) { Value = item.StrReturnCol.PadLeft(9, ' ') },
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value = item.Type },
                new EacParameter("alx", DbType.Int32) { Value = item.Int32ReturnCol }
            };

            var inven = DbBase.Settings.ExecuteList<DtoCommon>(
                CommandType.Text,
                "SELECT KDEINVEN ID FROM KPGARAN WHERE KDEIPB = :ipb AND KDETYP = :type AND KDEALX = :alx",
                parameters.ToArray()).FirstOrDefault();

            if (inven != null && inven.Id != 0)
            {
                var paramsInventaire = parameters.ToList();
                paramsInventaire.Add(new EacParameter("invenId", DbType.Int64) { Value = inven.Id });
                var kbekadid = DbBase.Settings.ExecuteList<DtoCommon>(
                    CommandType.Text,
                    "SELECT KBEKADID INT64RETURNCOL FROM KPINVEN WHERE KBEIPB = :ipb AND KBETYP = :type AND KBEALX = :alx AND KBEID = :invenId",
                    paramsInventaire.ToArray()).FirstOrDefault();

                if (kbekadid != null)
                {
                    var paramsDesignation = parameters.ToList();
                    paramsDesignation.Add(new EacParameter("kadid", DbType.Int64) { Value = kbekadid.Int64ReturnCol });
                    DbBase.Settings.ExecuteNonQuery(
                        CommandType.Text,
                        "DELETE FROM KPDESI WHERE KADIPB = :ipb AND KADTYP = :type AND KADALX = :alx AND KADCHR = :kadid",
                        paramsDesignation.ToArray());
                }

                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "DELETE FROM KPINVAPP WHERE KBGIPB = :ipb AND KBGTYP = :type AND KBGALX = :alx AND KBGKBEID = :invenId",
                    paramsInventaire.ToArray());

                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "DELETE FROM KPINVED WHERE KBFIPB = :ipb AND KBFTYP = :type AND KBFALX = :alx AND KBFKBEID = :invenId",
                    paramsInventaire.ToArray());

                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "DELETE FROM KPINVEN WHERE KBEIPB = :ipb AND KBETYP = :type AND KBEALX = :alx AND KBEID = :invenId",
                    paramsInventaire.ToArray());
            }

            //const string sqlLCI = @"
            //SELECT DISTINCT KDIID ID, KDIDESI DATEDEBRETURNCOL FROM KPEXPLCI
            //INNER JOIN KPGARTAR ON KDIID = KDGKDIID
            //WHERE UPPER(KDGIPB) = :ipb AND KDGKDIID != 0";

            const string sqlLCI = @"SELECT KDIID ID, KDIDESI DATEDEBRETURNCOL FROM KPEXPLCI L
                            INNER JOIN KPGARTAR ON KDIID = KDGKDIID
                        WHERE KDGIPB = :ipb AND KDGKDIID != 0
                            AND NOT EXISTS (SELECT * FROM KPRSQ R WHERE L.KDIID = R.KABKDIID)    
                            AND NOT EXISTS (SELECT * FROM KPENT E WHERE L.KDIID = E.KAAKDIID)";

            //            const string sqlLCI = @"
            //SELECT DISTINCT KDIID ID, KDIDESI DATEDEBRETURNCOL FROM KPEXPLCI
            //WHERE UPPER(KDIIPB) = :ipb";
            var listLci = DbBase.Settings.ExecuteList<DtoCommon>(
                CommandType.Text,
                sqlLCI,
                new[] { parameters.First() });

            //const string sqlFRH = @"
            //SELECT DISTINCT KDKID ID, KDKDESI DATEDEBRETURNCOL FROM KPEXPFRH
            //INNER JOIN KPGARTAR ON KDKID = KDGKDKID
            //WHERE UPPER(KDGIPB) = :ipb AND KDGKDKID != 0";

            const string sqlFRH = @"SELECT KDKID ID, KDKDESI DATEDEBRETURNCOL FROM KPEXPFRH F
	                        INNER JOIN KPGARTAR ON KDKID = KDGKDKID
                        WHERE KDGIPB = :ipb AND KDGKDKID != 0
	                        AND NOT EXISTS (SELECT * FROM KPRSQ R WHERE F.KDKID = R.KABKDKID)	
	                        AND NOT EXISTS (SELECT * FROM KPENT E WHERE F.KDKID = E.KAAKDKID)";

            //const string SqlFrh = @"SELECT DISTINCT KDKID ID, KDKDESI DATEDEBRETURNCOL FROM KPEXPFRH
            //                        WHERE UPPER(KDKIPB) =:ipb";

            var listFrh = DbBase.Settings.ExecuteList<DtoCommon>(
                CommandType.Text,
                sqlFRH,
                new[] { parameters.First() });

            foreach (var itemLci in listLci)
            {
                if (itemLci.DateDebReturnCol != 0)
                {
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, "DELETE FROM KPDESI WHERE KADCHR = :id", new[] { new EacParameter("id", DbType.Int64) { Value = itemLci.DateDebReturnCol } });
                }

                var paramLciId = new EacParameter("id", DbType.Int64) { Value = itemLci.Id };
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, "DELETE FROM KPEXPLCID WHERE KDJKDIID = :id", new[] { paramLciId });
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, "DELETE FROM KPEXPLCI WHERE KDIID = :id", new[] { paramLciId });
            }

            foreach (var itemFrh in listFrh)
            {
                if (itemFrh.DateDebReturnCol != 0)
                {
                    DbBase.Settings.ExecuteNonQuery(
                        CommandType.Text,
                        "DELETE FROM KPDESI WHERE KADCHR = :id",
                        new[] { new EacParameter("id", DbType.Int64) { Value = itemFrh.DateDebReturnCol } });
                }

                EacParameter paramFrhId = new EacParameter("id", DbType.Int64) { Value = itemFrh.Id };
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, "DELETE FROM KPEXPFRHD WHERE KDLKDKID = :id", new[] { paramFrhId });
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, "DELETE FROM KPEXPFRH WHERE KDKID = :id", new[] { paramFrhId });
            }

            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "DELETE FROM KPGARAN WHERE KDEIPB = :ipb AND KDETYP = :type AND KDEALX = :alx",
                parameters.ToArray());
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "DELETE FROM KPGARTAR WHERE KDGIPB =:ipb AND KDGTYP = :type AND KDGALX = :alx",
                parameters.ToArray());
            if (totalRegeneration)
            {
                DbBase.Settings.ExecuteNonQuery(
                    CommandType.Text,
                    "DELETE FROM KPGARAP WHERE KDFIPB = :ipb AND KDFTYP = :type AND KDFALX = :alx",
                    parameters.ToArray());
            }
            DbBase.Settings.ExecuteNonQuery(
                CommandType.Text,
                "DELETE FROM KPOPTD WHERE KDCIPB = :ipb AND KDCTYP = :type AND KDCALX = :alx",
                parameters.ToArray());
        }

        private static void PopulateCanevas(DtoCommon canevas, string user, bool totalRegeneration, IList<GaranLightDto> currentGaranties)
        {
            EacParameter[] paramListFormuleByCanvas = new EacParameter[3];
            paramListFormuleByCanvas[0] = new EacParameter("strReturnCol", DbType.AnsiStringFixedLength);
            paramListFormuleByCanvas[0].Value = canevas.StrReturnCol.PadLeft(9, ' ');
            paramListFormuleByCanvas[1] = new EacParameter("int32ReturnCol", DbType.Int32);
            paramListFormuleByCanvas[1].Value = canevas.Int32ReturnCol;
            paramListFormuleByCanvas[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramListFormuleByCanvas[2].Value = canevas.Type;


            var sqlListFormuleByCanvas = @"SELECT KDBFOR CODEID1, KDBOPT CODEID2, KDAKAIID ID 
										   FROM KPOPT  
												INNER JOIN KPFOR ON KDAIPB = KDBIPB AND KDAALX = KDBALX AND KDAFOR = KDBFOR 
										   WHERE KDBIPB = :strReturnCol AND KDBALX= :int32ReturnCol AND KDBTYP = :type ";

            var LstFormuleByCanvas = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlListFormuleByCanvas, paramListFormuleByCanvas);
            long codecible = 0;
            var voletsBlocs = new List<GaranModelDto>();
            var garanties = new List<GarantiesDto>();

            foreach (var Formulebycanevas in LstFormuleByCanvas)
            {
                if (Formulebycanevas.Id != codecible)
                {
                    voletsBlocs = LoadVoletBlocParameters(canevas.StrReturnCol, canevas.Int32ReturnCol, canevas.Type, Convert.ToInt32(Formulebycanevas.Id), canevas.Branche);
                    garanties = LoadParameters(voletsBlocs.Select(i => i.LibModele.Trim()).Distinct().ToList());
                    codecible = Formulebycanevas.Id;
                }

                int guidV = 0;
                int guidB = 0;
                long idGar = 0;
                KpgaranDto garantieNiv = null;
                KpgartarDto garantieTarDto = null;

                foreach (var item in voletsBlocs)
                {
                    #region optd
                    var kdbid = FormuleRepository.GetLienKpOpt(canevas.StrReturnCol, canevas.Int32ReturnCol.ToString(), canevas.Type, Formulebycanevas.CodeId1.ToString(), Formulebycanevas.CodeId2.ToString());
                    bool voletupdate = false;
                    if (guidV != item.Guidv && (item.CaracVolet == "O" || item.CaracVolet == "P"))
                    {
                        voletupdate = true;
                    }

                    guidV = FormuleRepository.PopulateKpoptd(canevas.StrReturnCol, canevas.Int32ReturnCol, canevas.Type, new KpoptdDto
                    {
                        KDCIPB = canevas.StrReturnCol,
                        KDCALX = canevas.Int32ReturnCol,
                        KDCTYP = canevas.Type,
                        KDCFOR = Convert.ToInt32(Formulebycanevas.CodeId1),
                        KDCOPT = Convert.ToInt32(Formulebycanevas.CodeId2),
                        KDCFLAG = 1,
                        KDCKAEID = 0,
                        KDCKAKID = item.GuidVolet,
                        KDCKAQID = 0,
                        KDCKARID = item.GuidM,
                        KDCKDBID = kdbid,
                        KDCMAJD = (long)AlbConvert.ConvertDateToInt(DateTime.Now),
                        KDCMAJU = user,
                        KDCMODELE = item.LibModele,
                        KDCTENG = "V",
                        KDCCRD = (long)AlbConvert.ConvertDateToInt(DateTime.Now),
                        KDCCRU = user,
                        KDCORDRE = item.VoletOrdre
                    }, voletupdate);

                    bool blocUpdate = false;
                    if (guidB != item.Guidb && voletupdate && (item.CaracBloc == "O" || item.CaracBloc == "P"))
                    {
                        blocUpdate = true;
                    }

                    guidB = FormuleRepository.PopulateKpoptd(canevas.StrReturnCol, canevas.Int32ReturnCol, canevas.Type, new KpoptdDto
                    {
                        KDCIPB = canevas.StrReturnCol,
                        KDCALX = canevas.Int32ReturnCol,
                        KDCTYP = canevas.Type,
                        KDCFOR = Convert.ToInt32(Formulebycanevas.CodeId1),
                        KDCOPT = Convert.ToInt32(Formulebycanevas.CodeId2),
                        KDCFLAG = 1,
                        KDCKAEID = item.GuidBloc,
                        KDCKAKID = item.GuidVolet,
                        KDCKAQID = item.Guidb,
                        KDCKARID = item.GuidM,
                        KDCKDBID = kdbid,
                        KDCMAJD = (long)AlbConvert.ConvertDateToInt(DateTime.Now),
                        KDCMAJU = user,
                        KDCMODELE = item.LibModele,
                        KDCTENG = "B",
                        KDCCRD = (long)AlbConvert.ConvertDateToInt(DateTime.Now),
                        KDCCRU = user,
                        KDCORDRE = item.BlocOrdre
                    }, blocUpdate);

                    #endregion

                    #region garanties
                    foreach (var gar in garanties) //TODO PARSE GARS BY LEVEL
                    {
                        if (gar.C2MGANIV == item.LibModele && blocUpdate && (gar.C2CARNIV == "P" || gar.C2CARNIV == "O" || (gar.C2CARNIV == "F" && gar.C2NATNIV == "E")))
                        {
                            #region kpgaran

                            garantieNiv = new KpgaranDto
                            {
                                KDETYP = canevas.Type,
                                KDEIPB = canevas.StrReturnCol,
                                KDEALX = Convert.ToInt32(canevas.Int32ReturnCol),
                                KDEFOR = Convert.ToInt32(Formulebycanevas.CodeId1),
                                KDEOPT = Convert.ToInt32(Formulebycanevas.CodeId2),
                                KDEKDCID = guidB,
                                KDEGARAN = gar.GAGARNIV,
                                KDESEQ = gar.C2SEQNIV,
                                KDENIVEAU = gar.C2NIVNIV,
                                KDESEM = gar.C2SEMNIV,
                                KDESE1 = gar.C2SE1NIV,
                                KDETRI = gar.C2TRINIV,
                                KDENUMPRES = 1,
                                KDEAJOUT = "N",
                                KDECAR = gar.C2CARNIV,
                                KDENAT = gar.C2NATNIV,
                                KDEGAN = gar.C2NATNIV,
                                KDEKDFID = 0,
                                KDEDEFG = gar.GADFGNIV,
                                KDEKDHID = 0,
                                KDEDUREE = 0,
                                KDEDURUNI = "",
                                KDEPRP = "A",
                                KDETYPEMI = "P",
                                KDEALIREF = gar.C2MRFNIV,
                                KDECATNAT = gar.C2CNANIV,
                                KDEINA = gar.C2INANIV,
                                KDETAXCOD = gar.C2TAXNIV,
                                KDETAXREP = 0,
                                KDECRAVN = 0,
                                KDECRU = user,
                                KDECRD = (long)AlbConvert.ConvertDateToInt(DateTime.Now),
                                KDEMAJAVN = 0,
                                KDEASVALO = gar.C4VALNIV,
                                KDEASVALA = gar.C4VALNIV,
                                KDEASVALW = 0,
                                KDEASUNIT = gar.C4UNTNIV,
                                KDEASBASE = gar.C4BASNIV,
                                KDEASMOD = gar.C4MAJNIV,
                                KDEASOBLI = gar.C4OBLNIV,
                                KDEINVSP = gar.GAINVNIV,
                                KDEINVEN = 0,
                                KDETCD = gar.C2TCDNIV,
                                KDEMODI = "N",
                                KDEPIND = gar.C2INANIV,
                                KDEPCATN = gar.C2CNANIV,
                                KDEPREF = gar.C2MRFNIV,
                                KDEPPRP = "A",
                                KDEPEMI = "P",
                                KDEPTAXC = gar.C2TAXNIV,
                                KDEPNTM = gar.C2NTMNIV,
                                KDEALA = gar.C4ALANIV,
                                KDEPALA = gar.C4ALANIV,
                                KDEALO = string.Empty
                            };

                            #endregion

                            #region kpgartar

                            garantieTarDto = new KpgartarDto
                            {
                                KDGIPB = canevas.StrReturnCol,
                                KDGALX = Convert.ToInt32(canevas.Int32ReturnCol),
                                KDGFOR = Convert.ToInt32(Formulebycanevas.CodeId1),
                                KDGOPT = Convert.ToInt32(Formulebycanevas.CodeId2),
                                KDGCHT = 0,
                                KDGCMC = 0,
                                KDGCTT = 0,
                                KDGTFF = 0,
                                KDGTMC = 0,
                                KDGTYP = canevas.Type,
                                KDGFMABASE = gar.FRHBASMAXNIV,
                                KDGFMAUNIT = gar.FRHBASMAXNIV,
                                KDGFMAVALA = gar.FRHVALMAXNIV,
                                KDGFMAVALO = gar.FRHVALMAXNIV,
                                KDGFMAVALW = 0,
                                KDGFMIBASE = gar.FRHBASMINNIV,
                                KDGFMIUNIT = gar.FRHUNTMINNIV,
                                KDGFMIVALA = gar.FRHVALMINNIV,
                                KDGFMIVALO = gar.FRHVALMINNIV,
                                KDGFMIVALW = 0,
                                KDGFRHBASE = gar.FRHBASNIV,
                                KDGFRHMOD = gar.FRHMODNIV,
                                KDGFRHOBL = gar.FRHOBLNIV,
                                KDGFRHUNIT = gar.FRHUNTNIV,
                                KDGFRHVALA = gar.FRHVALNIV,
                                KDGFRHVALO = gar.FRHVALNIV,
                                KDGFRHVALW = 0,
                                KDGGARAN = gar.C2GARNIV,
                                KDGKDEID = idGar,
                                KDGLCIBASE = gar.LCIBASNIV,
                                KDGLCIMOD = gar.LCIMODNIV,
                                KDGLCIOBL = gar.LCIOBLNIV,
                                KDGLCIUNIT = gar.LCIUNTNIV,
                                KDGLCIVALA = gar.LCIVALNIV,
                                KDGLCIVALO = gar.LCIVALNIV,
                                KDGLCIVALW = 0,
                                KDGMNTBASE = 0,
                                KDGNUMTAR = 1,
                                KDGPRIBASE = gar.PRIBASNIV,
                                KDGPRIMOD = gar.PRIMODNIV,
                                KDGPRIMPRO = 0,
                                KDGPRIOBL = gar.PRIOBLNIV,
                                KDGPRIUNIT = gar.PRIUNTNIV,
                                KDGPRIVALA = gar.PRIVALNIV,
                                KDGPRIVALO = gar.PRIVALNIV,
                                KDGPRIVALW = 0
                            };

                            #endregion

                            #region populate

                            idGar = FormuleRepository.PopulateKpgaran(canevas.StrReturnCol, Convert.ToInt32(canevas.Int32ReturnCol), canevas.Type, garantieNiv);
                            FormuleRepository.PopulateKpgartar(canevas.StrReturnCol, canevas.Int32ReturnCol, canevas.Type, idGar, garantieTarDto);

                            #endregion

                            #region Update KPGARAP if partial regeneration

                            if (!totalRegeneration)
                            {
                                var updatedGarantie = FormuleRepository.UpdateKPGARAP(idGar, garantieNiv, currentGaranties);
                                if (updatedGarantie != null)
                                {
                                    currentGaranties.Remove(updatedGarantie);
                                }
                            }

                            #endregion
                        }
                    }
                    #endregion
                }

                //Nettoyer les portées si la garantie a disparue
                if (!totalRegeneration && currentGaranties.Count > 0)
                {
                    foreach (var oldGarantie in currentGaranties)
                    {
                        var param = new EacParameter[1];
                        param[0] = new EacParameter("Id", DbType.Int64)
                        {
                            Value = oldGarantie.Id
                        };

                        var sql = @"DELETE FROM KPGARAP WHERE KDFKDEID = :Id";
                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                    }
                }

                PopulateKpexp(canevas.StrReturnCol, canevas.Int32ReturnCol.ToString(), canevas.Type, Formulebycanevas.CodeId1.ToString(), Formulebycanevas.CodeId2.ToString());
            }
        }

        public static void RegenerateCanevas(string user, bool totalRegeneration, string canevas = "CV")
        {
            //GET CANEVAS
            //canevas = "CVECM"; //TEST POUR UN SEUL CANEVAS
            if (canevas.IsEmptyOrNull())
            {
                canevas = "CV";
            }
            var recupCanevas = $@"SELECT PBIPB STRRETURNCOL, PBALX INT32RETURNCOL, PBTYP TYPE, PBBRA BRANCHE ,KAIID ID
												FROM YPOBASE 
												LEFT JOIN KPENT ON PBIPB=KAAIPB AND PBALX=KAAALX
												LEFT JOIN KCIBLE ON KAACIBLE = KAHCIBLE
												LEFT JOIN KCIBLEF ON KAHCIBLE = KAICIBLE
												WHERE UPPER(PBIPB) LIKE '%{canevas}%'"; //TODO SUR UN SEUL CANEVAS POUR TEST --- LIKE 'CV%' --- CVAUD002 - CVIMAG002
            List<DtoCommon> listCanevas = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, recupCanevas);


            foreach (var item in listCanevas)
            {
                //TODO UPDATE DATE SAISIE YPOBASE
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                var day = DateTime.Now.Day;
                var hour = DateTime.Now.Hour * 100 + DateTime.Now.Minute;

                EacParameter[] param = new EacParameter[7];
                param[0] = new EacParameter("year", DbType.Int32);
                param[0].Value = year;
                param[1] = new EacParameter("month", DbType.Int32);
                param[1].Value = month;
                param[2] = new EacParameter("day", DbType.Int32);
                param[2].Value = day;
                param[3] = new EacParameter("hour", DbType.Int32);
                param[3].Value = hour;
                param[4] = new EacParameter("strReturnCol", DbType.AnsiStringFixedLength);
                param[4].Value = item.StrReturnCol.PadLeft(9, ' ');
                param[5] = new EacParameter("int32ReturnCol", DbType.Int32);
                param[5].Value = item.Int32ReturnCol;
                param[6] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[6].Value = item.Type;

                string sql = @"UPDATE YPOBASE
								SET PBSAA = :year, PBSAM = :month, PBSAJ = :day, PBSAH = :hour
								WHERE PBIPB = :strReturnCol AND PBALX = :int32ReturnCol AND PBTYP = :type";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                EacParameter[] paramDelete = new EacParameter[7];
                paramDelete[0] = new EacParameter("strReturnCol", DbType.AnsiStringFixedLength);
                paramDelete[0].Value = item.StrReturnCol.PadLeft(9, ' ');
                //DELETE CLAUSES LIBRES
                sql = @"DELETE FROM KPCLAUSE WHERE KCAXTL = 'O' AND KCAIPB = :strReturnCol";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramDelete);

                var currentGaranties = GetGaranList(item);

                //DELETE INFOS CANEVAS
                CleanInfosCanevas(item, totalRegeneration);

                //INSERT INFOS CANEVAS
                PopulateCanevas(item, user, totalRegeneration, currentGaranties);
            }
        }

        private static IList<GaranLightDto> GetGaranList(DtoCommon item)
        {
            var parameters = new List<EacParameter> {
                new EacParameter("ipb", DbType.AnsiStringFixedLength) { Value = item.StrReturnCol.PadLeft(9, ' ') },
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value = item.Type },
                new EacParameter("alx", DbType.Int32) { Value = item.Int32ReturnCol }
                 };

            var garanList = DbBase.Settings.ExecuteList<GaranLightDto>(CommandType.Text,
                "SELECT KDEID, KDETYP, KDEIPB, KDEALX, KDEFOR, KDEOPT, KDESEQ, KDEALA FROM KPGARAN WHERE KDEIPB = :ipb AND KDETYP = :type AND KDEALX = :alx",
                parameters.ToArray()).ToList();

            return garanList;
        }

        public static void CopyCanevas(string source, string cible, string user)
        {
            // Récupération de la source et de la cible
            string scriptName = ConfigurationManager.AppSettings["PowerShellPgm"];
            string sqlFolderName = ConfigurationManager.AppSettings["SQLFolder"];
            string sqlFolder = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), sqlFolderName);
            ;
            string scriptPath = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), scriptName);
            string connectionString = ConfigurationManager.ConnectionStrings["PowershellScriptConnectionString"].ConnectionString;

            if(cible=="FORM" || cible == "PROD")
            {
                source = "PARAME";
                connectionString = connectionString.Replace("10.1.2.36", "10.1.2.2");
            }

            string arg = " -$SqlFolderPath '" + sqlFolder + "' -$Source '" + source + "' -$Cible '" + cible + "' -$ConnectionString '" + connectionString + "'";

            using (PowerShell shell = PowerShell.Create())
            {
                using (Runspace runspace = RunspaceFactory.CreateRunspace())
                {
                    runspace.Open();
                    //RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
                    //scriptInvoker.Invoke("Set-ExecutionPolicy Unrestricted");
                    shell.Runspace = runspace;
                    //powershell.Commands.AddScript("Add-PsSnapin Microsoft.SharePoint.PowerShell");
                    System.IO.StreamReader sr = new System.IO.StreamReader(scriptPath);
                    shell.AddScript(sr.ReadToEnd());
                    //shell.AddArgument("-ExecutionPolicy RemoteSigned ");
                    shell.AddParameter("SqlFolderPath", sqlFolder);
                    shell.AddParameter("Source", source);
                    shell.AddParameter("Cible", cible);
                    shell.AddParameter("ConnectionString", connectionString);

                    shell.AddCommand("Out-String");
                    var results = shell.Invoke();
                    if (shell.Streams.Error.Count > 0)
                    {
                        throw new AggregateException(shell.Streams.Error.Select(x => x.Exception).ToArray());
                    }
                }
            }

            var param = new[] {
                new EacParameter("P_DATESYSTEME", AlbConvert.ConvertDateToInt(DateTime.Now).ToString()),
                new EacParameter("P_USER", user)
            };

            string sql = string.Format("SP_COPYCANEVAS", source, cible);
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, sql, param);

            // Copie physique des fichiers 
            // Récupérer les canevas de la table KCANEV cible 
            var recupCanevas = @"SELECT KGOCNVA CODE, KGOTYP TYPE , 0 STRRETURNCOL FROM KCANEV";
            List<DtoCommon> listCanevas = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, recupCanevas);
            foreach (DtoCommon canevas in listCanevas)
            {
                if (canevas != null)
                {
                    CopieDocRepository.CopierDocuments(canevas.Code, canevas.StrReturnCol, canevas.Type, "0");
                }
            }


        }


        /// <summary>
        /// Récupère la formule de garantie
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <param name="codeCible"></param>
        /// <param name="codeAlpha"></param>
        /// <param name="branche"></param>
        /// <param name="libFormule"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static FormGarDto FormulesGarantiesGet(string codeOffre, int version, string type, string codeAvn, int codeFormule, int codeOption, int formGen, int codeCible,
                string codeAlpha, string branche, string libFormule, string user, int appliqueA, bool isReadOnly, ModeConsultation modeNavig)
        {
            var date = AlbConvert.ConvertDateToInt(DateTime.Now);
            bool isModeAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0";

            #region Récupération des données
            EacParameter[] param;
            List<FormuleGarantiePlatDto> result;

            if (isReadOnly && ModeConsultation.Standard == modeNavig)
            {
                param = new EacParameter[7];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = version;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = type;
                param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
                param[3].Value = codeFormule;
                param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
                param[4].Value = codeOption;
                param[5] = new EacParameter("P_CODECIBLE", DbType.Int32);
                param[5].Value = codeCible;
                param[6] = new EacParameter("P_DATENOW", DbType.Int32);
                param[6].Value = date;
                result = DbBase.Settings.ExecuteList<FormuleGarantiePlatDto>(CommandType.StoredProcedure, isModeAvt ? "SP_FORMGARREADONLYAVT" : "SP_FORMGARREADONLY", param);
            }
            else if (ModeConsultation.Standard == modeNavig)
            {
                var voletsBlocs = LoadVoletBlocParameters(codeOffre, version, type, codeCible, branche);

                var garanties = LoadParameters(voletsBlocs.Select(i => i.LibModele.Trim()).Distinct().ToList());

                param = new EacParameter[5];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[1].Value = type;
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = version;
                param[3] = new EacParameter("codeFormule", DbType.Int32);
                param[3].Value = codeFormule;
                param[4] = new EacParameter("codeOption", DbType.Int32);
                param[4].Value = codeOption;

                var sql = @"SELECT KDCTENG STRRETURNCOL,KDCFLAG CODEID3, KDCKAKID INT64RETURNCOL, KDCKAEID DATEDEBRETURNCOL FROM KPOPTD
										WHERE KDCIPB = :codeOffre and KDCTYP = :type AND KDCALX = :version AND KDCFOR = :codeFormule AND KDCOPT = :codeOption 
										AND (KDCTENG = 'V' OR  KDCTENG = 'B')";

                List<DtoCommon> checkedBlocsVolets = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

                param = new EacParameter[5];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[1].Value = type;
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = version;
                param[3] = new EacParameter("codeFormule", DbType.Int32);
                param[3].Value = codeFormule;
                param[4] = new EacParameter("codeOption", DbType.Int32);
                param[4].Value = codeOption;

                sql = @"SELECT KDEGAN STRRETURNCOL, KDEGARAN STRRETURNCOL2, KDESEQ INT64RETURNCOL, KDESEM DATEDEBRETURNCOL ,KDEMODI ETAT, KDECRAVN INT32RETURNCOL, KDEMAJAVN INT32RETURNCOL2,KDEINVEN ID, 
											IFNULL(KDEDATDEB,0) DATEDEBEFFRETURNCOL, CAST(LEFT(KDEHEUDEB,4) AS INTEGER) CODEID1, KDEDATFIN DATEFINEFFRETURNCOL, CAST(LEFT(KDEHEUFIN,4) AS INTEGER) CODEID2, KDEDUREE GARID, KDEDURUNI CODE, KDEALA SITUATION
										FROM KPGARAN WHERE KDEIPB = :codeOffre and KDETYP = :type and KDEALX = :version and KDEFOR = :codeFormule and KDEOPT = :codeOption";

                List<DtoCommon> checkedGaranties = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

                sql = @"SELECT GARAP.KDFGAN STRRETURNCOL , GARAN.KDESEQ INT64RETURNCOL FROM KPGARAP GARAP INNER JOIN  KPGARAN GARAN 
									ON GARAP.KDFKDEID = GARAN.KDEID WHERE GARAP.KDFIPB = :codeOffre AND GARAP.KDFTYP = :type AND GARAP.KDFALX = :version
									AND GARAP.KDFFOR = :codeFormule AND GARAP.KDFOPT = :codeOption";

                List<DtoCommon> portees = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

                result = MapFormuleGarantie(codeAvn, codeOption, appliqueA, voletsBlocs, garanties, checkedBlocsVolets, checkedGaranties, portees);
                result = result.OrderBy(v => v.VoletOrdre).ThenBy(b => b.BlocOrdre).ThenBy(n1 => n1.TriNiv1).ThenBy(n2 => n2.TriNiv2).ThenBy(n3 => n3.TriNiv3).ThenBy(n4 => n4.TriNiv4).ToList();
                //copie informations garanties dans W
                CopyInfoWorkGarantie(codeOffre, version, type, codeFormule, codeOption);
            }
            else
            {
                param = new EacParameter[8];
                param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("P_VERSION", DbType.Int32);
                param[1].Value = version;
                param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                param[2].Value = type;
                param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
                param[3].Value = codeFormule;
                param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
                param[4].Value = codeOption;
                param[5] = new EacParameter("P_CODECIBLE", DbType.Int32);
                param[5].Value = codeCible;
                param[6] = new EacParameter("P_DATENOW", DbType.Int32);
                param[6].Value = date;
                param[7] = new EacParameter("P_AVENANT", DbType.Int32);
                param[7].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;

                result = DbBase.Settings.ExecuteList<FormuleGarantiePlatDto>(CommandType.StoredProcedure, isModeAvt ? "SP_FORMGARHISTOAVT" : "SP_FORMGARHISTO", param);
            }

            #endregion

            Trace.WriteLine("Chargement Formule de Garantie");

            List<FormuleGarantiePlatDto> resultVBHisto = null;
            if (!isReadOnly && !string.IsNullOrEmpty(codeAvn) && codeAvn != "0")
            {
                resultVBHisto = GetListVBHisto(codeOffre, version, type, codeAvn, codeFormule, codeOption, date, codeCible);
            }

            FormGarDto lstFormule = LoadFormulePlat(result, resultVBHisto, isReadOnly, codeOffre, version.ToString(), type, codeAvn, codeFormule.ToString(), codeOption.ToString());

            var formGar = new FormGarDto
            {
                FormuleGarantie = lstFormule.FormuleGarantie,
                FormuleGarantieSave = lstFormule.FormuleGarantieSave,
                FormuleGarantieHisto = (resultVBHisto != null) ? LoadFormulePlat(resultVBHisto, null, isReadOnly, codeOffre, version.ToString(), type, codeAvn, codeFormule.ToString(), codeOption.ToString()).FormuleGarantie : null
            };
            return formGar;
        }

        public static List<GaranModelDto> LoadVoletBlocParameters(string codeOffre, int version, string type, int codeCible, string branche)
        {
            var date = AlbConvert.ConvertDateToInt(DateTime.Now);

            var param = new EacParameter[5];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODECIBLE", DbType.AnsiStringFixedLength);
            param[3].Value = codeCible;
            param[4] = new EacParameter("P_DATE", DbType.AnsiStringFixedLength);
            param[4].Value = date;

            var voletsBlocs = DbBase.Settings.ExecuteList<GaranModelDto>(CommandType.StoredProcedure, "SP_LOADVOLETSBLOCS", param);
            return voletsBlocs;
        }


        public static List<GarantiesDto> LoadParameters(List<string> listModels)
        {

            var modeles = string.Join(",", listModels.Select(i => string.Format("'{0}'", i)));


            var sql = string.Format(@"WITH Garanties(C2SEQNIV, C2MGANIV, C2OBENIV, C2NIVNIV, C2GARNIV, C2ORDNIV, C2LIBNIV, 
				C2SEMNIV, C2CARNIV, C2NATNIV, C2INANIV, C2CNANIV, C2TAXNIV, C2ALTNIV, 
				C2TRINIV, C2SE1NIV, C2SCRNIV, C2PRPNIV, C2TCDNIV, C2MRFNIV, C2NTMNIV,
				C2MASNIV, GAGARNIV, GADESNIV, GADEANIV, GATAXNIV, GACNXNIV, GACOMNIV,
				GACARNIV, GADFGNIV, GAIFCNIV, GAFAMNIV, GARGENIV, GATRGNIV, LIBGATRGNIV, GAINVNIV, 
				GATYINIV, GARUTNIV, C4SEQNIV, C4TYPNIV, C4BASNIV, C4VALNIV, C4UNTNIV,
				C4MAJNIV, C4OBLNIV, C4ALANIV,
				TPCN1NIV, PRIMODNIV, PRIOBLNIV, PRIVALNIV, PRIUNTNIV, PRIBASNIV,
				LCIMODNIV, LCIOBLNIV, LCIVALNIV, LCIUNTNIV, LCIBASNIV,
				FRHMODNIV, FRHOBLNIV, FRHVALNIV, FRHUNTNIV, FRHBASNIV,
				FRHMODMINNIV, FRHOBLMINNIV, FRHVALMINNIV, FRHUNTMINNIV, FRHBASMINNIV, 
				FRHMODMAXNIV, FRHOBLMAXNIV, FRHVALMAXNIV, FRHUNTMAXNIV, FRHBASMAXNIV) AS
				
			(SELECT GAR1 . C2SEQ C2SEQNIV1 , GAR1 . C2MGA C2MGANIV1 , GAR1 . C2OBE C2OBENIV1 , GAR1 . C2NIV C2NIVNIV1 , GAR1 . C2GAR C2GARNIV1 , GAR1 . C2ORD C2ORDNIV1 , GAR1 . C2LIB C2LIBNIV1 , 
			GAR1 . C2SEM C2SEMNIV1 , GAR1 . C2CAR C2CARNIV1 , GAR1 . C2NAT C2NATNIV1 , GAR1 . C2INA C2INANIV1 , GAR1 . C2CNA C2CNANIV1 , GAR1 . C2TAX C2TAXNIV1 , GAR1 . C2ALT C2ALTNIV1 , 
			GAR1 . C2TRI C2TRINIV1 , GAR1 . C2SE1 C2SE1NIV1 , GAR1 . C2SCR C2SCRNIV1 , GAR1 . C2PRP C2PRPNIV1 , GAR1 . C2TCD C2TCDNIV1 , GAR1 . C2MRF C2MRFNIV1 , GAR1 . C2NTM C2NTMNIV1 , 
			GAR1 . C2MAS C2MASNIV1 , GARAN1 . GAGAR GAGARNIV1 , GARAN1 . GADES GADESNIV1 , GARAN1 . GADEA GADEANIV1 , GARAN1 . GATAX GATAXNIV1 , GARAN1 . GACNX GACNXNIV1 , GARAN1 . GACOM GACOMNIV1 , 
			GARAN1 . GACAR GACARNIV1 , GARAN1 . GADFG GADFGNIV1 , GARAN1 . GAIFC GAIFCNIV1 , GARAN1 . GAFAM GAFAMNIV1 , GARAN1 . GARGE GARGENIV1 , GARAN1 . GATRG GATRGNIV1 , PAR2.TPLIB LIBGATRGNIV, GARAN1 . GAINV GAINVNIV1 ,
			GARAN1 . GATYI GATYINIV1 , GARAN1 . GARUT GARUTNIV1 , GAL10 . C4SEQ C4SEQNIV1 , GAL10 . C4TYP C4TYPNIV1 , GAL10 . C4BAS C4BASNIV1 , GAL10 . C4VAL C4VALNIV1 , GAL10 . C4UNT C4UNTNIV1 , 
			GAL10 . C4MAJ C4MAJNIV1 , GAL10 . C4OBL C4OBLNIV1 , GAL10 . C4ALA C4ALANIV1 , 
					PAR1 . TPCN1 TPCN1NIV1 , GAL11 . C4MAJ PRIMODNIV1 , GAL11 . C4OBL PRIOBLNIV1 , GAL11 . C4VAL PRIVALNIV1 , GAL11 . C4UNT PRIUNTNIV1 , GAL11 . C4BAS PRIBASNIV1 , 
					GAL12 . C4MAJ LCIMODNIV1 , GAL12 . C4OBL LCIOBLNIV1 , GAL12 . C4VAL LCIVALNIV1 , GAL12 . C4UNT LCIUNTNIV1 , GAL12 . C4BAS LCIBASNIV1 , 
					GAL13 . C4MAJ FRHMODNIV1 , GAL13 . C4OBL FRHOBLNIV1 , GAL13 . C4VAL FRHVALNIV1 , GAL13 . C4UNT FRHUNTNIV1 , GAL13 . C4BAS FRHBASNIV1 , 
					GAL14 . C4MAJ FRHMODMINNIV1 , GAL14 . C4OBL FRHOBLMINNIV1 , GAL14 . C4VAL FRHVALMINNIV1 , GAL14 . C4UNT FRHUNTMINNIV1 , GAL14 . C4BAS FRHBASMINNIV1 , 
					GAL15 . C4MAJ FRHMODMAXNIV1 , GAL15 . C4OBL FRHOBLMAXNIV1 , GAL15 . C4VAL FRHVALMAXNIV1 , GAL15 . C4UNT FRHUNTMAXNIV1 , GAL15 . C4BAS FRHBASMAXNIV1
			FROM YPLTGAR GAR1
				LEFT JOIN KGARAN GARAN1 ON GAR1 . C2GAR = GARAN1 . GAGAR 
				LEFT JOIN YPLTGAL GAL10 ON GAR1 . C2SEQ = GAL10 . C4SEQ AND GAL10 . C4TYP = 0 
				LEFT JOIN YPLTGAL GAL11 ON GAR1 . C2SEQ = GAL11 . C4SEQ AND GAL11 . C4TYP = 1 
				LEFT JOIN YPLTGAL GAL12 ON GAR1 . C2SEQ = GAL12 . C4SEQ AND GAL12 . C4TYP = 2 
				LEFT JOIN YPLTGAL GAL13 ON GAR1 . C2SEQ = GAL13 . C4SEQ AND GAL13 . C4TYP = 3 
				LEFT JOIN YPLTGAL GAL14 ON GAR1 . C2SEQ = GAL14 . C4SEQ AND GAL14 . C4TYP = 4 
				LEFT JOIN YPLTGAL GAL15 ON GAR1 . C2SEQ = GAL15 . C4SEQ AND GAL15 . C4TYP = 5 
				LEFT JOIN  YYYYPAR PAR1 ON PAR1 . TCON = 'PRODU' AND PAR1 . TFAM = 'GADFG' AND PAR1 . TCOD = GARAN1 . GADFG
				LEFT JOIN  YYYYPAR PAR2 ON PAR2 . TCON = 'PRODU' AND PAR2 . TFAM = 'GATRG' AND PAR2 . TCOD = GARAN1 . GATRG
				WHERE  GAR1 . C2MGA IN ({0})  AND GAR1 . C2CAR IN ( 'A' , 'F' , 'B' , 'O' , 'P' , 'S' ) AND GAR1 . C2NIV = 1 AND GAR1 . C2SEM = 0
	
				UNION ALL
	
				SELECT GAR1 . C2SEQ C2SEQNIV1 , GAR1 . C2MGA C2MGANIV1 , GAR1 . C2OBE C2OBENIV1 , GAR1 . C2NIV C2NIVNIV1 , GAR1 . C2GAR C2GARNIV1 , GAR1 . C2ORD C2ORDNIV1 , GAR1 . C2LIB C2LIBNIV1 , 
						GAR1 . C2SEM C2SEMNIV1 , GAR1 . C2CAR C2CARNIV1 , GAR1 . C2NAT C2NATNIV1 , GAR1 . C2INA C2INANIV1 , GAR1 . C2CNA C2CNANIV1 , GAR1 . C2TAX C2TAXNIV1 , GAR1 . C2ALT C2ALTNIV1 , 
						GAR1 . C2TRI C2TRINIV1 , GAR1 . C2SE1 C2SE1NIV1 , GAR1 . C2SCR C2SCRNIV1 , GAR1 . C2PRP C2PRPNIV1 , GAR1 . C2TCD C2TCDNIV1 , GAR1 . C2MRF C2MRFNIV1 , GAR1 . C2NTM C2NTMNIV1 , 
						GAR1 . C2MAS C2MASNIV1 , GARAN1 . GAGAR GAGARNIV1 , GARAN1 . GADES GADESNIV1 , GARAN1 . GADEA GADEANIV1 , GARAN1 . GATAX GATAXNIV1 , GARAN1 . GACNX GACNXNIV1 , GARAN1 . GACOM GACOMNIV1 , 
						GARAN1 . GACAR GACARNIV1 , GARAN1 . GADFG GADFGNIV1 , GARAN1 . GAIFC GAIFCNIV1 , GARAN1 . GAFAM GAFAMNIV1 , GARAN1 . GARGE GARGENIV1 , GARAN1 . GATRG GATRGNIV1 , PAR2.TPLIB LIBGATRGNIV, GARAN1 . GAINV GAINVNIV1 ,
						GARAN1 . GATYI GATYINIV1 , GARAN1 . GARUT GARUTNIV1 , GAL10 . C4SEQ C4SEQNIV1 , GAL10 . C4TYP C4TYPNIV1 , GAL10 . C4BAS C4BASNIV1 , GAL10 . C4VAL C4VALNIV1 , GAL10 . C4UNT C4UNTNIV1 , 
						GAL10 . C4MAJ C4MAJNIV1 , GAL10 . C4OBL C4OBLNIV1 , GAL10 . C4ALA C4ALANIV1 , 
								PAR1 . TPCN1 TPCN1NIV1 , GAL11 . C4MAJ PRIMODNIV1 , GAL11 . C4OBL PRIOBLNIV1 , GAL11 . C4VAL PRIVALNIV1 , GAL11 . C4UNT PRIUNTNIV1 , GAL11 . C4BAS PRIBASNIV1 , 
					GAL12 . C4MAJ LCIMODNIV1 , GAL12 . C4OBL LCIOBLNIV1 , GAL12 . C4VAL LCIVALNIV1 , GAL12 . C4UNT LCIUNTNIV1 , GAL12 . C4BAS LCIBASNIV1 , 
					GAL13 . C4MAJ FRHMODNIV1 , GAL13 . C4OBL FRHOBLNIV1 , GAL13 . C4VAL FRHVALNIV1 , GAL13 . C4UNT FRHUNTNIV1 , GAL13 . C4BAS FRHBASNIV1 , 
					GAL14 . C4MAJ FRHMODMINNIV1 , GAL14 . C4OBL FRHOBLMINNIV1 , GAL14 . C4VAL FRHVALMINNIV1 , GAL14 . C4UNT FRHUNTMINNIV1 , GAL14 . C4BAS FRHBASMINNIV1 , 
					GAL15 . C4MAJ FRHMODMAXNIV1 , GAL15 . C4OBL FRHOBLMAXNIV1 , GAL15 . C4VAL FRHVALMAXNIV1 , GAL15 . C4UNT FRHUNTMAXNIV1 , GAL15 . C4BAS FRHBASMAXNIV1
				FROM YPLTGAR GAR1
						LEFT JOIN KGARAN GARAN1 ON GAR1 . C2GAR = GARAN1 . GAGAR 
						LEFT JOIN YPLTGAL GAL10 ON GAR1 . C2SEQ = GAL10 . C4SEQ AND GAL10 . C4TYP = 0 
						LEFT JOIN YPLTGAL GAL11 ON GAR1 . C2SEQ = GAL11 . C4SEQ AND GAL11 . C4TYP = 1 
						LEFT JOIN YPLTGAL GAL12 ON GAR1 . C2SEQ = GAL12 . C4SEQ AND GAL12 . C4TYP = 2 
						LEFT JOIN YPLTGAL GAL13 ON GAR1 . C2SEQ = GAL13 . C4SEQ AND GAL13 . C4TYP = 3 
						LEFT JOIN YPLTGAL GAL14 ON GAR1 . C2SEQ = GAL14 . C4SEQ AND GAL14 . C4TYP = 4 
						LEFT JOIN YPLTGAL GAL15 ON GAR1 . C2SEQ = GAL15 . C4SEQ AND GAL15 . C4TYP = 5 
						LEFT JOIN  YYYYPAR PAR1 ON PAR1 . TCON = 'PRODU' AND PAR1 . TFAM = 'GADFG' AND PAR1 . TCOD = GARAN1 . GADFG 
						LEFT JOIN  YYYYPAR PAR2 ON PAR2 . TCON = 'PRODU' AND PAR2 . TFAM = 'GATRG' AND PAR2 . TCOD = GARAN1 . GATRG 
						INNER JOIN Garanties ON Garanties . C2SEQNIV = GAR1 . C2SEM  
				 WHERE GAR1 . C2CAR IN ( 'A' , 'F' , 'B' , 'O' , 'P' , 'S' )
			)
			SELECT *
			FROM Garanties", modeles);

            var garanties = DbBase.Settings.ExecuteList<GarantiesDto>(CommandType.Text, sql);
            return garanties;
        }


        private static List<FormuleGarantiePlatDto> MapFormuleGarantie(string codeAvn, int codeOption, long appliqueA, List<GaranModelDto> voletsBlocs, List<GarantiesDto> garanties, List<DtoCommon> checkedBlocsVolets, List<DtoCommon> checkedGaranties, List<DtoCommon> portees)
        {
            var result = new List<FormuleGarantiePlatDto>();

            garanties.Where(i => i.C2NIVNIV == 1).ToList().ForEach(gar => FindGarRecursive(new List<GarantiesDto>() { gar }, codeAvn, codeOption, appliqueA, voletsBlocs, garanties, checkedBlocsVolets, checkedGaranties, portees, ref result));

            return result;
        }

        private static void FindGarRecursive(List<GarantiesDto> gars, string codeAvn, int codeOption, long appliqueA, List<GaranModelDto> voletsBlocs, List<GarantiesDto> garanties, List<DtoCommon> checkedBlocsVolets, List<DtoCommon> checkedGaranties, List<DtoCommon> portees, ref List<FormuleGarantiePlatDto> result)
        {
            //var lstNatParam = GetParamNatGar();
            if (gars == null || gars.Count == 0)
            {
                return;
            }

            foreach (var gar in gars)
            {
                FormuleGarantiePlatDto garantie = null;
                FormuleGarantiePlatDto parent = null;
                FormuleGarantiePlatDto item = null;
                //var infoNatPar = lstNatParam.Find(e => e.Caractere == gar.C2CARNIV && e.Nature == gar.C2NATNIV);

                switch (gar.C2NIVNIV)
                {
                    case 1:
                        garantie = result.FirstOrDefault(i => i.CodeNiv1 == gar.C2SEQNIV);
                        break;
                    case 2:
                        garantie = result.FirstOrDefault(i => i.CodeNiv2 == gar.C2SEQNIV);
                        parent = result.FirstOrDefault(i => i.CodeNiv1 == gar.C2SEMNIV);
                        break;
                    case 3:
                        garantie = result.FirstOrDefault(i => i.CodeNiv3 == gar.C2SEQNIV);
                        parent = result.FirstOrDefault(i => i.CodeNiv2 == gar.C2SEMNIV);
                        break;
                    case 4:
                        garantie = result.FirstOrDefault(i => i.CodeNiv4 == gar.C2SEQNIV);
                        parent = result.FirstOrDefault(i => i.CodeNiv3 == gar.C2SEMNIV);
                        break;
                }

                item = garantie == null && parent == null ? new FormuleGarantiePlatDto() : (FormuleGarantiePlatDto)parent.Clone();
                if (garantie == null)
                {
                    result.Add(item);
                }



                //new FormuleGarantiePlatDto();
                item.GuidOption = codeOption;
                item.AppliqueA = appliqueA;

                var bloc = voletsBlocs.FirstOrDefault(i => i.LibModele == gar.C2MGANIV);

                var isVoletChecked = checkedBlocsVolets.Any(i => i.StrReturnCol == "V" && i.Int64ReturnCol == bloc.GuidVolet && i.CodeId3 == 1);
                var isBlockChecked = checkedBlocsVolets.Any(i => i.StrReturnCol == "B" && i.DateDebReturnCol == bloc.GuidBloc && i.CodeId3 == 1);

                var res = checkedGaranties.FirstOrDefault(i => i.Int64ReturnCol == gar.C2SEQNIV);
                var range = portees.FirstOrDefault(i => i.Int64ReturnCol == gar.C2SEQNIV);

                var isGarantieChecked = res == null ? string.Empty : res.StrReturnCol;

                if (res == null && (gar.C2CARNIV == "P" || (gar.C2CARNIV == "F" && gar.C2NATNIV == "E")))
                {
                    isGarantieChecked = gar.C2NATNIV;
                }

                var flagModif = res == null ? string.Empty : res.Etat;

                var createavn = res == null ? !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0 : res.Int32ReturnCol;
                var majavn = res == null ? 0 : res.Int32ReturnCol2;

                //VOLET
                item.GuidV = bloc.Guidv;
                item.CodeVolet = bloc.CodeVolet;
                item.DescrVolet = bloc.DescrVolet;
                item.CaracVolet = bloc.CaracVolet;
                item.GuidVolet = bloc.GuidVolet;
                if (checkedBlocsVolets.Count == 0)
                {
                    item.isCheckedV = Convert.ToInt64(bloc.CaracVolet == "O" || bloc.CaracVolet == "P");
                }
                else
                {
                    item.isCheckedV = Convert.ToInt64(isVoletChecked || bloc.CaracVolet == "O");
                }

                item.VoletOrdre = (decimal)bloc.VoletOrdre;
                item.VoletCollapse = bloc.VoletCollapse;

                //BLOC
                item.GuidB = bloc.Guidb;
                item.CodeBloc = bloc.CodeBloc;
                item.DescrBloc = bloc.DescrBloc;
                item.CaracBloc = bloc.CaracBloc;
                item.GuidBloc = bloc.GuidBloc;
                if (checkedBlocsVolets.Count == 0)
                {
                    item.isCheckedB = Convert.ToInt64(bloc.CaracBloc == "O" || bloc.CaracBloc == "P");
                }
                else
                {
                    item.isCheckedB = Convert.ToInt64(isBlockChecked || bloc.CaracBloc == "O");
                }

                item.BlocOrdre = (decimal)bloc.BlocOrdre;

                //MODELE
                item.GuidM = bloc.GuidM;
                item.CodeModele = bloc.LibModele;

                switch (gar.C2NIVNIV)
                {
                    case 1:
                        //item.GuidNiv1 = gar.;
                        item.ActionNiv1 = range != null ? range.StrReturnCol ?? "" : "";
                        item.CodeNiv1 = gar.C2SEQNIV;
                        item.CodeGarNiv1 = gar.C2GARNIV;
                        item.DescrNiv1 = gar.GADESNIV;
                        item.CaracNiv1 = gar.C2CARNIV;
                        item.NatureNiv1 = gar.C2NATNIV;
                        //if (checkedGaranties.Count == 0 && (gar.C2CARNIV == "O" || gar.C2CARNIV == "P"))
                        //    item.NatureParamNiv1 = gar.C2NATNIV;
                        //else
                        item.NatureParamNiv1 = isGarantieChecked;
                        item.CodeParentNiv1 = gar.C2SEMNIV;
                        item.CodeNiv1Niv1 = gar.C2NIVNIV;
                        item.FlagModifNiv1 = flagModif;
                        item.ParamNatModNiv1 = gar.C2NTMNIV;
                        item.InvenPossible1 = gar.GAINVNIV;
                        item.CodeInven1 = 0;
                        item.TypeInven1 = gar.GATYINIV;
                        item.CreateAvt1 = createavn;
                        item.MajAvt1 = majavn;
                        item.DateDeb1 = string.Empty;
                        item.HeureDeb1 = 0;
                        item.DateFin1 = 0;
                        item.HeureFin1 = 0;
                        item.Duree1 = 0;
                        item.DureeUnite1 = string.Empty;
                        item.AlimNiv1 = gar.C4ALANIV;
                        item.TriNiv1 = gar.C2TRINIV;

                        if (res != null)
                        {
                            item.CodeInven1 = res.Id;
                            item.DateDeb1 = (res.DateDebEffReturnCol * 10000 + res.CodeId1).ToString();
                            item.HeureDeb1 = res.CodeId1;
                            item.DateFin1 = res.DateFinEffReturnCol;
                            item.HeureFin1 = res.CodeId2;
                            item.Duree1 = res.GarId;
                            item.DureeUnite1 = res.Code;
                            item.AlimNiv1 = res.Situation;
                        }
                        break;
                    case 2:
                        //item.GuidNiv2 = gar.;
                        item.CodeNiv2 = gar.C2SEQNIV;
                        item.CodeGarNiv2 = gar.C2GARNIV;
                        item.DescrNiv2 = gar.GADESNIV;
                        item.CaracNiv2 = gar.C2CARNIV;
                        item.NatureNiv2 = gar.C2NATNIV;
                        //if (checkedGaranties.Count == 0 && (gar.C2CARNIV == "O" || gar.C2CARNIV == "P"))
                        //    item.NatureParamNiv2 = gar.C2NATNIV;
                        //else
                        item.NatureParamNiv2 = isGarantieChecked;
                        item.CodeParentNiv2 = gar.C2SEMNIV;
                        item.CodeNiv1Niv2 = gar.C2NIVNIV;
                        item.FlagModifNiv2 = flagModif;
                        item.ParamNatModNiv2 = gar.C2NTMNIV;
                        item.InvenPossible2 = gar.GAINVNIV;
                        item.CodeInven2 = 0;
                        item.TypeInven2 = gar.GATYINIV;
                        item.CreateAvt2 = createavn;
                        item.MajAvt2 = majavn;
                        item.DateDeb2 = string.Empty;
                        item.HeureDeb2 = 0;
                        item.DateFin2 = 0;
                        item.HeureFin2 = 0;
                        item.Duree2 = 0;
                        item.DureeUnite2 = string.Empty;
                        item.TriNiv2 = gar.C2TRINIV;

                        if (res != null)
                        {
                            item.CodeInven2 = res.Id;
                            item.DateDeb2 = (res.DateDebEffReturnCol * 10000 + res.CodeId1).ToString();
                            item.HeureDeb2 = res.CodeId1;
                            item.DateFin2 = res.DateFinEffReturnCol;
                            item.HeureFin2 = res.CodeId2;
                            item.Duree2 = res.GarId;
                            item.DureeUnite2 = res.Code;
                        }
                        break;
                    case 3:
                        //item.GuidNiv3 = gar.;
                        item.CodeNiv3 = gar.C2SEQNIV;
                        item.CodeGarNiv3 = gar.C2GARNIV;
                        item.DescrNiv3 = gar.GADESNIV;
                        item.CaracNiv3 = gar.C2CARNIV;
                        item.NatureNiv3 = gar.C2NATNIV;
                        //if (checkedGaranties.Count == 0 && (gar.C2CARNIV == "O" || gar.C2CARNIV == "P"))
                        //    item.NatureParamNiv3 = gar.C2NATNIV;
                        //else
                        item.NatureParamNiv3 = isGarantieChecked;
                        item.CodeParentNiv3 = gar.C2SEMNIV;
                        item.CodeNiv1Niv3 = gar.C2NIVNIV;
                        item.FlagModifNiv3 = flagModif;
                        item.ParamNatModNiv3 = gar.C2NTMNIV;
                        item.InvenPossible3 = gar.GAINVNIV;
                        item.CodeInven3 = 0;
                        item.TypeInven3 = gar.GATYINIV;
                        item.CreateAvt3 = createavn;
                        item.MajAvt3 = majavn;
                        item.DateDeb3 = string.Empty;
                        item.HeureDeb3 = 0;
                        item.DateFin3 = 0;
                        item.HeureFin3 = 0;
                        item.Duree3 = 0;
                        item.DureeUnite3 = string.Empty;
                        item.TriNiv3 = gar.C2TRINIV;

                        if (res != null)
                        {
                            item.CodeInven3 = res.Id;
                            item.DateDeb3 = (res.DateDebEffReturnCol * 10000 + res.CodeId1).ToString();
                            item.HeureDeb3 = res.CodeId1;
                            item.DateFin3 = res.DateFinEffReturnCol;
                            item.HeureFin3 = res.CodeId2;
                            item.Duree3 = res.GarId;
                            item.DureeUnite3 = res.Code;
                        }
                        break;
                    case 4:
                        //item.GuidNiv4 = gar.;
                        item.CodeNiv4 = gar.C2SEQNIV;
                        item.CodeGarNiv4 = gar.C2GARNIV;
                        item.DescrNiv4 = gar.GADESNIV;
                        item.CaracNiv4 = gar.C2CARNIV;
                        item.NatureNiv4 = gar.C2NATNIV;
                        //if (checkedGaranties.Count == 0 && (gar.C2CARNIV == "O" || gar.C2CARNIV == "P"))
                        //    item.NatureParamNiv3 = gar.C2NATNIV;
                        //else
                        item.NatureParamNiv4 = isGarantieChecked;
                        item.CodeParentNiv4 = gar.C2SEMNIV;
                        item.CodeNiv1Niv4 = gar.C2NIVNIV;
                        item.FlagModifNiv4 = flagModif;
                        item.ParamNatModNiv4 = gar.C2NTMNIV;
                        item.InvenPossible4 = gar.GAINVNIV;
                        item.CodeInven4 = res != null ? res.Id : 0;
                        item.TypeInven4 = gar.GATYINIV;
                        item.CreateAvt4 = createavn;
                        item.MajAvt4 = majavn;
                        item.DateDeb4 = string.Empty;
                        item.HeureDeb4 = 0;
                        item.DateFin4 = 0;
                        item.HeureFin4 = 0;
                        item.Duree4 = 0;
                        item.DureeUnite4 = string.Empty;
                        item.TriNiv4 = gar.C2TRINIV;

                        if (res != null)
                        {
                            item.CodeInven4 = res.Id;
                            item.DateDeb4 = (res.DateDebEffReturnCol * 10000 + res.CodeId1).ToString();
                            item.HeureDeb4 = res.CodeId1;
                            item.DateFin4 = res.DateFinEffReturnCol;
                            item.HeureFin4 = res.CodeId2;
                            item.Duree4 = res.GarId;
                            item.DureeUnite4 = res.Code;
                        }
                        break;
                }

                var list = garanties.Where(i => i.C2SEMNIV == gar.C2SEQNIV).ToList();
                FindGarRecursive(list, codeAvn, codeOption, appliqueA, voletsBlocs, garanties, checkedBlocsVolets, checkedGaranties, portees, ref result);
            }
        }

        public static GarantieInfosDto GetGarantieInfos(string codeOffre, int version, string type, int codeFormule, int codeOption)
        {
            var paramRsq = new EacParameter[5];
            paramRsq[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramRsq[0].Value = codeOffre.PadLeft(9, ' ');
            paramRsq[1] = new EacParameter("version", DbType.Int32);
            paramRsq[1].Value = version;
            paramRsq[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramRsq[2].Value = type;
            paramRsq[3] = new EacParameter("codeFormule", DbType.Int32);
            paramRsq[3].Value = codeFormule;
            paramRsq[4] = new EacParameter("codeOption", DbType.Int32);
            paramRsq[4].Value = codeOption;

            string sql = @"SELECT KDDRSQ INT32RETURNCOL 
										FROM KPOPTAP 
										WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type AND KDDFOR = :codeFormule AND KDDOPT = :codeOption FETCH FIRST 1 ROWS ONLY";

            var codeRsq = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramRsq).FirstOrDefault();

            var paramInfos = new EacParameter[3];
            paramInfos[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramInfos[0].Value = codeOffre.PadLeft(9, ' ');
            paramInfos[1] = new EacParameter("version", DbType.Int32);
            paramInfos[1].Value = version;
            paramInfos[2] = new EacParameter("codeRsq", DbType.Int32);
            paramInfos[2].Value = codeRsq.Int32ReturnCol;

            sql = @"SELECT JEOBJ CODEOBJET, JEINA GARANTIEINDEX, JEIXL LCI, JEIXF FRANCHISE, JEIXC ASSIETTE, JECNA CATNAT FROM YPRTRSQ WHERE JEIPB = :codeOffre AND JEALX = :version AND JERSQ = :codeRsq ";

            var infos = DbBase.Settings.ExecuteList<GarantieInfosDto>(CommandType.Text, sql, paramInfos).FirstOrDefault();
            infos.CodeRisque = codeRsq.Int32ReturnCol;


            if (infos.CodeObjet != 0)
            {
                paramInfos = new EacParameter[4];
                paramInfos[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramInfos[0].Value = codeOffre.PadLeft(9, ' ');
                paramInfos[1] = new EacParameter("version", DbType.Int32);
                paramInfos[1].Value = version;
                paramInfos[2] = new EacParameter("codeRsq", DbType.Int32);
                paramInfos[2].Value = codeRsq.Int32ReturnCol;
                paramInfos[3] = new EacParameter("codeObjet", DbType.Int32);
                paramInfos[3].Value = infos.CodeObjet;

                sql = @"SELECT JGINA GARANTIEINDEX, JGIXL LCI, JGIXF FRANCHISE, JGIXC ASSIETTE, JGCNA CATNAT
										FROM YPRTOBJ 
										WHERE JGIPB = :codeOffre AND JGALX = :version AND JGRSQ = :codeRsq AND JGOBJ = :codeObjet";

                infos = DbBase.Settings.ExecuteList<GarantieInfosDto>(CommandType.Text, sql, paramInfos).FirstOrDefault();
            }

            var paramInfosEnt = new EacParameter[2];
            paramInfosEnt[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramInfosEnt[0].Value = codeOffre.PadLeft(9, ' ');
            paramInfosEnt[1] = new EacParameter("version", DbType.Int32);
            paramInfosEnt[1].Value = version;

            sql = @"SELECT JDINA GARANTIEINDEX, JDIXL LCI, JDIXF FRANCHISE, JDIXC ASSIETTE, JDCNA CATNAT
									FROM YPRTENT 
									WHERE JDIPB = :codeOffre AND JDALX = :version";

            var infosEnt = DbBase.Settings.ExecuteList<GarantieInfosDto>(CommandType.Text, sql, paramInfosEnt).FirstOrDefault();

            if (infosEnt != null)
            {
                if (infos.GarantieIndex == "")
                {
                    infos.GarantieIndex = infosEnt.GarantieIndex;
                }

                if (infos.Lci == "")
                {
                    infos.Lci = infosEnt.Lci;
                }

                if (infos.Franchise == "")
                {
                    infos.Franchise = infosEnt.Franchise;
                }

                if (infos.Assiette == "")
                {
                    infos.Assiette = infosEnt.Assiette;
                }

                if (infos.CatNat == "")
                {
                    infos.CatNat = infosEnt.CatNat;
                }
            }
            return infos;
        }

        public static GarantieInfo GetInfoGarantieById(string idGar)
        {
            var param = new List<EacParameter>();

            var p = new EacParameter
            {
                ParameterName = "idGar",
                DbType = DbType.Int32,
                Value = Convert.ToInt32(idGar)
            };

            param.Add(p);

            var sql = @"
                    SELECT KDEFOR CODEFORMULE,KDEOPT CODEOPTION,KDEGARAN CODEGARANTIE,GADES LIBELLE 
                    FROM KPGARAN INNER JOIN KGARAN ON KDEGARAN = GAGAR AND KDEID = :idGar";

            var gr = DbBase.Settings.ExecuteList<GarantieInfo>(CommandType.Text, sql, param).FirstOrDefault();
            gr.Id = long.Parse(idGar);
            return gr;
        }
        //OBO
        public static void GetGarantieDateRange(string codeOffre, int version, string type, int codeFor, int codeOpt, ref DateTime? dateMin, ref DateTime? dateMax)
        {
            //récup des infos de KPOPTAP
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFor", DbType.Int32);
            param[3].Value = codeFor;
            param[4] = new EacParameter("codeOption", DbType.Int32);
            param[4].Value = codeOpt;

            var sql = @"SELECT KDDRSQ INT32RETURNCOL, KDDOBJ INT32RETURNCOL2 FROM KPOPTAP WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type AND KDDFOR = :codeFor AND KDDOPT = :codeOpt";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            var codeRsq = result.FirstOrDefault().Int32ReturnCol;
            var codesObj = string.Empty;
            if (result.Count > 1 || result.FirstOrDefault().Int32ReturnCol2 != 0)
            {
                result.ForEach(o =>
                {
                    codesObj += "," + o.Int32ReturnCol2;
                });
                codesObj = codesObj.Substring(1);
            }

            var param2 = new EacParameter[3];
            param2[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param2[0].Value = codeOffre.PadLeft(9, ' ');
            param2[1] = new EacParameter("version", DbType.Int32);
            param2[1].Value = version;
            param2[2] = new EacParameter("codeRsq", DbType.Int32);
            param2[2].Value = codeRsq;

            sql = string.Format(@"SELECT  IFNULL ( MIN ( JGVDA * 100000000 + JGVDM * 1000000 + JGVDJ * 10000 + JGVDH ) , 0 ) DATEDEBRETURNCOL , 
									IFNULL ( MAX ( JGVFA * 100000000 + JGVFM * 1000000 + JGVFJ * 10000 + JGVFH ) , 0 ) DATEFINRETURNCOL
									FROM YPRTOBJ
									WHERE JGIPB = :codeOffre AND JGALX = :version AND JGRSQ = :codeRsq {0}",
                         !string.IsNullOrEmpty(codesObj) ? " AND JGOBJ IN (" + codesObj + ")" : string.Empty);
            result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param2);

            dateMin = AlbConvert.ConvertIntToDateHour(result.Min(i => i.DateDebReturnCol));
            dateMax = AlbConvert.ConvertIntToDateHour(result.Max(i => i.DateFinReturnCol));


            if (dateMin == null || dateMax == null)
            {
                var param3 = new EacParameter[3];
                param3[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param3[0].Value = codeOffre.PadLeft(9, ' ');
                param3[1] = new EacParameter("version", DbType.Int32);
                param3[1].Value = version;
                param3[2] = new EacParameter("codeRsq", DbType.Int32);
                param3[2].Value = codeRsq;

                sql = @"SELECT JEVDA * 100000000 + JEVDM * 1000000 + JEVDJ * 10000 + JEVDH  DATEDEBRETURNCOL, 
							   JEVFA * 100000000 + JEVFM * 1000000 + JEVFJ * 10000 + JEVFH DATEFINRETURNCOL
						FROM  YPRTRSQ 
						WHERE JEIPB = :codeOffre AND  JEALX = :version AND JERSQ = :codeRsq ";
                var line = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param3).FirstOrDefault();

                if (dateMin == null)
                {
                    dateMin = AlbConvert.ConvertIntToDateHour(line != null ? line.DateDebReturnCol : 0);
                }

                if (dateMax == null)
                {
                    dateMax = AlbConvert.ConvertIntToDateHour(line != null ? line.DateFinReturnCol : 0);
                }

                if (dateMin == null || dateMax == null)
                {
                    var param4 = new EacParameter[3];
                    param4[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                    param4[0].Value = codeOffre.PadLeft(9, ' ');
                    param4[1] = new EacParameter("version", DbType.Int32);
                    param4[1].Value = version;
                    param4[2] = new EacParameter("codeRsq", DbType.Int32);
                    param4[2].Value = codeRsq;
                    sql = @"SELECT IFNULL ( PBEFA * 100000000 + PBEFM * 1000000 + PBEFJ * 10000 + PBEFH , 0 ) DATEDEBRETURNCOL, 
								   IFNULL ( PBFEA * 100000000 + PBFEM * 1000000 + PBFEJ * 10000 + PBFEH , 0 ) DATEFINRETURNCOL
						    FROM   YPOBASE 
							WHERE  PBIPB = :codeOffre  AND PBALX = :version AND PBTYP = :type ";

                    line = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param4).FirstOrDefault();

                    if (dateMin == null)
                    {
                        dateMin = AlbConvert.ConvertIntToDateHour(line != null ? line.DateDebReturnCol : 0);
                    }

                    if (dateMax == null)
                    {
                        dateMax = AlbConvert.ConvertIntToDateHour(line != null ? line.DateFinReturnCol : 0);
                    }
                }
            }


        }

        public static string GetGarantiePeriodicite(string codeOffre, int version, string type)
        {
            string garTmp = string.Empty;

            var paramPeriodicite = new EacParameter[3];
            paramPeriodicite[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramPeriodicite[0].Value = codeOffre.PadLeft(9, ' ');
            paramPeriodicite[1] = new EacParameter("version", DbType.Int32);
            paramPeriodicite[1].Value = version;
            paramPeriodicite[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramPeriodicite[2].Value = type;

            string sql = @"SELECT PBPER FROM YPOBASE WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            var periodicite = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, paramPeriodicite);

            var paramrsqTmp = new EacParameter[2];
            paramrsqTmp[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramrsqTmp[0].Value = codeOffre.PadLeft(9, ' ');
            paramrsqTmp[1] = new EacParameter("version", DbType.Int32);
            paramrsqTmp[1].Value = version;
            sql = @"SELECT DISTINCT JETEM STRRETURNCOL 
								FROM YPRTRSQ 
								WHERE JEIPB = :codeOffre AND JEALX = :version";

            var rsqTmp = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, paramrsqTmp);

            if (periodicite.ToString() == "E" || periodicite.ToString() == "U" || rsqTmp.ToString() == "O")
            {
                garTmp = "O";
            }

            return garTmp;
        }


        public static FormuleGarantieDetailsPlatDto CalculateAndUpdateDatesGar(string codeOffre, int version, string type, string codeAvn, int codeFormule, int codeOption, DateTime? dateEffAvnModifLocale, int codeGarantie = 0)
        {
            var param = new List<EacParameter>
                   {
                       new EacParameter("P_CODEAFFAIRE", DbType.AnsiStringFixedLength){ Value = codeOffre.PadLeft(9, ' ')},
                       new EacParameter("P_VERSION", DbType.Int32){ Value = version },
                       new EacParameter("P_TYPE", DbType.AnsiStringFixedLength){ Value = type },
                       new EacParameter("P_CODEFOR", DbType.AnsiStringFixedLength){ Value = codeFormule },
                       new EacParameter("P_CODEOPT", DbType.AnsiStringFixedLength){ Value = codeOption },
                       new EacParameter("P_CODEGAR", DbType.AnsiStringFixedLength){ Value = codeGarantie },
                       new EacParameter("P_DATEDEBSTD", DbType.AnsiStringFixedLength){ Value = 0, Direction = ParameterDirection.InputOutput, Size = 12 },
                       new EacParameter("P_DATEFINSTD", DbType.AnsiStringFixedLength){ Value = 0, Direction = ParameterDirection.InputOutput, Size = 12 }
                   };

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_GETDATEGARSTD", param);

            var dateDebutGar = param[6].Value.ToString() != "0" ? Convert.ToInt32(param[6].Value.ToString().Substring(0, 8)) : 0;
            var dateFinGar = param[7].Value.ToString() != "0" ? Convert.ToInt32(param[7].Value.ToString().Substring(0, 8)) : 0;
            var heureDebGar = param[6].Value.ToString() != "0" ? Convert.ToInt32(param[6].Value.ToString().Substring(8).PadRight(6, '0')) : 0;
            var heureFinGar = param[7].Value.ToString() != "0" ? Convert.ToInt32(param[7].Value.ToString().Substring(8).PadRight(6, '0')) : 0;

            if (Convert.ToInt32(codeAvn) > 0)
            {
                var sqlDateGar = string.Format(@"SELECT KDEDATDEB DATEDEBRETURNCOL, kdedatfin DATEFINRETURNCOL, KDEWDDEB DATEDEBEFFRETURNCOL, KDEWDFIN DATEFINEFFRETURNCOL, KDECRAVN AVN_CREA,
                            KDEMODI STRRETURNCOL FROM KPGARAN WHERE KDEID = {0}", codeGarantie);
                var resDateGar = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlDateGar);

                long tmpDateDebutGar = 0;
                int tmpCodeAvn = 0;
                string tmpModif = "N";

                foreach (DtoCommon garDate in resDateGar)
                {
                    if (garDate != null) // && garDate.DateDebReturnCol != 0)
                    {
                        tmpDateDebutGar = Convert.ToInt32(garDate.DateDebReturnCol);
                        tmpCodeAvn = Convert.ToInt32(garDate.AvenantCrea);
                        tmpModif = garDate.StrReturnCol;
                    }

                }

                if (tmpCodeAvn == Convert.ToInt32(codeAvn) && dateEffAvnModifLocale.HasValue)
                {
                    if (tmpDateDebutGar > AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value)
                    {
                        if (tmpModif == "O")
                        {
                            dateDebutGar = Convert.ToInt32(tmpDateDebutGar);
                        }
                        else
                        {
                            dateDebutGar = AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value;
                        }
                    }
                    else
                    {
                        dateDebutGar = AlbConvert.ConvertDateToInt(dateEffAvnModifLocale).Value;
                    }



                    var paramGar = new List<EacParameter>
                {
                    new EacParameter("dateDebut", DbType.Int32){ Value = dateDebutGar},
                    new EacParameter("heureDeb", DbType.Int32){ Value = heureDebGar },
                    new EacParameter("idGar", DbType.AnsiStringFixedLength){ Value = codeGarantie }
                };
                    var sqlGar = @"UPDATE KPGARAN SET KDEDATDEB = :dateDebutGar, KDEHEUDEB = :heureDebGar WHERE KDEID = :codeGarantie";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlGar, paramGar);
                }
            }

            return new FormuleGarantieDetailsPlatDto
            {
                DateDebW = dateDebutGar,
                HeureDebW = heureDebGar,
                DateFinW = dateFinGar,
                HeureFinW = heureFinGar
            };

        }

        public static GarantiePortee CalculateDatesGarDetails(string codeOffre, int version, string type, string codeAvn, int codeFormule, int codeOption)
        {
            var result = new List<GarantiePortee>();


            //GET PERI
            var paramSappliquea = new EacParameter[4];
            paramSappliquea[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramSappliquea[0].Value = codeOffre.PadLeft(9, ' ');
            paramSappliquea[1] = new EacParameter("version", DbType.Int32);
            paramSappliquea[1].Value = version;
            paramSappliquea[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramSappliquea[2].Value = type;
            paramSappliquea[3] = new EacParameter("codeFormule", DbType.Int32);
            paramSappliquea[3].Value = codeFormule;

            var sql = @"SELECT KDDPERI STRRETURNCOL,KDDOBJ INT32RETURNCOL,KDDRSQ INT32RETURNCOL2 
								  FROM KPOPTAP WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type  AND KDDFOR = :codeFormule";
            var sappliquea = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramSappliquea);

            var paramMatriceRisque = new EacParameter[2];
            paramMatriceRisque[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramMatriceRisque[0].Value = codeOffre.PadLeft(9, ' ');
            paramMatriceRisque[1] = new EacParameter("version", DbType.Int32);
            paramMatriceRisque[1].Value = version;

            sql = @"SELECT JGRSQ INT32RETURNCOL,JGOBJ INT32RETURNCOL2 
								  FROM YPRTOBJ WHERE JGIPB = :codeOffre AND JGALX = :version";
            var matriceRisque = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramMatriceRisque);



            //Initialisation des garanties
            var firstOrDefaultSapplique = sappliquea.FirstOrDefault();

            var codeRisque = firstOrDefaultSapplique != null ? firstOrDefaultSapplique.Int32ReturnCol2 : 0;
            if (sappliquea.Any(i => i.StrReturnCol == "OB"))
            {
                sappliquea.ForEach(i => result.Add(new GarantiePortee
                {
                    //IdGarantie = gar.Id,
                    CodeRisque = i.Int32ReturnCol2,
                    CodeObjet = i.Int32ReturnCol
                }));
            }
            else if (sappliquea.Any(i => i.StrReturnCol == "RQ"))
            {
                matriceRisque.Where(i => i.Int32ReturnCol == codeRisque).ToList().ForEach(
                    i => result.Add(new GarantiePortee
                    {
                        //IdGarantie = gar.Id,
                        CodeRisque = i.Int32ReturnCol,
                        CodeObjet = i.Int32ReturnCol2
                    }));
            }

            //OBJ
            var paramObjets = new EacParameter[3];
            paramObjets[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramObjets[0].Value = codeOffre.PadLeft(9, ' ');
            paramObjets[1] = new EacParameter("version", DbType.Int32);
            paramObjets[1].Value = version;
            paramObjets[2] = new EacParameter("codeRisque", DbType.Int32);
            paramObjets[2].Value = codeRisque;


            sql = string.Format(@"SELECT JGOBJ INT32RETURNCOL,
										IFNULL (  JGVDA * 100000000 + JGVDM * 1000000 + JGVDJ * 10000 + JGVDH  , 0 ) DATEDEBRETURNCOL , 
										IFNULL (  JGVFA * 100000000 + JGVFM * 1000000 + JGVFJ * 10000 + JGVFH  , 0 ) DATEFINRETURNCOL
							    FROM  YPRTOBJ
							    WHERE JGIPB = :codeOffre AND JGALX = :version AND JGRSQ = :codeRisque AND JGOBJ IN ({0})", string.Join(",", result.Select(i => i.CodeObjet).Distinct()));


            var objets = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramObjets);

            result.ForEach(g =>
            {
                var obj = objets.FirstOrDefault(i => i.Int32ReturnCol == g.CodeObjet);
                if (obj != null)
                {
                    g.DateDebut = obj.DateDebReturnCol;
                    g.DateFin = obj.DateFinReturnCol;
                }
            });

            result = result.GroupBy(i => i.IdGarantie).Select(i =>
            {
                var p = new GarantiePortee()
                {
                    IdGarantie = i.Key
                };

                //var itemsDeb = i; // .Where(t => t.DateDebut > 0);
                //p.DateDebut = !itemsDeb.Any() ? 0 : itemsDeb.Min(t => t.DateDebut);
                p.DateDebut = i.Min(t => t.DateDebut);

                //var itemsFin = i; // .Where(t => t.DateFin > 0);
                //p.DateFin = !itemsFin.Any() ? 0 : itemsFin.Max(t => t.DateFin);
                p.DateFin = i.Min(t => t.DateFin) == 0 ? 0 : i.Max(t => t.DateFin);

                return p;
            }).ToList();

            //RSQ
            var paramRisque = new EacParameter[3];
            paramRisque[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramRisque[0].Value = codeOffre.PadLeft(9, ' ');
            paramRisque[1] = new EacParameter("version", DbType.Int32);
            paramRisque[1].Value = version;
            paramRisque[2] = new EacParameter("codeRisque", DbType.Int32);
            paramRisque[2].Value = codeRisque;

            sql = @"SELECT JEVDA * 100000000 + JEVDM * 1000000 + JEVDJ * 10000 + JEVDH  DATEDEBRETURNCOL, 
										JEVFA * 100000000 + JEVFM * 1000000 + JEVFJ * 10000 + JEVFH DATEFINRETURNCOL
							FROM  YPRTRSQ 
							WHERE JEIPB = :codeOffre AND  JEALX = :version AND JERSQ = :codeRisque";



            var risque = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramRisque).FirstOrDefault();
            if (risque != null)
            {
                result.ForEach(g =>
                {
                    if (g.DateDebut == 0)
                    {
                        g.DateDebut = risque.DateDebReturnCol;
                    }

                    if (g.DateFin == 0)
                    {
                        g.DateFin = risque.DateFinReturnCol;
                    }
                });
            }

            //AN

            var paramActeGestion = new EacParameter[4];
            paramActeGestion[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramActeGestion[0].Value = codeOffre.PadLeft(9, ' ');
            paramActeGestion[1] = new EacParameter("version", DbType.Int32);
            paramActeGestion[1].Value = version;
            paramActeGestion[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramActeGestion[2].Value = type;
            paramActeGestion[3] = new EacParameter("codeAvn", DbType.AnsiStringFixedLength);
            paramActeGestion[3].Value = codeAvn;

            if (codeAvn == "0")
            {
                sql = @"SELECT IFNULL ( PBEFA * 100000000 + PBEFM * 1000000 + PBEFJ * 10000 + PBEFH , 0 ) DATEDEBRETURNCOL, 
                                         IFNULL ( PBFEA * 100000000 + PBFEM * 1000000 + PBFEJ * 10000 + PBFEH , 0 ) DATEFINRETURNCOL,
                                 PBAVN AVN_CREA
                                 FROM  YPOBASE 
                                 WHERE PBIPB = :codeOffre  AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn";
            }
            else
            {
                sql = @"SELECT IFNULL ( PBAVA * 100000000 + PBAVM * 1000000 + PBAVJ * 10000 + LEFT(KAAAVH,4) , 0 ) DATEDEBRETURNCOL, 
                                             IFNULL ( PBFEA * 100000000 + PBFEM * 1000000 + PBFEJ * 10000 + PBFEH , 0 ) DATEFINRETURNCOL,
                                 PBAVN AVN_CREA
                                 FROM  YPOBASE 
                                         INNER JOIN KPENT ON  KAAIPB  =  PBIPB  AND KAAALX = PBALX AND KAATYP = PBTYP
                                 WHERE PBIPB = :codeOffre  AND PBALX = :version AND PBTYP = :type AND PBAVN = :codeAvn";
            }

            var actedeGestion = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramActeGestion).FirstOrDefault();

            if (actedeGestion != null)
            {
                result.ForEach(g =>
                {
                    g.AvenantCreation = Convert.ToInt32(actedeGestion.AvenantCrea);
                    if (g.DateDebut == 0)
                    {
                        g.DateDebut = actedeGestion.DateDebReturnCol;
                    }

                    if (g.DateFin == 0)
                    {
                        g.DateFin = actedeGestion.DateFinReturnCol;
                    }
                });
            }

            var numAvn = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;

            if (numAvn != 0)
            {
                var paramOption = new List<EacParameter> {
                    new EacParameter("codeOffre", DbType.AnsiStringFixedLength){ Value = codeOffre.PadLeft(9, ' ') },
                    new EacParameter("type", DbType.AnsiStringFixedLength) { Value = type },
                    new EacParameter("version", DbType.Int32) { Value = version },
                    new EacParameter("codeFormule", DbType.Int32){ Value = codeFormule },
                    new EacParameter("codeOption", DbType.Int32) { Value = codeOption }
                };

                const string SelectDateAvenant = @"
SELECT (KDBAVA * 100000000 + KDBAVM * 1000000 + KDBAVJ * 10000) AVDATE 
FROM KPOPT WHERE KDBIPB = :codeOffre AND KDBTYP = :type AND KDBALX = :version AND KDBFOR = :codeFormule AND KDBOPT = :codeOption";
                var option = DbBase.Settings.ExecuteScalar(CommandType.Text, SelectDateAvenant, paramOption);

                if (option != null)
                {
                    long date = Convert.ToInt64(option);
                    result.ForEach(g =>
                    {
                        if (date > g.DateDebut)
                        {
                            g.DateDebut = date;
                        }
                    });
                }
            }
            return result.FirstOrDefault();
        }


        public static void PopulateKpoptap(string codeOffre, string version, string type, KpoptapDto itemtoInsert)
        {
            var paramOption = new EacParameter[7];
            paramOption[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramOption[0].Value = codeOffre.PadLeft(9, ' ');
            paramOption[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            paramOption[1].Value = version;
            paramOption[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramOption[2].Value = type;
            paramOption[3] = new EacParameter("KDDFOR", DbType.Int32);
            paramOption[3].Value = itemtoInsert.KDDFOR;
            paramOption[4] = new EacParameter("KDDOPT", DbType.Int32);
            paramOption[4].Value = itemtoInsert.KDDOPT;
            paramOption[5] = new EacParameter("KDDRSQ", DbType.Int32);
            paramOption[5].Value = itemtoInsert.KDDRSQ;
            paramOption[6] = new EacParameter("KDDOBJ", DbType.Int32);
            paramOption[6].Value = itemtoInsert.KDDOBJ;

            var sql = @"SELECT COUNT(*) FROM KPOPTAP WHERE  KDDIPB = :codeOffre AND KDDALX = :version  AND KDDTYP = :type AND KDDFOR = :KDDFOR AND KDDOPT = :KDDOPT AND KDDRSQ = :KDDRSQ AND KDDOBJ = :KDDOBJ";

            var exists = (int)DbBase.Settings.ExecuteScalar(CommandType.Text, sql, paramOption) != 0;
            if (!exists)
            {
                #region Insert
                var param = new EacParameter[16];
                param[0] = new EacParameter("P_KDDALX", DbType.Int32);
                param[0].Value = itemtoInsert.KDDALX;
                param[1] = new EacParameter("P_KDDCRD", DbType.Int64);
                param[1].Value = itemtoInsert.KDDCRD;
                param[2] = new EacParameter("P_KDDCRU", DbType.AnsiStringFixedLength);
                param[2].Value = itemtoInsert.KDDCRU;
                param[3] = new EacParameter("P_KDDFOR", DbType.Int32);
                param[3].Value = itemtoInsert.KDDFOR;
                param[4] = new EacParameter("P_KDDID", DbType.Int64);
                param[4].Value = itemtoInsert.KDDID;
                param[5] = new EacParameter("P_KDDINVEN", DbType.Int64);
                param[5].Value = itemtoInsert.KDDINVEN;
                param[6] = new EacParameter("P_KDDINVEP", DbType.Int32);
                param[6].Value = itemtoInsert.KDDINVEP;
                param[7] = new EacParameter("P_KDDIPB", DbType.AnsiStringFixedLength);
                param[7].Value = itemtoInsert.KDDIPB.PadLeft(9, ' ');
                param[8] = new EacParameter("P_KDDKDBID", DbType.Int64);
                param[8].Value = itemtoInsert.KDDKDBID;
                param[9] = new EacParameter("P_KDDMAJD", DbType.Int64);
                param[9].Value = itemtoInsert.KDDMAJD;
                param[10] = new EacParameter("P_KDDMAJU", DbType.AnsiStringFixedLength);
                param[10].Value = itemtoInsert.KDDMAJU;
                param[11] = new EacParameter("P_KDDOBJ", DbType.Int32);
                param[11].Value = itemtoInsert.KDDOBJ;
                param[12] = new EacParameter("P_KDDOPT", DbType.Int32);
                param[12].Value = itemtoInsert.KDDOPT;
                param[13] = new EacParameter("P_KDDPERI", DbType.AnsiStringFixedLength);
                param[13].Value = itemtoInsert.KDDPERI;
                param[14] = new EacParameter("P_KDDRSQ", DbType.Int32);
                param[14].Value = itemtoInsert.KDDRSQ;
                param[15] = new EacParameter("P_KDDTYP", DbType.AnsiStringFixedLength);
                param[15].Value = itemtoInsert.KDDTYP;

                sql = string.Format(@"INSERT INTO  KPOPTAP 
										( 	
											KDDALX,	
											KDDCRD,	
											KDDCRU,	
											KDDFOR,	
											KDDID,	
											KDDINVEN,	
											KDDINVEP,	
											KDDIPB,	
											KDDKDBID,	
											KDDMAJD,	
											KDDMAJU,	
											KDDOBJ,	
											KDDOPT,	
											KDDPERI,	
											KDDRSQ,	
											KDDTYP
											 ) 
										VALUES 
											( :P_KDDALX,	
											:P_KDDCRD,	
											:P_KDDCRU,	
											:P_KDDFOR,	
											:P_KDDID,	
											:P_KDDINVEN,	
											:P_KDDINVEP,	
											:P_KDDIPB,	
											:P_KDDKDBID,	
											:P_KDDMAJD,	
											:P_KDDMAJU,	
											:P_KDDOBJ,	
											:P_KDDOPT,	
											:P_KDDPERI,	
											:P_KDDRSQ,	
											:P_KDDTYP)");

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                #endregion
            }


        }


        public static int PopulateKpoptd(string codeOffre, int version, string type, KpoptdDto itemtoInsert, bool VoletBlocUpdate)
        {
            int idVolet = 0;
            var paramOption = new EacParameter[8];
            paramOption[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramOption[0].Value = codeOffre.PadLeft(9, ' ');
            paramOption[1] = new EacParameter("version", DbType.Int32);
            paramOption[1].Value = version;
            paramOption[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramOption[2].Value = type;
            paramOption[3] = new EacParameter("KDCFOR", DbType.Int32);
            paramOption[3].Value = itemtoInsert.KDCFOR;
            paramOption[4] = new EacParameter("KDCOPT", DbType.Int32);
            paramOption[4].Value = itemtoInsert.KDCOPT;
            paramOption[5] = new EacParameter("KDCTENG", DbType.AnsiStringFixedLength);
            paramOption[5].Value = itemtoInsert.KDCTENG;
            paramOption[6] = new EacParameter("KDCKAKID", DbType.Int64);
            paramOption[6].Value = itemtoInsert.KDCKAKID;
            paramOption[7] = new EacParameter("KDCKAEID", DbType.Int64);
            paramOption[7].Value = itemtoInsert.KDCKAEID;

            var sql = @"SELECT KDCID ID,KDCKAKID INT64RETURNCOL FROM KPOPTD WHERE  KDCIPB = :codeOffre AND KDCALX = :version  AND KDCTYP = :type AND KDCFOR = :KDCFOR
									AND KDCOPT = :KDCOPT AND KDCTENG = :KDCTENG AND KDCKAKID = :KDCKAKID AND KDCKAEID = :KDCKAEID";

            var kpoptd = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramOption).FirstOrDefault();
            var exists = kpoptd != null;
            if (exists)
            {
                idVolet = (int)kpoptd.Id;
            }

            if (VoletBlocUpdate)
            {
                if (!exists)// && Convert.ToBoolean(itemtoInsert.KDCFLAG))
                {
                    idVolet = CommonRepository.GetAS400Id("KDCID");
                    #region Insert

                    var param = new EacParameter[19];
                    param[0] = new EacParameter("P_KDCALX", DbType.Int32);
                    param[0].Value = itemtoInsert.KDCALX;
                    param[1] = new EacParameter("P_KDCCRD", DbType.Int64);
                    param[1].Value = itemtoInsert.KDCCRD;
                    param[2] = new EacParameter("P_KDCCRU", DbType.AnsiStringFixedLength);
                    param[2].Value = itemtoInsert.KDCCRU;
                    param[3] = new EacParameter("P_KDCFLAG", DbType.Int32);
                    param[3].Value = itemtoInsert.KDCFLAG;
                    param[4] = new EacParameter("P_KDCFOR", DbType.Int32);
                    param[4].Value = itemtoInsert.KDCFOR;
                    param[5] = new EacParameter("P_KDCID", DbType.Int32);
                    param[5].Value = idVolet;
                    param[6] = new EacParameter("P_KDCIPB", DbType.AnsiStringFixedLength);
                    param[6].Value = itemtoInsert.KDCIPB.PadLeft(9, ' ');
                    param[7] = new EacParameter("P_KDCKAEID", DbType.Int64);
                    param[7].Value = itemtoInsert.KDCKAEID;
                    param[8] = new EacParameter("P_KDCKAKID", DbType.Int64);
                    param[8].Value = itemtoInsert.KDCKAKID;
                    param[9] = new EacParameter("P_KDCKAQID", DbType.Int64);
                    param[9].Value = itemtoInsert.KDCKAQID;
                    param[10] = new EacParameter("P_KDCKARID", DbType.Int64);
                    param[10].Value = itemtoInsert.KDCKARID;
                    param[11] = new EacParameter("P_KDCKDBID", DbType.Int64);
                    param[11].Value = itemtoInsert.KDCKDBID;
                    param[12] = new EacParameter("P_KDCMAJD", DbType.Int64);
                    param[12].Value = itemtoInsert.KDCMAJD;
                    param[13] = new EacParameter("P_KDCMAJU", DbType.AnsiStringFixedLength);
                    param[13].Value = itemtoInsert.KDCMAJU;
                    param[14] = new EacParameter("P_KDCMODELE", DbType.AnsiStringFixedLength);
                    param[14].Value = itemtoInsert.KDCMODELE;
                    param[15] = new EacParameter("P_KDCOPT", DbType.Int32);
                    param[15].Value = itemtoInsert.KDCOPT;
                    param[16] = new EacParameter("P_KDCTENG", DbType.AnsiStringFixedLength);
                    param[16].Value = itemtoInsert.KDCTENG;
                    param[17] = new EacParameter("P_KDCTYP", DbType.AnsiStringFixedLength);
                    param[17].Value = itemtoInsert.KDCTYP;
                    param[18] = new EacParameter("P_KDCORDRE", DbType.Single);
                    param[18].Value = itemtoInsert.KDCORDRE;

                    sql = string.Format(@"INSERT INTO KPOPTD 
										(   KDCALX,	
											KDCCRD,	
											KDCCRU,	
											KDCFLAG,	
											KDCFOR,	
											KDCID,	
											KDCIPB,	
											KDCKAEID,	
											KDCKAKID,	
											KDCKAQID,	
											KDCKARID,	
											KDCKDBID,	
											KDCMAJD,	
											KDCMAJU,	
											KDCMODELE,	
											KDCOPT,	
											KDCTENG,	
											KDCTYP,
											KDCORDRE)
										VALUES
											( :P_KDCALX,	
											:P_KDCCRD,	
											:P_KDCCRU,	
											:P_KDCFLAG,	
											:P_KDCFOR,	
											:P_KDCID,	
											:P_KDCIPB,	
											:P_KDCKAEID,	
											:P_KDCKAKID,	
											:P_KDCKAQID,	
											:P_KDCKARID,	
											:P_KDCKDBID,	
											:P_KDCMAJD,	
											:P_KDCMAJU,	
											:P_KDCMODELE,	
											:P_KDCOPT,	
											:P_KDCTENG,	
											:P_KDCTYP,	
											:P_KDCORDRE)");


                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                    #endregion
                }
                else if (exists && !Convert.ToBoolean(itemtoInsert.KDCFLAG))
                {
                    #region DELETE

                    if (itemtoInsert.KDCTENG == "V")
                    {
                        CommonRepository.SetTraceLog(codeOffre, version.ToString(), type, 0, "delete garan V", "PopulateOPTD", AlbConvert.ConvertDateToStr(DateTime.Now), "TRACEDEL");
                        //Supprimer tous les garanties du bloc dans KPGARTAR
                        var param = new EacParameter[7];
                        param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                        param[0].Value = codeOffre.PadLeft(9, ' ');
                        param[1] = new EacParameter("version", DbType.Int32);
                        param[1].Value = version;
                        param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                        param[2].Value = type;
                        param[3] = new EacParameter("KDCFOR", DbType.Int32);
                        param[3].Value = itemtoInsert.KDCFOR;
                        param[4] = new EacParameter("KDCOPT", DbType.Int32);
                        param[4].Value = itemtoInsert.KDCOPT;
                        param[5] = new EacParameter("KDCTENG", DbType.AnsiStringFixedLength);
                        param[5].Value = "B";
                        param[6] = new EacParameter("KDCKAKID", DbType.Int64);
                        param[6].Value = itemtoInsert.KDCKAKID;


                        sql = @"DELETE FROM KPGARTAR  WHERE KDGKDEID IN 
										 (SELECT KDEID FROM KPGARAN WHERE KDEKDCID IN 
										  (SELECT KDCID FROM KPOPTD 
											WHERE KDCIPB = :codeOffre AND KDCALX = :version AND KDCTYP = :type AND KDCFOR = :KDCFOR 
											AND KDCOPT = :KDCOPT AND KDCTENG = :KDCTENG AND KDCKAKID = :KDCKAKID))";

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                        //Supprimer tous les garanties du bloc dans KPGARAN
                        sql = @"DELETE FROM KPGARAN WHERE KDEKDCID IN (SELECT KDCID FROM KPOPTD 
											WHERE KDCIPB = :codeOffre AND KDCALX = :version AND KDCTYP = :type AND KDCFOR = :KDCFOR 
											AND KDCOPT = :KDCOPT AND KDCTENG = :KDCTENG AND KDCKAKID = :KDCKAKID)";

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                        // Supprimer tous les blocs  du volet
                        var paramDelete = new EacParameter[6];
                        paramDelete[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                        paramDelete[0].Value = codeOffre.PadLeft(9, ' ');
                        paramDelete[1] = new EacParameter("version", DbType.Int32);
                        paramDelete[1].Value = version;
                        paramDelete[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                        paramDelete[2].Value = type;
                        paramDelete[3] = new EacParameter("KDCFOR", DbType.Int32);
                        paramDelete[3].Value = itemtoInsert.KDCFOR;
                        paramDelete[4] = new EacParameter("KDCOPT", DbType.Int32);
                        paramDelete[4].Value = itemtoInsert.KDCOPT;
                        paramDelete[5] = new EacParameter("kpoptd", DbType.Int64);
                        paramDelete[5].Value = kpoptd.Int64ReturnCol;

                        sql = @"DELETE FROM KPOPTD WHERE KDCIPB = :codeOffre AND KDCALX = :version AND KDCTYP = :type AND KDCFOR = :KDCFOR AND KDCOPT = :KDCOPT AND KDCKAKID = :kpoptd AND KDCTENG = 'B'";

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramDelete);
                        idVolet = 0;
                    }
                    else if (itemtoInsert.KDCTENG == "B")
                    {
                        CommonRepository.SetTraceLog(codeOffre, version.ToString(), type, 0, "delete garan B", "PopulateOPTD", AlbConvert.ConvertDateToStr(DateTime.Now), "TRACEDEL");
                        //Supprimer tous les garanties du bloc 
                        var param = new EacParameter[1];
                        param[0] = new EacParameter("id", DbType.Int64);
                        param[0].Value = kpoptd.Id;

                        sql = @"DELETE FROM KPGARTAR  WHERE KDGKDEID IN 
										 (SELECT KDEID FROM KPGARAN WHERE KDEKDCID = :id)";

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                        sql = @"DELETE FROM KPGARAN WHERE KDEKDCID = :id";

                        DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

                        idVolet = 0;

                    }


                    //Supprime le Volet ou le bloc
                    var paramDel = new EacParameter[8];
                    paramDel[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                    paramDel[0].Value = codeOffre.PadLeft(9, ' ');
                    paramDel[1] = new EacParameter("version", DbType.Int32);
                    paramDel[1].Value = version;
                    paramDel[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                    paramDel[2].Value = type;
                    paramDel[3] = new EacParameter("KDCFOR", DbType.Int32);
                    paramDel[3].Value = itemtoInsert.KDCFOR;
                    paramDel[4] = new EacParameter("KDCOPT", DbType.Int32);
                    paramDel[4].Value = itemtoInsert.KDCOPT;
                    paramDel[5] = new EacParameter("KDCTENG", DbType.AnsiStringFixedLength);
                    paramDel[5].Value = itemtoInsert.KDCTENG;
                    paramDel[6] = new EacParameter("KDCKAKID", DbType.Int64);
                    paramDel[6].Value = itemtoInsert.KDCKAKID;
                    paramDel[7] = new EacParameter("KDCKAEID", DbType.Int64);
                    paramDel[7].Value = itemtoInsert.KDCKAEID;

                    sql = @"DELETE FROM KPOPTD WHERE  KDCIPB = :codeOffre AND KDCALX = :version  AND KDCTYP = :type AND KDCFOR = :KDCFOR AND KDCOPT = :KDCOPT AND KDCTENG = :KDCTENG AND KDCKAKID = :KDCKAKID AND KDCKAEID = :KDCKAEID";

                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramDel);

                    #endregion
                }
                else
                {
                    #region Update

                    var paramUp = new EacParameter[11];
                    paramUp[0] = new EacParameter("aValue", DbType.Int32);
                    paramUp[0].Value = 1;
                    paramUp[1] = new EacParameter("KDCKAKID", DbType.Int64);
                    paramUp[1].Value = itemtoInsert.KDCKARID;
                    paramUp[2] = new EacParameter("KDCMODELE", DbType.AnsiStringFixedLength);
                    paramUp[2].Value = itemtoInsert.KDCMODELE;
                    paramUp[3] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                    paramUp[3].Value = codeOffre.PadLeft(9, ' ');
                    paramUp[4] = new EacParameter("version", DbType.Int32);
                    paramUp[4].Value = version;
                    paramUp[5] = new EacParameter("type", DbType.AnsiStringFixedLength);
                    paramUp[5].Value = type;
                    paramUp[6] = new EacParameter("KDCFOR", DbType.Int32);
                    paramUp[6].Value = itemtoInsert.KDCFOR;
                    paramUp[7] = new EacParameter("KDCOPT", DbType.Int32);
                    paramUp[7].Value = itemtoInsert.KDCOPT;
                    paramUp[8] = new EacParameter("KDCTENG", DbType.AnsiStringFixedLength);
                    paramUp[8].Value = itemtoInsert.KDCTENG;
                    paramUp[9] = new EacParameter("KDCKAKID", DbType.Int64);
                    paramUp[9].Value = itemtoInsert.KDCKAKID;
                    paramUp[10] = new EacParameter("KDCKAEID", DbType.Int64);
                    paramUp[10].Value = itemtoInsert.KDCKAEID;

                    var sqlUpdate = @"UPDATE  KPOPTD SET KDCFLAG = :aValue, KDCKARID = :KDCKARID, KDCMODELE = :KDCMODELE WHERE KDCIPB = :codeOffre AND KDCALX = :version
											AND KDCTYP = :type AND KDCFOR = :KDCFOR AND KDCOPT = :KDCOPT AND KDCTENG = :KDCTENG AND KDCKAKID = :KDCKAKID AND KDCKAEID = :KDCKAEID ";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate, paramUp);
                    #endregion
                }
            }
            return idVolet;
        }


        public static long PopulateKpgaran(string codeOffre, int version, string type, KpgaranDto itemtoInsert)
        {
            var idGar = GetCodeGarantieBySeq(itemtoInsert.KDESEQ.ToString(CultureInfo.InvariantCulture),
                                             codeOffre,
                                             version.ToString(CultureInfo.InvariantCulture),
                                             type,
                                             itemtoInsert.KDEFOR.ToString(CultureInfo.InvariantCulture),
                                             itemtoInsert.KDEOPT.ToString(CultureInfo.InvariantCulture), ModeConsultation.Standard);
            var exists = idGar != 0;
            var sql = string.Empty;

            if (!exists)
            {
                #region INSERT
                idGar = CommonRepository.GetAS400Id("KDEID");

                var paramInven = new EacParameter[5];
                paramInven[0] = new EacParameter("codeOffre", DbType.Int32);
                paramInven[0].Value = codeOffre.PadLeft(9, ' ');
                paramInven[1] = new EacParameter("type", DbType.Int64);
                paramInven[1].Value = type;
                paramInven[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
                paramInven[2].Value = version;
                paramInven[3] = new EacParameter("KDEFOR", DbType.AnsiStringFixedLength);
                paramInven[3].Value = itemtoInsert.KDEFOR;
                paramInven[4] = new EacParameter("KDEGARAN", DbType.Int32);
                paramInven[4].Value = itemtoInsert.KDEGARAN.PadLeft(10, ' ');

                var sqlInven = @"SELECT KBGKBEID ID FROM KPINVAPP WHERE KBGIPB = :codeOffre AND KBGTYP = :type AND KBGALX = :version AND KBGPERI = 'GA'
											AND KBGFOR = :KDEFOR AND KBGGAR = :KDEGARAN ";

                long idInven = 0;
                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlInven, paramInven).FirstOrDefault();

                if (result != null)
                {
                    idInven = result.Id;
                }

                var param = new EacParameter[63];
                param[0] = new EacParameter("P_KDEAJOUT", DbType.AnsiStringFixedLength);
                param[0].Value = itemtoInsert.KDEAJOUT;
                param[1] = new EacParameter("P_KDEALA", DbType.AnsiStringFixedLength);
                param[1].Value = itemtoInsert.KDEALA;
                param[2] = new EacParameter("P_KDEALIREF", DbType.AnsiStringFixedLength);
                param[2].Value = itemtoInsert.KDEALIREF;
                param[3] = new EacParameter("P_KDEALO", DbType.AnsiStringFixedLength);
                param[3].Value = itemtoInsert.KDEALO;
                param[4] = new EacParameter("P_KDEALX", DbType.Int32);
                param[4].Value = itemtoInsert.KDEALX;
                param[5] = new EacParameter("P_KDEASBASE", DbType.AnsiStringFixedLength);
                param[5].Value = itemtoInsert.KDEASBASE;
                param[6] = new EacParameter("P_KDEASMOD", DbType.AnsiStringFixedLength);
                param[6].Value = itemtoInsert.KDEASMOD;
                param[7] = new EacParameter("P_KDEASOBLI", DbType.AnsiStringFixedLength);
                param[7].Value = itemtoInsert.KDEASOBLI;
                param[8] = new EacParameter("P_KDEASUNIT", DbType.AnsiStringFixedLength);
                param[8].Value = itemtoInsert.KDEASUNIT;
                param[9] = new EacParameter("P_KDEASVALA", DbType.Double);
                param[9].Value = itemtoInsert.KDEASVALA;
                param[10] = new EacParameter("P_KDEASVALO", DbType.Double);
                param[10].Value = itemtoInsert.KDEASVALO;
                param[11] = new EacParameter("P_KDEASVALW", DbType.Double);
                param[11].Value = itemtoInsert.KDEASVALW;
                param[12] = new EacParameter("P_KDECAR", DbType.AnsiStringFixedLength);
                param[12].Value = itemtoInsert.KDECAR;
                param[13] = new EacParameter("P_KDECATNAT", DbType.AnsiStringFixedLength);
                param[13].Value = itemtoInsert.KDECATNAT;
                param[14] = new EacParameter("P_KDECRAVN", DbType.Int32);
                param[14].Value = itemtoInsert.KDECRAVN;
                param[15] = new EacParameter("P_KDECRD", DbType.Int64);
                param[15].Value = itemtoInsert.KDECRD;
                param[16] = new EacParameter("P_KDECRU", DbType.AnsiStringFixedLength);
                param[16].Value = itemtoInsert.KDECRU;
                param[17] = new EacParameter("P_KDEDATDEB", DbType.Int64);
                param[17].Value = itemtoInsert.KDEDATDEB;
                param[18] = new EacParameter("P_KDEDATFIN", DbType.Int64);
                param[18].Value = itemtoInsert.KDEDATFIN;
                param[19] = new EacParameter("P_KDEDEFG", DbType.AnsiStringFixedLength);
                param[19].Value = itemtoInsert.KDEDEFG;
                param[20] = new EacParameter("P_KDEDUREE", DbType.Int32);
                param[20].Value = itemtoInsert.KDEDUREE;
                param[21] = new EacParameter("P_KDEDURUNI", DbType.AnsiStringFixedLength);
                param[21].Value = itemtoInsert.KDEDURUNI;
                param[22] = new EacParameter("P_KDEFOR", DbType.Int32);
                param[22].Value = itemtoInsert.KDEFOR;
                param[23] = new EacParameter("P_KDEGAN", DbType.AnsiStringFixedLength);
                param[23].Value = itemtoInsert.KDEGAN;
                param[24] = new EacParameter("P_KDEGARAN", DbType.AnsiStringFixedLength);
                param[24].Value = itemtoInsert.KDEGARAN;
                param[25] = new EacParameter("P_KDEHEUDEB", DbType.Int64);
                param[25].Value = itemtoInsert.KDEHEUDEB;
                param[26] = new EacParameter("P_KDEHEUFIN", DbType.Int64);
                param[26].Value = itemtoInsert.KDEHEUFIN;
                param[27] = new EacParameter("P_KDEID", DbType.Int64);
                param[27].Value = idGar;
                param[28] = new EacParameter("P_KDEINA", DbType.AnsiStringFixedLength);
                param[28].Value = itemtoInsert.KDEINA;
                param[29] = new EacParameter("P_KDEINVEN", DbType.Int64);
                param[29].Value = idInven;
                param[30] = new EacParameter("P_KDEINVSP", DbType.AnsiStringFixedLength);
                param[30].Value = itemtoInsert.KDEINVSP;
                param[31] = new EacParameter("P_KDEIPB", DbType.AnsiStringFixedLength);
                param[31].Value = itemtoInsert.KDEIPB.PadLeft(9, ' ');
                param[32] = new EacParameter("P_KDEKDCID", DbType.Int64);
                param[32].Value = itemtoInsert.KDEKDCID;
                param[33] = new EacParameter("P_KDEKDFID", DbType.Int64);
                param[33].Value = itemtoInsert.KDEKDFID;
                param[34] = new EacParameter("P_KDEKDHID", DbType.Int64);
                param[34].Value = itemtoInsert.KDEKDHID;
                param[35] = new EacParameter("P_KDEMAJAVN", DbType.Int32);
                param[35].Value = itemtoInsert.KDEMAJAVN;
                param[36] = new EacParameter("P_KDEMODI", DbType.AnsiStringFixedLength);
                param[36].Value = itemtoInsert.KDEMODI;
                param[37] = new EacParameter("P_KDENAT", DbType.Double);
                param[37].Value = itemtoInsert.KDENAT;
                param[38] = new EacParameter("P_KDENIVEAU", DbType.Double);
                param[38].Value = itemtoInsert.KDENIVEAU;
                param[39] = new EacParameter("P_KDENUMPRES", DbType.Double);
                param[39].Value = itemtoInsert.KDENUMPRES;
                param[40] = new EacParameter("P_KDEOPT", DbType.Int32);
                param[40].Value = itemtoInsert.KDEOPT;
                param[41] = new EacParameter("P_KDEPALA", DbType.AnsiStringFixedLength);
                param[41].Value = itemtoInsert.KDEPALA;
                param[42] = new EacParameter("P_KDEPCATN", DbType.AnsiStringFixedLength);
                param[42].Value = itemtoInsert.KDEPCATN;
                param[43] = new EacParameter("P_KDEPEMI", DbType.AnsiStringFixedLength);
                param[43].Value = itemtoInsert.KDEPEMI;
                param[44] = new EacParameter("P_KDEPIND", DbType.AnsiStringFixedLength);
                param[44].Value = itemtoInsert.KDEPIND;
                param[45] = new EacParameter("P_KDEPNTM", DbType.AnsiStringFixedLength);
                param[45].Value = itemtoInsert.KDEPNTM;
                param[46] = new EacParameter("P_KDEPPRP", DbType.AnsiStringFixedLength);
                param[46].Value = itemtoInsert.KDEPPRP;
                param[47] = new EacParameter("P_KDEPREF", DbType.AnsiStringFixedLength);
                param[47].Value = itemtoInsert.KDEPREF;
                param[48] = new EacParameter("P_KDEPRP", DbType.AnsiStringFixedLength);
                param[48].Value = itemtoInsert.KDEPRP;
                param[49] = new EacParameter("P_KDEPTAXC", DbType.AnsiStringFixedLength);
                param[49].Value = itemtoInsert.KDEPTAXC;
                param[50] = new EacParameter("P_KDESEM", DbType.Int64);
                param[50].Value = itemtoInsert.KDESEM;
                param[51] = new EacParameter("P_KDESEQ", DbType.Int64);
                param[51].Value = itemtoInsert.KDESEQ;
                param[52] = new EacParameter("P_KDESE1", DbType.Int64);
                param[52].Value = itemtoInsert.KDESE1;
                param[53] = new EacParameter("P_KDETAXCOD", DbType.AnsiStringFixedLength);
                param[53].Value = itemtoInsert.KDETAXCOD;
                param[54] = new EacParameter("P_KDETAXREP", DbType.Double);
                param[54].Value = itemtoInsert.KDETAXREP;
                param[55] = new EacParameter("P_KDETCD", DbType.AnsiStringFixedLength);
                param[55].Value = itemtoInsert.KDETCD;
                param[56] = new EacParameter("P_KDETRI", DbType.AnsiStringFixedLength);
                param[56].Value = itemtoInsert.KDETRI;
                param[57] = new EacParameter("P_KDETYP", DbType.AnsiStringFixedLength);
                param[57].Value = itemtoInsert.KDETYP;
                param[58] = new EacParameter("P_KDETYPEMI", DbType.AnsiStringFixedLength);
                param[58].Value = itemtoInsert.KDETYPEMI;
                param[59] = new EacParameter("P_KDEWDDEB", DbType.Int64);
                param[59].Value = itemtoInsert.KDEWDDEB;
                param[60] = new EacParameter("P_KDEWDFIN", DbType.Int64);
                param[60].Value = itemtoInsert.KDEWDFIN;
                param[61] = new EacParameter("P_KDEWHDEB", DbType.Int64);
                param[61].Value = itemtoInsert.KDEWHDEB;
                param[62] = new EacParameter("P_KDEWHFIN", DbType.Int64);
                param[62].Value = itemtoInsert.KDEWHFIN;

                sql = string.Format(@"INSERT INTO KPGARAN 
										(   KDEAJOUT,	
											KDEALA,	
											KDEALIREF,	
											KDEALO,	
											KDEALX,	
											KDEASBASE,	
											KDEASMOD,	
											KDEASOBLI,	
											KDEASUNIT,	
											KDEASVALA,	
											KDEASVALO,	
											KDEASVALW,	
											KDECAR,	
											KDECATNAT,	
											KDECRAVN,	
											KDECRD,	
											KDECRU,	
											KDEDATDEB,	
											KDEDATFIN,	
											KDEDEFG,	
											KDEDUREE,	
											KDEDURUNI,	
											KDEFOR,	
											KDEGAN,	
											KDEGARAN,	
											KDEHEUDEB,	
											KDEHEUFIN,	
											KDEID,	
											KDEINA,	
											KDEINVEN,	
											KDEINVSP,	
											KDEIPB,	
											KDEKDCID,	
											KDEKDFID,	
											KDEKDHID,	
											KDEMAJAVN,	
											KDEMODI,	
											KDENAT,	
											KDENIVEAU,	
											KDENUMPRES,	
											KDEOPT,	
											KDEPALA,	
											KDEPCATN,	
											KDEPEMI,	
											KDEPIND,	
											KDEPNTM,	
											KDEPPRP,	
											KDEPREF,	
											KDEPRP,	
											KDEPTAXC,	
											KDESEM,	
											KDESEQ,	
											KDESE1,	
											KDETAXCOD,	
											KDETAXREP,	
											KDETCD,	
											KDETRI,	
											KDETYP,	
											KDETYPEMI,	
											KDEWDDEB,	
											KDEWDFIN,	
											KDEWHDEB,	
											KDEWHFIN )		
										VALUES 
											( :P_KDEAJOUT,	
											:P_KDEALA,	
											:P_KDEALIREF,	
											:P_KDEALO,	
											:P_KDEALX,	
											:P_KDEASBASE,	
											:P_KDEASMOD,	
											:P_KDEASOBLI,	
											:P_KDEASUNIT,	
											:P_KDEASVALA,	
											:P_KDEASVALO,	
											:P_KDEASVALW,	
											:P_KDECAR,	
											:P_KDECATNAT,	
											:P_KDECRAVN,	
											:P_KDECRD,	
											:P_KDECRU,	
											:P_KDEDATDEB,	
											:P_KDEDATFIN,	
											:P_KDEDEFG,	
											:P_KDEDUREE,	
											:P_KDEDURUNI,	
											:P_KDEFOR,	
											:P_KDEGAN,	
											:P_KDEGARAN,	
											:P_KDEHEUDEB,	
											:P_KDEHEUFIN,	
											:P_KDEID,	
											:P_KDEINA,	
											:P_KDEINVEN,	
											:P_KDEINVSP,	
											:P_KDEIPB,	
											:P_KDEKDCID,	
											:P_KDEKDFID,	
											:P_KDEKDHID,	
											:P_KDEMAJAVN,	
											:P_KDEMODI,	
											:P_KDENAT,	
											:P_KDENIVEAU,	
											:P_KDENUMPRES,	
											:P_KDEOPT,	
											:P_KDEPALA,	
											:P_KDEPCATN,	
											:P_KDEPEMI,	
											:P_KDEPIND,	
											:P_KDEPNTM,	
											:P_KDEPPRP,	
											:P_KDEPREF,	
											:P_KDEPRP,	
											:P_KDEPTAXC,	
											:P_KDESEM,	
											:P_KDESEQ,	
											:P_KDESE1,	
											:P_KDETAXCOD,	
											:P_KDETAXREP,	
											:P_KDETCD,	
											:P_KDETRI,	
											:P_KDETYP,	
											:P_KDETYPEMI,	
											:P_KDEWDDEB,	
											:P_KDEWDFIN,	
											:P_KDEWHDEB,	
											:P_KDEWHFIN )");

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                #endregion
            }
            else if (itemtoInsert.KDECAR == "F" && (itemtoInsert.KDEGAN == string.Empty || itemtoInsert.KDEGAN == "E"))
            {
                CommonRepository.SetTraceLog(codeOffre, version.ToString(), type, 0, "delete garan", "PopulateGARAN", AlbConvert.ConvertDateToStr(DateTime.Now), "TRACEDEL");
                sql = string.Format(@"DELETE FROM KPGARAN WHERE KDEIPB = '{0}' AND KDEALX = {1} AND KDETYP = '{2}' AND KDEFOR = {3} AND KDEOPT = {4} AND KDESEQ = {5}"
                    , codeOffre.PadLeft(9, ' '), version, type, itemtoInsert.KDEFOR, itemtoInsert.KDEOPT, itemtoInsert.KDESEQ);

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
                sql = string.Format(@"DELETE FROM KPGARTAR WHERE KDGKDEID = {0}", idGar);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);

                idGar = 0;
            }
            else
            {
                sql = string.Format(@"UPDATE  KPGARAN SET KDEGAN = '{0}' WHERE KDEIPB='{1}' AND KDEALX ={2} AND KDETYP ='{3}' AND KDEFOR ={4} AND KDEOPT= {5} AND KDESEQ = {6} ",
                    itemtoInsert.KDEGAN, itemtoInsert.KDEIPB.Trim().PadLeft(9, ' '), itemtoInsert.KDEALX, itemtoInsert.KDETYP, itemtoInsert.KDEFOR, itemtoInsert.KDEOPT, itemtoInsert.KDESEQ);
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql);
            }

            return idGar;
        }

        public static void PopulateKpgartar(string codeOffre, int version, string type, long idGar, KpgartarDto itemtoInsert)
        {
            if (idGar == 0)
            {
                return;
            }

            var param2 = new EacParameter[1];
            param2[0] = new EacParameter("idGar", DbType.Int64);
            param2[0].Value = idGar;

            string sql = @"SELECT COUNT(*) FROM KPGARTAR WHERE KDGKDEID = :idGar";

            var exists = (int)DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param2) != 0;

            if (!exists)
            {
                #region INSERT
                var idGarTar = CommonRepository.GetAS400Id("KDGID");
                var param = new EacParameter[49];
                param[0] = new EacParameter("P_KDGALX", DbType.Int32);
                param[0].Value = itemtoInsert.KDGALX;
                param[1] = new EacParameter("P_KDGCHT", DbType.Double);
                param[1].Value = itemtoInsert.KDGCHT;
                param[2] = new EacParameter("P_KDGCMC", DbType.Double);
                param[2].Value = itemtoInsert.KDGCMC;
                param[3] = new EacParameter("P_KDGCTT", DbType.Double);
                param[3].Value = itemtoInsert.KDGCTT;
                param[4] = new EacParameter("P_KDGFMABASE", DbType.AnsiStringFixedLength);
                param[4].Value = itemtoInsert.KDGFMABASE;
                param[5] = new EacParameter("P_KDGFMAUNIT", DbType.AnsiStringFixedLength);
                param[5].Value = itemtoInsert.KDGFMAUNIT;
                param[6] = new EacParameter("P_KDGFMAVALA", DbType.Double);
                param[6].Value = itemtoInsert.KDGFMAVALA;
                param[7] = new EacParameter("P_KDGFMAVALO", DbType.Double);
                param[7].Value = itemtoInsert.KDGFMAVALO;
                param[8] = new EacParameter("P_KDGFMAVALW", DbType.Double);
                param[8].Value = itemtoInsert.KDGFMAVALW;
                param[9] = new EacParameter("P_KDGFMIBASE", DbType.AnsiStringFixedLength);
                param[9].Value = itemtoInsert.KDGFMIBASE;
                param[10] = new EacParameter("P_KDGFMIUNIT", DbType.AnsiStringFixedLength);
                param[10].Value = itemtoInsert.KDGFMIUNIT;
                param[11] = new EacParameter("P_KDGFMIVALA", DbType.Double);
                param[11].Value = itemtoInsert.KDGFMIVALA;
                param[12] = new EacParameter("P_KDGFMIVALO", DbType.Double);
                param[12].Value = itemtoInsert.KDGFMIVALO;
                param[13] = new EacParameter("P_KDGFMIVALW", DbType.Double);
                param[13].Value = itemtoInsert.KDGFMIVALW;
                param[14] = new EacParameter("P_KDGFOR", DbType.Int32);
                param[14].Value = itemtoInsert.KDGFOR;
                param[15] = new EacParameter("P_KDGFRHBASE", DbType.AnsiStringFixedLength);
                param[15].Value = itemtoInsert.KDGFRHBASE;
                param[16] = new EacParameter("P_KDGFRHMOD", DbType.AnsiStringFixedLength);
                param[16].Value = itemtoInsert.KDGFRHMOD;
                param[17] = new EacParameter("P_KDGFRHOBL", DbType.AnsiStringFixedLength);
                param[17].Value = itemtoInsert.KDGFRHOBL;
                param[18] = new EacParameter("P_KDGFRHUNIT", DbType.AnsiStringFixedLength);
                param[18].Value = itemtoInsert.KDGFRHUNIT;
                param[19] = new EacParameter("P_KDGFRHVALA", DbType.Double);
                param[19].Value = itemtoInsert.KDGFRHVALA;
                param[20] = new EacParameter("P_KDGFRHVALO", DbType.Double);
                param[20].Value = itemtoInsert.KDGFRHVALO;
                param[21] = new EacParameter("P_KDGFRHVALW", DbType.Double);
                param[21].Value = itemtoInsert.KDGFRHVALW;
                param[22] = new EacParameter("P_KDGGARAN", DbType.AnsiStringFixedLength);
                param[22].Value = itemtoInsert.KDGGARAN;
                param[23] = new EacParameter("P_KDGID", DbType.Int32);
                param[23].Value = idGarTar;
                param[24] = new EacParameter("P_KDGIPB", DbType.AnsiStringFixedLength);
                param[24].Value = itemtoInsert.KDGIPB.PadLeft(9, ' ');
                param[25] = new EacParameter("P_KDGKDEID", DbType.Int64);
                param[25].Value = idGar;
                param[26] = new EacParameter("P_KDGKDIID", DbType.Int64);
                param[26].Value = itemtoInsert.KDGKDIID;
                param[27] = new EacParameter("P_KDGKDKID", DbType.Int64);
                param[27].Value = itemtoInsert.KDGKDKID;
                param[28] = new EacParameter("P_KDGLCIBASE", DbType.AnsiStringFixedLength);
                param[28].Value = itemtoInsert.KDGLCIBASE;
                param[29] = new EacParameter("P_KDGLCIMOD", DbType.AnsiStringFixedLength);
                param[29].Value = itemtoInsert.KDGLCIMOD;
                param[30] = new EacParameter("P_KDGLCIOBL", DbType.AnsiStringFixedLength);
                param[30].Value = itemtoInsert.KDGLCIOBL;
                param[31] = new EacParameter("P_KDGLCIUNIT", DbType.AnsiStringFixedLength);
                param[31].Value = itemtoInsert.KDGLCIUNIT;
                param[32] = new EacParameter("P_KDGLCIVALA", DbType.Double);
                param[32].Value = itemtoInsert.KDGLCIVALA;
                param[33] = new EacParameter("P_KDGLCIVALO", DbType.Double);
                param[33].Value = itemtoInsert.KDGLCIVALO;
                param[34] = new EacParameter("P_KDGLCIVALW", DbType.Double);
                param[34].Value = itemtoInsert.KDGLCIVALW;
                param[35] = new EacParameter("P_KDGMNTBASE", DbType.Double);
                param[35].Value = itemtoInsert.KDGMNTBASE;
                param[36] = new EacParameter("P_KDGNUMTAR", DbType.Int32);
                param[36].Value = itemtoInsert.KDGNUMTAR;
                param[37] = new EacParameter("P_KDGOPT", DbType.Int32);
                param[37].Value = itemtoInsert.KDGOPT;
                param[38] = new EacParameter("P_KDGPRIBASE", DbType.AnsiStringFixedLength);
                param[38].Value = itemtoInsert.KDGPRIBASE;
                param[39] = new EacParameter("P_KDGPRIMOD", DbType.AnsiStringFixedLength);
                param[39].Value = itemtoInsert.KDGPRIMOD;
                param[40] = new EacParameter("P_KDGPRIMPRO", DbType.Double);
                param[40].Value = itemtoInsert.KDGPRIMPRO;
                param[41] = new EacParameter("P_KDGPRIOBL", DbType.AnsiStringFixedLength);
                param[41].Value = itemtoInsert.KDGPRIOBL;
                param[42] = new EacParameter("P_KDGPRIUNIT", DbType.AnsiStringFixedLength);
                param[42].Value = itemtoInsert.KDGPRIUNIT;
                param[43] = new EacParameter("P_KDGPRIVALA", DbType.Double);
                param[43].Value = itemtoInsert.KDGPRIVALA;
                param[44] = new EacParameter("P_KDGPRIVALO", DbType.Double);
                param[44].Value = itemtoInsert.KDGPRIVALO;
                param[45] = new EacParameter("P_KDGPRIVALW", DbType.Double);
                param[45].Value = itemtoInsert.KDGPRIVALW;
                param[46] = new EacParameter("P_KDGTFF", DbType.Double);
                param[46].Value = itemtoInsert.KDGTFF;
                param[47] = new EacParameter("P_KDGTMC", DbType.Double);
                param[47].Value = itemtoInsert.KDGTMC;
                param[48] = new EacParameter("P_KDGTYP", DbType.AnsiStringFixedLength);
                param[48].Value = itemtoInsert.KDGTYP;
                sql = string.Format(@"INSERT INTO  KPGARTAR 
										(   KDGALX,	
											KDGCHT,	
											KDGCMC,	
											KDGCTT,	
											KDGFMABASE,	
											KDGFMAUNIT,	
											KDGFMAVALA,	
											KDGFMAVALO,	
											KDGFMAVALW,	
											KDGFMIBASE,	
											KDGFMIUNIT,	
											KDGFMIVALA,	
											KDGFMIVALO,	
											KDGFMIVALW,	
											KDGFOR,	
											KDGFRHBASE,	
											KDGFRHMOD,	
											KDGFRHOBL,	
											KDGFRHUNIT,	
											KDGFRHVALA,	
											KDGFRHVALO,	
											KDGFRHVALW,	
											KDGGARAN,	
											KDGID,	
											KDGIPB,	
											KDGKDEID,	
											KDGKDIID,	
											KDGKDKID,	
											KDGLCIBASE,	
											KDGLCIMOD,	
											KDGLCIOBL,	
											KDGLCIUNIT,	
											KDGLCIVALA,	
											KDGLCIVALO,	
											KDGLCIVALW,	
											KDGMNTBASE,	
											KDGNUMTAR,	
											KDGOPT,	
											KDGPRIBASE,	
											KDGPRIMOD,	
											KDGPRIMPRO,	
											KDGPRIOBL,	
											KDGPRIUNIT,	
											KDGPRIVALA,	
											KDGPRIVALO,	
											KDGPRIVALW,	
											KDGTFF,	
											KDGTMC,	
											KDGTYP )
										VALUES 		
											( :P_KDGALX,	
											:P_KDGCHT,	
											:P_KDGCMC,	
											:P_KDGCTT,	
											:P_KDGFMABASE,	
											:P_KDGFMAUNIT,	
											:P_KDGFMAVALA,	
											:P_KDGFMAVALO,	
											:P_KDGFMAVALW,	
											:P_KDGFMIBASE,	
											:P_KDGFMIUNIT,	
											:P_KDGFMIVALA,	
											:P_KDGFMIVALO,	
											:P_KDGFMIVALW,	
											:P_KDGFOR,	
											:P_KDGFRHBASE,	
											:P_KDGFRHMOD,	
											:P_KDGFRHOBL,	
											:P_KDGFRHUNIT,	
											:P_KDGFRHVALA,	
											:P_KDGFRHVALO,	
											:P_KDGFRHVALW,	
											:P_KDGGARAN,	
											:P_KDGID,	
											:P_KDGIPB,	
											:P_KDGKDEID,	
											:P_KDGKDIID,	
											:P_KDGKDKID,	
											:P_KDGLCIBASE,	
											:P_KDGLCIMOD,	
											:P_KDGLCIOBL,	
											:P_KDGLCIUNIT,	
											:P_KDGLCIVALA,	
											:P_KDGLCIVALO,	
											:P_KDGLCIVALW,	
											:P_KDGMNTBASE,	
											:P_KDGNUMTAR,	
											:P_KDGOPT,	
											:P_KDGPRIBASE,	
											:P_KDGPRIMOD,	
											:P_KDGPRIMPRO,	
											:P_KDGPRIOBL,	
											:P_KDGPRIUNIT,	
											:P_KDGPRIVALA,	
											:P_KDGPRIVALO,	
											:P_KDGPRIVALW,	
											:P_KDGTFF,	
											:P_KDGTMC,	
											:P_KDGTYP )");

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                #endregion
            }
        }

        public static void PopulateKpexp(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeFormule);
            param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeOption);


            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_INITEXPRCOMPLEX", param);
        }

        public static long GetCodeGarantieBySeq(string seq, string codeOffre, string version, string type, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            long idGar = 0;
            var param = new EacParameter[6];
            param[0] = new EacParameter("seq", DbType.AnsiStringFixedLength);
            param[0].Value = seq;
            param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[2].Value = version;
            param[3] = new EacParameter("type", DbType.Int32);
            param[3].Value = type;
            param[4] = new EacParameter("codeFormule", DbType.Int32);
            param[4].Value = codeFormule;
            param[5] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[5].Value = codeOption;

            var sql = string.Format(@"SELECT KDEID INT64RETURNCOL FROM {0} WHERE KDESEQ = :seq  AND KDEIPB = :codeOffre
									  AND  KDEALX = :version AND KDETYP = :type AND KDEFOR = :codeFormule AND KDEOPT = :codeOption", CommonRepository.GetPrefixeHisto(modeNavig, "KPGARAN"));

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();
            if (result != null)
            {
                idGar = result.Int64ReturnCol.Value;
            }

            return idGar;
        }

        private static GaranLightDto UpdateKPGARAP(long idGar, KpgaranDto garantieNiv, IList<GaranLightDto> oldGaranInfos)
        {
            if ((garantieNiv == null) || (oldGaranInfos == null))
            {
                return null;
            }

            var oldGarantie = oldGaranInfos.Where(g => g.Type == garantieNiv.KDETYP &&
                                            g.CodeAffaire == garantieNiv.KDEIPB &&
                                            g.Aliment == garantieNiv.KDEALX &&
                                            g.CodeFormule == garantieNiv.KDEFOR &&
                                            g.Option == garantieNiv.KDEOPT &&
                                            g.Sequence == garantieNiv.KDESEQ).FirstOrDefault();
            if (oldGarantie == null)
            {
                return null;
            }

            var oldIdGar = oldGarantie.Id;

            var paramUpdateGarap = new EacParameter[2];
            paramUpdateGarap[0] = new EacParameter("IdGar", DbType.Int32) { Value = idGar };
            paramUpdateGarap[1] = new EacParameter("OldIdGar", DbType.Int32) { Value = oldIdGar };

            var sqlUpdateGarap = @"UPDATE KPGARAP SET KDFKDEID = :IdGar WHERE KDFKDEID = :OldIdGar";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateGarap, paramUpdateGarap);

            //Modif de l'alimentation de KPGaran
            var paramUpdateGaran = new EacParameter[2];
            paramUpdateGaran[0] = new EacParameter("KDEALA", DbType.Int32) { Value = oldGarantie.Alimentation };
            paramUpdateGaran[1] = new EacParameter("KDEID", DbType.Int32) { Value = idGar };

            var sqlUpdateGaran = @"UPDATE KPGARAN SET KDEALA = :KDEALA WHERE KDEID = :KDEID";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdateGaran, paramUpdateGaran);

            return oldGarantie;
        }

        /// <summary>
        /// Sauvegarde la formule de garantie
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <param name="libelle"></param>
        /// <param name="formuleGarantie"></param>
        public static string FormulesGarantiesSet(string codeOffre, int version, string type, string codeAvenant, string dateAvenant, int codeFormule, int codeOption, int formGen, string libelle, FormuleGarantieSaveDto formuleGarantie, string user)
        {
            foreach (var volet in formuleGarantie.Volets)
            {
                if (volet.MAJ)
                {
                    UpdateOptionFormuleGarantie(volet.GuidId.ToString(), volet.isChecked, "V");
                }

                foreach (var bloc in volet.Blocs)
                {
                    if (bloc.MAJ)
                    {
                        UpdateOptionFormuleGarantie(bloc.GuidId.ToString(), bloc.isChecked, "B");
                    }

                    foreach (var modele in bloc.Modeles)
                    {
                        foreach (var niv1 in modele.Modeles)
                        {
                            RecursiveUpdateGarantie(codeAvenant, niv1);
                        }

                    }
                }
            }



            DateTime dateNow = DateTime.Now;

            EacParameter[] param = new EacParameter[12];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.UInt32);
            param[1].Value = version;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEAVENANT", DbType.UInt32);
            param[3].Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) : 0;
            param[4] = new EacParameter("P_CODEFORMULE", DbType.AnsiStringFixedLength);
            param[4].Value = codeFormule;
            param[5] = new EacParameter("P_CODEOPTION", DbType.AnsiStringFixedLength);
            param[5].Value = codeOption;
            param[6] = new EacParameter("P_LIBELLE", DbType.AnsiStringFixedLength);
            param[6].Value = libelle;
            param[7] = new EacParameter("P_DATEAVT", DbType.UInt32);
            param[7].Value = !string.IsNullOrEmpty(dateAvenant) ? Convert.ToInt32(dateAvenant) : 0;
            param[8] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[8].Value = user;
            param[9] = new EacParameter("P_DATENOW", DbType.UInt32);
            param[9].Value = AlbConvert.ConvertDateToInt(dateNow);
            param[10] = new EacParameter("P_HEURENOW", DbType.UInt32);
            param[10].Value = AlbConvert.ConvertTimeToIntMinute(AlbConvert.GetTimeFromDate(dateNow));
            param[11] = new EacParameter("P_ERROR", DbType.AnsiStringFixedLength);
            param[11].Direction = ParameterDirection.Output;
            param[11].DbType = DbType.AnsiString;
            param[11].Size = 5000;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SVOPTD", param);

            return param[11].Value.ToString();
        }

        public static List<RisqueObjetPlatDto> GetRisqueObjetFormule(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            string toReturn = string.Empty;
            string codesObj = string.Empty;

            var param = new List<EacParameter> {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version",  Convert.ToInt32(version)),
                new EacParameter("type", type),
                new EacParameter("codeFormule", Convert.ToInt32(codeFormule)),
                new EacParameter("codeOption", Convert.ToInt32(codeOption))
            };

            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string optapTablename = CommonRepository.GetPrefixeHisto(modeNavig, "KPOPTAP");
            string rsqTablename = CommonRepository.GetPrefixeHisto(modeNavig, "KPRSQ");
            string avnCondition = modeNavig == ModeConsultation.Historique ? "AND KDDAVN = :avn" : string.Empty;

            string sql = $@"
SELECT DISTINCT KDDRSQ CODERSQ, KDDOBJ CODEOBJ , KAHID CODECIBLE, KABCIBLE CIBLE, KAHDESC DESCCIBLE
FROM {optapTablename}
LEFT JOIN {rsqTablename} ON KDDALX = KABALX
AND KDDIPB = KABIPB
AND KDDTYP = KABTYP
LEFT JOIN KCIBLE ON KABCIBLE = KAHCIBLE
WHERE KDDIPB = :codeOffre
AND KDDALX = :version 
AND KDDTYP = :type 
AND KDDFOR = :codeFormule
AND KDDOPT = :codeOption
{avnCondition}
ORDER BY KDDRSQ, KDDOBJ, KAHID, KABCIBLE, KAHDESC";

            return DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.Text, sql, param);
        }

        public static List<RisqueObjetPlatDto> GetEarliestRisqueObjetFormule(string codeOffre, string version, string type)
        {
            var param = new List<EacParameter> {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version",  Convert.ToInt32(version)),
                new EacParameter("type", type)
            };

            const string Selection = @"
SELECT KABRSQ CODERSQ, KAHID CODECIBLE, KABCIBLE CIBLE, KAHDESC DESCCIBLE
FROM KPRSQ
INNER JOIN KCIBLE ON KABCIBLE = KAHCIBLE
AND KABIPB = :codeOffre
AND KABALX = :version
AND KABTYP = :type
ORDER BY KABID, KAHID, KABCIBLE, KAHDESC
FETCH FIRST 1 ROWS ONLY";

            return DbBase.Settings.ExecuteList<RisqueObjetPlatDto>(CommandType.Text, Selection, param);
        }

        private static void RecursiveUpdateGarantie(string codeAvenant, ModeleNiveauSave_Base niveau)
        {
            if (niveau.MAJ)
            {
                UpdateGarantieFormuleGarantie(codeAvenant, niveau.GuidGarantie, niveau.NatureParam);
            }

            if (niveau.Modeles != null && niveau.Modeles.Any())
            {
                foreach (var sousNiveau in niveau.Modeles)
                {
                    RecursiveUpdateGarantie(codeAvenant, sousNiveau);
                }
            }
        }

        public static bool FormuleGarantiesExists(string codeOffre, string version, string type)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.UInt32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            var sql = @"SELECT COUNT(*) FROM KPGARAN WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type";
            return (int)DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param) > 0;
        }

        public static void SaveAppliqueA(string codeOffre, string version, string type, string codeFormule, string codeOption, string cible, string formGen, string objetRisqueCode, string user)
        {
            int countSelObjet = formGen == "0" || string.IsNullOrEmpty(formGen) ? objetRisqueCode.Split(';')[1].Split('_').Length : 0;

            var param = new EacParameter[11];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeFormule);
            param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeOption);
            param[5] = new EacParameter("P_CIBLE", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(cible) ? Convert.ToInt32(cible) : 0;
            param[6] = new EacParameter("P_FORMGEN", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(formGen) ? Convert.ToInt32(formGen) : 0;
            param[7] = new EacParameter("P_OBJETRISQUE", DbType.AnsiStringFixedLength);
            param[7].Value = objetRisqueCode;
            param[8] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[8].Value = user;
            param[9] = new EacParameter("P_DATENOW", DbType.Int32);
            param[9].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
            param[10] = new EacParameter("P_COUNTSELOBJ", DbType.Int32);
            param[10].Value = countSelObjet;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SVAPPLI", param);
        }

        public static void DeleteFormuleGarantieHistorique(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[5];
            string sql = @"DELETE FROM KPGARAH WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type
								AND KDEFOR = :codeFormule AND KDEOPT = :codeOption";
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeFormule);
            param[4] = new EacParameter("codeOption", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeOption);

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static FormuleDto InitFormuleGarantie(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption, string formGen, ModeConsultation modeNavig, bool readOnly, string user)
        {
            var toReturn = new FormuleDto();
            int.TryParse(formGen, out int valFormGen);

            if (valFormGen != 0)
            {
                return InitializeFormuleGarantie(codeOffre, version, type, avenant, codeFormule, modeNavig, toReturn);
            }

            if (codeFormule != "0")
            {
                toReturn = GetFormuleGarantieInfo(codeOffre, version, type, avenant, codeFormule, modeNavig) ?? new FormuleDto();
            }
            else
            {
                CheckFormule(codeOffre, version, type, codeFormule);
                toReturn.LettreLib = GetLettreLibFormuleGarantie(codeOffre, version, type);
            }

            toReturn.ObjetRisqueCode = GetRisqueObjetFormuleString(codeOffre, version, type, avenant, codeFormule, codeOption, modeNavig);

            if (!avenant.IsEmptyOrNull() && modeNavig == ModeConsultation.Historique)
            {
                toReturn.Risques = RisqueRepository.ObtenirRisquesAvenant(codeOffre, version, type, avenant, modeNavig);
            }
            else
            {
                toReturn.Risques = RisqueRepository.ObtenirRisquesSP(codeOffre, version, type, avenant, codeFormule, codeOption, modeNavig);
            }

            toReturn.OffreAppliqueA = IsOffreAppliqueA(
                new IdContratDto { CodeOffre = codeOffre, Version = int.Parse(version), Type = type },
                int.Parse(avenant.OrDefault("0")),
                modeNavig == ModeConsultation.Historique);

            //var param = IntializeEachParameterFormuleGarantie(codeOffre, version, type, avenant, codeFormule, codeOption);
            var param = IntializeEachParameterFormuleGarantie(codeOffre, version, type, avenant, codeFormule, codeOption);

            #region Partie avenant trace
            SetIsTraceAvenantExist(modeNavig, toReturn, param);
            SetDateEffetAvenantModif(modeNavig, toReturn, param);
            #endregion

            //Partie refacto loading Formule de Garantie
            if ((codeFormule != "0" && !codeFormule.IsEmptyOrNull()) || toReturn.LettreLib == "A")
            {
                LoadFormuleGarantie(codeOffre, version, type, avenant, codeFormule, codeOption, modeNavig, readOnly, user, toReturn, valFormGen, param);
            }
            //Fin refacto loading Formule de Garantie

            SetIsSorti(modeNavig, toReturn, param);

            return toReturn;
        }

        public static bool IsOffreAppliqueA(IdContratDto contrat, int avenant, bool isHisto)
        {
            string tablename = "KPOPTAP";
            if (isHisto)
            {
                tablename = CommonRepository.GetHistoTableName(tablename);
            }

            string sql = $"SELECT COUNT(*) NBLIGN FROM {tablename} WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type";

            var parameters = new List<EacParameter>()
            {
                new EacParameter("codeOffre", DbType.AnsiStringFixedLength) { Value = contrat.CodeOffre.PadLeft(9, ' ') },
                new EacParameter("version", DbType.Int32) { Value = contrat.Version },
                new EacParameter("type", DbType.AnsiStringFixedLength) { Value = contrat.Type }
            };

            if (isHisto)
            {
                parameters.Add(new EacParameter("avenant", DbType.Int32) { Value = avenant });
            }

            return CommonRepository.ExistRowParam(string.Format(sql + "{0}", (isHisto ? " AND KDDAVN = :avenant" : string.Empty)), parameters);
        }

        private static void LoadFormuleGarantie(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption, ModeConsultation modeNavig, bool readOnly, string user, FormuleDto toReturn, int valFormGen, List<EacParameter> param)
        {
            if (string.IsNullOrEmpty(toReturn.ObjetRisqueCode))
            {
                toReturn.ObjetRisqueCode = GetFirstRsqObj(codeOffre, version, modeNavig);
            }

            if (toReturn.ObjetRisqueCode.Split(';').Length <= 1)
            {
                toReturn.ObjetRisqueCode = GetAllRsqObj(codeOffre, version, toReturn.ObjetRisqueCode, modeNavig);
            }

            string branche = string.Empty;
            var paramSqlBrancheCible = new List<EacParameter>();
            //Put CodeOffre and Version
            paramSqlBrancheCible.AddRange(param.Take(2));

            var paramCodeRisqueObjet = new EacParameter("objetRisqueCode", DbType.AnsiStringFixedLength);
            paramCodeRisqueObjet.Value = toReturn.ObjetRisqueCode.Split(';')[0];
            paramSqlBrancheCible.Add(paramCodeRisqueObjet);

            string sqlBranche = @"SELECT JEBRA STRRETURNCOL FROM YPRTRSQ WHERE JEIPB = :codeOffre AND JEALX = :version AND JERSQ = :objetRisqueCode";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlBranche, paramSqlBrancheCible);
            if (result != null && result.Any())
            {
                branche = result.FirstOrDefault().StrReturnCol;
            }

            if (toReturn.CodeCible == 0)
            {
                var paramType = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramType.Value = type;
                paramSqlBrancheCible.Add(paramType);

                string sqlCible = "SELECT KAIID INT64RETURNCOL FROM KCIBLEF INNER JOIN KPRSQ ON KABCIBLE = KAICIBLE WHERE KABIPB = :codeOffre AND KABALX = :version AND KABRSQ = :objetRisqueCode AND KABTYP = :type";
                var resultCible = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlCible, paramSqlBrancheCible);
                if (resultCible != null && resultCible.Any())
                {
                    toReturn.CodeCible = resultCible.FirstOrDefault().Int64ReturnCol.Value;
                }
            }

            var libCible = GetLibCible(toReturn.CodeCible.ToString());
            if (!string.IsNullOrEmpty(libCible))
            {
                toReturn.Cible = libCible.Split('-')[0].Trim();
                toReturn.DescCible = libCible.Split('-')[1].Trim();
            }

            if (!readOnly && (!string.IsNullOrEmpty(toReturn.ObjetRisqueCode) || valFormGen == 1))
            {
                codeOption = (codeOption == "0" ? "1" : codeOption);
                string newCode = EnregistrerFormuleGarantie(codeOffre, version, type, avenant, codeFormule, codeOption, valFormGen.ToString(), toReturn.LettreLib, branche, toReturn.CodeCible.ToString(), toReturn.Libelle, toReturn.ObjetRisqueCode, user);
                if (!string.IsNullOrEmpty(newCode))
                {
                    toReturn.Code = Convert.ToInt32(newCode.Split('_')[0]);
                    toReturn.LettreLib = newCode.Split('_')[1];
                    codeFormule = toReturn.Code.ToString();
                }
            }

            toReturn.Formule = FormulesGarantiesGet(codeOffre, Convert.ToInt32(version), type, avenant, Convert.ToInt32(codeFormule), Convert.ToInt32(codeOption), valFormGen, Convert.ToInt32(toReturn.CodeCible), toReturn.LettreLib, branche, toReturn.Libelle, user, 1, readOnly, modeNavig);
        }


        #region Refaction Intialisation Formule de Gaarantie

        public static List<EacParameter> IntializeEachParameterFormuleGarantie(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption)
        {
            var parameters = new List<EacParameter>();

            var paramCodeOffre = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramCodeOffre.Value = codeOffre.PadLeft(9, ' ');
            var paramVersion = new EacParameter("version", DbType.AnsiStringFixedLength);
            paramVersion.Value = version;
            var paramType = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramType.Value = type;
            var paramCodeFormule = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            paramCodeFormule.Value = codeFormule;
            var paramCodeOption = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            paramCodeOption.Value = codeOption;
            var paramAvenant = new EacParameter("avenant", DbType.AnsiStringFixedLength);
            paramAvenant.Value = avenant;

            parameters.Add(paramCodeOffre);
            parameters.Add(paramVersion);
            parameters.Add(paramType);
            parameters.Add(paramCodeFormule);
            parameters.Add(paramCodeOption);
            parameters.Add(paramAvenant);

            return parameters;
        }

        private static FormuleDto InitializeFormuleGarantie(string codeOffre, string version, string type, string avenant, string codeFormule, ModeConsultation modeNavig, FormuleDto toReturn)
        {
            CheckFormule(codeOffre, version, type, codeFormule);
            toReturn.LettreLib = string.Empty;
            toReturn.Libelle = "Formule générale s'appliquant à l'ensemble de l'offre";

            var resCible = GetCibleOffre(codeOffre, version, type, avenant, modeNavig);
            if (resCible != null)
            {
                toReturn.CodeCible = resCible.CodeCible;
                toReturn.Cible = resCible.Cible;
                toReturn.DescCible = resCible.DescCible;
            }
            return toReturn;
        }

        private static void SetIsSorti(ModeConsultation modeNavig, FormuleDto toReturn, List<EacParameter> param)
        {
            if (modeNavig == ModeConsultation.Standard)
            {
                string sqlSorti = @"SELECT CASE WHEN (PBAVA * 10000 + PBAVM * 100 + PBAVJ > JEVFA * 10000 + JEVFM * 100 + JEVFJ AND JEVFA * 10000 + JEVFM * 100 + JEVFJ > 0) THEN 'O' ELSE 'N' END STRRETURNCOL
								FROM KPOPT 
									INNER JOIN KPOPTAP ON KDDIPB = KDBIPB AND KDDALX = KDBALX AND KDDTYP = KDBTYP AND KDDFOR = KDBFOR AND KDDOPT = KDBOPT and kddperi = 'RQ'
									INNER JOIN YPRTRSQ ON KDDIPB = JEIPB AND KDDALX = JEALX AND KDDRSQ = JERSQ
									INNER JOIN YPOBASE ON PBIPB = JEIPB AND PBALX = JEALX
								WHERE KDBIPB = :codeOffre AND KDBALX = :version AND KDBTYP = :type
							UNION 
								SELECT DISTINCT CASE WHEN (PBAVA * 10000 + PBAVM * 100 + PBAVJ > JGVFA * 10000 + JGVFM * 100 + JGVFJ AND JGVFA * 10000 + JGVFM * 100 + JGVFJ > 0) THEN 'O' ELSE 'N' END
									FROM KPOPT 
									INNER JOIN KPOPTAP ON KDDIPB = KDBIPB AND KDDALX = KDBALX AND KDDTYP = KDBTYP AND KDDFOR = KDBFOR AND KDDOPT = KDBOPT AND KDDPERI = 'OB'
									INNER JOIN YPRTOBJ ON KDDIPB = JGIPB AND KDDALX = JGALX AND KDDRSQ = JGRSQ AND KDDOBJ = JGOBJ
									INNER JOIN YPOBASE ON PBIPB = JGIPB AND PBALX = JGALX
								WHERE KDBIPB = :codeOffre AND KDBALX = :version AND KDBTYP = :type";

                var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlSorti, param.Take(3));
                if (result != null && result.Any())
                {
                    toReturn.IsSorti = result.Count > 1 ? false : result.FirstOrDefault().StrReturnCol == "O";
                }
            }
        }

        private static void SetDateEffetAvenantModif(ModeConsultation modeNavig, FormuleDto toReturn, List<EacParameter> param)
        {
            if (toReturn.IsTraceAvnExist)
            {
                string sql = $@"
SELECT KDBAVA * 10000 + KDBAVM * 100 + KDBAVJ DATEEFFETAVTMODIF
FROM {CommonRepository.GetPrefixeHisto(modeNavig, "KPOPT")}
WHERE KDBIPB = :codeOffre AND KDBALX = :version AND KDBTYP = :type AND KDBFOR = :codeFormule AND KDBOPT = :codeOption";
                var dateAvn = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param.Take(5));
                if (dateAvn != null)
                {
                    toReturn.DateEffetAvenantModificationLocale = AlbConvert.ConvertIntToDate(Convert.ToInt32(dateAvn));
                }
            }
        }

        private static void SetIsTraceAvenantExist(ModeConsultation modeNavig, FormuleDto toReturn, List<EacParameter> param)
        {
            string sql;
            if (modeNavig != ModeConsultation.Historique)
            {
                sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC 
						WHERE KHOIPB = :codeOffre AND KHOALX = :version AND KHOTYP = :type AND KHOPERI = 'OPT' 
						AND KHOFOR = :codeFormule AND KHOOPT = :codeOption AND KHOETAPE = '**********'";
                toReturn.IsTraceAvnExist = CommonRepository.ExistRowParam(sql, param.Take(5));
            }
            else
            {
                sql = string.Format(@"SELECT COUNT(*) NBLIGN FROM {0} 
								WHERE KDBIPB = :codeOffre AND KDBALX = :version AND KDBTYP = :type AND KDBFOR = :codeFormule
								AND KDBOPT = :codeOption AND (KDBAVE = :avenant OR KDBAVG = :avenant)",
                                                                                    CommonRepository.GetPrefixeHisto(modeNavig, "KPOPT"));
                toReturn.IsTraceAvnExist = CommonRepository.ExistRowParam(sql, param.Take(6));
            }
        }

        #endregion

        public static string EnregistrerFormuleGarantie(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption, string formGen, string codeAlpha, string branche, string codeCible, string libFormule, string codeObjetRisque, string user)
        {
            string toReturn = string.Empty;
            string dateModifRsq = string.Empty;

            if (codeFormule != "0")
            {
                toReturn = string.Format("{0}_{1}", UpdateFormule(codeOffre, version, type, codeFormule, codeCible, libFormule, user), codeAlpha);
            }
            else
            {
                toReturn = InsertFormule(codeOffre, version, type, avenant, codeFormule, formGen, codeAlpha, branche, codeCible, libFormule, user);
                //Récupération de la date de modif du rsq pour la création en avenant
                if (!string.IsNullOrEmpty(avenant) && avenant != "0")
                {
                    dateModifRsq = GetDateModifRsq(codeOffre, version, codeObjetRisque.Split(';')[0]);
                }
            }

            codeFormule = toReturn.Split('_')[0];
            if (codeOption == "0")
            {
                codeOption = "1";
            }

            SaveAppliqueA(codeOffre, version, type, codeFormule, codeOption, codeCible, formGen, codeObjetRisque, user);

            return toReturn + "_" + dateModifRsq;
        }

        public static List<GarantiePeriodeDto> GetDateDebByGaranties(int[] ids, int codeAvn)
        {
            var result = new List<GarantiePeriodeDto>();

            if (ids == null || ids.Length == 0)
            {
                return result;
            }

            var query = @"SELECT KDEID IDGARANTIE,

								 CASE  
								 WHEN  KDEDATDEB <> 0 THEN (KDEDATDEB * 10000 + LEFT(KDEHEUDEB,4)) 
								 ELSE NULL
								 END DATEDEBINT,

								 KDECRAVN ISCREATEAVN,
								 KDEMAJAVN ISUPDATEAVN
						  FROM KPGARAW
						  WHERE KDEID IN (" + string.Join(",", ids) + ")";

            result = DbBase.Settings.ExecuteList<GarantiePeriodeDto>(CommandType.Text, query);
            result.ForEach(i =>
            {
                i.DateDebut = AlbConvert.ConvertIntToDateHour((long?)i.DateDebutInt);
                //i.DateFin = AlbConvert.ConvertIntToDateHour((long?)i.DateFinInt);
                i.IsCreateAvn = i.CreateAvn == codeAvn ? true : false;
                i.IsUpdateAvn = i.UpdateAvn == codeAvn ? true : false;
            });
            return result;
        }

        public static List<GarantiePeriodeDto> GetDatesByGaranties(string codeOffre, string version, string type, string codeFormule, string codeOption, int[] ids)
        {
            var result = new List<GarantiePeriodeDto>();

            if (ids == null || ids.Length == 0)
            {
                return result;
            }

            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;

            var query = string.Format(@"SELECT KDESEQ IDGARANTIE, 
											   CASE  
											   WHEN  KDEDATDEB = 0 AND KDEWDDEB <> 0  THEN (KDEWDDEB * 10000 + LEFT(RIGHT(REPEAT('0', 6) || KDEWHDEB, 6), 4))
											   WHEN  KDEDATDEB <> 0 THEN (KDEDATDEB * 10000 + LEFT(RIGHT(REPEAT('0', 6) || KDEHEUDEB, 6), 4)) 
											   ELSE NULL
											   END DATEDEBINT,
											   
											   CASE  
											   WHEN KDEDATFIN = 0 AND KDEWDFIN <> 0  THEN (KDEWDFIN * 10000 + LEFT(RIGHT(REPEAT('0', 6) || KDEWHFIN, 6), 4)) 
											   WHEN KDEDATFIN <> 0  THEN (KDEDATFIN * 10000 + LEFT(RIGHT(REPEAT('0', 6) || KDEHEUFIN, 6), 4))   
											   ELSE NULL
											   END DATEFININT
										FROM KPGARAN
										WHERE KDEIPB = :codeOffre  AND KDEALX = :version  AND KDETYP = :type AND KDEFOR = :codeFormule AND KDEOPT= :codeOption  AND KDESEQ IN ({0})",
                                        string.Join(",", ids));

            result = DbBase.Settings.ExecuteList<GarantiePeriodeDto>(CommandType.Text, query, param);
            result.ForEach(i =>
            {
                i.DateDebut = AlbConvert.ConvertIntToDateHour((long?)i.DateDebutInt);
                i.DateFin = AlbConvert.ConvertIntToDateHour((long?)i.DateFinInt);
            });
            return result;
        }


        public static List<GarBlocRelationDto> GetGarBlocsRelations(List<long> garanties, List<long> blocs)
        {
            var sql = string.Empty;


            if (blocs.Any())
            {
                sql += string.Format(@" SELECT 'B'AS TYPE,KGJREL RELATION,CATBLOC1.KAQID ID1,CATBLOC2.KAQID ID2
									  FROM KBLOREL
											INNER JOIN KCATBLOC CATBLOC1 ON CATBLOC1.KAQKAEID= KGJIDBLO1 
											INNER JOIN KCATBLOC CATBLOC2 ON CATBLOC2.KAQKAEID= KGJIDBLO2 
									  WHERE  CATBLOC1.KAQID IN ({0}) AND CATBLOC2.KAQID IN ({0})", string.Join(",", blocs));
            }

            if (blocs.Any() && garanties.Any())
            {
                sql += " UNION ALL ";
            }

            if (garanties.Any())
            {
                sql += string.Format(@"SELECT 'G' TYPE , C5TYP RELATION ,C5SEQ ID1,C5SEM ID2
									  FROM YPLTGAA
									  WHERE C5SEQ IN ({0}) OR C5SEM IN ({0})",
                                     string.Join(",", garanties));
            }

            if (string.IsNullOrEmpty(sql))
            {
                return new List<GarBlocRelationDto>();
            }

            return DbBase.Settings.ExecuteList<GarBlocRelationDto>(CommandType.Text, sql);
        }





        /// <summary>
        /// Supprime les informations des garanties
        /// lors du changement de cible sur un risque
        /// ou lors du changement de risque avec cible différente
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="typeDel"></param>
        public static void DeleteFormuleGarantie(string codeOffre, string version, string type, string codeFormule, string typeDel)
        {
            EacParameter[] param = new EacParameter[5];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeFormule);
            param[4] = new EacParameter("P_TYPEDEL", DbType.AnsiStringFixedLength);
            param[4].Value = typeDel;

            CommonRepository.SetTraceLog(codeOffre, version, type, 0, "delete formule", "DELFORM", AlbConvert.ConvertDateToStr(DateTime.Now), "TRACEDEL");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELFORM", param);
        }

        public static void DeleteFormuleGarantieRsq(string codeOffre, string version, string type, string codeRsq, string typeDel)
        {
            var codeFormule = GetCodeFormuleRsq(codeOffre, version, type, codeRsq);
            DeleteFormuleGarantie(codeOffre, version, type, codeFormule, typeDel);
        }

        public static void CheckFormule(string codeOffre, string version, string type, string codeFormule)
        {
            var paramFor = new EacParameter[5];
            paramFor[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            paramFor[0].Value = codeOffre.PadLeft(9, ' ');
            paramFor[1] = new EacParameter("P_VERSION", DbType.Int32);
            paramFor[1].Value = Convert.ToInt32(version);
            paramFor[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            paramFor[2].Value = type;
            paramFor[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            paramFor[3].Value = string.IsNullOrEmpty(codeFormule) ? 0 : Convert.ToInt32(codeFormule);
            paramFor[4] = new EacParameter("P_TYPEDEL", DbType.AnsiStringFixedLength);
            paramFor[4].Value = "C";

            CommonRepository.SetTraceLog(codeOffre, version.ToString(), type, 0, "delete for", "CHCKFOR", AlbConvert.ConvertDateToStr(DateTime.Now), "TRACEDEL");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CHCKFOR", paramFor);
        }

        public static void BackwardFormule(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            EacParameter[] param = new EacParameter[5];
            param[0] = new EacParameter("P_CODEAFFAIRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEFOR", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeFormule);
            param[4] = new EacParameter("P_CODEOPT", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeOption);

            CommonRepository.SetTraceLog(codeOffre, version, type, 0, "delete garan", "BACKGARANTIE", AlbConvert.ConvertDateToStr(DateTime.Now), "TRACEDEL");

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_BACKGARANTIE", param);
        }

        public static string SaveDetailsGarantie(FormuleGarantieDetailsDto garantieDetails, string codeAvenant)
        {
            EacParameter[] param = new EacParameter[16];
            param[0] = new EacParameter("P_GUIDID", DbType.Int32);
            param[0].Value = Convert.ToInt32(garantieDetails.CodeGarantie);
            param[1] = new EacParameter("P_CODEAVENANT", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) : 0;
            param[2] = new EacParameter("P_DATEDEB", DbType.Int32);
            param[2].Value = garantieDetails.DateDeb != null ? AlbConvert.ConvertDateToInt(garantieDetails.DateDeb) : 0;
            param[3] = new EacParameter("P_DATEFIN", DbType.Int32);
            param[3].Value = garantieDetails.DateFin != null ? AlbConvert.ConvertDateToInt(garantieDetails.DateFin) : 0;
            param[4] = new EacParameter("P_HEUREDEB", DbType.Int32);
            param[4].Value = garantieDetails.HeureDeb != null ? AlbConvert.ConvertTimeToInt(garantieDetails.HeureDeb) : 0;
            param[5] = new EacParameter("P_HEUREFIN", DbType.Int32);
            param[5].Value = garantieDetails.HeureFin != null ? AlbConvert.ConvertTimeToInt(garantieDetails.HeureFin) : 0;
            param[6] = new EacParameter("P_DUREE", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(garantieDetails.Duree) ? Convert.ToInt32(garantieDetails.Duree) : 0;
            param[7] = new EacParameter("P_DUREEUNITE", DbType.AnsiStringFixedLength);
            param[7].Value = !string.IsNullOrEmpty(garantieDetails.DureeUnite) ? garantieDetails.DureeUnite : string.Empty;
            param[8] = new EacParameter("P_GARANTIEINDEXE", DbType.AnsiStringFixedLength);
            param[8].Value = garantieDetails.GarantieIndexe ? "O" : "N";
            param[9] = new EacParameter("P_CATNAT", DbType.AnsiStringFixedLength);
            param[9].Value = garantieDetails.CATNAT ? "O" : "N";
            param[10] = new EacParameter("P_MONTANTREF", DbType.AnsiStringFixedLength);
            param[10].Value = garantieDetails.InclusMontant ? "O" : "N";
            param[11] = new EacParameter("P_APPLICATION", DbType.AnsiStringFixedLength);
            param[11].Value = garantieDetails.Application;
            param[12] = new EacParameter("P_TYPEEMISSION", DbType.AnsiStringFixedLength);
            param[12].Value = garantieDetails.TypeEmission;
            param[13] = new EacParameter("P_CODETAXE", DbType.AnsiStringFixedLength);
            param[13].Value = garantieDetails.CodeTaxe;
            param[14] = new EacParameter("P_ALIMAUTO", DbType.AnsiStringFixedLength);
            param[14].Value = garantieDetails.AlimAssiette;
            param[15] = new EacParameter("P_FLAGMODIF", DbType.AnsiStringFixedLength);
            param[15].Value = "";
            param[15].Direction = ParameterDirection.Output;
            param[15].Size = 1;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SVDETGA", param);

            return param[15].Value.ToString();
        }

        public static bool CheckDatesGarantie(FormuleGarantieDetailsDto garantieDetails)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("P_GUIDID", DbType.Int32);
            param[0].Value = Convert.ToInt32(garantieDetails.CodeGarantie);
            param[1] = new EacParameter("P_DATEDEB", DbType.Int32);
            param[1].Value = garantieDetails.DateDeb != null ? (Convert.ToInt64(AlbConvert.ConvertDateToInt(garantieDetails.DateDeb)) * 1000000) + AlbConvert.ConvertTimeToInt(garantieDetails.HeureDeb) : 0;
            param[2] = new EacParameter("P_DATEFIN", DbType.Int32);
            param[2].Value = garantieDetails.DateFin != null ? (Convert.ToInt64(AlbConvert.ConvertDateToInt(garantieDetails.DateFin)) * 1000000) + AlbConvert.ConvertTimeToInt(garantieDetails.HeureFin) : 0;
            param[3] = new EacParameter("P_RETOUR", DbType.Int32);
            param[3].Direction = ParameterDirection.Output;
            param[3].DbType = DbType.Int32;
            param[3].Value = 0;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CHECKDATESGAR", param);
            return ((int)param[3].Value) == 0;
        }
        /// <summary>
        /// Récupère les informations de portée de la garantie
        /// </summary>
        public static FormuleGarantiePorteeDto ObtenirGarantiePortee(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie, ModeConsultation modeNavig, string alimAssiette, string branche, string cible, string codeObjetRisque, string dateModifAvn = "")
        {
            if (string.IsNullOrEmpty(dateModifAvn))
            {
                var param = new EacParameter[3];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                param[1].Value = version;
                param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[2].Value = type;

                string sql = @"select (pbava * 10000 + pbavm * 100 + pbavj) INT64RETURNCOL from ypobase where pbipb = :codeOffre and pbalx = :version and pbtyp = :type";
                var resDateModif = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
                if (resDateModif != null && resDateModif.Any())
                {
                    dateModifAvn = resDateModif.FirstOrDefault().Int64ReturnCol.ToString();
                }
            }
            else
            {
                dateModifAvn = AlbConvert.ConvertDateToInt(AlbConvert.ConvertStrToDate(dateModifAvn)).ToString();
            }

            var model = new FormuleGarantiePorteeDto();

            var result = modeNavig == ModeConsultation.Standard ?
                GetInfoPorteeGarantie(codeOffre, version, type, codeFormule, codeOption, codeGarantie, codeObjetRisque, modeNavig) :
                GetInfoPorteeGarantieHisto(codeOffre, version, type, codeAvn, codeFormule, codeOption, codeGarantie);

            List<RisqueDto> risqueDto = new List<RisqueDto>();
            if (result != null && result.Count > 0)
            {
                string action = string.Empty;
                var lstRsq = result.GroupBy(el => el.CodeRsq).Select(r => r.First()).ToList();
                lstRsq.ForEach(r =>
                {
                    var lstObj = result.FindAll(o => o.CodeRsq == r.CodeRsq).ToList();
                    List<ObjetDto> objetDto = new List<ObjetDto>();
                    lstObj.ForEach(obj =>
                    {
                        //2016-12-07 correction portée : ajout du test dateModifAvn = "0"
                        if (dateModifAvn == "0" || string.IsNullOrEmpty(dateModifAvn) || AlbConvert.ConvertIntToDate(Convert.ToInt32(dateModifAvn)) <= AlbConvert.ConvertIntToDate(Convert.ToInt32(obj.DateSortieObj)) || obj.DateSortieObj == 0)
                        {
                            objetDto.Add(new ObjetDto
                            {
                                Code = Convert.ToInt32(obj.CodeObj),
                                Designation = obj.LibObj,
                                Action = obj.Action,
                                Valeur = Convert.ToInt32(obj.ValeurObj),
                                Unite = new ParametreDto { Code = obj.UnitObj },
                                Type = new ParametreDto { Code = obj.TypeObj },
                                ValPorteeObj = obj.ValPorteeObj,
                                UnitPorteeObj = obj.UnitPorteObj.Trim(),
                                TypePorteeCal = obj.TypePorteeCal.Trim(),
                                PrimeMntCal = obj.PrimeMntCal
                            });
                            if (!string.IsNullOrEmpty(obj.Action))
                            {
                                action = obj.Action;
                            }
                        }

                    });
                    risqueDto.Add(new RisqueDto
                    {
                        Code = Convert.ToInt32(r.CodeRsq),
                        Designation = r.LibRsq,
                        Objets = objetDto
                    });
                });
                model.IdGarantie = result[0].IdGar;
                model.SequenceGarantie = result[0].SeqGar;
                model.CodeGarantie = result[0].CodeGar;
                model.LibelleGarantie = result[0].LibGar;
                model.IdPortee = result[0].IdPortee;
                model.Action = action;
                model.ReportCal = result[0].ReportCal == "B" || result[0].ReportCal == "C";
            }
            model.Risque = risqueDto.FirstOrDefault();
            model.Actions = new List<ParametreDto>();


            foreach (AlbConstantesMetiers.InvenExclusInclus val in Enum.GetValues(typeof(AlbConstantesMetiers.InvenExclusInclus)))
            {
                if ((AlbEnumInfoValue.GetEnumInfo(val) == "E" && alimAssiette != "B" && alimAssiette != "C") || AlbEnumInfoValue.GetEnumInfo(val) != "E")
                {
                    model.Actions.Add(new ParametreDto { Code = AlbEnumInfoValue.GetEnumInfo(val), Descriptif = val.ToString(), Libelle = val.ToString() });
                }
            }

            model.UnitesTaux = CommonRepository.GetParametres(branche, cible, "ALSPK", "UNPRI");
            model.TypesCalTaux = new List<ParametreDto>();
            foreach (AlbConstantesMetiers.TypesCal val in Enum.GetValues(typeof(AlbConstantesMetiers.TypesCal)))
            {
                model.TypesCalTaux.Add(new ParametreDto { Code = AlbEnumInfoValue.GetEnumInfo(val), Descriptif = val.ToString(), Libelle = val.ToString() });
            }

            return model;
        }

        /// <summary>
        /// Sauvegarde les données de portée de garantie
        /// </summary>
        public static void SavePorteeGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, string idGarantie, string codeGarantie, string nature, string codeRsq, string codesObj, string user)
        {
            EacParameter[] param = new EacParameter[12];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeFormule);
            param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeOption);
            param[5] = new EacParameter("P_IDGARANTIE", DbType.Int32);
            param[5].Value = Convert.ToInt32(idGarantie);
            param[6] = new EacParameter("P_CODEGARANTIE", DbType.AnsiStringFixedLength);
            param[6].Value = codeGarantie;
            param[7] = new EacParameter("P_NATURE", DbType.AnsiStringFixedLength);
            param[7].Value = nature;
            param[8] = new EacParameter("P_CODERSQ", DbType.Int32);
            param[8].Value = Convert.ToInt32(codeRsq);
            param[9] = new EacParameter("P_CODESOBJ", DbType.AnsiStringFixedLength);
            param[9].Value = codesObj;
            param[10] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[10].Value = user;
            param[11] = new EacParameter("P_DATENOW", DbType.Int32);
            param[11].Value = AlbConvert.ConvertDateToInt(DateTime.Now);

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SAVEPORTEEGARANTIE", param);
        }

        public static void SavePorteeGarantieAlimAssiette(string codeOffre, string version, string type, string codeFormule, string codeOption, string idGarantie, string codeGarantie, string nature, RisqueDto rsq, string alimAssiette, string user, bool reportCal)
        {
            #region Suppression de la portée

            EacParameter[] paramDel = new EacParameter[6];
            paramDel[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramDel[0].Value = codeOffre.PadLeft(9, ' ');
            paramDel[1] = new EacParameter("version", DbType.Int32);
            paramDel[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            paramDel[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramDel[2].Value = type;
            paramDel[3] = new EacParameter("codeFor", DbType.Int32);
            paramDel[3].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
            paramDel[4] = new EacParameter("codeOpt", DbType.Int32);
            paramDel[4].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;
            paramDel[5] = new EacParameter("codeGar", DbType.Int32);
            paramDel[5].Value = !string.IsNullOrEmpty(idGarantie) ? Convert.ToInt32(idGarantie) : 0;

            string sqlDel = @"DELETE FROM KPGARAP WHERE KDFIPB = :codeOffre AND KDFALX = :version AND KDFTYP = :type AND KDFFOR = :codeFor AND KDFOPT = :codeOpt AND KDFKDEID = :codeGar";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDel, paramDel);

            #endregion

            #region Insertion de la portée

            double mntTotal = 0;
            bool errMntTotal = false;

            var dateNow = DateTime.Now;
            foreach (var obj in rsq.Objets)
            {
                EacParameter[] paramIns = new EacParameter[22];
                paramIns[0] = new EacParameter("KDFID", DbType.Int32);
                paramIns[0].Value = CommonRepository.GetAS400Id("KDFID");
                paramIns[1] = new EacParameter("KDFIPB", DbType.AnsiStringFixedLength);
                paramIns[1].Value = codeOffre.PadLeft(9, ' ');
                paramIns[2] = new EacParameter("KDFALX", DbType.Int32);
                paramIns[2].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                paramIns[3] = new EacParameter("KDFTYP", DbType.AnsiStringFixedLength);
                paramIns[3].Value = type;
                paramIns[4] = new EacParameter("KDFFOR", DbType.Int32);
                paramIns[4].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
                paramIns[5] = new EacParameter("KDFOPT", DbType.Int32);
                paramIns[5].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;
                paramIns[6] = new EacParameter("KDFGARAN", DbType.AnsiStringFixedLength);
                paramIns[6].Value = codeGarantie;
                paramIns[7] = new EacParameter("KDFKDEID", DbType.Int32);
                paramIns[7].Value = !string.IsNullOrEmpty(idGarantie) ? Convert.ToInt32(idGarantie) : 0;
                paramIns[8] = new EacParameter("KDFGAN", DbType.AnsiStringFixedLength);
                paramIns[8].Value = nature;
                paramIns[9] = new EacParameter("KDFPERI", DbType.AnsiStringFixedLength);
                paramIns[9].Value = string.Empty;
                paramIns[10] = new EacParameter("KDFRSQ", DbType.Int32);
                paramIns[10].Value = rsq.Code;
                paramIns[11] = new EacParameter("KDFOBJ", DbType.Int32);
                paramIns[11].Value = obj.Code;
                paramIns[12] = new EacParameter("KDFINVEN", DbType.Int32);
                paramIns[12].Value = 0;
                paramIns[13] = new EacParameter("KDFINVEP", DbType.Int32);
                paramIns[13].Value = 0;
                paramIns[14] = new EacParameter("KDFCRU", DbType.AnsiStringFixedLength);
                paramIns[14].Value = user;
                paramIns[15] = new EacParameter("KDFCRD", DbType.Int32);
                paramIns[15].Value = AlbConvert.ConvertDateToInt(dateNow);
                paramIns[16] = new EacParameter("KDFMAJU", DbType.AnsiStringFixedLength);
                paramIns[16].Value = user;
                paramIns[17] = new EacParameter("KDFMAJD", DbType.Int32);
                paramIns[17].Value = AlbConvert.ConvertDateToInt(dateNow);
                paramIns[18] = new EacParameter("KDFPRA", DbType.Int32);
                paramIns[18].Value = obj.ValPorteeObj;
                paramIns[19] = new EacParameter("KDFPRU", DbType.AnsiStringFixedLength);
                paramIns[19].Value = obj.UnitPorteeObj;
                paramIns[20] = new EacParameter("KDFTYC", DbType.AnsiStringFixedLength);
                paramIns[20].Value = obj.TypePorteeCal;
                paramIns[21] = new EacParameter("KDFMNT", DbType.Int32);
                paramIns[21].Value = obj.PrimeMntCal;

                string sqlIns = @"INSERT INTO KPGARAP
									   (KDFID , KDFIPB , KDFALX , KDFTYP , KDFFOR , KDFOPT , KDFGARAN , KDFKDEID , KDFGAN , 
										KDFPERI , KDFRSQ , KDFOBJ , KDFINVEN , KDFINVEP , KDFCRU , KDFCRD , KDFMAJU , KDFMAJD,
										KDFPRA , KDFPRU , KDFTYC , KDFMNT)
									VALUES
										(:KDFID , :KDFIPB , :KDFALX , :KDFTYP , :KDFFOR , :KDFOPT , :KDFGARAN , :KDFKDEID , :KDFGAN , 
										:KDFPERI , :KDFRSQ , :KDFOBJ , :KDFINVEN , :KDFINVEP , :KDFCRU , :KDFCRD , :KDFMAJU , :KDFMAJD,
										:KDFPRA , :KDFPRU , :KDFTYC , :KDFMNT)";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlIns, paramIns);

                mntTotal += obj.PrimeMntCal;
                if (obj.PrimeMntCal == 0)
                {
                    errMntTotal = true;
                }

            }

            #endregion

            if (reportCal)
            {
                #region Alimentation de la prime

                EacParameter[] paramPrime = new EacParameter[9];
                paramPrime[0] = new EacParameter("mntPrimeO", DbType.Int32);
                paramPrime[0].Value = errMntTotal ? 0 : mntTotal;
                paramPrime[1] = new EacParameter("mntPrimeA", DbType.Int32);
                paramPrime[1].Value = errMntTotal ? 0 : mntTotal;
                paramPrime[2] = new EacParameter("primeUnit", DbType.AnsiStringFixedLength);
                paramPrime[2].Value = "D";
                paramPrime[3] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramPrime[3].Value = codeOffre.PadLeft(9, ' ');
                paramPrime[4] = new EacParameter("version", DbType.Int32);
                paramPrime[4].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                paramPrime[5] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramPrime[5].Value = type;
                paramPrime[6] = new EacParameter("codeFor", DbType.Int32);
                paramPrime[6].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
                paramPrime[7] = new EacParameter("codeOpt", DbType.Int32);
                paramPrime[7].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;
                paramPrime[8] = new EacParameter("idGarantie", DbType.Int32);
                paramPrime[8].Value = !string.IsNullOrEmpty(idGarantie) ? Convert.ToInt32(idGarantie) : 0;

                string sqlPrime = @"UPDATE KPGARTAR SET KDGPRIVALO = :mntPrimeO, KDGPRIVALA = :mntPrimeA, KDGPRIUNIT = :primeUnit 
								WHERE KDGIPB = :codeOffre AND KDGALX = :version AND KDGTYP = :type AND KDGFOR = :codeFor AND KDGOPT = :codeOpt AND KDGKDEID = :idGarantie";

                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlPrime, paramPrime);

                #endregion
            }

            #region MAJ champ prime garantie

            EacParameter[] paramAlimPrim = new EacParameter[1];
            paramAlimPrim[0] = new EacParameter("idGarantie", DbType.Int32);
            paramAlimPrim[0].Value = !string.IsNullOrEmpty(idGarantie) ? Convert.ToInt32(idGarantie) : 0;

            string alimPrime = string.Empty;
            string alimCurPrime = string.Empty;
            string sqlAlimPrim = @"SELECT KDEPALA STRRETURNCOL, KDEALA STRRETURNCOL2 FROM KPGARAN WHERE KDEID = :idGarantie";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sqlAlimPrim, paramAlimPrim);
            if (result != null && result.Any())
            {
                alimPrime = result.FirstOrDefault().StrReturnCol;
                alimCurPrime = result.FirstOrDefault().StrReturnCol2;
                if (!reportCal)
                {
                    switch (alimPrime)
                    {
                        case "B":
                            alimPrime = "A";
                            break;
                        case "C":
                            alimPrime = string.Empty;
                            break;
                        default:
                            alimPrime = alimCurPrime != "B" && alimCurPrime != "C" ? alimCurPrime : alimPrime;
                            break;
                    }
                }
                else
                {
                    //2017-02-07 : Correction bug 2246
                    alimPrime = alimCurPrime;
                    //switch (alimPrime)
                    //{
                    //    case "A":
                    //        alimPrime = "B";
                    //        break;
                    //    case "":
                    //        alimPrime = "C";
                    //        break;
                    //    default:                    
                    //        break;
                    //}
                }
            }

            EacParameter[] paramPrimeGar = new EacParameter[2];
            paramPrimeGar[0] = new EacParameter("KDEALA", DbType.AnsiStringFixedLength);
            paramPrimeGar[0].Value = alimPrime;
            paramPrimeGar[1] = new EacParameter("idGarantie", DbType.Int32);
            paramPrimeGar[1].Value = !string.IsNullOrEmpty(idGarantie) ? Convert.ToInt32(idGarantie) : 0;

            string sqlPrimeGar = @"UPDATE KPGARAN SET KDEALA = :KDEALA WHERE KDEID = :idGarantie";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlPrimeGar, paramPrimeGar);

            #endregion


            if (!CommonRepository.CheckControlAssiette(codeOffre, version, type))
            {
                CommonRepository.InsertControlAssiette(codeOffre, version, type, "GAR", "S'applique à", user);
            }
        }

        /// <summary>
        /// Retourne la typologie à sélectionner pour les modèles de garantie
        /// en fonction de l'offre
        /// </summary>
        /// <returns></returns>
        public static string GetTypologieModele(string codeOffre, int version, string type, int codeCible, string branche, int? date)
        {
            string typo;

            var param = new EacParameter[3];
            param[0] = new EacParameter("branche", DbType.AnsiStringFixedLength);
            param[0].Value = branche;
            param[1] = new EacParameter("codeCible", DbType.Int32);
            param[1].Value = codeCible;
            param[2] = new EacParameter("dateApp", DbType.Int32);
            param[2].Value = date;

            string sql = @"SELECT DISTINCT KARTYPO CODE
					FROM KCATMODELE
						INNER JOIN KCATBLOC ON KARKAQID = KAQID
						INNER JOIN KCATVOLET ON KAQKAPID = KAPID
					WHERE KAPBRA = :branche AND KAPKAIID = :codeCible AND KARDATEAPP <= :dateApp";

            var resultTypo = DbBase.Settings.ExecuteList<ParametreDto>(CommandType.Text, sql, param);

            var paramInfoOffre = new EacParameter[3];
            paramInfoOffre[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramInfoOffre[0].Value = codeOffre.PadLeft(9, ' ');
            paramInfoOffre[1] = new EacParameter("version", DbType.Int32);
            paramInfoOffre[1].Value = version;
            paramInfoOffre[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramInfoOffre[2].Value = type;

            string sqlInfoOffre = @"SELECT PBNPL NATURECONTRAT, JDITC INTERCALAIRE
					FROM YPOBASE 
						INNER JOIN YPRTENT ON JDIPB = PBIPB AND JDALX = PBALX
					WHERE PBIPB = :codeOffre AND PBALX = :version AND PBTYP = :type";

            var result = DbBase.Settings.ExecuteList<OffreDto>(CommandType.Text, sqlInfoOffre, paramInfoOffre).FirstOrDefault();
            if (result != null && result.NatureContratStr == "C")
            {
                if (result.IntercalaireStr == "O")
                {
                    typo = resultTypo.Exists(t => t.Code == "COA") ? "COA" : resultTypo.Exists(t => t.Code == "ITC") ? "ITC" : "STD";
                }
                else
                {
                    typo = resultTypo.Exists(t => t.Code == "COA") ? "COA" : "STD";
                }
            }
            else
            {
                if (result.IntercalaireStr == "O")
                {
                    typo = resultTypo.Exists(t => t.Code == "ITC") ? "ITC" : "STD";
                }
                else
                {
                    typo = "STD";
                }
            }


            return typo;
        }

        public static void DeleteTraceFormule(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;

            string sql = @"DELETE FROM KPCTRLE WHERE KEVIPB = :codeOffre AND KEVALX = :version AND KEVTYP = :type 
							  AND KEVFOR = :codeFormule AND KEVOPT = :codeOption";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        public static string GetLibCible(string codeCible)
        {
            var param = new EacParameter[1];
            param[0] = new EacParameter("codeCible", DbType.AnsiStringFixedLength);
            param[0].Value = codeCible;

            string sql = @"SELECT KAHCIBLE CODE, KAHDESC DESCRIPTION FROM KCIBLEF INNER JOIN KCIBLE ON KAIKAHID = KAHID WHERE KAIID = :codeCible";
            var result = DbBase.Settings.ExecuteList<CibleDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return string.Format("{0} - {1}", result.FirstOrDefault().Code, result.FirstOrDefault().Description);
            }

            return string.Empty;
        }

        public static void SaveFormuleFromCondition(string codeOffre, string version, string type, string codeFormule, string codeOption, string libelle)
        {
            EacParameter[] param = new EacParameter[12];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEAVENANT", DbType.Int32);
            param[3].Value = 0;
            param[4] = new EacParameter("P_CODEFORMULE", DbType.AnsiStringFixedLength);
            param[4].Value = codeFormule;
            param[5] = new EacParameter("P_CODEOPTION", DbType.AnsiStringFixedLength);
            param[5].Value = codeOption;
            param[6] = new EacParameter("P_LIBELLE", DbType.AnsiStringFixedLength);
            param[6].Value = libelle;
            param[7] = new EacParameter("P_DATEAVT", DbType.Int32);
            param[7].Value = 0;
            param[8] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[8].Value = string.Empty;
            param[9] = new EacParameter("P_DATENOW", DbType.Int32);
            param[9].Value = 0;
            param[10] = new EacParameter("P_HEURENOW", DbType.Int32);
            param[10].Value = 0;
            param[11] = new EacParameter("P_ERROR", DbType.AnsiStringFixedLength);
            param[11].Direction = ParameterDirection.Output;
            param[11].DbType = DbType.AnsiString;
            param[11].Size = 5000;
            param[11].Value = string.Empty;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SVOPTD", param);
        }

        /// <summary>
        /// Renvoi les informations de la cible de la formule
        /// </summary>
        public static FormuleDto GetCibleInfoFormule(string codeOffre, string version, string type, string codeFormule)
        {
            FormuleDto formule = new FormuleDto();
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;

            string sql = @"SELECT KDACIBLE CIBLE, KAHDESC DESCCIBLE FROM KPFOR INNER JOIN KCIBLE ON KAHCIBLE = KDACIBLE 
											WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KDAFOR = :codeFormule";
            var result = DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                formule.Cible = result.FirstOrDefault().Cible;
                formule.DescCible = result.FirstOrDefault().DescCible;
            }

            return formule;
        }

        /// <summary>
        /// Met à jour les dates de début forcées pour les garanties d'un volet/bloc
        /// </summary>
        public static void UpdateDateDebForcee(string codeOffre, string version, string type, string codeAvn, string niveauLib, string guidV, string guidB, string guidG, int? dateModifAvt, string user)
        {

            EacParameter[] param = new EacParameter[10];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.String);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("P_TYPE", DbType.String);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEAVN", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            param[4] = new EacParameter("P_NIVEAULIB", DbType.AnsiStringFixedLength);
            param[4].Value = niveauLib.ToUpper();
            param[5] = new EacParameter("P_GUIDV", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(guidV) ? Convert.ToInt32(guidV) : 0;
            param[6] = new EacParameter("P_GUIDB", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(guidB) ? Convert.ToInt32(guidB) : 0;
            param[7] = new EacParameter("P_GUIDG", DbType.Int32);
            param[7].Value = !string.IsNullOrEmpty(guidG) ? Convert.ToInt32(guidG) : 0;
            param[8] = new EacParameter("P_DATEUPD", DbType.Int32);
            param[8].Value = dateModifAvt;
            param[9] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[9].Value = user;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATEDATEDEBFORCEE", param);
        }

        /// <summary>
        /// Met à jour les dates de fin forcées pour les garanties d'un volet/bloc
        /// </summary>
        public static void UpdateDateFinForcee(string codeAvn, string niveauLib, string guidV, string guidB, string guidG, int? dateModifAvt, string user)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_CODEAVN", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            param[1] = new EacParameter("P_NIVEAULIB", DbType.AnsiStringFixedLength);
            param[1].Value = niveauLib.ToUpper();
            param[2] = new EacParameter("P_GUIDV", DbType.Int32);
            param[2].Value = !string.IsNullOrEmpty(guidV) ? Convert.ToInt32(guidV) : 0;
            param[3] = new EacParameter("P_GUIDB", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(guidB) ? Convert.ToInt32(guidB) : 0;
            param[4] = new EacParameter("P_GUIDG", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(guidG) ? Convert.ToInt32(guidG) : 0;
            param[5] = new EacParameter("P_DATEUPD", DbType.Int32);
            param[5].Value = dateModifAvt;
            param[6] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
            param[6].Value = user;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATEDATEFINFORCEE", param);
        }
        /// <summary>
        /// Parfait achèvement : initialisation des dates de garantie
        /// </summary>
        public static void InitParfaitAchevement(string codeOffre, string version, string type, string codeGarantie)
        {
            /*lire le référentiel et la durée  */
            var param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeGarantie", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeGarantie) ? Convert.ToInt32(codeGarantie) : 0;

            string sql = @"SELECT GADFG STRRETURNCOL,TPCN1 DECRETURNCOL, KDEWDFIN DATEFINRETURNCOL,SUBSTR(KDEWHFIN,0,5) INT32RETURNCOL FROM KPGARAN 
									INNER JOIN KGARAN ON TRIM(GAGAR) = TRIM(KDEGARAN)
									INNER JOIN YYYYPAR ON TCON='PRODU' AND TFAM='GADFG' AND TCOD =GADFG 
									WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type AND KDEID = :codeGarantie";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                var ligneGar = result.FirstOrDefault();
                if (ligneGar.StrReturnCol == "H" && ligneGar.DateFinReturnCol != 0)
                {
                    DateTime? dateFin = AlbConvert.ConvertIntToDateHour(ligneGar.DateFinReturnCol * 10000 + ligneGar.Int32ReturnCol).Value.AddMinutes(1);
                    DateTime? dateDeb = dateFin.Value.AddMonths(-Convert.ToInt32(Math.Floor(ligneGar.DecReturnCol)));
                    var paramUpdate = new EacParameter[7];
                    paramUpdate[0] = new EacParameter("datefin", DbType.Int32);
                    paramUpdate[0].Value = ligneGar.DateFinReturnCol;
                    paramUpdate[1] = new EacParameter("heurefin", DbType.Int32);
                    paramUpdate[1].Value = ligneGar.Int32ReturnCol;
                    paramUpdate[2] = new EacParameter("datedeb", DbType.Int32);
                    paramUpdate[2].Value = AlbConvert.ConvertDateToInt(dateDeb);
                    paramUpdate[3] = new EacParameter("codeoffre", DbType.AnsiStringFixedLength);
                    paramUpdate[3].Value = codeOffre.PadLeft(9, ' ');
                    paramUpdate[4] = new EacParameter("version", DbType.Int32);
                    paramUpdate[4].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                    paramUpdate[5] = new EacParameter("type", DbType.AnsiStringFixedLength);
                    paramUpdate[5].Value = type;
                    paramUpdate[6] = new EacParameter("codegarantie", DbType.Int32);
                    paramUpdate[6].Value = !string.IsNullOrEmpty(codeGarantie) ? Convert.ToInt32(codeGarantie) : 0;
                    string sqlUpdate = @" UPDATE KPGARAN
													SET KDEDATFIN = :datefin , KDEHEUFIN= :heurefin ,KDEDATDEB = :datedeb ,KDEHEUDEB= 0 
													 WHERE KDEIPB  = :codeoffre AND KDEALX = :version AND KDETYP = :type AND KDEID = :codegarantie ";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlUpdate, paramUpdate);

                }
            }
        }

        public static DtoCommon InitParfaitAchevementNoUpdate(string gar)
        {
            /*lire le référentiel et la durée  */
            var param = new EacParameter[1];
            param[0] = new EacParameter("garantie", DbType.AnsiStringFixedLength);
            param[0].Value = gar;
            string sql = @"SELECT GADFG STRRETURNCOL,TPCN1 DECRETURNCOL FROM KGARAN
										INNER JOIN YYYYPAR ON TCON='PRODU' AND TFAM='GADFG' AND TCOD =GADFG 
										WHERE GAGAR = :garantie";
            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }

            return new DtoCommon();
        }

        public static GarantieDetailInfoDto GetInfoDetailsGarantie(string codeOffre, string version, string type, string codeGarantie)
        {
            var toReturn = new GarantieDetailInfoDto();

            var parameters = new List<DbParameter>()
            {
                new EacParameter("codeOffre", codeOffre.Trim().PadLeft(9, ' ')),
                new EacParameter("version", DbType.Int32) { Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0 },
                new EacParameter("type", type),
                new EacParameter("codeGarantie", DbType.Int32) { Value = !string.IsNullOrEmpty(codeGarantie) ? Convert.ToInt32(codeGarantie) : 0 }
            };

            const string Selection = @"
SELECT KDEGARAN CODEGAR, GADES LIBGAR, C2TCD TYPECONTROLE, '' LIBTYPECONTROLE, C2ALT GROUPEALT, C2PRP TYPEAPPLI,
ASS.C4VAL ASSVAL, ASS.C4UNT ASSUNIT, ASS.C4BAS ASSBASE, ASS.C4MAJ ASSMODI, ASS.C4OBL ASSOBLI, 
FRH.C4VAL FRHVAL, FRH.C4UNT FRHUNIT, FRH.C4BAS FRHBASE, FRH.C4MAJ FRHMODI, FRH.C4OBL FRHOBLI, 
LCI.C4VAL LCIVAL, LCI.C4UNT LCIUNIT, LCI.C4BAS LCIBASE, LCI.C4MAJ LCIMODI, LCI.C4OBL LCIOBLI, 
PRI.C4VAL PRIVAL, PRI.C4UNT PRIUNIT, PRI.C4BAS PRIBASE, PRI.C4MAJ PRIMODI, PRI.C4OBL PRIOBLI 
FROM KPGARAN 
INNER JOIN KGARAN ON GAGAR = KDEGARAN
INNER JOIN YPLTGAR ON KDESEQ = C2SEQ
INNER JOIN YPLTGAL FRH ON KDESEQ = FRH.C4SEQ AND FRH.C4TYP = 3
INNER JOIN YPLTGAL LCI ON KDESEQ = LCI.C4SEQ AND LCI.C4TYP = 2 
INNER JOIN YPLTGAL ASS ON KDESEQ = ASS.C4SEQ AND ASS.C4TYP = 0 
INNER JOIN YPLTGAL PRI ON KDESEQ = PRI.C4SEQ AND PRI.C4TYP = 1 
WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type AND  KDEID = :codeGarantie";

            var result = DbBase.Settings.ExecuteList<GarantieDetailInfoDto>(CommandType.Text, Selection, parameters);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();
                toReturn = firstRes;
            }

            return toReturn;
        }

        public static GarantieDetailInfoDto GetInfoDetailsGarantie(string codeOffre, string version, string type, string codeSequence, string codeFormule, string codeOption)
        {
            var toReturn = new GarantieDetailInfoDto();

            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;
            param[5] = new EacParameter("codeSequence", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(codeSequence) ? Convert.ToInt32(codeSequence) : 0;

            string sql = @"SELECT KDEGARAN CODEGAR, GADES LIBGAR, C2TCD TYPECONTROLE, '' LIBTYPECONTROLE, C2ALT GROUPEALT, C2PRP TYPEAPPLI,
							ASS.C4VAL ASSVAL, ASS.C4UNT ASSUNIT, ASS.C4BAS ASSBASE, ASS.C4MAJ ASSMODI, ASS.C4OBL ASSOBLI, 
							FRH.C4VAL FRHVAL, FRH.C4UNT FRHUNIT, FRH.C4BAS FRHBASE, FRH.C4MAJ FRHMODI, FRH.C4OBL FRHOBLI, 
							LCI.C4VAL LCIVAL, LCI.C4UNT LCIUNIT, LCI.C4BAS LCIBASE, LCI.C4MAJ LCIMODI, LCI.C4OBL LCIOBLI, 
							PRI.C4VAL PRIVAL, PRI.C4UNT PRIUNIT, PRI.C4BAS PRIBASE, PRI.C4MAJ PRIMODI, PRI.C4OBL PRIOBLI 
						FROM KPGARAN 
							INNER JOIN KGARAN ON TRIM(GAGAR) = TRIM(KDEGARAN)
							INNER JOIN YPLTGAR ON KDESEQ = C2SEQ
							INNER JOIN YPLTGAL FRH ON KDESEQ = FRH.C4SEQ AND FRH.C4TYP = 3
							INNER JOIN YPLTGAL LCI ON KDESEQ = LCI.C4SEQ AND LCI.C4TYP = 2 
							INNER JOIN YPLTGAL ASS ON KDESEQ = ASS.C4SEQ AND ASS.C4TYP = 0 
							INNER JOIN YPLTGAL PRI ON KDESEQ = PRI.C4SEQ AND PRI.C4TYP = 1 
						WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type AND KDEFOR = :codeFormule AND KDEOPT = :codeOption AND KDESEQ = :codeSequence";


            var result = DbBase.Settings.ExecuteList<GarantieDetailInfoDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                var firstRes = result.FirstOrDefault();
                toReturn = firstRes;
            }

            return toReturn;
        }

        public static string CheckDateModifFormule(string codeOffre, string version, string type, string codeObjetRisque, string dateAvenant)
        {
            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("codeRsq", DbType.Int32);
            param[2].Value = !string.IsNullOrEmpty(codeObjetRisque) ? Convert.ToInt32(codeObjetRisque.Split(';')[0]) : 0;

            string sql = @"SELECT JEVDA * 100000000 + JEVDM * 1000000 + JEVDJ * 10000 + JEVDH DATEDEBRETURNCOL, 
									JEVFA * 100000000 + JEVFM * 1000000 + JEVFJ * 10000 + JEVFH DATEFINRETURNCOL
							FROM YPRTRSQ
							WHERE JEIPB = :codeoffre AND JEALX = :version AND JERSQ = :codeRsq";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                var dateDeb = AlbConvert.ConvertIntToDateHour(result.FirstOrDefault().DateDebReturnCol);
                var dateFin = AlbConvert.ConvertIntToDateHour(result.FirstOrDefault().DateFinReturnCol);

                var dateAvt = AlbConvert.ConvertIntToDate(Convert.ToInt32(dateAvenant));

                if ((dateDeb != null && dateAvt < dateDeb.Value.Date) || (dateFin != null && dateAvt > dateFin.Value.AddDays(1)))
                {
                    return "##;ERRORDATE;Date de modification incohérente avec la période du risque;##";
                }
            }

            return string.Empty;
        }

        public static bool IsTraceAvnExist(string codeAffaire, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeAffaire", DbType.AnsiStringFixedLength);
            param[0].Value = codeAffaire.PadLeft(9, ' ');
            param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[1].Value = type;
            param[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[2].Value = version;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;

            string sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC 
										WHERE KHOIPB = :codeAffaire AND KHOTYP = :type AND KHOALX = :version AND KHOPERI = 'OPT' 
											AND KHOFOR = :codeFormule AND KHOOPT = :codeOption AND KHOETAPE = '**********'";

            return CommonRepository.ExistRowParam(sql, param);
        }

        public static bool IsGaranSortie_1(DateTime? dateEffetAvt, string dateDeb, int dateFin, int heureFin, int duree, string dureeUnit)
        {
            DateTime? dateSortie = null;
            DateTime? dateDebut = !string.IsNullOrEmpty(dateDeb) ? AlbConvert.ConvertIntToDateHour(Convert.ToInt64(dateDeb)) : null;

            if (dateDebut != null)
            {
                if (duree > 0)
                {
                    dateSortie = AlbConvert.GetFinPeriode(dateDebut, duree, dureeUnit);
                }
            }
            if (dateFin > 0)
            {
                dateSortie = AlbConvert.ConvertIntToDate(dateFin);
                if (heureFin > 0)
                {
                    TimeSpan? heureSortie = AlbConvert.ConvertIntToTime(heureFin);
                    if (dateSortie != null && heureSortie != null)
                    {
                        dateSortie.Value.AddHours(heureSortie.Value.Hours).AddMinutes(heureSortie.Value.Minutes);
                    }
                }
            }

            if (dateEffetAvt != null && dateSortie != null)
            {
                return dateSortie < dateEffetAvt;
            }

            return false;
        }

        public static bool IsGaranSortie(string idGarantie, DateTime? dateEffetAvt, List<DtoCommon> lstObjPortee, DateTime? dateDebRsq, DateTime? dateEffet, int dateFin, int heureFin, int duree, string dureeUnit)
        {
            DateTime? dateSortie = null;
            DateTime? dateDebut = null;

            var objGar = lstObjPortee.FindAll(el => el.Id.ToString() == idGarantie).ToArray();
            if (objGar == null || objGar.Count() == 0)
            {
                dateDebut = AlbConvert.ConvertIntToDateHour(lstObjPortee.Min(el => el.DateDebReturnCol));
            }
            else
            {
                var naturePortee = objGar.FirstOrDefault().StrReturnCol;
                if (naturePortee == "A")
                {
                    dateDebut = AlbConvert.ConvertIntToDateHour(objGar.Min(el => el.DateDebReturnCol));
                }

                if (naturePortee == "E")
                {
                    dateDebut = AlbConvert.ConvertIntToDateHour(lstObjPortee.FindAll(el => el.Id.ToString() != idGarantie).ToArray().Min(el => el.DateDebReturnCol));
                }
            }
            if (dateDebut == null)
            {
                dateDebut = dateDebRsq;
            }

            if (dateDebut == null)
            {
                dateDebut = dateEffet;
            }

            if (dateDebut != null)
            {
                if (duree > 0)
                {
                    dateSortie = AlbConvert.GetFinPeriode(dateDebut, duree, dureeUnit);
                }
            }
            if (dateFin > 0)
            {
                dateSortie = AlbConvert.ConvertIntToDate(dateFin);
                if (heureFin > 0)
                {
                    TimeSpan? heureSortie = AlbConvert.ConvertIntToTime(heureFin);
                    if (dateSortie != null && heureSortie != null)
                    {
                        dateSortie.Value.AddHours(heureSortie.Value.Hours).AddMinutes(heureSortie.Value.Minutes);
                    }
                }
            }

            if (dateEffetAvt != null && dateSortie != null)
            {
                return dateSortie < dateEffetAvt;
            }

            return false;
        }

        public static string GetLettreLibFormuleGarantie(string codeOffre, string version, string type)
        {
            string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };

            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql = @"SELECT MAX(KDAALPHA) LETTRELIB 
					FROM KPFOR
						WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type";

            var firstOrDefault = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();
            if (firstOrDefault != null)
            {
                return letters[Array.IndexOf(letters, firstOrDefault.LettreLib) + 1];
            }

            return string.Empty;
        }
        
        public static FormuleDto GetFormuleGarantieInfo(string codeOffre, string version, string type, string codeAvn, string codeFormule, ModeConsultation modeNavig)
        {
            var param = new List<EacParameter>{
                 new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                 new EacParameter("version", Convert.ToInt32(version)),
                 new EacParameter("type", type),
                 new EacParameter("codeFormule", Convert.ToInt32(codeFormule))
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }

            string sql = string.Format(@"SELECT KDAALPHA LETTRELIB, KDACIBLE CIBLE, KDAKAIID CODECIBLE,KAHDESC DESCCIBLE, KDADESC LIBELLE 
					FROM {0}
					INNER JOIN KCIBLE ON KAHCIBLE = KDACIBLE
						WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KDAFOR = :codeFormule {1}",
                    CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                    modeNavig == ModeConsultation.Historique ? " AND KDAAVN = :avn" : string.Empty);

            return DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        public static string GetFirstRsqObj(string codeOffre, string version, ModeConsultation modeNavig)
        {
            var param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;

            string sql = string.Format("SELECT JGRSQ CODE, JGOBJ CODEOBJET FROM {0}  WHERE JGIPB = :codeOffre AND JGALX = :version",
                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"));
            var result = DbBase.Settings.ExecuteList<RisqueDto>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                var rsqobj = result.FirstOrDefault().Code;

                var resObj = result.FindAll(r => r.Code == rsqobj).ToList();
                var codeObjs = string.Empty;
                resObj.ForEach(m =>
                {
                    codeObjs += "_" + m.CodeObjet;
                });
                return rsqobj + ";" + codeObjs.Substring(1);
            }
            return string.Empty;
        }

        public static string GetAllRsqObj(string codeOffre, string version, string codeRisque, ModeConsultation modeNavig)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("codeRisque", DbType.AnsiStringFixedLength);
            param[2].Value = codeRisque;

            string sql = string.Format("SELECT JGRSQ CODE, JGOBJ CODEOBJET FROM {0} WHERE JGIPB = :codeOffre AND JGALX = :version AND JGRSQ = :codeRisque",
                CommonRepository.GetPrefixeHisto(modeNavig, "YPRTOBJ"));
            var result = DbBase.Settings.ExecuteList<RisqueDto>(CommandType.Text, sql, param);

            if (result != null && result.Any())
            {
                var rsqobj = result.FirstOrDefault().Code;

                var resObj = result.FindAll(r => r.Code == rsqobj).ToList();
                var codeObjs = string.Empty;
                resObj.ForEach(m =>
                {
                    codeObjs += "_" + m.CodeObjet;
                });
                return rsqobj + ";" + codeObjs.Substring(1);
            }
            return string.Empty;
        }

        public static string GetRisqueObjetFormuleString(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            string toReturn = string.Empty;
            string codesObj = string.Empty;

            List<RisqueObjetPlatDto> result = GetRisqueObjetFormule(codeOffre, version, type, codeAvn, codeFormule, codeOption, modeNavig);
            if (result != null && result.Count > 0)
            {
                string codeRsq = result[0].CodeRsq.ToString();
                result.ForEach(el =>
                {
                    if (el.CodeObj > 0)
                    {
                        codesObj += "_" + el.CodeObj;
                    }
                });
                toReturn = !string.IsNullOrEmpty(codesObj) ? string.Format("{0};{1}", codeRsq, codesObj.Substring(1)) : codeRsq;
            }
            return toReturn;
        }


        #endregion

        #region Méthodes Privées

        private static FormGarDto LoadFormulePlat(List<FormuleGarantiePlatDto> result, List<FormuleGarantiePlatDto> resultVBHisto, bool isReadOnly, string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption)
        {
            bool isAvtModif = false;
            DateTime? dateEffetAvtModif = null;
            DateTime? dateEffetAvt = null;

            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[1].Value = type;
            param[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[2].Value = version;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;

            var sql = @"SELECT COUNT(*) NBLIGN FROM KPAVTRC 
										WHERE KHOIPB = :codeOffre AND KHOTYP = :type AND KHOALX = :version AND KHOPERI = 'OPT' 
											AND KHOFOR = :codeFormule AND KHOOPT = :codeOption AND KHOETAPE = '**********'";
            isAvtModif = CommonRepository.ExistRowParam(sql, param);
            if (isAvtModif)
            {
                sql = @"SELECT KDBAVA * 10000 + KDBAVM * 100 + KDBAVJ DATEEFFETAVTMODIF FROM KPOPT WHERE KDBIPB = :codeOffre AND KDBTYP = :type AND KDBALX = :version AND KDBFOR = :codeFormule AND KDBOPT = :codeOption";
                var dateAvn = DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param);
                if (dateAvn != null)
                {
                    dateEffetAvtModif = AlbConvert.ConvertIntToDate(Convert.ToInt32(dateAvn));
                }
            }

            var param2 = new EacParameter[3];
            param2[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param2[0].Value = codeOffre.PadLeft(9, ' ');
            param2[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param2[1].Value = version;
            param2[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param2[2].Value = type;

            sql = @"select PBAVA * 10000 + PBAVM * 100 + PBAVJ DATEEFFETAVTMODIF from ypobase where pbipb = :codeOffre and pbalx = :version and pbtyp = :type";
            var resultDate = DbBase.Settings.ExecuteList<FormuleGarantieDto>(CommandType.Text, sql, param2);
            if (resultDate != null && resultDate.Any())
            {
                dateEffetAvt = AlbConvert.ConvertIntToDate(Convert.ToInt32(resultDate.FirstOrDefault().DateEffetAvtModif));
            }

            bool isModeAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0";
            FormGarDto toReturn = new FormGarDto();

            //TODO clean dead code
            var lstGarIncomp = GetLstGarantieIncomp(result);
            var lstGarAssoc = GetLstGarantieAssoc(result);


            var lstVolet = result.GroupBy(el => el.GuidV).Select(v => v.First()).ToList();

            #region Volet
            var listVolets = new List<DtoVolet>();
            var listVoletsSave = new List<VoletSaveDto>();

            lstVolet.ForEach(vol =>
            {
                #region Bloc
                //préparer laliste de blocs
                var lstBlocs = new List<BlocDto>();
                var lstBlocsSave = new List<BlocSaveDto>();
                var resBlc = result.FindAll(r => r.GuidV == vol.GuidV).GroupBy(el => el.GuidB).Select(b => b.First()).ToList();
                resBlc.ForEach(bl =>
                {
                    #region Modele
                    //préparer les modèles
                    var lstModeles = new List<ModeleDto>();
                    var lstModelesSave = new List<ModeleSaveDto>();
                    var resModel = result.FindAll(r => r.GuidV == bl.GuidV && r.GuidB == bl.GuidB).GroupBy(el => el.GuidM).Select(m => m.First()).ToList();

                    resModel.ForEach(model =>
                    {
                        #region Niveau 1
                        //préparer les niveaux1
                        var lstNiv1 = new List<GarantieModeleNiveau1Dto>();
                        var lstNiv1Save = new List<ModeleNiv1SaveDto>();
                        var resniv1 = result.FindAll(r => r.GuidV == model.GuidV && r.GuidB == model.GuidB && r.GuidM == model.GuidM)
                                        .GroupBy(el => el.CodeNiv1).Select(nv => nv.First()).ToList()
                                        .FindAll(el => el.CodeNiv1 != 0);

                        resniv1.ForEach(nv1 =>
                        {
                            #region Niveau 2
                            //préparer le niveau 2
                            var lstNiv2 = new List<GarantieModeleSousNiveauDto>();
                            var lstNiv2Save = new List<ModeleSousNivSaveDto>();
                            var resniv2 = result.FindAll(r => r.GuidV == nv1.GuidV && r.GuidB == nv1.GuidB && r.GuidM == nv1.GuidM && r.CodeNiv1 == nv1.CodeNiv1)
                                                .GroupBy(el => el.CodeNiv2).Select(nv => nv.First()).ToList()
                                                .FindAll(el => el.CodeNiv2 != 0);

                            resniv2.ForEach(nv2 =>
                            {
                                #region Niveau 3
                                //préparer le niveau 3
                                var lstNiv3 = new List<GarantieModeleSousNiveauDto>();
                                var lstNiv3Save = new List<ModeleSousNivSaveDto>();
                                var resniv3 = result.FindAll(r => r.GuidV == nv2.GuidV && r.GuidB == nv2.GuidB && r.GuidM == nv2.GuidM && r.CodeNiv1 == nv2.CodeNiv1 && r.CodeNiv2 == nv2.CodeNiv2)
                                                .GroupBy(el => el.CodeNiv3).Select(nv => nv.First()).ToList()
                                                .FindAll(el => el.CodeNiv3 != 0);
                                resniv3.ForEach(nv3 =>
                                {

                                    #region Niveau 4
                                    //préparer le niveau 4
                                    var lstNiv4 = new List<GarantieModeleSousNiveauDto>();
                                    var lstNiv4Save = new List<ModeleSousNivSaveDto>();
                                    var resniv4 = result.FindAll(r => r.GuidV == nv3.GuidV && r.GuidB == nv3.GuidB && r.GuidM == nv3.GuidM
                                                            && r.CodeNiv1 == nv3.CodeNiv1 && r.CodeNiv2 == nv3.CodeNiv2 && r.CodeNiv3 == nv3.CodeNiv3)
                                                    .GroupBy(el => el.CodeNiv4).Select(nv => nv.First()).ToList()
                                                    .FindAll(el => el.CodeNiv4 != 0);

                                    resniv4.ForEach(nv4 =>
                                    {
                                        var paramNatModN4 = (nv4.ParamNatModNiv4 == "O" ?
                                                                (!string.IsNullOrEmpty(nv4.NatureParamNiv4) ?
                                                                    nv4.NatureParamNiv4
                                                                    : (nv4.CaracNiv4 == "F" && (nv4.NatureNiv4 == "A" || nv4.NatureNiv4 == "C") ?
                                                                        (nv4.NatureNiv4 == "C" ?
                                                                            nv4.NatureNiv4
                                                                        : string.Empty)
                                                                    : string.Empty))
                                                                : string.Empty);

                                        if ((nv4.NatureParamNiv4 != "E" && nv4.NatureParamNiv4 != "" && isReadOnly) || !isReadOnly)
                                        {
                                            //remplir l'objet Niveau4
                                            lstNiv4.Add(new GarantieModeleSousNiveauDto
                                            {
                                                Code = Convert.ToInt32(nv4.CodeNiv4),
                                                Niveau = 4,
                                                FCTCodeGarantie = nv4.CodeGarNiv4,
                                                CodeGarantie = nv4.GuidNiv4.ToString(),
                                                Description = nv4.DescrNiv4,
                                                Caractere = nv4.CaracNiv4,
                                                Nature = nv4.NatureNiv4,
                                                CodeParent = Convert.ToInt32(nv4.CodeParentNiv4),
                                                CodeNiv1 = Convert.ToInt32(nv4.CodeNiv1Niv4),
                                                GuidGarantie = nv4.CodeNiv4.ToString(),
                                                AppliqueA = nv4.AppliqueA != 0,
                                                NatureParam = nv4.NatureParamNiv4,
                                                FlagModif = nv4.FlagModifNiv4,
                                                ParamNatMod = nv4.ParamNatModNiv4,
                                                ParamNatModVal = paramNatModN4,
                                                InvenPossible = nv4.InvenPossible4,
                                                CodeInven = nv4.CodeInven4,
                                                TypeInven = nv4.TypeInven4,
                                                CreateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv4.CreateAvt4 != 0 && nv4.CreateAvt4 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                                UpdateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv4.MajAvt4 != 0 && nv4.MajAvt4 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                                ModeAvt = isModeAvt,
                                                IsReadOnly = isReadOnly,
                                                DateEffetAvtModifLocale = dateEffetAvtModif,
                                                IsGarantieSortie = isModeAvt ? IsGaranSortie_1(dateEffetAvt, nv4.DateDeb4, Convert.ToInt32(nv4.DateFin4), Convert.ToInt32(nv4.HeureFin4), Convert.ToInt32(nv4.Duree4), nv4.DureeUnite4) : false
                                            });

                                            lstNiv4Save.Add(new ModeleSousNivSaveDto
                                            {
                                                Caractere = nv4.CaracNiv4,
                                                GuidGarantie = nv4.CodeNiv4.ToString(),
                                                Nature = nv4.NatureNiv4,
                                                NatureParam = nv4.NatureParamNiv4,
                                                ParamNatMod = nv4.ParamNatModNiv4
                                            });
                                        }
                                    });
                                    #endregion
                                    var paramNatModN3 = (nv3.ParamNatModNiv3 == "O" ?
                                                            (!string.IsNullOrEmpty(nv3.NatureParamNiv3) ?
                                                                nv3.NatureParamNiv3
                                                                : (nv3.CaracNiv3 == "F" && (nv3.NatureNiv3 == "A" || nv3.NatureNiv3 == "C") ?
                                                                    (nv3.NatureNiv3 == "C" ?
                                                                        nv3.NatureNiv3
                                                                    : string.Empty)
                                                                : string.Empty))
                                                            : string.Empty);

                                    if ((nv3.NatureParamNiv3 != "E" && nv3.NatureParamNiv3 != "" && isReadOnly) || !isReadOnly)
                                    {
                                        //remplir l'objet Niveau3
                                        lstNiv3.Add(new GarantieModeleSousNiveauDto
                                        {
                                            Code = Convert.ToInt32(nv3.CodeNiv3),
                                            Niveau = 3,
                                            FCTCodeGarantie = nv3.CodeGarNiv3,
                                            CodeGarantie = nv3.GuidNiv3.ToString(),
                                            Description = nv3.DescrNiv3,
                                            Caractere = nv3.CaracNiv3,
                                            Nature = nv3.NatureNiv3,
                                            CodeParent = Convert.ToInt32(nv3.CodeParentNiv3),
                                            CodeNiv1 = Convert.ToInt32(nv3.CodeNiv1Niv3),
                                            GuidGarantie = nv3.CodeNiv3.ToString(),
                                            AppliqueA = nv3.AppliqueA != 0,
                                            NatureParam = nv3.NatureParamNiv3,
                                            Modeles = lstNiv4,
                                            FlagModif = nv3.FlagModifNiv3,
                                            ParamNatMod = nv3.ParamNatModNiv3,
                                            ParamNatModVal = paramNatModN3,
                                            InvenPossible = nv3.InvenPossible3,
                                            CodeInven = nv3.CodeInven3,
                                            TypeInven = nv3.TypeInven3,
                                            CreateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv3.CreateAvt3 != 0 && nv3.CreateAvt3 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                            UpdateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv3.MajAvt3 != 0 && nv3.MajAvt3 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                            ModeAvt = isModeAvt,
                                            IsReadOnly = isReadOnly,
                                            DateEffetAvtModifLocale = dateEffetAvtModif,
                                            IsGarantieSortie = isModeAvt ? IsGaranSortie_1(dateEffetAvt, nv3.DateDeb3, Convert.ToInt32(nv3.DateFin3), Convert.ToInt32(nv3.HeureFin3), Convert.ToInt32(nv3.Duree3), nv3.DureeUnite3) : false
                                        });

                                        lstNiv3Save.Add(new ModeleSousNivSaveDto
                                        {
                                            Caractere = nv3.CaracNiv3,
                                            GuidGarantie = nv3.CodeNiv3.ToString(),
                                            Nature = nv3.NatureNiv3,
                                            NatureParam = nv3.NatureParamNiv3,
                                            Modeles = lstNiv4Save,
                                            ParamNatMod = nv3.ParamNatModNiv3
                                        });
                                    }
                                });
                                #endregion

                                //Correction bug 1234 2015-01-09
                                var paramNatModN2 = (nv2.ParamNatModNiv2 == "O" ?
                                                        (!string.IsNullOrEmpty(nv2.NatureParamNiv2) ?
                                                            nv2.NatureParamNiv2
                                                            : (nv2.CaracNiv2 == "F" && (nv2.NatureNiv2 == "A" || nv2.NatureNiv2 == "C") ?
                                                                (nv2.NatureNiv2 == "C" ?
                                                                    nv2.NatureNiv2
                                                                : string.Empty)
                                                            : string.Empty))
                                                        : string.Empty);

                                if ((nv2.NatureParamNiv2 != "E" && nv2.NatureParamNiv2 != "" && isReadOnly) || !isReadOnly)
                                {
                                    //remplir l'objet Niveau2
                                    lstNiv2.Add(new GarantieModeleSousNiveauDto
                                    {
                                        Code = Convert.ToInt32(nv2.CodeNiv2),
                                        Niveau = 2,
                                        FCTCodeGarantie = nv2.CodeGarNiv2,
                                        CodeGarantie = nv2.GuidNiv2.ToString(),
                                        Description = nv2.DescrNiv2,
                                        Caractere = nv2.CaracNiv2,
                                        Nature = nv2.NatureNiv2,
                                        CodeParent = Convert.ToInt32(nv2.CodeParentNiv2),
                                        CodeNiv1 = Convert.ToInt32(nv2.CodeNiv1Niv2),
                                        GuidGarantie = nv2.CodeNiv2.ToString(),
                                        AppliqueA = nv2.AppliqueA != 0,
                                        NatureParam = nv2.NatureParamNiv2,
                                        Modeles = lstNiv3,
                                        FlagModif = nv2.FlagModifNiv2,
                                        ParamNatMod = nv2.ParamNatModNiv2,
                                        ParamNatModVal = paramNatModN2,
                                        InvenPossible = nv2.InvenPossible2,
                                        CodeInven = nv2.CodeInven2,
                                        TypeInven = nv2.TypeInven2,
                                        CreateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv2.CreateAvt2 != 0 && nv2.CreateAvt2 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                        UpdateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv2.MajAvt2 != 0 && nv2.MajAvt2 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                        ModeAvt = isModeAvt,
                                        IsReadOnly = isReadOnly,
                                        DateEffetAvtModifLocale = dateEffetAvtModif,
                                        IsGarantieSortie = isModeAvt ? IsGaranSortie_1(dateEffetAvt, nv2.DateDeb2, Convert.ToInt32(nv2.DateFin2), Convert.ToInt32(nv2.HeureFin2), Convert.ToInt32(nv2.Duree2), nv2.DureeUnite2) : false
                                    });

                                    lstNiv2Save.Add(new ModeleSousNivSaveDto
                                    {
                                        Caractere = nv2.CaracNiv2,
                                        GuidGarantie = nv2.CodeNiv2.ToString(),
                                        Nature = nv2.NatureNiv2,
                                        NatureParam = nv2.NatureParamNiv2,
                                        Modeles = lstNiv3Save,
                                        ParamNatMod = nv2.ParamNatModNiv2
                                    });
                                }
                            });
                            #endregion
                            var paramNatModN1 = (nv1.ParamNatModNiv1 == "O" ?
                                                    (!string.IsNullOrEmpty(nv1.NatureParamNiv1) ?
                                                        nv1.NatureParamNiv1
                                                        : (nv1.CaracNiv1 == "F" && (nv1.NatureNiv1 == "A" || nv1.NatureNiv1 == "C") ?
                                                            (nv1.NatureNiv1 == "C" ?
                                                                nv1.NatureNiv1
                                                            : string.Empty)
                                                        : string.Empty))
                                                    : string.Empty);

                            if ((nv1.NatureParamNiv1 != "E" && nv1.NatureParamNiv1 != "" && isReadOnly) || !isReadOnly)
                            {
                                //remplir l'objet Niveau1
                                lstNiv1.Add(new GarantieModeleNiveau1Dto
                                {
                                    Code = Convert.ToInt32(nv1.CodeNiv1),
                                    Niveau = 1,
                                    FCTCodeGarantie = nv1.CodeGarNiv1,
                                    CodeGarantie = nv1.GuidNiv1.ToString(),
                                    Description = nv1.DescrNiv1,
                                    Caractere = nv1.CaracNiv1,
                                    Nature = nv1.NatureNiv1,
                                    CodeParent = Convert.ToInt32(nv1.CodeParentNiv1),
                                    CodeNiv1 = Convert.ToInt32(nv1.CodeNiv1Niv1),
                                    GuidGarantie = nv1.CodeNiv1.ToString(),
                                    AppliqueA = nv1.AppliqueA != 0,
                                    NatureParam = nv1.NatureParamNiv1,
                                    Modeles = lstNiv2,
                                    FlagModif = nv1.FlagModifNiv1,
                                    ParamNatMod = nv1.ParamNatModNiv1,
                                    ParamNatModVal = paramNatModN1,
                                    Action = nv1.ActionNiv1,
                                    InvenPossible = nv1.InvenPossible1,
                                    CodeInven = nv1.CodeInven1,
                                    TypeInven = nv1.TypeInven1,
                                    CreateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv1.CreateAvt1 != 0 && nv1.CreateAvt1 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                    UpdateInAvt = !string.IsNullOrEmpty(codeAvn) && codeAvn != "0" ? nv1.MajAvt1 != 0 && nv1.MajAvt1 == (!string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : false,
                                    ModeAvt = isModeAvt,
                                    IsReadOnly = isReadOnly,
                                    DateEffetAvtModifLocale = dateEffetAvtModif,
                                    IsGarantieSortie = isModeAvt ? IsGaranSortie_1(dateEffetAvt, nv1.DateDeb1, Convert.ToInt32(nv1.DateFin1), Convert.ToInt32(nv1.HeureFin1), Convert.ToInt32(nv1.Duree1), nv1.DureeUnite1) : false,
                                    AlimAssiette = nv1.AlimNiv1
                                });

                                lstNiv1Save.Add(new ModeleNiv1SaveDto
                                {
                                    Caractere = nv1.CaracNiv1,
                                    GuidGarantie = nv1.CodeNiv1.ToString(),
                                    Nature = nv1.NatureNiv1,
                                    NatureParam = nv1.NatureParamNiv1,
                                    Modeles = lstNiv2Save,
                                    ParamNatMod = nv1.ParamNatModNiv1,
                                    Action = nv1.ActionNiv1
                                });

                            }
                        });
                        #endregion
                        if ((isReadOnly && lstNiv1.Count > 0) || (!isReadOnly))
                        {
                            lstModeles.Add(new ModeleDto
                            {
                                Code = model.CodeModele,
                                Description = model.DescrModele,
                                GuidId = model.GuidM.ToString(),
                                Modeles = lstNiv1
                            });

                            lstModelesSave.Add(new ModeleSaveDto
                            {
                                GuidId = model.GuidM.ToString(),
                                Modeles = lstNiv1Save
                            });
                        }
                    });
                    #endregion
                    if ((bl.isCheckedB != 0 && isReadOnly && lstModeles.Count > 0) || (!isReadOnly))
                    {
                        //remplir l'objet bloc
                        lstBlocs.Add(new BlocDto
                        {
                            Code = bl.CodeBloc,
                            Description = bl.DescrBloc,
                            Caractere = bl.CaracBloc,
                            GuidId = bl.GuidB,
                            isChecked = bl.isCheckedB != 0,
                            CodeOption = bl.GuidOption.ToString(),
                            Modeles = lstModeles,
                            ModifAvt = resultVBHisto != null && resultVBHisto.Any() ? bl.isCheckedB != 0 && !resultVBHisto.Exists(el => el.GuidVolet == bl.GuidVolet && el.GuidBloc == bl.GuidBloc)
                                    || bl.isCheckedB == 0 && resultVBHisto.Exists(el => el.GuidVolet == bl.GuidVolet && el.GuidBloc == bl.GuidBloc) : false,
                            IsReadOnly = isReadOnly,
                            ModeAvt = isModeAvt
                        });

                        lstBlocsSave.Add(new BlocSaveDto
                        {
                            GuidId = bl.GuidB,
                            isChecked = bl.isCheckedB != 0,
                            Modeles = lstModelesSave
                        });
                    }
                });
                #endregion
                if ((vol.isCheckedV != 0 && isReadOnly && lstBlocs.Count > 0) || (!isReadOnly))
                {
                    //remplir l'objet volets
                    listVolets.Add(new DtoVolet
                    {
                        Code = vol.CodeVolet,
                        Description = vol.DescrVolet,
                        Caractere = vol.CaracVolet,
                        GuidId = vol.GuidV,
                        isChecked = vol.isCheckedV != 0,
                        IsVoletCollapse = vol.VoletCollapse,
                        CodeOption = vol.GuidOption.ToString(),
                        Blocs = lstBlocs,
                        ModifAvt = resultVBHisto != null && resultVBHisto.Any() ? vol.isCheckedV != 0 && !resultVBHisto.Exists(el => el.GuidVolet == vol.GuidVolet)
                                || vol.isCheckedV == 0 && resultVBHisto.Exists(el => el.GuidVolet == vol.GuidVolet) : false,
                        IsReadOnly = isReadOnly,
                        ModeAvt = isModeAvt
                    });

                    listVoletsSave.Add(new VoletSaveDto
                    {
                        GuidId = vol.GuidV,
                        isChecked = vol.isCheckedV != 0,
                        Blocs = lstBlocsSave
                    });
                }

            });
            #endregion

            var formuleGaranties = new FormuleGarantieDto();
            var formuleGarantieSave = new FormuleGarantieSaveDto();

            formuleGaranties.Volets = listVolets;
            formuleGarantieSave.Volets = listVoletsSave;

            toReturn = new FormGarDto
            {
                FormuleGarantie = formuleGaranties,
                FormuleGarantieSave = formuleGarantieSave
            };

            toReturn.FormuleGarantie.IsAvenantModificationLocale = isAvtModif;
            toReturn.FormuleGarantie.DateEffetAvenantModificationLocale = dateEffetAvtModif;


            return toReturn;
        }

        /// <summary>
        /// Sauvegarde l'option (volet/bloc) dans la table KPOPTDW de la BDD
        /// </summary>
        /// <param name="guidOption"></param>
        /// <param name="isChecked"></param>
        private static void UpdateOptionFormuleGarantie(string guidOption, bool isChecked, string typeOption)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("P_GUIDOPTION", DbType.Int32);
            param[0].Value = Convert.ToInt32(guidOption);
            param[1] = new EacParameter("P_ISCHECKED", DbType.Int32);
            param[1].Value = isChecked ? 1 : 0;
            param[2] = new EacParameter("P_TYPEOPTION", DbType.AnsiStringFixedLength);
            param[2].Value = !string.IsNullOrEmpty(typeOption) ? typeOption : string.Empty;

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SVOPTIO", param);
        }

        /// <summary>
        /// Sauvegarde la garantie dans la table KPGARAW de la BDD
        /// </summary>
        /// <param name="guidGarantie"></param>
        /// <param name="natureParam"></param>
        private static void UpdateGarantieFormuleGarantie(string codeAvenant, string guidGarantie, string natureParam)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("P_CODEAVENANT", DbType.Int32);
            param[0].Value = !string.IsNullOrEmpty(codeAvenant) ? Convert.ToInt32(codeAvenant) : 0;
            param[1] = new EacParameter("P_GUIDGARANTIE", DbType.Int32);
            param[1].Value = Convert.ToInt32(guidGarantie);
            param[2] = new EacParameter("P_NATUREPARAM", DbType.AnsiStringFixedLength);
            param[2].Value = natureParam;
            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SVGARAW", param);
        }

        private static string UpdateFormule(string codeOffre, string version, string type, string codeFormule, string codeCible, string libFormule, string user)
        {
            var param = new EacParameter[9];
            param[0] = new EacParameter("cible", DbType.AnsiStringFixedLength);
            param[0].Value = CibleRepository.GetLibCibleF(codeCible);
            param[1] = new EacParameter("codeCible", DbType.Int32);
            param[1].Value = Convert.ToInt32(codeCible);
            param[2] = new EacParameter("libFormule", DbType.AnsiStringFixedLength);
            param[2].Value = !string.IsNullOrEmpty(libFormule) ? libFormule : string.Empty;
            param[3] = new EacParameter("updateUser", DbType.AnsiStringFixedLength);
            param[3].Value = user;
            param[4] = new EacParameter("updateDate", DbType.Int32);
            param[4].Value = Convert.ToInt32(AlbConvert.ConvertDateToInt(DateTime.Now));
            param[5] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[5].Value = type;
            param[6] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[6].Value = codeOffre.PadLeft(9, ' ');
            param[7] = new EacParameter("version", DbType.Int32);
            param[7].Value = Convert.ToInt32(version);
            param[8] = new EacParameter("codeFormule", DbType.Int32);
            param[8].Value = Convert.ToInt32(codeFormule);

            string sql = @"UPDATE KPFOR SET KDACIBLE = :cible, KDAKAIID = :codeCible, KDADESC = :libFormule, KDAMAJU = :updateUser, KDAMAJD = :updateDate
						WHERE KDATYP = :type AND KDAIPB = :codeOffre AND KDAALX = :version AND KDAFOR = :codeFormule";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            return codeFormule;
        }

        private static string InsertFormule(string codeOffre, string version, string type, string avenant, string codeFormule, string formGen, string codeAlpha, string branche, string codeCible, string libFormule, string user)
        {
            if (formGen == "0")
            {
                codeAlpha = GetLettreLibFormuleGarantie(codeOffre, version, type);// GetAlpha(codeOffre, version, type, codeAlpha);
            }
            else
            {
                codeAlpha = string.Empty;
            }

            codeFormule = GetNextCodeFormule(codeOffre, version, type);
            int? date = AlbConvert.ConvertDateToInt(DateTime.Now);
            int guidFormule = CommonRepository.GetAS400Id("KDAID");

            EacParameter[] param = new EacParameter[16];
            param[0] = new EacParameter("codeId", DbType.Int32);
            param[0].Value = guidFormule;
            param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[1].Value = type;
            param[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[2].Value = codeOffre.PadLeft(9, ' ');
            param[3] = new EacParameter("version", DbType.Int32);
            param[3].Value = Convert.ToInt32(version);
            param[4] = new EacParameter("codeFormule", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeFormule);
            param[5] = new EacParameter("numChrono", DbType.Int32);
            param[5].Value = Convert.ToInt32(codeFormule);
            param[6] = new EacParameter("codeAlpha", DbType.AnsiStringFixedLength);
            param[6].Value = codeAlpha;
            param[7] = new EacParameter("branche", DbType.AnsiStringFixedLength);
            param[7].Value = branche;
            param[8] = new EacParameter("cible", DbType.AnsiStringFixedLength);
            param[8].Value = CibleRepository.GetLibCibleF(codeCible);
            param[9] = new EacParameter("codeCible", DbType.Int32);
            param[9].Value = Convert.ToInt32(codeCible);
            param[10] = new EacParameter("libFormule", DbType.AnsiStringFixedLength);
            param[10].Value = !string.IsNullOrEmpty(libFormule) ? libFormule : string.Empty;
            param[11] = new EacParameter("createUser", DbType.AnsiStringFixedLength);
            param[11].Value = user;
            param[12] = new EacParameter("createDate", DbType.Int32);
            param[12].Value = date;
            param[13] = new EacParameter("updateUser", DbType.AnsiStringFixedLength);
            param[13].Value = user;
            param[14] = new EacParameter("updateDate", DbType.Int32);
            param[14].Value = date;
            param[15] = new EacParameter("formGen", DbType.AnsiStringFixedLength);
            param[15].Value = formGen == "1" ? "O" : "N";

            string sql = @"INSERT INTO KPFOR (KDAID, KDATYP, KDAIPB, KDAALX, KDAFOR, KDACCH, KDAALPHA, KDABRA, KDACIBLE, KDAKAIID, KDADESC, KDACRU, KDACRD, KDAMAJU, KDAMAJD, KDAFGEN) 
						VALUES (:codeId, :type, :codeOffre, :version, :codeFormule, :numChrono, :codeAlpha, :branche, :cible, :codeCible, :libFormule, :createUser, :creationdate, :updateuser, :updatedate, :formGen)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            InsertOption(codeOffre, version, type, avenant, codeFormule, guidFormule, user);

            return string.Format("{0}_{1}", codeFormule, codeAlpha);
        }

        private static void InsertOption(string codeOffre, string version, string type, string avenant, string codeFormule, int guidFormule, string user)
        {
            int? date = AlbConvert.ConvertDateToInt(DateTime.Now);

            EacParameter[] param = new EacParameter[12];
            param[0] = new EacParameter("codeId", DbType.Int32);
            param[0].Value = CommonRepository.GetAS400Id("KDBID");
            param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[1].Value = type;
            param[2] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[2].Value = codeOffre.PadLeft(9, ' ');
            param[3] = new EacParameter("version", DbType.Int32);
            param[3].Value = Convert.ToInt32(version);
            param[4] = new EacParameter("codeFormule", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeFormule);
            param[5] = new EacParameter("guidFormule", DbType.Int32);
            param[5].Value = guidFormule;
            param[6] = new EacParameter("codeOption", DbType.Int32);
            param[6].Value = 1;
            param[7] = new EacParameter("createUser", DbType.AnsiStringFixedLength);
            param[7].Value = user;
            param[8] = new EacParameter("createDate", DbType.Int32);
            param[8].Value = date;
            param[9] = new EacParameter("updateUser", DbType.AnsiStringFixedLength);
            param[9].Value = user;
            param[10] = new EacParameter("updateDate", DbType.Int32);
            param[10].Value = date;
            param[11] = new EacParameter("avenant", DbType.Int32);
            param[11].Value = !string.IsNullOrEmpty(avenant) ? Convert.ToInt32(avenant) : 0;

            string sql = @"INSERT INTO KPOPT (KDBID, KDBTYP, KDBIPB, KDBALX, KDBFOR, KDBKDAID, KDBOPT, KDBCRU, KDBCRD, KDBMAJU, KDBMAJD, KDBAVE) 
						VALUES (:codeId, :type, :codeOffre, :version, :codeFormule, :guidFormule, :codeOption, :createUser, :creationdate, :updateuser, :updatedate, :avenant)";

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static string GetNextCodeFormule(string codeOffre, string version, string type)
        {


            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql = @"SELECT IFNULL(MAX(KDAFOR), 0) + 1 AS CODE
					FROM KPFOR
						WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type";

            var firstOrDefault = DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param).FirstOrDefault();
            return firstOrDefault != null ? firstOrDefault.Code.ToString() : string.Empty;
        }

        private static string GetCodeFormuleRsq(string codeOffre, string version, string type, string codeRsq)
        {
            var param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeRsq", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeRsq);

            string sql = @"SELECT DISTINCT KDDFOR CODE 
					FROM KPOPTAP 
						WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type AND KDDRSQ = :codeRsq";

            var firstOrDefault = DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (firstOrDefault != null)
            {
                return firstOrDefault.Code.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gestion des garanties incompatibles
        /// </summary>
        private static List<RelationGarantieDto> GetLstGarantieIncomp(List<FormuleGarantiePlatDto> result)
        {
            var lstGarIncomp = new List<RelationGarantieDto>();

            //---------Garantie Incompatible Niveau 1
            var lstGarIncompNiv1 = result.GroupBy(el => new { el.CodeNiv1, el.CodeGarIncompNiv1 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarIncompNiv1 != 0);
            lstGarIncompNiv1.ForEach(el =>
            {
                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeNiv1 && g.CodeGarantie2 == el.CodeGarIncompNiv1).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv1, CodeGarantie2 = el.CodeGarIncompNiv1 });
                }

                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeGarIncompNiv1 && g.CodeGarantie2 == el.CodeNiv1).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarIncompNiv1, CodeGarantie2 = el.CodeNiv1 });
                }
            });

            //---------Garantie Incompatible Niveau 2
            var lstGarIncompNiv2 = result.GroupBy(el => new { el.CodeNiv2, el.CodeGarIncompNiv2 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarIncompNiv2 != 0);
            lstGarIncompNiv2.ForEach(el =>
            {
                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeNiv2 && g.CodeGarantie2 == el.CodeGarIncompNiv2).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv2, CodeGarantie2 = el.CodeGarIncompNiv2 });
                }

                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeGarIncompNiv2 && g.CodeGarantie2 == el.CodeNiv2).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarIncompNiv2, CodeGarantie2 = el.CodeNiv2 });
                }
            });

            //---------Garantie Incompatible Niveau 3
            var lstGarIncompNiv3 = result.GroupBy(el => new { el.CodeNiv3, el.CodeGarIncompNiv3 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarIncompNiv3 != 0);
            lstGarIncompNiv3.ForEach(el =>
            {
                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeNiv3 && g.CodeGarantie2 == el.CodeGarIncompNiv3).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv3, CodeGarantie2 = el.CodeGarIncompNiv3 });
                }

                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeGarIncompNiv3 && g.CodeGarantie2 == el.CodeNiv3).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarIncompNiv3, CodeGarantie2 = el.CodeNiv3 });
                }
            });

            //---------Garantie Incompatible Niveau 4
            var lstGarIncompNiv4 = result.GroupBy(el => new { el.CodeNiv4, el.CodeGarIncompNiv4 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarIncompNiv4 != 0);
            lstGarIncompNiv4.ForEach(el =>
            {
                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeNiv4 && g.CodeGarantie2 == el.CodeGarIncompNiv4).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv4, CodeGarantie2 = el.CodeGarIncompNiv4 });
                }

                if (lstGarIncomp.FindAll(g => g.CodeGarantie1 == el.CodeGarIncompNiv4 && g.CodeGarantie2 == el.CodeNiv4).Count() == 0)
                {
                    lstGarIncomp.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarIncompNiv4, CodeGarantie2 = el.CodeNiv4 });
                }
            });

            return lstGarIncomp;
        }

        /// <summary>
        /// Gestion des garanties associées
        /// </summary>
        private static List<RelationGarantieDto> GetLstGarantieAssoc(List<FormuleGarantiePlatDto> result)
        {
            var lstGarAssoc = new List<RelationGarantieDto>();

            //---------Garantie Incompatible Niveau 1
            var lstGarAssocNiv1 = result.GroupBy(el => new { el.CodeNiv1, el.CodeGarAssocNiv1 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarAssocNiv1 != 0);
            lstGarAssocNiv1.ForEach(el =>
            {
                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeNiv1 && g.CodeGarantie2 == el.CodeGarAssocNiv1).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv1, CodeGarantie2 = el.CodeGarAssocNiv1 });
                }

                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeGarAssocNiv1 && g.CodeGarantie2 == el.CodeNiv1).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarAssocNiv1, CodeGarantie2 = el.CodeNiv1 });
                }
            });

            //---------Garantie Incompatible Niveau 2
            var lstGarAssocNiv2 = result.GroupBy(el => new { el.CodeNiv2, el.CodeGarAssocNiv2 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarAssocNiv2 != 0);
            lstGarAssocNiv2.ForEach(el =>
            {
                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeNiv2 && g.CodeGarantie2 == el.CodeGarAssocNiv2).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv2, CodeGarantie2 = el.CodeGarAssocNiv2 });
                }

                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeGarAssocNiv2 && g.CodeGarantie2 == el.CodeNiv2).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarAssocNiv2, CodeGarantie2 = el.CodeNiv2 });
                }
            });

            //---------Garantie Incompatible Niveau 3
            var lstGarAssocNiv3 = result.GroupBy(el => new { el.CodeNiv3, el.CodeGarAssocNiv3 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarAssocNiv3 != 0);
            lstGarAssocNiv3.ForEach(el =>
            {
                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeNiv3 && g.CodeGarantie2 == el.CodeGarAssocNiv3).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv3, CodeGarantie2 = el.CodeGarAssocNiv3 });
                }

                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeGarAssocNiv3 && g.CodeGarantie2 == el.CodeNiv3).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarAssocNiv3, CodeGarantie2 = el.CodeNiv3 });
                }
            });

            //---------Garantie Incompatible Niveau 4
            var lstGarAssocNiv4 = result.GroupBy(el => new { el.CodeNiv4, el.CodeGarAssocNiv4 }).Select(g => g.First()).ToList().FindAll(el => el.CodeGarAssocNiv4 != 0);
            lstGarAssocNiv4.ForEach(el =>
            {
                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeNiv4 && g.CodeGarantie2 == el.CodeGarAssocNiv4).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeNiv4, CodeGarantie2 = el.CodeGarAssocNiv4 });
                }

                if (lstGarAssoc.FindAll(g => g.CodeGarantie1 == el.CodeGarAssocNiv4 && g.CodeGarantie2 == el.CodeNiv4).Count() == 0)
                {
                    lstGarAssoc.Add(new RelationGarantieDto { CodeGarantie1 = el.CodeGarAssocNiv4, CodeGarantie2 = el.CodeNiv4 });
                }
            });

            return lstGarAssoc;
        }

        /// <summary>
        /// Récupération des garanties reliées
        /// pour une garantie donnée
        /// </summary>
        private static string GetGarantiesReliees(List<RelationGarantieDto> lstGar, long codeNiveau)
        {
            string garanties = string.Empty;
            var lstGaranties = lstGar.FindAll(g => g.CodeGarantie1 == codeNiveau);
            if (lstGaranties != null && lstGaranties.Count > 0)
            {
                lstGaranties.ForEach(el =>
                {
                    garanties += "||" + el.CodeGarantie2;
                });
            }

            if (!string.IsNullOrEmpty(garanties))
            {
                garanties = garanties.Substring(2);
            }

            return garanties;
        }

        private static FormuleDto GetCibleOffre(string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            var param = new List<EacParameter>() {
                new EacParameter("codeOffre", codeOffre.PadLeft(9, ' ')),
                new EacParameter("version", Convert.ToInt32(version)),
                new EacParameter("type", type)
            };
            if (modeNavig == ModeConsultation.Historique)
            {
                param.Add(new EacParameter("avn", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0));
            }
            string sql = string.Format(@"SELECT KAIID CODECIBLE, KAICIBLE CIBLE, KAHDESC DESCCIBLE
								FROM {0} 
									INNER JOIN KCIBLEF ON KAACIBLE = KAICIBLE
									INNER JOIN KCIBLE ON KAICIBLE = KAHCIBLE
								WHERE KAAIPB = :codeOffre AND KAAALX = :version AND KAATYP = :type {1}",
                            CommonRepository.GetPrefixeHisto(modeNavig, "KPENT"),
                            modeNavig == ModeConsultation.Historique ? " AND KAAAVN = :avn" : string.Empty);

            return DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param).FirstOrDefault();
        }

        private static List<FormuleGarantiePorteePlatDto> GetInfoPorteeGarantie(string codeOffre, string version, string type, string codeFormule, string codeOption, string codeGarantie, string codeObjetRisque, ModeConsultation modeNavig)
        {
            var garExists = GetCodeGarantieBySeq(codeGarantie, codeOffre, version, type, codeFormule, codeOption, modeNavig) != 0;
            var sql = string.Empty;
            List<FormuleGarantiePorteePlatDto> result = null;

            if (garExists)
            {
                var param = new EacParameter[6];
                param[0] = new EacParameter("codeGarantie", DbType.Int32);
                param[0].Value = Convert.ToInt32(codeGarantie);
                param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[1].Value = codeOffre.PadLeft(9, ' ');
                param[2] = new EacParameter("version", DbType.Int32);
                param[2].Value = Convert.ToInt32(version);
                param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[3].Value = type;
                param[4] = new EacParameter("codeFormule", DbType.Int32);
                param[4].Value = Convert.ToInt32(codeFormule);
                param[5] = new EacParameter("codeOption", DbType.Int32);
                param[5].Value = Convert.ToInt32(codeOption);

                sql = @"SELECT KABRSQ CODERSQ, KABDESC LIBRSQ, KACOBJ CODEOBJ, KACDESC LIBOBJ, KDFGAN ACTION, KDEID IDGAR, KDESEQ SEQGAR, KDEGARAN CODEGAR, GADES LIBGAR,
									KDEALA REPORTCAL,
									JGVAL VALEUROBJ, JGVAU UNITOBJ, JGVAT TYPEOBJ, IFNULL(KDFID, 0) IDPORTEE,
									IFNULL(KDFPRA, 0) VALPORTEEOBJ, IFNULL(KDFPRU, '') UNITPORTEEOBJ, IFNULL(KDFTYC, '') TYPEPORTEECAL, IFNULL(KDFMNT, 0) PRIMEMNTCAL,
									(JGVFA * 10000 + JGVFM * 100 + JGVFJ) DATESORTIEOBJ
								FROM KPOPTAP
									INNER JOIN KPRSQ ON KDDIPB = KABIPB AND KDDALX = KABALX AND KDDTYP = KABTYP AND KDDRSQ = KABRSQ
									INNER JOIN KPOBJ ON KDDIPB = KACIPB AND KDDALX = KACALX AND KDDTYP = KACTYP AND KDDRSQ = KACRSQ";
                if (GetPerimetreApplique(codeOffre, version, type, codeFormule, codeOption))
                {
                    sql += " AND KDDOBJ = KACOBJ";
                }

                sql += @" INNER JOIN YPRTOBJ ON JGIPB = KACIPB AND JGALX = KACALX AND JGRSQ = KACRSQ AND JGOBJ = KACOBJ
									INNER JOIN KPGARAN ON KDEIPB = KDDIPB AND KDEALX = KDDALX AND KDETYP = KDDTYP AND KDEFOR = KDDFOR AND KDEOPT = KDDOPT AND KDESEQ = :codeGarantie
									INNER JOIN KGARAN ON GAGAR = KDEGARAN
									LEFT JOIN KPGARAP ON KDFIPB = KDDIPB AND KDFALX = KDDALX AND KDFTYP = KDDTYP AND KDFFOR = KDDFOR AND KDFOPT = KDDOPT AND KDFRSQ = KABRSQ AND KDFOBJ = KACOBJ AND KDFKDEID = KDEID
								WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type AND KDDFOR = :codeFormule AND KDDOPT = :codeOption
								ORDER BY KDDOBJ";
                result = DbBase.Settings.ExecuteList<FormuleGarantiePorteePlatDto>(CommandType.Text, sql, param);
            }
            else
            {
                var splits = codeObjetRisque.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var codeRsq = splits[0];
                var codeObjs = string.Join(",", splits[1].Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries));

                var param = new EacParameter[4];
                param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param[0].Value = codeOffre.PadLeft(9, ' ');
                param[1] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param[1].Value = type;
                param[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
                param[2].Value = version;
                param[3] = new EacParameter("codeRsq", DbType.AnsiStringFixedLength);
                param[3].Value = codeRsq;

                sql = @"SELECT KABRSQ CODERSQ, KABDESC LIBRSQ,
											KACOBJ CODEOBJ, KACDESC LIBOBJ,
											JGVAL VALEUROBJ, JGVAU UNITOBJ, JGVAT TYPEOBJ,
											(JGVFA * 10000 + JGVFM * 100 + JGVFJ) DATESORTIEOBJ
								FROM KPRSQ 
										INNER JOIN KPOBJ  ON KACIPB = KABIPB AND KACTYP =KABTYP AND KACALX =KABALX  AND  KACRSQ = KABRSQ
										INNER JOIN YPRTOBJ ON JGIPB = KACIPB AND JGALX = KACALX AND JGRSQ = KACRSQ AND JGOBJ = KACOBJ
								WHERE KABIPB = :codeOffre AND KABTYP = :type AND KABALX = :version AND KACRSQ = :codeRsq AND KACOBJ IN (" + codeObjs + ") ORDER BY KACOBJ";

                result = DbBase.Settings.ExecuteList<FormuleGarantiePorteePlatDto>(CommandType.Text, sql, param);

                //TODO MODIF

                var brancheCible = CommonRepository.GetBrancheCibleFormule(codeOffre, version, type, string.Empty, codeFormule, ModeConsultation.Standard);
                var codeCible = (int)brancheCible.Cible.GuidId;
                var branche = brancheCible.Code;



                var paramsVoletBlocs = LoadVoletBlocParameters(codeOffre, Convert.ToInt32(version), type, codeCible, branche);
                var paramsGaranties = LoadParameters(paramsVoletBlocs.Select(i => i.LibModele.Trim()).Distinct().ToList());
                var g = paramsGaranties.FirstOrDefault(i => i.C2SEQNIV.ToString(CultureInfo.InvariantCulture) == codeGarantie);
                result.ForEach(i =>
                {
                    i.Action = string.Empty;
                    i.IdGar = 0;
                    i.SeqGar = g.C2SEQNIV;
                    i.CodeGar = g.GAGARNIV;
                    i.LibGar = g.GADESNIV;
                    i.ReportCal = g.C4ALANIV;
                    i.IdPortee = 0;
                    i.ValPorteeObj = 0;
                    i.UnitPorteObj = string.Empty;
                    i.TypePorteeCal = string.Empty;
                    i.PrimeMntCal = 0;

                });
            }



            return result;
        }

        private static bool GetPerimetreApplique(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            EacParameter[] param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;

            string sql = @"SELECT COUNT(*) NBLIGN FROM KPOPTAP WHERE KDDPERI = 'OB' AND KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type
										AND KDDFOR = :codeFormule AND KDDOPT = :codeOption";
            return CommonRepository.ExistRowParam(sql, param);
        }

        private static List<FormuleGarantiePorteePlatDto> GetInfoPorteeGarantieHisto(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string codeGarantie)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("codeGarantie", DbType.Int32);
            param[0].Value = Convert.ToInt32(codeGarantie);
            param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("version", DbType.Int32);
            param[2].Value = Convert.ToInt32(version);
            param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[3].Value = type;
            param[4] = new EacParameter("codeAvn", DbType.Int32);
            param[4].Value = Convert.ToInt32(codeAvn);
            param[5] = new EacParameter("codeFormule", DbType.Int32);
            param[5].Value = Convert.ToInt32(codeFormule);
            param[6] = new EacParameter("codeOption", DbType.Int32);
            param[6].Value = Convert.ToInt32(codeOption);

            string sql = @"SELECT KABRSQ CODERSQ, KABDESC LIBRSQ, KACOBJ CODEOBJ, KACDESC LIBOBJ, KDFGAN ACTION, KDEID IDGAR, KDESEQ SEQGAR, KDEGARAN CODEGAR, GADES LIBGAR,
									KDEALA REPORTCAL,
									JGVAL VALEUROBJ, JGVAU UNITOBJ, JGVAT TYPEOBJ, IFNULL(KDFID, 0) IDPORTEE,
									IFNULL(KDFPRA, 0) VALPORTEEOBJ, IFNULL(KDFPRU, '') UNITPORTEEOBJ, IFNULL(KDFTYC, '') TYPEPORTEECAL, IFNULL(KDFMNT, 0) PRIMEMNTCAL,
									(JGVFA * 10000 + JGVFM * 100 + JGVFJ) DATESORTIEOBJ
								FROM HPOPTAP
									INNER JOIN HPRSQ ON KDDIPB = KABIPB AND KDDALX = KABALX AND KDDTYP = KABTYP AND KDDRSQ = KABRSQ AND KDDAVN = KABAVN
									INNER JOIN HPOBJ ON KDDIPB = KACIPB AND KDDALX = KACALX AND KDDTYP = KACTYP AND KDDRSQ = KACRSQ AND KDDAVN = KACAVN";
            if (GetPerimetreAppliqueHisto(codeOffre, version, type, codeAvn, codeFormule, codeOption))
            {
                sql += " AND KDDOBJ = KACOBJ";
            }

            sql += @" INNER JOIN YhRTOBJ ON JGIPB = KACIPB AND JGALX = KACALX AND JGRSQ = KACRSQ AND JGOBJ = KACOBJ AND JGAVN = KACAVN
									INNER JOIN HPGARAN ON KDEIPB = KDDIPB AND KDEALX = KDDALX AND KDETYP = KDDTYP AND KDEFOR = KDDFOR AND KDEOPT = KDDOPT AND KDESEQ = :codeGarantie AND KDEAVN = KDDAVN
									INNER JOIN KGARAN ON GAGAR = KDEGARAN
									LEFT JOIN HPGARAP ON KDFIPB = KDDIPB AND KDFALX = KDDALX AND KDFTYP = KDDTYP AND KDFFOR = KDDFOR AND KDFOPT = KDDOPT AND KDFRSQ = KABRSQ AND KDFOBJ = KACOBJ AND KDFKDEID = KDEID  AND KDFAVN = KDEAVN
								WHERE KDDIPB = :codeOffre AND KDDALX = :version AND KDDTYP = :type AND KDDAVN = :codeAvn AND KDDFOR = :codeFormule AND KDDOPT = :codeOption
								ORDER BY KDDOBJ";

            var result = DbBase.Settings.ExecuteList<FormuleGarantiePorteePlatDto>(CommandType.Text, sql, param);
            return result;
        }

        private static bool GetPerimetreAppliqueHisto(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption)
        {
            var param = new EacParameter[6];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeAvn", DbType.AnsiStringFixedLength);
            param[3].Value = codeAvn;
            param[4] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[4].Value = codeFormule;
            param[5] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[5].Value = codeOption;

            string sql = @"SELECT COUNT(*) NBLIGN FROM HPOPTAP WHERE KDDPERI = 'OB' AND KDDIPB = :codeOffre AND KDDALX = :version
										AND KDDTYP = :type AND KDDAVN = :codeAvn AND KDDFOR = :codeFormule AND KDDOPT = :codeOption";
            return CommonRepository.ExistRowParam(sql, param);
        }

        private static List<FormuleGarantiePlatDto> GetListVBHisto(string codeOffre, int version, string type, string codeAvn, int codeFormule, int codeOption, int? date, int codeCible)
        {
            var param = new EacParameter[8];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
            param[4].Value = codeOption;
            param[5] = new EacParameter("P_CODECIBLE", DbType.Int32);
            param[5].Value = codeCible;
            param[6] = new EacParameter("P_DATENOW", DbType.Int32);
            param[6].Value = date;
            param[7] = new EacParameter("P_AVENANT", DbType.Int32);
            param[7].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) - 1 : 0;

            return DbBase.Settings.ExecuteList<FormuleGarantiePlatDto>(CommandType.StoredProcedure, "SP_FORMGARHISTO", param);
        }

        private static string GetDateModifRsq(string codeOffre, string version, string codeRsq)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("codeRsq", DbType.AnsiStringFixedLength);
            param[2].Value = codeRsq;

            string sql = @"select jeava * 10000 + jeavm * 100 + jeavj DATEDEBRETURNCOL from yprtrsq where jeipb = :codeOffre and jealx = :version and jersq = :codeRsq";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                int? date = Convert.ToInt32(result.FirstOrDefault().DateDebReturnCol);
                return AlbConvert.ConvertDateToStr(AlbConvert.ConvertIntToDate(date));
            }

            return string.Empty;
        }

        private static void CopyInfoWorkGarantie(string codeOffre, int version, string type, int codeFormule, int codeOption)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.Int32);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.Int32);
            param[4].Value = codeOption;

            string sqlDel = @"DELETE FROM KPOPTDW WHERE KDCIPB = :codeOffre AND KDCALX = :version AND KDCTYP = :type AND KDCFOR = :codeFormule AND KDCOPT = :codeOption";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDel, param);
            string sqlCopy = @"INSERT INTO KPOPTDW (SELECT * FROM KPOPTD WHERE KDCIPB = :codeOffre AND KDCALX = :version AND KDCTYP = :type AND KDCFOR = :codeFormule AND KDCOPT = :codeOption)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlCopy, param);
            sqlDel = @"DELETE FROM KPGARAW WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type AND KDEFOR = :codeFormule AND KDEOPT = :codeOption";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDel, param);
            sqlCopy = @"INSERT INTO KPGARAW (SELECT * FROM KPGARAN WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type AND KDEFOR = :codeFormule AND KDEOPT = :codeOption)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlCopy, param);
            sqlDel = @"DELETE FROM KPGARTAW WHERE KDGIPB = :codeOffre AND KDGALX = :version AND KDGTYP = :type AND KDGFOR = :codeFormule AND KDGOPT = :codeOption";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDel, param);
            sqlCopy = @"INSERT INTO KPGARTAW (SELECT * FROM KPGARTAR WHERE KDGIPB = :codeOffre AND KDGALX = :version AND KDGTYP = :type AND KDGFOR = :codeFormule AND KDGOPT = :codeOption)";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlCopy, param);
        }

        #endregion

        #endregion
        #region Ecran Condition de Garantie

        #region Méthodes Publiques

        public static string GetFormuleGarantieBranche(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            EacParameter[] param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
            param[4] = new EacParameter("codeOption", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;

            string sql = "SELECT KDABRA BRANCHE FROM KPFOR WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KDAFOR = :codeFormule";

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return result.FirstOrDefault().Branche;
            }

            return string.Empty;
        }

        #endregion

        #endregion
        #region Ecran Matrice Formule

        /// <summary>
        /// Supprime une formule de garantie 
        /// à partir de la matrice formule de garantie
        /// </summary>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="typeDel"></param>
        public static void DeleteFormule(string codeOffre, string version, string type, string codeFormule, string typeDel)
        {
            string sql = @"DELETE FROM KPFOR
					   WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type AND KDAFOR = :codeFormule";
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeFormule);

            CommonRepository.SetTraceLog(codeOffre, version.ToString(), type, 0, "delete for", "DeleteFormule", AlbConvert.ConvertDateToStr(DateTime.Now), "TRACEDEL");

            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            DeleteFormuleGarantie(codeOffre, version, type, codeFormule, typeDel);
        }

        /// <summary>
        /// Duplique la formule de garantie
        /// à partir de la matrice de formule
        /// </summary>
        public static string DuplicateFormule(string codeOffre, string version, string type, string codeFormule, string user)
        {
            string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
            long newCodeFormule = 0;
            string newAlphaFormule = string.Empty;

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", codeOffre.PadLeft(9, ' '));
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;

            string sql = @"SELECT MAX(IFNULL(KDAFOR, 0) + 1) CODE, MAX(KDAALPHA) LETTRELIB FROM KPFOR WHERE KDAIPB = :codeOffre AND KDAALX = :version AND KDATYP = :type";

            var result = DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param).FirstOrDefault();
            if (result != null)
            {
                newCodeFormule = result.Code;
                newAlphaFormule = letters[Array.IndexOf(letters, result.LettreLib) + 1];

                EacParameter[] paramSP = new EacParameter[9];
                paramSP[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
                paramSP[0].Value = codeOffre.PadLeft(9, ' ');
                paramSP[1] = new EacParameter("P_VERSION", DbType.Int32);
                paramSP[1].Value = Convert.ToInt32(version);
                paramSP[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
                paramSP[2].Value = type;
                paramSP[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
                paramSP[3].Value = Convert.ToInt32(codeFormule);
                paramSP[4] = new EacParameter("P_NEWIDFORMULE", DbType.Int32);
                paramSP[4].Value = newCodeFormule;
                paramSP[5] = new EacParameter("P_NEWALPHAFORMULE", DbType.AnsiStringFixedLength);
                paramSP[5].Value = newAlphaFormule;
                paramSP[6] = new EacParameter("P_DATESYSTEME", DbType.AnsiStringFixedLength);
                paramSP[6].Value = AlbConvert.ConvertDateToInt(DateTime.Now).ToString();
                paramSP[7] = new EacParameter("P_USER", DbType.AnsiStringFixedLength);
                paramSP[7].Value = user;
                paramSP[8] = new EacParameter("P_TRAITEMENT", DbType.AnsiStringFixedLength);
                paramSP[8].Value = AlbConstantesMetiers.Traitement.Formule.AsCode();

                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DFORMUL", paramSP);


                return string.Format("{0}_{1}", newCodeFormule, 1);
            }

            else
            {
                return "Error";
            }

        }

        /// <summary>
        /// Renvoie le libelle d'une formule
        /// </summary>
        public static string GetLibFormule(int codeFormule, string codeOffre, string version, string type, string codeAvn, ModeConsultation modeNavig)
        {
            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeFormule", DbType.Int32);
            param[0].Value = codeFormule;
            param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[2].Value = version;
            param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[3].Value = type;

            string sql = string.Format(@"SELECT KDAALPHA LETTRELIB,KDADESC LIBELLE FROM {0}
					   WHERE KDAFOR = :codeFormule AND KDAIPB = : AND KDAALX = :version AND KDATYP = :type {1}",
                CommonRepository.GetPrefixeHisto(modeNavig, "KPFOR"),
                modeNavig == ModeConsultation.Historique ? string.Format(" AND KDAAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            var formule = DbBase.Settings.ExecuteList<FormuleDto>(CommandType.Text, sql, param).FirstOrDefault();
            return formule != null ? (string.IsNullOrEmpty(formule.Libelle) ? formule.LettreLib : formule.LettreLib + "-" + formule.Libelle) : string.Empty;
        }

        public static string ObtenirFormuleOptionByOffre(string codeOffre, string version, string codeAvn, ModeConsultation modeNavig)
        {
            EacParameter[] param = new EacParameter[2];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);

            string toReturn = "0_0";
            string sql = string.Format(@"SELECT KDBFOR CODEFORMULE, KDBOPT CODEOPTION FROM {0} 
												   WHERE KDBIPB= :codeOffre AND KDBALX= :version {1}", CommonRepository.GetPrefixeHisto(modeNavig, "KPOPT"),
                                       modeNavig == ModeConsultation.Historique ? string.Format(" AND KDBAVN = {0}", !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0) : string.Empty);
            var formuleDto = DbBase.Settings.ExecuteList<MatriceFormuleColumnDto>(CommandType.Text, sql, param).FirstOrDefault();

            if (formuleDto != null)
            {
                toReturn = string.Concat(formuleDto.CodeFormule, "_", formuleDto.CodeOption);
            }
            return toReturn;
        }

        public static string ObtenirRisqueObjetFormule(string codeOffre, string version, string codeFormule, string codeOption)
        {
            var toReturn = string.Empty;
            var codeRisque = string.Empty;
            var codeObjets = string.Empty;

            EacParameter[] param = new EacParameter[4];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = Convert.ToInt32(version);
            param[2] = new EacParameter("codeFormule", DbType.Int32);
            param[2].Value = Convert.ToInt32(codeFormule);
            param[3] = new EacParameter("codeOption", DbType.Int32);
            param[3].Value = Convert.ToInt32(codeOption);

            string sql = @"SELECT DISTINCT KDDRSQ CODERISQUE, KDDOBJ CODEOBJET FROM KPOPTAP 
									  WHERE KDDIPB= :codeOffre AND KDDALX= :version AND KDDFOR= :codeFormule AND KDDOPT= :codeOption";
            var listFormuleLineDto = DbBase.Settings.ExecuteList<MatriceFormuleLineDto>(CommandType.Text, sql, param);
            if (listFormuleLineDto != null && listFormuleLineDto.Any())
            {
                foreach (var item in listFormuleLineDto)
                {
                    codeRisque = item.CodeRisque.ToString();
                    if (item.CodeObjet.ToString() != "0")
                    {
                        codeObjets += "_" + item.CodeObjet.ToString();
                    }
                }
            }
            if (!string.IsNullOrEmpty(codeRisque))
            {
                toReturn = codeRisque;
                if (!string.IsNullOrEmpty(codeObjets))
                {
                    toReturn += ";" + codeObjets.Substring(1);
                }
            }
            return toReturn;
        }
        public static List<FormuleGarantieDetailsPlatDto> ObtenirGarantieDetailsInfo(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string id, bool isReadonly, ModeConsultation modeNavig)
        {
            EacParameter[] param = new EacParameter[7];
            param[0] = new EacParameter("P_CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("P_VERSION", DbType.Int32);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("P_CODEFORMULE", DbType.Int32);
            param[3].Value = !string.IsNullOrEmpty(codeFormule) ? Convert.ToInt32(codeFormule) : 0;
            param[4] = new EacParameter("P_CODEOPTION", DbType.Int32);
            param[4].Value = !string.IsNullOrEmpty(codeOption) ? Convert.ToInt32(codeOption) : 0;
            param[5] = new EacParameter("P_CODEAVN", DbType.Int32);
            param[5].Value = !string.IsNullOrEmpty(codeAvn) ? Convert.ToInt32(codeAvn) : 0;
            param[6] = new EacParameter("P_IDGAR", DbType.Int32);
            param[6].Value = !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0;

            if (!string.IsNullOrEmpty(codeAvn) && codeAvn != "0")
            {
                return DbBase.Settings.ExecuteList<FormuleGarantieDetailsPlatDto>(CommandType.StoredProcedure, modeNavig == ModeConsultation.Standard && !isReadonly ? "SP_GETGARANTIEDETAILS_AVN" : modeNavig == ModeConsultation.Historique ? "SP_GETGARANTIEDETAILS_AVN_HIST" : "SP_GETGARANTIEDETAILS_AVN_READONLY", param);
            }
            else
            {
                return DbBase.Settings.ExecuteList<FormuleGarantieDetailsPlatDto>(CommandType.StoredProcedure, modeNavig == ModeConsultation.Standard && !isReadonly ? "SP_GETGARANTIEDETAILS" : modeNavig == ModeConsultation.Historique ? "SP_GETGARANTIEDETAILS_HIST" : "SP_GETGARANTIEDETAILS_READONLY", param);
            }
        }

        #endregion
        #region Inventaire

        public static string GetInfoInventGarantie(string idGarantie, string codeOffre, int version, string type, int codeFormule, int codeOption, ModeConsultation modeNavig)
        {
            EacParameter[] param = new EacParameter[6];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.Int32);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.Int32);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.Int32);
            param[4].Value = codeOption;
            param[5] = new EacParameter("idGarantie", DbType.AnsiStringFixedLength);
            param[5].Value = idGarantie;


            string sql = string.Format(@"SELECT KDEINVSP INVENSPEC, KDEALA TYPEALIM FROM {0} WHERE KDEIPB = :codeOffre AND KDEALX = :version 
										AND KDETYP = :type AND KDEFOR = :codeFormule AND KDEOPT= :codeOption AND KDESEQ = :idGarantie",
                modeNavig == ModeConsultation.Historique ? "HPGARAN" : "KPGARAN");
            var result = DbBase.Settings.ExecuteList<InventaireGarantieDto>(CommandType.Text, sql, param);
            if (result != null && result.Count > 0)
            {
                return string.Format("{0}_{1}", result[0].InvenSpec, result[0].TypeAlim);
            }

            return string.Empty;
        }

        #endregion

        public static int GetNbrObj(string codeOffre, string version, string codeRisque)
        {
            var param = new EacParameter[3];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("codeRisque", DbType.AnsiStringFixedLength);
            param[2].Value = codeRisque;

            string sql = @"SELECT COUNT(*) FROM YPRTOBJ WHERE JGIPB = :codeOffre AND JGALX = :version AND JGRSQ = :codeRisque";

            return Convert.ToInt32(DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param));

        }

        public static int GetLienKpOpt(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;

            string sql = @"SELECT KDBID FROM KPOPT WHERE KDBIPB = :codeOffre AND KDBALX = :version AND KDBTYP = :type AND KDBFOR = :codeFormule AND KDBOPT = :codeOption ";

            return Convert.ToInt32(DbBase.Settings.ExecuteScalar(CommandType.Text, sql, param));

        }

        private static void ClearInfosGar(string codeOffre, string version, string type, string codeFormule, string codeOption, long? seq, long idGar)
        {
            //GET KDGKDKID & KDGKDIID & KDEINVEN
            #region paramRes
            var paramRes = new EacParameter[6];
            paramRes[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramRes[0].Value = codeOffre.PadLeft(9, ' ');
            paramRes[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            paramRes[1].Value = version;
            paramRes[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramRes[2].Value = type;
            paramRes[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            paramRes[3].Value = codeFormule;
            paramRes[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            paramRes[4].Value = codeOption;
            paramRes[5] = new EacParameter("idGar", DbType.Int64);
            paramRes[5].Value = idGar;
            #endregion

            string sql = @"SELECT KDGKDIID INT64RETURNCOL, KDGKDKID ID FROM KPGARTAR WHERE KDGIPB = :codeOffre AND KDGALX = :version
										AND KDGTYP = :type AND KDGFOR = :codeFormule AND KDGOPT = :codeOption AND KDGKDEID = :idGar";
            var res = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramRes).FirstOrDefault();

            #region paramInven
            var paramInven = new EacParameter[6];
            paramInven[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            paramInven[0].Value = codeOffre.PadLeft(9, ' ');
            paramInven[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            paramInven[1].Value = version;
            paramInven[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            paramInven[2].Value = type;
            paramInven[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            paramInven[3].Value = codeFormule;
            paramInven[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            paramInven[4].Value = codeOption;
            paramInven[5] = new EacParameter("seq", DbType.Int64);
            paramInven[5].Value = seq;
            #endregion

            sql = @"SELECT KDEINVEN ID FROM KPGARAN WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type AND KDEFOR = :codeFormule
									AND KDEOPT = :codeOption AND KDESEQ = :seq";
            var inven = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramInven).FirstOrDefault();

            //DELETE KPINVEN / KPINVED / KPINVAPP
            if (inven != null && inven.Id != 0)
            {
                #region paramKbekadid
                var paramKbekadid = new EacParameter[4];
                paramKbekadid[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramKbekadid[0].Value = codeOffre.PadLeft(9, ' ');
                paramKbekadid[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                paramKbekadid[1].Value = version;
                paramKbekadid[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramKbekadid[2].Value = type;
                paramKbekadid[3] = new EacParameter("invenId", DbType.Int64);
                paramKbekadid[3].Value = inven.Id;
                #endregion
                sql = @"SELECT KBEKADID INT64RETURNCOL FROM KPINVEN WHERE KBEIPB = :codeOffre AND KBEALX = :version AND KBETYP = :type
									AND KBEID = :invenId";
                var kbekadid = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramKbekadid).FirstOrDefault();

                if (kbekadid != null)
                {
                    paramKbekadid[3] = new EacParameter("kbekadid", DbType.Int64);
                    paramKbekadid[3].Value = kbekadid.Int64ReturnCol;

                    sql = @"DELETE FROM KPDESI WHERE KADIPB = :codeOffre AND KADALX = :version AND KADTYP = :type AND KADCHR = :kbekadid";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramKbekadid);
                }

                #region param2
                var param2 = new EacParameter[5];
                param2[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param2[0].Value = codeOffre.PadLeft(9, ' ');
                param2[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                param2[1].Value = version;
                param2[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param2[2].Value = type;
                param2[3] = new EacParameter("invenId", DbType.Int64);
                param2[3].Value = inven.Id;
                param2[4] = new EacParameter("codeFormule", DbType.Int64);
                param2[4].Value = codeFormule;
                #endregion

                sql = @"DELETE FROM KPINVAPP WHERE KBGIPB = :codeOffre AND KBGALX = :version AND KBGTYP = :type AND KBGKBEID = :invenId AND KBGFOR = :codeFormule";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param2);

                #region param2
                param2 = new EacParameter[4];
                param2[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                param2[0].Value = codeOffre.PadLeft(9, ' ');
                param2[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                param2[1].Value = version;
                param2[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                param2[2].Value = type;
                param2[3] = new EacParameter("invenId", DbType.Int64);
                param2[3].Value = inven.Id;
                #endregion
                sql = @"DELETE FROM KPINVED WHERE KBFIPB = :codeOffre AND KBFALX = :version AND KBFTYP = :type AND KBFKBEID = :invenId";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param2);

                sql = @"DELETE FROM KPINVEN WHERE KBEIPB = :codeOffre AND KBEALX = :version AND KBETYP = :type AND KBEID = :invenId";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param2);
            }

            //DELETE KPEXPLCI / KPEXPLCID
            if (res != null && res.Int64ReturnCol != 0)
            {
                #region paramKdidesi
                var paramKdidesi = new EacParameter[4];
                paramKdidesi[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramKdidesi[0].Value = codeOffre.PadLeft(9, ' ');
                paramKdidesi[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                paramKdidesi[1].Value = version;
                paramKdidesi[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramKdidesi[2].Value = type;
                paramKdidesi[3] = new EacParameter("res", DbType.Int64);
                paramKdidesi[3].Value = res.Int64ReturnCol;
                #endregion

                sql = @"SELECT KDIDESI INT64RETURNCOL FROM KPEXPLCI WHERE KDIIPB = :codeOffre AND KDIALX = :version AND KDITYP = :type AND KDIID = :res";
                var kdidesi = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramKdidesi).FirstOrDefault();

                var param3 = new EacParameter[1];
                param3[0] = new EacParameter("res", DbType.Int64);
                param3[0].Value = res.Int64ReturnCol;

                sql = @"DELETE FROM KPEXPLCID WHERE KDJKDIID = :res";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param3);

                if (kdidesi != null)
                {
                    paramKdidesi[3] = new EacParameter("kdidesi", DbType.Int64);
                    paramKdidesi[3].Value = kdidesi.Int64ReturnCol;

                    sql = @"DELETE FROM KPDESI WHERE KADIPB = :codeOffre AND KADALX = :version AND KADTYP = :type AND KADCHR = :kdidesi";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramKdidesi);

                    paramKdidesi[3] = new EacParameter("res", DbType.Int64);
                    paramKdidesi[3].Value = res.Int64ReturnCol;
                }

                sql = @"DELETE FROM KPEXPLCI WHERE KDIIPB = :codeOffre AND KDIALX = :version AND KDITYP = :type AND KDIID = :res";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramKdidesi);
            }

            //DELETE KPEXPFRH / KPEXPFRHD
            if (res != null && res.Id != 0)
            {
                #region paramKdidesi
                var paramKdidesi = new EacParameter[4];
                paramKdidesi[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
                paramKdidesi[0].Value = codeOffre.PadLeft(9, ' ');
                paramKdidesi[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
                paramKdidesi[1].Value = version;
                paramKdidesi[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
                paramKdidesi[2].Value = type;
                paramKdidesi[3] = new EacParameter("resId", DbType.Int64);
                paramKdidesi[3].Value = res.Id;
                #endregion

                sql = @"SELECT KDKDESI INT64RETURNCOL FROM KPEXPFRH WHERE KDKIPB = :codeOffre AND KDKALX = :version AND KDKTYP = :type AND KDKID = :resId";
                var kdkdesi = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, paramKdidesi).FirstOrDefault();

                var param2 = new EacParameter[1];
                param2[0] = new EacParameter("res", DbType.Int64);
                param2[0].Value = res.Id;
                sql = @"DELETE FROM KPEXPFRHD WHERE KDLKDKID = :res";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param2);

                if (kdkdesi != null)
                {
                    paramKdidesi[3] = new EacParameter("kdkdesi", DbType.Int64);
                    paramKdidesi[3].Value = kdkdesi.Int64ReturnCol;

                    sql = @"DELETE FROM KPDESI WHERE KADIPB = :codeOffre AND KADALX = :version AND KADTYP = :type AND KADCHR = :kdkdesi";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramKdidesi);
                }

                paramKdidesi[3] = new EacParameter("resId", DbType.Int64);
                paramKdidesi[3].Value = res.Id;

                sql = @"DELETE FROM KPEXPFRH WHERE KDKIPB = :codeOffre AND KDKALX = :version AND KDKTYP = :type AND KDKID = :resId";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, paramKdidesi);
            }

            //DELETE KPGARAP
            #region param
            var param = new EacParameter[6];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;
            param[5] = new EacParameter("idGar", DbType.Int64);
            param[5].Value = idGar;
            #endregion

            sql = @"DELETE FROM KPGARAP WHERE KDFIPB = :codeOffre AND KDFALX = :version AND KDFTYP = :type AND KDFFOR = :codeFormule AND KDFOPT = :codeOption
								  AND KDFKDEID = :idGar";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            //DELETE KPGARTAR
            sql = @"DELETE FROM KPGARTAR WHERE KDGIPB = :codeOffre AND KDGALX = :version AND KDGTYP = :type AND KDGFOR = :codeFormule AND KDGOPT = :codeOption
								  AND KDGKDEID = :idGar";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);

            //DELETE KPGARAN
            sql = @"DELETE FROM KPGARAN WHERE KDEIPB = :codeOffre AND KDEALX = :version AND KDETYP = :type AND KDEFOR = :codeFormule
								  AND KDEOPT = :codeOption AND KDEID = :idGar";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
        }

        private static void FinalClear(string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[5];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;

            string sql = @"SELECT DISTINCT garan.KDEID ID, garan.KDESEQ INT64RETURNCOL, tgar.C2MGA STRRETURNCOL
							FROM KPGARAN garan
							INNER JOIN YPLTGAR tgar ON tgar.C2SEQ = garan.KDESEQ
							INNER JOIN KPOPTD optd ON optd.KDCID = garan.KDEKDCID
							WHERE garan.KDEIPB = :codeOffre AND garan.KDEALX = :version AND garan.KDETYP = :type 
								AND garan.KDEFOR = :codeFormule AND garan.KDEOPT = :codeOption
								AND tgar.C2MGA <> optd.KDCMODELE";


            var garList = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param);

            foreach (var item in garList)
            {
                ClearInfosGar(codeOffre, version, type, codeFormule, codeOption, item.Int64ReturnCol, item.Id);
            }
        }

        public static void ClearOldGars(string codeOffre, string version, string type, string codeFormule, string codeOption, FormuleGarantieSaveDto formuleGarantie, List<GarantiesDto> paramsGaranties)
        {
            FinalClear(codeOffre, version, type, codeFormule, codeOption);
        }

        public static long? getGuidMVoletbyGuidId(long GuidId, string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[6];
            param[0] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[1].Value = version;
            param[2] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[2].Value = type;
            param[3] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[3].Value = codeFormule;
            param[4] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[4].Value = codeOption;
            param[5] = new EacParameter("GuidId", DbType.Int64);
            param[5].Value = GuidId;

            string sql = @"SELECT KDCKARID INT64RETURNCOL FROM KCATVOLET
												INNER JOIN KPOPTD ON KAPKAKID = KDCKAKID AND KDCIPB = :codeOffre AND KDCALX = :version AND KDCTYP = :type AND KDCFOR = :codeFormule AND KDCOPT = :codeOption AND KDCTENG = 'V'
												WHERE KAPID = :GuidId";

            var res = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();

            return res == null ? 0 : res.Int64ReturnCol;
        }

        public static long? getGuidMBlocbyGuidId(long GuidId, string codeOffre, string version, string type, string codeFormule, string codeOption)
        {
            var param = new EacParameter[6];
            param[0] = new EacParameter("GuidId", DbType.Int64);
            param[0].Value = GuidId;
            param[1] = new EacParameter("codeOffre", DbType.AnsiStringFixedLength);
            param[1].Value = codeOffre.PadLeft(9, ' ');
            param[2] = new EacParameter("version", DbType.AnsiStringFixedLength);
            param[2].Value = version;
            param[3] = new EacParameter("type", DbType.AnsiStringFixedLength);
            param[3].Value = type;
            param[4] = new EacParameter("codeFormule", DbType.AnsiStringFixedLength);
            param[4].Value = codeFormule;
            param[5] = new EacParameter("codeOption", DbType.AnsiStringFixedLength);
            param[5].Value = codeOption;


            string sql = @"SELECT KDCKARID INT64RETURNCOL FROM KPOPTD 
										WHERE KDCKAQID = :GuidId AND KDCIPB = :codeOffre AND KDCALX = :v AND KDCTYP = :type AND KDCFOR = :codeFormule AND KDCOPT = :codeOption 
										AND KDCTENG = 'B'";

            var res = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql, param).FirstOrDefault();

            return res == null ? 0 : res.Int64ReturnCol;
        }
    }
}