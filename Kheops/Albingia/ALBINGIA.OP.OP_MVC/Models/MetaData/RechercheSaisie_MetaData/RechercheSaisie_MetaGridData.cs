using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.RechercheSaisie_MetaData
{
    public class RechercheSaisie_MetaGridData : List<RechercheSaisie_MetaData>
    {
        public int NbCount { get; set; }
        public int StartLigne { get; set; }
        public int EndLigne { get; set; }
        public int PageNumber { get; set; }
        public int LineCount { get; set; }
       
        public RechercheSaisie_MetaGridData() : base() { }
        public RechercheSaisie_MetaGridData(int capacity) : base(capacity) { }
        public RechercheSaisie_MetaGridData(IEnumerable<RechercheSaisie_MetaData> items) : base(items) { }
        public RechercheSaisie_MetaGridData(IEnumerable<RechercheSaisie_MetaData> items, int nbCount, int startLigne, int endLine, int pageNumber, int lineCount)
            : base(items)
        {
            this.NbCount = nbCount;
            this.StartLigne = startLigne;
            this.EndLigne = endLine;
            this.PageNumber = pageNumber;
            this.LineCount = lineCount;
        }
    }
}   