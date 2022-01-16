using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using OP.WSAS400.DTO.Avenant;
using System;

namespace OP.WSAS400.DTO
{
    public class HistorizationContext
    {

        public HistorizationContext(IAvenantDto avenant = null)
        {
            if (avenant != null)
            {
                TypeAvenant = avenant.Type;
                NumeroAvenantInterne = (int)avenant.NumInterne;
                MotifAvenant = avenant.Motif;
                DateAvenant = avenant.Type == AlbConstantesMetiers.TYPE_AVENANT_RESIL ? avenant.Date.Value.Date.AddDays(1) : avenant.Date;
                InitDateFinGarantie(avenant);
                if (avenant.Type == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR)
                {
                    var dateFinREV = ((AvenantRemiseEnVigueurDto)avenant).DateFinEffet;
                    var heureFinREV = ((AvenantRemiseEnVigueurDto)avenant).HeureFinEffet;
                    if (dateFinREV.HasValue)
                    {
                        DateFinEffetREV = new DateTime(dateFinREV.Value.Year, dateFinREV.Value.Month, dateFinREV.Value.Day, heureFinREV.HasValue ? heureFinREV.Value.Hours : 0, heureFinREV.HasValue ? heureFinREV.Value.Minutes : 0, 0);
                    }
                    NumPrime = ((AvenantRemiseEnVigueurDto)avenant).PrimeReglee;
                }
                Description = avenant.Description;
                Observations = avenant.Observations;
                DefineArchiving(avenant);
                IsModifHorsAvenant = avenant.IsModifHorsAvenant;
            }

            Now = DateTime.Now;
        }

        public DateTime Now { get; private set; }
        public Folder Folder { get; set; }
        public char Mode { get; set; }
        public string MotifAvenant { get; set; }
        public int NumeroAvenantInterne { get; set; }
        public DateTime? DateAvenant { get; set; }
        public string TypeAvenant { get; set; }
        public string User { get; set; }
        public string Description { get; set; }
        public string Observations { get; set; }
        public DateTime? DateFinGarantie { get; set; }
        public DateTime? DateFinEffetREV { get; set; }
        public Int32 NumPrime { get; set; }
        public bool IsModifHorsAvenant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the historization context is archiving Avenant create/update
        /// </summary>
        public bool IsArchived { get; set; }

        private void InitDateFinGarantie(IAvenantDto avenant)
        {
            if (avenant is AvenantRemiseEnVigueurDto rmv)
            {
                DateFinGarantie = rmv.DateResil + rmv.HeureResil;
            }
            else if (avenant is AvenantResiliationDto rs) {
                DateFinGarantie = rs.DateFinGarantie + rs.HeureFinGarantie;
            }
            else
            {
                DateFinGarantie = null;
            }
        }

        private void DefineArchiving(IAvenantDto avenant)
        {
            IsArchived = true;
            if (avenant is AvenantRegularisationDto regulAvn)
            {
                IsArchived = regulAvn.HasHisto;
            }
        }
    }
}
