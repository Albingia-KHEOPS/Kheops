using ALBINGIA.Framework.Common;
using System;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.NavigationArbre {

    public class ArbreDto
    {
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [Column(Name = "CODEOFFRE")]
        public string CodeOffre { get; set; }
        [Column(Name = "VERSION")]
        public Int64 Version { get; set; }
        [Column(Name = "NUMAVN")]
        public Int64 NumAvn { get; set; }
        [Column(Name = "DESCOFFRE")]
        public string DescOffre { get; set; }
        [Column(Name = "ETAPE")]
        public string Etape { get; set; }
        [Column(Name = "ETAPEORD")]
        public Int64 EtapeOrd { get; set; }
        [Column(Name = "ORDRE")]
        public Int64 Ordre { get; set; }
        [Column(Name = "PERIMETRE")]
        public string Perimetre { get; set; }
        [Column(Name = "CODERSQ")]
        public Int64 CodeRsq { get; set; }
        [Column(Name = "DESCRSQ")]
        public string DescRsq { get; set; }
        [Column(Name = "CODEOBJ")]
        public Int64 CodeObj { get; set; }
        [Column(Name = "CODEFOR")]
        public Int64 CodeFor { get; set; }
        [Column(Name = "LETTREFOR")]
        public string LettreFor { get; set; }
        [Column(Name = "DESCFOR")]
        public string DescFor { get; set; }
        [Column(Name = "CODEOPT")]
        public Int64 CodeOpt { get; set; }
        [Column(Name = "CHRONORSQ")]
        public Int64 ChronoRsq { get; set; }
        [Column(Name = "PASSAGETAG")]
        public string PassageTag { get; set; }
        [Column(Name = "AVTFOR")]
        public Int64 CodeFormuleAvt { get; set; }
        [Column(Name = "CREATEAVT")]
        public Int64 CreateAvt { get; set; }
        [Column(Name = "MODIFAVT")]
        public Int64 ModifAvt { get; set; }
        [Column(Name = "DATEDEBAVN")]
        public Int64 DateDebAvn { get; set; }
        [Column(Name = "DATEFINRSQ")]
        public Int64 DateFinRsq { get; set; }
        public bool isBadDate { get; set; }

        public bool IsFolder(Folder folder) {
            return (CodeOffre?.Trim() ?? ".") == (folder?.CodeOffre?.Trim() ?? "..")
                && Version == folder?.Version
                && (Type?.Trim() ?? ".") == (folder?.Type?.Trim() ?? "..");
        }
    }
}
