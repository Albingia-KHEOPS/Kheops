using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Risque;
using ALBINGIA.Framework.Common;

namespace ALBINGIA.OP.OP_MVC.Common {
    /// <summary>
    /// Propotype de modelisation du flux de navigation : ne pas utiliser
    /// </summary>
    public class NavigationFlow {

        private static Dictionary<FlowName,  Dictionary<(FlowStep, FlowAction), FlowStep>> Flows =
            new Dictionary<FlowName,Dictionary<(FlowStep, FlowAction), FlowStep>> {
            [FlowName.Contrat] = new Dictionary<(FlowStep, FlowAction), FlowStep> {
                [(FlowStep.Consult,FlowAction.Next)]                 = FlowStep.InfoGenerale,
                [(FlowStep.New,FlowAction.Next)]                     = FlowStep.InfoBase,
                [(FlowStep.InfoGenerale, FlowAction.Next)]           = FlowStep.ChoixClause,
                [(FlowStep.Coassureurs, FlowAction.Next)]            = FlowStep.Commissions,
                [(FlowStep.Commissions, FlowAction.Next)]            = FlowStep.MatriceRisque,
                [(FlowStep.ObjetCreation, FlowAction.Previous)]      = FlowStep.MatriceRisque,
                [(FlowStep.Objet, FlowAction.Previous)]              = FlowStep.Risque,
                [(FlowStep.Objet, FlowAction.Next)]                  = FlowStep.InfoSpecifiqueRisque,
                [(FlowStep.Risque, FlowAction.Previous)]             = FlowStep.MatriceRisque,
                [(FlowStep.MatriceFormule, FlowAction.Next)]         = FlowStep.EngagementPeriodes,
                [(FlowStep.MatriceFormule, FlowAction.Previous)]     = FlowStep.MatriceRisque,
                [(FlowStep.MatriceGarantie, FlowAction.Next)]        = FlowStep.EngagementPeriodes,
                [(FlowStep.MatriceGarantie, FlowAction.Previous)]    = FlowStep.MatriceRisque,
                [(FlowStep.EngagementPeriodes, FlowAction.Next)]     = FlowStep.MontantsRef,
                [(FlowStep.EngagementPeriodes, FlowAction.Previous)] = FlowStep.MatriceRisque,
                [(FlowStep.Engagement, FlowAction.Previous)]         = FlowStep.EngagementPeriodes,
                [(FlowStep.EngagementTraite, FlowAction.Previous)]   = FlowStep.Engagement,
                [(FlowStep.MontantsRef, FlowAction.Next)]            = FlowStep.Recherche,
                [(FlowStep.MontantsRef, FlowAction.Previous)]        = FlowStep.EngagementPeriodes,
                [(FlowStep.ControlesFin, FlowAction.Previous)]       = FlowStep.Recherche,
                [(FlowStep.ControlesFin, FlowAction.Next)]           = FlowStep.ControlesFin,
                [(FlowStep.Document, FlowAction.Next)]               = FlowStep.Validation,
                [(FlowStep.Document, FlowAction.Previous)]           = FlowStep.Cotisation,
                [(FlowStep.Validation, FlowAction.Next)]             = FlowStep.Recherche,
                [(FlowStep.Validation, FlowAction.Previous)]         = FlowStep.ControlesFin,
                [(FlowStep.Formule, FlowAction.Next)]                = FlowStep.InfoSpecifiqueFormule,
                [(FlowStep.Formule, FlowAction.Previous)]            = FlowStep.InfoSpecifiqueFormule,

            },
            [FlowName.AvenantModif] = new Dictionary<(FlowStep, FlowAction), FlowStep> {
                [(FlowStep.Consult, FlowAction.Next)]                = FlowStep.InfoGenerale,
                [(FlowStep.New, FlowAction.Next)]                    = FlowStep.InfoBase,
                [(FlowStep.InfoAvenant, FlowAction.Next)]            = FlowStep.InfoGenerale,
                [(FlowStep.InfoGenerale, FlowAction.Next)]           = FlowStep.ChoixClause,
                [(FlowStep.Coassureurs, FlowAction.Next)]            = FlowStep.Commissions,
                [(FlowStep.Commissions, FlowAction.Next)]            = FlowStep.MatriceRisque,
                [(FlowStep.ObjetCreation, FlowAction.Previous)]      = FlowStep.MatriceRisque,
                [(FlowStep.Objet, FlowAction.Previous)]              = FlowStep.Risque,
                [(FlowStep.Objet, FlowAction.Next)]                  = FlowStep.InfoSpecifiqueRisque,
                [(FlowStep.Risque, FlowAction.Previous)]             = FlowStep.MatriceRisque,
                [(FlowStep.MatriceFormule, FlowAction.Next)]         = FlowStep.EngagementPeriodes,
                [(FlowStep.MatriceFormule, FlowAction.Previous)]     = FlowStep.MatriceRisque,
                [(FlowStep.MatriceGarantie, FlowAction.Next)]        = FlowStep.EngagementPeriodes,
                [(FlowStep.MatriceGarantie, FlowAction.Previous)]    = FlowStep.MatriceRisque,
                [(FlowStep.EngagementPeriodes, FlowAction.Next)]     = FlowStep.MontantsRef,
                [(FlowStep.EngagementPeriodes, FlowAction.Previous)] = FlowStep.MatriceRisque,
                [(FlowStep.Engagement, FlowAction.Previous)]         = FlowStep.EngagementPeriodes,
                [(FlowStep.EngagementTraite, FlowAction.Previous)]   = FlowStep.Engagement,
                [(FlowStep.MontantsRef, FlowAction.Next)]            = FlowStep.Recherche,
                [(FlowStep.MontantsRef, FlowAction.Previous)]        = FlowStep.EngagementPeriodes,
                [(FlowStep.ControlesFin, FlowAction.Previous)]       = FlowStep.Recherche,
                [(FlowStep.ControlesFin, FlowAction.Next)]           = FlowStep.ControlesFin,
                [(FlowStep.Document, FlowAction.Next)]               = FlowStep.Validation,
                [(FlowStep.Document, FlowAction.Previous)]           = FlowStep.Cotisation,
                [(FlowStep.Validation, FlowAction.Next)]             = FlowStep.Recherche,
                [(FlowStep.Validation, FlowAction.Previous)]         = FlowStep.ControlesFin,
                [(FlowStep.Formule, FlowAction.Next)]                = FlowStep.InfoSpecifiqueFormule,
                [(FlowStep.Formule, FlowAction.Previous)]            = FlowStep.InfoSpecifiqueFormule,
            },
            [FlowName.Offre] = new Dictionary<(FlowStep, FlowAction), FlowStep> {
                [(FlowStep.Consult, FlowAction.Next)]                = FlowStep.InfoGenerale,
                [(FlowStep.New, FlowAction.Next)]                    = FlowStep.InfoBase,
                [(FlowStep.InfoGenerale, FlowAction.Next)]           = FlowStep.ChoixClause,
                [(FlowStep.Coassureurs, FlowAction.Next)]            = FlowStep.Commissions,
                [(FlowStep.Commissions, FlowAction.Next)]            = FlowStep.MatriceRisque,
                [(FlowStep.ObjetCreation, FlowAction.Previous)]      = FlowStep.MatriceRisque,
                [(FlowStep.Objet, FlowAction.Previous)]              = FlowStep.Risque,
                [(FlowStep.Objet, FlowAction.Next)]                  = FlowStep.InfoSpecifiqueRisque,
                [(FlowStep.Risque, FlowAction.Previous)]             = FlowStep.MatriceRisque,
                [(FlowStep.MatriceFormule, FlowAction.Next)]         = FlowStep.EngagementPeriodes,
                [(FlowStep.MatriceFormule, FlowAction.Previous)]     = FlowStep.MatriceRisque,
                [(FlowStep.MatriceGarantie, FlowAction.Next)]        = FlowStep.EngagementPeriodes,
                [(FlowStep.MatriceGarantie, FlowAction.Previous)]    = FlowStep.MatriceRisque,
                [(FlowStep.EngagementPeriodes, FlowAction.Next)]     = FlowStep.MontantsRef,
                [(FlowStep.EngagementPeriodes, FlowAction.Previous)] = FlowStep.MatriceRisque,
                [(FlowStep.Engagement, FlowAction.Previous)]         = FlowStep.EngagementPeriodes,
                [(FlowStep.EngagementTraite, FlowAction.Previous)]   = FlowStep.Engagement,
                [(FlowStep.MontantsRef, FlowAction.Next)]            = FlowStep.Recherche,
                [(FlowStep.MontantsRef, FlowAction.Previous)]        = FlowStep.EngagementPeriodes,
                [(FlowStep.ControlesFin, FlowAction.Previous)]       = FlowStep.Recherche,
                [(FlowStep.ControlesFin, FlowAction.Next)]           = FlowStep.ControlesFin,
                [(FlowStep.Document, FlowAction.Next)]               = FlowStep.Validation,
                [(FlowStep.Document, FlowAction.Previous)]           = FlowStep.Cotisation,
                [(FlowStep.Validation, FlowAction.Next)]             = FlowStep.Recherche,
                [(FlowStep.Validation, FlowAction.Previous)]         = FlowStep.ControlesFin,
                [(FlowStep.Formule, FlowAction.Next)]                = FlowStep.InfoSpecifiqueFormule,
                [(FlowStep.Formule, FlowAction.Previous)]            = FlowStep.InfoSpecifiqueFormule,
            },
            [FlowName.Regularisation]                                       = new Dictionary<(FlowStep, FlowAction), FlowStep> {
                [(FlowStep.ListeRegule, FlowAction.Next)]               = FlowStep.PeriodeRegul,
                [(FlowStep.PeriodeRegul, FlowAction.Next)]              = FlowStep.RisqueRegul,
                [(FlowStep.RisqueRegul, FlowAction.Next)]               = FlowStep.GarantiesRegul,
                [(FlowStep.GarantiesRegul, FlowAction.Next)]            = FlowStep.PeriodGarantiesRegul,
                [(FlowStep.PeriodGarantiesRegul, FlowAction.Next)]      = FlowStep.CalculRegul,
                [(FlowStep.CalculRegul, FlowAction.Next)]               = FlowStep.Cotisation,
                [(FlowStep.Cotisation, FlowAction.Next)]                = FlowStep.ControlesFin,
                [(FlowStep.ControlesFin, FlowAction.Next)]              = FlowStep.Document,
                [(FlowStep.Document, FlowAction.Next)]                  = FlowStep.Validation,
                [(FlowStep.Validation, FlowAction.Next)]                = FlowStep.Recherche,

                [(FlowStep.ListeRegule , FlowAction.Previous)]          = FlowStep.Recherche,
                [(FlowStep.PeriodeRegul, FlowAction.Previous)]          = FlowStep.ListeRegule,
                [(FlowStep.RisqueRegul , FlowAction.Previous)]          = FlowStep.PeriodeRegul,
                [(FlowStep.GarantiesRegul , FlowAction.Previous)]       = FlowStep.RisqueRegul,
                [(FlowStep.PeriodGarantiesRegul , FlowAction.Previous)] = FlowStep.GarantiesRegul,
                [(FlowStep.CalculRegul , FlowAction.Previous)]          = FlowStep.PeriodGarantiesRegul,
                [(FlowStep.Cotisation , FlowAction.Previous)]           = FlowStep.CalculRegul,
                [(FlowStep.ControlesFin , FlowAction.Previous)]         = FlowStep.Cotisation,
                [(FlowStep.Document , FlowAction.Previous)]             = FlowStep.ControlesFin,
                [(FlowStep.Validation , FlowAction.Previous)]           = FlowStep.Document,

            },
            [FlowName.Engagement] = new Dictionary<(FlowStep, FlowAction), FlowStep> {
                [(FlowStep.EngagementPeriodes, FlowAction.Next)] = FlowStep.MontantsRef,
                [(FlowStep.EngagementPeriodes, FlowAction.Previous)] = FlowStep.MatriceRisque,
                [(FlowStep.Engagement, FlowAction.Previous)] = FlowStep.EngagementPeriodes,
                [(FlowStep.EngagementTraite, FlowAction.Previous)] = FlowStep.Engagement,
            },
        };

