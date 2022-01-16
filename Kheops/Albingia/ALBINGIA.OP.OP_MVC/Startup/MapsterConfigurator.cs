
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Inventaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.Connexites;
using Mapster;
using OP.WSAS400.DTO.Engagement;
using System;
using System.Collections.Generic;
using System.Linq;
using WSOPDTO = OP.WSAS400.DTO;

namespace ALBINGIA.OP.OP_MVC {
    internal class MapsterConfigurator
    {
        public static void Initialize()
        {
            MapInventaireGarantie(true);

            TypeAdapterConfig<Affaire, Folder>.NewConfig();

            TypeAdapterConfig<Albingia.Kheops.OP.Domain.Affaire.AffaireId, AccesAffaire>
                .NewConfig()
                .Map(target => target.Code, source => source.CodeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment)
                .Map(target => target.Avenant, source => source.NumeroAvenant);

            TypeAdapterConfig<GarantieDetailsDto, Models.FormuleGarantie.DetailsGarantie>
                .NewConfig()
                .Map(target => target.GrilleRegul, source => string.IsNullOrWhiteSpace(source.CodeGrilleRegul) ? "" : source.CodeGrilleRegul + " - " + source.LabelGrilleRegul)
                .Map(target => target.NatureRetenue, source => new LabeledValue(source.NatureRetenue.AsString(), source.LabelNatureRetenue))
                .Map(target => target.Duree, source => source.Duree == 0 ? null : source.Duree)
                .Map(target => target.DureeUnite, source => source.CodeDureeUnite)
                .Map(target => target.AvenantInitial, source => source.NumeroAvenantCreation);

            TypeAdapterConfig<Models.FormuleGarantie.DetailsGarantie, GarantieDetailsDto>
                .NewConfig()
                .Map(target => target.NatureRetenue, source => source.NatureRetenue.Code.AsEnum<Albingia.Kheops.OP.Domain.Referentiel.NatureValue>())
                .Map(target => target.CodeDureeUnite, source => source.DureeUnite)
                .Map(target => target.NumeroAvenantCreation, source => source.AvenantInitial);

            TypeAdapterConfig<PorteeGarantieDto, Models.FormuleGarantie.PorteeObjet>
                .NewConfig()
                .Map(target => target.TypeCalculPortee, source => source.TypeCalcul.AsString())
                .Map(target => target.Valeur, source => source.Valeurs.ValeurTravail)
                .Map(target => target.ValeurPortee, source => source.Valeurs.ValeurActualise)
                .Map(target => target.UniteTaux, source => source.Valeurs.Unite.Code);

            TypeAdapterConfig<Models.FormuleGarantie.PorteeObjet, PorteeGarantieDto>
                .NewConfig()
                .Map(target => target.TypeCalcul, source => source.TypeCalculPortee.AsEnum<TypeCalcul>())
                .Map(target => target.Valeurs, source => new ValeursUnite
                {
                    ValeurTravail = source.Valeur,
                    ValeurActualise = source.ValeurPortee,
                    Unite = new Albingia.Kheops.OP.Domain.Referentiel.Unite { Code = source.UniteTaux }
                });

            MapGarantie(true);

            TypeAdapterConfig<OptionsDetailBlocDto, Models.FormuleGarantie.Bloc>
                .NewConfig()
                .Map(target => target.UniqueId, source => source.ParamBloc.CatBlocId)
                .Map(target => target.Caractere, source => new LabeledValue(source.Caractere.AsString(), source.Caractere.ToString()))
                .Map(target => target.Code, source => source.ParamBloc.Code)
                .Map(target => target.Libelle, source => source.ParamBloc.Description)
                .Map(target => target.IsCollapsed, source => source.ParamVolet.IsVoletCollapsed)
                .Map(target => target.Type, source => source.Type.AsString());

            TypeAdapterConfig<OptionsDetailVoletDto, Models.FormuleGarantie.Volet>
                .NewConfig()
                .Map(target => target.UniqueId, source => source.ParamVolet.CatVoletId)
                .Map(target => target.AvenantModifie, source => false)
                .Map(target => target.Caractere, source => new LabeledValue(source.Caractere.AsString(), source.Caractere.ToString()))
                .Map(target => target.Code, source => source.ParamVolet.Code)
                .Map(target => target.Libelle, source => source.ParamVolet.Description)
                .Map(target => target.IsCollapsed, source => source.ParamVolet.IsVoletCollapsed)
                .Map(target => target.Type, source => source.Type.AsString());

            TypeAdapterConfig<Models.FormuleGarantie.Bloc, OptionsDetailBlocDto>
                .NewConfig()
                .Map(target => target.ParamBloc, source => new ParamBlocDto { CatBlocId = source.UniqueId })
                .Map(target => target.Caractere, source => source.Caractere.Code.AsEnum<Albingia.Kheops.OP.Domain.Model.CaractereSelection>())
                .Map(target => target.Type, source => source.Type.AsEnum<TypeOption>());

            TypeAdapterConfig<Models.FormuleGarantie.Volet, OptionsDetailVoletDto>
                .NewConfig()
                .Map(target => target.ParamVolet, source => new ParamVoletDto { CatVoletId = source.UniqueId })
                .Map(target => target.Caractere, source => source.Caractere.Code.AsEnum<Albingia.Kheops.OP.Domain.Model.CaractereSelection>())
                .Map(target => target.Type, source => source.Type.AsEnum<TypeOption>());

            TypeAdapterConfig<OptionDto, Models.FormuleGarantie.Option>
                .NewConfig()
                .Map(target => target.Volets, source => source.OptionVolets)
                .Map(target => target.Numero, source => source.OptionNumber);

            TypeAdapterConfig<Models.FormuleGarantie.Option, OptionDto>
               .NewConfig()
               .Map(target => target.OptionVolets, source => source.Volets)
               .Map(target => target.OptionNumber, source => source.Numero);

            TypeAdapterConfig<Albingia.Kheops.OP.Domain.Referentiel.Cible, LabeledValue>
                .NewConfig()
                .Map(target => target.Id, source => source.ID)
                .Map(target => target.Label, source => source.Description);

            TypeAdapterConfig<FormuleDto, Models.FormuleGarantie.Formule>
                .NewConfig()
                .Map(target => target.Numero, source => source.FormuleNumber)
                .Map(target => target.Libelle, source => source.Description);

            TypeAdapterConfig<Models.FormuleGarantie.Formule, FormuleDto>
                .NewConfig()
                .Map(target => target.FormuleNumber, source => source.Numero)
                .Map(target => target.Description, source => source.Libelle);

            TypeAdapterConfig<Models.FormuleGarantie.Formule, NouvelleAffaireFormule>
                .NewConfig()
                .Map(target => target.IsSelected, source => source.IsSelected.GetValueOrDefault())
                .Map(target => target.TarifGaranties, source => new Dictionary<int, int>());

            TypeAdapterConfig<Models.FormuleGarantie.NouvelleAffaire, NouvelleAffaireDto>
                .NewConfig()
                .Map(target => target.CodeContrat, source => source.Code)
                .Map(target => target.Formules, source => source.Formules);

            TypeAdapterConfig<SelectionObjet, NouvelleAffaireObjet>
                .NewConfig()
                .Map(target => target.Numero, source => source.Code)
                .Map(target => target.IsSelected, source => source.Selected);

            TypeAdapterConfig<SelectionRisqueObjets, NouvelleAffaireRisque>
                .NewConfig()
                .Map(target => target.Numero, source => source.Code)
                .Map(target => target.IsSelected, source => source.Selected);

            TypeAdapterConfig<SelectionRisquesObjets, NouvelleAffaireDto>
                .NewConfig()
                .Map(target => target.CodeContrat, source => source.CodeAffaireNouvelle)
                .Map(target => target.Offre, source => source.Folder);

            TypeAdapterConfig<global::OP.WSAS400.DTO.SelectionRisqueObjets, SelectionRisqueObjets>
                .NewConfig()
                .Map(target => target.Unite, source => source.CodeUniteValeur)
                .Map(target => target.Type, source => source.CodeTypeValeur);

            TypeAdapterConfig<Albingia.Kheops.OP.Domain.Affaire.AffaireId, Folder>
                .NewConfig()
                .Map(target => target.CodeOffre, source => source.CodeAffaire)
                .Map(target => target.NumeroAvenant, source => source.NumeroAvenant.GetValueOrDefault())
                .Map(target => target.Type, source => source.TypeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment);

            TypeAdapterConfig<Albingia.Kheops.OP.Domain.Affaire.AffaireId, Affaire>
                .NewConfig()
                .Map(target => target.CodeOffre, source => source.CodeAffaire)
                .Map(target => target.NumeroAvenant, source => source.NumeroAvenant.GetValueOrDefault())
                .Map(target => target.Type, source => source.TypeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment);

            TypeAdapterConfig<Affaire, Albingia.Kheops.OP.Domain.Affaire.AffaireId>
                .NewConfig()
                .Map(target => target.CodeAffaire, source => source.CodeOffre)
                .Map(target => target.TypeAffaire, source => source.Type.ParseCode<Albingia.Kheops.OP.Domain.Affaire.AffaireType>())
                .Map(target => target.NumeroAliment, source => source.Version);
            TypeAdapterConfig<Folder, Albingia.Kheops.OP.Domain.Affaire.AffaireId>
                .NewConfig()
                .Map(target => target.CodeAffaire, source => source.CodeOffre)
                .Map(target => target.TypeAffaire, source => source.Type.ParseCode<Albingia.Kheops.OP.Domain.Affaire.AffaireType>())
                .Map(target => target.NumeroAliment, source => source.Version);

            TypeAdapterConfig<PeriodeConnexiteDto, PeriodeEngagement>
                .NewConfig()
                .Map(target => target.Beginning, source => source.DateDebut)
                .Map(target => target.End, source => source.DateFin)
                .Map(target => target.Id, source => (int)source.CodeEngagement)
                .Map(target => target.Valeurs, source => source.Traites.Select(x => new ValeurEngagement { CodeTraite = x.Key, Montant = x.Value, MontantTotal = 0 }).ToList());

            TypeAdapterConfig<PeriodeEngagement, PeriodeConnexiteDto>
                .NewConfig()
                .Map(target => target.DateDebut, source => source.Beginning)
                .Map(target => target.DateFin, source => source.End)
                .Map(target => target.CodeEngagement, source => source.Id)
                .Map(target => target.Traites, source => source.Valeurs.ToDictionary(v => v.CodeTraite, v => v.Montant));

            TypeAdapterConfig<PrimeDto, Models.Primes.Prime>
                .NewConfig()
                .Map(target => target.DateEcheance, source => source.DateEcheance.ToShortDateString())
                .Map(target => target.LibelleRelance, source => source.TypeRelance == null ? string.Empty : source.TypeRelance.LibelleSuppose);

            TypeAdapterConfig<AffaireDto, Contrat>
                .NewConfig()
                .Map(target => target.Code, source => source.CodeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment)
                .Map(target => target.Branche, source => source.Branche == null ? string.Empty : source.Branche.Code)
                .Map(target => target.DateEffetReelle, source => source.NumeroAvenant > 0 ? source.DateEffetAvenant : source.DateEffet);

            TypeAdapterConfig<AffaireDto, Folder>
                .NewConfig()
                .Map(target => target.CodeOffre, source => source.CodeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment)
                .Map(target => target.Type, source => source.TypeAffaire.AsCode(null, true));

            TypeAdapterConfig<RetardPaiementDto, Models.Primes.RetardPaiement>
                .NewConfig()
                .Map(target => target.Contrat, source => source.Folder)
                .Map(target => target.DateValidationContrat, source => source.Folder.DateValidation.HasValue ? source.Folder.DateValidation.Value.ToShortDateString() : string.Empty)
                .Map(target => target.Courtier, source => source.Folder.CourtierGestionnaire == null ? string.Empty : source.Folder.CourtierGestionnaire.Nom)
                .Map(target => target.DateLimite, source => source.DateLimit.ToShortDateString());

            TypeAdapterConfig<ImpayeDto, Models.Primes.Impaye>
                .NewConfig()
                .Inherits<RetardPaiementDto, Models.Primes.RetardPaiement>();

            TypeAdapterConfig<SinistreDto, Models.Sinistre>
                .NewConfig()
                .Map(target => target.Contrat, source => source.Affaire)
                .Map(target => target.ChargementTotal, source => source.CalculChargement == null ? decimal.Zero : source.CalculChargement.TotalChargement);

            TypeAdapterConfig<RelanceDto, RelanceOffre>
                .NewConfig()
                .Map(target => target.DateRelance, source => source.DateValidation.AddDays(source.DelaisRelanceJours))
                .Map(target => target.Courtier, source => source.CourtierGestionnaire != null ? source.CourtierGestionnaire.Nom : string.Empty)
                .Map(target => target.Gestionnaire, source => source.Gestionnaire.Code != null ? source.Gestionnaire.Code : string.Empty)
                .Map(target => target.Situation, source => source.Situation.AsCode(null, true))
                .Map(target => target.MotifStatut, source => source.MotifSituation != null ? source.MotifSituation.Code : string.Empty)
                .Map(target => target.Souscripteur, source => source.Souscripteur.Code != null ? source.Souscripteur.Code : string.Empty)
                .Map(target => target.Offre, source => new Offre { Branche = source.Branche.Code, Code = source.CodeOffre.ToString().ToIPB(), Version = source.Version, Libelle = source.Descriptif });

            TypeAdapterConfig<RelanceOffre, RelanceDto>
                .NewConfig()
                .Map(target => target.DelaisRelanceJours, source => (source.DateRelance - source.DateValidation).TotalDays)
                .Map(target => target.CourtierGestionnaire, source => new Albingia.Kheops.OP.Domain.Affaire.Courtier { Nom = source.Courtier })
                .Map(target => target.Gestionnaire, source => new Utilisateur { Code = source.Gestionnaire })
                .Map(target => target.Situation, source => source.Situation.ParseCode<Albingia.Kheops.OP.Domain.Affaire.SituationAffaire>())
                .Map(target => target.MotifSituation, source => new MotifSituation { Code = source.MotifStatut })
                .Map(target => target.Souscripteur, source => new Utilisateur { Code = source.Souscripteur })
                .Map(target => target.Branche, source => new Branche { Code = source.Offre.Branche })
                .Map(target => target.CodeOffre, source => int.Parse(source.Offre.Code))
                .Map(target => target.Version, source => source.Offre.Version);

            TypeAdapterConfig<RelanceOffre, Albingia.Kheops.OP.Domain.Affaire.AffaireId>
                .NewConfig()
                .Map(target => target.CodeAffaire, source => source.Offre.Code)
                .Map(target => target.TypeAffaire, source => Albingia.Kheops.OP.Domain.Affaire.AffaireType.Offre)
                .Map(target => target.NumeroAliment, source => source.Offre.Version);

            TypeAdapterConfig<FusionConnexites, WSOPDTO.FusionConnexitesDto>.NewConfig();

            TypeAdapterConfig<Models.AffaireNouvelle.ModelePage.AnInformationsBaseEnregister, Models.AffaireNouvelle.ModelePage.AnCreationContratPage>
                .NewConfig()
                .Map(target => target.GpIdentiqueApporteur, source => source.GpIdentiqueApporteur != null && source.GpIdentiqueApporteur.ToLower() == "true");
        }

