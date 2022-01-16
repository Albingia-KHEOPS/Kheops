using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Mapster;

namespace Albingia.Kheops.OP.Service
{
    public static class MapperConfig
    {
        public static void Init() {
            var Mapper = TypeAdapterConfig.GlobalSettings;
            TypeAdapterConfig<Garantie, GarantieDto>
                .NewConfig()
                .Map(target => target.IsFlagModifie, source => source.IsFlagModifie.GetValueOrDefault())
                .Map(target => target.NumeroAvenant, source => source.NumeroAvenant)
                .Map(target => target.AvenantMAJ, source => source.NumeroAvenantModif)
                .Map(target => target.AvenantInitial, source => source.NumeroAvenantCreation)
                .Map(target => target.Sequence, source => source.ParamGarantie.Sequence);

            TypeAdapterConfig<Garantie, GarantieDetailsDto>
                .NewConfig()
                .Map(target => target.CodeAlimentationAssiette, source => source.TypeAlimentation.AsString())
                .Map(target => target.CodeCaractere, source => source.Caractere.AsString())
                .Map(target => target.CodeDureeUnite, source => source.DureeUnite.Code)
                .Map(target => target.CodeNature, source => source.Nature.AsString())
                .Map(target => target.CodeTaxe, source => source.Taxe.Code)
                .Map(target => target.CodeTypeApplication, source => source.PeriodeApplication.Code)
                .Map(target => target.CodeTypeEmission, source => source.TypeEmission.Code)
                .Map(target => target.DateDebutStd, source => source.DatestandardDebut)
                .Map(target => target.DateFinStd, source => source.DatestandardFin)
                .Map(target => target.HasCATNAT, source => source.IsApplicationCATNAT.GetValueOrDefault())
                .Map(target => target.IsGarantieIndexee, source => source.IsIndexe.GetValueOrDefault())
                .Map(target => target.Code, source => source.CodeGarantie)
                .Map(target => target.Libelle, source => source.DesignationGarantie)
                .Map(target => target.InclusMontant, source => source.IsAlimMontantReference.GetValueOrDefault())
                .Map(target => target.Regularisable, source => source.ParamGarantie.ParamGarantie.IsRegularisable.GetValueOrDefault())
                .Map(target => target.CodeGrilleRegul, source => source.ParamGarantie.ParamGarantie.GrilleRegul.Code)
                .Map(target => target.LabelGrilleRegul, source => source.ParamGarantie.ParamGarantie.GrilleRegul.Libelle)
                .Map(target => target.Definition, source => source.DefinitionGarantie)
                .Map(target => target.Sequence, source => source.ParamGarantie.Sequence);

            TypeAdapterConfig<OptionDetail, OptionsDetailDto>
                .NewConfig()
                .Include<OptionBloc, OptionsDetailBlocDto>()
                .Include<OptionVolet, OptionsDetailVoletDto>();
            TypeAdapterConfig<ExpressionComplexeBase, ExpressionComplexeBase>
                .NewConfig()
                .Include<ExpressionComplexeLCI, ExpressionComplexeLCI>()
                .Include<ExpressionComplexeFranchise, ExpressionComplexeFranchise>();
            TypeAdapterConfig<ExpressionComplexeDetailBase, ExpressionComplexeDetailBase>
                .NewConfig()
                .Include<ExpressionComplexeDetailLCI, ExpressionComplexeDetailLCI>()
                .Include<ExpressionComplexeDetailFranchise, ExpressionComplexeDetailFranchise>();
            TypeAdapterConfig<BaseDeCalcul, BaseDeCalcul>.NewConfig()
                .Include<BaseFranchise, BaseFranchise>()
                .Include<BasePrime, BasePrime>()
                .Include<BaseCapitaux, BaseCapitaux>()
                .Include<BaseLCI, BaseLCI>();
            TypeAdapterConfig<ParamExpressionComplexeBase, ParamExpressionComplexeBase>
                .NewConfig()
                .Include<ParamExpressionComplexeLCI, ParamExpressionComplexeLCI>()
                .Include<ParamExpressionComplexeFranchise, ParamExpressionComplexeFranchise>();
            TypeAdapterConfig<ParamExpressionComplexeDetailBase, ParamExpressionComplexeDetailBase>
                .NewConfig()
                .Include<ParamExpressionComplexeDetailLCI, ParamExpressionComplexeDetailLCI>()
                .Include<ParamExpressionComplexeDetailFranchise, ParamExpressionComplexeDetailFranchise>();

            //Mapper.Register<TarifGarantie, TarifGarantieDto>();
            //Mapper.Register<Garantie, GarantieDto>();
            //Mapper.Register<OptionBloc, OptionsDetailBlocDto>();
            //Mapper.Register<OptionVolet, OptionsDetailVoletDto>();
            //Mapper.Register<Option, OptionDto>();
            //Mapper.Register<Formule, FormuleDto>();
            //Mapper.Register<Affaire, AffaireDto>();
        }
    }
}