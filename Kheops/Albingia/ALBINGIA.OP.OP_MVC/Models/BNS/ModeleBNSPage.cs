using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.Regularisation;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegulGar;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.BNS
{
    public class ModeleBNSPage : MetaModelsBase, IRegulModel
    {

        public DateTime? EffetGaranties { get; set; }
        public DateTime? FinEffet { get; set; }
        public TimeSpan? FinEffetHeure { get; set; }
        public DateTime? Echeance { get; set; }
        public string AvnMode { get; set; }

        public List<ModeleAvenantAlerte> Alertes { get; set; }
        public List<ModeleLigneRegularisation> Regularisations { get; set; }

        public ModeleCreationRegule InfoBNS { get; set; }
        public ModeleRisquesRegularisation ModelRisques { get; set; }
        public ModeleGarantiesRegularisation ModelGaranties { get; set; }
        public RegularisationGarInfo GarInfo { get; set; }

        public string LibTypeContrat { get; set; }

        public bool IsValidRegul { get; set; }
        public bool HasBNS { get; set; }

        //Determine si un contrat est de type temporaire ( periodicitée != de A, S, T ou R )
        public bool IsTempo { get; set; }

        // Propriété venant du IRegulMode
        public RegularisationContext Context { get; set; }
        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public long RgId
        {
            get
            {
                if (InfoBNS != null)
                {
                    return InfoBNS.ReguleId;
                }
                if (GarInfo != null)
                {
                    return Int64.Parse(GarInfo.idregul);
                }

                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.REGULEID), out long id);
                return id;

            }
        }

        public long LotId
        {
            get
            {
                if (InfoBNS != null)
                {
                    return InfoBNS.LotId;
                }
                if (GarInfo != null)
                {
                    return Int64.Parse(GarInfo.idLot);
                }
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.LOTID), out long id);
                return id;
            }
        }

    }
}