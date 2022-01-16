using OP.WSAS400.DTO.Stat;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleStatisque
{
    public class StatClausesLibreModel
    {

        public string DelegationUser { get; set; }
        public string CreateUser { get; set; }
        public string DateCreation { get; set; }
        public string DateSaisie { get; set; }
        public string NumPolice { get; set; }
        public int Version { get; set; }
        public int CodeCourtier { get; set; }
        public string NomCourtier { get; set; }
        public string DelegationCourtier { get; set; }
        public int CodePreneurass { get; set; }
        public string NomPreneurass { get; set; }
        public string Souscripteur { get; set; }

        public static explicit operator StatClausesLibreModel(StatClausesLibresDto  clausesdto)
        {
            return new StatClausesLibreModel
            {
                DelegationUser =clausesdto.DelegationUser,
                CreateUser=clausesdto.CreateUser,
                DateCreation=clausesdto.DateCreation,
                DateSaisie=clausesdto.DateSaisie,
                NumPolice=clausesdto.NumPolice,
                Version=clausesdto.Version,
                CodeCourtier=clausesdto.CodeCourtier,
                NomCourtier=clausesdto.DelegationCourtier,
                DelegationCourtier=clausesdto.DelegationCourtier,
                CodePreneurass= clausesdto.CodePreneurass,
                NomPreneurass = clausesdto.NomPreneurass,
                 Souscripteur=clausesdto.Souscripteur,
            };
        }
    }
}