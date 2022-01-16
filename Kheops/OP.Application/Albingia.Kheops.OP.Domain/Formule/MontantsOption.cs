using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public class MontantsOption : Value
    {
      /* KDBPAQ */  public bool? IsMontantAcquis { get; set; }
      /* KDBACQ */  public decimal Montantacquis { get; set; }
      /* KDBTMC */  public decimal TotalCalculeRef { get; set; }
      /* KDBTFF */  public decimal TotalMontantForceRef { get; set; }
      /* KDBTFP */  public decimal TotalCoefcalcul { get; set; }
      /* KDBPRO */  public bool? IsMontantprovisionnel { get; set; }
      /* KDBTMI */  public bool? IsMontantForcepourMini { get; set; }
      /* KDBTFM */  public string MotifTotalforce { get; set; }
      /* KDBCMC */  public decimal ComptantMontantCalcule { get; set; }
      /* KDBCFO */  public bool? IsComptantMontantForce { get; set; }
      /* KDBCHT */  public decimal ComptantMontantForceHT { get; set; }
      /* KDBCTT */  public decimal ComptantMontantForceTTC { get; set; }
      /* KDBCCP */  public decimal CoeffCalculForceComptant { get; set; }
      /* KDBVAL */  public decimal ValeurOrigine { get; set; }
      /* KDBVAA */  public decimal ValeurActualisee { get; set; }
      /* KDBVAW */  public decimal Valeurdetravail { get; set; }
      /* KDBVAT */  public string TypeDevaleur { get; set; }
      /* KDBVAU */  public UniteCapitaux UniteDeValeur { get; set; }
      /* KDBVAH */  public bool? IsTTC { get; set; }
      /* KDBIVO */  public decimal ValeurIndiceOrigin { get; set; }
      /* KDBIVA */  public decimal ValeurIndiceActual { get; set; }
      /* KDBIVW */  public decimal ValeurIndiceTravai { get; set; }

      /* KDBEHH */  public decimal ProchaineEChHT { get; set; }
      /* KDBEHC */  public decimal ProchaineEchCatnat { get; set; }
      /* KDBEHI */  public decimal ProchaineEchIncend { get; set; }
      /* KDBASVALO */  public decimal AssietteValOrigine { get; set; }
      /* KDBASVALA */  public decimal AssietteValActual { get; set; }
      /* KDBASVALW */  public decimal AssietteValeurW { get; set; }
      /* KDBASUNIT */  public UniteCapitaux AssietteUnite { get; set; }
      /* KDBASBASE */  public string AssietteBaseTypeValeur { get; set; }
      /* KDBGER */  public decimal MontantRefForcesaisi { get; set; }

    }
}











































































