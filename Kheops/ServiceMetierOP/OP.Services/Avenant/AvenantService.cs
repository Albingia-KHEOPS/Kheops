using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using Mapster;
using ALBINGIA.Framework.Common.Tools;
using OP.DataAccess;
using OP.Services.Historization;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.PGM;
using OPServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Activation;

namespace OP.Services.BLServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AvenantService : IAvenant
    {
        private readonly IDbConnection connection;
        private readonly RegularisationRepository regularisationRepository;
        private readonly RemiseEnVigueurRepository remiseEnVigueurRepository;
        private readonly CibleService cibleService;
        private readonly AffaireNouvelleRepository repository;
        private readonly IAffairePort affaireService;
        private readonly IRegularisationPort regularisationService;

        public AvenantService(IDbConnection connection, AffaireNouvelleRepository repository, RegularisationRepository regularisationRepository, RemiseEnVigueurRepository remiseEnVigueurRepository, CibleService cibleService, IAffairePort affaireService, IRegularisationPort regularisationService)
        {
            this.connection = connection;
            this.affaireService = affaireService;
            this.repository = repository;
            this.regularisationRepository = regularisationRepository;
            this.remiseEnVigueurRepository = remiseEnVigueurRepository;
            this.cibleService = cibleService;
            this.affaireService = affaireService;
            this.regularisationService = regularisationService;
        }

        internal static IAvenantDto DefineAvenant(string type, params IAvenantDto[] models)
        {
            switch (type)
            {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                    return models.FirstOrDefault(m => m?.GetType() == typeof(AvenantModificationDto));
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                    var avnrg = models.FirstOrDefault(m => m?.GetType() == typeof(AvenantModificationDto));
                    if (avnrg == null)
                    {
                        avnrg = models.FirstOrDefault(m => m?.GetType() == typeof(AvenantRegularisationDto));
                    }
                    return avnrg;
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    return models.FirstOrDefault(m => m?.GetType() == typeof(AvenantRegularisationDto));
                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    return models.FirstOrDefault(m => m?.GetType() == typeof(AvenantRemiseEnVigueurDto));
                case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                    return models.FirstOrDefault(m => m?.GetType() == typeof(AvenantResiliationDto));
                default:
                    return null;
            }
        }

        public string[] CreationAvenantModif(string ipb, int alx, string date, string description, string observations)
        {
            try
            {
                var folder = new Folder(ipb.ToIPB(), alx, 'P');
                var affaire = this.affaireService.GetAffaire(new AffaireId
                {
                    CodeAffaire = ipb.ToIPB(),
                    NumeroAliment = alx,
                    TypeAffaire = AffaireType.Contrat
                });
                if (affaire is null)
                {
                    return new[] { $"{ipb} - {alx} : contrat non trouvé" };
                }
                if (!affaire.IsKheops)
                {
                    return new[] { $"{ipb} - {alx} : contrat non Kheops" };
                }
                if (affaire.Etat != EtatAffaire.Validee.ToString())
                {
                    return new[] { $"{ipb} - {alx} : le contrat n'est pas validé" };
                }
                //else if (affaire.Situation != SituationAffaire.EnCours)
                //{
                //    return new[] { $"{ipb} - {alx} : le contrat n'est plus en-cours" };
                //}
                var modelAvenant = new AvenantModificationDto
                {
                    NumInterneAvt = affaire.NumeroAvenant + 1,
                    NumAvt = affaire.NumeroAvenant + 1,
                    DateEffetAvt = DateTime.TryParse(date, out var d) ? d : default(DateTime?),
                    TypeAvt = AlbConstantesMetiers.TYPE_AVENANT_MODIF,
                    ObservationsAvt = observations.IsEmptyOrNull() ? affaire.Observations : observations,
                    DescriptionAvt = description.IsEmptyOrNull() ? affaire.DescriptionAvenant : description,
                    MotifAvt = "M1"
                };
                if (modelAvenant.DateEffetAvt is null)
                {
                    return new[] { $"'{date}' : date d'avenant invalide (Format attendu YYYY-MM-DDTHH:mm:ss)" };
                }
                else
                {
                    modelAvenant.HeureEffetAvt = modelAvenant.DateEffetAvt.Value.TimeOfDay;
                    if (modelAvenant.DateEffetAvt < affaire.DateEffet)
                    {
                        return new[] { $"{modelAvenant.DateEffetAvt} : date d'avenant invalide (Date d'effet : {affaire.DateEffet})" };
                    }
                    if (modelAvenant.DateEffetAvt > affaire.DateFinCalculee)
                    {
                        return new[] { $"{modelAvenant.DateEffetAvt} : date d'avenant invalide (Date de fin : {affaire.DateFinCalculee})" };
                    }
                    //if (modelAvenant.DateEffetAvt < affaire.DateEffetAvenant)
                    //{
                    //    return $"{date} : date d'avenant invalide (Date du dernier avenant : {affaire.DateEffetAvenant})";
                    //}
                    if (modelAvenant.DateEffetAvt > affaire.ProchaineEcheance?.Date)
                    {
                        return new[] { $"{modelAvenant.DateEffetAvt} : date d'avenant invalide (Date de prochaine échéance : {affaire.ProchaineEcheance.Date})" };
                    }
                }
                string user = string.Empty;
                try
                {
                    user = WCFHelper.GetFromHeader("UserAS400");
                }
                catch
                {
                    user = "AS400";
                }
                return CreateOrUpdate(folder, AlbConstantesMetiers.TYPE_AVENANT_MODIF, AvenantAccess.CREATE.ToString(), modelAvenant, null, null, null, user ?? "AS400");
            }
            catch (Exception ex)
            {
                return new[] { $"Une erreur inattendue s'est produite : {ex}" };
            }
        }

        public string[] CreateOrUpdate(Folder folder, string typeAvt, string modeAvt,
            AvenantModificationDto modeleAvtModif, AvenantResiliationDto modeleAvtResil, AvenantRemiseEnVigueurDto modeleRemiseVig, AvenantRegularisationDto regularisationDto,
            string user)
        {
            var returnMessage = string.Empty;

            IAvenantDto avenant = DefineAvenant(typeAvt, modeleAvtModif, modeleAvtResil, modeleRemiseVig, regularisationDto);
            folder.NumeroAvenant = (int)avenant.Numero;
            var errors = CheckData(avenant, folder);
            if (errors.Any())
            {
                return errors.ToArray();
            }
            try
            {
                var histoService = new HistorizationService();
                histoService.Archive(this.connection, new HistorizationContext(avenant)
                {
                    Folder = folder,
                    User = user,
                    Mode = modeAvt.IsIn(AvenantAccess.CREATE.ToString(), AvenantAccess.UPDATE.ToString()) ? modeAvt[0] : default
                });
                switch (typeAvt)
                {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        this.cibleService.UpdateSousBranche(folder, user);
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        this.cibleService.UpdateSousBranche(folder, user);
                        this.regularisationRepository.UpdateAvenantNumDate(folder, ((IAvenantDto)modeleAvtModif ?? (IAvenantDto)regularisationDto).Date);
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                        if (modeAvt == AvenantAccess.CREATE.ToString())
                        {
                            RemiseEnVigueur(folder, modeleRemiseVig, user);
                        }
                        else
                        {
                            ModifyRemiseEnVigueur(folder, modeleRemiseVig.TypeGestion);
                        }
                        AffaireNouvelleRepository.ComputeEcheance(folder.CodeOffre.ToIPB(), folder.Version.ToString(), folder.Type, user, "#**#", typeAvt, this.connection);
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        break;
                    default:
                        return new[] { "Erreur : informations erronées" };
                }
            }
            catch (HistorizationException histoEx)
            {
                return histoEx.Errors.ToArray();
            }

            return new string[0];
        }

        public void RemiseEnVigueur(Folder folder, AvenantRemiseEnVigueurDto modeleRemiseVig, string user)
        {
            var dto = this.remiseEnVigueurRepository.InitRemiseEnVigueur(new RemiseEnVigueurParams
            {
                ActeGestion = AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR,
                CodeContrat = folder.CodeOffre.ToIPB(),
                Type = folder.Type,
                Version = folder.Version,
                User = user
            });
            if (int.TryParse(dto.Result, out int i) && i == 1 || dto.Result == "S")
            {
                this.remiseEnVigueurRepository.CreateRemiveEnVigeur(folder, dto, modeleRemiseVig.TypeGestion);
            }
        }

        public void ModifyRemiseEnVigueur(Folder folder, string typeGestion)
        {
            this.remiseEnVigueurRepository.ModifyRemiseEnVigeur(folder, typeGestion);
        }

        public bool IsReadonlyRemiseEnVigueur(Folder folder)
        {
            return this.remiseEnVigueurRepository.GetTypeGestion(folder) == "V";
        }

        public int GetCurrentAvn(Folder folder)
        {
            var a = this.affaireService.GetAffaire(new Albingia.Kheops.OP.Domain.Affaire.AffaireId
            {
                CodeAffaire = folder.CodeOffre,
                NumeroAliment = folder.Version,
                TypeAffaire = AffaireType.Contrat
            });
            return a.NumeroAvenant;
        }

        private IEnumerable<string> CheckData(IAvenantDto avenant, Folder folder)
        {
            switch (avenant)
            {
                case AvenantModificationDto avenantModification:
                    var a = folder.Adapt<AffaireId>();
                    var affaire = this.affaireService.GetAffaire(a);
                    AffaireDto histo = null;
                    RegularisationDto regularisation = null;
                    if (avenant.NumInterne == affaire.NumeroAvenant)
                    {
                        var ha = a.MakeCopy(true);
                        ha.NumeroAvenant--;
                        histo = this.affaireService.GetAffaire(ha);
                    }
                    if (avenant.Type == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                    {
                        regularisation = this.regularisationService.GetDtoByAffaire(a);
                    }
                    return CheckDataAvenant(avenantModification, affaire, histo, regularisation);
                case AvenantRegularisationDto avenantRegularisation:
                    break;
                case null:
                    return new[] { "Erreur : informations erronées" };
            }
            return Enumerable.Empty<string>();
        }

        private static IEnumerable<string> CheckDataAvenant(AvenantModificationDto avenant, AffaireDto affaire, AffaireDto histo, RegularisationDto regularisation)
        {
            if (!avenant.DateEffetAvt.HasValue)
            {
                yield return nameof(avenant.DateEffetAvt);
            }
            if (!avenant.HeureEffetAvt.HasValue)
            {
                yield return nameof(avenant.HeureEffetAvt);
            }
            if (avenant.MotifAvt.IsEmptyOrNull())
            {
                yield return nameof(avenant.MotifAvt);
            }
            if (avenant.NumAvt < avenant.NumInterneAvt)
            {
                yield return nameof(avenant.NumAvt);
            }
            if (avenant.DateEffetAvt.HasValue && avenant.HeureEffetAvt.HasValue)
            {
                var date = avenant.DateEffetAvt.Value + avenant.HeureEffetAvt.Value;
                bool isValidDate = true;
                bool isCreateMode = (avenant.NumInterneAvt - 1) == affaire.NumeroAvenant;
                if (regularisation != null)
                {
                    if (date.Date < regularisation.DateFin.AddDays(1))
                    {
                        // test without hour
                        isValidDate = false;
                        yield return "La date d'effet de l'avenant doit être postérieure à la dernière période de régularisation";
                    }
                }
                else if (date < (histo?.DateEffet ?? affaire.DateEffet)) {
                    isValidDate = false;
                    yield return "La date d'effet de l'avenant ne doit pas être antérieure à la date d'effet du contrat";
                }

                if (affaire.DateFin < date)
                {
                    isValidDate = false;
                    yield return "La date d'avenant doit être comprise dans la période de garanties";
                }

                if (!affaire.Periodicite.Code.IsIn("U", "E"))
                {
                    if (isCreateMode)
                    {
                        // creation
                        if (affaire.ProchaineEcheance?.Date < date)
                        {
                            isValidDate = false;
                            yield return "Incohérence entre la prochaine échéance et la date d'avenant, ";
                            yield return "Echeance";
                        }
                    }
                    else
                    {
                        if (histo.ProchaineEcheance?.Date < date)
                        {
                            isValidDate = false;
                            yield return "Date d'avenant incohérente avec la prochaine échéance en historique";
                            yield return "Echeance";
                        }
                    }
                }

                if (!isValidDate)
                {
                    yield return nameof(avenant.DateEffetAvt);
                    yield return nameof(avenant.HeureEffetAvt);
                }
            }
        }
    }
}
