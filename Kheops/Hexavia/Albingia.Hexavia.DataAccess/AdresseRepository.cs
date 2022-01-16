using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Albingia.Hexavia.DataAccess.Helper;
using System.Data;
using Albingia.Hexavia.CoreDomain;
using Albingia.Hexavia.Core;
using System.Runtime.Caching;
using NLog;
using Albingia.Hexavia.Core.Helper;

namespace Albingia.Hexavia.DataAccess
{
    public class AdresseRepository : BaseRepository
    {
        private static Logger perfLogger = LogManager.GetLogger("PerfLogger");

        /// <summary>
        /// Nombre d'adresse maximum.
        /// </summary>
        public const int MAX_ADRESSE = 1000;

        /// <summary>
        /// Obtient ou définie l'objet ReferenceRepository.
        /// </summary>
        public ReferenceRepository ReferenceRepository { get; set; }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Obtient ou définie le MemoryCache.
        /// </summary>
        public MemoryCache MemoryCache { get; set; }

        /// <summary>
        /// Obtient la liste de toutes les extensions.
        /// </summary>
        public List<CodeLibelle> Extension
        {
            get
            {
                List<CodeLibelle> result = MemoryCache["Extension"] as List<CodeLibelle>;
                if (result == null)
                {
                    result = new List<CodeLibelle>();

                    result.Add(new CodeLibelle { Code = "", Libelle = "" });
                    result.Add(new CodeLibelle { Code = "B", Libelle = "Bis" });
                    result.Add(new CodeLibelle { Code = "T", Libelle = "Ter" });
                    result.Add(new CodeLibelle { Code = "a", Libelle = "a" });
                    result.Add(new CodeLibelle { Code = "b", Libelle = "b" });
                    result.Add(new CodeLibelle { Code = "c", Libelle = "c" });
                    result.Add(new CodeLibelle { Code = "d", Libelle = "d" });
                    result.Add(new CodeLibelle { Code = "e", Libelle = "e" });
                    result.Add(new CodeLibelle { Code = "f", Libelle = "f" });

                    MemoryCache.Set("Extension", result, DateTimeOffset.Now.AddHours(6));
                }
                return result;
            }
        }

        /// <summary>
        /// Obtient la liste de tous les pays.
        /// </summary>
        public List<Pays> Pays
        {
            get
            {
                List<Pays> result = MemoryCache["Pays"] as List<Pays>;
                if (result == null)
                {
                    result = ReferenceRepository.Parametres("GENER", "CPAYS").Select(x => new Pays { Code = x.Code, Libelle = x.Libelle.ToString().Substring(2) }).ToList();
                    MemoryCache.Set("Pays", result, DateTimeOffset.Now.AddHours(6));
                }
                return result;
            }
        }

        public AdresseRepository(DataAccessManager dataAccessManager, MemoryCache memoryCache, ReferenceRepository referenceRepository)
            : base(dataAccessManager)
        {
            MemoryCache = memoryCache;
            ReferenceRepository = referenceRepository;
        }

        /// <summary>
        /// Permet d'obtenir une adresse par son numéro chrono.
        /// </summary>
        /// <param name="numeroChrono"></param>
        /// <returns></returns>
        public Adresse GetAdresseByNumeroChrono(int numeroChrono)
        {
            Adresse result = null;
            CmdWrapper cmd = dataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT ABPCHR, ABPNUM, ABPEXT, ABPLG3, ABPVI6, ABPVIX, ABPDP6, ABPCP6, ABPCEX, ABPCDX, ABPLG4 FROM YADRESS";
            cmd.Where("ABPCHR", numeroChrono, DbType.Int16);

            DataTable dataTable = dataAccessManager.ExecuteDataTable(cmd);
            if (dataTable.Rows.Count > 0)
            {
                result = GetAdresse(dataTable.Rows[0]);
            }
            return result;
        }

        /// <summary>
        /// Permet d'obtenir un numéro chrono, fournit par la base AS400.
        /// </summary>
        /// <returns></returns>
        public virtual int ObtenirNumeroChrono()
        {
            int result = -1;
            CmdWrapper cmd = dataAccessManager.CmdWrapper;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "*PGM/ALYDH105CL";
            cmd.AppendParameter(DbType.AnsiStringFixedLength, "P0CLE", "ADRESSE_CHRONO");
            cmd.AppendParameter(DbType.AnsiStringFixedLength, "P0EXT", "***");
            cmd.AppendParameter(DbType.AnsiStringFixedLength, "P0ACT", "INC");
            cmd.AppendParameter(DbType.Int16, "P0NUU", 0);
            cmd.AppendParameter(DbType.AnsiStringFixedLength, "P0RET", " ");
            dataAccessManager.ExecuteNonQuery(cmd);
            result = (int)((System.Data.EasycomClient.EacParameter)cmd.Parameters["P0NUU"]).Value;
            return result;
        }

