using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VisualisationQuittancesLigne
    {
        public bool IsModeAnnulation { get; set; }
        public bool IsAnnulable { get; set; }
        public bool IsAnnulee { get; set; }
        public DateTime? Emission { get; set; }
        public string NumQuittance { get; set; }
        public string Avis { get; set; }
        public DateTime? DateDeb { get; set; }
        public DateTime? DateFin { get; set; }

        public DateTime? Echeance { get; set; }
        public Int32 Avenant { get; set; }
        public string DemCode { get; set; }
        public string DemLibelle { get; set; }
        public string MvtCode { get; set; }
        public string MvtLibelle { get; set; }
        public int OpeCode { get; set; }
        public string OpeLibelle { get; set; }
        public string SitCode { get; set; }
        public string SitLibelle { get; set; }
        public double HT { get; set; }
        public double TTC { get; set; }
        public double Regle { get; set; }
        public Int32 CurrentCodeAvt { get; set; }
        public string IsCheck { get; set; }
        public bool IsReadOnly { get; set; }
        public bool DisplayEditionQuittance { get; set; }
        public bool IsModifHorsAvenant { get; set; }
        public string Etat { get; set; }

        public static explicit operator VisualisationQuittancesLigne(QuittanceVisualisationLigneDto dataDto)
        {
            VisualisationQuittancesLigne toReturn = ObjectMapperManager.DefaultInstance.GetMapper<QuittanceVisualisationLigneDto, VisualisationQuittancesLigne>().Map(dataDto);

            if (dataDto.DateDebAnnee > 0 && dataDto.DateDebMois > 0 && dataDto.DateDebJours > 0)
                toReturn.DateDeb = new DateTime(dataDto.DateDebAnnee, dataDto.DateDebMois, dataDto.DateDebJours);
            else
                toReturn.DateDeb = null;

            if (dataDto.DateFinAnnee > 0 && dataDto.DateFinMois > 0 && dataDto.DateFinJours > 0)
                toReturn.DateFin = new DateTime(dataDto.DateFinAnnee, dataDto.DateFinMois, dataDto.DateFinJours);
            else
                toReturn.DateFin = null;

            if (dataDto.EcheanceAnnee > 0 && dataDto.EcheanceMois > 0 && dataDto.EcheanceJour > 0)
                toReturn.Echeance = new DateTime(dataDto.EcheanceAnnee, dataDto.EcheanceMois, dataDto.EcheanceJour);
            else
                toReturn.Echeance = null;

            if (dataDto.EmissionAnnee > 0 && dataDto.EmissionMois > 0 && dataDto.EmissionJour > 0)
                toReturn.Emission = new DateTime(dataDto.EmissionAnnee, dataDto.EmissionMois, dataDto.EmissionJour);
            else
                toReturn.Emission = null;
            toReturn.IsAnnulable = dataDto.SitCode == "A";
            toReturn.IsAnnulee = dataDto.IsAnnule == "O";
            return toReturn;
        }
    }
}