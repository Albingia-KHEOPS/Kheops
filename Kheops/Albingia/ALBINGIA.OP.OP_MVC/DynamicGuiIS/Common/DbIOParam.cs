using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
namespace ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common
{
    public static class DbIOParam
    {

      /// <summary>
      /// Enregistre dynamiquement les infos des IS
      /// </summary>
      /// <param name="branche">Code branche</param>
      /// <param name="section">Code section</param>
      /// <param name="cible">Code cible</param>
      /// <param name="additionalParams">paramètres additionels</param>
      /// <param name="dataToSave">données à enregistrer</param>
      /// <param name="splitChars">caractère ou chaine de split</param>
      /// <param name="strParameters">paramètres</param>
      /// <returns></returns>
      public static bool SaveISToDB(string branche, string section, string cible, string additionalParams, string dataToSave, string splitChars, string strParameters)
      {
        if(string.IsNullOrEmpty(dataToSave))
          return true;
        if (!AlbSessionHelper.IsReadOnly() )
            {
                var parameters = DbIOParam.GetParams(HttpUtility.UrlDecode(strParameters), HttpUtility.UrlDecode(splitChars));
                //Format de données reçus :val1||cells1#**#val2||cells2#**#.....
                var idModele = DbIOParam.PrepareIsIdModele(HttpUtility.UrlDecode(branche), HttpUtility.UrlDecode(section));
                if (DbDataEntity.SetIsData(idModele, HttpUtility.UrlDecode(section), HttpUtility.UrlDecode(dataToSave).Replace("'","''"), parameters))
                {
                    return true;
                }
                return false;
            }
            return true;
      }
      /// <summary>
      /// Récupère les controls html à afficher à partir de la BD
      /// </summary>
      /// <param name="modele">ID du modèle encours</param>
      /// <returns></returns>

      public static ParamISInfo GetControlsFromDB(string modele)
        {
            var toReturn = new ParamISInfo {
              ParamISDBLignesInfo = new List<ParamISLigneInfo>()
              , Name = modele};


            OP_MVC.Common.CacheIS.AllISModeles.ForEach(elm =>
        {
          if (elm.ModeleID.ToLower() == modele.ToLower())
            toReturn.ParamISDBLignesInfo.Add(elm);
        });
        using ( var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
         {
                var serviceContext = chan.Channel;
                var lstModeleDto = serviceContext.GetISModeles(modele);
                var modeleDetails = lstModeleDto.FirstOrDefault();
              if (modeleDetails != null)
              {
                toReturn.SqlRequests = new SqlRequests
                  {
                    Insert = new Request {Sql = modeleDetails.SqlInsert},
                    Select = new Request {Sql = modeleDetails.SqlSelect},
                    Update = new Request {Sql = modeleDetails.SqlUpdate},
                    SelectExist = new Request {Sql = modeleDetails.SqlExist}
                  };
              }
            }
            return toReturn;
        }

        public static IEnumerable<object> GetParams(string parameters, string spliChars)
        {
            var lstParams = parameters.Split(new[] { spliChars }, StringSplitOptions.None);

            for (var i = 0; i < lstParams.Length; i++)
            {
                yield return new KeyValuePair<string, string>(i.ToString(CultureInfo.CurrentCulture), lstParams[i]);
            }
        }

      /// <summary>
      /// Vérifie si la section existe
      /// </summary>
      /// <param name="section">nom de la section</param>
      /// <returns></returns>
        public static bool IsExistSection(string section)
        {
            switch (section)
            {
                case AlbConstantesMetiers.INFORMATIONS_ENTETE:
                    return true;
                case AlbConstantesMetiers.INFORMATIONS_RISQUES:
                    return true;
                case AlbConstantesMetiers.INFORMATIONS_OBJETS:
                    return true;
                case AlbConstantesMetiers.INFORMATIONS_GARANTIES:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Préparation des paramètres pour la requ^te sql
        /// </summary>
        /// <param name="param">{ { "{P_0}", "O" }, { "{P_1}", "    23812" }}</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, string>>
            PrepareParameter(IEnumerable<object> param)
        {

            //new Dictionary<object, object> { { "{P_0}", "O" }, { "{P_1}", "    23812" }, { "{P_2}", "0" }, { "{P_3}", "1" }, { "{P_4}", "1" } };

            foreach (KeyValuePair<string, string> elem in param)
            {
                yield return new KeyValuePair<string, string>("{P_" + elem.Key+ "}", elem.Value);
            }
        }

        /// <summary>
        /// /retourne l'id Modèle IS
        /// </summary>
        /// <param name="branche">Branche utilisé</param>
        /// <param name="section">Section utilisé</param>
        /// <param name="cible">Cible utilisé</param>
        /// <param name="additionalParams">paramètre additionnal </param>
        /// <returns>ID du modèle</returns>
        public static string PrepareIsIdModele(string branche, string section, string cible="", string additionalParams="")
        {
          //TODO: cette méthode doit ere enrichie une fois spécifié le mode d'utilisation des paramètres 'cible' et 'addtionalParams' utilisés
          return string.Format("{0}-{1}", branche, section);
        }
    }
}