namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class CourtierApporteur
    {
        public Courtier Courtier { get; set; }
        public bool EditMode { get; set; }
        public bool IdentiqueChecked { get; set; }
        //public void SetCourtierApporteur(ContratInfoBaseDto contratInfoBaseDto)
        //{
        //    this.Courtier = GetCourtier(contratInfoBaseDto);
        //}

        //private Courtier GetCourtier(ContratInfoBaseDto contratInfoBaseDto)
        //{
        //    var model = new Courtier();
        //    model.CodeCourtier = contratInfoBaseDto.CourtierApporteur;
        //    model.NomCourtier = contratInfoBaseDto.NomCourtierAppo;          
        //    model.EditMode = this.EditMode;
        //    return model;
        //}
    }
}