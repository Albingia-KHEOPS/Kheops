using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesEcheance
{
    public class ModeleEcheance
    {
        public bool IsReadonly { get; set; }
        public int Guid { get; set; }
        [Display(Name = "Date échéance")]
        public Nullable<DateTime> EcheanceDate { get; set; }
        [Display(Name = "Montant échéance HT (€)")]
        public double MontantEcheanceHT { get; set; }
        public double MontantEcheanceCalcule { get; set; }
        [Display(Name = "ou % prime")]
        public decimal PourcentagePrime { get; set; }
        public decimal PourcentageCalcule { get; set; }
        [Display(Name = "Frais accessoires (€)")]
        public decimal FraisAccessoire { get; set; }
        [Display(Name = "FGA")]
        public bool TaxeAttentat { get; set; }
        public bool IsEcheanceEmise { get; set; }
           
        public static explicit operator ModeleEcheance(EcheanceDto echeanceDto)
        {
            var toReturn = ObjectMapperManager.DefaultInstance.GetMapper<EcheanceDto, ModeleEcheance>().Map(echeanceDto);
            if (echeanceDto.DateEcheanceAnnee != 0 && echeanceDto.DateEcheanceMois != 0 && echeanceDto.DateEcheanceJour != 0)
                toReturn.EcheanceDate = new DateTime(echeanceDto.DateEcheanceAnnee, echeanceDto.DateEcheanceMois, echeanceDto.DateEcheanceJour);
            toReturn.MontantEcheanceHT = echeanceDto.Montant;
            toReturn.MontantEcheanceCalcule = echeanceDto.MontantCalcule;
            toReturn.TaxeAttentat = echeanceDto.AppliqueTaxeAttentat == "O" ? true : false;
            toReturn.IsEcheanceEmise = echeanceDto.NumPrime != 0;
            return toReturn;
        }
        public static List<ModeleEcheance> LoadEcheances(List<EcheanceDto> echeancesDto, bool isreadonly)
        {
            var toReturn = new List<ModeleEcheance>();
            foreach (var echDto in echeancesDto)
            {
                var toAdd = (ModeleEcheance)echDto;
                toAdd.IsReadonly = isreadonly;
                toReturn.Add(toAdd);               
            }
            return toReturn;
        }
    }
}