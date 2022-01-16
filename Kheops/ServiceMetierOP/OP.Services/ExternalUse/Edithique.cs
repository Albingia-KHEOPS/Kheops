using System.Collections.Generic;
using System.ServiceModel.Activation;
using ALBINGIA.Framework.Common.Constants;
using OP.IOWebService.BLServices;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.FormuleGarantie;
using OPServiceContract.ExternalUse;

namespace OP.Services.ExternalUse
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Edithique : IEdithique
    {

        public FormGarDto GetFormuleGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string formGen, string codeCategorie, string codeAlpha, string branche, string libFormule, string user, int appliqueA)
        {
            //MISE EN COMMENTAIRE SUITE AU REFACTO DU BLFORMULESGARANTIE
            //return BLFormuleGarantie.FormulesGarantiesGet(codeOffre, version, type, codeAvn, codeFormule, codeOption, formGen, codeCategorie, codeAlpha, branche, libFormule, user, appliqueA, true,ModeConsultation.Standard); 
            return null;
        }


        public bool SetCoordinateAdr()
        {
         return DataAccess.GeoRepository.SetAdrToLngLat();
         //return false;
        }

        public List<BandeauDto> GetGeo(string numOffre, string type, int version, int perimetre, string departement)
        {
            //return null;
            return DataAccess.GeoRepository.GetListAdrByPerm(numOffre, type, version, perimetre,departement);
        }

        public List<BandeauDto> GetGeoInRectangle(double eastLatitude, double eastlongitude, double southWestLatitude, double southWestLongitude)
        {
            //return null;
           return DataAccess.GeoRepository.GetListAdrInrectangle(eastLatitude, eastlongitude, southWestLatitude,
                southWestLongitude);
            
        }
    }
}
