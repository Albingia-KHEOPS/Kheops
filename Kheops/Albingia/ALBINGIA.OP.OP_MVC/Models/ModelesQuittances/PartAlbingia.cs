using OP.WSAS400.DTO.Cotisations;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class PartAlbingia
    {
        public string ModeAffichage { get; set; }
        public string NumQuittanceVisu { get; set; }

        public double PartAlbingiaVal { get; set; }
        public List<Garanties> ListeGaranties { get; set; }

        public double CommissionTauxHCatnat { get; set; }
        public double CommissionTauxCatnat { get; set; }
        public double CommissionHorsCatnat { get; set; }
        public double CommissionCatnat { get; set; }
        public double CommissionTotal { get; set; }

        public double CoassFraisAperition { get; set; }
        public double CoassHTHorsCatnat { get; set; }
        public double CoassHTCatnat { get; set; }
        public double CoassHTTotal { get; set; }
        public double CoassCommHorsCatnat { get; set; }
        public double CoassCommCatnat { get; set; }
        public double CoassCommTotal { get; set; }

        public string NatureContrat { get; set; }

        public static explicit operator PartAlbingia(QuittanceVentilationAperitionDto dataDto)
        {
            PartAlbingia toReturn = new PartAlbingia();
            toReturn.ListeGaranties = new List<Garanties>();
            if (dataDto != null)
            {
                toReturn.PartAlbingiaVal = dataDto.PartAlbingia;
                toReturn.CommissionTauxHCatnat = dataDto.CommissionTauxHCatnat;
                toReturn.CommissionTauxCatnat = dataDto.CommissionTauxCatnat;
                toReturn.CommissionHorsCatnat = dataDto.CommissionValHCatnat;
                toReturn.CommissionCatnat = dataDto.CommissionValCatnat;
                toReturn.CommissionTotal = dataDto.CommissionTotal;
                toReturn.CoassFraisAperition = dataDto.FraisAperition;
                toReturn.CoassHTHorsCatnat = dataDto.CoassuranceHTHCatnat;
                toReturn.CoassHTCatnat = dataDto.CoassuranceHTCatnat;
                toReturn.CoassHTTotal = dataDto.CoassuranceHTTotal;
                toReturn.CoassCommHorsCatnat = dataDto.CoassuranceCommHCatnat;
                toReturn.CoassCommCatnat = dataDto.CoassuranceCommCatnat;
                toReturn.CoassCommTotal = dataDto.CoassuranceCommTotal;

                if (dataDto.ListeLignesGaranties != null && dataDto.ListeLignesGaranties.Any())
                {
                    dataDto.ListeLignesGaranties.ForEach(elm => toReturn.ListeGaranties.Add(
                        new Garanties
                        {
                            CodeGarantie = elm.Code,
                            LibelleGarantie = elm.Libelle,
                            HCatnat = elm.MainHCatnat,
                            Catnat = elm.MainCatnat,
                            Total = elm.MainTotal
                        }
                        ));
                }
            }

            return toReturn;
        }

    }
}