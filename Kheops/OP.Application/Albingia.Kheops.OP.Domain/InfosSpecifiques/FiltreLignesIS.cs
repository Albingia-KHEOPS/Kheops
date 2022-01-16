using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.InfosSpecifiques
{
    public class FiltreLignesIS
    {

        enum Condition
        {
            _none = 0,
            TEST_CACHER_IS,
            FORM_IA_NB_PERS_1,
            FORM_IA_INFIRMITE,
            FORM_FRAISCO,
            RSQ_CIBLE_AUDIO_TV,
            IA_MISSION,
            FORM_IA_NB_PERS_2,
            NOMEN_PERTEPEC_ACTIV,
            RSQ_NOMEN_PERTEPEC_ACTIV,
            RISQUE_CIBLE_CONG_FOIRE_MAN_SPE_THE,
            RISQUE_CIBLE_CONGRES_FOIRE,
            RISQUE_CONG_CEREM,
            RSQ_NOMEN_PRODU,
            FORM_IA_NB_DATE_NAISSANCE,
            FORM_IA_REPORT_LIMITE_AGE,
            FORM_IA_INFIRMITE_PREEXIS,
            FORM_RCAUD,
            FORM_RCPIM,
            FORM_RCSPS,
            RSQ_SANSOBJ_CIBLE_AUDIO_TV,
            RSQ_SANSOBJ_TV,
            FORM_CIBLE_AUD_TV,
            FORM_ANNUL,
            FORM_INTEMP,
            FORM_ATTANN,
            FORM_ATTAN_2,
            FORM_CIBLE_THEATRE,
            FORM_RSAUD,
            FORM_FFREDI,
            FORM_DOMMAG,
            FORM_UNI,
            FORM_IMPACC,
            FORM_EXCLU_AUDIO_TV_CER,
            FORM_AP_ENCOL,
            FORM_AP_VIN
        }

        public Affaire.Affaire Affaire { get; set; }
        public Risque.Risque Risque { get; set; }
        public Risque.Objet Objet { get; set; }
        public Formule.Option Option { get; set; }

        public IEnumerable<LigneModeleIS> Filtrer(IEnumerable<LigneModeleIS> lignes)
        {
            bool previousVisible1 = false;
            bool previousVisible2 = false;
            var results = lignes.OrderBy(x => x.Ordre).Where(p =>
            {
                bool display;
                switch (p.TypePresentation)
                {
                    case 2:
                        display = previousVisible1 ? AfficherLigne(p.CodeConditions) : previousVisible1;
                        previousVisible2 = display;
                        break;
                    case 3:
                        display = previousVisible2 ? AfficherLigne(p.CodeConditions) : previousVisible2;
                        break;
                    default:
                        display = AfficherLigne(p.CodeConditions);
                        previousVisible1 = display;
                        break;
                }
                return display;
            }).ToList();
            if (!results.Any(x => x.TypePresentation == 1) || !results.Any(x => x.TypePresentation == 2) || !results.Any(x => x.TypePresentation == 3))
            {
                results.Clear();
            }
            return results;
        }

        public bool AfficherLigne(string codeScript)
        {
            var condition = Enum.TryParse(codeScript, out Condition c) ? c : default;
            if (Affaire.Branche.Code == Referentiel.Branche.RisquesSpeciaux.Code)
            {
                return AfficherLigneRS(condition);
            }
            if (Affaire.Branche.Code == Referentiel.Branche.ResponcabiliteCivile.Code)
            {
                return AfficherLigneRC(condition);
            }
            if (Affaire.Branche.Code == Referentiel.Branche.IndividuelleAccident.Code)
            {
                return AfficherLigneIA(condition);
            }
            if (Affaire.Branche.Code == Referentiel.Branche.ArtEtPrecieux.Code)
            {
                return AfficherLigneAP(condition);
            }

            if (Affaire.Branche.Code == Referentiel.Branche.Multirisques.Code)
            {
                return true;
            }

            return false;
        }

        private bool AfficherLigneRC(Condition condition)
        {
            switch (condition)
            {
                case Condition.FORM_RCAUD:
                    return Affaire.Cible.Code == "AUDRC";
                case Condition.FORM_RCPIM:
                    return Affaire.Cible.Code == "RCPIM";
                case Condition.FORM_RCSPS:
                    return Affaire.Cible.Code == "SPS";
                default:
                    return true;
            }
        }

        private bool AfficherLigneRS(Condition condition)
        {
            var dateLimiteAttann = new DateTime(2017, 6, 28);
            switch (condition)
            {
                case Condition.TEST_CACHER_IS:
                    return false;
                case Condition.FORM_FRAISCO:
                    return Risque.Cible.Code == "FRAISCO";
                case Condition.RSQ_CIBLE_AUDIO_TV:
                    return Risque.Cible.Code.IsIn("AUDIORS", "TV");
                case Condition.NOMEN_PERTEPEC_ACTIV:
                case Condition.RSQ_NOMEN_PERTEPEC_ACTIV:
                    return Risque.Nomenclature01.IsIn("RRS3", "RRS1", "RRS7", "RRS8", "RRS10") && !Risque.Cible.Code.IsIn("AUDIORS", "TV");
                case Condition.RISQUE_CIBLE_CONG_FOIRE_MAN_SPE_THE:
                    return Risque.Cible.Code.IsIn("CONGRES", "FOIRE", "MANIFSPO", "SPECTACLE", "THEATRE");
                case Condition.RISQUE_CIBLE_CONGRES_FOIRE:
                    return Risque.Cible.Code.IsIn("CONGRES", "FOIRE");
                case Condition.RISQUE_CONG_CEREM:
                    return Risque.Cible.Code.IsIn("CONGRES", "CEREMONIE");
                case Condition.RSQ_NOMEN_PRODU:
                    return Risque.Nomenclature01 == "RRS5";
                case Condition.RSQ_SANSOBJ_CIBLE_AUDIO_TV:
                    return Risque.Cible.Code.IsIn("AUDIORS", "TV") && Risque.Objets.Count == 1;
                case Condition.RSQ_SANSOBJ_TV:
                    return Risque.Cible.Code == "TV" && Risque.Objets.Count == 1;
                case Condition.FORM_CIBLE_AUD_TV:
                    return FormuleInCibles("AUDIORS", "TV");
                case Condition.FORM_ANNUL:
                    if (Option.OptionVolets.Any(v => v.IsChecked && v.Blocs.Any(b => b.IsChecked && b.ParamBloc.Code == "ANNEXP"))
                        || !Option.OptionVolets.Any(v => v.IsChecked && v.ParamVolet.Code.IsIn("ANNUL", "ANNULORG", "ANNULORP")))
                    {
                        return false;
                    }
                    return Option.OptionVolets.Any(v => v.IsChecked && v.Blocs.Any(b => b.IsChecked && b.ParamBloc.Code == "RSAUD"))
                        || Option.AllSelectedGaranties.Any(g => g.CodeGarantie.IsIn("ATTANN", "FRREDI"))
                        || FormuleInCibles("THEATRE");
                case Condition.FORM_INTEMP:
                    return !Option.OptionVolets.Any(v => v.IsChecked && v.Blocs.Any(b => b.IsChecked && b.ParamBloc.Code == "ANNEXP"))
                        && Option.AllSelectedGaranties.Any(g => g.CodeGarantie == "INTEMP");
                case Condition.FORM_ATTANN:
                    if (!Option.AllSelectedGaranties.Any(g => g.CodeGarantie == "ATTANN"))
                    {
                        return false;
                    }
                    if (Affaire.TypeAffaire == Domain.Affaire.AffaireType.Offre || Affaire.OffreOrigine is null)
                    {
                        return Affaire.DateCreation < dateLimiteAttann;
                    }
                    else
                    {
                        return Affaire.DateSaisieOffre < dateLimiteAttann;
                    }
                case Condition.FORM_ATTAN_2:
                    if (!Option.AllSelectedGaranties.Any(g => g.CodeGarantie == "ATTANN"))
                    {
                        return false;
                    }
                    if (Affaire.TypeAffaire == Domain.Affaire.AffaireType.Offre)
                    {
                        return Affaire.DateSaisie >= dateLimiteAttann;
                    }
                    else
                    {
                        if (Affaire.OffreOrigine is null)
                        {
                            return Affaire.DateAccord >= dateLimiteAttann;
                        }
                        else
                        {
                            return Affaire.DateSaisieOffre >= dateLimiteAttann;
                        }
                    }
                case Condition.FORM_CIBLE_THEATRE:
                    return FormuleInCibles("THEATRE");
                case Condition.FORM_RSAUD:
                    return Option.OptionVolets.Any(v => v.IsChecked && v.Blocs.Any(b => b.IsChecked && b.ParamBloc.Code == "RSAUD"))
                        && FormuleInCibles("TV");
                case Condition.FORM_FFREDI:
                    return Option.AllSelectedGaranties.Any(g => g.CodeGarantie == "FRREDI");
                case Condition.FORM_DOMMAG:
                case Condition.FORM_UNI:
                    return FormuleInCibles("UNI") && Option.AllSelectedGaranties.Any(g => g.CodeGarantie == "DOMUNI")
                        || Option.OptionVolets.Any(v => v.IsChecked && v.Blocs.Any(b => b.IsChecked && b.ParamBloc.Code == "DOMEXP"))
                        || Option.OptionVolets.Any(v => v.IsChecked && v.Blocs.Any(b => b.IsChecked && b.ParamBloc.Code == "DOMORG")) && FormuleInCibles("CONGRES", "FOIRE");
                case Condition.FORM_IMPACC:
                    return Option.AllSelectedGaranties.Any(g => g.CodeGarantie == "IMPACC");
                case Condition.FORM_EXCLU_AUDIO_TV_CER:
                    return !FormuleInCibles("PPECU", "FOIRE", "CONGRES", "AUDIORS", "TV")
                        && !Option.OptionVolets.Any(v => v.IsChecked && v.ParamVolet.Code == "EXPO")
                        && !Option.OptionVolets.Any(v => v.IsChecked && v.Blocs.Any(b => b.IsChecked && b.ParamBloc.Code == "ANNEXP"))
                        && !Option.AllSelectedGaranties.Any(g => g.CodeGarantie == "INTEMP");
                default:
                    return true;
            }
        }

        private bool AfficherLigneIA(Condition condition)
        {
            switch (condition)
            {
                case Condition.FORM_IA_NB_PERS_1:
                    return !Affaire.Cible.Code.IsIn("IMAGE", "GRCEQCLI", "GRCEQSAL", "IMACEQ") && (Risque.Nomenclature01 == "RADP1" || Objet?.Nomenclature01 == "RADP1");
                case Condition.FORM_IA_NB_PERS_2:
                    return Affaire.Cible.Code != "IMAGE" && (Risque.Nomenclature01 == "RADP1" || Objet?.Nomenclature01 == "RADP1");
                case Condition.FORM_IA_INFIRMITE:
                case Condition.FORM_IA_INFIRMITE_PREEXIS:
                    return Affaire.Cible.Code.IsIn("PARTICULIE", "MISSION", "VOY", "FRG", "HCLE") && (Risque.Nomenclature01 == "RADP1" || Objet?.Nomenclature01 == "RADP1");
                case Condition.IA_MISSION:
                    return Affaire.Cible.Code == "MISSION";
                case Condition.FORM_IA_NB_DATE_NAISSANCE:
                case Condition.FORM_IA_REPORT_LIMITE_AGE:
                    return Risque.Nomenclature01 == "RADP1" || Objet?.Nomenclature01 == "RADP1";
                default:
                    return true;
            }
        }

        private bool AfficherLigneAP(Condition condition)
        {
            switch (condition)
            {
                case Condition.FORM_AP_ENCOL:
                    return Affaire.Cible.Code == "ENCOL";
                case Condition.FORM_AP_VIN:
                    return Affaire.Cible.Code.IsIn("CPCOL", "CVCOL");
                default:
                    return true;
            }
        }

        private bool FormuleInCibles(params string[] codeCibles)
        {
            if (Option != null)
            {
                var appls = Option.Applications.Where(apl => apl.NumRisque == Risque.Numero);
                if (appls.Any(a => a.NumObjet > 0))
                {
                    var nums = appls.Select(a => a.NumObjet);
                    return Risque.Objets.Where(o => nums.Contains(o.Id.NumObjet)).Any(o =>
                    {
                        var cible = o.Cible?.Code ?? Risque.Cible.Code;
                        return codeCibles.Contains(cible);
                    });
                }
                return codeCibles.Contains(Risque.Cible.Code);
            }
            else
            {
                return codeCibles.Contains(Objet?.Cible?.Code)
                    || Risque.Objets.Any(o => codeCibles.Contains(o.Cible?.Code))
                    || codeCibles.Contains(Risque.Cible.Code);
            }
        }
    }
}
