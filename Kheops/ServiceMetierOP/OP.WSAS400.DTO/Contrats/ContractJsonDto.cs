using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Contrats
{

    public class Cible
    {
        public string Code { get; set; }
    }

    public class Branche
    {
        public string Code { get; set; }
        public Cible Cible { get; set; }
    }

    public class DateEffet
    {
        public string Debut { get; set; }
        public string Fin { get; set; }
    }

    public class Interlocuteur
    {
        public string Code { get; set; }
        public string Nom { get; set; }
    }

    public class Gestionnaire
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Quittancement { get; set; }
        public Interlocuteur Interlocuteur { get; set; }
    }

    public class Apporteur
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }

    public class Payeur
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }

    public class Courtier
    {
        public Gestionnaire Gestionnaire { get; set; }
        public Apporteur Apporteur { get; set; }
        public Payeur Payeur { get; set; }
    }

    public class Assure
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }

    public class Periodicite
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }

    public class Devise
    {
        public string Code { get; set; }
    }

    public class IndiceRef
    {
        public string Code { get; set; }
        public string Valeur { get; set; }
    }

    public class Commissions
    {
        public string TauxHCATNAT { get; set; }
        public string TauxCATNAT { get; set; }
    }

    public class IntercalaireFile
    {
        public string Url { get; set; }
        public string Nom { get; set; }
        public string Reference { get; set; }
        public string FileDoc { get; set; }
        public string FilePath { get; set; }
    }

    public class MotsCle
    {
        public string Code { get; set; }
    }

    public class Nomenclature
    {
        public string Code { get; set; }
    }


    public class PeriodeGarantie
    {
        public string Debut { get; set; }
        public string Fin { get; set; }
    }

    public class Territorialite
    {
        public string Code { get; set; }
    }

    public class Objet
    {
        public string Designation { get; set; }
        public List<Nomenclature> Nomenclatures { get; set; }
        public PeriodeGarantie PeriodeGarantie { get; set; }
        public Territorialite Territorialite { get; set; }
        public string Valeur { get; set; }
        public string Unite { get; set; }
        public string Type { get; set; }
        public string ValeurHT { get; set; }
        public string CoutM2 { get; set; }
    }


    public class Franchise
    {
        public string Valeur { get; set; }
        public string Unite { get; set; }
        public string Type { get; set; }
    }

    public class LCI
    {
        public string Valeur { get; set; }
        public string Unite { get; set; }
        public string Type { get; set; }
    }

    public class Assiette
    {
        public string Valeur { get; set; }
        public string Unite { get; set; }
        public string Type { get; set; }
    }

    public class Prime
    {
        public string Valeur { get; set; }
        public string Unite { get; set; }
        public string PrimeMini { get; set; }
    }

    public class Garanty
    {


        [IgnoreDataMember] public virtual ICollection<Garanty> AllGaranties => Garanties?.SelectMany(x => x.Flatten()).ToList();

        public string Code { get; set; }
        public Franchise Franchise { get; set; }
        public LCI LCI { get; set; }
        public Assiette Assiette { get; set; }
        public Prime Prime { get; set; }
        public List<Garanty> Garanties { get; set; } = new List<Garanty>();

        internal ICollection<Garanty> Flatten()
        {
            return new[] { this }.Concat(this.Garanties?.SelectMany(x => x.Flatten())).ToList();
        }
    }
    public class Bloc
    {
        public string Code { get; set; }
        public List<Garanty> Garanties { get; set; } = new List<Garanty>();
        [IgnoreDataMember] public virtual ICollection<Garanty> AllGaranties => Garanties?.SelectMany(x => x.Flatten()).ToList();
    }

    public class Volet
    {
        public string Code { get; set; }
        public List<Bloc> Blocs { get; set; }
        [IgnoreDataMember] public virtual ICollection<Garanty> AllGaranties => Blocs?.SelectMany(x => x.AllGaranties).ToList();
    }


    public class Formule
    {
        [IgnoreDataMember] public virtual ICollection<Garanty> AllGaranties => Volets?.SelectMany(x => x.AllGaranties).ToList();


        public string Code { get; set; }
        public string Libelle { get; set; }
        public List<Volet> Volets { get; set; }
    }

    public class Risque
    {
        public string Designation { get; set; }
        public PeriodeGarantie PeriodeGarantie { get; set; }
        public string Valeur { get; set; }
        public string Unite { get; set; }
        public string Type { get; set; }
        public string ValeurHT { get; set; }
        public string CoutM2 { get; set; }
        public List<Objet> Objets { get; set; }
        public List<Formule> Formules { get; set; }
    }
    public class Cotisations
    {
        public string Force { get; set; }
        public string Valeur { get; set; }
    }
    public class Adresse
    {
        public Int32 NumeroChrono { get; set; }
        public string Numero { get; set; }
        public string Extension { get; set; }
        public string Rue { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
    }

    public class LCICpx
    {
        public string CodeLCI { get; set; }
        public int ValeurLCI { get; set; }
        public string UniteLCI { get; set; }
        public string TypeLCI { get; set; }
        public int ValeurMax { get; set; }
        public string UniteMax { get; set; }
        public string TypeMax { get; set; }
    }
    
    public class ContractJsonDto
    {
        
        public string CodeAffaire { get; set; }
        public string Aliment { get; set; }
        public string Type { get; set; }
        public string Designation { get; set; }
        public Branche Branche { get; set; }
        public string Gestionnaire { get; set; }
        public string Souscripteur { get; set; }
        public string DateAccord { get; set; }
        public DateEffet DateEffet { get; set; }
        public Courtier Courtier { get; set; }
        public Assure Assure { get; set; }
        public Periodicite Periodicite { get; set; }
        public string EcheancePrincipale { get; set; }
        public string ProchaineEcheance { get; set; }
        public string DebutPeriode { get; set; }
        public string FinPeriode { get; set; }
        public string Preavis { get; set; }
        public string Nature { get; set; }
        public string PartAlbingia { get; set; }
        public Devise Devise { get; set; }
        public IndiceRef IndiceRef { get; set; }
        public string CATNAT { get; set; }
        public Commissions Commissions { get; set; }
        public string IntercalaireCourtier { get; set; }
        public List<IntercalaireFile> IntercalaireFiles { get; set; }
        public List<MotsCle> MotsCles { get; set; }
        public Adresse Adresse { get; set; }
        public List<Risque> Risques { get; set; }
        public Cotisations Cotisations { get; set; }
        public List<LCICpx> LCIComplexes { get; set; }
    }

}
