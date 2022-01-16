using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class Adresse
    {
        public int? NumeroChrono { get; set; }
        public string MatriculeHexavia { get; set; }
        public string Batiment { get; set; }
        public string NumeroVoie { get; set; }
        public string ExtensionVoie { get; set; }
        public string NomVoie { get; set; }
        public string BoitePostale { get; set; }
        public Ville Ville { get; set; }
        public Pays Pays { get; set; }

        ///// <summary>
        ///// Indique si la chaine d'entrée contient un cedex.
        ///// </summary>
        ///// <param name="villeOuBoitePostale"></param>
        ///// <returns></returns>
        //public static RetourDetectionCedex DetecterCedex(Adresse adresse)
        //{
        //    RetourDetectionCedex result = RetourDetectionCedex.CedexNonDétecté;
        //    if (adresse != null && adresse.Ville != null && !String.IsNullOrEmpty(adresse.Ville.Nom))
        //    {
        //        if (adresse.Ville.Nom.Split().Any(x => Ville.IdentifiantsCedex.Contains(x)))
        //        {
        //            result = RetourDetectionCedex.CedexDétecté;
        //        }
        //    }
        //    if (result == RetourDetectionCedex.CedexNonDétecté && adresse != null && !String.IsNullOrEmpty(adresse.BoitePostale))
        //    {
        //        if (adresse.BoitePostale.Split().Any(x => Ville.IdentifiantsCedex.Contains(x)))
        //        {
        //            result = RetourDetectionCedex.CedexDétectéNécessiteRemiseAuxNormes;
        //        }
        //    }
        //    return result;
        //}

        //public enum RetourDetectionCedex
        //{
        //    CedexDétecté,
        //    CedexNonDétecté,
        //    CedexDétectéNécessiteRemiseAuxNormes
        //}

        ///// <summary>
        ///// Copie tous les champs sauf ville et code postal de adresseModel vers adresseDestination
        ///// </summary>
        ///// <param name="adressesDestination"></param>
        ///// <returns></returns>
        //public List<Adresse> FusionneAdressesSaufVilleEtCodePostal(List<Ville> adressesDestination)
        //{
        //    return adressesDestination.Select(x =>
        //    {
        //        return new Adresse()
        //        {
        //            Batiment = this.Batiment,
        //            Ville = new Ville
        //            {
        //                CodePostal = x.CodePostal,
        //                CodePostalCedex = this.Ville.CodePostalCedex,
        //                Nom  = x.Nom
        //            },
        //            BoitePostale = this.BoitePostale,
        //            ExtensionVoie = this.ExtensionVoie,
        //            NomVoie = this.NomVoie,
        //            NumeroVoie = this.NumeroVoie,
        //            NumeroChrono = this.NumeroChrono,
        //            Pays = this.Pays
        //        };
        //    }).ToList();
        //}

        ///// <summary>
        ///// Extrait une collection filtrée, de la rue et du distributeur.
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<string> FiltreRueEtDistributionEnMots()
        //{
        //    List<string> result = FiltreDistribution();
        //    result.AddRange(Filtre(NomVoie));
        //    return result;
        //}

        ///// <summary>
        ///// Extrait une collection filtrée, du distributeur.
        ///// </summary>
        ///// <returns></returns>
        //public List<string> FiltreDistribution()
        //{
        //    List<string> result = Filtre(BoitePostale);
        //    return result;
        //}

        ///// <summary>
        ///// Extrait une collection de string à plus de 2 caracteres, <br/>
        ///// provenant de la chaine d'entrée, puis filtrée.
        ///// </summary>
        ///// <param name="chaine"></param>
        ///// <returns></returns>
        //private List<string> Filtre(string chaine)
        //{
        //    List<string> result = new List<string>();
        //    if (!String.IsNullOrEmpty(chaine))
        //    {
        //        IEnumerable<string> chaineSeparee = chaine.Split().Where(x => x.Length > 2);
        //        result = Filtre(chaineSeparee).ToList();
        //    }
        //    return result;
        //}

        //private static List<string> motsIgnores = new List<string> { "RUE", "BLD", "BD", "AVE", "AV", "AVENUE", "BOULEVARD", "RTE", "ROUTE", "DU", "DES", "LES", "LE", "LA", "ET", "DE", "L'", "D'", "Z'", "ST", "ZI", "PLACE", "QUAI", "À", "LIEU", "DIT", "SAINT", "SAINT-", "CEDEX" };

        ///// <summary>
        ///// Permet de filtrer les mots interdit (RUE, BLD...),<br/>
        ///// sur une collection generique de string.
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //private IEnumerable<string> Filtre(IEnumerable<string> input)
        //{
        //    return input.Where(x => !motsIgnores.Contains(x));
        //}

    }
}
