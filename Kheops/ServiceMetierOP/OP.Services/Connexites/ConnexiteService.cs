using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using OP.DataAccess;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.Engagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OP.Services.Connexites
{
    public class ConnexiteService
    {
        private readonly IDbConnection connection;
        private readonly ConnexiteRepository repository;
        private readonly AffaireNouvelleRepository affaireRepository;
        private readonly EngagementRepository engagementRepository;
        private readonly ProgramAS400Repository as400Repository;
        private readonly ObservationsRepository observationsRepository;
        private readonly IAffairePort affaireService;

        public ConnexiteService(IDbConnection connection, ConnexiteRepository connexiteRepository, AffaireNouvelleRepository affaireRepository, EngagementRepository engagementRepository, ProgramAS400Repository as400Repository, ObservationsRepository observationsRepository, IAffairePort affaireService)
        {
            this.connection = connection;
            this.repository = connexiteRepository;
            this.engagementRepository = engagementRepository;
            this.as400Repository = as400Repository;
            this.observationsRepository = observationsRepository;
            this.affaireRepository = affaireRepository;
            this.affaireService = affaireService;
        }

        public IEnumerable<ContratConnexeDto> GetConnexites(Folder folder, TypeConnexite? singleType = null)
        {
            List<TypeConnexite> types = singleType.GetValueOrDefault() > 0 ?
                new List<TypeConnexite> { singleType.Value } :
                Enum.GetValues(typeof(TypeConnexite)).Cast<TypeConnexite>().ToList();
            var connexites = new List<ContratConnexeDto>();
            types.ForEach(type =>
            {
                int numero = this.repository.GetNumero(folder, type);
                connexites.AddRange(this.repository.GetContrats(folder, type, numero));
            });

            return connexites;
        }

        public IEnumerable<Folder> GetFolders(TypeConnexite type, int numero)
        {
            return this.repository.GetFolders(type, numero).Select(x => new Folder(x.NumContrat, x.VersionContrat, x.TypeContrat[0])).ToList();
        }

        public void ConnectContrat(Folder folder, ContratConnexeDto contratConnexe, string user)
        {

            var newConnectedC = new Folder(contratConnexe.NumContrat, contratConnexe.VersionContrat,contratConnexe.TypeContrat[0]);
            var affaireC = this.affaireService.GetAffaire(newConnectedC.Adapt<AffaireId>());
            contratConnexe.CodeBranche = affaireC.Branche.Code;
            contratConnexe.CodeSousBranche = affaireC.SousBranche;
            contratConnexe.CodeCible = affaireC.Cible.Code;
            contratConnexe.CodeCategorie = affaireC.Categorie;

            //var affaireC = this.affaireService.GetAffaire(new AffaireId(contratConnexe.NumContrat,));
            var typeConnexite = (TypeConnexite)int.Parse(contratConnexe.CodeTypeConnexite);
            var folders = new { main = folder, connected = new Folder(contratConnexe.NumContrat, contratConnexe.VersionContrat, contratConnexe.TypeContrat[0]) };
            if (folders.main.CodeOffre == folders.connected.CodeOffre && folders.main.Version == folders.connected.Version)
            {
                throw new BusinessValidationException(new ValidationError("Impossible d'effectuer une connexité sur le même contrat"));
            }
            int folderNum = this.repository.GetNumero(folders.main, typeConnexite);
            int otherNum = this.repository.GetNumero(folders.connected, typeConnexite);
            if (folderNum > 0) {
                if (otherNum == folderNum) {
                    throw new BusinessValidationException(new ValidationError("Ce contrat est déjà dans la connexité"));
                }
                else if (otherNum > 0) {
                    if (typeConnexite == TypeConnexite.Engagement) {
                        throw new BusinessValidationException(new ValidationError("Ce contrat est déjà dans une autre connexité. Fusionner/Détacher impossible pour les connexités d'engagements"));
                    }
                    throw new BusinessValidationException(new ValidationError("FUSION", "Ce contrat est déjà dans une autre connexité"));
                }
                contratConnexe.NumConnexite = folderNum;
            }
            else {
                if (otherNum > 0) {
                    // replace target and source
                    var newConnected = new Folder(folder.CodeOffre, folder.Version, folder.Type[0]);
                    folder = new Folder(contratConnexe.NumContrat, contratConnexe.VersionContrat, contratConnexe.TypeContrat[0]);
                    var otherCnx = GetConnexites(folder, typeConnexite);
                    var affaire = this.affaireService.GetAffaire(newConnected.Adapt<AffaireId>());
                    contratConnexe.NumContrat = newConnected.CodeOffre;
                    contratConnexe.VersionContrat = (short)newConnected.Version;
                    contratConnexe.TypeContrat = newConnected.Type;
                    contratConnexe.CodeBranche = affaire.Branche.Code;
                    contratConnexe.CodeSousBranche = affaire.SousBranche;
                    contratConnexe.CodeCible = affaire.Cible.Code;
                    contratConnexe.CodeCategorie = affaire.Categorie;
                    contratConnexe.CodeObservation = otherCnx.First().CodeObservation;
                    contratConnexe.NumConnexite = otherNum;
                }
                else {
                    contratConnexe.NumConnexite = this.as400Repository.GetNextNumber(new PGMFolder(folder) { User = user, ActeGestion = "" }, GetNumeroKey(typeConnexite));
                }
            }

            if (typeConnexite == TypeConnexite.Engagement)
            {
                var mainPolice = this.repository.GetFolders(typeConnexite, contratConnexe.NumConnexite)
                    .FirstOrDefault(p => p.NumContrat == folder.CodeOffre && p.VersionContrat == folder.Version);
                contratConnexe.IdeConnexite = mainPolice?.IdeConnexite ?? 0;
            }

            contratConnexe.CodeObservation = this.observationsRepository.AddOrUpdate(new Observation { Id = (int)contratConnexe.CodeObservation, Folder = folder, IsGenerale = true, Texte = contratConnexe.Observation });
            this.repository.Add(folder, contratConnexe);
        }

        public void DetachContrat(Folder folder, Folder folderToDetach, TypeConnexite typeConnexite)
        {
            int numConnexite = this.repository.GetNumero(folder, typeConnexite);
            //var typeCnx = ((int)typeConnexite).ToString().PadLeft(2, '0');
            this.repository.Delete(folderToDetach,(int)typeConnexite, numConnexite);
            var remaining = GetConnexites(folder, typeConnexite);
            bool noMoreConnections = remaining.Select(c => c.NumContrat).Distinct().Count() == 1;
            if (noMoreConnections)
            {
                this.repository.Delete(folder, (int)typeConnexite, numConnexite);
                if (typeConnexite == TypeConnexite.Engagement)
                {
                    //this.engagementRepository.DeleteAll(remaining.First().IdeConnexite);
                    // todo: rather save last CONX
                }
            }
        }

        public void CreateOrUpdateEngagement(Folder folder, PeriodeConnexiteDto periode, int? ide = null)
        {
            bool isUpdating = periode.CodeEngagement > 0;
            periode.CodeEngagement = this.engagementRepository.AddOrUpdate(periode);
            if (folder == null)
            {
                folder = new Folder(periode.CodeOffre, periode.Version, periode.Type[0]);
            }
            if (ide.GetValueOrDefault() > 0)
            {
                this.engagementRepository.AddConnexite((int)periode.CodeEngagement, ide.Value, periode.Ordre);
            }
            else
            {
                (int numero, int id, bool isIdNew) = this.engagementRepository.GetNumAndId(folder, !isUpdating);
                if (!isUpdating && isIdNew && periode.Ordre < 1)
                {
                    periode.Ordre = 1;
                }
                this.engagementRepository.AddConnexite((int)periode.CodeEngagement, id, periode.Ordre);
                if (!isUpdating && isIdNew)
                {
                    this.repository.SetIdConnexiteEngagement(numero, id);
                }
            }
        }

        public void PreparePeriodesOrdering(int numeroEngagement)
        {
            this.engagementRepository.DeleteCNXOnly(numeroEngagement);
        }

        public void DeleteEngagement(int code)
        {
            this.engagementRepository.Delete(code);
        }

        internal static string GetNumeroKey(TypeConnexite type)
        {
            return $"POLICE_CONNEX_{type.AsCode()}";
        }

        internal IEnumerable<PeriodeConnexiteDto> GetPeriodesEngagements(int numeroConnexite)
        {
            var periodesData = this.engagementRepository.GetPeriodes(numeroConnexite);
            var periodes = periodesData.OrderBy(x => x.Ordre).GroupBy(p => p.CodeEngagement).Select(g =>
            {
                var p = g.First();
                return new PeriodeConnexiteDto
                {
                    CodeEngagement = p.CodeEngagement,
                    CodeOffre = string.Empty,
                    DateDebut = p.DateDebut.YMDToDate(),
                    DateFin = p.DateFin.YMDToDate(),
                    Type = string.Empty,
                    Version = 0,
                    IsInactive = p.Active == Booleen.Non.AsCode(),
                    IsUsed = false,
                    IsEnCours = p.EnCours != Booleen.Non.AsCode(),
                    Ordre = p.Ordre,
                    Traites = g.GroupBy(x => x.CodeTraite).ToDictionary(x => x.Key, x => x.First().Montant)
                };
            }).ToList();
            if (periodes?.Any() == true)
            {
                var usages = engagementRepository.GetPeriodesUsed(periodes.Select(p => (int)p.CodeEngagement)).ToDictionary(x => x.id, x => x.count);
                periodes.ForEach(p =>
                {
                    usages.TryGetValue((int)p.CodeEngagement, out int count);
                    p.IsUsed = count > 0;
                });
            }
            return periodes;
        }

        internal void ModifyPeriodesEngagements(Folder folder, IEnumerable<PeriodeConnexiteDto> periodes)
        {
            ValidateEngagement(folder, periodes, out var deletedPeriodes);
            if (deletedPeriodes?.Any() ?? false)
            {
                deletedPeriodes.ToList().ForEach(p => DeleteEngagement((int)p.CodeEngagement));
            }
            if (periodes.Any())
            {
                var orderedPeriodes = SortPeriodesEngagements(periodes);
                orderedPeriodes.ForEach(x => x.IsEnCours = false);
                var p = orderedPeriodes.First(x => !x.IsInactive);
                p.DateDebut = null;
                p = orderedPeriodes.Last(x => !x.IsInactive);
                p.DateFin = null;
                p.IsEnCours = true;
                (int numero, int id, bool isIdNew) = this.engagementRepository.GetNumAndId(folder, false);
                PreparePeriodesOrdering(id);
                orderedPeriodes.ForEach(x =>
                {
                    CreateOrUpdateEngagement(folder, x, id);
                });
            }
        }

        internal void Merge(FusionConnexitesDto fusionDto) {
            var (num, targetNum) = CheckFusionParams(fusionDto.Affaire, fusionDto.AffaireCible, fusionDto.TypeConnexite, nameof(Merge));
            var mainCnx = this.affaireService.GetAffaireConnexite(fusionDto.Affaire.Adapt<AffaireId>(), num, (int)fusionDto.TypeConnexite);
            var targetFolders = GetFolders(fusionDto.TypeConnexite, targetNum);
            var firstTarget = targetFolders.First();
            targetFolders.Skip(1).ToList().ForEach(x => DetachContrat(firstTarget, x, fusionDto.TypeConnexite));
            targetFolders.ToList().ForEach(f => {
                var a = this.affaireService.GetAffaire(f.Adapt<AffaireId>());
                var contratConnexe = new ContratConnexeDto {
                    NumContrat = f.CodeOffre,
                    VersionContrat = (short)f.Version,
                    TypeContrat = f.Type,
                    CodeBranche = a.Branche.Code,
                    CodeSousBranche = a.SousBranche,
                    CodeCible = a.Cible.Code,
                    CodeCategorie = a.Categorie,
                    NumConnexite = num,
                    CodeTypeConnexite = fusionDto.TypeConnexite.AsCode(),
                    CodeObservation = mainCnx.CodeObservation
                };
                this.repository.Add(fusionDto.Affaire, contratConnexe);
            });
        }

        internal void PickTarget(FusionConnexitesDto fusionDto) {
            var (num, targetNum) = CheckFusionParams(fusionDto.Affaire, fusionDto.AffaireCible, fusionDto.TypeConnexite, nameof(PickTarget));
            var targetFolders = GetFolders(fusionDto.TypeConnexite, targetNum);
            var firstTarget = targetFolders.First(x => x.Identifier != fusionDto.AffaireCible.Identifier);
            DetachContrat(firstTarget, fusionDto.AffaireCible, fusionDto.TypeConnexite);
            var mainCnx = this.affaireService.GetAffaireConnexite(fusionDto.Affaire.Adapt<AffaireId>(), num, (int)fusionDto.TypeConnexite);
            var a = this.affaireService.GetAffaire(fusionDto.AffaireCible.Adapt<AffaireId>());
            var contratConnexe = new ContratConnexeDto {
                NumContrat = a.CodeAffaire,
                VersionContrat = (short)a.NumeroAliment,
                TypeContrat = a.TypeAffaire.AsCode(),
                CodeBranche = a.Branche.Code,
                CodeSousBranche = a.SousBranche,
                CodeCible = a.Cible.Code,
                CodeCategorie = a.Categorie,
                NumConnexite = num,
                CodeTypeConnexite = fusionDto.TypeConnexite.AsCode(),
                CodeObservation = mainCnx.CodeObservation
            };
            this.repository.Add(fusionDto.Affaire, contratConnexe);
        }

        internal void MoveSource(FusionConnexitesDto fusionDto) {
            var (num, targetNum) = CheckFusionParams(fusionDto.Affaire, fusionDto.AffaireCible, fusionDto.TypeConnexite, nameof(MoveSource));
            var sourceFolders = GetFolders(fusionDto.TypeConnexite, num);
            var firstSource = sourceFolders.First(x => x.Identifier != fusionDto.Affaire.Identifier);
            DetachContrat(firstSource, fusionDto.Affaire, fusionDto.TypeConnexite);
            var mainCnx = this.affaireService.GetAffaireConnexite(fusionDto.AffaireCible.Adapt<AffaireId>(), targetNum, (int)fusionDto.TypeConnexite);
            var a = this.affaireService.GetAffaire(fusionDto.Affaire.Adapt<AffaireId>());
            var contratConnexe = new ContratConnexeDto {
                NumContrat = a.CodeAffaire,
                VersionContrat = (short)a.NumeroAliment,
                TypeContrat = a.TypeAffaire.AsCode(),
                CodeBranche = a.Branche.Code,
                CodeSousBranche = a.SousBranche,
                CodeCible = a.Cible.Code,
                CodeCategorie = a.Categorie,
                NumConnexite = targetNum,
                CodeTypeConnexite = fusionDto.TypeConnexite.AsCode(),
                CodeObservation = mainCnx.CodeObservation
            };
            this.repository.Add(fusionDto.AffaireCible, contratConnexe);
        }

        private (int num, int targetNum) CheckFusionParams(Folder affaire, Folder affaireCible, TypeConnexite typeConnexite, string operationName) {
            if (!Enum.IsDefined(typeof(TypeConnexite), typeConnexite)) {
                throw new ArgumentException(nameof(typeConnexite));
            }
            else if (typeConnexite == TypeConnexite.Engagement) {
                throw new BusinessValidationException(new ValidationError(operationName == nameof(Merge) 
                    ? "Impossible d'effectuer une fusion de connexités d'engagements"
                    : "Impossible d'effectuer un déplacement pour des connexités d'engagements"));
            }
            int num = this.repository.GetNumero(affaire, typeConnexite);
            int targetNum = this.repository.GetNumero(affaireCible, typeConnexite);
            if (num == targetNum) {
                throw new BusinessValidationException(new ValidationError("Contrat déjà dans la connexité"));
            }
            else if (num < 1 || targetNum < 1) {
                throw new BusinessValidationException(new ValidationError("Opération invalide"));
            }
            return (num, targetNum);
        }

        private void ValidateEngagement(Folder folder, IEnumerable<PeriodeConnexiteDto> periodes, out IEnumerable<PeriodeConnexiteDto> deletedPeriodes)
        {
            var connexites = GetConnexites(folder, TypeConnexite.Engagement).ToList();
            var connexite = connexites?.FirstOrDefault();
            if (connexite == null)
            {
                throw new BusinessException("Aucune connexité d'engagement n'a été trouvée");
            }
            var currentPeriodes = GetPeriodesEngagements(connexite.NumConnexite);
            ComparePeriodes(currentPeriodes, periodes, out deletedPeriodes);
            CheckPeriodesDates(periodes, connexites.Select(x => new Folder(x.NumContrat, x.VersionContrat, x.TypeContrat[0])));
            CheckMontantsPeriodes(periodes, connexites);
        }

        private void ComparePeriodes(IEnumerable<PeriodeConnexiteDto> currentPeriodes, IEnumerable<PeriodeConnexiteDto> periodes, out IEnumerable<PeriodeConnexiteDto> toDelete)
        {
            var currentIds = currentPeriodes.Select(p => p.CodeEngagement).OrderBy(x => x).ToList();
            var ids = periodes.Select(p => p.CodeEngagement).OrderBy(x => x);
            if (currentIds.SequenceEqual(ids))
            {
                toDelete = Enumerable.Empty<PeriodeConnexiteDto>();
            }
            else
            {
                if (ids.Count() == 0 && currentPeriodes.Any(p => p.IsUsed))
                {
                    string message = "Impossible de supprimer des périodes utilisées";
                    if (currentPeriodes.Count(p => p.IsUsed) == 1)
                    {
                        message = "Impossible de supprimer une période utilisée";
                    }
                    throw new BusinessValidationException(new ValidationError(string.Empty, string.Empty, string.Empty, message));
                }
                var absentIds = currentIds.Except(ids);
                toDelete = currentPeriodes.Where(p => absentIds.Contains(p.CodeEngagement));
            }
        }

        private void CheckPeriodesDates(IEnumerable<PeriodeConnexiteDto> periodes, IEnumerable<Folder> folders)
        {
            if (periodes.Any())
            {
                var errors = new List<ValidationError>();
                var periodesActives = periodes.Where(p => !p.IsInactive);
                var dateFinMin = periodesActives.First().DateFin;
                var dateDebutMax = periodesActives.Last().DateDebut;
                var dateList = periodesActives.Select(p => new { id = p.CodeEngagement, order = p.Ordre, dates = new[] { p.DateDebut, p.DateFin } }).ToDictionary(x => $"{x.id}-{x.order}", x => x.dates);
                DateTime? last = null;
                bool hasInvalidBeginEnd = false;
                foreach (var id in dateList.Keys)
                {
                    if (dateList[id][0] > dateList[id][1])
                    {
                        hasInvalidBeginEnd = true;
                        errors.RemoveAll(e => e.TargetType == string.Empty);
                        errors.Add(new ValidationError("deb-fin", id.Split('-')[1], id.Split('-')[0], "DateDebut", "La date de début doit être inférieure ou égale à la date de fin"));
                    }
                    else if (!hasInvalidBeginEnd && last.HasValue && last.Value.AddDays(1) != dateList[id][0])
                    {
                        errors.Add(new ValidationError(string.Empty, id.Split('-')[1], id.Split('-')[0], "DateDebut", "Les périodes doivent être contigues"));
                    }
                    last = dateList[id][1];
                }

                if (periodesActives.Count() > 1 && !errors.Any())
                {
                    var foldersEffets = this.affaireRepository.GetDatesEffets(folders.Select(f => f.CodeOffre)).ToList();
                    foreach (var f in folders)
                    {
                        foldersEffets.RemoveAll(fd => fd.Ipb == f.CodeOffre && fd.Alx != f.Version);
                    }
                    if (dateFinMin.Value < foldersEffets.Min(f => f.Debut.Value))
                    {
                        errors.Add(new ValidationError(string.Empty, string.Empty, periodesActives.First().CodeEngagement.ToString(), "DateFin", "Date de fin trop petite"));
                    }
                    if (foldersEffets.Any(f => f.Fin.HasValue) && dateDebutMax.Value > foldersEffets.Where(f => f.Fin.HasValue).Max(f => f.Fin.Value))
                    {
                        errors.Add(new ValidationError(string.Empty, string.Empty, periodesActives.Last().CodeEngagement.ToString(), "DateDebut", "Date de début trop grande"));
                    }
                }

                if (errors.Any())
                {
                    throw new BusinessValidationException(errors);
                }
            }
        }

        private void CheckMontantsPeriodes(IEnumerable<PeriodeConnexiteDto> periodes, List<ContratConnexeDto> connexites)
        {
            if (periodes?.Any() ?? false)
            {
                if (connexites?.Any() != true)
                {
                    throw new ArgumentNullException(nameof(connexites), "Les connexités sont requises");
                }
                var errors = new List<ValidationError>();
                var montantsGroupes = connexites.ToLookup(c => c.CodeEngagement);
                foreach (var p in periodes)
                {
                    foreach (var t in p.Traites)
                    {
                        if (montantsGroupes[t.Key].Count() < 2 && t.Value != 0)
                        {
                            errors.Add(new ValidationError(string.Empty, p.Ordre.ToString(), p.CodeEngagement.ToString(), "Montant", "Impossible de saisir une valeur"));
                        }
                    }
                }
                if (errors.Any())
                {
                    throw new BusinessValidationException(errors);
                }
            }
        }

        private List<PeriodeConnexiteDto> SortPeriodesEngagements(IEnumerable<PeriodeConnexiteDto> periodes)
        {
            int x = 0;
            return periodes.OrderBy(p => p.DateDebut).Select(p =>
            {
                if (p.IsInactive)
                {
                    p.Ordre = x;
                }
                else
                {
                    p.Ordre = ++x;
                }
                return p;
            }).ToList();
        }
    }
}
