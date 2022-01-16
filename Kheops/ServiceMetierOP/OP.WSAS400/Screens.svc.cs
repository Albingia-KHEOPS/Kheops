using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OP.WSAS400.DTO.Ecran.ConfirmationSaisie;
using System.Configuration;
using OP.WSAS400.DataLogic;

namespace OP.WSAS400
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    public class Screens : IScreens
    {
        public ConfirmationSaisieGetResultDto ConfirmationSaisieGet(ConfirmationSaisieGetQueryDto query)
        {
            ConfirmationSaisieGetResultDto toReturn = new ConfirmationSaisieGetResultDto();
            try
            {
                using (IOAS400 _dataContext = new IOAS400(ConfigurationManager.ConnectionStrings["EasyCom"].ConnectionString))
                {
                    toReturn.Result = DTO.enIOAS400Results.success;
                    toReturn.Message = string.Empty;
                    toReturn.MotifsRefus = BusinessLogic.ScreenBuilding.MotifsRefusGet(_dataContext);
                    toReturn.Offre = BusinessLogic.ScreenBuilding.OffreGet(_dataContext, query.CodeOffre, query.VersionOffre);
                }
            }
            catch 
            {
                //toReturn.Result = DTO.enIOAS400Results.failure;
                //toReturn.Message = ex.Message;
                throw;
                    
            }
            toReturn.SendDate = DateTime.Now;
            return toReturn;
        }
    }
}
