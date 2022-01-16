using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CoAssureurs;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage
{
    public class AnCoAssureursPage : MetaModelsBase
    {
        public bool IsModeAvenant { get; set; }
        public bool IsAvenantModificationLocale { get; set; }
        public bool IsTraceAvnExist { get; set; }
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
        public DateTime? DateEffetAvenant { get; set; }

        public List<CoAssureur> ListeCoAssureurs { get; set; }

        public Single PartAlbingia { get; set; }
        public Single PartCouverte { get; set; }
        public bool EstVerrouillee { get; set; }       

        public static explicit operator AnCoAssureursPage(FormCoAssureurDto data) {
            var toReturn = ObjectMapperManager.DefaultInstance.GetMapper<FormCoAssureurDto, AnCoAssureursPage>().Map(data);
            toReturn.DateEffetAvenant = new DateTime(data.DateEffetAvnAnnee, data.DateEffetAvnMois, data.DateEffetAvnJour);
            return toReturn;
        }
    }
}