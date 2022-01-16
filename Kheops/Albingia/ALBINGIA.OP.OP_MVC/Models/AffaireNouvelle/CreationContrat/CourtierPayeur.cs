using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class CourtierPayeur
    {
        public Courtier Courtier { get; set; }
        [Display(Name = "Encaissement *")]
        public String Encaissement { get; set; }
        public List<AlbSelectListItem> Encaissements { get; set; }
        //public void SetPayeur(ContratInfoBaseDto contratInfoBaseDto)
        //{            
        //    if (this.Encaissements != null)
        //        this.Encaissements.Clear();
        //    else this.Encaissements = new List<AlbSelectListItem>();
        //    contratInfoBaseDto.Encaissements.ToList().ForEach(
        //         elem => this.Encaissements.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
        //     );
        //    this.Encaissement = contratInfoBaseDto.Encaissement;
        //    this.Courtier = GetCourtier(contratInfoBaseDto);
        //}
        //private Courtier GetCourtier(ContratInfoBaseDto contratInfoBaseDto)
        //{
        //    var model = new Courtier();
        //    model.CodeCourtier = contratInfoBaseDto.CourtierPayeur;
        //    model.NomCourtier = contratInfoBaseDto.NomCourtierPayeur;       
        //    return model;
        //}
    }
}