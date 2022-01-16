using Hexavia.Repository.Interfaces;
using Hexavia.Models;
using Hexavia.Tools.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Hexavia.Tools.Exceptions;

namespace Hexavia.Repository
{
    public class AdresseRepository : BaseRepository, IAddressRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AdresseRepository));

        /// <summary>
        /// Nombre d'adresse maximum.
        /// </summary>
        public const int MAX_ADRESSE = 1000;

        public AdresseRepository(DataAccessManager dataAccessManager)
           : base(dataAccessManager)
        {
        }

        /// <summary>
        /// Permet d'obtenir une adresse par son numéro chrono.
        /// </summary>
        /// <param name="numeroChrono"></param>
        /// <returns></returns>
        public IList<Adresse> GetAllAdresses()
        {
            var listAdresses = new List<Adresse>();
            CmdWrapper cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT ABPCHR, ABPNUM, ABPEXT, ABPLG3, ABPVI6, ABPVIX, ABPDP6, ABPCP6, ABPCEX, ABPCDX, ABPLG4 FROM YADRESS";
            
            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            if ((dataTable!= null) && (dataTable.Rows.Count > 0))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    listAdresses.Add(GetAdresse(row));
                }
            }
            return listAdresses;
        }

        public IList<Adresse> GetAllAdressesNotGeolocalisated()
        {
            var listAdresses = new List<Adresse>();
            CmdWrapper cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = string.Format(@"SELECT YADRESS.ABPCHR, ABPNUM, ABPEXT, 
                                                     ABPLG3, ABPVI6, ABPVIX, ABPDP6, 
                                                     ABPCP6, ABPCEX, ABPCDX, ABPLG4 
                                              FROM   YADRESS
                                                     LEFT JOIN KGEOLOC ON YADRESS.ABPCHR = KGEOLOC.KGEOCHR
                                              WHERE  KGEOLOC.KGEOCHR IS NULL");

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            if ((dataTable != null) && (dataTable.Rows.Count > 0))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    listAdresses.Add(GetAdresse(row));
                }
            }
            return listAdresses;
        }
        public IList<Adresse> GetAllAdressesNotGeolocalisatedByPage(int pageNumber, int pageSize)
        {
            var listAdresses = new List<Adresse>();
            if (pageSize !=0 && pageNumber >= 1)
            {

          
                CmdWrapper cmd = DataAccessManager.CmdWrapper;
                cmd.CommandText = string.Format(@"SELECT DISTINCT YADRESS.ABPCHR, ABPNUM, ABPEXT, 
                                                     ABPLG3, ABPVI6, ABPVIX, ABPDP6, 
                                                     ABPCP6, ABPCEX, ABPCDX, ABPLG4 ,ABPLG5,
                                                     ABPPAY, REPLACE(TPLIB, '-', '') AS TPLIB
                                              FROM   YADRESS
                                                     LEFT JOIN KGEOLOC ON YADRESS.ABPCHR = KGEOLOC.KGEOCHR
                                                     LEFT JOIN YYYYPAR CPAYS ON CPAYS.TCON = 'GENER' AND CPAYS.TFAM = 'CPAYS' AND CPAYS.TPCA2 = ABPPAY
                                              WHERE  KGEOLOC.KGEOCHR IS NULL 
                                              ORDER BY YADRESS.ABPCHR 
                                              LIMIT {0} OFFSET {1}", pageSize, pageSize * (pageNumber - 1));

                var dataTable = DataAccessManager.ExecuteDataTable(cmd);

                if ((dataTable != null) && (dataTable.Rows.Count > 0))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        listAdresses.Add(GetAdresse(row));
                    }
                }

            }
            return listAdresses;
        }
        /// <summary>
        /// Permet d'obtenir une adresse par son numéro chrono.
        /// </summary>
        /// <param name="numeroChrono"></param>
        /// <returns></returns>
        public Adresse GetAdresseByNumeroChrono(int numeroChrono)
        {
            Adresse result = null;
            CmdWrapper cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT ABPCHR, ABPNUM, ABPEXT, ABPLG3, ABPVI6, ABPVIX, ABPDP6, ABPCP6, ABPCEX, ABPCDX, ABPLG4 FROM YADRESS";
            cmd.Where("ABPCHR", numeroChrono, DbType.Int16);

            DataTable dataTable = DataAccessManager.ExecuteDataTable(cmd);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAdresse(dataTable.Rows[0]);
            }
            return result;
        }

        /// <summary>
        /// Permet de recuperé une liste d'adresses paginée.
        /// </summary>
        /// <param name="adresses"></param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public IEnumerable<Adresse> RechercheAdresse(IEnumerable<Adresse> adresses, int startRow = 0, int endRow = 100, bool adrExacte = false)
        {
            foreach (Adresse adresse in adresses)
            {
                if (String.IsNullOrEmpty(adresse.CodePostal) && String.IsNullOrEmpty(adresse.Ville))
                {
                    throw new AdresseException("RechercheAdresse necessite au moins le code postal ou la ville");
                }
            }

            var result = new List<Adresse>();
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT ABLLOC, ABLCPO, ABLLIB, ABLACH, ABMLIB, ABMMAT, ABMCPO, ABMLOC FROM ( ";
            CreateRequeteRechercheAdresse(cmd, adresses, adrExacte);
            cmd.CommandText += ") AS TABLE WHERE ID_NEXT BETWEEN " + startRow + " AND " + endRow + " ";

            var dataTable = DataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(GetAdresse(row));
            }
            return result;
        }

        /// <summary>
        /// RechercheAdresseCount
        /// </summary>
        /// <param name="adresses"></param>
        /// <param name="adrExacte"></param>
        /// <returns></returns>
        public int RechercheAdresseCount(IEnumerable<Adresse> adresses, bool adrExacte = false)
        {
            if ((from Adr in adresses where String.IsNullOrEmpty(Adr.CodePostal) && String.IsNullOrEmpty(Adr.Ville) select Adr).Count() > 0)
            {
                throw new AdresseException("RechercheAdresse necessite au moins le code postal ou la ville");
            }
            var cmd = DataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT COUNT (*) FROM ( ";
            CreateRequeteRechercheAdresse(cmd, adresses, adrExacte);
            cmd.CommandText += ") AS TABLE";
            return (int)DataAccessManager.ExecuteScalar(cmd);
        }

        /// <summary>
        /// Permet de recuperer une liste d'adresse suivant des criteres.
        /// </summary>
        /// <param name="codePostal"></param>
        /// <param name="motsVille"></param>
        /// <returns></returns>
        public IEnumerable<Adresse> RechercheVille(string codePostal, IEnumerable<string> motsVille)
        {
            List<Adresse> result = new List<Adresse>();
            CmdWrapper cmdWrapper = DataAccessManager.CmdWrapper;
            //modif ville : type ville arrondissement
            //cmdWrapper.CommandText = "SELECT ABLACH, ABLCPO FROM YHEXLOC";
            cmdWrapper.CommandText = "SELECT ABLLIB, ABLCPO FROM YHEXLOC";
            cmdWrapper.WhereStartWith("ABLCPO", codePostal);

            List<string> mots = motsVille.Where(x => x != null).ToList();
            if (mots.Count() > 0)
            {
                if (cmdWrapper.HasWhere)
                {
                    cmdWrapper.CommandText += " AND ";
                }
                else
                {
                    cmdWrapper.CommandText += " WHERE ";
                }
                cmdWrapper.CommandText += "(";
                for (int i = 0; i < mots.Count(); i++)
                {
                    //modif ville : type ville arrondissement
                    //cmdWrapper.CommandText += " ABLACH LIKE :mot" + i + " OR";
                    cmdWrapper.CommandText += " ABLLIB LIKE :mot" + i + " OR";
                    cmdWrapper.AppendParameter(DbType.AnsiStringFixedLength, "mot" + i, "%" + mots[i] + "%");
                }
                cmdWrapper.CommandText = cmdWrapper.CommandText.Substring(0, cmdWrapper.CommandText.Length - 2);
                cmdWrapper.CommandText += ")";
            }

            cmdWrapper.CommandText += " FETCH FIRST " + MAX_ADRESSE + " ROWS ONLY";

            DataTable dataTable = DataAccessManager.ExecuteDataTable(cmdWrapper);
            foreach (DataRow row in dataTable.Rows)
            {
                Adresse nouvelleAdresse = new Adresse();
                nouvelleAdresse.CodePostal = row["ABLCPO"].ToString();
                //modif ville : type ville arrondissement
                //nouvelleAdresse.Ville = row["ABLACH"].ToString();
                nouvelleAdresse.Ville = row["ABLLIB"].ToString();
                result.Add(nouvelleAdresse);
            }
            return result;
        }

        #region Private functions

        /// <summary>
        /// CreateRequeteRechercheAdresse
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="adresses"></param>
        /// <param name="adrExacte"></param>
        private void CreateRequeteRechercheAdresse(CmdWrapper cmd, IEnumerable<Adresse> adresses, bool adrExacte)
        {
            var adressesList = adresses.ToList();

            //modif ville : type ville arrondissement ajout dans les 2 lignes de ABLLIB
            cmd.CommandText += "SELECT rownumber() OVER (ORDER BY ABLACH) AS ID_NEXT, ABLLOC, ABLCPO, ABLLIB, ABLACH, ABMLIB, ABMMAT, ABMCPO, ABMLOC FROM (" +
                "SELECT ABLLOC, ABLCPO, ABLLIB, ABLACH FROM YHEXLOC WHERE ";

            for (int i = 0; i < adressesList.Count; i++)
            {
                var adresse = adressesList[i];
                cmd.CommandText += " ( ";

                cmd.CommandText += "ABLLIB = :ville_" + i;
                cmd.AppendParameter(DbType.AnsiStringFixedLength, "ville_" + i, OutilsHelper.ToString(adresse.VilleSansCedex));

                if (adresse.CodePostal != null)
                {
                    AjouteCodePostal(cmd, adresse, i);
                }

                cmd.CommandText += ") OR ";
            }

            cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 5);
            cmd.CommandText += ")) AS TABLE " + "LEFT JOIN YHEXVIA ON ABMCPO=ABLCPO AND ABMLOC=ABLLOC ";

            if (adrExacte)
            {
                cmd.CommandText += " WHERE (";
                cmd.CommandText += " ABMLIB LIKE :mot OR";
                cmd.AppendParameter(DbType.AnsiStringFixedLength, "mot", "%" + adresses.First().NomVoie + "%");
                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 2);
                cmd.CommandText += ")";
            }
            else
            {
                List<string> mots = adresses.First().FiltreRueEtDistributionEnMots().ToList();
                if (mots.Count() > 0)
                {
                    cmd.CommandText += " WHERE (";
                    for (int j = 0; j < mots.Count(); j++)
                    {
                        cmd.CommandText += " ABMLIB LIKE :mot_" + j + " OR";
                        cmd.AppendParameter(DbType.AnsiStringFixedLength, "mot_" + j, "%" + mots[j] + "%");
                    }
                    cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 2);
                    cmd.CommandText += ")";
                }
            }
        }

        /// <summary>
        /// AjouteCodePostal
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="adresse"></param>
        /// <param name="i"></param>
        private void AjouteCodePostal(CmdWrapper cmd, Adresse adresse, int i)
        {
            if (adresse.CodePostal.Length == 5)
            {
                cmd.CommandText += " AND ABLCPO = :codePostal_" + i;
                cmd.AppendParameter(DbType.AnsiStringFixedLength, "codePostal_" + i, adresse.CodePostal);
            }
            else
            {
                cmd.CommandText += " AND ABLCPO LIKE :codePostal_" + i;
                cmd.AppendParameter(DbType.AnsiStringFixedLength, "codePostal_" + i, adresse.CodePostal + "%");
            }
        }

        /// <summary>
        /// Permet de recuperer une adresse provenant d'une DataRow.
        /// </summary>
        /// <param name="ligne"></param>
        /// <returns></returns>
        public static Adresse GetAdresse(DataRow ligne)
        {
            var adresse = new Adresse
            {
                Pays = new Pays()
            };

            // TODO : verifier les nom des champs de la base
            if (ligne.Table.Columns.Contains("ABPCHR")) { adresse.NumeroChrono = IntOutilsHelper.ToNullableInt(Outils.SiNull(ligne["ABPCHR"]).ToString()); } else if (ligne.Table.Columns.Contains("PBADH")) { adresse.NumeroChrono = IntOutilsHelper.ToNullableInt(Outils.SiNull(ligne["PBADH"]).ToString()); };
            if (ligne.Table.Columns.Contains("ABPLG3")) { adresse.Batiment = Outils.SiNull(ligne["ABPLG3"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPEXT")) { adresse.ExtensionVoie = Outils.SiNull(ligne["ABPEXT"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPNUM")) { adresse.NumeroVoie = IntOutilsHelper.ToNullableInt(ligne["ABPNUM"].ToString())!=0 && IntOutilsHelper.ToNullableInt(ligne["ABPNUM"].ToString()) != null? ligne["ABPNUM"].ToString():string.Empty; };
            if (ligne.Table.Columns.Contains("ABPLG4")) { adresse.NomVoie = Outils.SiNull(ligne["ABPLG4"]).ToString(); } else if (ligne.Table.Columns.Contains("ABMLIB")) { adresse.NomVoie = Outils.SiNull(ligne["ABMLIB"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPLG5")) { adresse.BoitePostale = Outils.SiNull(ligne["ABPLG5"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPDP6") && ligne.Table.Columns.Contains("ABPCP6")) { adresse.CodePostal = Outils.SiNull(ligne["ABPDP6"]).ToString() + Outils.SiNull(ligne["ABPCP6"]).ToString().PadLeft(3,'0'); } else if (ligne.Table.Columns.Contains("ABMCPO")) { adresse.CodePostal = Outils.SiNull(ligne["ABMCPO"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPVI6")) { adresse.Ville = Outils.SiNull(ligne["ABPVI6"]).ToString(); } else if (ligne.Table.Columns.Contains("ABLLIB")) { adresse.Ville = Outils.SiNull(ligne["ABLLIB"]).ToString(); }; //modif ville : type ville arrondissement -> remplacement ABLACH par ABLLIB
            if (ligne.Table.Columns.Contains("ABPDP6") && ligne.Table.Columns.Contains("ABPCEX")) { adresse.CodePostalCedex = Outils.SiNull(ligne["ABPDP6"]).ToString() + Outils.SiNull(ligne["ABPCEX"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPVIX")) { adresse.VilleCedex = Outils.SiNull(ligne["ABPVIX"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPCDX")) { adresse.TypeCedexEnBase = Convert.ToBoolean(Outils.SiNull(ligne["ABPCDX"]).ToString() == "O"); };
            if (ligne.Table.Columns.Contains("ABPPAY")) { adresse.Pays.Code = Outils.SiNull(ligne["ABPPAY"]).ToString(); } else if (ligne.Table.Columns.Contains("TCOD")) { adresse.Pays.Code = Outils.SiNull(ligne["TCOD"]).ToString(); };
            if (ligne.Table.Columns.Contains("TPLIB")) { adresse.Pays.Libelle = Outils.SiNull(ligne["TPLIB"]).ToString().Replace("Monde","France").Trim(); };
            if (ligne.Table.Columns.Contains("ABMMAT")) { adresse.MatriculeHexavia = IntOutilsHelper.ToNullableInt(Outils.SiNull(ligne["ABMMAT"]).ToString()); };
            if (ligne.Table.Columns.Contains("ABMLOC")) { adresse.INSEE = Outils.SiNull(ligne["ABMLOC"]).ToString(); };
            return adresse;
        }

        #endregion
    }
}
