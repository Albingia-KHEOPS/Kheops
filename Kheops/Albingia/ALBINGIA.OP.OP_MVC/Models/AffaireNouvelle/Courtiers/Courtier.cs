using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.Courtiers
{
    public class Courtier
    {
        public int CodeCourtier { get; set; }
        public string NomCourtier { get; set; }
        public string CodePostal { get; set; }
        public Single Commission { get; set; }
        public bool Apport { get; set; }
        public bool Gest { get; set; }
        public bool Payeur { get; set; }

        public static explicit operator Courtier(CourtierDto courtierDto)
        {
            Courtier courtier = ObjectMapperManager.DefaultInstance.GetMapper<CourtierDto, Courtier>().Map(courtierDto);
            courtier.Apport = courtierDto.CodeApporteur == courtierDto.CodeCourtier ? true : false;
            courtier.Gest = courtierDto.CodeGest == courtierDto.CodeCourtier ? true : false;
            courtier.Payeur = courtierDto.CodePayeur == courtierDto.CodeCourtier ? true : false;
            return courtier;
        }
        public static List<Courtier> LoadCourtiers(List<CourtierDto> courtiersDto)
        {
            var courtiers = new List<Courtier>();
            if (courtiersDto != null && courtiersDto.Count > 0)
                foreach (var courtierDto in courtiersDto)
                    courtiers.Add((Courtier)courtierDto);
            //Ordonner les apporteurs, gestionnaires et payeurs
            var courtiers1 = new List<Courtier>();
            foreach (var courtier in courtiers)
            {
                if (courtier.Apport)
                    courtiers1.Insert(0, courtier);
                else if (courtier.Gest)
                {
                    if (courtiers1.Count > 0)
                        courtiers1.Insert(1, courtier);
                    else courtiers1.Insert(0, courtier);
                }
                else if (courtier.Payeur)
                {
                    courtiers1.Insert(courtiers1.Count, courtier);
                }
            }
            //ordonner les autres courtiers par ordre decroissant de commissions
            var courtiers2 = new List<Courtier>();
            foreach (var courtier in courtiers)
            {
                courtiers2 = courtiers.OrderByDescending(c => c.Commission).Where(c=>!c.Apport && !c.Gest&& !c.Payeur).ToList();
            }
            //concaténation des deux listes
            courtiers1.AddRange(courtiers2);
            return courtiers1;
        }
    }
}