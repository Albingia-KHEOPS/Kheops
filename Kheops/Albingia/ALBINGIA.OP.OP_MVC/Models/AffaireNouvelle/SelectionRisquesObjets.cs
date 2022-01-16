using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Controllers;
using OP.WSAS400.DTO.Contrats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models {
    [Serializable]
    public class SelectionRisquesObjets : IKeyLocker {
        public Folder Folder { get; set; }
        public string TabGuid { get; set; }
        public string CodeAffaireNouvelle { get; set; }
        public int VersionAffaireNouvelle { get; set; }
        public List<SelectionRisqueObjets> Risques { get; set; }
        public List<SelectionFormuleOption> AvailableOptions { get; set; }
        string[] IKeyLocker.KeyValues {
            get => Folder is null ? null : new[] { Folder.CodeOffre.ToIPB(), Folder.Version.ToString(), Folder.Type, BaseController.GetSurroundedTabGuid(TabGuid) };
            set => throw new NotImplementedException();
        }
    }
}