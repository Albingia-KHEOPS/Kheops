using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Offres.Risque;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRisque
{
    public class ModeleOpposition
    {
        //GuidId
        public Int64 GuidId { get; set; }
        public string Mode { get; set; }

        //Risque
        public string LibelleRisque { get; set; }
        public List<AlbSelectListItem> ObjetsRisque { get; set; }
        public bool AppliqueAuRisqueEntier { get; set; }

        //Type Financement
        public List<AlbSelectListItem> TypesFinancement { get; set; }
        public string TypeFinancement { get; set; }
        public string TypeFinancementLabel { get; set; }

        //Organisme de l'opposition
        public int? CodeOrganisme { get; set; }
        public string NomOrganisme { get; set; }
        public string CPOrganisme { get; set; }
        public string VilleOrganisme { get; set; }
        public string PaysOrganisme { get; set; }
        public string NomPays { get; set; }

        //Opposition
        public string Description { get; set; }
        public string Reference { get; set; }
        public DateTime? Echeance { get; set; }
        public Double Montant { get; set; }
        public Int64 KDESIRef { get; set; }
        public string AppliqueALabel { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }  

        public Int64 LienFichierOrigine { get; set; }
        public string NomOrganismeWarning { get; set; }            
        public string Adresse1Warning { get; set; }      
        public string Adresse2Warning { get; set; }      
        public string CPOrganismeWarning { get; set; }      
        public string VilleWarning { get; set; }      
        public string CodePaysWarning { get; set; }
        public string NomPaysWarning { get; set; }

        public List<AlbSelectListItem> ListTypesDest { get; set; }
        public string TypeDest { get; set; }
        public string TypeInterv { get; set; }

        public static explicit operator ModeleOpposition(OppositionDto data)
        {
            ModeleOpposition toReturn = ObjectMapperManager.DefaultInstance.GetMapper<OppositionDto, ModeleOpposition>().Map(data);
            if (data.ObjetsRisque != null)
                toReturn.ObjetsRisque = data.ObjetsRisque.Select(m => new AlbSelectListItem { Value = m.Code }).ToList();
            return toReturn;
        }

    }
}