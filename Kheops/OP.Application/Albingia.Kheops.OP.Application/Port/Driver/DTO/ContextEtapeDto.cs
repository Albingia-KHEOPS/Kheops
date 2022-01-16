using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using OP.WSAS400.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Port.Driver.DTO {
    public class ContextEtapeDto {
        private readonly ContextEtapeState stepState;
        private readonly ContextEtapeState[] stepsToInvalidate;
        private bool isValidated;
        public ContextEtapeDto(ContextStepName step) {
            this.isValidated = false;
            this.stepState = step;
            switch (step) {
                //case ContextStepName.Inconnue:
                //    break;
                //case ContextStepName.Recherche:
                //    break;
                case ContextStepName.Creation:
                    break;
                case ContextStepName.EtablirAffaireNouvelle:
                    break;
                case ContextStepName.NouvelleAffaire:
                    break;
                //case ContextStepName.ConsulterOuEditer:
                //    break;
                //case ContextStepName.ConsulterRegule:
                //    break;
                case ContextStepName.DoubleSaisie:
                    break;
                //case ContextStepName.Edition:
                //    break;
                case ContextStepName.EditionInfosBase:
                    break;
                case ContextStepName.EditionInfosGenerales:
                    break;
                case ContextStepName.EditionCoAssureurs:
                    break;
                case ContextStepName.EditionCommissions:
                    break;
                case ContextStepName.CreationAvenant:
                    break;
                case ContextStepName.EditionParitelle:
                    break;
                case ContextStepName.ConfirmerOffre:
                    break;
                case ContextStepName.RetourPieces:
                    break;
                case ContextStepName.EditionAvenant:
                    break;
                case ContextStepName.EditionInfosAvenant:
                    break;
                case ContextStepName.CreationRegularisation:
                    break;
                case ContextStepName.EditionRegularisation:
                    break;
                case ContextStepName.EditionRegularisationEtAvenant:
                    break;
                case ContextStepName.EditionResiliation:
                    break;
                case ContextStepName.EditionRemiseEnVigueur:
                    break;
                case ContextStepName.ChoixClauses:
                    break;
                case ContextStepName.MatriceRisque:
                    break;
                case ContextStepName.MatriceFormule:
                    break;
                case ContextStepName.MatriceGarantie:
                    break;
                case ContextStepName.DetailsRisque:
                    break;
                case ContextStepName.DetailsObjetRisque:
                    break;
                case ContextStepName.InformationsSpecifiquesRisque:
                    break;
                case ContextStepName.InformationsComplementairesRisque:
                    this.stepsToInvalidate = new ContextEtapeState[] {
                        ContextStepName.EditionOption,
                        ContextStepName.ConditionsGarantie,
                        ContextStepName.EditionQuittance,
                        ContextStepName.ControleFin,
                        ContextStepName.Attentat,
                        ContextStepName.EditionMontantsReference
                    };
                    break;
                case ContextStepName.InformationsSpecifiquesObjets:
                    break;
                case ContextStepName.EditionInventaires:
                    break;
                //case ContextStepName.EditionOption:
                //    break;
                case ContextStepName.InformationsSpecifiquesGarantie:
                    break;
                case ContextStepName.ConditionsGarantie:
                    this.stepsToInvalidate = new ContextEtapeState[] {
                        ContextStepName.EditionQuittance,
                        ContextStepName.ControleFin,
                        ContextStepName.Attentat,
                        ContextStepName.EditionMontantsReference
                    };
                    break;
                case ContextStepName.EditionEngagements:
                    break;
                case ContextStepName.EngagementPeriodes:
                    break;
                case ContextStepName.EditionMontantsReference:
                    break;
                case ContextStepName.EditionQuittance:
                    break;
                case ContextStepName.AnnulationQuittances:
                    break;
                case ContextStepName.GestionDocuments:
                    break;
                case ContextStepName.InfosFinOffre:
                    break;
                case ContextStepName.ControleFin:
                    break;
                case ContextStepName.Validation:
                    break;
                case ContextStepName.Attestation:
                    break;
                case ContextStepName.PrisePosition:
                    break;
                case ContextStepName.ClassementSansSuite:
                    break;
                default:
                    break;
            }
        }

        public AffaireId AffaireId { get; set; }
        public int? NumRisque { get; set; }

        public bool MustUpdate => this.isValidated
            && (this.stepState.State != ContextEtateStateValue.Undefined
            || this.stepsToInvalidate.Any(s => s.State != ContextEtateStateValue.Undefined));

        public void NotifyUpdate(ContextEtapeData data) {
            if (this.isValidated) {
                return;
            }
            if (this.stepState.Step == ContextStepName.ConditionsGarantie) {
                if ((data & ContextEtapeData.Gareat) == ContextEtapeData.Gareat) {
                    this.stepState.State = ContextEtateStateValue.Validate;
                    foreach (var s in this.stepsToInvalidate) {
                        s.State = ContextEtateStateValue.Invalidate;
                    }
                }
                else if ((data & ContextEtapeData.Garantie) == ContextEtapeData.Garantie) {
                    this.stepState.State = ContextEtateStateValue.Validate;
                    foreach (var s in this.stepsToInvalidate.Where(x => x.Step.IsIn(ContextStepName.EditionQuittance, ContextStepName.ControleFin))) {
                        s.State = ContextEtateStateValue.Invalidate;
                    }
                }
            }
            else if (this.stepState.Step == ContextStepName.InformationsComplementairesRisque) {
                if ((data & ContextEtapeData.Garantie) == ContextEtapeData.Garantie) {
                    foreach (var s in this.stepsToInvalidate) {
                        s.State = ContextEtateStateValue.Invalidate;
                    }
                }
            }
            this.isValidated = true;
        }

        public IEnumerable<ContextEtapeState> StepsToUpdate {
            get {
                if (!this.isValidated) {
                    yield break;
                }
                foreach (var s in this.stepsToInvalidate.Where(s => s.State != ContextEtateStateValue.Undefined)) {
                    yield return s;
                }
                if (this.stepState.State != ContextEtateStateValue.Undefined) {
                    yield return this.stepState;
                }
            }
        }
    }

    public class ContextEtapeState {
        public ContextStepName Step { get; set; }
        public ContextEtateStateValue State { get; set; }
        public static implicit operator ContextEtapeState(ContextStepName stepName) {
            return new ContextEtapeState { Step = stepName };
        }
        public static implicit operator ContextStepName(ContextEtapeState stepState) {
            return stepState?.Step ?? ContextStepName.Inconnue;
        }
    }

    [Flags]
    public enum ContextEtapeData {
        Garantie = 0x01,
        Police = 0x02,
        Risque = 0x04,
        Objet = 0x08,
        Gareat = 0x10
    }

    public enum ContextEtateStateValue {
        Undefined = 0,
        Validate,
        Invalidate
    }
}
