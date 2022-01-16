namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class CourtierGestionnaire
    {
        public Courtier Courtier { get; set; }
        //public void SetGestionnaire(ContratInfoBaseDto contratInfoBaseDto)
        //{
        //    this.Courtier = GetCourtier(contratInfoBaseDto);
        //}
        //private Courtier GetCourtier(ContratInfoBaseDto contratInfoBaseDto)
        //{
        //    var model = new Courtier();
        //    model.CodeCourtier = contratInfoBaseDto.CourtierGestionnaire;
        //    model.NomCourtier = contratInfoBaseDto.NomCourtierGest;
        //    if (contratInfoBaseDto.CodeInterlocuteur != 0)
        //    {
        //        model.CodeInterlocuteur = contratInfoBaseDto.CodeInterlocuteur;
        //        model.NomInterlocuteur = contratInfoBaseDto.CodeInterlocuteur + " - " + contratInfoBaseDto.NomInterlocuteur;
        //    }
        //    model.Reference = contratInfoBaseDto.RefCourtier;
        //    return model;
        //}
    }
}