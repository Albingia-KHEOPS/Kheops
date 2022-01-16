using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.GestionIntervenants;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionIntervenants
{
    public class ModeleIntervenant
    {
        public Int64 GuidId { get; set; }
        public string ModeEcran { get; set; }
        public string Type { get; set; }
        public string Denomination { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Interlocuteur { get; set; }
        public string Reference { get; set; }
        public string Fonction { get; set; }      
        public string IsMedecinConseil { get; set; }
        public string IsPrincipal { get; set; }
        public string IsFinValidite { get; set; }
        public bool IsReadOnly { get; set; }

        #region détails

        public List<AlbSelectListItem> ListeTypes { get; set; }
        public DateTime? DFinValidite { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public int CodeIntervenant { get; set; }
        public int CodeInterlocuteur { get; set; }


        #endregion

        public static explicit operator ModeleIntervenant(IntervenantDto data)
        {
            var toReturn = ObjectMapperManager.DefaultInstance.GetMapper<IntervenantDto, ModeleIntervenant>().Map(data);
            toReturn.Fonction = toReturn.IsMedecinConseil == "O" ? "Médecin conseil" : string.Empty;
            if (data.AnneeFinValidite > 0)
                toReturn.DFinValidite =  new DateTime(data.AnneeFinValidite, data.MoisFinValidite, data.JourFinValidite);
            return toReturn;
        }

    }
}