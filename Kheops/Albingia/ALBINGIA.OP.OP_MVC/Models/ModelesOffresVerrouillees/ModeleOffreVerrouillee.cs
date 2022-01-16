using ALBINGIA.Framework.Common.Tools;
using OP.WSAS400.DTO.VerouillageOffres;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesOffresVerrouillees
{
    public class ModeleOffreVerrouillee
    {
        [Display(Name = "Date de verouillage")]
        public DateTime? DateVerouillage { get; set; }
        
        [Display(Name = "Code Offre")]
        public string NumOffre { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        [Display(Name = "Utilisateur")]
        public string Utilisateur { get; set; }
        [Display(Name = "Numéro Avenant")]
        public int NumAvenant { get; set; }    
       

        
        public static implicit operator ModeleOffreVerrouillee(string lockedOffer)
        {
            var elemsLocked = lockedOffer.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
            var user = elemsLocked[0].Contains("\\") ? elemsLocked[0].ToUpper().Split('\\')[1] : elemsLocked[0].ToUpper();
        
            var dateVerrou = string.IsNullOrEmpty(elemsLocked[4]) || elemsLocked[4]=="0"?null: AlbConvert.ConvertStrToDate(elemsLocked[4]);
            return new ModeleOffreVerrouillee { DateVerouillage = dateVerrou, NumOffre = elemsLocked[1],
              Version = elemsLocked[2], Utilisateur = string.IsNullOrEmpty(elemsLocked[0]) ? string.Empty : user,
              Type = elemsLocked[3],NumAvenant = Convert.ToInt32(elemsLocked[4])
              
            };            
        }

        public static explicit operator ModeleOffreVerrouillee(OffreVerouilleeDto data)
        {
            //return ObjectMapperManager.DefaultInstance.GetMapper<OffreVerouilleeDto, ModeleOffreVerrouillee>().Map(data);
          return new ModeleOffreVerrouillee
            {
              DateVerouillage = AlbConvert.ConvertIntToDate(data.DateVerouillage),
              NumOffre = data.NumOffre,
              Type = data.Type,
              Version = data.Version.ToString(CultureInfo.CurrentCulture),
              Utilisateur = data.Utilisateur,
              NumAvenant = data.NumAvenant
            };
        }
    }
}