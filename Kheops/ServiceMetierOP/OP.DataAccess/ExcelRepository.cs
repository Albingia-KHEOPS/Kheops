using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using ALBINGIA.Framework.Common.Data;

namespace OP.DataAccess
{
    /// <summary>
    /// Repository de gestion des des intéractions excel avec la base de données
    /// </summary>
    public class ExcelRepository
    {
        #region Méthodes publiques
        /// <summary>
        /// Excecute une requete de selection selon des paramètres
        /// </summary>
        /// <typeparam name="T">Type de l'opbjet de retour</typeparam>
        /// <param name="sql">requête select à éxecuter</param>
        /// <param name="hsqlParam">Liste de clés/Valeurs des paramètres de requeête</param>
        /// <returns>Une liste d'objets de type T mappé contenant le resultat d'exécution de la requête </returns>
        public static List<T> GetDataFromDB<T>(string sql, List<KeyValuePair<string, string>> hsqlParam) where T : class, new()
        {
            if (string.IsNullOrEmpty(sql))
                return null;
            //Remplacer chaque element de la chiane sql {0},{1} par sa valeur correspondante de la liste
            sql = hsqlParam.Aggregate(sql, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
            var res = DbBase.Settings.ExecuteList<T>(CommandType.Text, sql);
            return res;
        }

        /// <summary>
        /// Test l'existance de la ligne
        /// </summary>
        /// <param name="sql">requete sql à éxecuter</param>
        /// <param name="hsqlParam">Liste de clés/Valeurs des paramètres de requeête</param>
        /// <returns>True si l'objet existe sinon false</returns>
        public static bool ExistRow(string sql, IEnumerable hsqlParam)
        {

            sql = hsqlParam.Cast<KeyValuePair<string, string>>().Aggregate(sql, (current, elem) => current.Replace(elem.Key.ToString(CultureInfo.InvariantCulture), elem.Value.ToString(CultureInfo.InvariantCulture)));
          var firstOrDefault = DbBase.Settings.ExecuteList<WSAS400.DTO.Common.DtoCommon>(CommandType.Text, sql).FirstOrDefault();
          return firstOrDefault != null && firstOrDefault.NbLigne > 0;
        }
        /// <summary>
        /// MAJ de la table
        /// </summary>
        /// <param name="sql">requête de mise à jour (Insert/Update) à exécuter</param>
        /// <returns>True si la mise à jour a étét effectué avec succés sinon false</returns>
        public static bool UpdateDB(string sql)
        {
          return DbBase.Settings.ExecuteNonQuery(CommandType.Text, sql) > 0;
        }

        ///// <summary>
        ///// retour une liste qui va servir à alimenter une DropDownList
        ///// </summary>
        ///// <param name="sqlRequest">requete Sql select</param>
        ///// <returns>Liste d'objet Clé / valeur</returns>
        //public static List<T>  GetDropDownValue<T>(string sqlRequest) where T : new()
        //{
        //    return DbBase.Settings.ExecuteList<T>(CommandType.Text, sqlRequest);
        //}

        #endregion
    }
}
