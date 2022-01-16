using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmitMapper;
using ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData;
using ALBINGIA.OP.OP_MVC.WSPoliceServices;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    [Serializable]
    public class ModelePreneurAssurance
    {
        public string Code { get; set; }
        public string Nom { get; set; }
        public string[] NomSecondaires { get; set; }
        public string Departement { get; set; }
        public string Ville { get; set; }
        public string SIREN { get; set; }          

        public static explicit operator ModelePreneurAssurance(PreneurAssurance_JSON_MetaData preneurAssuranceDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<PreneurAssurance_JSON_MetaData, ModelePreneurAssurance>().Map(preneurAssuranceDto);
        }

        public static explicit operator ModelePreneurAssurance(AssureDto preneurAssuranceDto)
        {
            var modeleAssuree = new ModelePreneurAssurance
                        {
                            Code = preneurAssuranceDto.Code,
                            Nom = preneurAssuranceDto.NomAssure,
                            NomSecondaires = preneurAssuranceDto.NomSecondaires.ToArray(),
                            Ville = ((preneurAssuranceDto.Adresse != null && preneurAssuranceDto.Adresse.Ville != null) ? preneurAssuranceDto.Adresse.Ville.Nom : string.Empty),
                            Departement = (preneurAssuranceDto.Adresse != null &&
                                           preneurAssuranceDto.Adresse.Ville != null &&
                                           !string.IsNullOrEmpty(preneurAssuranceDto.Adresse.Ville.CodePostal) &&
                                           preneurAssuranceDto.Adresse.Ville.CodePostal.Length >= 2 ? preneurAssuranceDto.Adresse.Ville.CodePostal.Substring(0, 2) : string.Empty),
                            SIREN = (preneurAssuranceDto.Siren != 0 ? preneurAssuranceDto.Siren.ToString() : string.Empty)
                        };
            return modeleAssuree;
        }
       
    }
}