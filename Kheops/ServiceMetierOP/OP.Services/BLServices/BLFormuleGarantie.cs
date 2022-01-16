using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using OP.DataAccess;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OP.IOWebService.BLServices {
    public static class BLFormuleGarantie
    {
        public static void SavePorteeGarantie(string codeOffre, string version, string type, string codeAvn, string codeFormule, string codeOption, string idGarantie, string codeGarantie, string nature, string codeRsq, string codesObj, string user, RisqueDto rsq, string alimAssiette, bool reportCal)
        {
            if (alimAssiette == "B" || alimAssiette == "C")
            {
                FormuleRepository.SavePorteeGarantieAlimAssiette(codeOffre, version, type, codeFormule, codeOption, idGarantie, codeGarantie, nature, rsq, alimAssiette, user, reportCal);
            }
            else
            {
                FormuleRepository.SavePorteeGarantie(codeOffre, version, type, codeFormule, codeOption, idGarantie, codeGarantie, nature, codeRsq, codesObj, user);
            }
        }

        public static List<RisqueDto> GetFormuleRisquesApplicables(string codeOffre, string version, string type, string avenant, string codeFormule, string codeOption, ModeConsultation modeNavig)
        {
            if (!avenant.IsEmptyOrNull() && modeNavig == ModeConsultation.Historique)
            {
               return RisqueRepository.ObtenirRisquesAvenant(codeOffre, version, type, avenant, modeNavig);
            }
            else
            {
                return RisqueRepository.ObtenirRisquesSP(codeOffre, version, type, avenant, codeFormule, codeOption, modeNavig);
            }
        }

        public static RisqueDto GetRisqueFormule(Folder folder, int codeFormule, IDbConnection connection = null) {
            var repository = new FormuleRepository(connection);
            List<(int kabrsq, string kabdesc, int kacobj, string kacdesc)> objets = repository.GetRisquesObjets(folder, codeFormule).ToList();
            var risque = new RisqueDto();
            if (objets.Any()) {
                var lstRsq = objets.GroupBy(el => el.kabrsq).Select(r => r.First()).FirstOrDefault();
                risque.Code = Convert.ToInt32(lstRsq.kabrsq);
                risque.Designation = lstRsq.kabdesc;
                risque.Descriptif = lstRsq.kabdesc;
                risque.Objets = new List<ObjetDto>();

                var lstObj = objets.FindAll(o => o.kabrsq == lstRsq.kabrsq);
                foreach (var obj in lstObj) {
                    risque.Objets.Add(new ObjetDto {
                        Code = Convert.ToInt32(obj.kacobj),
                        Designation = obj.kacdesc,
                        Descriptif = obj.kacdesc
                    });
                }
            }
            return risque;
        }
    }
}
