using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.Constants;
using System;

namespace ALBINGIA.OP.OP_MVC.Common {
    [Serializable]
    public class LablatPartner : Partner {
        public string TypeDescription => AlbEnumInfoValue.GetEnumInfo(Type);

        public Partner ToPartner() {
            return new Partner {
                Type = Type,
                Code = Code,
                CountryName = CountryName,
                IsInvalid = IsInvalid,
                Name = Name,
                RepresentativeCode = RepresentativeCode,
                TypeAS400 = TypeAS400
            };
        }
    }
}