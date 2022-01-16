using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Models.AutoComplete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Models
{
    [Serializable]
    public class FiltreAffaire {
        public FiltreAffaire() {
            Souscripteur = null;
            Gestionnaire = null;
            PreneurAssurance = null;
            PreneurAssuranceVille = null;
            Courtier = null;
            TypesDates = AlbConstantesMetiers.GetTypesDatesRecherche().Select(x => new LabeledValue(x.code.ToString(), x.text)).ToList();
            CodeTypeDate = AlbConstantesMetiers.TypeDateRecherche.MAJ.ToString();
            Branches = new List<LabeledValue>();
            Cibles = new List<LabeledValue>();
            Etats = new List<LabeledValue>();
            ExcludeCodeEtat = false;
            Situations = new List<LabeledValue>();
            ExcludedAffairs = new List<Folder>();
            Mode = 'S';
            Text = string.Empty;
        }
        public string Code { get; set; }
        public int? Version { get; set; }
        public char Mode { get; set; }
        public bool GetOffresOnly { get; set; }
        public bool GetContratsOnly { get; set; }
        public string Text { get; set; }
        public List<LabeledValue> TypesDates { get; set; }
        public string CodeTypeDate { get; set; }
        public DateTime? DateMin { get; set; }
        public DateTime? DateMax { get; set; }
        public List<LabeledValue> Branches { get; set; }
        public string CodeBranche { get; set; }
        public List<LabeledValue> Cibles { get; set; }
        public string CodeCible { get; set; }
        public List<LabeledValue> Etats { get; set; }
        public string CodeEtat { get; set; }
        public bool ExcludeCodeEtat { get; set; }
        public List<LabeledValue> Situations { get; set; }
        public string CodeSituation { get; set; }
        public bool GetInactifsOnly { get; set; }
        public bool GetEnCoursOnly { get; set; }
        public bool GetGestionnairesOnly { get; set; }

        public bool GetApporteursOnly { get; set; }
        public Souscripteur Souscripteur { get; set; }
        public Gestionnaire Gestionnaire { get; set; }
        public Preneur PreneurAssurance { get; set; }
        public string Siren { get; set; }
        public Ville PreneurAssuranceVille { get; set; }
        public Courtier Courtier { get; set; }
        public string TexteAdresse { get; set; }
        public Ville AdresseVille { get; set; }
        public string SortingField { get; set; }
        public char? SortingDirection { get; set; }
        public string Sorting => SortingDirection.HasValue ? string.Empty : SortingDirection == 'A' ? "ASC" : "DESC";
        public List<Folder> ExcludedAffairs { get; set; }
    }
}
