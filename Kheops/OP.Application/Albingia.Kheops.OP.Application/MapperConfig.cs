using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Garantie;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Regularisation;
using Albingia.Kheops.OP.Domain.Risque;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using WSDTO = OP.WSAS400.DTO;

namespace Albingia.Kheops.OP.Application {
    public class ServiceMapper
    {
        public static void Init() {
            var Mapper = TypeAdapterConfig.GlobalSettings;

            TypeAdapterConfig<Domain.Affaire.Affaire, AffaireDto>
                .NewConfig()
                .Map(target => target.Valeur, source => source.ValeurIndiceActualisee)
                .Map(target => target.Nature, source => source.NatureContrat)
                .Map(target => target.DateFin, source => source.DateFinCalculee)
                .Map(target => target.DateMajTraitement, source => source.DateSituation)
                .Map(target => target.MotifRegularisation, source => source.Regularisation == null ? null : source.Regularisation.Motif)
                .Map(target => target.ModeRegularisation, source => source.Regularisation == null ? null : source.Regularisation.Mode)
                .Map(target => target.CodeInterlocuteur, source => source.Interlocuteur == null || source.Interlocuteur.Interlocuteur == null ? string.Empty : source.Interlocuteur.Interlocuteur.Code.ToString())
                .Map(target => target.MontantReference, source => source.MontantReference1 != 0 ? source.MontantReference1 : source.MontantReference2)
                .Map(target => target.TarifAffaireLCI, source => source.TarifAffaireLCI != null ? new TarifAffaireDto {
                    ValeurActualisee = source.TarifAffaireLCI.ValeurActualisee,
                    Unite = source.TarifAffaireLCI.Unite == null ? string.Empty : source.TarifAffaireLCI.Unite.Code
                } : null);

            TypeAdapterConfig<AffaireDto, Domain.Affaire.Affaire>
                .NewConfig()
                .Map(target => target.ValeurIndiceActualisee, source => source.Valeur)
                .Map(target => target.NatureContrat, source => source.Nature)
                .Map(target => target.DateSituation, source => source.DateMajTraitement)
                .Map(target => target.Regularisation, source => source.MotifRegularisation == null ? null : new Domain.Regularisation.Regularisation {
                    Motif = new MotifRegularisation { Code = source.MotifRegularisation.Code },
                    Mode = new ModeRegularisation { Code = source.ModeRegularisation.Code }
                })
                .Map(target => target.Interlocuteur, source => source.CodeInterlocuteur.ParseInt(0).Value < 1 ? null : new Intervenant {
                    Interlocuteur = new Interlocuteur { Code = source.CodeInterlocuteur.ParseInt(0).Value }
                })
                .Map(target => target.TarifAffaireLCI, source => source.TarifAffaireLCI != null ? new Domain.Affaire.TarifAffaire {
                    Unite = source.TarifAffaireLCI.Unite.IsEmptyOrNull(false) ? null : new Unite { Code = source.TarifAffaireLCI.Unite },
                    ValeurActualisee = source.TarifAffaireLCI.ValeurActualisee,
                    ValeurOrigine = source.TarifAffaireLCI.ValeurActualisee
                } : null);

            TypeAdapterConfig<Domain.Affaire.Affaire, Domain.Affaire.AffaireId>.NewConfig();
            TypeAdapterConfig<Domain.Affaire.AffaireId, Domain.Affaire.Affaire>.NewConfig();

            TypeAdapterConfig<Domain.Affaire.AffaireId, AffaireDto>.NewConfig();
            TypeAdapterConfig<AffaireDto, Domain.Affaire.AffaireId>.NewConfig();
            TypeAdapterConfig<ALBINGIA.Framework.Common.Folder, Domain.Affaire.AffaireId>
                .NewConfig()
                .Map(target => target.CodeAffaire, source => source.CodeOffre)
                .Map(target => target.NumeroAliment, source => source.Version)
                .Map(target => target.TypeAffaire, source => source.Type.ParseCode<Domain.Affaire.AffaireType>());
            TypeAdapterConfig<Domain.Affaire.AffaireId, ALBINGIA.Framework.Common.Folder>
                .NewConfig()
                .Map(target => target.CodeOffre, source => source.CodeAffaire)
                .Map(target => target.Version, source => source.NumeroAliment)
                .Map(target => target.NumeroAvenant, source => source.NumeroAvenant.GetValueOrDefault())
                .Map(target => target.Type, source => source.TypeAffaire.AsCode(null, true));

            TypeAdapterConfig<Domain.Affaire.Affaire, RelanceDto>
                .NewConfig()
                .Map(target => target.CodeOffre, source => int.Parse(source.CodeAffaire))
                .Map(target => target.Version, source => source.NumeroAliment)
                .Map(target => target.IsAttenteDocCourtier, source => source.IsAttenteDocumentsCourtier.GetValueOrDefault());

            TypeAdapterConfig<RelanceDto, Domain.Affaire.AffaireId>
                .NewConfig()
                .Map(target => target.CodeAffaire, source => source.CodeOffre.ToString())
                .Map(target => target.NumeroAliment, source => source.Version)
                .Map(target => target.NumeroAvenant, source => default(int?))
                .Map(target => target.TypeAffaire, source => Domain.Affaire.AffaireType.Offre);

            TypeAdapterConfig<Risque, Risque>.NewConfig().PreserveReference(true); 
            TypeAdapterConfig<List<Formule>, List<Formule>>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<Formule, Formule>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<Option, Option>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<OptionVolet, OptionVolet>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<OptionBloc, OptionBloc>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<Garantie, Garantie>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<TarifGarantie, TarifGarantie>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<ExpressionComplexeFranchise, ExpressionComplexeFranchise>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<ExpressionComplexeLCI, ExpressionComplexeLCI>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<ExpressionComplexeDetailFranchise, ExpressionComplexeDetailFranchise>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<ExpressionComplexeDetailLCI, ExpressionComplexeDetailLCI>.NewConfig().PreserveReference(true);

            TypeAdapterConfig<PeriodeGarantie, PeriodeGarantieDto>.NewConfig();
            TypeAdapterConfig<MouvementGarantie, global::OP.WSAS400.DTO.Regularisation.LigneMouvementDto>.NewConfig()
                .Map(target => target.MouvementPeriodeDeb, source => source.DateDebut.ToIntYMD())
                .Map(target => target.MouvementPeriodeFin, source => source.DateFin.ToIntYMD())
                .Map(target => target.Taux, source => System.Convert.ToDouble(source.TauxBase))
                .Map(target => target.Unite, source => source.UniteTauxBase.AsCode(null, true));

            TypeAdapterConfig<Domain.Prime, PrimeDto>
                .NewConfig()
                .Map(target => target.Numero, source => source.Id.Numero)
                .Map(target => target.Montant, source => source.MontantHT);

            TypeAdapterConfig<Domain.Sinistre, SinistreDto>
                .NewConfig()
                .Map(target => target.Affaire, source => source.Affaire)
                .Map(target => target.Numero, source => source.Id.Numero)
                .Map(target => target.DateSurvenance, source => source.Id.DateSurvenance)
                .Map(target => target.CodeSousBranche, source => (source.Id.SousBranche == null ? null : source.Id.SousBranche.Code) ?? string.Empty)
                .Map(target => target.Situation, source => (source.Situation == null ? null : source.Situation.LibelleLong) ?? string.Empty);

            TypeAdapterConfig<Objet, ObjetDto>
                .NewConfig()
                .Map(target => target.Code, source => source.Id.NumObjet)
                .Map(target => target.NumRisque, source => source.Id.NumRisque)
                .Map(target => target.AffaireId, source => source.Id.AffaireId);

            TypeAdapterConfig<Risque, RisqueDto>.NewConfig();

            TypeAdapterConfig<OP.Domain.Inventaire.InventaireItem, OP.Domain.Inventaire.InventaireItem>
                .NewConfig()
                .MapWith(obj => obj);

            TypeAdapterConfig<PorteeGarantie, PorteeGarantieDto>
                .NewConfig()
                .Map(target => target.Valeurs, source => source.ValeursPrime);

            TypeAdapterConfig<OP.Domain.Parametrage.Inventaire.TypeInventaire, TypeInventaireDto>
                 .NewConfig()
                 .Map(target => target.Code, source => source.CodeInventaire)
                 .Map(target => target.Numero, source => source.TypeItem);

            TypeAdapterConfig<Garantie, GarantieDto>
                .NewConfig()
                .Map(target => target.IsFlagModifie, source => source.IsFlagModifie.GetValueOrDefault())
                .Map(target => target.NumeroAvenant, source => source.NumeroAvenant)
                .Map(target => target.AvenantMAJ, source => source.NumeroAvenantModif)
                .Map(target => target.AvenantInitial, source => source.NumeroAvenantCreation)
                .Map(target => target.Sequence, source => source.ParamGarantie.Sequence)
                .Map(target => target.Portees, source => source.Portees)
                .Map(target => target.DateSortie, source => source.DateSortieEffective)
                .Map(target => target.TypeInventaire, source => source.ParamGarantie.ParamGarantie.TypeInventaire)
                .Map(target => target.PrimeProvisionnelle, source => source.Tarif == null ? 0 : source.Tarif.PrimeProvisionnelle)
                .Map(target => target.Prime, source => source.Tarif == null ? 0 : source.Tarif.PrimeValeur.ValeurActualise);

            MapDetailsGarantie(false);

            TypeAdapterConfig<OptionDetail, OptionsDetailDto>
                .NewConfig()
                .Include<OptionBloc, OptionsDetailBlocDto>()
                .Include<OptionVolet, OptionsDetailVoletDto>()
                .PreserveReference(true);

            TypeAdapterConfig<ExpressionComplexeBase, ExpressionComplexeBase>
                .NewConfig()
                .Include<ExpressionComplexeLCI, ExpressionComplexeLCI>()
                .Include<ExpressionComplexeFranchise, ExpressionComplexeFranchise>()
                .PreserveReference(true);
            TypeAdapterConfig<ExpressionComplexeDetailBase, ExpressionComplexeDetailBase>
                .NewConfig()
                .Include<ExpressionComplexeDetailLCI, ExpressionComplexeDetailLCI>()
                .Include<ExpressionComplexeDetailFranchise, ExpressionComplexeDetailFranchise>()
                .PreserveReference(true);

            TypeAdapterConfig<Option, OptionDto>
                .NewConfig()
                .Map(target => target.DateAvenantModif, source => source.DateAvenant)
                .Map(target => target.NumeroFormule, source => source.Formule == null ? 0 : source.Formule.FormuleNumber)
                .Map(target => target.AffaireId, source => source.Formule == null ? null : source.Formule.AffaireId);

            TypeAdapterConfig<OptionDto, Option>
                .NewConfig()
                .Map(target => target.DateAvenant, source => source.DateAvenantModif);

            TypeAdapterConfig<ParamExpressionComplexeBase, ParamExpressionComplexeBase>
               .NewConfig()
               .Include<ParamExpressionComplexeLCI, ParamExpressionComplexeLCI>()
               .Include<ParamExpressionComplexeFranchise, ParamExpressionComplexeFranchise>();
            TypeAdapterConfig<ParamExpressionComplexeDetailBase, ParamExpressionComplexeDetailBase>
                .NewConfig()
                .Include<ParamExpressionComplexeDetailLCI, ParamExpressionComplexeDetailLCI>()
                .Include<ParamExpressionComplexeDetailFranchise, ParamExpressionComplexeDetailFranchise>();

            TypeAdapterConfig<BaseDeCalcul, BaseDeCalcul>.NewConfig()
                .Include<BaseFranchise, BaseFranchise>()
                .Include<BasePrime, BasePrime>()
                .Include<BaseCapitaux, BaseCapitaux>()
                .Include<BaseLCI, BaseLCI>();

            TypeAdapterConfig<SelectionRisqueRegularisationDto, SelectionRegularisation>
                .NewConfig()
                .Map(target => target.Perimetre, source => source.Perimetre.ParseCode<PerimetreSelectionRegul>())
                .Map(target => target.AffaireId, source => source.Risque.AffaireId)
                .Map(target => target.DateDebut, source => source.Risque.DateDebutImplicite.Value)
                .Map(target => target.DateFin, source => source.Risque.DateFinImplicite)
                .Map(target => target.NumeroRisque, source => source.Risque.Numero);

            TypeAdapterConfig<SelectionObjetRegularisationDto, SelectionRegularisation>
                .NewConfig()
                .Map(target => target.Perimetre, source => source.Perimetre.ParseCode<PerimetreSelectionRegul>())
                .Map(target => target.AffaireId, source => source.Objet.AffaireId)
                .Map(target => target.DateDebut, source => source.Objet.DateDebutImplicite.Value)
                .Map(target => target.DateFin, source => source.Objet.DateFinImplicite)
                .Map(target => target.NumeroRisque, source => source.Objet.NumRisque)
                .Map(target => target.NumeroObjet, source => source.Objet.Code);
            
            TypeAdapterConfig<RegularisationDto, Regularisation>.NewConfig();

            TypeAdapterConfig<WSDTO.Common.IdContratDto, Domain.Affaire.AffaireId>
                .NewConfig()
                .Map(target => target.CodeAffaire, source => source.CodeOffre)
                .Map(target => target.NumeroAliment, source => source.Version)
                .Map(target => target.TypeAffaire, source => source.Type.ParseCode<Domain.Affaire.AffaireType>());

            TypeAdapterConfig<GareatState, GareatStateDto>
                .NewConfig()
                .Map(target => target.CodeTranche, source => source.TrancheGareat.Code)
                .Map(target => target.TauxTranche, source => source.TrancheGareat.Rate)
                .Map(target => target.TauxCommissions, source => source.TrancheGareat.RateCommissions)
                .Map(target => target.TauxRetenu, source => source.TrancheGareat.ActualRate)
                .Map(target => target.TauxFraisGeneraux, source => source.TrancheGareat.RateFraisGeneraux)
                .Map(target => target.CodeRegimeTaxe, source => source.RegimeTaxe != null ? source.RegimeTaxe.Code : string.Empty);

            TypeAdapterConfig<GareatStateDto, GareatState>
                .NewConfig()
                .Map(target => target.RegimeTaxe, source => source.CodeRegimeTaxe.IsEmptyOrNull(false) ? null : new RegimeTaxe { Code = source.CodeRegimeTaxe })
                .Map(target => target.TrancheGareat, source => new TrancheGareat {
                    Code = source.CodeTranche,
                    Rate = source.TauxTranche,
                    RateFraisGeneraux = source.TauxFraisGeneraux,
                    RateCommissions = source.TauxCommissions
                });

            TypeAdapterConfig<Domain.Transverse.Montant, PrimeGarantieGareatDto>
                .NewConfig()
                .Map(target => target.MontantHorsTaxe, source => source.ValeurHorsTaxe)
                .Map(target => target.MontantTaxe, source => source.Taxe)
                .Map(target => target.MontantTotal, source => source.Valeur);

            TypeAdapterConfig<ValeursUniteDto, ValeursOptionsTarif>
                .NewConfig()
                .Map(target => target.Unite, source => source.CodeUnite.IsEmptyOrNull(false) ? null : new Unite { Code = source.CodeUnite })
                .Map(target => target.Base, source => source.CodeBase.IsEmptyOrNull(false) ? null : new BaseDeCalcul { Code = source.CodeBase })
                .Map(target => target.ExpressionComplexe, source => source.IdCPX > 0 ? new ExpressionComplexeBase { Id = source.IdCPX.Value } : null);

            TypeAdapterConfig<Domain.InfosSpecifiques.LigneModeleIS, LigneModeleISDto>.NewConfig()
                .Map(target => target.ParentCode, source => source.Parent != null ? source.Parent.Code : string.Empty)
                .Map(target => target.ListeUnites, source => source.HasUnite
                    ? source.ListeUnites.ToDictionary(x => x.code, x => x.libelle)
                    : source.Propriete == null ? null : source.Propriete.HasUnite ? source.Propriete.ListeUnites.ToDictionary(x => x.code, x => x.libelle) : null);

            TypeAdapterConfig<LigneModeleISDto, Domain.InfosSpecifiques.LigneModeleIS>.NewConfig()
                .Map(target => target.Parent, source => null as Domain.InfosSpecifiques.LigneModeleIS)
                .Ignore(nameof(LigneModeleISDto.ListeUnites));

            TypeAdapterConfig<Domain.InfosSpecifiques.InformationSpecifique, InformationSpecifiqueDto>.NewConfig()
                .Ignore(nameof(InformationSpecifiqueDto.Valeur));

            TypeAdapterConfig <InfoSpeValeurDto, Domain.InfosSpecifiques.ValeurInformationSpecifique>.NewConfig()
                .Ignore(nameof(InfoSpeValeurDto.Unite));

            TypeAdapterConfig<Domain.InfosSpecifiques.ProprieteIS, ProprieteISDto>.NewConfig()
                .Map(target => target.ListeUnites, source => source.ListeUnites == null || !source.ListeUnites.Any() ? null : source.ListeUnites.ToDictionary(u => u.code, u => u.libelle));

            TypeAdapterConfig<ProprieteISDto, Domain.InfosSpecifiques.ProprieteIS>.NewConfig()
                .Ignore(nameof(ProprieteISDto.ListeUnites));
        }

