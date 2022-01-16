using EmitMapper;
using OP.WSAS400.DTO.Offres.Risque;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTransverse
{
    public class ModeleOrganisme
    {
        public int Code { get; set; }
        public string Nom { get; set; }
        public string CP { get; set; }
        public string Ville { get; set; }
        public string Pays { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string NomPays { get; set; }

        public static explicit operator ModeleOrganisme(OrganismeOppDto data)
        {
            var organisme = ObjectMapperManager.DefaultInstance.GetMapper<OrganismeOppDto, ModeleOrganisme>().Map(data);
            if (!string.IsNullOrEmpty(organisme.NomPays) && organisme.NomPays.StartsWith("-"))
                organisme.NomPays = organisme.NomPays.Remove(0, 1);
            return organisme;
        }
    }
}