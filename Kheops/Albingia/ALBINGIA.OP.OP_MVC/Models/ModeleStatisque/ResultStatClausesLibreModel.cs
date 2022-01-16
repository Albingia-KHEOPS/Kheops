using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleStatisque
{
    public class ResultStatClausesLibreModel
    {
        public int NbCount { get; set; }
        public int StartLigne { get; set; }
        public int EndLigne { get; set; }
        public int PageNumber { get; set; }
        public int LineCount { get; set; }
        public List<StatClausesLibreModel> ListResult { get; set; }

    }
}