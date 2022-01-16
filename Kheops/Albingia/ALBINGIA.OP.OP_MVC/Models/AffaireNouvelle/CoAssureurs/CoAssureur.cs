using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CoAssureurs
{
    public class CoAssureur
    {
        public string GuidId { get; set; }

        //Liste des champs
        public string Code { get; set; }
        public string Nom { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public Single PourcentPart { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }

        public int IdInterlocuteur { get; set; }
        public string Interlocuteur { get; set; }
        public string Reference { get; set; }
        public Single FraisAcc { get; set; }
        public Single CommissionAperiteur { get; set; }
        public Single FraisApeAlb { get; set; }

        public bool ModeCoass { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsModifHorsAvn { get; set; }
        public string TypeOperation { get; set; }

        public static explicit operator CoAssureur(CoAssureurDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CoAssureurDto, CoAssureur>().Map(data);
        }

    }


}