using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.DataAccess;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.ParametreGaranties;
using ALBINGIA.Framework.Common.Constants;

namespace OP.Services.BLServices
{
    public class BLInventaire
    {
        public static int GetNewInventaireId(string codeOffre, string version, string type, string perimetre, string codeRsq, string codeObj, string codeFor, string codeGaran)
        {
            int invenId = InventaireRepository.GetNewInventaireId(codeOffre, version, type, perimetre, codeRsq, codeObj, codeFor, codeGaran);
            if (invenId != 0)
            {
                return invenId;
            }
            return CommonRepository.GetAS400Id("KBEID");
        }
    }
}
