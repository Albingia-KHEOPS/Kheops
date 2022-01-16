using ALBINGIA.Framework.Common;
using OP.WSAS400.DTO.Avenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OPServiceContract {
    [ServiceContract]
    public interface IAvenant {
        [OperationContract]
        string[] CreationAvenantModif(string ipb, int alx, string date, string description, string observations);

        [OperationContract]
        string[] CreateOrUpdate(
            Folder folder, string typeAvt, string modeAvt,
            AvenantModificationDto modeleAvtModif, AvenantResiliationDto modeleAvtResil, AvenantRemiseEnVigueurDto modeleRemiseVig, AvenantRegularisationDto regularisationDto,
            string user);

        [OperationContract]
        bool IsReadonlyRemiseEnVigueur(Folder folder);
    }
}
