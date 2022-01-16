using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleCreationSaisiePage : MetaModelsBase
    {
        public string txtParamRedirect { get; set; }

        /*public new string CodeBranche
        {
            get
            {
                return AllParameters?[AlbParameterName.BRANCHEOFFRE] as string ?? string.Empty;
            }
        }*/
        public ModeleCabinetCourtage CabinetCourtage { get; set; }

        public ModelePreneurAssurance PreneurAssurance { get; set; }

        public ModeleInformationSaisie InformationSaisie { get; set; }

        public ModeleDescription Description { get; set; }

        public ModeleContactAdresse ContactAdresse { get; set; }

        public ModeleRechercheAvancee ListCabinetCourtageGestion { get; set; }

        public bool EditMode { get; set; }
        public string CodeOffre { get; set; }
        public int? Version { get; set; }
        public string Type { get; set; }
        //public string TabGuid { get; set; }

        public bool CopyMode { get; set; }

        /// <summary>
        /// Permet de savoir si la page est chargée dynamiquement sur changement de template dans la liste
        /// </summary>
        public bool LoadTemplateMode { get; set; }

        public string CodeOffreCopy { get; set; }
        public string VersionCopy { get; set; }

        public string Cible { get; set; }

        public string SouscripteurSelect { get; set; }
        public string GestionnaireSelect { get; set; }

        public bool IsConfirmation { get; set; }

        public bool IsReadOnlyDisplay { get; set; }
    
        public ModeleCreationSaisiePage()
        {
            ContactAdresse = new ModeleContactAdresse(16, false, true);
            CabinetCourtage = new ModeleCabinetCourtage();
            PreneurAssurance = new ModelePreneurAssurance();
            InformationSaisie = new ModeleInformationSaisie();
            Description = new ModeleDescription();
        }
    }
}