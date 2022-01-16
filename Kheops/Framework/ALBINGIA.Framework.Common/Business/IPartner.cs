using ALBINGIA.Framework.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Business {
    public interface IPartner {
        TypePartenaireKheops Type { get; }
        TypePartenaireAs400 TypeAS400 { get; }
        string Code { get; }
        string Name { get; }
        string RepresentativeCode { get; }
        string CountryName { get; }
        bool IsInvalid { get; set; }
    }
}
