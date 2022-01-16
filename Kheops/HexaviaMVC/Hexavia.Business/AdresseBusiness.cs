using Hexavia.Business.Interfaces;
using Hexavia.Repository.Interfaces;
using Hexavia.Models;
using Hexavia.Models.EnumDir;
using System.Collections.Generic;
using System.Linq;
using Hexavia.Tools.Exceptions;
using System;
using System.Net;
using Hexavia.Tools.Helpers;
using System.Globalization;
using Hexavia.Models.GoogleMaps;

namespace Hexavia.Business
{
    public class AdresseBusiness : IAdresseBusiness
    {
        private readonly IAddressRepository AdresseRepository;
        private const int MAX_ADRESSE = 1000;

        /// <summary>
        /// AdresseBusiness
        /// </summary>
        /// <param name="addressRepository"></param>
        public AdresseBusiness(IAddressRepository addressRepository)
        {
            AdresseRepository = addressRepository;
        }

        public List<AdresseLight> GetAllAdresses()
        {
            var adresses = AdresseRepository.GetAllAdresses();
            return adresses.Select(a => new AdresseLight
            {
                AdresseComplete4 = a.AdresseComplete4,
                AdresseCompleteSansCP4 = a.AdresseCompleteSansCP4,
                NumeroChrono = a.NumeroChrono
            }).ToList();
        }

        public List<AdresseLight> GetAllAdressesNotGeolocalisated(int pageNumber = 0, int pageSize = 0)
        {
            IList<Adresse> adresses = new List<Adresse>();
            if (pageNumber == 0 && pageSize == 0)
            {
                adresses = AdresseRepository.GetAllAdressesNotGeolocalisated();
            }
            else
            {
                adresses = AdresseRepository.GetAllAdressesNotGeolocalisatedByPage(pageNumber, pageSize);
            }
            return adresses.Select(a => new AdresseLight
            {
                AdresseComplete = a.AdresseComplete,
                AdresseCompleteSansCP = a.AdresseCompleteSansCP,
                AdresseComplete4 = a.AdresseComplete4,
                AdresseCompleteSansCP4 = a.AdresseCompleteSansCP4,
                AdresseComplete3 = a.AdresseComplete3,
                AdresseCompleteSansCP3 = a.AdresseCompleteSansCP3,
                AdresseComplete5 = a.AdresseComplete5,
                AdresseCompleteSansCP5 = a.AdresseCompleteSansCP5,
                NumeroChrono = a.NumeroChrono
            }).ToList();
        }

        /// <summary>
        /// VerifieAdresse
        /// </summary>
        /// <param name="adresse"></param>
        /// <returns></returns>
        public ResultatRechercheAdresse VerifieAdresse(Adresse adresse)
        {
            ResultatRechercheAdresse result = ResultatRechercheAdresse.ERREUR;
            int nb = AdresseRepository.RechercheAdresseCount(new List<Adresse> { adresse });
            if (nb == 1)
            {
                result = ResultatRechercheAdresse.OK;
            }
            else if (nb > 1)
            {
                result = ResultatRechercheAdresse.MULTIPLE;
            }
            return result;
        }

        /// <summary>
        /// GetAdresseByNumeroChrono
        /// </summary>
        /// <param name="numeroChrono"></param>
        /// <returns></returns>
        public Adresse GetAdresseByNumeroChrono(int numeroChrono)
        {
            if (numeroChrono <= 0)
            {
                throw new AdresseException("Invalid numeroChrono");
            }
            return AdresseRepository.GetAdresseByNumeroChrono(numeroChrono);
        }

        /// <summary>
        /// RechercheAdresse
        /// </summary>
        /// <param name="adresseRecherchee"></param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public AdressesWrapper RechercheAdresse(Adresse adresseRecherchee, int startRow, int endRow)
        {
            var result = new AdressesWrapper { Adresses = new List<Adresse>() };
            var adressesDeVille = VillesQuiConviennent(adresseRecherchee, result);

            //si le nombre de ville atteint est le nombre maximal
            //on stope le traitement car on peut avoir des resultats errones
            if (adressesDeVille.Count() == MAX_ADRESSE)
            {
                result.Overflow = true;
            }

            else if (adressesDeVille.Count() != 0)
            {

                var adressesDeVilleFusionees = adresseRecherchee.FusionneAdressesSaufVilleEtCodePostal(adressesDeVille);

                //ECM 03/04/2012 recherche de l'adresse exacte 
                result.Adresses = AdresseRepository.RechercheAdresse(adressesDeVilleFusionees, startRow, endRow, true).ToList();
                result.Count = AdresseRepository.RechercheAdresseCount(adressesDeVilleFusionees, true);

                //ECM 03/04/2012 si aucun résultat, recherche des mots clefs (voie) sur le CP et Ville
                if (result.Count == 0)
                {
                    result.Adresses = AdresseRepository.RechercheAdresse(adressesDeVilleFusionees, startRow, endRow).ToList();
                    result.Count = AdresseRepository.RechercheAdresseCount(adressesDeVilleFusionees);
                }
                //si pas de resultat alors qu'on a des villes qui conviennent
                //on affiche toutes les rues des villes qui conviennent
                if (result.Count == 0)
                {
                    result.Adresses = AdresseRepository.RechercheAdresse(adressesDeVille, startRow, endRow).ToList();
                    result.Count = AdresseRepository.RechercheAdresseCount(adressesDeVille);
                    result.AucuneVoieNeConvient = true;
                }
            }
            return result;
        }

