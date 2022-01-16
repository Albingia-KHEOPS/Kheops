using Hexavia.Business.Interfaces;
using Hexavia.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hexavia.Console.Configurations;
using Hexavia.Console.Infrastructure.Photon;
using Hexavia.Tools.Helpers;
using System.Net.Http;
using Hexavia.Models.GoogleMaps;

namespace Hexavia.Console
{
    class GeoCoding
    {
        public enum AddressType
        {
            ABPLG3, ABPLG4, ABPLG5
        }

        private readonly IAdresseBusiness AdresseBusiness;
        private readonly ILatitudeLogitudeBusiness LatitudeLogitudeBusiness;
        private const string PHOTON_URL = "http://photon.komoot.de/api/";
        private const int NB_CALLS_SECOND = 35;
        private const int PAGE_SIZE = 5000;
        public GeoCoding(IAdresseBusiness adresseBusiness, ILatitudeLogitudeBusiness latitudeLogitudeBusiness)
        {
            AdresseBusiness = adresseBusiness;
            LatitudeLogitudeBusiness = latitudeLogitudeBusiness;
        }

        public void Run()
        {
            // Get Location
            var startExecutionDate = DateTime.Now;
            var AdressesCount = 0;
            List<AdresseLight> adresses = null;

            List<FailedAddress> notGeocodedAddress = new List<FailedAddress>();
            Dictionary<int, string> notGeocodedAddressMessage = new Dictionary<int, string>();
            var lockMe = new object();
            var readPage = true;
            var pageNumber = 1;
            var requestNumber = 0;
            //if (GeocodeAppSettings.ResetGeocoding)
            //{
            //    LatitudeLogitudeBusiness.ClearKGEOLOCTable();
            //}
            void SaveLocation(AdresseLight selectedAddress, LatLong location)
            {
                LatitudeLogitudeBusiness.InsertIntoKGEOLOC(new KGeoloc
                {
                    NumeroChrono = selectedAddress.NumeroChrono.Value,
                    Lat = location.Lat,
                    Lon = location.Lon,
                    DateModification = DateTime.Now
                });
            }


            async Task<bool> ExecuteAddressRequestAsync(AdresseLight selectedAddress, AddressType addressType)
            {
                var result = false;
                string address = selectedAddress.AdresseComplete4.ToString();
                string addressWithoutCp = selectedAddress.AdresseCompleteSansCP4.ToString();

                switch (addressType)
                {
                    case AddressType.ABPLG3:
                        address = selectedAddress.AdresseComplete3.ToString();
                        addressWithoutCp = selectedAddress.AdresseCompleteSansCP3.ToString();
                        break;
                    case AddressType.ABPLG5:
                        address = selectedAddress.AdresseComplete5.ToString();
                        addressWithoutCp = selectedAddress.AdresseCompleteSansCP5.ToString();
                        break;
                }
                if (!string.IsNullOrEmpty(address))
                {
                    var location = await GetLocationAsync(address);
                    Interlocked.Increment(ref requestNumber);
                    if (location.Result != null)
                    {
                        SaveLocation(selectedAddress, location.Result);
                        System.Console.WriteLine($"N° Chrono : {selectedAddress.NumeroChrono}  |  Status : Done   | Address : {address}");
                        result = true;
                    }
                    else
                    {
                        if (notGeocodedAddressMessage.ContainsKey(selectedAddress.NumeroChrono ?? 0))
                            notGeocodedAddressMessage[selectedAddress.NumeroChrono ?? 0] += "\r\n" + addressType.ToString() + " : " + location.Message;
                        else
                            notGeocodedAddressMessage.Add(selectedAddress.NumeroChrono ?? 0, "\r\n" + addressType.ToString() + " : " + location.Message);


                        if (!string.IsNullOrEmpty(addressWithoutCp))
                        {
                            location = await GetLocationAsync(addressWithoutCp);
                            Interlocked.Increment(ref requestNumber);
                            if (location.Result != null)
                            {
                                SaveLocation(selectedAddress, location.Result);
                                System.Console.WriteLine($"N° Chrono : {selectedAddress.NumeroChrono}  |  Status : Done   | Address : {addressWithoutCp}");
                                result = true;

                            } else
                            {
                                if (notGeocodedAddressMessage.ContainsKey(selectedAddress.NumeroChrono ?? 0))
                                    notGeocodedAddressMessage[selectedAddress.NumeroChrono ?? 0] += "\r\n" + addressType.ToString() + " : " + location.Message;
                                else
                                    notGeocodedAddressMessage.Add(selectedAddress.NumeroChrono ?? 0, "\r\n" + addressType.ToString() + " : " + location.Message);
                            }

                        }
                    }
                }
                return result;
            }


            while (readPage)
            {
                adresses = AdresseBusiness.GetAllAdressesNotGeolocalisated(pageNumber, PAGE_SIZE);
                if (adresses?.Any() == true)
                {
                    AdressesCount += adresses.Count;
                    // Test Nbre page = 2  && pageNumber<2
                    readPage = adresses.Count == PAGE_SIZE && pageNumber < 1;
                    pageNumber++;
                    DateTime start, end = DateTime.Now;

                    for (int i = adresses.Count; i > 0; i -= NB_CALLS_SECOND)
                    {
                        start = DateTime.Now;
                        var selected = adresses.Take(NB_CALLS_SECOND).ToList();
                        adresses.RemoveRange(0, selected.Count);
                        List<Task> tasks = new List<Task>();
                        foreach (var selectedAddress in selected)
                        {

                            tasks.Add(Task.Factory.StartNew(async () =>
                            {
                                SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
                                await semaphoreSlim.WaitAsync();
                                try
                                {
                                    if (await ExecuteAddressRequestAsync(selectedAddress, AddressType.ABPLG4) == false)
                                    {
                                        if (await ExecuteAddressRequestAsync(selectedAddress, AddressType.ABPLG5) == false)
                                        {
                                            if (await ExecuteAddressRequestAsync(selectedAddress, AddressType.ABPLG3) == false)
                                            {
                                                notGeocodedAddress.Add(new FailedAddress { NumeroChrono = selectedAddress.NumeroChrono, Address = selectedAddress.AdresseComplete4 + notGeocodedAddressMessage.FirstOrDefault(x => x.Key == selectedAddress.NumeroChrono).Value });
                                                SaveLocation(selectedAddress, new LatLong { Lat = 0, Lon = 0 });
                                                System.Console.WriteLine($"N° Chrono : {selectedAddress.NumeroChrono}  |  Status : Failed | Address : {selectedAddress.AdresseComplete4}");
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    notGeocodedAddress.Add(new FailedAddress { NumeroChrono = selectedAddress.NumeroChrono, Address = selectedAddress.AdresseComplete4 + "\r\n" + ex.Message });
                                    System.Console.WriteLine($"N° Chrono : {selectedAddress.NumeroChrono}  |  Status : Exception | Address : {selectedAddress.AdresseComplete4}");
                                }
                                finally
                                {
                                    semaphoreSlim.Release();
                                }
                            }));


                        }
                        Task.WaitAll(tasks.ToArray());
                        end = DateTime.Now;

                        if (end.Subtract(start).TotalSeconds < 1)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
                else
                {
                    readPage = false;
                }
            }
            TraceResult(startExecutionDate, DateTime.Now, requestNumber, AdressesCount, notGeocodedAddress);


        }

        /// <summary>
        /// Get Location
        /// </summary>
        /// <param name="adresse"></param>
        /// <returns></returns>
        private async Task<GeolocResult> GetLocationAsync(string adresse)
        {
            var geolocResult = new GeolocResult { IsSuccess = false, Result = null };
            var useGmapsApi = GeocodeAppSettings.UseGoogleMaps;
            adresse = System.Text.RegularExpressions.Regex.Replace(adresse, "[^a-zA-Z0-9]+", " ", System.Text.RegularExpressions.RegexOptions.Compiled);
            adresse = Uri.EscapeDataString(adresse);
            if (string.IsNullOrWhiteSpace(adresse))
            {
                geolocResult.Message = "Adresse vide";
                return geolocResult;
            }
            var url = PHOTON_URL + "?q=" + adresse + "&limit=1";
            if (useGmapsApi)
            {

                url = GeocodeAppSettings.Url + "address=" + adresse + "&key=" + GeocodeAppSettings.key;

            }
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromMinutes(10);
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();

                        if (result != null)
                        {
                            if (useGmapsApi)
                            {
                                var obj = ConverterHelper.DeserializeObject<GmapsJsonObject>(result);
                                string status = obj.status;
                                if (status == "OK")
                                {
                                    if (obj != null && obj?.results?.Count > 0)
                                    {
                                        geolocResult.Result = new LatLong
                                        {
                                            Lat = Convert.ToDouble(obj.results[0].geometry.location.lat, CultureInfo.InvariantCulture),
                                            Lon = Convert.ToDouble(obj.results[0].geometry.location.lng, CultureInfo.InvariantCulture)
                                        };
                                        geolocResult.IsSuccess = true;
                                        geolocResult.Message = "OK";
                                    }
                                    else
                                    {
                                        geolocResult.Message = "Result empty";
                                    }
                                }
                                else if (obj.status == "OVER_QUERY_LIMIT")
                                {
                                    throw new Exception("OVER_QUERY_LIMIT");
                                }
                                else
                                {
                                    geolocResult.Message = "Status not OK " + obj.status; ;
                                }
                            }
                            else
                            {
                                var obj = ConverterHelper.DeserializeObject<PhotonJsonObject>(result);
                                if (obj != null && obj?.features?.Count > 0)
                                {
                                    geolocResult.Result = new LatLong
                                    {
                                        Lat = Convert.ToDouble(obj.features[0].geometry.coordinates[0], CultureInfo.InvariantCulture),
                                        Lon = Convert.ToDouble(obj.features[0].geometry.coordinates[1], CultureInfo.InvariantCulture)
                                    };
                                    geolocResult.IsSuccess = true;
                                    geolocResult.Message = "OK";
                                }
                                else
                                {
                                    geolocResult.Message = "Result empty";
                                }
                            }
                        }
                        else
                        {
                            geolocResult.Message = "No Result";
                        }
                        return geolocResult;

                    }
                }
            }
        }
        /// <summary>
        /// Trace result of geocoding
        /// </summary>
        /// <param name="startlogDate"></param>
        /// <param name="endLogDate"></param>
        /// <param name="addressCount"></param>
        /// <param name="notGeocodedAddress"></param>
        private void TraceResult(DateTime startlogDate, DateTime endLogDate, int requestCount, int addressCount, List<FailedAddress> notGeocodedAddress)
        {
            // Write report
            var path = GeocodeAppSettings.GetLogPathByFileName(startlogDate);
            var logger = new Logger(path);
            var geocodedAddressCount = addressCount - notGeocodedAddress.Count;

            logger.WriteLine("===== Hexavia Console Geocoding =====");
            logger.WriteLine($"# Date de début d'exécution       : {startlogDate.ToString("dddd, dd MMMM yyyy HH:mm:ss")}");
            logger.WriteLine($"# Date de fin d'exécution         : {endLogDate.ToString("dddd, dd MMMM yyyy HH:mm:ss")}");
            logger.WriteLine($"# Durée d'exécution               : {(endLogDate - startlogDate).ToString(@"hh\:mm\:ss")}");
            logger.WriteLine($"# Nombre des requêtes             : {requestCount}");
            logger.WriteLine($"# Nombre total d'adresses         : {addressCount}");
            logger.WriteLine($"# Nombre d'adresses traitées      : {geocodedAddressCount}");
            logger.WriteLine($"# Nombre d'adresses non traitées  : {notGeocodedAddress.Count}");
            if (notGeocodedAddress.Count != 0)
            {
                logger.WriteLine($"# Liste d'adresses non traitées   :");

            }
            foreach (var address in notGeocodedAddress.OrderBy(x => x.NumeroChrono))
            {
                logger.WriteLine($"N° Chrono : {address.NumeroChrono}  |  Adresse : {address.Address}");
            }

            //Send mail
            var body = "Bonjour , </br>Vous trouverez ci-joint les résultats de l'exécution d'Hexavia Console Geocoding.</br>Cordialement,";
            var subject = $"[Nbre d'adresses traitées : {geocodedAddressCount} | Nbre d'adresses non traitées : {notGeocodedAddress.Count} ] HEXAVIA CONSOLE";
            EmailSender.Send(GeocodeAppSettings.ToMails, subject, body, true, path);
        }
    }
}














