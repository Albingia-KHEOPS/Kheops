using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTransverse
{
    [Serializable]
    public class ModeleCommonCabinetPreneur
    {
        public string TitreBoiteDialog { get; set; }

        public string Code { get; set; }
        public string Nom { get; set; }
        public string[] NomSecondaires { get; set; }
        public string Departement { get; set; }
        public string Ville { get; set; }
        public string VilleCedex { get; set; }
        public string SIREN { get; set; }

        public string NumeroExt { get; set; }
        public string Batiment { get; set; }
        public string Rue { get; set; }
        public string CodePostal { get; set; }
        public string CodePostalCedex { get; set; }
        public string CodePostalString { get; set; }
        public string Distribution { get; set; }
        public string Type { get; set; }
        public string TelephoneBureau { get; set; }
        public string EmailBureau { get; set; }

        public string Delegation { get; set; }
        public bool EstValide { get; set; }

        public string CodeDelegation { get; set; }
        public string NomDelegation { get; set; }
        public bool Sinistre { get; set; }
        public bool RetardsPaiements { get; set; }
        public bool DemarcheCom { get; set; }
        public bool EstActif { get; set; }

        #region Inspecteur
        public string NomInspecteur { get; set; }
        public string PrenomInspecteur { get; set; }
        #endregion
        #region Info générale
        public string EncaissementCode { get; set; }
        public string EncaissementLibelle { get; set; }
        #endregion
        #region Interlocuteur
        public string NomInterlocuteur { get; set; }
        public string PrenomInterlocuteur { get; set; }
        public string IdInterlocuteur { get; set; }
        public bool ValideInterlocuteur { get; set; }
        public string FonctionInterlocuteur { get; set; }
        public string TelephoneInterlocuteur { get; set; }
        public string MailInterlocuteur { get; set; }
        public bool Impayes { get; set; }
        #endregion

        public static explicit operator ModeleCommonCabinetPreneur(AssureDto preneurAssuranceDto)
        {
            var modeleAssuree = new ModeleCommonCabinetPreneur
            {
                Code = preneurAssuranceDto.Code,
                Nom = preneurAssuranceDto.NomAssure,
                NomSecondaires = preneurAssuranceDto.NomSecondaires.ToArray(),
                Ville = (preneurAssuranceDto.Adresse != null ? preneurAssuranceDto.Adresse.NomVille : string.Empty),
                CodePostal = (preneurAssuranceDto.Adresse != null ? preneurAssuranceDto.Adresse.CodePostal.ToString() : string.Empty),
                CodePostalString = (preneurAssuranceDto.Adresse != null ? preneurAssuranceDto.Adresse.CodePostalString : string.Empty),
                Departement = (preneurAssuranceDto.Adresse != null &&
                               !string.IsNullOrEmpty(preneurAssuranceDto.Adresse.CodePostalString) &&
                               preneurAssuranceDto.Adresse.CodePostalString.Length >= 2 ? preneurAssuranceDto.Adresse.CodePostalString.Substring(0, 2) : string.Empty),
                //Departement = (preneurAssuranceDto.Adresse != null &&
                //               !string.IsNullOrEmpty(preneurAssuranceDto.Adresse.CodePostal.ToString()) &&
                //               preneurAssuranceDto.Adresse.CodePostal.ToString().Length >= 2 ? preneurAssuranceDto.Adresse.CodePostal.ToString().Substring(0, 2) : string.Empty),
                SIREN = (preneurAssuranceDto.Siren != 0 ? preneurAssuranceDto.Siren.ToString() : string.Empty),
                Batiment = ((preneurAssuranceDto.Adresse != null) ? preneurAssuranceDto.Adresse.Batiment : string.Empty),
                NumeroExt = ((preneurAssuranceDto.Adresse != null) ? preneurAssuranceDto.Adresse.ExtensionVoie : string.Empty),
                Distribution = ((preneurAssuranceDto.Adresse != null) ? preneurAssuranceDto.Adresse.BoitePostale : string.Empty),
                CodePostalCedex = ((preneurAssuranceDto.Adresse != null) ? preneurAssuranceDto.Adresse.CodePostalCedex.ToString() : string.Empty),
                VilleCedex = ((preneurAssuranceDto.Adresse != null) ? preneurAssuranceDto.Adresse.NomCedex : string.Empty),
                TelephoneBureau = preneurAssuranceDto.TelephoneBureau,
                Sinistre = preneurAssuranceDto.NbSinistres > 0,
                RetardsPaiements = preneurAssuranceDto.RetardsPaiements,
                Impayes = preneurAssuranceDto.Impayes,
                EstActif = preneurAssuranceDto.EstActif
            };
            return modeleAssuree;
        }

        public static explicit operator ModeleCommonCabinetPreneur(AssurePlatDto preneurAssuranceDto)
        {
            var modeleAssuree = new ModeleCommonCabinetPreneur
            {
                Code = preneurAssuranceDto.Code.ToString(),
                Nom = preneurAssuranceDto.NomAssure,
                NomSecondaires = new string[] { preneurAssuranceDto.NomSecondaire },
                Ville = preneurAssuranceDto.NomVille,
                CodePostal = preneurAssuranceDto.CodePostal.ToString(),
                Departement = (!string.IsNullOrEmpty(preneurAssuranceDto.CodePostal.ToString()) &&
                               preneurAssuranceDto.CodePostal.ToString().Length >= 2 ? preneurAssuranceDto.CodePostal.ToString().Substring(0, 2) : string.Empty),
                SIREN = (preneurAssuranceDto.Siren != 0 ? preneurAssuranceDto.Siren.ToString() : string.Empty),
                Batiment = preneurAssuranceDto.Batiment,
                NumeroExt = preneurAssuranceDto.ExtensionVoie,
                Distribution = preneurAssuranceDto.BoitePostale,
                CodePostalCedex = preneurAssuranceDto.CodePostalCedex.ToString(),
                VilleCedex = preneurAssuranceDto.NomCedex,
                TelephoneBureau = preneurAssuranceDto.TelephoneBureau,
                Sinistre = preneurAssuranceDto.NombreSinistres > 0,
                RetardsPaiements = preneurAssuranceDto.Retards == 1,
                Impayes = preneurAssuranceDto.Impayes == 1,
                EstActif = preneurAssuranceDto.EstActif,
            };
            return modeleAssuree;
        }

        public static explicit operator ModeleCommonCabinetPreneur(CabinetCourtageDto cabinetCourtageDto)
        {
            var modeleCabinet = new ModeleCommonCabinetPreneur
            {
                Code = cabinetCourtageDto.Code.ToString(),
                Nom = cabinetCourtageDto.NomCabinet,
                Type = cabinetCourtageDto.Type,
                EstValide = cabinetCourtageDto.EstValide,
                ValideInterlocuteur = cabinetCourtageDto.ValideInterlocuteur,
                NomSecondaires = cabinetCourtageDto.NomSecondaires.ToArray(),
                CodePostal = ((cabinetCourtageDto.Adresse != null) ? cabinetCourtageDto.Adresse.CodePostal.ToString() : string.Empty),
                Ville = (cabinetCourtageDto.Adresse != null ? cabinetCourtageDto.Adresse.NomVille : string.Empty),
                Delegation = (cabinetCourtageDto.Delegation != null ? cabinetCourtageDto.Delegation.Nom : string.Empty),
                Batiment = ((cabinetCourtageDto.Adresse != null) ? cabinetCourtageDto.Adresse.Batiment : string.Empty),
                NumeroExt = ((cabinetCourtageDto.Adresse != null) ? cabinetCourtageDto.Adresse.ExtensionVoie : string.Empty),
                Distribution = ((cabinetCourtageDto.Adresse != null) ? cabinetCourtageDto.Adresse.BoitePostale : string.Empty),
                CodePostalCedex = ((cabinetCourtageDto.Adresse != null) ? cabinetCourtageDto.Adresse.CodePostalCedex.ToString() : string.Empty),
                VilleCedex = ((cabinetCourtageDto.Adresse != null) ? cabinetCourtageDto.Adresse.NomCedex : string.Empty),
                TelephoneBureau = cabinetCourtageDto.TelephoneBureau,
                EmailBureau = cabinetCourtageDto.EmailBureau,
                EncaissementCode = cabinetCourtageDto.CodeEncaissement,
                EncaissementLibelle = cabinetCourtageDto.LibEncaissement,
                NomInterlocuteur = cabinetCourtageDto.NomInterlocuteur,
                FonctionInterlocuteur = cabinetCourtageDto.FonctionInterlocuteur,
                TelephoneInterlocuteur = cabinetCourtageDto.TelephoneInterlocuteur,
                MailInterlocuteur = cabinetCourtageDto.EmailInterlocuteur,
                NomInspecteur = cabinetCourtageDto.Inspecteur,
                DemarcheCom = cabinetCourtageDto.DemarcheCom
            };
            return modeleCabinet;
        }

        public static explicit operator ModeleCommonCabinetPreneur(InterlocuteurDto interlocuteurDto)
        {
            var modeleInterlocuteur = new ModeleCommonCabinetPreneur
            {
                Nom = interlocuteurDto.CabinetCourtage.NomCabinet,
                Code = interlocuteurDto.CabinetCourtage.Code.ToString(),
                Type = interlocuteurDto.CabinetCourtage.Type,
                NomInterlocuteur = interlocuteurDto.Nom,
                IdInterlocuteur = interlocuteurDto.Id.ToString(),
                ValideInterlocuteur = interlocuteurDto.EstValide,
                CodePostal = interlocuteurDto.CabinetCourtage.Adresse.CodePostal.ToString(),
                Ville = interlocuteurDto.CabinetCourtage.Adresse.NomVille,
                EstValide = interlocuteurDto.CabinetCourtage.EstValide,
                CodeDelegation = interlocuteurDto.CabinetCourtage.Delegation != null ? interlocuteurDto.CabinetCourtage.Delegation.Code : string.Empty,
                NomDelegation = interlocuteurDto.CabinetCourtage.Delegation != null ? interlocuteurDto.CabinetCourtage.Delegation.Nom : string.Empty,
                Delegation = interlocuteurDto.CabinetCourtage.Delegation != null ? interlocuteurDto.CabinetCourtage.Delegation.Nom : string.Empty,
                DemarcheCom = interlocuteurDto.CabinetCourtage.DemarcheCom
            };
            return modeleInterlocuteur;
        }
    }
}