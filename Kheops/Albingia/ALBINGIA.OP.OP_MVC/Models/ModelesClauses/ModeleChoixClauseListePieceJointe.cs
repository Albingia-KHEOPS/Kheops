using ALBINGIA.Framework.Common.IOFile;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleChoixClauseListePieceJointe
    {
        public Int64 PieceJointeId { get; set; }
        public DateTime? DateAjout { get; set; }
        public string Acte { get; set; }
        public Int64 Avenant { get; set; }
        public string Titre { get; set; }
        public string Chemin { get; set; }
        public string Fichier { get; set; }
        public string Reference { get; set; }

        public string DateAjtStr
        {
            get
            {

                return DateAjout.HasValue ? string.Format("{0}/{1}/{2}", DateAjout.Value.Day.ToString().PadLeft(2, '0'), DateAjout.Value.Month.ToString().PadLeft(2, '0'), DateAjout.Value.Year) : string.Empty;
            }
        }

        public bool IsExist
        {
            get
            {
                return IOFileManager.IsExistFile(Chemin.Replace(Fichier, string.Empty), Fichier);
            }
        }

        public bool Checkable
        {
            get
            {
                return IOFileManager.IsInExtensionFiles(FileContentManager.GetConfigValue("ExtensionClause"), IOFileManager.GetStrExtension(Fichier));
            }
        }

    }
}