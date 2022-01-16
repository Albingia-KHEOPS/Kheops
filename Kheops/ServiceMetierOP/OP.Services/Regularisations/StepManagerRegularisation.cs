using OP.WSAS400.DTO.Regularisation;
using System.Linq;

namespace OP.Services.BLServices.Regularisations
{
    public class StepManagerRegularisation
    {
        public static RegularisationStep[] NextAvailableSteps(RegularisationContext context)
        {
            if (context == null)
            {
                return new RegularisationStep[] { RegularisationStep.ChoixMode };
            }

            switch (context.Mode)
            {
                default:
                    return DefaultNextSteps(context);
            }
        }

        public static RegularisationStep PreviousStep(RegularisationContext context, RegularisationManager regularisationManager) {
            switch (context.Mode) {
                default:
                    return DefaultPreviousSteps(context, regularisationManager);
            }
        }

        public static RegularisationStep SelectChoixCourtierNextStep(RegularisationContext context, RegularisationStep[] steps, RegularisationManager regularisationManager)
        {
            if (steps.Length == 1)
            {
                return steps.First();
            }

            // if multiple steps possible, redefine the step
            switch (context.Mode)
            {
                case RegularisationMode.Standard:
                    return Regularisation.GetStepSimplifiedRegule(context, regularisationManager);
                case RegularisationMode.PB:
                case RegularisationMode.BNS:
                case RegularisationMode.Burner:
                    return context.Scope == RegularisationScope.Contrat ? RegularisationStep.Regularisation : RegularisationStep.ChoixRisques;
            }

            return 0;
        }

        private static RegularisationStep[] DefaultNextSteps(RegularisationContext context)
        {
            switch (context.Step)
            {
                case RegularisationStep.Regularisation:
                    if (context.Scope == RegularisationScope.Contrat)
                    {
                        return new RegularisationStep[] { RegularisationStep.Cotisations };
                    }
                    else if (context.Scope == RegularisationScope.Risque)
                    {
                        return new RegularisationStep[] { RegularisationStep.ChoixRisques };
                    }
                    else
                    {
                        return new RegularisationStep[] { RegularisationStep.ChoixPeriodesGarantie };
                    }
                case RegularisationStep.ChoixRisques:
                    return new RegularisationStep[] { RegularisationStep.Cotisations };
                case RegularisationStep.ChoixPeriodeCourtier:
                    return new RegularisationStep[]
                    {
                        RegularisationStep._ControlesSpecifiques,
                        RegularisationStep.ChoixRisques,
                        RegularisationStep.ChoixGaranties,
                        RegularisationStep.ChoixPeriodesGarantie,
                        RegularisationStep.Regularisation
                    };
                default:
                    return new RegularisationStep[] { RegularisationStep.Invalid };
            }
        }

        private static RegularisationStep DefaultPreviousSteps(RegularisationContext context, RegularisationManager regularisationManager)
        {
            if (context.Step == RegularisationStep.Regularisation)
            {
                if (context.Scope == RegularisationScope.Contrat)
                {
                    return RegularisationStep.ChoixPeriodeCourtier;
                }
                else if (context.Scope == RegularisationScope.Risque)
                {
                    return RegularisationStep.ChoixRisques;
                }
                else
                {
                    return RegularisationStep.ChoixPeriodesGarantie;
                }
            }
            else if (context.Step == RegularisationStep.ChoixRisques)
            {
                return RegularisationStep.ChoixPeriodeCourtier;
            }
            else if (context.Step == RegularisationStep.Cotisations)
            {
                var steps = new RegularisationStep[]
                    {
                        RegularisationStep._ControlesSpecifiques,
                        RegularisationStep.ChoixRisques,
                        RegularisationStep.ChoixGaranties,
                        RegularisationStep.ChoixPeriodesGarantie,
                        RegularisationStep.Regularisation
                    };
                return SelectChoixCourtierNextStep(context, steps, regularisationManager);                
            }

            return 0;
        }


    }
}