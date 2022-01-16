using Albingia.Kheops.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OPServiceContract {
    [ServiceContract]
    public interface ISinistres {
        [OperationContract]
        SinistreDto CalculProvisionsPrevisionsChargement(SinistreDto sinistre, DateTime? dateDebut = null, DateTime? dateFin = null);

        [OperationContract]
        decimal TotalByPreneurCalculProvisionsPrevisionsChargement(int codePreneur);
    }
}
