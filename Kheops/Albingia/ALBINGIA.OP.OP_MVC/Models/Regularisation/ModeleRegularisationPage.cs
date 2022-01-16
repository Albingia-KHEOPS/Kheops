using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation;
using ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegulGar;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.Regularisation
{
    public class ModeleRegularisationPage : MetaModelsBase, IRegulModel
    {
        public ModeleRegularisationPage()
        {
            TypeReguls = new List<AlbSelectListItem> ();
        }
        public DateTime? EffetGaranties { get; set; }
        public DateTime? FinEffet { get; set; }
        public TimeSpan? FinEffetHeure { get; set; }
        public DateTime? Echeance { get; set; }
        public string AvnMode { get; set; }

        public List<ModeleAvenantAlerte> Alertes { get; set; }
        public List<ModeleLigneRegularisation> Regularisations { get; set; }

        public ModeleCreationRegule InfoRegule { get; set; }
        public ModeleRisquesRegularisation ModelRisques { get; set; }
        public ModeleGarantiesRegularisation ModelGaranties { get; set; }
        public RegularisationGarInfo GarInfo { get; set; }

        public ModeleSaisieRegulGarantiePage SaisieRegulGarantiePage { get; set; }
        public List<AlbSelectListItem> TypeReguls { get; set; }
        public string CodeTypeRegul { get; set; }

        public string LibTypeContrat { get; set; }

        public bool IsValidRegul { get; set; }

        //Determine si un contrat est de type temporaire ( periodicitée != de A, S, T ou R )
        public bool IsTempo { get; set; }

        public RegularisationContext Context { get; set; }

        public IdContratDto IdContrat
        {
            get { return new IdContratDto { CodeOffre = CodePolicePage, Version = Int32.Parse(VersionPolicePage), Type = TypePolicePage }; }
        }

        public long RgId
        {
            get
            {
                if (InfoRegule != null)
                {
                    return InfoRegule.ReguleId;
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
                if (InfoRegule != null)
                {
                    return InfoRegule.LotId;
                }
                if (GarInfo != null)
                {
                    return Int64.Parse(GarInfo.idLot);
                }
                Int64.TryParse(InformationGeneraleTransverse.GetAddParamValue(AddParamValue, AlbParameterName.LOTID), out long id);
                return id;
            }
        }
        public RegularisationBandeauContratDto BandeauContrat { get; set; } //BandeauContratRegularisation
    }
}