        public GeolocResult GetLocalisationFromAdresse(string adresse)
        {
            Models.GoogleMaps.GeolocResult geolocResult = new GeolocResult() { IsSuccess = false, Result = null };
            if (string.IsNullOrWhiteSpace(adresse))
            {
                geolocResult.Message = "Adresse vide";
                return geolocResult;
            }
            adresse = System.Text.RegularExpressions.Regex.Replace(adresse, "[^a-zA-Z0-9]+", " ", System.Text.RegularExpressions.RegexOptions.Compiled);
            adresse = Uri.EscapeDataString(adresse);
            
            var url = AppSettingsHelper.GenerateGoogleMapsGeocodeLink() + "&address=" + adresse;

            using (var client = new WebClient())
            {
                var response = client.DownloadString(url);
                var result = ConverterHelper.DeserializeObject<GmapsJsonObject>(response);

                if (result != null)
                {
                    string status = result.status;
                    if (status == "OK")
                    {
                        if (result != null && result?.results?.Count > 0)
                        {
                            geolocResult.Result = new LatLong
                            {
                                Lat = Convert.ToDouble(result.results[0].geometry.location.lat, CultureInfo.InvariantCulture),
                                Lon = Convert.ToDouble(result.results[0].geometry.location.lng, CultureInfo.InvariantCulture)
                            };
                            geolocResult.IsSuccess = true;
                            geolocResult.Message = "OK";
                        }
                        else
                        {
                            geolocResult.Message = "Result empty";
                        }
                    }
                    else
                    {
                        geolocResult.Message = "Status not OK " + result.status;
                    }
                }
                else
                {
                    geolocResult.Message = "No Result";
                }
                return geolocResult;

            }
        }

        #region Private methods

        private IEnumerable<Adresse> VillesQuiConviennent(Adresse adresseRecherchee, AdressesWrapper adressesWrapper)
        {
            IEnumerable<Adresse> result;
            if (adresseRecherchee.IsCedex.Value)
            {
                result = VillesQuiConviennentAlgoCedex(adresseRecherchee, adressesWrapper);
            }
            else
            {
                result = VillesQuiConvienneAlgoNonCedex(adresseRecherchee, adressesWrapper);
            }
            return result;
        }

        private IEnumerable<Adresse> VillesQuiConvienneAlgoNonCedex(Adresse adresseRecherchee, AdressesWrapper adressesWrapper)
        {
            IEnumerable<Adresse> result;
            adressesWrapper.HasCedex = false;
            result = VilleParCodePostalEtVille(adresseRecherchee);
            if (result.Count() == 0)
            {
                result = VilleParDepartementEtVille(adresseRecherchee);
            }
            return result;
        }

        private IEnumerable<Adresse> VillesQuiConviennentAlgoCedex(Adresse adresseRecherchee, AdressesWrapper adressesWrapper)
        {
            IEnumerable<Adresse> result = new List<Adresse>();
            adressesWrapper.HasCedex = true;

            //Recherche des villes à partir du departement et de la distribution
            //s il existe des mots clefs dans distribution
            if (adresseRecherchee.FiltreDistribution().Count != 0)
            {
                result = VilleParDepartementEtDistribution(adresseRecherchee);
            }

            //s il n'y a pas de mots clefs dans distribution
            //ou s'il la recherche par distribution n'a pas de resultat
            //on recherche par ville et departement
            if (result.Count() == 0)
            {
                result = VilleParDepartementEtVille(adresseRecherchee);
            }
            return result;
        }

        private IEnumerable<Adresse> VilleParDepartementEtDistribution(Adresse adresse)
        {
            return AdresseRepository.RechercheVille(adresse.Departement, adresse.FiltreDistribution());
        }

        private IEnumerable<Adresse> VilleParDepartementEtVille(Adresse adresse)
        {
            //ZBO - 12/03/2012 : Appliquer le filtre sur la ville
            //return adresseRepository.RechercheVille(adresse.Departement, new List<String> { adresse.VilleSansCedex });
            return AdresseRepository.RechercheVille(adresse.Departement, adresse.FiltreVille(adresse.VilleSansCedex));

        }

        private IEnumerable<Adresse> VilleParCodePostalEtVille(Adresse adresse)
        {
            //ZBO - 12/03/2012 : Appliquer le filtre sur la ville
            //return adresseRepository.RechercheVille(adresse.CodePostal, new List<String> { adresse.VilleSansCedex });
            return AdresseRepository.RechercheVille(adresse.CodePostal, adresse.FiltreVille(adresse.VilleSansCedex));
        }

        #endregion
    }
}