        /// <summary>
        /// Permet d'enregistrer l'adresse dans la base.
        /// </summary>
        /// <param name="adresse"></param>
        public virtual void EnregistrerAdresse(Adresse adresse)
        {
            CmdWrapper cmd = dataAccessManager.CmdWrapper;
            List<Param> input = new List<Param>();

            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPLG3", adresse.Batiment));
            input.Add(new Param(DbType.Int16, "ABPNUM", adresse.NumeroVoie));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPEXT", adresse.ExtensionVoie));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPLG4", adresse.NomVoie));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPLG5", adresse.BoitePostale));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPLOC", adresse.CodePostal));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPDP6", adresse.Departement));
            input.Add(new Param(DbType.Int16, "ABPCP6", Convert.ToInt16(adresse.CodePostal.Last())));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPVI6", adresse.Ville));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPL4F", adresse.ConcatVoie));
            input.Add(new Param(DbType.AnsiStringFixedLength, "ABPL6F", adresse.CodePostal + " " + adresse.Ville));
            input.Add(new Param(DbType.Int16, "ABPCHR", adresse.NumeroChrono));

            cmd.InsertInto("YADRESS", input);
            dataAccessManager.ExecuteNonQuery(cmd);
        }

        //public virtual IEnumerable<Adresse> RechercheAdresseExacte(Adresse adresse, int startRow = 0, int endRow = 100)
        //{
        //    if (String.IsNullOrEmpty(adresse.CodePostal) && String.IsNullOrEmpty(adresse.Ville))
        //    {
        //        throw new AdresseException("RechercheAdresse necessite au moins le code postal ou la ville");
        //    }

        //    List<Adresse> result = new List<Adresse>();
        //    CmdWrapper cmd = dataAccessManager.CmdWrapper;
        //    cmd.CommandText = "SELECT ABLLOC, ABLCPO, ABLLIB, ABLACH, ABMLIB, ABMMAT, ABMCPO, ABMLOC FROM ( ";
        //    CreateRequeteRechercheAdresseExacte(cmd, adresse);
        //    cmd.CommandText += ") AS TABLE WHERE ID_NEXT BETWEEN " + startRow + " AND " + endRow + " ";

        //    DataTable dataTable = dataAccessManager.ExecuteDataTable(cmd);

        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        result.Add(GetAdresse(row));
        //    }
        //    return result;
        //}

        /// <summary>
        /// Permet de recuperé une liste d'adresses paginée.
        /// </summary>
        /// <param name="adresses"></param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public virtual IEnumerable<Adresse> RechercheAdresse(IEnumerable<Adresse> adresses, int startRow = 0, int endRow = 100, bool adrExacte = false)
        {
            foreach (Adresse adresse in adresses)
            {
                if (String.IsNullOrEmpty(adresse.CodePostal) && String.IsNullOrEmpty(adresse.Ville))
                {
                    throw new AdresseException("RechercheAdresse necessite au moins le code postal ou la ville");
                }
            }

            List<Adresse> result = new List<Adresse>();
            CmdWrapper cmd = dataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT ABLLOC, ABLCPO, ABLLIB, ABLACH, ABMLIB, ABMMAT, ABMCPO, ABMLOC FROM ( ";
            CreateRequeteRechercheAdresse(cmd, adresses, adrExacte);
            cmd.CommandText += ") AS TABLE WHERE ID_NEXT BETWEEN " + startRow + " AND " + endRow + " ";

            DataTable dataTable = dataAccessManager.ExecuteDataTable(cmd);

            foreach (DataRow row in dataTable.Rows)
            {
                result.Add(GetAdresse(row));
            }
            return result;
        }

        //public virtual int RechercheAdresseExacteCount(Adresse adresse)
        //{
        //    if (string.IsNullOrEmpty(adresse.CodePostal) && string.IsNullOrEmpty(adresse.Ville))
        //    {
        //        throw new AdresseException("RechercheAdresse necessite au moins le code postal ou la ville");
        //    }
        //    CmdWrapper cmd = dataAccessManager.CmdWrapper;
        //    cmd.CommandText = "SELECT COUNT (*) FROM ( ";
        //    CreateRequeteRechercheAdresseExacte(cmd, adresse);
        //    cmd.CommandText += ") AS TABLE";
        //    return (int)dataAccessManager.ExecuteScalar(cmd);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adresses"></param>
        /// <returns></returns>
        public virtual int RechercheAdresseCount(IEnumerable<Adresse> adresses, bool adrExacte = false)
        {
            if ((from Adr in adresses where String.IsNullOrEmpty(Adr.CodePostal) && String.IsNullOrEmpty(Adr.Ville) select Adr).Count() > 0)
            {
                throw new AdresseException("RechercheAdresse necessite au moins le code postal ou la ville");
            }
            CmdWrapper cmd = dataAccessManager.CmdWrapper;
            cmd.CommandText = "SELECT COUNT (*) FROM ( ";
            CreateRequeteRechercheAdresse(cmd, adresses, adrExacte);
            cmd.CommandText += ") AS TABLE";
            return (int)dataAccessManager.ExecuteScalar(cmd);
        }

        //private void CreateRequeteRechercheAdresseExacte(CmdWrapper cmd, Adresse adresse)
        //{
        //    //modif ville : type ville arrondissement ajout dans les 2 lignes de ABLLIB
        //    cmd.CommandText +=
        //        "SELECT rownumber() OVER (ORDER BY ABLACH) AS ID_NEXT, ABLLOC, ABLCPO, ABLLIB, ABLACH, ABMLIB, ABMMAT, ABMCPO, ABMLOC FROM (" +
        //        "SELECT ABLLOC, ABLCPO, ABLLIB, ABLACH FROM YHEXLOC WHERE ";

        //    cmd.CommandText += " ( ";
        //    //modif ville : type ville arrondissement
        //    cmd.CommandText += "ABLLIB = :ville ";
        //    cmd.AppendParameter(DbType.AnsiStringFixedLength, "ville", OutilsHelper.ToString(adresse.VilleSansCedex));

        //    if (adresse.CodePostal != null)
        //    {
        //        AjouteCodePostal(cmd, adresse, 1);
        //    }

        //    cmd.CommandText += ") OR ";

        //    cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 5);

        //    cmd.CommandText += ")) AS TABLE " +
        //                      "LEFT JOIN YHEXVIA ON ABMCPO=ABLCPO AND ABMLOC=ABLLOC ";

        //    cmd.CommandText += " WHERE (";
        //    cmd.CommandText += " ABMLIB LIKE :mot ";

        //    cmd.AppendParameter(DbType.AnsiStringFixedLength, "mot", "%" + adresse.NomVoie + "%");
        //    cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 2);
        //    cmd.CommandText += ")";
        //}

        private void CreateRequeteRechercheAdresse(CmdWrapper cmd, IEnumerable<Adresse> adresses, bool adrExacte)
        {
            List<Adresse> adressesList = adresses.ToList();
            //modif ville : type ville arrondissement ajout dans les 2 lignes de ABLLIB
            cmd.CommandText +=
                "SELECT rownumber() OVER (ORDER BY ABLACH) AS ID_NEXT, ABLLOC, ABLCPO, ABLLIB, ABLACH, ABMLIB, ABMMAT, ABMCPO, ABMLOC FROM (" +
                "SELECT ABLLOC, ABLCPO, ABLLIB, ABLACH FROM YHEXLOC WHERE ";

            for (int i = 0; i < adressesList.Count; i++)
            {
                Adresse adresse = adressesList[i];
                cmd.CommandText += " ( ";
                //modif ville : type ville arrondissement
                //cmd.CommandText += "ABLACH = :ville_" + i;
                cmd.CommandText += "ABLLIB = :ville_" + i;
                cmd.AppendParameter(DbType.AnsiStringFixedLength, "ville_" + i, OutilsHelper.ToString(adresse.VilleSansCedex));

                if (adresse.CodePostal != null)
                {
                    AjouteCodePostal(cmd, adresse, i);
                }

                cmd.CommandText += ") OR ";
            }

            cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 5);

            cmd.CommandText += ")) AS TABLE " +
                              "LEFT JOIN YHEXVIA ON ABMCPO=ABLCPO AND ABMLOC=ABLLOC ";

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
        /// Permet de recuperer une liste d'adresse suivant des criteres.
        /// </summary>
        /// <param name="codePostal"></param>
        /// <param name="motsVille"></param>
        /// <returns></returns>
        public virtual IEnumerable<Adresse> RechercheVille(string codePostal, IEnumerable<string> motsVille)
        {
            List<Adresse> result = new List<Adresse>();
            CmdWrapper cmdWrapper = dataAccessManager.CmdWrapper;
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

            DataTable dataTable = dataAccessManager.ExecuteDataTable(cmdWrapper);
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

        /// <summary>
        /// Permet de recuperer une adresse provenant d'une DataRow.
        /// </summary>
        /// <param name="ligne"></param>
        /// <returns></returns>
        public static Adresse GetAdresse(DataRow ligne)
        {
            Adresse adresse = new Adresse();
            adresse.Pays = new Pays();

            // TODO : verifier les nom des champs de la base
            if (ligne.Table.Columns.Contains("ABPCHR")) { adresse.NumeroChrono = Core.Helper.IntOutilsHelper.ToNullableInt(Helper.Outils.SiNull(ligne["ABPCHR"]).ToString()); } else if (ligne.Table.Columns.Contains("PBADH")) { adresse.NumeroChrono = Core.Helper.IntOutilsHelper.ToNullableInt(Helper.Outils.SiNull(ligne["PBADH"]).ToString()); };
            if (ligne.Table.Columns.Contains("ABPLG3")) { adresse.Batiment = Helper.Outils.SiNull(ligne["ABPLG3"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPEXT")) { adresse.ExtensionVoie = Helper.Outils.SiNull(ligne["ABPEXT"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPNUM")) { adresse.NumeroVoie = Helper.Outils.SiNull(ligne["ABPNUM"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPLG4")) { adresse.NomVoie = Helper.Outils.SiNull(ligne["ABPLG4"]).ToString(); } else if (ligne.Table.Columns.Contains("ABMLIB")) { adresse.NomVoie = Helper.Outils.SiNull(ligne["ABMLIB"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPLG5")) { adresse.BoitePostale = Helper.Outils.SiNull(ligne["ABPLG5"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPDP6") && ligne.Table.Columns.Contains("ABPCP6")) { adresse.CodePostal = Helper.Outils.SiNull(ligne["ABPDP6"]).ToString() + Helper.Outils.SiNull(ligne["ABPCP6"]).ToString(); } else if (ligne.Table.Columns.Contains("ABMCPO")) { adresse.CodePostal = Helper.Outils.SiNull(ligne["ABMCPO"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPVI6")) { adresse.Ville = Helper.Outils.SiNull(ligne["ABPVI6"]).ToString(); } else if (ligne.Table.Columns.Contains("ABLLIB")) { adresse.Ville = Helper.Outils.SiNull(ligne["ABLLIB"]).ToString(); }; //modif ville : type ville arrondissement -> remplacement ABLACH par ABLLIB
            if (ligne.Table.Columns.Contains("ABPDP6") && ligne.Table.Columns.Contains("ABPCEX")) { adresse.CodePostalCedex = Helper.Outils.SiNull(ligne["ABPDP6"]).ToString() + Helper.Outils.SiNull(ligne["ABPCEX"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPVIX")) { adresse.VilleCedex = Helper.Outils.SiNull(ligne["ABPVIX"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABPCDX")) { adresse.TypeCedexEnBase = Convert.ToBoolean(Helper.Outils.SiNull(ligne["ABPCDX"]).ToString() == "O"); };
            if (ligne.Table.Columns.Contains("ABPPAY")) { adresse.Pays.Code = Helper.Outils.SiNull(ligne["ABPPAY"]).ToString(); } else if (ligne.Table.Columns.Contains("TCOD")) { adresse.Pays.Code = Helper.Outils.SiNull(ligne["TCOD"]).ToString(); };
            if (ligne.Table.Columns.Contains("TLIB")) { adresse.Pays.Libelle = Helper.Outils.SiNull(ligne["TLIB"]).ToString(); };
            if (ligne.Table.Columns.Contains("ABMMAT")) { adresse.MatriculeHexavia = Core.Helper.IntOutilsHelper.ToNullableInt(Helper.Outils.SiNull(ligne["ABMMAT"]).ToString()); };
            if (ligne.Table.Columns.Contains("ABMLOC")) { adresse.INSEE = Helper.Outils.SiNull(ligne["ABMLOC"]).ToString(); };
            return adresse;
        }
    }
}
