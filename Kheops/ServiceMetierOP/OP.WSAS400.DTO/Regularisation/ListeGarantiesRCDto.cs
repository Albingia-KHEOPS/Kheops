using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class ListeGarantiesRCDto
    {
        public ListeGarantiesRCDto()
        {
            Garanties = new List<SaisieGarantieRCValuesDto>();
        }

        [DataMember]
        public List<SaisieGarantieRCValuesDto> Garanties { get; set; }

        [DataMember]
        public double TotalAmount { get; set; }

        [DataMember]
        public bool FirstAccess { get; set; }

        public void AddRCFR(SaisieGarantieInfosDto rcfr)
        {
            if (rcfr == null)
            {
                return;
            }

            AddRC(rcfr, null);
        }

        public void AddRC(SaisieGarantieInfosDto rc, string defaultCode)
        {
            var previsionnel = new GarantieRCValuesDto
            {
                Id = rc?.Id ?? 0,
                BasicValues = new GarantieValuesDto
                {
                    Assiette = rc?.PrevAssiette ?? 0,
                    TauxMontant = rc?.PrevTaux ?? 0,
                    Unite = new UniteTauxMontantDto() { Code = rc?.PrevUnite },
                    CodeTaxes = new CodeTaxesDto() { Code = rc?.PrevCodTaxe },
                    IsAuto = rc?.TypeReguleGar == "A"
                },
                CalculAssiette = rc?.PrevMntHt ?? 0,
                CalculTaxesAssiette = rc?.PrevTax ?? 0,
                Coefficient = rc?.Coef ?? 0
            };

            var uniteDefinitif = new UniteTauxMontantDto();
            var codeTaxeDefinitif = new CodeTaxesDto();
            double tauxMontantDefinitif = 0;

            if (rc != null)
            {
                uniteDefinitif.Code = string.IsNullOrEmpty(rc.DefUnite) ? rc?.PrevUnite : rc?.DefUnite;
                codeTaxeDefinitif.Code = string.IsNullOrEmpty(rc.DefCodTaxe) ? rc?.PrevCodTaxe : rc?.DefCodTaxe;
                tauxMontantDefinitif = (rc.DefTaux != 0.0) ? rc.DefTaux : rc.PrevTaux;
            }



            var definitif = new GarantieRCValuesDto
            {
                Id = rc?.Id ?? 0,
                BasicValues = new GarantieValuesDto
                {
                    Assiette = rc?.DefAssiette ?? 0,
                    TauxMontant = tauxMontantDefinitif,
                    Unite = uniteDefinitif,
                    CodeTaxes = codeTaxeDefinitif,
                    IsAuto = rc?.TypeReguleGar == "A"
                },
                CalculAssiette = rc?.DefVmntHt ?? 0,
                CalculTaxesAssiette = rc?.DefVtax ?? 0,
                Label = rc?.Libelle ?? defaultCode,
                CotisationEmise = rc?.MntCotisEmis ?? 0,
                TaxesCotisationEmise = rc?.MntTxEmis ?? 0,
                Coefficient = rc?.Coef ?? 0,
                CodeGarantie = rc?.CodeGarantie ?? defaultCode
            };

            Garanties.Add(new SaisieGarantieRCValuesDto
            {
                IsRegulZero = rc?.Force0 == Booleen.Oui.AsCode(),
                IsZeroLocked = rc?.MotifInferieur == 1,
                IsReadOnly = rc?.IsReadOnlyRCUS == true,
                Previsionnel = previsionnel,
                Definitif = definitif,
                MotifInferieur = rc?.MotifInferieur ?? 0,
                Attentat = rc?.Attentat ?? 0,
                CotisationForcee = rc?.MntForceEmis ?? 0,
                TaxesCotisationForcee = rc?.MntForceTx ?? 0,
                RegulForcee = rc?.Force0 == Booleen.Non.AsCode() ? 0 : (rc?.MntForceCalc == rc?.MntRegulHt ? rc?.MntForceCalc ?? 0 : 0)
            });
        }

        public ListeGarantiesRCDto ComputingRefresh()
        {
            TotalAmount = 0D;
            foreach (var g in Garanties)
            {
                TotalAmount += g.ComputeRegulAmount();
            }

            return this;
        }
    }
}
