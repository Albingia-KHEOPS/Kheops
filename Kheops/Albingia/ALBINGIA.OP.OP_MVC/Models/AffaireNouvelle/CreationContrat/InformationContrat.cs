using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class InformationContrat
    {
        [Display(Name = "Mots-clés")]
        public List<AlbSelectListItem> MotsClefs { get; set; }

        public string MotClef1 { get; set; }
        public List<AlbSelectListItem> MotsClefs1 { get; set; }

        public string MotClef2 { get; set; }
        public List<AlbSelectListItem> MotsClefs2 { get; set; }

        public string MotClef3 { get; set; }
        public List<AlbSelectListItem> MotsClefs3 { get; set; }

        [Display(Name = "Descriptif")]
        public string Descriptif { get; set; }

        public string NumChronoObsv { get; set; }

        [Display(Name = "Observations")]
        public string Observation { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsModifHorsAvn { get; set; }

        //public void SetInformationContrat(ContratInfoBaseDto contratInfoBaseDto)
        //{
        //    using (var screenClient = new SaisieOffreClient())
        //    {
        //        var query = new ModifierOffreGetQueryDto();
        //        var result = screenClient.ModifierOffreGet(query);
        //        if (result != null)
        //        {
        //            List<AlbSelectListItem> motsCles1 = result.MotsCles.Select(
        //                m => new AlbSelectListItem
        //                {
        //                    Value = m.Code,
        //                    Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
        //                    Selected = false,
        //                    Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
        //                }).ToList();
        //            this.MotsClefs1 = motsCles1;

        //            List<AlbSelectListItem> motsCles2 = result.MotsCles.Select(
        //                m => new AlbSelectListItem
        //                {
        //                    Value = m.Code,
        //                    Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
        //                    Selected = false,
        //                    Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
        //                }).ToList();
        //            this.MotsClefs2 = motsCles2;

        //            List<AlbSelectListItem> motsCles3 = result.MotsCles.Select(
        //                m => new AlbSelectListItem
        //                {
        //                    Value = m.Code,
        //                    Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
        //                    Selected = false,
        //                    Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
        //                }).ToList();
        //            this.MotsClefs3 = motsCles3;
        //        }
        //    }
        //    this.MotClef1 = contratInfoBaseDto.CodeMotsClef1;
        //    this.MotClef2 = contratInfoBaseDto.CodeMotsClef2;
        //    this.MotClef3 = contratInfoBaseDto.CodeMotsClef3;
        //    this.Descriptif = contratInfoBaseDto.Descriptif;
        //    this.Observation = contratInfoBaseDto.Obersvations;
        //}
    }
}