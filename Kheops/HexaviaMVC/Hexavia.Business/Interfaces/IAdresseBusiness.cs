using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Models.GoogleMaps;
using System.Collections.Generic;

namespace Hexavia.Business.Interfaces
{
    public interface IAdresseBusiness
    {
        List<AdresseLight> GetAllAdresses();

        List<AdresseLight> GetAllAdressesNotGeolocalisated(int pageNumber = 0, int pageSize = 0);

        ResultatRechercheAdresse VerifieAdresse(Adresse adresse);

        Adresse GetAdresseByNumeroChrono(int numeroChrono);

        AdressesWrapper RechercheAdresse(Adresse adresseRecherchee, int startRow, int endRow);

        GeolocResult GetLocalisationFromAdresse(string adresse);
    }
}
