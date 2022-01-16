using Hexavia.Tools.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hexavia.Models
{
    /// <summary>
    /// Représente une adresse.
    /// </summary>
    [Serializable]
    public class Adresse
    {
        private static List<string> motsIgnores = new List<string> { "RUE", "BLD", "BD", "AVE", "AV", "AVENUE", "BOULEVARD", "RTE", "ROUTE", "DU", "DES", "LES", "LE", "LA", "ET", "DE", "L'", "D'", "Z'", "ST", "STE", "ZI", "PLACE", "QUAI", "À", "LIEU", "DIT", "SAINT", "SAINT-", "SAINTE", "SAINTE-", "CEDEX" };
        private static List<string> cedex = new List<string> { "CEDEX", "CDX" };

        /// <summary>
        /// Obtient ou définie le numéro chrono.<br/>
        /// Mappé sur YADRESS.ABPCHR
        /// </summary>
        public int? NumeroChrono
        {
            get;
            set;
        }
        public string AdresseComplete
        {
            get
            {


                return GetCompleteAddress();
            }
            set
            {
                AdresseComplete = value;
            }
        }

        public string AdresseCompleteSansCP
        {
            get
            {


                return GetCompleteAddressWithoutCp();
            }
            set
            {
                AdresseCompleteSansCP = value;
            }
        }

        public string AdresseComplete4
        {
            get
            {
               

                return GetAddress(NumeroVoie,extensionVoie,nomVoie);
            }
            set
            {
                AdresseComplete4 = value;
            }
        }

        public string AdresseCompleteSansCP4
        {
            get
            {


                return GetAddressWithoutCp(NumeroVoie, extensionVoie, nomVoie);
            }
            set
            {
                AdresseCompleteSansCP4 = value;
            }
        }
        public string AdresseComplete3
        {
            get
            {


                return GetAddress(null, null, Batiment);
            }
            set
            {
                AdresseComplete3 = value;
            }
        }

        public string AdresseCompleteSansCP3
        {
            get
            {


                return GetAddressWithoutCp(null, null, Batiment);
            }
            set
            {
                AdresseCompleteSansCP3 = value;
            }
        }
        public string AdresseComplete5
        {
            get
            {


                return GetAddress(null, null, BoitePostale);
            }
            set
            {
                AdresseComplete5 = value;
            }
        }

        public string AdresseCompleteSansCP5
        {
            get
            {


                return GetAddressWithoutCp(null, null, BoitePostale);
            }
            set
            {
                AdresseCompleteSansCP5 = value;
            }
        }

        public string UrlExterieur { get; set; }
        public char Separateur { get; set; }

        private string _batiment;
        /// <summary>
        /// Obtient ou définie le libellé du batiment,<br/>
        /// qui ne doit pas dépasser les 32 caractères.<br/>
        /// Champ de saisie Bâtiment / Zone Industrielle(L3): YADRESS.ABPLG3
        /// </summary>
        public string Batiment
        {
            get { return _batiment; }
            set
            {
                _batiment = Normalise(value);
                if (!String.IsNullOrEmpty(value) && value.Length > 32)
                {
                    throw new AdresseException("L'adresse Batiment doit être inférieur à 32 caractères");
                }
            }
        }

        /// <summary>
        /// Obtient ou définie le numéro de la voie.<br/>
        /// champ de saisie No Voie(L4): YADRESS.ABPLBN
        /// </summary>
        public string NumeroVoie
        {
            get;
            set;
        }

        public string NumeroVoie2
        {
            get;
            set;
        }

        private string extensionVoie;
        /// <summary>
        /// Obtient ou définie l'extension de la voie (Bis, Ter...),<br/>
        /// retour en majuscule.<br/>
        /// champ de saisie Extension de voie(L4): YADRESS.ABPEXT
        /// </summary>
        public string ExtensionVoie
        {
            get { return extensionVoie; }
            set { if (!String.IsNullOrEmpty(value)) { extensionVoie = value.ToUpper(); } }
        }

        private string nomVoie;
        /// <summary>
        /// Obtient ou définie le nom de la voie,<br/>
        /// retour en majuscule, sans accents.<br/>
        /// champ de saisie Nom de la voie (L4): YADRESS.ABPLG4
        /// </summary>
        public string NomVoie
        {
            get { return nomVoie; }
            set { nomVoie = Normalise(value); }
        }

        private string _boitepostale;
        /// <summary>
        /// Obtient ou définie la distribution,<br/>
        /// retour en majuscule, sans accents.<br/>
        /// champ de saisie Distribution, Boite Postale (L5): YADRESS.ABPLG5
        /// </summary>
        public string BoitePostale
        {
            get { return _boitepostale; }
            set { _boitepostale = Normalise(value); }
        }

        private string codePostal;
        /// <summary>
        /// Obtient ou définie le code postal,<br/>
        /// qui ne doit pas dépasser les 5 caractères.<br/>
        /// champ de saisie Code Postal(L6):  YADRESS.ABPDP6 + YADRESS.ABPCP6
        /// </summary>
        public string CodePostal
        {
            get { return codePostal; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    codePostal = value;
                }
            }
        }

        /// <summary>
        /// Obtient les 3 derniers chiffres du code postal.<br/>
        /// mappe a YPOBASE.PBCPO
        /// </summary>
        public string FinCodePostal
        {
            get
            {
                string result = null;
                if (!String.IsNullOrEmpty(CodePostal) && CodePostal.Length == 5)
                {
                    result = CodePostal.Substring(2, 3);
                }
                return result;
            }
        }

        private string ville;
        /// <summary>
        /// Obtient ou définie le libellé de la ville,<br/>
        /// qui ne doit pas dépasser les 38 caractères.<br/>
        /// champ de saisie Ville(L6): YADRESS.ABPVI6
        /// </summary>
        public string Ville
        {
            get { return ville; }
            set
            {
                ville = Normalise(value);
                if (!String.IsNullOrEmpty(Ville) && Ville.Length > 38)
                {
                    throw new AdresseException("la ville ne doit pas depasser 38 caracteres: " + ville);
                }
            }
        }

        /// <summary>
        /// Obtient le libelle de la ville,<br/>
        /// si elle n'est pas en CEDEX (sinon NULL).
        /// </summary>
        public string VilleSansCedex
        {
            get
            {
                string result = null;
                if (!String.IsNullOrEmpty(Ville) && !ville.Equals(cedex[0]) && !ville.Equals(cedex[1]))
                {
                    result = Ville.Split().Where(x => !cedex.Contains(x)).Aggregate((x, y) => x + " " + y);

                    if (String.IsNullOrWhiteSpace(result))
                    {
                        result = null;
                    }
                }
                return result;
            }
        }

        private string codePostalCedex;
        /// <summary>
        /// Obtient ou définie le code postal CEDEX,<br/>
        /// qui ne doit pas dépasser 5 caractères.<br/>
        /// champ de saisie Code postal Cedex(L6): YADRESS.ABPDP6 + YADRESS.ABPCEX
        /// </summary>
        public string CodePostalCedex
        {

            get { return codePostalCedex; }
            set
            {
                codePostalCedex = value;
                //if (!String.IsNullOrEmpty(codePostalCedex) && codePostalCedex.Length > 5)
                //{
                //    throw new AdresseException("le code postal cedex ne doit pas depasser 5 caracteres: " + codePostalCedex);
                //}
            }
        }

        private string villeCedex;
        /// <summary>
        /// Obtient ou définie le libellé de la ville, avec CEDEX,<br/>
        /// qui ne doit pas dépasser les 38 caractères.<br/>
        /// champ de saisie Ville(L6): YADRESS.ABPVIX
        /// </summary>
        public string VilleCedex
        {
            get { return villeCedex; }
            set
            {
                villeCedex = Normalise(value);
                if (!String.IsNullOrEmpty(Ville) && Ville.Length > 38)
                {
                    throw new AdresseException("la ville ne doit pas depasser 38 caracteres: " + ville);
                }
            }
        }

        /// <summary>
        /// Obtient le numéro du département, à partir du code postal.<br/>
        /// champ de saisie Code Postal(L6): YADRESS.ABPDP6
        /// </summary>
        public string Departement
        {
            get
            {
                string result = String.Empty;
                //if (!String.IsNullOrEmpty(CodePostal) && CodePostal.Length >= 2)
                //{
                //    result = CodePostal.Substring(0, 2);
                //}
                return result;
            }
        }

        private Pays pays;
        /// <summary>
        /// Obtient ou définie le pays.
        /// </summary>
        public Pays Pays
        {
            get
            {
                if ((pays == null) || (string.IsNullOrWhiteSpace(pays.Libelle)))
                {
                    pays = new Pays { Code = "", Libelle = "" };
                    //pays = new Pays { Code = "Fr", Libelle = "France" };
                }
                return pays;
            }
            set
            {
                pays = value;
            }
        }

        /// <summary>
        /// Obtient ou définie le numéro de matricule Hexavia.<br/>
        /// mappé de HEXAVIA.ABMMAT vers YADRESS.ABPMAT
        /// </summary>
        public int? MatriculeHexavia
        {
            get;
            set;
        }

        private string insee;
        /// <summary>
        /// Obtient ou définie le Code INSEE.<br/>
        /// qui ne doit pas dépasser les 8 caractères.<br/>
        /// mappé de HEXAVIA.ABMLOC vers YADRESS.ABPLOC
        /// </summary>
        public string INSEE
        {
            get { return insee; }
            set
            {
                if (!String.IsNullOrEmpty(value) && value.Length > 8)
                {
                    throw new AdresseException("l'insee doit être inférieur a 8 characteres, ici: " + value);
                }
                insee = value;
            }
        }

        /// <summary>
        /// Obtient l'adresse avec le numéro, l'extension et le nom de la voie.
        /// </summary>
        public string ConcatVoie
        {
            get
            {
                string result = NumeroVoie + " " + ExtensionVoie + " " + nomVoie;
                return result.Trim();
            }
        }

        /// <summary>
        /// Copie tous les champs sauf ville et code postal de adresseModel vers adresseDestination
        /// </summary>
        /// <param name="adressesDestination"></param>
        /// <returns></returns>
        public IEnumerable<Adresse> FusionneAdressesSaufVilleEtCodePostal(IEnumerable<Adresse> adressesDestination)
        {
            return adressesDestination.Select(x =>
            {
                return new Adresse()
                {
                    Batiment = this.Batiment,
                    CodePostal = x.CodePostal,
                    CodePostalCedex = this.CodePostalCedex,
                    BoitePostale = this.BoitePostale,
                    ExtensionVoie = this.ExtensionVoie,
                    NomVoie = this.NomVoie,
                    NumeroVoie = this.NumeroVoie,
                    NumeroChrono = this.NumeroChrono,
                    Pays = this.Pays,
                    Ville = x.ville
                };
            });
        }

        /// <summary>
        /// Extrait une collection filtrée, de la rue et du distributeur.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FiltreRueEtDistributionEnMots()
        {
            List<string> result = FiltreDistribution();
            result.AddRange(Filtre(nomVoie));
            return result;
        }

        /// <summary>
        /// Extrait une collection filtrée, du distributeur.
        /// </summary>
        /// <returns></returns>
        public List<string> FiltreDistribution()
        {
            List<string> result = Filtre(BoitePostale);
            return result;
        }

        /// <summary>
        ///  Extrait une collection filtrée, de la ville.
        /// </summary>
        /// <param name="paramVille">ville saisie</param>
        /// <returns></returns>
        public List<string> FiltreVille(string paramVille)
        {
            List<string> result = Filtre(paramVille);
            return result;
        }

        /// <summary>
        /// Extrait une collection de string à plus de 2 caracteres, <br/>
        /// provenant de la chaine d'entrée, puis filtrée.
        /// </summary>
        /// <param name="chaine"></param>
        /// <returns></returns>
        private List<string> Filtre(string chaine)
        {
            List<string> result = new List<string>();
            if (!String.IsNullOrEmpty(chaine))
            {
                IEnumerable<string> chaineSeparee = chaine.Split().Where(x => x.Length > 2);
                result = Filtre(chaineSeparee).ToList();
            }
            return result;
        }

        /// <summary>
        /// Permet à une chaine de caractères de passer en majuscule et de supprimer ses accents.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string Normalise(string input)
        {
            string result = input;
            if (!String.IsNullOrEmpty(input))
            {
                result = input.ToUpper();
                result = EnleveAccentsEtApostrophe(result);
            }
            return result;
        }

        /// <summary>
        /// Permet de filtrer les mots interdit (RUE, BLD...),<br/>
        /// sur une collection generique de string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IEnumerable<string> Filtre(IEnumerable<string> input)
        {
            return input.Where(x => !motsIgnores.Contains(x));
        }

        /// <summary>
        /// Permet d'enlever les accents et les apostrophes à la chaine d'entrée.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string EnleveAccentsEtApostrophe(string input)
        {
            byte[] aOctets = System.Text.Encoding.GetEncoding(1251).GetBytes(input);
            String res = System.Text.Encoding.ASCII.GetString(aOctets);
            res = res.Replace('\'', ' ');
            res = res.Replace('-', ' ');
            return res;
        }

        /// <summary>
        /// Indique si la ville ou la distribution contient un cedex.
        /// </summary>
        public bool? IsCedex
        {
            get
            {
                bool? result = false;
                result = HasCedex(Ville).Value || HasCedex(BoitePostale).Value;
                return result;
            }
        }

        /// <summary>
        /// Obtient ou définie l'indication du type cedex.
        /// </summary>
        public bool? TypeCedexEnBase
        {
            get;
            set;
        }

        /// <summary>
        /// Obtient ou définie la latitude.
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// Obtient ou définie la longitude.
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Indique si la chaine d'entrée contient un cedex.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool? HasCedex(string input)
        {
            bool? result = false;
            if (!String.IsNullOrEmpty(input))
            {
                result = input.Split().Any(x => cedex.Contains(x));
            }
            return result;
        }
        private string GetCity()
        {
            var city = ville ?? String.Empty;

            if (city.IndexOf("cedex", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                city = Regex.Replace(city, "cedex", " ", RegexOptions.IgnoreCase);
            }

            if (Regex.Match(city, @"\d{2}$").Success)
            {
                city = city.Substring(0, city.Length - 2);
            }

            if (Regex.Match(city, @"\d{1}$").Success)
            {
                city = city.Substring(0, city.Length - 1);
            }
            return city;
        }
        private string GetAddress(string number,string extension, string name)
        {
            var city = GetCity();

            var adresse = "";
            if ((!string.IsNullOrEmpty(city) || (string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(codePostal))) && !string.IsNullOrEmpty(name))
            {
                adresse = ((string.IsNullOrWhiteSpace(number) || number.Trim() == "0") ? string.Empty : number)
               + " " + (extension ?? string.Empty)
               + " " + (name ?? string.Empty)
               + " " + (codePostal != null && codePostal.Length == 5 ? codePostal : string.Empty)
               + " " + city
               + " " + Pays.Libelle;
            }
            return adresse.Trim();

        }
        private string GetAddressWithoutCp(string number, string extension, string name)
        {

            var city = GetCity();

            var adresse = "";
            if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(name))
            {
                adresse = ((string.IsNullOrWhiteSpace(number) || NumeroVoie.Trim() == "0") ? string.Empty : number)
                 + " " + (extension ?? string.Empty)
                 + " " + (name ?? string.Empty)
                 + " " + city
                 + " " + Pays.Libelle;
            }
            return adresse.Trim(); ;

        }


        private string GetCompleteAddress()
        {
                   
              var  adresse = $"{Batiment ?? string.Empty} {NumeroVoie ?? string.Empty} {ExtensionVoie ?? string.Empty} {NomVoie ?? string.Empty}  {BoitePostale ?? string.Empty}" + //
                         $"{(CodePostal != null && CodePostal.Length == 5 ? CodePostal : string.Empty)} {GetCity()}".Trim();
                     
            if (!string.IsNullOrEmpty(adresse))
            {
                adresse = $"{adresse} {Pays.Libelle}";
            }
            return adresse.Trim();

        }
        private string GetCompleteAddressWithoutCp()
        {

            var adresse = $"{Batiment ?? string.Empty} {NumeroVoie ?? string.Empty} {ExtensionVoie ?? string.Empty} {NomVoie ?? string.Empty} {BoitePostale ?? string.Empty} " + //
                          $" {GetCity()}".Trim();

            if (!string.IsNullOrEmpty(adresse))
            {
                adresse = $"{adresse} {Pays.Libelle}";
            }
            return adresse.Trim();

        }
    }
}