        private static void MapGarantie(bool twoWay)
        {
            TypeAdapterConfig<GarantieDto, Models.FormuleGarantie.Garantie>
                .NewConfig()
                .Map(target => target.UniqueId, source => source.Sequence)
                .Map(target => target.Code, source => source.CodeGarantie)
                .Map(target => target.NatureModifiable, source => source.ParamIsNatModifiable.GetValueOrDefault())
                .Map(target => target.Avenant, source => source.NumeroAvenant)
                .Map(target => target.TypeAlimentation, source => new LabeledValue(source.TypeAlimentation.AsString(), source.TypeAlimentation.ToString()))
                .Map(target => target.Nature, source => new LabeledValue(source.Nature.AsString(), source.Nature.ToString()))
                .Map(target => target.NatureRetenue, source => new LabeledValue(source.NatureRetenue.AsString(), source.NatureRetenue.ToString()))
                .Map(target => target.Caractere, source => new LabeledValue(source.Caractere.AsString(), source.Caractere.ToString()))
                .Map(target => target.SousGaranties, source => source.SousGaranties, dto => dto.Niveau < 4)
                .Map(target => target.Libelle, source => source.DesignationGarantie)
                .Map(target => target.ActionsPortees, source => source.ActionsPortees.Select(kv => new LabeledValue(kv.Key, kv.Value)))
                .Map(target => target.Portees, source => new Models.FormuleGarantie.PorteesGarantie
                {
                    ReportCalcul = source.TypeAlimentation == Albingia.Kheops.OP.Domain.Referentiel.AlimentationValue.AssiettePrime || source.TypeAlimentation == Albingia.Kheops.OP.Domain.Referentiel.AlimentationValue.Prime,
                    ObjetsRisque = source.Portees.Adapt<List<Models.FormuleGarantie.PorteeObjet>>(),
                    CodeAction = source.Portees != null && source.Portees.Any() ? source.Portees.First().CodeAction : string.Empty,
                    TypesCalculs = Enum.GetValues(typeof(TypeCalcul))
                            .Cast<TypeCalcul>()
                            .Select(v => new LabeledValue(v.AsString(), string.IsNullOrWhiteSpace(v.AsString()) ? string.Empty : v.ToString()))
                });

            if (twoWay)
            {
                TypeAdapterConfig<Models.FormuleGarantie.Garantie, GarantieDto>
                    .NewConfig()
                    .Map(target => target.Sequence, source => source.UniqueId)
                    .Map(target => target.CodeGarantie, source => source.Code)
                    .Map(target => target.ParamIsNatModifiable, source => source.NatureModifiable)
                    .Map(target => target.NumeroAvenant, source => source.Avenant)
                    .Map(target => target.TypeAlimentation, source => source.TypeAlimentation.Code.AsEnum<Albingia.Kheops.OP.Domain.Referentiel.AlimentationValue>())
                    .Map(target => target.Nature, source => source.Nature.Code.AsEnum<Albingia.Kheops.OP.Domain.Referentiel.NatureValue>())
                    .Map(target => target.NatureRetenue, source => source.NatureRetenue == null ? Albingia.Kheops.OP.Domain.Referentiel.NatureValue.None : source.NatureRetenue.Code.AsEnum<Albingia.Kheops.OP.Domain.Referentiel.NatureValue>())
                    .Map(target => target.Caractere, source => source.Caractere.Code.AsEnum<Albingia.Kheops.OP.Domain.Model.CaractereSelection>())
                    .Map(target => target.DesignationGarantie, source => source.Libelle)
                    .Map(target => target.Portees, source => source.Portees.ObjetsRisque)
                    .Map(target => target.ActionsPortees, source => new Dictionary<string, string>());
            }
        }

