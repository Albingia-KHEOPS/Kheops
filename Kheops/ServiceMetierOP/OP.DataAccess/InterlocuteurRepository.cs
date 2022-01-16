using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Helpers;
using System.Globalization;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Common;

namespace OP.DataAccess
{
    public class InterlocuteurRepository
    {
        public static InterlocuteurDto Initialiser(DataRow ligne)
        {
            InterlocuteurDto interlocuteur = null;
            if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "PBIN5", "TNXN5"))
            {
                interlocuteur = new InterlocuteurDto();
                int iid = 0;
                if (ligne.Table.Columns.Contains("PBIN5")) { interlocuteur.Id = int.TryParse(ligne["PBIN5"].ToString(), out iid) ? iid : 0; } else if (ligne.Table.Columns.Contains("TNXN5")) { interlocuteur.Id = int.TryParse(ligne["TNXN5"].ToString(), out iid) ? iid : 0; };
                if (ligne.Table.Columns.Contains("TNNOM")) { interlocuteur.Nom = ligne["TNNOM"].ToString(); };
                if (ligne.Table.Columns.Contains("TCGEP")) { interlocuteur.EstValide = (ligne["TCGEP"].ToString() == "I" ? false : true); };
                interlocuteur.CabinetCourtage = CabinetCourtageRepository.Initialiser(ligne);
            }
            return interlocuteur;
        }

        public static string RechercherNomInterlocuteur(int codeInterlocuteur, int codeCourtier)
        {
            string sql = string.Format(@"SELECT TNNOM STRRETURNCOL
                                         FROM YCOURTN
                                         WHERE TNXN5 = {0}
                                         AND TNICT = {1}
                                         AND TNTNM = 'A'", codeInterlocuteur, codeCourtier);

            var result = DbBase.Settings.ExecuteList<DtoCommon>(CommandType.Text, sql);
            if (result != null && result.Count > 0)
                return result[0].StrReturnCol;
            return string.Empty;
        }

        public static List<InterlocuteurDto> RechercherInterlocuteurs(string nomInterlocuteur, int codeCourtier)
        {
            string sql = @"SELECT TNICT, TNXN5 PBIN5, TNNOM NOM, TNNOMC TNNOMCAB, TCGEP, TCTYP, TCCP, TCFVA, TCFVM, TCFVJ, TCVILC, TCGEPC, TCFVAC, TCFVMC, TCFVJC, TCBUR , BUDBU
                              FROM YCOURTI 
                              INNER JOIN YCOURTN ON TCICT = TNICT AND TNXN5 > 0 AND TNTNM = 'A'
                              LEFT JOIN (SELECT TNICT TNICTC, TNNOM TNNOMC, CONCAT(TCDEP, TCCPO) TCCP, TCVIL TCVILC, TCGEP TCGEPC, TCFVA TCFVAC, TCFVM TCFVMC, TCFVJ TCFVJC FROM YCOURTI
                              LEFT JOIN YCOURTN ON TCICT = TNICT AND TNXN5 = 0 AND TNTNM = 'A'
                              ) YCOURTC ON TNICTC = TNICT
                              LEFT JOIN YBUREAU ON TCBUR = BUIBU";

            string andWhere = " WHERE ";
            if (codeCourtier > 0)
            {
                sql += andWhere;
                sql += string.Format(" TCICT = {0}", codeCourtier);
                andWhere = " AND ";
            }

            if (!String.IsNullOrEmpty(nomInterlocuteur.Trim()))
            {
                sql += andWhere;
                sql += string.Format(" UPPER(TNNOM) LIKE '%{0}%'", nomInterlocuteur.ToUpper());
                andWhere = " AND ";
            }
            sql += " FETCH FIRST 10 ROWS ONLY";

            var result = DbBase.Settings.ExecuteList<InterlocuteurDto>(CommandType.Text, sql);

            if (result != null && result.Any())
            {
                foreach (var ligne in result)
                {
                    // Par defaut Interlocuteur est valide 
                    ligne.EstValide = true;
                    //if (!string.IsNullOrEmpty(ligne.SEstValide))
                    //ligne.EstValide = ligne.SEstValide != "I";

                    if (ligne.FinValiditeAnneeInterlocteur > 0)
                    {                        
                        int annee = Convert.ToInt32(ligne.FinValiditeAnneeInterlocteur.ToString());
                        int mois = Convert.ToInt32(ligne.FinValiditeMoisInterlocteur.ToString());
                        int jour = Convert.ToInt32(ligne.FinValiditeJourInterlocteur.ToString());  

                        ligne.FinValidite = new DateTime(annee, mois, jour);
                        if (ligne.FinValidite.Value < DateTime.Now)
                        {
                            ligne.EstValide = false;
                        }
                    }                                      

                    //Cabinet courtage                    
                    if (ligne.CabinetCourtageCode > 0)
                    {
                       
                        ligne.CabinetCourtage = new CabinetCourtageDto();
                        // Par defaut le cabinet courtage est valide 
                        ligne.CabinetCourtage.EstValide = true;
                        ligne.CabinetCourtage.DemarcheCom = true;
                        ligne.CabinetCourtage.Code = ligne.CabinetCourtageCode;
                        ligne.CabinetCourtage.NomCabinet = ligne.CabinetCourtageNom;
                        ligne.CabinetCourtage.Type = ligne.CabinetCourtageType;
                        if (ligne.FinValiditeAnnee > 0)
                        {
                            int anneeCab = Convert.ToInt32(ligne.FinValiditeAnnee.ToString());
                            int moisCab = Convert.ToInt32(ligne.FinValiditeMois.ToString());
                            int jourCab = Convert.ToInt32(ligne.FinValiditeJour.ToString());
                            ligne.CabinetCourtage.FinValidite = new DateTime(anneeCab, moisCab, jourCab);
                            if (ligne.CabinetCourtage.FinValidite.Value < DateTime.Now)
                            {
                                ligne.CabinetCourtage.EstValide = false;
                                //Si le cabinet courtage est non valide Alors l'interlocuteur est non valide 
                                ligne.EstValide = false;
                            }
                        }

                        ligne.CabinetCourtage.Delegation = new DelegationDto { Code = ligne.CodeDelegation, Nom = ligne.NomDelegation };                      

                        if (ligne.SEstValide == "I")
                        {
                            ligne.CabinetCourtage.DemarcheCom = false;
                            ligne.EstValide = false;
                        }

                        ligne.CabinetCourtage.ValideInterlocuteur = ligne.EstValide; // ligne.SEstValide != "I";

                        if (!string.IsNullOrEmpty(ligne.CabinetCourtageCP))
                        {
                            int cp = 0;
                            ligne.CabinetCourtage.Adresse = new WSAS400.DTO.Adresses.AdressePlatDto();
                            ligne.CabinetCourtage.Adresse.CodePostal = Int32.TryParse(ligne.CabinetCourtageCP, out cp) ? cp : cp;
                            ligne.CabinetCourtage.Adresse.NomVille = ligne.CabinetCourtageVille;
                        }
                    }
                }
            }
            return result;
        }

    }
}
