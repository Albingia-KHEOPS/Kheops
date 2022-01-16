using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.ParametreGaranties;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamGaranties {
    public class LigneGarantie
    {
        public string ModeOperation { get; set; }
        public string AdditionalParam { get; set; }
        public string CodeGarantie { get; set; }
        public string DesignationGarantie { get; set; }
        public string Abrege { get; set; }
        
        public string CodeTaxe { get; set; }
        public List<AlbSelectListItem> Taxes { get; set; }

        public string CodeCatNat { get; set; }
        public List<AlbSelectListItem> CatNats { get; set; }

        public bool IsGarantieCommune { get; set; }
        
        public string CodeTypeDefinition { get; set; }
        public List<AlbSelectListItem> TypesDefinition { get; set; }

        public string CodeTypeInformation { get; set; }
        public List<AlbSelectListItem> TypesInformation { get; set; }

        public bool IsRegularisable { get; set; }
        
        public string CodeTypeGrille { get; set; }
        public List<AlbSelectListItem> TypesGrille { get; set; }

        public bool IsLieInventaire { get; set; }
        public bool IsAttentatGareat { get; set; }

        public string CodeTypeInventaire { get; set; }
        public List<AlbSelectListItem> TypesInventaire { get; set; }

        public List<ModeleGarTypeRegul> GarTypeReguls { get; set; }
        public List<AlbSelectListItem> TypeReguls { get; set; }
        public string CodeTypeRegul { get; set; }

        public static explicit operator LigneGarantie(ParamGarantieDto paramGarantieDto)
        {
            var ligneGarantie = ObjectMapperManager.DefaultInstance.GetMapper<ParamGarantieDto, LigneGarantie>().Map(paramGarantieDto);
            ligneGarantie.IsGarantieCommune = paramGarantieDto.GarantieCommune.AsBoolean() ?? false;
            ligneGarantie.IsRegularisable = paramGarantieDto.Regularisable.AsBoolean() ?? false;
            ligneGarantie.IsLieInventaire = paramGarantieDto.Inventaire.AsBoolean() ?? false;
            ligneGarantie.IsAttentatGareat = paramGarantieDto.AttentatGareat.AsBoolean() ?? false;
            return ligneGarantie;
        }
    }
}