using System.Data.Linq.Mapping;


namespace OP.WSAS400.DTO.FormuleGarantie
{
    public class GarBlocRelationDto
    {
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [Column(Name = "RELATION")]
        public string Relation { get; set; }
        [Column(Name = "ID1")]
        public long Id1 { get; set; }
        [Column(Name = "ID2")]
        public long Id2 { get; set; }
    }
}
