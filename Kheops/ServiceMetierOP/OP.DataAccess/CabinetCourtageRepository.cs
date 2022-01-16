using System;
using System.Collections.Generic;
using System.Data;
using DataAccess.Helpers;
using System.Globalization;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using System.Linq;
using System.Data.EasycomClient;
using System.Data.Common;
using OP.WSAS400.DTO.Adresses;

namespace OP.DataAccess
{
    public class CabinetCourtageRepository
    {


        public static CabinetCourtageDto Initialiser(DataRow ligne, string prefixe = "")
        {
            CabinetCourtageDto cabinetCourtage = null;

            if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "PBICT", "TNICT", "TCICT"))
            {
                cabinetCourtage = new CabinetCourtageDto();

                //cle primaire
                if (ligne.Table.Columns.Contains("PBICT")) { cabinetCourtage.Code = int.Parse(ligne["PBICT"].ToString()); }
                else if (ligne.Table.Columns.Contains("TNICT")) { cabinetCourtage.Code = int.Parse(ligne["TNICT"].ToString()); }
                else if (ligne.Table.Columns.Contains("TCICT")) { cabinetCourtage.Code = int.Parse(ligne["TCICT"].ToString()); };

                if (ligne.Table.Columns.Contains("TNNOMCAB")) { cabinetCourtage.NomCabinet = ligne["TNNOMCAB"].ToString(); } else if (ligne.Table.Columns.Contains("NOM")) { cabinetCourtage.NomCabinet = ligne["NOM"].ToString(); } else if (ligne.Table.Columns.Contains("NOMAPP")) { cabinetCourtage.NomCabinet = ligne["NOMAPP"].ToString(); } else if (ligne.Table.Columns.Contains("NOMGEST")) { cabinetCourtage.NomCabinet = ligne["NOMGEST"].ToString(); };
                if (ligne.Table.Columns.Contains("NOM2")) { cabinetCourtage.NomSecondaires.Add(ligne["NOM2"].ToString()); };
                if (ligne.Table.Columns.Contains("TCTYP")) { cabinetCourtage.Type = ligne["TCTYP"].ToString(); };
                if (ligne.Table.Columns.Contains("TCGEPC")) { cabinetCourtage.EstValide = (ligne["TCGEPC"].ToString() == "I" ? false : true); };
                cabinetCourtage.Delegation = DelegationRepository.Initialiser(ligne);
                cabinetCourtage.Adresse = AdresseRepository.Initialiser(ligne, prefixe);
            }
            return cabinetCourtage;
        }
        public static int RechercherCount(string nomCabinet = "")
        {
            string sql = String.Format(CultureInfo.CurrentCulture,
            @"SELECT COUNT(*) FROM
            (
                SELECT TCICT, courtn1.TNNOM AS NOM, rownumber() OVER (ORDER BY courtn1.TNNOM, TCICT) AS ID_NEXT, MAX(courtn2.TNNOM) AS NOM2, MAX(TCTYP) AS TCTYP, MAX(TCBUR) AS TCBUR, MAX (BUDBU) AS BUDBU, MAX(ABPVI6) AS ABPVI6, MAX(ABPDP6) AS ABPDP6, MAX(ABPCHR) AS ABPCHR, MAX(ABPLG4) AS ABPLG4, MAX(ABPCP6) AS ABPCP6, MAX(TCGEP) AS TCGEP, MAX(TCFVJ) AS TCFVJ, MAX(TCFVM) AS TCFVM, MAX(TCFVA) AS TCFVA
                FROM YCOURTI LEFT JOIN YADRESS ON TCADH = ABPCHR LEFT JOIN YCOURTN courtn1 ON TCICT = courtn1.TNICT AND courtn1.TNXN5 = 0 AND courtn1.TNTNM = 'A' 
                LEFT JOIN YCOURTN courtn2 ON TCICT = courtn2.TNICT AND courtn2.TNXN5 = 0 AND courtn2.TNTNM = 'S'
                LEFT JOIN YBUREAU ON BUIBU = TCBUR 
                WHERE (LOWER(courtn1.TNNOM) LIKE LOWER('%{0}%') OR LOWER(courtn2.TNNOM) LIKE LOWER('%{0}%'))             
                GROUP BY TCICT, courtn1.TNNOM
                ORDER BY courtn1.TNNOM, TCICT
            ) AS TABLE", nomCabinet.Replace("'", "''").ToUpperInvariant());
            return (int)DbBase.Settings.ExecuteScalar(CommandType.Text, sql);
        }

        public static CabinetCourtageDto Obtenir(int codeCabinetCourtage, int codeInterlocuteur = 0)
        {
            CabinetCourtageDto cabinetCourtage = new CabinetCourtageDto();

            //EacParameter[] param = new EacParameter[2];
            //param[0] = new EacParameter("codeint",DbType.Int32);
            //param[0].Value = codeInterlocuteur;
            //param[1] = new EacParameter("codecab", DbType.Int32);
            //param[1].Value = codeCabinetCourtage;
            int codeint = codeInterlocuteur;
            int codecab = codeCabinetCourtage;


            string sql = $@"SELECT TCICT CODECAB, 
                                        COURTIER.TNNOM NOMCAB, 
                                        TCBUR CODEDELEGATION, 
                                        BUDBU NOMDELEGATION, 
                                        TCFVJ FINVJOUR, 
                                        TCFVM FINVMOIS, 
                                        TCFVA FINVANNEE, 
                                        TCGEP DEMARCHECOM, 
                                        TCVIL, 
                                        TCCOM, 
                                        TCTYP TYPECAB, 
                                        TCADH, 
                                        ABPCHR NUMEROCHRONO, 
                                        IFNULL(ABPLG3, TCAD1) BATIMENT, 
                                        ABPNUM NUMEROVOIE,                                                                            
                                        ABPCEX CODEPOSTALCEDEX, 
                                        ABPLG4 NOMVOIE, 
                                        ABPLG5 BOITEPOSTALE, 
                                        ABPDP6 DEPARTEMENT, 
                                        ABPCP6 CODEPOSTAL, 
                                        ABPVI6 NOMVILLE, 
                                        ABPPAY CODEPAYS, 
                                        ABPCDX TYPECEDEX ,
                                        ABPVIX NOMCEDEX,
                                        IFNULL(ABPL4F, TCAD2) EXTENSIONVOIE,
                                        TCTEL TELEPHONEBUREAU,
                                        TCAEM EMAILBUREAU,
                                        CASE WHEN TCENC = 'O' THEN 'D' ELSE IFNULL(NULLIF(TCYEN, ''), 'C') END CODEENCAISSEMENT,
                                        ENCAISSEMENT.TPLIB LIBENCAISSEMENT,
                                        IFNULL(INTERLOCUTEUR.TNNOM, '') NOMINTER,
                                        IFNULL(FONCTIONINTERL.TPLIB, '') FONCTIONINTERLOCUTEUR,
                                        IFNULL(INTERLOCUTEURDETAIL.TLTEL, '') TELEPHONEINTERLOCUTEUR,
                                        IFNULL(INTERLOCUTEURDETAIL.TLAEM, '') EMAILINTERLOCUTEUR,
                                        ACLUIN INSPECTEUR
                                FROM YCOURTI 
                                        LEFT JOIN YCOURTN COURTIER ON TCICT = COURTIER.TNICT AND COURTIER.TNXN5 = 0 AND COURTIER.TNTNM = 'A' 
                                        LEFT JOIN YCOURTN INTERLOCUTEUR ON INTERLOCUTEUR.TNICT = COURTIER.TNICT AND INTERLOCUTEUR.TNXN5 = {codeint} AND INTERLOCUTEUR.TNXN5 > 0 AND INTERLOCUTEUR.TNTNM = 'A' 
                                        LEFT JOIN YCOURTL INTERLOCUTEURDETAIL ON INTERLOCUTEURDETAIL.TLICT = TCICT AND INTERLOCUTEURDETAIL.TLINL = INTERLOCUTEUR.TNXN5
                                        LEFT JOIN YYYYPAR FONCTIONINTERL ON FONCTIONINTERL.TCOD = INTERLOCUTEURDETAIL.TLFOC AND FONCTIONINTERL.TCON = 'PRODU' AND FONCTIONINTERL.TFAM = 'FONCT'
                                        LEFT JOIN YADRESS ON  TCADH = ABPCHR 
                                        LEFT JOIN YBUREAU ON BUIBU = TCBUR
                                        LEFT JOIN YYYYPAR ENCAISSEMENT ON ENCAISSEMENT.TCOD = IFNULL(NULLIF(TCYEN, ''), 'C') AND  ENCAISSEMENT.TCON = 'GENER' AND ENCAISSEMENT.TFAM = 'TCYEN'
                                        LEFT JOIN YSECTEU ON ABHSEC=TCSEC
                                        LEFT JOIN YINSPEC ON ACLINS = ABHINS
                                WHERE TCICT = {codecab} ";
            //codeCabinetCourtage, codeInterlocuteur);
            var result = DbBase.Settings.ExecuteList<CabinetCourtagePlatDto>(CommandType.Text, sql);
            if (result != null && result.Any())
            {
                var cabinetCourtagePlatDto = result.FirstOrDefault();
                int annee = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeAnee.ToString());
                int mois = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeMois.ToString());
                int jour = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeJour.ToString());
                cabinetCourtage = GetCabinetCourtage(cabinetCourtage, cabinetCourtagePlatDto, annee, mois, jour);
                cabinetCourtage.ValideInterlocuteur = ValideInterlocuteur(cabinetCourtage.Code);
            }
            return cabinetCourtage;
        }
        public static List<CabinetCourtageDto> Rechercher(int debut, int fin, string nomCabinet, string order, int by, bool modeAutoComplete)
        {
            string[] sortFields = { "TCICT", "courtn1.TNNOM", "TCICT", "MAX(ABPDP6)", "MAX(ABPVI6)", "MAX(TCTYP)", "TCICT", "TCICT", "TCICT", "TCICT" };
            List<CabinetCourtageDto> result = new List<CabinetCourtageDto>();
            bool necessiteChargement = true;
            if (debut == 1 && fin == 10 && string.IsNullOrEmpty(nomCabinet))
            {
                if (result != null)
                {
                    necessiteChargement = false;
                }
            }
            if (necessiteChargement)
            {
                DbParameter[] param = new DbParameter[6];
                param[0] = new EacParameter("P_NOMCABINET", nomCabinet.Replace("'", "''").ToUpperInvariant().Trim());
                param[1] = new EacParameter("P_STARTLINE", 0);
                param[1].Value = debut;
                param[2] = new EacParameter("P_ENDLINE", 0);
                param[2].Value = nomCabinet.Replace("'", "''").ToUpperInvariant().Trim().Length > 3 ? 100 : fin;
                param[3] = new EacParameter("P_SORTINGBY", sortFields[by - 1]);
                param[4] = new EacParameter("P_ORDERBY", order);
                param[5] = new EacParameter("P_MODE", modeAutoComplete ? "AUTOCOMPLETE" : string.Empty);

                CabinetCourtageDto cabinetCourtage = null;

                var listCabinetCourtagePlatDto = DbBase.Settings.ExecuteList<CabinetCourtagePlatDto>(CommandType.StoredProcedure, "SP_RECHERCHECABINETCOURTAGE", param);
                if (listCabinetCourtagePlatDto.Any())
                {
                    foreach (var cabinetCourtagePlatDto in listCabinetCourtagePlatDto)
                    {
                        int annee = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeAnee.ToString());
                        int mois = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeMois.ToString());
                        int jour = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeJour.ToString());

                        if (cabinetCourtage == null || cabinetCourtage.Code.ToString() != cabinetCourtagePlatDto.Code.ToString())
                        {
                            cabinetCourtage = GetCabinetCourtage(cabinetCourtage, cabinetCourtagePlatDto, annee, mois, jour);
                            result.Add(cabinetCourtage);
                        }
                        else
                        {
                            cabinetCourtage.NomSecondaires.Add(!string.IsNullOrEmpty(cabinetCourtagePlatDto.NomSecondaire) ? cabinetCourtagePlatDto.NomSecondaire : string.Empty);
                        }
                        cabinetCourtage = CabinetsCourtagesInterlocuteurs(cabinetCourtage);
                    }
                }
            }
            if (result.Any() && sortFields[by - 1] == sortFields[1]) {
                // resort using StartsWith pattern
                result = result.OrderBy(x => {
                    var names = new HashSet<string>();
                    if (x.NomCabinet.StartsWith(nomCabinet, StringComparison.InvariantCultureIgnoreCase)) {
                        names.Add(x.NomCabinet);
                    }
                    foreach (string name in x.NomSecondaires?.Where(ns => ns.StartsWith(nomCabinet, StringComparison.InvariantCultureIgnoreCase))) {
                        names.Add(name);
                    }
                    return names.OrderBy(n => n.Length).First();
                }).ToList();
            }
            return result;
        }

        private static CabinetCourtageDto CabinetsCourtagesInterlocuteurs(CabinetCourtageDto cabinetCourtage)
        {
            List<InterlocuteurDto> interlocuteurs = new List<InterlocuteurDto>();

            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codecab", DbType.Int32);
            param[0].Value = cabinetCourtage.Code;

            string sql = @"SELECT TNICT CODECAB, TNXN5 IDINTER, TNNOM NOMINTER, TNNOMC NOMCAB, TCGEP ESTVALIDEINTER, TCTYP TYPECAB,DEP DEPARTEMENT,CP CODEPOSTALYC, TCVILC NOMVILLE, TCGEPC ESTVALIDE
                              FROM YCOURTI 
                              INNER JOIN YCOURTN ON TCICT = TNICT AND TNXN5 > 0 AND TNTNM = 'A'
                              LEFT JOIN (SELECT TNICT TNICTC, TNNOM TNNOMC,TCDEP DEP, TCCPO CP, TCVIL TCVILC, TCGEP TCGEPC FROM YCOURTI
                              LEFT JOIN YCOURTN ON TCICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A'
                              ) YCOURTC ON TNICTC = TNICT WHERE TCICT=:codecab";
            //cabinetCourtage.Code);

            var listCabinetCourtagePlatDto = DbBase.Settings.ExecuteList<CabinetCourtagePlatDto>(CommandType.Text, sql, param);
            if (listCabinetCourtagePlatDto.Any())
            {
                foreach (var cabinetCourtagePlatDto in listCabinetCourtagePlatDto)
                {
                    if (cabinetCourtagePlatDto != null)
                    {
                        CabinetCourtageDto cabCourtage = null;
                        int annee = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeAnee.ToString());
                        int mois = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeMois.ToString());
                        int jour = Convert.ToInt32(cabinetCourtagePlatDto.FinValiditeJour.ToString());

                        InterlocuteurDto interlocuteur = new InterlocuteurDto
                        {
                            Id = cabinetCourtagePlatDto.IdInterlocuteur,
                            Nom = cabinetCourtagePlatDto.NomInterlocuteur,
                            EstValide = cabinetCourtagePlatDto.EstValideInterlocuteur == "I" ? false : true,
                            CabinetCourtage = GetCabinetCourtage(cabCourtage, cabinetCourtagePlatDto, annee, mois, jour)
                        };
                        if (interlocuteur.EstValide)
                        {
                            cabinetCourtage.ValideInterlocuteur = true;
                        }
                        interlocuteurs.Add(interlocuteur);
                    }
                }
            }
            cabinetCourtage.Interlocuteurs = interlocuteurs;
            return cabinetCourtage;
        }

        /// <summary>
        /// Vérification de la validation des interlocuteurs
        /// </summary>
        /// <param name="cabinetCourtageCode"></param>
        /// <returns></returns>
        private static bool ValideInterlocuteur(int cabinetCourtageCode)
        {
            var interlocuteurs = new List<InterlocuteurDto>();
            var result = true;
            EacParameter[] param = new EacParameter[1];
            param[0] = new EacParameter("codecab", DbType.Int32)
            {
                Value = cabinetCourtageCode
            };

            string sql = @"SELECT TCGEP ESTVALIDEINTER
                              FROM YCOURTI 
                              INNER JOIN YCOURTN ON TCICT = TNICT AND TNXN5 > 0 AND TNTNM = 'A'
                              LEFT JOIN (SELECT TNICT TNICTC, TNNOM TNNOMC,TCDEP DEP, TCCPO CP, TCVIL TCVILC, TCGEP TCGEPC FROM YCOURTI
                              LEFT JOIN YCOURTN ON TCICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A'
                              ) YCOURTC ON TNICTC = TNICT WHERE TCICT=:codecab";
            var listCabinetCourtagePlatDto = DbBase.Settings.ExecuteList<CabinetCourtagePlatDto>(CommandType.Text, sql, param);
            if (listCabinetCourtagePlatDto.Any())
            {
                foreach (var cabinetCourtagePlatDto in listCabinetCourtagePlatDto)
                {
                    if (cabinetCourtagePlatDto != null)
                    {
                        if (cabinetCourtagePlatDto.EstValideInterlocuteur == "I")
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private static AdressePlatDto GetAdresse(CabinetCourtagePlatDto cabinetPlatDto)
        {
            AdressePlatDto adresse = null;
            if (cabinetPlatDto != null)
            {
                if (!string.IsNullOrEmpty(cabinetPlatDto.Departement) || !string.IsNullOrEmpty(cabinetPlatDto.NomVille))
                {
                    int cp = 0;
                    int cpCedex = 0;
                    adresse = new AdressePlatDto
                    {
                        Batiment = cabinetPlatDto.Batiment,
                        BoitePostale = cabinetPlatDto.BoitePostale,
                        ExtensionVoie = cabinetPlatDto.ExtensionVoie,
                        MatriculeHexavia = cabinetPlatDto.MatriculeHexavia,
                        NomVoie = cabinetPlatDto.NomVoie,
                        NumeroChrono = cabinetPlatDto.NumeroChrono,
                        NumeroVoie = cabinetPlatDto.NumeroVoie,
                        CodeInsee = cabinetPlatDto.CodeInsee,
                        CodePostal = Int32.TryParse(cabinetPlatDto.Departement + cabinetPlatDto.CodePostal.ToString().PadLeft(3, '0'), out cp) ? cp : cp,
                        CodePostalCedex = Int32.TryParse(cabinetPlatDto.Departement + cabinetPlatDto.CodePostalCedex.ToString().PadLeft(3, '0'), out cpCedex) ? cpCedex : cpCedex,
                        NomVille = cabinetPlatDto.NomVille,
                        NomCedex = cabinetPlatDto.NomCedex,
                        TypeCedex = cabinetPlatDto.TypeCedex,
                        CodePays = cabinetPlatDto.CodePays,
                        NomPays = cabinetPlatDto.NomPays,

                    };
                }
            }
            return adresse;
        }
        private static CabinetCourtageDto GetCabinetCourtage(CabinetCourtageDto cabinetCourtage, CabinetCourtagePlatDto cabinetCourtagePlatDto, int annee, int mois, int jour)
        {
            cabinetCourtage = new CabinetCourtageDto();
            cabinetCourtage.DemarcheCom = true;
            cabinetCourtage.EstValide = true;
            cabinetCourtage.Adresse = GetAdresse(cabinetCourtagePlatDto);
            cabinetCourtage.Code = cabinetCourtagePlatDto.Code;
            cabinetCourtage.NomCabinet = cabinetCourtagePlatDto.NomCabinet;
            cabinetCourtage.Type = cabinetCourtagePlatDto.Type;
            cabinetCourtage.NomSecondaires.Add(!string.IsNullOrEmpty(cabinetCourtagePlatDto.NomSecondaire) ? cabinetCourtagePlatDto.NomSecondaire : string.Empty);
            if (!string.IsNullOrEmpty(cabinetCourtagePlatDto.CodeDelegation))
                cabinetCourtage.Delegation = new DelegationDto
                {
                    Code = cabinetCourtagePlatDto.CodeDelegation,
                    Nom = cabinetCourtagePlatDto.NomDelegation
                };

            if (annee != 0)
            {
                cabinetCourtage.FinValidite = new DateTime(annee, mois, jour);
                if (cabinetCourtage.FinValidite.Value < DateTime.Now)
                {
                    cabinetCourtage.EstValide = false;
                }
            }
            if (cabinetCourtagePlatDto.DemarcheCom == "I")
            {
                cabinetCourtage.DemarcheCom = false;
            }

            cabinetCourtage.TelephoneBureau = cabinetCourtagePlatDto.TelephoneBureau;
            cabinetCourtage.EmailBureau = cabinetCourtagePlatDto.EmailBureau;
            cabinetCourtage.CodeEncaissement = cabinetCourtagePlatDto.CodeEncaissement;
            cabinetCourtage.LibEncaissement = cabinetCourtagePlatDto.LibEncaissement;

            cabinetCourtage.NomInterlocuteur = cabinetCourtagePlatDto.NomInterlocuteur;
            cabinetCourtage.FonctionInterlocuteur = cabinetCourtagePlatDto.FonctionInterlocuteur;
            cabinetCourtage.TelephoneInterlocuteur = cabinetCourtagePlatDto.TelephoneInterlocuteur;
            cabinetCourtage.EmailInterlocuteur = cabinetCourtagePlatDto.EmailInterlocuteur;

            cabinetCourtage.Inspecteur = cabinetCourtagePlatDto.Inspecteur;

            return cabinetCourtage;
        }
    }
}
