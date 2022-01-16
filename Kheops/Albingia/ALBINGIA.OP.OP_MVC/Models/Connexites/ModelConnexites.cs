using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.Connexites;
using OP.WSAS400.DTO.Engagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Models
{
    public class ModelConnexites {
        public ModelConnexites() {
            Engagements = new List<ConnexiteEngagement>();
            FamillesReassurances = new List<LabeledValue>();
            PeriodesEngagements = new List<PeriodeEngagement>();
            Remplacements = new List<ConnexiteRemplacement>();
            Informations = new List<ConnexiteInformation>();
            Resiliations = new List<ConnexiteResiliation>();
            Regularisations = new List<ConnexiteRegularisation>();
        }
        public DateTime? DateEngagementMin { get; set; }
        public DateTime? DateEngagementMax { get; set; }
        public bool MustFixPeriodes { get; set; }
        public Folder Folder { get; set; }
        public string CodeBranche { get; set; }
        public string CodeSousBranche { get; set; }
        public string CodeCible { get; set; }
        public ConnexiteContext Context => new ConnexiteContext {
            Engagement = Engagements?.FirstOrDefault(e => Equals(e.Folder, Folder)) ?? new ConnexiteEngagement(),
            Information = Informations?.FirstOrDefault(e => Equals(e.Folder, Folder)) ?? new ConnexiteInformation(),
            Regularisation = Regularisations?.FirstOrDefault(e => Equals(e.Folder, Folder)) ?? new ConnexiteRegularisation(),
            Resiliation = Resiliations?.FirstOrDefault(e => Equals(e.Folder, Folder)) ?? new ConnexiteResiliation(),
            Remplacement = Remplacements?.FirstOrDefault(e => Equals(e.Folder, Folder)) ?? new ConnexiteRemplacement()
        };
        public List<ConnexiteEngagement> Engagements { get; set; }
        public List<LabeledValue> FamillesReassurances { get; set; }
        public List<PeriodeEngagement> PeriodesEngagements { get; set; }
        public PeriodeEngagement PeriodePattern => new PeriodeEngagement();
        public List<ConnexiteRemplacement> Remplacements { get; set; }
        public List<ConnexiteInformation> Informations { get; set; }
        public List<ConnexiteResiliation> Resiliations { get; set; }
        public List<ConnexiteRegularisation> Regularisations { get; set; }
        public bool IsReadonly { get; set; }
        public List<LabeledValue> AllTypes
            => Enum.GetValues(typeof(TypeConnexite)).Cast<TypeConnexite>().Select(v => new LabeledValue(v.AsCode(), v.ToString())).ToList();

        public static Type DetermineType(TypeConnexite enumType) {
            switch (enumType) {
                case TypeConnexite.Remplacement:
                    return typeof(ConnexiteRemplacement);
                case TypeConnexite.Information:
                    return typeof(ConnexiteInformation);
                case TypeConnexite.Engagement:
                    return typeof(ConnexiteEngagement);
                case TypeConnexite.Resiliation:
                    return typeof(ConnexiteResiliation);
                case TypeConnexite.Regularisation:
                    return typeof(ConnexiteRegularisation);
            }
            return null;
        }

        public void SetAppropriateList(TypeConnexite enumType, IEnumerable<ConnexiteBase> connexites) {
            switch (enumType) {
                case TypeConnexite.Remplacement:
                    Remplacements = connexites?.Cast<ConnexiteRemplacement>().ToList();
                    break;
                case TypeConnexite.Information:
                    Informations = connexites?.Cast<ConnexiteInformation>().ToList();
                    break;
                case TypeConnexite.Engagement:
                    Engagements = connexites?.Cast<ConnexiteEngagement>().ToList();
                    if (Engagements?.Any() == true) {
                        var traites = Engagements.SelectMany(x => x.Valeurs.Select(y => y.CodeTraite)).Distinct().ToArray();
                        var quantities = traites.Where(x => x.ContainsChars()).ToDictionary(x => x, x => Engagements.Where(e => e.Valeurs.Any(v => v.CodeTraite == x)).Count());
                        FamillesReassurances.AddRange(quantities.OrderByDescending(x => x.Value).Select(x => new LabeledValue(x.Key, x.Key)));
                        Engagements.ForEach(e => {
                            e.Valeurs = FamillesReassurances.Select(x => {
                                var valeurs = e.Valeurs.Where(v => v.CodeTraite == x.Code).FirstOrDefault();
                                return new ValeurEngagement { CodeTraite = x.Code, Montant = valeurs?.Montant, MontantTotal = valeurs?.MontantTotal };
                            }).ToList();
                        });
                        PeriodesEngagements.ForEach(p => {
                            p.Valeurs = FamillesReassurances.Select(x => {
                                var valeurs = p.Valeurs.Where(v => v.CodeTraite == x.Code).FirstOrDefault();
                                return new ValeurEngagement { CodeTraite = x.Code, Montant = valeurs?.Montant ?? 0, MontantTotal = valeurs?.MontantTotal ?? 0 };
                            }).ToList();
                        });
                    }
                    break;
                case TypeConnexite.Resiliation:
                    Resiliations = connexites?.Cast<ConnexiteResiliation>().ToList();
                    break;
                case TypeConnexite.Regularisation:
                    Regularisations = connexites?.Cast<ConnexiteRegularisation>().ToList();
                    break;
            }
        }

        public IEnumerable<ConnexiteBase> GetListFormType(TypeConnexite? typeConnexite) {
            switch (typeConnexite) {
                case TypeConnexite.Remplacement:
                    return Remplacements;
                case TypeConnexite.Information:
                    return Informations;
                case TypeConnexite.Engagement:
                    return Engagements;
                case TypeConnexite.Resiliation:
                    return Resiliations;
                case TypeConnexite.Regularisation:
                    return Regularisations;
                default:
                    return Enumerable.Empty<ConnexiteBase>();
            }
        }

        public static List<ConnexiteBase> BuildListFrom(Folder masterFolder, IEnumerable<ContratConnexeDto> connexitesDto, TypeConnexite t) {
            List<ConnexiteBase> connexites;
            if (t == TypeConnexite.Engagement) {
                connexites = BuildListEngagements(connexitesDto.Where(c => int.Parse(c.CodeTypeConnexite) == (int)t)).ToList();
            }
            else {
                Type type = DetermineType(t);
                connexites = connexitesDto.Where(c => int.Parse(c.CodeTypeConnexite) == (int)t)
                    .Select(c => {
                        var connexite = Activator.CreateInstance(type) as ConnexiteBase;
                        connexite.CopyFrom(c);
                        return connexite;
                    })
                    .OrderByDescending(c => c.Folder.CodeOffre)
                    .ToList(); 
            }

            var firstCnx = connexites.FirstOrDefault(c => c.Folder.Equals(masterFolder));
            if (firstCnx != null) {
                connexites.Remove(firstCnx);
                connexites.Insert(0, firstCnx); 
            }
            return connexites;
        }

        static IEnumerable<ConnexiteBase> BuildListEngagements(IEnumerable<ContratConnexeDto> engagementsDto) {
            return engagementsDto.GroupBy(x => x.NumContrat)
                .Select(g => {
                    var engagement = new ConnexiteEngagement();
                    engagement.CopyFrom(g.First());
                    engagement.Valeurs.AddRange(g.Select(x => new ValeurEngagement { CodeTraite = x.CodeEngagement, Montant = x.ValeurEngagement, MontantTotal = x.TotalEngagement }));
                    return engagement;
                })
                .OrderByDescending(c => c.Folder.CodeOffre);
        }
    }
}