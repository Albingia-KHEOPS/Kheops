using ALBINGIA.Framework.Common.Constants;

namespace ALBINGIA.Framework.Common.Business {
    public class Partner : IPartner {
        public TypePartenaireKheops Type { get; set; }
        public TypePartenaireAs400 TypeAS400 { get; set; }
        public string Code { get; set; }
        public string RepresentativeCode { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public bool IsInvalid { get; set; }
    }
}
