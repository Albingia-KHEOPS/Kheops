using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.EasycomClient;
using System.Data.Common;
using System.Data;
using System.Reflection;
using OP.CoreDomain.Helper;

namespace OP.WSAS400.DataLogic
{
    public partial class IOAS400
    {
        /// <summary>
        /// Liste des champs de la requête SQL
        /// </summary>
        private class SelectQueryFields_OffreGet : _SelectQueryFields_Base
        {
            public static int PBIPB = 0;
            public static int PBALX = 1;
            public static int PBSAA = 2;
            public static int PBSAM = 3;
            public static int PBSAJ = 4;
            public static int PBSAH = 5;
            public static int PBBRA = 6;
            public static int PBICT = 7;
            public static int PBIAS = 8;
            public static int TNNOM = 9;
            public static int C_NUM = 10;
            public static int C_EXT = 11;
            public static int C_VOIE = 12;
            public static int C_DISTR = 13;
            public static int C_DEP = 14;
            public static int C_CP = 15;
            public static int C_CEX = 16;
            public static int C_CDX = 17;
            public static int C_VIL = 18;
            public static int C_VIX = 19;
            public static int ANNOM = 20;
            public static int A_NUM = 21;
            public static int A_EXT = 22;
            public static int A_VOIE = 23;
            public static int A_DISTR = 24;
            public static int A_DEP = 25;
            public static int A_CP = 26;
            public static int A_CEX = 27;
            public static int A_CDX = 28;
            public static int A_VIL = 29;
            public static int A_VIX = 30;
            public static int TCGEP = 31;
            public static int TCFVA = 32;
            public static int TCFVM = 33;
            public static int TCFVJ = 34;
        }