        private static void MapInventaireGarantie(bool twoWay = false)
        {
            TypeAdapterConfig<PersonneDesigneeIndispoTournage, Models.Inventaires.PersonneDesigneeIndispoTournage>
                .NewConfig()
                .Map(target => target.DateNaissance, source => source.DateNaissance.GetValueOrDefault())
                .Map(target => target.CodeExtension, source => source.Extention.Code);

            TypeAdapterConfig<PersonneDesigneeIndispo, Models.Inventaires.PersonneDesigneeIndispo>
                .NewConfig()
                .Map(target => target.DateNaissance, source => source.DateNaissance.GetValueOrDefault())
                .Map(target => target.CodeExtension, source => source.Extention.Code);

            TypeAdapterConfig<InventaireItem, Models.Inventaires.InventoryItem>
                .NewConfig()
                .Map(target => target.LineNumber, source => source.NumeroLigne)
                .Map(target => target.ItemId, source => source.Id)
                .Include<PersonneDesigneeIndispoTournage, Models.Inventaires.PersonneDesigneeIndispoTournage>()
                .Include<PersonneDesigneeIndispo, Models.Inventaires.PersonneDesigneeIndispo>();

            TypeAdapterConfig<InventaireDto, Models.Inventaires.Inventaire>
                .NewConfig()
                .Map(target => target.Infos, source => source.Items)
                .Map(target => target.Description, source => source.Designation)
                .Map(target => target.NumeroType, source => source.TypeInventaire.Numero)
                .Map(target => target.CodeType, source => source.TypeInventaire.Code)
                .Map(target => target.LabelType, source => source.TypeInventaire.Description)
                .Map(target => target.ActiverReport, source => source.ReportvaleurObjet.GetValueOrDefault())
                .Map(
                    target => target.TypeTaxe,
                    source => source.IsHTnotTTC == true ?
                        ModeTaxation.HorsTaxes.AsString()
                        : source.IsHTnotTTC.HasValue ? ModeTaxation.ToutesTaxesComprises.AsString() : string.Empty)
                .Map(target => target.TypeValeur, source => source.Typedevaleur.Code)
                .Map(target => target.UniteValeur, source => source.Valeurs.Unite.Code)
                .Map(target => target.Valeur, source => source.Valeurs.ValeurActualise);

            if (twoWay)
            {
                TypeAdapterConfig<Models.Inventaires.InventoryItem, InventaireItem>
                    .NewConfig()
                    .Map(target => target.NumeroLigne, source => source.LineNumber)
                    .Map(target => target.Id, source => source.ItemId)
                    .Include<Models.Inventaires.PersonneDesigneeIndispoTournage, PersonneDesigneeIndispoTournage>()
                    .Include<Models.Inventaires.PersonneDesigneeIndispo, PersonneDesigneeIndispo>();

                TypeAdapterConfig<Models.Inventaires.PersonneDesigneeIndispoTournage, PersonneDesigneeIndispoTournage>
                    .NewConfig()
                    .Map(target => target.DateNaissance, source => source.DateNaissance == DateTime.MinValue ? default(DateTime?) : source.DateNaissance)
                    .Map(target => target.Extention, source => new Albingia.Kheops.OP.Domain.Referentiel.Indisponibilite() { Code = source.CodeExtension });

                TypeAdapterConfig<Models.Inventaires.PersonneDesigneeIndispo, PersonneDesigneeIndispo>
                    .NewConfig()
                    .Map(target => target.DateNaissance, source => source.DateNaissance == DateTime.MinValue ? default(DateTime?) : source.DateNaissance)
                    .Map(target => target.Extention, source => new Albingia.Kheops.OP.Domain.Referentiel.Indisponibilite() { Code = source.CodeExtension });

                TypeAdapterConfig<Models.Inventaires.Inventaire, InventaireDto>
                  .NewConfig()
                  .Map(target => target.Items, source => source.Infos)
                  .Map(target => target.Designation, source => source.Description)
                  .Map(target => target.TypeInventaire, source => new TypeInventaireDto
                  {
                      Numero = source.NumeroType,
                      Code = source.CodeType,
                      Description = source.LabelType
                  })
                  .Map(target => target.ReportvaleurObjet, source => source.ActiverReport)
                  .Map(target => target.Valeurs, source => new ValeursUnite
                  {
                      Unite = new Albingia.Kheops.OP.Domain.Referentiel.Unite { Code = source.UniteValeur },
                      ValeurActualise = source.Valeur,
                      ValeurOrigine = source.Valeur,
                      ValeurTravail = source.Valeur
                  })
                  .Map(target => target.Typedevaleur, source => new Albingia.Kheops.OP.Domain.Referentiel.TypeValeurRisque { Code = source.TypeValeur })
                  .Map(
                        target => target.IsHTnotTTC,
                        source => string.IsNullOrWhiteSpace(source.TypeTaxe) ? null
                            : (bool?)(source.TypeTaxe == ModeTaxation.HorsTaxes.AsString()));
            }
        }
    }
}
