
using OP.WSAS400.DTO.GarantieModele;
using System.Collections.Generic;
using System.ServiceModel;

namespace OPServiceContract.IBOParametrage
{

    [ServiceContract]
    public interface IParametrageModeles {
        #region Garantie Modele
        [OperationContract]
        GarantieModeleDto GarantieModeleInfoGet(string code);

        [OperationContract]
        List<GarantieModeleDto> GarantieModeleGet(string code, string description);

        [OperationContract]
        void EnregistrerGarantieModele(string code, string description, bool isNew, out string msgRetour);

        [OperationContract]
        void CopierGarantieModele(string code, string codeCopie, out string msgRetour);

        [OperationContract]
        void SupprimerGarantieModele(string code, out string msgRetour);
        #endregion

        #region Garantie Type
        [OperationContract]
        bool ExistGarantieModeleDansContrat(string code);

        [OperationContract]
        List<GarantieTypeDto> GarantieTypeGet(string codeModele);

        [OperationContract]
        GarantieTypeDto GarantieTypeInfoGet(int seq);
        [OperationContract]
        GarantieTypeDto GarantieTypeLienInfoGet(int seq);

        [OperationContract]
        void EnregistrerGarantieType(GarantieTypeDto garType, bool isNew, out string msgRetour);

        [OperationContract]
        void SupprimerGarantieType(int seq, out string msgRetour);
        #endregion
    }
}