        /// <summary>
        /// Offres the get.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        internal CoreDomain.Offre OffreGet(string code, int? version)
        {
            CoreDomain.Offre toReturn = null;
            EacCommand oCmd = new EacCommand();
            DbDataReader oDr;

            string SQLQ = string.Empty;
            SQLQ += "SELECT PBIPB, PBALX, PBSAA, PBSAM, PBSAJ, PBSAH, PBBRA, PBICT, PBIAS";
            SQLQ += " TNNOM, AC.ABPNUM C_NUM, AC.ABPEXT C_EXT, AC.ABPLG4 C_VOIE, AC.ABPLG5 C_DISTR, AC.ABPDP6 C_DEP, AC.ABPCP6 C_CP, AC.ABPCEX C_CEX, AC.ABPCDX C_CDX, AC.ABPVI6 C_VIL, AC.ABPVIX C_VIX,";
            SQLQ += " ANNOM, AA.ABPNUM A_NUM, AA.ABPEXT A_EXT, AA.ABPLG4 A_VOIE, AA.ABPLG5 A_DISTR, AA.ABPDP6 A_DEP, AA.ABPCP6 A_CP, AA.ABPCEX C_CEX, AA.ABPCDX C_CDX, AA.ABPVI6 A_VIL, AA.ABPVIX A_VIX,";
            SQLQ += " TCGEP, TCFVA, TCFVM, TCFVJ";
            SQLQ += " FROM YPOBASE ";
            SQLQ += " LEFT JOIN YCOURTN ON PBICT = TNICT AND TNINL = 0";
            SQLQ += " LEFT JOIN YCOURTI ON TNICT = TCICT";
            SQLQ += " LEFT JOIN YADRESS AC ON TCADH = AC.ABPCHR";
            SQLQ += " LEFT JOIN YASSURE ON PBIAS = ASIAS";
            SQLQ += " LEFT JOIN YASSNOM ON ASIAS = ANIAS AND ANINL = 0 AND ANORD = 0";
            SQLQ += " LEFT JOIN YADRESS AA ON ASADH = AA.ABPCHR";
            SQLQ += " WHERE PBTYP = 'O'";
            SQLQ += String.Format(" AND PBIPB='{0}'", code.PadLeft(9, ' ').Replace("'", "''"));
            if (version != null)
            {
                SQLQ += String.Format(" AND PBALX={0}", version);
            }
            SQLQ += " ORDER BY TPLIB;";

            oCmd.CommandType = System.Data.CommandType.Text;
            oCmd.Connection = oConn;
            oCmd.CommandText = SQLQ;

            oDr = oCmd.ExecuteReader();

            if (oDr.Read())
            {
                toReturn = new CoreDomain.Offre()
                {
                    CodeOffre = oDr.GetString(SelectQueryFields_OffreGet.PBIPB),
                    DateSaisie = DateHelper.AS400GetDate(oDr.GetInt32(SelectQueryFields_OffreGet.PBSAA), oDr.GetInt32(SelectQueryFields_OffreGet.PBSAM), oDr.GetInt32(SelectQueryFields_OffreGet.PBSAJ), oDr.GetInt32(SelectQueryFields_OffreGet.PBSAH)),
                    CabinetApporteur = new CoreDomain.CabinetCourtage()
                    {
                        Code = oDr.GetInt32(SelectQueryFields_OffreGet.PBICT),
                        NomCabinet = oDr.GetString(SelectQueryFields_OffreGet.TNNOM),
                        EstValide = (oDr.GetString(SelectQueryFields_OffreGet.TCGEP) != "I"),
                        FinValidite = DateHelper.AS400GetDate(oDr.GetInt32(SelectQueryFields_OffreGet.TCFVA), oDr.GetInt32(SelectQueryFields_OffreGet.TCFVM), oDr.GetInt32(SelectQueryFields_OffreGet.TCFVJ), 0),
                        Adresse = new CoreDomain.Adresse()
                        {
                            NumeroVoie = oDr.GetString(SelectQueryFields_OffreGet.C_NUM),
                            ExtensionVoie = oDr.GetString(SelectQueryFields_OffreGet.C_EXT),
                            NomVoie = oDr.GetString(SelectQueryFields_OffreGet.C_VOIE),
                            BoitePostale = oDr.GetString(SelectQueryFields_OffreGet.C_DISTR),
                            Ville = new CoreDomain.Ville()
                            {
                                CodePostal = string.Concat(oDr.GetString(SelectQueryFields_OffreGet.C_DEP), oDr.GetString(SelectQueryFields_OffreGet.C_CP)),
                                Nom = oDr.GetString(SelectQueryFields_OffreGet.C_VIL),
                                TypeCedex = (oDr.GetString(SelectQueryFields_OffreGet.C_CDX) == "O"),
                                CodePostalCedex = string.Concat(oDr.GetString(SelectQueryFields_OffreGet.C_DEP), oDr.GetString(SelectQueryFields_OffreGet.C_CEX)),
                                NomCedex = oDr.GetString(SelectQueryFields_OffreGet.C_VIX)
                            }
                        }
                    },
                    PreneurAssurance = new CoreDomain.Assure()
                    {
                        Code = oDr.GetString(SelectQueryFields_OffreGet.PBIAS),
                        NomAssure = oDr.GetString(SelectQueryFields_OffreGet.ANNOM),
                        Adresse = new CoreDomain.Adresse()
                        {
                            NumeroVoie = oDr.GetString(SelectQueryFields_OffreGet.A_NUM),
                            ExtensionVoie = oDr.GetString(SelectQueryFields_OffreGet.A_EXT),
                            NomVoie = oDr.GetString(SelectQueryFields_OffreGet.A_VOIE),
                            BoitePostale = oDr.GetString(SelectQueryFields_OffreGet.A_DISTR),
                            Ville = new CoreDomain.Ville()
                            {
                                CodePostal = string.Concat(oDr.GetString(SelectQueryFields_OffreGet.A_DEP), oDr.GetString(SelectQueryFields_OffreGet.A_CP)),
                                Nom = oDr.GetString(SelectQueryFields_OffreGet.A_VIL),
                                TypeCedex = (oDr.GetString(SelectQueryFields_OffreGet.A_CDX) == "O"),
                                CodePostalCedex = string.Concat(oDr.GetString(SelectQueryFields_OffreGet.A_DEP), oDr.GetString(SelectQueryFields_OffreGet.A_CEX)),
                                NomCedex = oDr.GetString(SelectQueryFields_OffreGet.A_VIX)
                            }
                        }
                    },
                    Branche = new CoreDomain.Branche()
                    {
                        Code = oDr.GetString(SelectQueryFields_OffreGet.PBBRA)
                    },
                    Version = oDr.GetInt32(SelectQueryFields_OffreGet.PBALX)
                };
                if (toReturn.CabinetApporteur.FinValidite != null)
                {
                    toReturn.CabinetApporteur.EstValide = (toReturn.CabinetApporteur.FinValidite >= DateTime.Now && toReturn.CabinetApporteur.EstValide);
                }
            }
            else
            {
                throw new DataException("L'offre n'existe pas");
            }

            return toReturn;

        }

    }
}