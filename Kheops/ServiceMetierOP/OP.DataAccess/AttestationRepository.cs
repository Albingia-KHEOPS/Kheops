using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.Attestation;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess
{
    public class AttestationRepository
    {
        #region Méthodes publiques

        public static List<RisqueDto> GetRisqueAttestation(string codeContrat, string version, string type, string attestId)
        {
            DbParameter[] param = new DbParameter[4];
            param[0] = new EacParameter("codeContrat", codeContrat.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("version", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("type", type);
            param[3] = new EacParameter("attestId", 0);
            param[3].Value = !string.IsNullOrEmpty(attestId) ? Convert.ToInt32(attestId) : 0;

            string sql = @"SELECT KABRSQ CODERSQ, KABDESC LIBRSQ, JEVDA * 10000 + JEVDM * 100 + JEVDJ DATEDEBRSQ, JEVFA * 10000 + JEVFM * 100 + JEVFJ DATEFINRSQ, 
                                    KABCIBLE CIBLERSQ, JEVAL VALRSQ, JEVAU UNITERSQ, IFNULL(PARRSQ.TPLIB, '') LIBUNTRSQ, ATTFR.KHUSIT RSQUSE, 
	                            KACOBJ CODEOBJ, KACDESC LIBOBJ, JGVDA * 10000 + JGVDM * 100 + JGVDJ DATEDEBOBJ, JGVFA * 10000 + JGVFM * 100 + JGVFJ DATEFINOBJ, 
                                KACCIBLE CIBLEOBJ, JGVAL VALOBJ, JGVAU UNITEOBJ, IFNULL(PAROBJ.TPLIB, '') LIBUNTOBJ, ATTFO.KHUSIT OBJUSE
	                        FROM KPATT
		                        INNER JOIN KPATTF ATTFR ON ATTFR.KHUKHTID = KHTID AND ATTFR.KHUPERI = 'RQ'
		                        INNER JOIN KPATTF ATTFO ON ATTFO.KHUKHTID = ATTFR.KHUKHTID AND ATTFO.KHUPERI = 'OB' AND ATTFO.KHURSQ = ATTFR.KHURSQ
		                        INNER JOIN YPRTRSQ ON KHTIPB = JEIPB AND KHTALX = JEALX AND ATTFR.KHURSQ = JERSQ
                                LEFT JOIN YYYYPAR PARRSQ ON PARRSQ.TCON = 'PRODU' AND PARRSQ.TFAM = 'QCVAU' AND PARRSQ.TCOD = JEVAU
		                        INNER JOIN KPRSQ ON KHTIPB = KABIPB AND KHTALX = KABALX AND KHTTYP = KABTYP AND ATTFR.KHURSQ = KABRSQ
		                        INNER JOIN YPRTOBJ ON KHTIPB = JGIPB AND KHTALX = JGALX AND ATTFO.KHURSQ = JGRSQ AND ATTFO.KHUOBJ = JGOBJ
                                LEFT JOIN YYYYPAR PAROBJ ON PAROBJ.TCON = 'PRODU' AND PAROBJ.TFAM = 'QCVAU' AND PAROBJ.TCOD = JGVAU
		                        INNER JOIN KPOBJ ON KHTIPB = KACIPB AND KHTALX = KACALX AND KHTTYP = KACTYP AND ATTFO.KHURSQ = KACRSQ AND ATTFO.KHUOBJ = KACOBJ
                            WHERE KHTIPB = :codeContrat AND KHTALX = :version AND KHTTYP = :type AND KHTID = :attestId
                            ORDER BY ATTFR.KHURSQ, ATTFO.KHUOBJ";

            var result = DbBase.Settings.ExecuteList<AttestationSelRsqPlatDto>(CommandType.Text, sql, param);
            if (result != null && result.Any())
            {
                return MapAttestationRisque(result);
            }
            return new List<RisqueDto>();
        }

        public static string ValidSelectRsqObj(string codeContrat, string version, string type, string lotId, string selRsqObj, string user)
        {
            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_LOTID", 0);
            param[3].Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt32(lotId) : 0;
            param[4] = new EacParameter("P_SELRSQOBJ", selRsqObj);
            param[5] = new EacParameter("P_USER", user);
            param[6] = new EacParameter("P_DATENOW", AlbConvert.ConvertDateToInt(DateTime.Now));

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_VALIDSELECTIONRSQ", param);
            return string.Empty;

        }

        public static List<AttestationRisqueDto> GetGarantieAttestation(string codeContrat, string version, string type, string lotId)
        {
            DbParameter[] param = new DbParameter[4];

            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_LOTID", 0);
            param[3].Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt32(lotId) : 0;
            //param[3] = new EacParameter("P_SEARCHGARANTIE", searchGarantie);

            var result = DbBase.Settings.ExecuteList<AttestationSelGarPlatDto>(CommandType.StoredProcedure, "SP_LOADGARANTIEATTES", param);
            
            if (result != null && result.Any())
            {
                return MapAttestationGarantie(result);
            }

            return new List<AttestationRisqueDto>();
        }

        public static string ValidSelectionGar(string codeContrat, string version, string type, string lotId, string selGarantie, string user)
        {
            DbParameter[] param = new DbParameter[7];
            param[0] = new EacParameter("P_CODECONTRAT", codeContrat.Trim().PadLeft(9, ' '));
            param[1] = new EacParameter("P_VERSION", 0);
            param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
            param[2] = new EacParameter("P_TYPE", type);
            param[3] = new EacParameter("P_LOTID", 0);
            param[3].Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt32(lotId) : 0;
            param[4] = new EacParameter("P_SELGARANTIES", selGarantie);
            param[5] = new EacParameter("P_USER", user);
            param[6] = new EacParameter("P_DATENOW", AlbConvert.ConvertDateToInt(DateTime.Now));

            DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_VALIDSELECTIONGAR", param);

            return string.Empty;
        }

        public static string ValidPeriodeAttestation(string codeContrat, string version, string type, string lotId, string exercice, DateTime? periodeDeb, DateTime? periodeFin,
            string typeAttes, bool integralite, string user)
        {
            string erreur = string.Empty;

            //Si le lot n'a pas encore été créé, on appelle la procédure de sélection des infos
            if (lotId == "0" || string.IsNullOrEmpty(lotId))
            {
                #region Paramètres
                DbParameter[] param = new DbParameter[13];
                param[0] = new EacParameter("P_CODECONTRAT", codeContrat.PadLeft(9, ' '));
                param[1] = new EacParameter("P_VERSION", 0);
                param[1].Value = !string.IsNullOrEmpty(version) ? Convert.ToInt32(version) : 0;
                param[2] = new EacParameter("P_TYPE", type);
                param[3] = new EacParameter("P_FONCTION", AlbConstantesMetiers.FONC_ATTEST);
                param[4] = new EacParameter("P_PERIODEDEB", 0);
                param[4].Value = periodeDeb != null ? AlbConvert.ConvertDateToInt(periodeDeb) : 0;
                param[5] = new EacParameter("P_PERIODEFIN", 0);
                param[5].Value = periodeFin != null ? AlbConvert.ConvertDateToInt(periodeFin) : 0;
                param[6] = new EacParameter("P_EXERCICE", 0);
                param[6].Value = !string.IsNullOrEmpty(exercice) ? Convert.ToInt32(exercice) : 0;
                param[7] = new EacParameter("P_TYPEATTES", typeAttes);
                param[8] = new EacParameter("P_COUVATTES", integralite ? "O" : "N");
                param[9] = new EacParameter("P_USER", user);
                param[10] = new EacParameter("P_DATENOW", 0);
                param[10].Value = AlbConvert.ConvertDateToInt(DateTime.Now);
                param[11] = new EacParameter("P_ERREUR", string.Empty);
                param[11].Direction = ParameterDirection.InputOutput;
                param[11].DbType = DbType.AnsiStringFixedLength;
                param[11].Size = 50;
                param[11].Value = string.Empty;
                param[12] = new EacParameter("P_LOTID", 0);
                param[12].Value = 0;
                param[12].Direction = ParameterDirection.InputOutput;
                param[12].DbType = DbType.Int32;
                #endregion
                DbBase.Settings.ExecuteNonQuery(CommandType.StoredProcedure, "SP_SETINFOATTESTATION", param);

                erreur = param[11].Value.ToString();
                lotId = param[12].Value.ToString();

                if (!string.IsNullOrEmpty(erreur))
                {
                    return "ERRORMSG;" + erreur;
                }
            }
            else
            {
                //Remise à jour du type d'attestation
                DbParameter[] paramTypeAttes = new DbParameter[2];
                paramTypeAttes[0] = new EacParameter("typeAttes", typeAttes);
                paramTypeAttes[1] = new EacParameter("lotId", 0);
                paramTypeAttes[1].Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt32(lotId) : 0;

                string sqlTypeAttes = @"UPDATE KPATT SET KHTTYAT = :typeAttes WHERE KHTID = :lotId";
                DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlTypeAttes, paramTypeAttes);

                //Si la case à cocher est cochée, on sélectionne tous les enregistrements du lot
                if (integralite)
                {
                    DbParameter[] param = new DbParameter[1];
                    param[0] = new EacParameter("lotId", 0);
                    param[0].Value = !string.IsNullOrEmpty(lotId) ? Convert.ToInt32(lotId) : 0;

                    string sql = @"UPDATE KPATTF SET KHUSIT = 'V' WHERE KHUKHTID = :lotId";
                    DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql, param);
                }
            }

            //Suppression de tous les enregistrements du lot qui ne sont pas cochés
            DbParameter[] paramDel = new DbParameter[1];
            paramDel[0] = new EacParameter("lotId", 0);
            paramDel[0].Value = Convert.ToInt32(lotId);
            string sqlDel = @"DELETE FROM KPATTF WHERE KHUKHTID = :LOTID AND KHUSIT != 'V'";
            DbBase.Settings.ExecuteNonQuery(CommandType.Text, sqlDel, paramDel);

            //lancement du programme de DAN

            return lotId;
        }

        #endregion

        #region Méthodes privées

        private static List<RisqueDto> MapAttestationRisque(List<AttestationSelRsqPlatDto> risquesPlat)
        {
            List<RisqueDto> risques = new List<RisqueDto>();

            var lstRsq = risquesPlat.GroupBy(el => el.CodeRsq).Select(r => r.First()).ToList();
            lstRsq.ForEach(rsq =>
            {
                var lstObj = risquesPlat.FindAll(r => r.CodeRsq == rsq.CodeRsq).GroupBy(el => el.CodeObj).Select(o => o.First()).ToList();
                var objets = new List<ObjetDto>();

                lstObj.ForEach(obj =>
                {
                    objets.Add(new ObjetDto
                    {
                        Code = obj.CodeObj,
                        Designation = obj.LibObj,
                        EntreeGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(obj.DateDebObj)),
                        SortieGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(obj.DateFinObj)),
                        Cible = new CibleDto { Code = obj.CibleObj },
                        Valeur = Convert.ToInt32(obj.ValObj),
                        Unite = new ParametreDto { Code = obj.UniteObj, Libelle = obj.LibUnitObj },
                        IsUsed = obj.ObjUse == "V"
                    });
                });

                risques.Add(new RisqueDto
                {
                    Code = rsq.CodeRsq,
                    Designation = rsq.LibRsq,
                    EntreeGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(rsq.DateDebRsq)),
                    SortieGarantie = AlbConvert.ConvertIntToDate(Convert.ToInt32(rsq.DateFinRsq)),
                    Cible = new CibleDto { Code = rsq.CibleRsq },
                    Valeur = rsq.ValRsq,
                    Unite = new ParametreDto { Code = rsq.UniteRsq, Libelle = rsq.LibUnitRsq },
                    IsUsed = rsq.RsqUse == "V",
                    Objets = objets
                });
            });

            return risques;
        }

        private static List<AttestationRisqueDto> MapAttestationGarantie(List<AttestationSelGarPlatDto> garantiesPlat)
        {
            List<AttestationRisqueDto> risques = new List<AttestationRisqueDto>();

            var lstRsq = garantiesPlat.GroupBy(el => el.CodeRsq).Select(r => r.First()).ToList();
            lstRsq.ForEach(rsq =>
            {
                var codeObjets = string.Empty;
                var lstObj = garantiesPlat.FindAll(r => r.CodeRsq == rsq.CodeRsq && r.CodeObj != 0).GroupBy(el => el.CodeObj).Select(o => o.First()).ToList();
                var objets = new List<AttestationObjetDto>();

                lstObj.ForEach(obj =>
                {
                    codeObjets += "/O" + obj.CodeObj;

                    objets.Add(new AttestationObjetDto
                    {
                        Code = obj.CodeObj
                    });
                });

                var lstFor = garantiesPlat.FindAll(r => r.CodeRsq == rsq.CodeRsq).GroupBy(el => el.LettreFor).Select(f => f.First()).ToList();
                var formules = new List<AttestationFormuleDto>();

                lstFor.ForEach(form =>
                {
                    var lstGar1 = garantiesPlat.FindAll(r => r.CodeRsq == form.CodeRsq && r.LettreFor == form.LettreFor && r.MasterGaran == 0 && r.NivGaran == 1).GroupBy(el => el.IdGaran).Select(g => g.First()).ToList();
                    var garanties1 = new List<AttestationGarantieNiv1Dto>();

                    lstGar1.ForEach(gar1 =>
                    {
                        var lstGar2 = garantiesPlat.FindAll(r => r.CodeRsq == gar1.CodeRsq && r.LettreFor == gar1.LettreFor && r.MasterGaran == gar1.SeqGaran && r.NivGaran == 2)
                            .GroupBy(el => el.IdGaran).Select(g => g.First()).ToList();
                        var garanties2 = new List<AttestationGarantieNiv2Dto>();

                        lstGar2.ForEach(gar2 =>
                        {
                            var lstGar3 = garantiesPlat.FindAll(r => r.CodeRsq == gar2.CodeRsq && r.LettreFor == gar2.LettreFor && r.MasterGaran == gar2.SeqGaran && r.NivGaran == 3)
                                .GroupBy(el => el.IdGaran).Select(g => g.First()).ToList();
                            var garanties3 = new List<AttestationGarantieNiv3Dto>();

                            lstGar3.ForEach(gar3 =>
                            {
                                var lstGar4 = garantiesPlat.FindAll(r => r.CodeRsq == gar3.CodeRsq && r.LettreFor == gar3.LettreFor && r.MasterGaran == gar3.SeqGaran && r.NivGaran == 4)
                                    .GroupBy(el => el.IdGaran).Select(g => g.First()).ToList();
                                var garanties4 = new List<AttestationGarantieNiv4Dto>();

                                lstGar4.ForEach(gar4 =>
                                {
                                    var newGar4 = new AttestationGarantieNiv4Dto
                                    {
                                        IdGaran = gar4.IdGaran,
                                        CodeGarantie = gar4.CodeGaran,
                                        LibelleGarantie = gar4.LibGaran,
                                        Montant = gar4.ValGaran,
                                        Unite = gar4.UnitGaran,
                                        LibUnite = gar4.LibUntGar,
                                        Base = gar4.BaseGaran,
                                        DateDebut = AlbConvert.ConvertIntToDate(Convert.ToInt32(gar4.DateDebGaran)),
                                        IsUsed = gar4.GarUse == "V",
                                        IsShown = true
                                    };
                                    newGar4.DateFin = gar4.DateFinGaran == 0 ? gar4.DureeGaran == 0 ? null
                                                : AlbConvert.GetFinPeriode(newGar4.DateDebut, gar4.DureeGaran, gar4.DurUnitGaran)
                                                : AlbConvert.ConvertIntToDate(Convert.ToInt32(gar4.DateFinGaran));

                                    garanties4.Add(newGar4);
                                });

                                var newGar3 = new AttestationGarantieNiv3Dto
                                {
                                    IdGaran = gar3.IdGaran,
                                    CodeGarantie = gar3.CodeGaran,
                                    LibelleGarantie = gar3.LibGaran,
                                    Montant = gar3.ValGaran,
                                    Unite = gar3.UnitGaran,
                                    LibUnite = gar3.LibUntGar,
                                    Base = gar3.BaseGaran,
                                    DateDebut = AlbConvert.ConvertIntToDate(Convert.ToInt32(gar3.DateDebGaran)),
                                    IsUsed = gar3.GarUse == "V",
                                    IsShown = true,
                                    Garanties = garanties4
                                };
                                newGar3.DateFin = gar3.DateFinGaran == 0 ? gar3.DureeGaran == 0 ? null
                                            : AlbConvert.GetFinPeriode(newGar3.DateDebut, gar3.DureeGaran, gar3.DurUnitGaran)
                                            : AlbConvert.ConvertIntToDate(Convert.ToInt32(gar3.DateFinGaran));

                                garanties3.Add(newGar3);
                            });

                            var newGar2 = new AttestationGarantieNiv2Dto
                            {
                                IdGaran = gar2.IdGaran,
                                CodeGarantie = gar2.CodeGaran,
                                LibelleGarantie = gar2.LibGaran,
                                Montant = gar2.ValGaran,
                                Unite = gar2.UnitGaran,
                                LibUnite = gar2.LibUntGar,
                                Base = gar2.BaseGaran,
                                DateDebut = AlbConvert.ConvertIntToDate(Convert.ToInt32(gar2.DateDebGaran)),
                                IsUsed = gar2.GarUse == "V",
                                IsShown = true,
                                Garanties = garanties3
                            };
                            newGar2.DateFin = gar2.DateFinGaran == 0 ? gar2.DureeGaran == 0 ? null
                                        : AlbConvert.GetFinPeriode(newGar2.DateDebut, gar2.DureeGaran, gar2.DurUnitGaran)
                                        : AlbConvert.ConvertIntToDate(Convert.ToInt32(gar2.DateFinGaran));

                            garanties2.Add(newGar2);
                        });

                        var newGar1 = new AttestationGarantieNiv1Dto
                        {
                            IdGaran = gar1.IdGaran,
                            CodeGarantie = gar1.CodeGaran,
                            LibelleGarantie = gar1.LibGaran,
                            Montant = gar1.ValGaran,
                            Unite = gar1.UnitGaran,
                            LibUnite = gar1.LibUntGar,
                            Base = gar1.BaseGaran,
                            DateDebut = AlbConvert.ConvertIntToDate(Convert.ToInt32(gar1.DateDebGaran)),
                            IsUsed = gar1.GarUse == "V",
                            IsShown = true,
                            Garanties = garanties2
                        };
                        newGar1.DateFin = gar1.DateFinGaran == 0 ? gar1.DureeGaran == 0 ? null
                                    : AlbConvert.GetFinPeriode(newGar1.DateDebut, gar1.DureeGaran, gar1.DurUnitGaran)
                                    : AlbConvert.ConvertIntToDate(Convert.ToInt32(gar1.DateFinGaran));

                        //newGar1.DateFin = gar1.DateFinGaran == 0 ? gar1.DureeGaran == 0 ? AlbConvert.ConvertIntToDate(Convert.ToInt32(gar1.DateWFGaran))
                        //            : AlbConvert.GetFinPeriode(newGar1.DateDebut, gar1.DureeGaran, gar1.DurUnitGaran)
                        //            : AlbConvert.ConvertIntToDate(Convert.ToInt32(gar1.DateFinGaran));

                        garanties1.Add(newGar1);
                    });

                    formules.Add(new AttestationFormuleDto
                    {
                        LettreFormule = form.LettreFor,
                        LibFormule = form.LibFor,
                        Garanties = garanties1
                    });
                });

                risques.Add(new AttestationRisqueDto
                {
                    Code = rsq.CodeRsq,
                    CodesObj = !string.IsNullOrEmpty(codeObjets) ? codeObjets.Substring(1) : codeObjets,
                    Objets = objets,
                    Formules = formules
                });
            });

            return risques;
        }

        #endregion

    }
}
