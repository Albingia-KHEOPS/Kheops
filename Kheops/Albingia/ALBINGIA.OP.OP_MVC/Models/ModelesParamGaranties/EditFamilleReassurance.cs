namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamGaranties
{
    public class EditFamilleReassurance
    {
        public string CodeBrancheEdited { get; set; }
        public string CodeSousBrancheEdited { get; set; }
        public string CodeCategorieEdited { get; set; }
        public string CodeFamilleEdited { get; set; }
        public DDLBranches DDLBranches { get; set; }
        public DDLSousBranches DDLSousBranche { get; set; }
        public DDLCategories DDLCategories { get; set; }
        public DDLFamilles DDLFamilles { get; set; }
      
    }
}