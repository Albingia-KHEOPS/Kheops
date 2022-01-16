using ALBINGIA.Framework.Common.Data;
using DataAccess.Helpers;
using OP.WSAS400.DTO.Adresses;
using System;
using System.Configuration;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;

namespace OP.DataAccess {
    public class AdresseRepository
    {
        private const int MAX_ADRESSE = 1000;
        internal static readonly object GeolocId = ConfigurationManager.AppSettings["GeolocIdColumn"] ?? "KGEOCHR";

        public static AdressePlatDto Initialiser(DataRow ligne, string prefixe = "")
        {
            if (!string.IsNullOrEmpty(prefixe))
            {
                ligne = RetirerPrefixeDesColonnes(ligne, prefixe);
            }

            AdressePlatDto adresse = null;
            if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "ABPCHR", "PBADH", "ABPDP6", "ABPCP6", "ABPVI6", "TCCP", "JFADH"))
            {
                adresse = new AdressePlatDto();
                int numeroChrono = 0;
                int numVoie = 0;
                if (ligne.Table.Columns.Contains("ABMMAT")) { adresse.MatriculeHexavia = ligne["ABMMAT"].ToString(); };
                if (ligne.Table.Columns.Contains("ABPCHR"))
                {
                    if (Int32.TryParse(ligne["ABPCHR"].ToString(), out numeroChrono)) { adresse.NumeroChrono = numeroChrono; }
                }
                else if (ligne.Table.Columns.Contains("PBADH"))
                {
                    if (Int32.TryParse(ligne["PBADH"].ToString(), out numeroChrono)) { adresse.NumeroChrono = numeroChrono; }
                }
                else if (ligne.Table.Columns.Contains("JFADH"))
                {
                    if (Int32.TryParse(ligne["JFADH"].ToString(), out numeroChrono)) { adresse.NumeroChrono = numeroChrono; }
                };
                if (ligne.Table.Columns.Contains("ABPLG3")) { adresse.Batiment = ligne["ABPLG3"].ToString(); };
                if (ligne.Table.Columns.Contains("ABPEXT")) { adresse.ExtensionVoie = ligne["ABPEXT"].ToString(); };
                if (ligne.Table.Columns.Contains("ABPNUM") && ligne["ABPNUM"].ToString() != "0") { adresse.NumeroVoie = Int32.TryParse(ligne["ABPNUM"].ToString(), out numVoie) ? numVoie : numVoie; };
                if (ligne.Table.Columns.Contains("ABPLG4")) { adresse.NomVoie = ligne["ABPLG4"].ToString(); } else if (ligne.Table.Columns.Contains("ABMLIB")) { adresse.NomVoie = ligne["ABMLIB"].ToString(); };
                if (ligne.Table.Columns.Contains("ABPLG5")) { adresse.BoitePostale = ligne["ABPLG5"].ToString(); };

                if (!String.IsNullOrEmpty(prefixe))
                {
                    ligne = RetirerPrefixeDesColonnes(ligne, prefixe);
                }

                if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "ABPDP6", "ABPCP6", "ABPVI6", "ABPCEX", "ABPVIX", "ABLCPO", "ABMCPO", "TCCP", "JFVIL", "JFDEP", "JFCPO"))
                {
                    int cp = 0;
                    int cpCedex = 0;
                    if (ligne.Table.Columns.Contains("ABPDP6") && ligne["ABPDP6"].ToString() != String.Empty && ligne.Table.Columns.Contains("ABPCP6")) { adresse.CodePostal = Int32.TryParse(ligne["ABPDP6"].ToString() + ligne["ABPCP6"].ToString().PadLeft(3, '0'), out cp) ? cp : cp; } else if (ligne.Table.Columns.Contains("ABLCPO")) { adresse.CodePostal = Int32.TryParse(ligne["ABLCPO"].ToString(), out cp) ? cp : cp; } else if (ligne.Table.Columns.Contains("ABMCPO")) { adresse.CodePostal = Int32.TryParse(ligne["ABMCPO"].ToString(), out cp) ? cp : cp; } else if (ligne.Table.Columns.Contains("TCCP")) { adresse.CodePostal = Int32.TryParse(ligne["TCCP"].ToString(), out cp) ? cp : cp; } else if (ligne.Table.Columns.Contains("JFCPO") && ligne.Table.Columns.Contains("JFVIL")) { adresse.CodePostal = Int32.TryParse(ligne["JFDEP"].ToString() + ligne["JFCPO"].ToString().PadLeft(3, '0'), out cp) ? cp : cp; };
                    if (ligne.Table.Columns.Contains("ABPVI6")) { adresse.NomVille = ligne["ABPVI6"].ToString(); } else if (ligne.Table.Columns.Contains("ABLACH")) { adresse.NomVille = ligne["ABLACH"].ToString(); } else if (ligne.Table.Columns.Contains("TCVILC")) { adresse.NomVille = ligne["TCVILC"].ToString(); } else if (ligne.Table.Columns.Contains("JFVIL")) { adresse.NomVille = ligne["JFVIL"].ToString(); };
                    if (ligne.Table.Columns.Contains("ABPDP6") && ligne.Table.Columns.Contains("ABPCEX")) { adresse.CodePostalCedex = Int32.TryParse(ligne["ABPDP6"].ToString() + ligne["ABPCEX"].ToString().PadLeft(3, '0'), out cpCedex) ? cpCedex : cpCedex; };
                    if (ligne.Table.Columns.Contains("ABPVIX")) { adresse.NomCedex = ligne["ABPVIX"].ToString(); };
                    if (ligne.Table.Columns.Contains("ABPCDX")) { adresse.TypeCedex = ligne["ABPCDX"].ToString(); };
                    if (ligne.Table.Columns.Contains("ABMLOC")) { adresse.CodeInsee = ligne["ABMLOC"].ToString(); } else if (ligne.Table.Columns.Contains("ABLLOC")) { adresse.CodeInsee = ligne["ABLLOC"].ToString(); };
                }

                if (!String.IsNullOrEmpty(prefixe))
                {
                    ligne = RetirerPrefixeDesColonnes(ligne, prefixe);
                }

                if (OutilsHelper.ContientUnDesChampsEtEstNonNull(ligne, "ABPPAY", "TCOD", "JFPAY"))
                {
                    if (ligne.Table.Columns.Contains("ABPPAY")) { adresse.CodePays = ligne["ABPPAY"].ToString(); } else if (ligne.Table.Columns.Contains("TCOD")) { adresse.CodePays = ligne["TCOD"].ToString(); } else if (ligne.Table.Columns.Contains("JFPAY")) { adresse.CodePays = ligne["JFPAY"].ToString(); };
                    if (ligne.Table.Columns.Contains("TPLIB")) { adresse.NomPays = ligne["TPLIB"].ToString(); };
                }
            }
            return adresse;
        }

        public static AdressePlatDto ObtenirAdresse(int idAdresse)
        {
            string sql = $@"SELECT YADRESS.ABPCHR NUMEROCHRONO, ABPNUM NUMEROVOIE, ABPLBN NUMEROVOIE2, ABPEXT EXTENSIONVOIE, ABPLG3 BATIMENT, ABPVI6 NOMVILLE, ABPVIX NOMCEDEX, ABPDP6 DEPARTEMENT, ABPCP6 CODEPOSTAL, ABPCEX CODEPOSTALCEDEX, ABPCDX TYPECEDEX, ABPLG4 NOMVOIE, ABPLG5 BOITEPOSTALE,TCOD CODEPAYS, TPLIB NOMPAYS, ABPMAT MATHEX, LATITUDE, LONGITUDE
                                FROM YADRESS LEFT JOIN KGEOLOC ON KGEOLOC.{GeolocId} = YADRESS.ABPCHR";
            sql += CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "CPAYS", otherCriteria: " AND TCOD = ABPPAY");
            sql += string.Format(" WHERE YADRESS.ABPCHR='{0}'", idAdresse);

            var adressePlatDto = DbBase.Settings.ExecuteList<AdressePlatDto>(CommandType.Text, sql).FirstOrDefault();
            return adressePlatDto;//GetAdresse(adressePlatDto);
        }

        public static AdressePlatDto ObtenirAdresse(string codeOffre, string version, string type)
        {
            string sql = $@"SELECT ABPNUM NUMEROVOIE, ABPLBN NUMEROVOIE2, ABPEXT EXTENSIONVOIE, ABPLG3 BATIMENT, ABPVI6 NOMVILLE, ABPVIX NOMCEDEX, ABPDP6 DEPARTEMENT, ABPCP6 CODEPOSTAL, ABPCEX CODEPOSTALCEDEX, ABPCDX TYPECEDEX, ABPLG4 NOMVOIE, ABPLG5 BOITEPOSTALE,TCOD CODEPAYS, TPLIB NOMPAYS, ABPMAT MATHEX, LATITUDE, LONGITUDE
                            FROM YPOBASE
                            INNER JOIN YADRESS ON YADRESS.ABPCHR = PBADH
                            LEFT JOIN KGEOLOC ON YADRESS.ABPCHR = KGEOLOC.{GeolocId}";
            sql += CommonRepository.BuildJoinYYYYPAR("LEFT", "GENER", "CPAYS", otherCriteria: " AND TCOD = ABPPAY");
            sql += "WHERE PBIPB = :CODEOFFRE AND PBALX = :VERSION AND PBTYP = :TYPE AND PBADH <> 0;";

            EacParameter[] param = new EacParameter[3];
            param[0] = new EacParameter("CODEOFFRE", DbType.AnsiStringFixedLength);
            param[0].Value = codeOffre.PadLeft(9, ' ');
            param[1] = new EacParameter("VERSION", DbType.Int32);
            param[1].Value = int.Parse(version);
            param[2] = new EacParameter("TYPE", DbType.AnsiStringFixedLength);
            param[2].Value = type;




            var adressePlatDto = DbBase.Settings.ExecuteList<AdressePlatDto>(CommandType.Text, sql,param).FirstOrDefault();
            return adressePlatDto;//GetAdresse(adressePlatDto);
        }

        public static DataRow RetirerPrefixeDesColonnes(DataRow ligne, string prefixe)
        {
            //on copie la ligne
            DataTable tableCopie = ligne.Table.Clone();
            DataRow result = tableCopie.NewRow();
            result.ItemArray = ligne.ItemArray;

            //on enleve les prefixe
            foreach (DataColumn column in result.Table.Columns)
            {
                column.ColumnName = column.ColumnName.Replace(prefixe, String.Empty);
            }
            return result;
        }
    }
}
