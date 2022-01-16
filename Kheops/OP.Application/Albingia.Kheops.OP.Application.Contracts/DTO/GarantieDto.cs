using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO
{
    public class GarantieDto
    {
        public long Id { get; set; }

        public long Sequence { get; set; }

        public string Libelle { get; set; }

        public List<GarantieDto> SousGaranties { get; set; }

        public CaractereSelection Caractere { get; set; }

        public NatureValue Nature { get; set; }

        public NatureValue NatureRetenue { get; set; }

        public AlimentationValue TypeAlimentation { get; set; }

        public string CodeBloc { get; set; }

        public int? NumeroAvenant { get; set; }
        public int NumeroAvenantCreation { get; set; }
        public int NumeroAvenantModif { get; set; }

        public int AvenantInitial { get; set; }

        public int AvenantMAJ { get; set; }

        public string CodeGarantie { get; set; }

        public string DesignationGarantie { get; set; }

        public string Abrege { get; set; }

        public string CodeTaxe { get; set; }

        public string CodeCatNat { get; set; }

        public bool? GarantieCommune { get; set; }

        public bool IsFlagModifie { get; set; }

        public bool IsHidden { get; set; }

        public bool InventairePossible { get; set; }

        public string CodeTypeDefinition { get; set; }

        public string CodeTypeInformation { get; set; }

        public string Regularisable { get; set; }

        public string CodeTypeGrille { get; set; }

        public int Niveau { get; set; }

        public string CodeTypeInventaire { get; set; }

        public bool IsChecked { get; set; }
        
        public bool HasPortees { get; set; }

        public Dictionary<string, string> ActionsPortees { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateSortie { get; set; }

        public long IdInventaire { get; set; }

        public TypeInventaireDto TypeInventaire { get; set; }

        public bool? ParamIsNatModifiable { get; set; }

        public ICollection<PorteeGarantieDto> Portees { get; set; }

        public int Formule { get; set; }

        public decimal PrimeProvisionnelle { get; set; }
        public decimal Prime { get; set; }

        public bool HasPrime => PrimeProvisionnelle > 0 || Prime > 0;

        //public TarifGarantieDto Tarif { get; set; }
    }
}