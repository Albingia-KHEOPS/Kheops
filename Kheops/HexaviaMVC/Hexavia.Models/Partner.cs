using Hexavia.Models.EnumDir;

namespace Hexavia.Models
{
    public class Partner
    {
      
        public int? Code { get; set; }
        public string Name { get; set; }
        public string SecondaryName { get; set; }
        public int? Orias { get; set; }
        public TypePartner Type { get; set; }
        public PartnerAdresse Address { get; set; }

    }
}
