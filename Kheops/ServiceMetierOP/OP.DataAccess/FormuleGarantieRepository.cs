using System;
using System.Collections.Generic;
using OP.DataAccess.Helper;
using System.Data;
using ALBINGIA.Framework.Common.Data;
using OP.WSAS400.DTO.Volet;
using System.Data.Common;
using System.Data.EasycomClient;
using OP.WSAS400.DTO.FormuleGarantie;
using ALBINGIA.Framework.Common.Constants;

namespace OP.DataAccess
{
    public static class FormuleGarantieRepository
    {
        //#region Inventaire

        //public static string GetInfoInventGarantie(string idGarantie, AlbOpConstants.ModeConsultation modeNavig)
        //{
        //    string sql = string.Format(@"SELECT KDEINVSP INVENSPEC, KDEALA TYPEALIM FROM {0} WHERE KDEID = {1}",
        //        modeNavig == AlbOpConstants.ModeConsultation.Historique ? "HPGARAN" : "KPGARAW",
        //                idGarantie);
        //    var result = DbBase.Settings.ExecuteList<InventaireGarantieDto>(CommandType.Text, sql);
        //    if (result != null && result.Count > 0)
        //        return string.Format("{0}_{1}", result[0].InvenSpec, result[0].TypeAlim);
        //    return string.Empty;
        //}

        //#endregion

    }
}

