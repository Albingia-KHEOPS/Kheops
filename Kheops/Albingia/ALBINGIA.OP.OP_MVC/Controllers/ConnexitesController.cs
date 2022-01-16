using Albingia.Kheops.Common;
using Albingia.Kheops.Mvc;
using Albingia.Kheops.Mvc.Models;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Mvc.Controllers;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.Connexites;
using Mapster;
using OP.WSAS400.DTO.Engagement;
using OPServiceContract.IAffaireNouvelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    [ModelLessLoader]
    public class ConnexitesController : BaseController, IModelLessController {
        public Guid TabGuid { get; set; }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetEmptyModel() {
            return CustomResult.JsonNetResult.NewResultToGet(new ModelConnexites());
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetEmptyFusionConnexites() {
            return CustomResult.JsonNetResult.NewResultToGet(new FusionConnexites());
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetEmptyPeriode() {
            return CustomResult.JsonNetResult.NewResultToGet(new PeriodeEngagement());
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetAll(Affaire affair, ModeConsultation? mode) {
            ModelConnexites modelConnexites;
            using (var client = ServiceClientFactory.GetClient<IReferentielPort>()) {
                modelConnexites = BuildModel(affair, client.Channel.GetFamillesReassurances(), mode);
            }
            var guid = Guid.TryParse(affair.TabGuid, out var g) ? g : default;
            modelConnexites.IsReadonly = guid == default || GetState(guid, affair.Adapt<Albingia.Kheops.OP.Domain.Affaire.AffaireId>()).IsIn(ControllerState.PartialEdit, ControllerState.Readonly);
            return CustomResult.JsonNetResult.NewResultToGet(modelConnexites);
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetListConnexites(Folder folder, TypeConnexite? t) {
            ModelConnexites modelConnexites = BuildListModel(folder, t);
            if (t == TypeConnexite.Engagement) {
                return CustomResult.JsonNetResult.NewResultToGet(new { list = modelConnexites.Engagements, familles = modelConnexites.FamillesReassurances });
            }
            else {
                return CustomResult.JsonNetResult.NewResultToGet(new { list = modelConnexites.GetListFormType(t) });
            }
        }

        [HttpPost]
        [ErrorHandler]
        public JsonResult GetModelFusionConnexites(Folder affaire, Folder affaireCible, TypeConnexite? t) {
            ModelConnexites modelConnexites = BuildListModel(affaire, t);
            ModelConnexites modelConnexitesCible = BuildListModel(affaireCible, t);
            var cnxList = modelConnexites.GetListFormType(t);
            return CustomResult.JsonNetResult.NewResultToGet(new FusionConnexites{
                Connexites = cnxList.ToList(),
                ConnexitesExternes = modelConnexitesCible.GetListFormType(t).ToList(),
                Affaire = affaire,
                AffaireCible = affaireCible,
                TypeConnexite = t.Value,
                Observations = cnxList.First().Observation
            });
        }

        [HttpPost]
        [HandleJsonError]
        public void AddContratConnexe(Folder folder, ContratConnexeDto folderConnexe) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.AddConnexite(folder, folderConnexe, AlbSessionHelper.ConnectedUser);
            }
        }

        [HttpPost]
        [ErrorHandler]
        public void RemoveConnexite(Folder folder, ContratConnexeDto folderConnexe) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.RemoveConnexite(folder, folderConnexe);
            }
        }

        [HttpPost]
        [HandleJsonError]
        public void AddFirstPeriodeEngagement(Folder folder, PeriodeEngagement periode) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                client.Channel.AddPeriodeEngagement(new PeriodeConnexiteDto {
                    DateDebut = null,
                    DateFin = null,
                    CodeEngagement = 0,
                    CodeOffre = folder.CodeOffre,
                    Version = folder.Version,
                    Type = folder.Type,
                    Traites = periode.Valeurs.ToDictionary(v => v.CodeTraite, v => v.Montant.GetValueOrDefault()),
                    User = AlbSessionHelper.ConnectedUser,
                    IsEnCours = true,
                    IsInactive = false
                });
            }
        }

        [HttpGet]
        [ErrorHandler]
        public JsonResult GetPeriodesEngagements(int numeroConnexite) {
            List<PeriodeConnexiteDto> periodesDto;
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                periodesDto = client.Channel.GetPeriodesEngagements(numeroConnexite).ToList();
            }
            var periodes = periodesDto.Select(p => p.Adapt<PeriodeEngagement>());
            return CustomResult.JsonNetResult.NewResultToGet(new { periodes, mustUpdate = ValidatePeriodes(periodes).Any() });
        }

        [HttpPost]
        [HandleJsonError]
        public void ModifyPeriodes(Folder folder, IEnumerable<PeriodeEngagement> periodes) {
            if (periodes == null) {
                //throw new ArgumentNullException(nameof(periodes));
                periodes = Enumerable.Empty<PeriodeEngagement>();
            }

            List<ValidationError> errors = ValidatePeriodes(periodes);
            if (errors.Any()) {
                throw new BusinessValidationException(errors);
            }
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                try {
                    client.Channel.ModifyPeriodesEngagements(folder, periodes.Select(x => {
                        var p = x.Adapt<PeriodeConnexiteDto>();
                        p.User = AlbSessionHelper.ConnectedUser;
                        return p;
                    }));
                }
                catch (FaultException<List<ValidationError>> faultEx) {
                    throw new BusinessValidationException(faultEx.Detail, faultEx.Message);
                }
            }
        }

        internal static List<ValidationError> ValidatePeriodes(IEnumerable<PeriodeEngagement> periodes) {
            var errors = new List<ValidationError>();
            var periodList = periodes.OrderBy(p => p.Beginning).ToList();
            var lastp = periodList.LastOrDefault();
            foreach (var p in periodList) {
                if (!p.Beginning.HasValue) {
                    errors.Add(new ValidationError(string.Empty, p.Ordre.ToString(), p.Id.ToString(), nameof(p.Beginning), "La date de début est obligatoire"));
                }
                else if (!p.End.HasValue && !ReferenceEquals(p, lastp)) {
                    errors.Add(new ValidationError(string.Empty, p.Ordre.ToString(), p.Id.ToString(), nameof(p.End), "La date de fin est obligatoire"));
                }
                if (p.Beginning > p.End) {
                    errors.Add(new ValidationError(string.Empty, p.Ordre.ToString(), p.Id.ToString(), nameof(p.Beginning), "La date de début doit être inférieure ou égale à la date de fin"));
                }
            }

            return errors;
        }

        internal static ModelConnexites BuildModel(Affaire affair, IEnumerable<FamilleReassurance> familles, ModeConsultation? mode, TypeConnexite? typeFilter = null) {
            var modelConnexites = new ModelConnexites { Folder = affair, MustFixPeriodes = false };
            List<ContratConnexeDto> contrats;
            var periodes = new List<PeriodeConnexiteDto>();
            string codeBranche;
            string codeSousBranche;
            string codeCible;
            string codeCatgorie;
            IDictionary<Folder, (DateTime? debut, DateTime? fin)> foldersDates;
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var folder = affair.Adapt<Folder>();
                contrats = client.Channel.GetAllConnexites(folder).ToList();
                (codeBranche, codeSousBranche, codeCatgorie, codeCible) = client.Channel.GetCiblage(folder, mode.GetValueOrDefault());
                if (contrats?.Any(c => c.CodeTypeConnexite == TypeConnexite.Engagement.AsCode()) ?? false) {
                    int numero = contrats.First(c => c.CodeTypeConnexite == TypeConnexite.Engagement.AsCode()).NumConnexite;
                    periodes = client.Channel.GetPeriodesEngagements(numero).ToList();
                    foldersDates = client.Channel.GetDatesEffets(contrats.Select(c => new Folder(c.NumContrat, c.VersionContrat, c.TypeContrat[0])));
                    modelConnexites.DateEngagementMin = foldersDates.Values.Min(x => x.debut);
                    modelConnexites.DateEngagementMax = foldersDates.Values.Max(x => x.fin);
                }
            }
            if (periodes.Any()) {
                modelConnexites.PeriodesEngagements.AddRange(periodes.Select(p => p.Adapt<PeriodeEngagement>()));
                modelConnexites.MustFixPeriodes = ValidatePeriodes(modelConnexites.PeriodesEngagements).Any();
            }
            if (contrats?.Any() ?? false) {
                List<TypeConnexite> types = null;
                if (typeFilter.GetValueOrDefault() > 0) {
                    types = new List<TypeConnexite> { typeFilter.Value };
                }
                else {
                    types = Enum.GetValues(typeof(TypeConnexite)).Cast<TypeConnexite>().ToList();
                }
                types.ForEach(t => modelConnexites.SetAppropriateList(t, ModelConnexites.BuildListFrom(affair, contrats, t)));
                if ((modelConnexites.FamillesReassurances?.Any() ?? false) && (familles?.Any() ?? false)) {
                    modelConnexites.FamillesReassurances.ForEach(f => {
                        f.Label = familles.FirstOrDefault(frs => frs.Code == f.Code)?.LibelleLong ?? f.Code;
                    });
                }
            }

            modelConnexites.CodeBranche = codeBranche;
            modelConnexites.CodeSousBranche = codeSousBranche;
            modelConnexites.CodeCible = codeCible;
            
            return modelConnexites;
        }

        internal static ModelConnexites BuildListModel(Folder folder, TypeConnexite? t) {
            List<ContratConnexeDto> contrats;
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                contrats = client.Channel.GetConnexites(folder, t.Value).ToList();
            }

            var modelConnexites = new ModelConnexites { Folder = folder };
            modelConnexites.SetAppropriateList(t.Value, ModelConnexites.BuildListFrom(folder, contrats, t.Value));
            return modelConnexites;
        }
    }
}
