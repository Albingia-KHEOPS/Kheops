using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public class Valeurs
    {
        /* KDGLCIVALO */
        public decimal ValeurOrigine { get; set; }
        /* KDGLCIVALA */
        public decimal ValeurActualise { get; set; }
        /* KDGLCIVALW */
        public decimal ValeurTravail { get; set; }

    }
    public class ValeursUnite : Valeurs
    {
        public const string CodeUniteComplexe = "CPX";
        public const string CodeUniteNombre = "N";

        /* KDGLCIUNIT */
        public Unite Unite { get; set; }
    }
    public class ValeursTarif : ValeursUnite
    {
        /* KDGLCIBASE */
        public BaseDeCalcul Base { get; set; }

        public virtual bool IsEmpty => string.IsNullOrWhiteSpace(Unite?.Code) && string.IsNullOrWhiteSpace(Base?.Code) && ValeurOrigine == decimal.Zero;

        public virtual bool IsValidIgnoreValue => (!string.IsNullOrWhiteSpace(Unite?.Code) && !string.IsNullOrWhiteSpace(Base?.Code)) || Unite?.Code == CodeUniteComplexe;

        public virtual bool IsValid => (ValeurOrigine > decimal.Zero || Unite?.Code == CodeUniteComplexe) && IsValidIgnoreValue;
    }
    public class ValeursOptionsTarif : ValeursTarif
    {
        /* KDGLCIMOD */
        public bool? IsModifiable { get; set; }
        /* KDGLCIOBL */
        public bool? IsObligatoire { get; set; }
        /* KDGKDIID */
        public ExpressionComplexeBase ExpressionComplexe { get; set; }

        internal void ResetIds()
        {
            ExpressionComplexe?.ResetIds();
        }

        public override bool IsEmpty => base.IsEmpty
            && (ExpressionComplexe is null ||
                ExpressionComplexe.Id < 1 && string.IsNullOrWhiteSpace(ExpressionComplexe.Code));

        public override bool IsValid => IsEmpty && (!IsObligatoire ?? false) || base.IsValid;

        public bool IsValidIgnoreBase => !string.IsNullOrWhiteSpace(Unite?.Code)
            && (ValeurOrigine > decimal.Zero || Unite?.Code == CodeUniteComplexe)
            && (ValeurOrigine > decimal.Zero && Unite?.Code != CodeUniteComplexe || (!IsObligatoire ?? true))
            && (ExpressionComplexe != null || Unite?.Code != CodeUniteComplexe);

        public bool ValuesEqual(ValeursOptionsTarif tarif)
        {
            return tarif != null
                && (!(ValeurOrigine != tarif.ValeurOrigine
                    || ValeurActualise != tarif.ValeurActualise
                    || ValeurTravail != tarif.ValeurTravail
                    || Unite is null && tarif.Unite != null
                    || Unite != null && tarif.Unite is null
                    || Unite.Code != tarif.Unite.Code
                    || Base is null && Base != null
                    || Base != null && Base is null
                    || Base.Code != tarif.Base.Code
                    || ExpressionComplexe is null && tarif.ExpressionComplexe != null
                    || ExpressionComplexe != null && tarif.ExpressionComplexe is null
                    || ExpressionComplexe.Id != tarif.ExpressionComplexe.Id));
        }
    }

    public class TarifGarantie
    {
        /* KDGNUMTAR */
        public short NumeroTarif { get; set; }

        public ValeursOptionsTarif LCI;
        public ValeursOptionsTarif Franchise;
        public ValeursTarif FranchiseMini;
        public ValeursTarif FranchiseMax;
        public ValeursOptionsTarif PrimeValeur;

        public bool IsLCIValid => (!LCI?.IsObligatoire ?? true) || LCI.IsValid;

        public bool IsFRHValid => (!Franchise?.IsObligatoire ?? true) || Franchise.IsValid;

        public bool IsPrimeValid => !PrimeValeur?.IsObligatoire ?? true
            || PrimeValeur.ValeurOrigine != decimal.Zero && !string.IsNullOrWhiteSpace(PrimeValeur.Unite?.Code)
            || PrimeProvisionnelle != decimal.Zero;

        /* KDGMNTBASE */
        public decimal PrimeMontantBase { get; set; }
        /* KDGPRIMPRO */
        public decimal PrimeProvisionnelle { get; set; }
        /* KDGTMC */
        public decimal TotalMontantCalcule { get; set; }
        /* KDGTFF */
        public decimal TotalMontantForce { get; set; }

        /* KDGCMC */
        public decimal ComptantMontantcalcule { get; set; }
        /* KDGCHT */
        public decimal ComptantMontantForceHT { get; set; }
        /* KDGCTT */
        public decimal ComptantMontantForceTTC { get; set; }
        public long Id { get; set; }
        public int? NumeroAvenant { get; set; }

        internal void ResetIds()
        {
            Id = default;
            LCI?.ResetIds();
            Franchise?.ResetIds();
        }

        ///* KDGLCIMOD */
        //public bool? IsLCIModifiable { get; set; }
        ///* KDGLCIOBL */
        //public bool? IsLCIObligatoire { get; set; }
        ///* KDGLCIVALO */
        //public decimal LCIValeurOrigine { get; set; }
        ///* KDGLCIVALA */
        //public decimal LCIValeurActualis { get; set; }
        ///* KDGLCIVALW */
        //public decimal LCIValeurTravail { get; set; }
        ///* KDGLCIUNIT */
        //public Unite LCIUnite { get; set; }
        ///* KDGLCIBASE */
        //public BaseDeCalcul LCIBase { get; set; }
        ///* KDGKDIID */
        //public ExpressionComplexeLCI LienKPEXPLCI { get; set; }
        ///* KDGFRHMOD */
        //public bool? IsFranchiseModifiable { get; set; }
        ///* KDGFRHOBL */
        //public bool? IsFranchiseObligatoire { get; set; }
        ///* KDGFRHVALO */
        //public decimal FranchiseValeurOrigine { get; set; }
        ///* KDGFRHVALA */
        //public decimal FranchiseValeurActuel { get; set; }
        ///* KDGFRHVALW */
        //public decimal FranchiseValeurTravail { get; set; }
        ///* KDGFRHUNIT */
        //public Unite FranchiseUnite { get; set; }
        ///* KDGFRHBASE */
        //public BaseDeCalcul FranchiseBase { get; set; }
        ///* KDGKDKID */
        //public ExpressionComplexeFranchise LienKEXPFRH { get; set; }
        ///* KDGFMIVALO */
        //public decimal FranchiseMiniValOrigine { get; set; }
        ///* KDGFMIVALA */
        //public decimal FranchiseMiniValActuel { get; set; }
        ///* KDGFMIVALW */
        //public decimal FranchiseMiniValTravail { get; set; }
        ///* KDGFMIUNIT */
        //public string FRanchiseMiniUnite { get; set; }
        ///* KDGFMIBASE */
        //public string FranchiseMiniBase { get; set; }
        ///* KDGFMAVALO */
        //public decimal FranchiseMaxValOrigine { get; set; }
        ///* KDGFMAVALA */
        //public decimal FranchiseMaxValActuel { get; set; }
        ///* KDGFMAVALW */
        //public decimal FranchiseMaxValTravail { get; set; }
        ///* KDGFMAUNIT */
        //public string FranchiseMaxUnite { get; set; }
        ///* KDGFMABASE */
        //public string FranchiseMaxBase { get; set; }
        ///* KDGPRIMOD */
        //public bool? IsPrimeModifiable { get; set; }
        ///* KDGPRIOBL */
        //public bool? IsPrimeObligatoire { get; set; }
        ///* KDGPRIVALO */
        //public decimal PrimeValeurOrigine { get; set; }
        ///* KDGPRIVALA */
        //public decimal PrimeValeurActuel { get; set; }
        ///* KDGPRIVALW */
        //public decimal PrimevaleurTravail { get; set; }
        ///* KDGPRIUNIT */
        //public string PrimeUnite { get; set; }
        ///* KDGPRIBASE */
        //public string PrimeBase { get; set; }


    }
}
