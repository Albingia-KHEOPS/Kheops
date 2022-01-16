using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using OP.DataAccess;
using OP.Services.BLServices;
using OP.WSAS400.DTO.Inventaires;
using OP.WSAS400.DTO.Offres.Parametres;
using OPServiceContract.IHexavia;

namespace OP.Services.Hexavia {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AdresseHexavia : IAdresseHexavia {
        private readonly AdresseHexaviaRepository repository;
        public AdresseHexavia(AdresseHexaviaRepository hexaviaRepository) {
            this.repository = hexaviaRepository;
        }

        public List<ParametreDto> GetVillesByCP(string codePostal) {
            return AdresseHexaviaRepository.GetVillesByCP(codePostal);
            //return BLAdresseHexavia.GetVillesByCP(codePostal);
        }
        public List<ParametreDto> GetCPByVille(string ville) {
            return AdresseHexaviaRepository.GetCPByVille(ville);
            //return BLAdresseHexavia.GetCPByVille(ville);
        }
        public List<CPVilleDto> GetCodePostalVille(string value, string mode) {
            return BLCommon.GetCodePostalVille(value, mode);
            //return BLAdresseHexavia.GetCodePostalVille(value, mode);
        }

        public List<(string nom, int cp)> SearchVilleByCP(int codePostal, bool matchAnywhere = false) {
            return this.repository.FindVilles(codePostal, matchAnywhere).ToList();
        }
    }
}
