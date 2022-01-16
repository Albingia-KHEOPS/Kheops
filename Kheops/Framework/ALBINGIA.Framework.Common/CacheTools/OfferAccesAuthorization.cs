using System;

namespace ALBINGIA.Framework.Common.CacheTools
{
    public static class OfferAccesAuthorization
    {
      /// <summary>
      /// Vérifie si l'offre est en cache
      /// </summary>
      /// <param name="user">Utilisateur conecté</param>
      /// <param name="numOffre">numéro de l'offre</param>
      /// <param name="version">verson de l'offre</param>
      /// <param name="splitCaracter">caractère de split</param>
      /// <param name="lockUser"> </param>
      /// <param name="isIncache"></param>
      /// <returns></returns>
      public static bool OfferInCache(string user, string numOffre, string version, string type, string splitCaracter, out string lockUser,out bool isIncache)
        {

            return CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.IsInCache(user, numOffre, version, type, splitCaracter, out lockUser,out isIncache);
        }

      /// <summary>
      /// Vérouille l'offre
      /// </summary>
      /// <param name="user">Utilisateur connecté</param>
      /// <param name="numOffre">numéro  de l'offre</param>
      /// <param name="typeOffre"></param>
      /// <param name="version">version de l'offre</param>
      /// <param name="splitCaracter">caractère de split</param>
      /// <returns></returns>
        public static void VerouillerOffre(string user, string numOffre, string version, string typeOffre, string splitCaracter)
        {
            CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.SetIdOffre(user, numOffre, version,typeOffre, splitCaracter);
           
        }


       public static void RemoveCacheOffre(string splitCaracter)
       {
         CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.RemoveCacheOffre(splitCaracter);
       }

    }
}
