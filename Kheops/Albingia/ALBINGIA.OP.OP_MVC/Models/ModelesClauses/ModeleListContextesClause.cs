namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleContextesClause
    {
     
       
        public string Origine { get; set; }
        public string EtatTitre { get; set; }
        public string Etape { get; set; }
        public string Contexte { get; set; }
        public string ContexteLabel { get; set; }
        public string OrigineFiltre { get; set; }

        //public static explicit operator ModeleContextesClause(ClausesContextDto ligneDto)
        //{
        //    ModeleContextesClause toReturn = ObjectMapperManager.DefaultInstance.GetMapper<ClausesContextDto, ModeleContextesClause>().Map(ligneDto);
       

        //    return toReturn;
        //}
    }
}
