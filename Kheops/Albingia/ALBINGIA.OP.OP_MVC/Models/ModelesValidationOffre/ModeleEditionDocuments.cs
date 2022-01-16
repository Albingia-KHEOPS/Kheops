using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion;
using EmitMapper;
using OP.WSAS400.DTO.Validation;
using System.Collections.Generic;
using System.IO;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesValidationOffre
{
    public class ModeleEditionDocuments
    {
        public List<DocumentGestionDoc> ListeDocuments { get; set; }
        public List<ModeleEngagementTraite> EngagementsTraites { get; set; }

        public string CodeAffaire { get; set; }//numero offre/contrat
        public string VersionAffaire { get; set; }//Version offre/contrat
        public string TypeAffaire { get; set; }//offre ou contrat
        public string ModeEcran { get; set; }//validation ou edition
        public string ActeGestion { get; set; }
        public string RegulMode { get; set; }

        //Cotisations
        public double Cot100HTAvecCatNat { get; set; }
        public double Cot100CatNat { get; set; }
        public double Cot100TTC { get; set; }
        public double CotAlbHT { get; set; }
        public double CotAlbCatNat { get; set; }
        public double CotAlbTTC { get; set; }
        public string TraceEmiss { get; set; }

        public double CotReguleHT { get; set; }
        public double CotReguleCatNat { get; set; }
        public double CotReguleTTC { get; set; }
        public string TraceEmissRegule { get; set; }


        public static explicit operator ModeleEditionDocuments(ValidationEditionDto data)
        {
            var result = ObjectMapperManager.DefaultInstance.GetMapper<ValidationEditionDto, ModeleEditionDocuments>().Map(data);

            foreach (var docs in result.ListeDocuments)
            {
                foreach (var item in docs.ListDocInfos)
                {
                    if (!string.IsNullOrEmpty(item.NomDoc.Trim()))
                    {
                        var file = new FileInfo(item.NomDoc.Trim());
                        item.Extension = file.Extension;
                    }
                    item.IsModifiable = true;
                }
            }


            return result;
        }
    }
}