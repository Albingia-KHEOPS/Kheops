using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesLCIFranchise
{
    public class ModeleLCIFranchise
    {
        #region parmétrage
        public AlbConstantesMetiers.ExpressionComplexe TypeVue { get; set; }
        public AlbConstantesMetiers.TypeAppel TypeAppel { get; set; }
        public bool IsReadOnly { get; set; }
        #endregion

        public string Valeur { get; set; }
        public string Unite { get; set; }
        public List<AlbSelectListItem> Unites { get; set; }
        public string Type { get; set; }
        public List<AlbSelectListItem> Types { get; set; }
        [Display(Name = "Indexée")]
        public bool IsIndexe { get; set; }
        public string LienComplexe { get; set; }               
        public string CodeComplexe { get; set; }       
        public string LibComplexe { get; set; }
        public string AccessMode { get; set; }
    }
}