        private NavigationFlowState state;

        public NavigationFlow(NavigationFlowState state) {
            this.state = state;
        }

        public static NavigationFlowState ProcessAction(NavigationFlowState state, FlowAction action, object ActionParam) {
            return new NavigationFlow( (NavigationFlowState)state.Clone()).ProcessAction(action,ActionParam);
        }
        public NavigationFlowState ProcessAction(FlowAction action, object ActionParam) {
            FlowStep nextStep = state.CurrentFlowStep;
            if (Flows.TryGetValue(state.FlowName, out var flow) && flow.TryGetValue((state.CurrentFlowStep, action), out var newStep)) {
                nextStep = newStep;
                state.CurrentFlowStep = newStep;
                return state;
            }
            switch (action) {
                case FlowAction.Next:
                    if (state.CurrentFlowStep == FlowStep.MatriceRisque) {
                        if (GetAffaireRisques(state.CurrentFolder).Any()) {
                            state.CurrentFlowStep = FlowStep.Objet;
                        } else {
                            state.CurrentFlowStep = FlowStep.Engagement;
                        }
                    }
                    break;
                case FlowAction.Previous:

                    break;
                case FlowAction.Detail:

                    break;
                case FlowAction.Jump:

                    break;
                case FlowAction.Exit:
                    break;
                default:
                    break;
            }
            return state;
        }

        private IEnumerable<Risque> GetAffaireRisques(Folder currentFolder) {
            throw new NotImplementedException();
        }
    }
        public enum FlowStep {
            Consult = 0,
            New = 1,
            InfoAvenant,
            InfoBase,
            InfoGenerale,
            Coassureurs,
            Commissions,
            MatriceRisque,
            MatriceFormule,
            MatriceGarantie,
            Risque,
            Objet,
            Formule,
            ConditionTarifaires,
            EngagementPeriodes,
            Engagement,
            EngagementTraite,
            MontantsRef,
            Recherche,
            ControlesFin,
            PeriodeRegul,
            RisqueRegul,
            GarantiesRegul,
            PeriodGarantiesRegul,
            CalculRegul,
            Cotisation,
            CourtierRegul,
            ChoixClause,
            ObjetCreation,
            InfoSpecifiqueRisque,
            Document,
            Validation,
            InfoSpecifiqueFormule,
            ListeRegule
    }
}