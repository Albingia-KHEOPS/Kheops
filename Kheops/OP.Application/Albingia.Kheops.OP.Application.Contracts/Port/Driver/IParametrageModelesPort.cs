
using OP.WSAS400.DTO.GarantieModele;
using System.Collections.Generic;
using System.ServiceModel;

namespace Albingia.Kheops.OP.Application.Port.Driver
{
    [ServiceContract]
    public interface IParametrageModelesPort
    {
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
        GarantieTypeDto GarantieTypeInfoGet(long seq);
        [OperationContract]
        GarantieTypeDto GarantieTypeLienInfoGet(long seq);

        [OperationContract]
        List<GarantieTypeDto> GarantieTypeGetAll();

        [OperationContract]
        void EnregistrerGarantieType(GarantieTypeDto garType, bool isNew, out string msgRetour);

        [OperationContract]
        void SupprimerGarantieType(long seq, out string msgRetour);
        [OperationContract]
        void AjouterGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour);
        [OperationContract]
        void SupprimerGarantieTypeLien(string type, long seqA, long seqB, out string msgRetour);
        #endregion
    }
}
