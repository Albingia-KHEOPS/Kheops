using System.ComponentModel.DataAnnotations;

namespace Albingia.Kheops.OP.Domain.Parametrage.Inventaire
{
    public enum TypeInventaireItem
    {
        [Display(Name="Manifestation(s) Assurée(s)")]
        ManifestationAssurees = 1,
        [Display(Name="Tournage(s) Assuré(s)")]
        TournagesAssures = 2,
        [Display(Name="Personnes Désignées en indisponibilité")]
        PersonneDesigneeIndispo = 3,
        [Display(Name="Personne(s) Désignée(s)")]
        PersonneDesignee = 4,
        [Display(Name="Personnes Désignées en indisponibilité")]
        PersonneDesigneeIndispoTournage = 5,
        [Display(Name="Matériel(s) Assuré(s)")]
        Materielsassures = 6,
        [Display(Name="Matériel(s) et/ou bien(s) assuré(s)")]
        Biensassures = 7,
        [Display(Name="RS Postes assurés")]
        Postesassures = 8,
        [Display(Name="DAB Multiples situations")]
        MultiplesSituations = 9,
        [Display(Name="Production et/ou Réalisation")]
        Audioproduction = 10,
        [Display(Name="Location à des Tiers")]
        Audiolocation = 11,
        [Display(Name="Activités immobilières")]
        ProImmo = 12,
        [Display(Name="Marchandises transportées assurées")]
        Marchandises = 13,
        [Display(Name="Marchandises stockées assurées")]
        Stockage = 14,
        [Display(Name="Véhicules pour transport Propre Compte")]
        ParcVehicules = 15
    }
}