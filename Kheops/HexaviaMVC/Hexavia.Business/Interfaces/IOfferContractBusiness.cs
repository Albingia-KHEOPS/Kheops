using Hexavia.Models;
using Hexavia.Models.EnumDir;
using Hexavia.Tools.Helpers;
using System.Collections.Generic;

namespace Hexavia.Business.Interfaces
{
    public interface IOfferContractBusiness
    {
        List<LatLong> LoadAffairesAroundGPSPoint(double longitude, double latitude, double diametre, string typeDesignation, string branche);

        List<LatLong> LoadAffairesByDesignation(string code, string version, string designation, string typeDesignation, string etat, string situation, string branche, string garantie, string departement, string evenement, DiffDisplayModeBranche displayMode, out int count);

        List<LatLong> LoadActeGestionFromFile(double longitude, double latitude, double diametre, string filePath);
        List<LatLong> SearchCasesLatLonByPartner(int? code, TypePartner type);
    }
}
