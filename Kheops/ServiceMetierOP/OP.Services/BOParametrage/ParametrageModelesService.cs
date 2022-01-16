
using Albingia.Kheops.OP.Application.Port.Driver;
using OP.WSAS400.DTO.GarantieModele;
using OPServiceContract.IBOParametrage;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace OP.Services.BOParametrage
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ParametrageModelesService : IParametrageModeles {
        private readonly IParametrageModelesPort parametrageModeleService;
        public ParametrageModelesService(IParametrageModelesPort parametrageModeleService) {
            this.parametrageModeleService = parametrageModeleService;
        }

        #region Garantie Modele
        public List<GarantieModeleDto> GarantieModeleGet(string code, string description) { 
            return parametrageModeleService.GarantieModeleGet(code, description);
        }

        public GarantieModeleDto GarantieModeleInfoGet(string code)
        {
            return parametrageModeleService.GarantieModeleInfoGet(code);
        }

        public void EnregistrerGarantieModele(string code, string description, bool isNew, out string msgRetour)
        {
            parametrageModeleService.EnregistrerGarantieModele(code, description, isNew, out msgRetour);
        }

        public void CopierGarantieModele(string code, string codeCopie, out string msgRetour)
        {
            parametrageModeleService.CopierGarantieModele(code, codeCopie, out msgRetour);
        }

        public void SupprimerGarantieModele(string code, out string msgRetour)
        {
            parametrageModeleService.SupprimerGarantieModele(code, out msgRetour);
        }
        #endregion

        #region Garantie Type
        public bool ExistGarantieModeleDansContrat(string code)
        {
            return parametrageModeleService.ExistGarantieModeleDansContrat(code);
        }

        public List<GarantieTypeDto> GarantieTypeGet(string codeModele)
        {
            return parametrageModeleService.GarantieTypeGet(codeModele);
        }

        public GarantieTypeDto GarantieTypeInfoGet(int seq)
        {
            return parametrageModeleService.GarantieTypeInfoGet(seq);
        }

        public GarantieTypeDto GarantieTypeLienInfoGet(int seq)
        {
            return parametrageModeleService.GarantieTypeLienInfoGet(seq);
        }

        public void EnregistrerGarantieType(GarantieTypeDto garType, bool isNew, out string msgRetour)
        {

            parametrageModeleService.EnregistrerGarantieType(garType, isNew, out msgRetour);
        }

        public void SupprimerGarantieType(int seq, out string msgRetour)
        {
            parametrageModeleService.SupprimerGarantieType(seq, out msgRetour);
        }
        #endregion
    }
}

