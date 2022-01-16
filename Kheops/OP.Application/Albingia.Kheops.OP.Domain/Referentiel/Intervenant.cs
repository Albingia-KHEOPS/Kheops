namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Intervenant
    {
        virtual public int Id { get; set; }
        virtual public string Nom { get; set; }
        virtual public string TypeInterv { get; set; }
        virtual public string Telepone { get; set; }
        virtual public string Email { get; set; }
        virtual public Adresse Adresse { get; set; }
        virtual public Interlocuteur Interlocuteur { get; set; }
    }
}