        private static void MapDetailsGarantie(bool twoWay = false) {
            TypeAdapterConfig<Garantie, GarantieDetailsDto>
                .NewConfig()
                .Map(target => target.CodeAlimentationAssiette, source => source.TypeAlimentation.AsString())
                .Map(target => target.CodeCaractere, source => source.Caractere.AsString())
                .Map(target => target.CodeDureeUnite, source => source.DureeUnite == null ? string.Empty : source.DureeUnite.Code)
                .Map(target => target.CodeNature, source => source.Nature.AsString())
                .Map(target => target.CodeTaxe, source => source.Taxe == null ? string.Empty : source.Taxe.Code)
                .Map(target => target.CodeTypeApplication, source => source.PeriodeApplication == null ? string.Empty : source.PeriodeApplication.Code)
                .Map(target => target.CodeTypeEmission, source => source.TypeEmission == null ? string.Empty : source.TypeEmission.Code)
                .Map(target => target.DateDebutStd, source => source.DatestandardDebut)
                .Map(target => target.DateFinStd, source => source.DatestandardFin)
                .Map(target => target.DateFin, source => source.DateFinDeGarantie)
                .Map(target => target.HasCATNAT, source => source.IsApplicationCATNAT.GetValueOrDefault())
                .Map(target => target.IsGarantieIndexee, source => source.IsIndexe.GetValueOrDefault())
                .Map(target => target.Code, source => source.CodeGarantie)
                .Map(target => target.Libelle, source => source.DesignationGarantie)
                .Map(target => target.InclusMontant, source => source.IsAlimMontantReference.GetValueOrDefault())
                .Map(target => target.Regularisable, source => source.ParamGarantie.ParamGarantie.IsRegularisable.GetValueOrDefault())
                .Map(target => target.CodeGrilleRegul, source => source.ParamGarantie.ParamGarantie.GrilleRegul == null ? null : source.ParamGarantie.ParamGarantie.GrilleRegul.Code)
                .Map(target => target.LabelGrilleRegul, source => source.ParamGarantie.ParamGarantie.GrilleRegul == null ? null : source.ParamGarantie.ParamGarantie.GrilleRegul.Libelle)
                .Map(target => target.Definition, source => source.DefinitionGarantie)
                .Map(target => target.Sequence, source => source.ParamGarantie.Sequence);

            if (twoWay) {
                TypeAdapterConfig<GarantieDetailsDto, Garantie>
                  .NewConfig()
                  .Map(target => target.TypeAlimentation, source => source.CodeAlimentationAssiette.AsEnum<AlimentationValue>())
                  .Map(target => target.Caractere, source => source.CodeCaractere.AsEnum<OP.Domain.Model.CaractereSelection>())
                  .Map(target => target.DureeUnite, source => new UniteDuree { Code = source.CodeDureeUnite ?? string.Empty })
                  .Map(target => target.Nature, source => source.CodeNature.AsEnum<NatureValue>())
                  .Map(target => target.Taxe, source => new Taxe { Code = source.CodeTaxe ?? string.Empty })
                  .Map(target => target.PeriodeApplication, source => new PeriodeApplication { Code = source.CodeTypeApplication ?? string.Empty })
                  .Map(target => target.TypeEmission, source => new TypeEmission { Code = source.CodeTypeEmission ?? string.Empty })
                  .Map(target => target.DatestandardDebut, source => source.DateDebutStd)
                  .Map(target => target.DatestandardFin, source => source.DateFinStd)
                  .Map(target => target.DateFinDeGarantie, source => source.DateFin)
                  .Map(target => target.IsApplicationCATNAT, source => source.HasCATNAT)
                  .Map(target => target.IsIndexe, source => source.IsGarantieIndexee)
                  .Map(target => target.CodeGarantie, source => source.Code)
                  .Map(target => target.DesignationGarantie, source => source.Libelle)
                  .Map(target => target.IsAlimMontantReference, source => source.InclusMontant);
            }
        }
    }
}