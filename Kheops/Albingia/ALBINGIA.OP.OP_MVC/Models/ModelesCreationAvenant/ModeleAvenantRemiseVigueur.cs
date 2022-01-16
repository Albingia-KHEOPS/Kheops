using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.Avenant;
using System;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAvenant
{
    public class ModeleAvenantRemiseVigueur
    {
        public string ErrorAvt { get; set; }
        public string TypeAvt { get; set; }
        public string ModeAvt { get; set; }
        public string LibelleAvt { get; set; }
        public Int32 NumInterneAvt { get; set; }
        public Int32 NumAvt { get; set; }

        public string DescriptionAvt { get; set; }
        public string ObservationsAvt { get; set; }

        public DateTime? DateResil { get; set; }
        public TimeSpan? HeureResil { get; set; }

        public DateTime? DateRemiseVig { get; set; }
        public TimeSpan? HeureRemiseVig { get; set; }
        public List<AlbSelectListItem> TypesGestion { get; set; }
        public string TypeGestion { get; set; }

        public Int32 PrimeReglee { get; set; }
        public DateTime? PrimeReglementDate { get; set; }

        public bool IsModifHorsAvenant { get; set; }
        public DateTime? DateSuspDeb { get; set; }
        public TimeSpan? HeureSuspDeb { get; set; }


        public DateTime? DateSuspFin { get; set; }
        public TimeSpan? HeureSuspFin { get; set; }

        public DateTime? DateDebNonSinistre { get; set; }
        public DateTime? DateFinNonSinistre { get; set; }

        public TimeSpan? HeureDebNonSinistre { get; set; }
        public TimeSpan? HeureFinNonSinistre { get; set; }

        public DateTime? ProchaineEchHisto { get; set; }
        public string Etat { get; set; }
        public string Situation { get; set; }
        public string Periodicite { get; set; }

        public DateTime? DateFinEffet { get; set; }
        public TimeSpan? HeureFinEffet { get; set; }


        public static explicit operator ModeleAvenantRemiseVigueur(AvenantRemiseEnVigueurDto modeleDto)
        {
            var model = ObjectMapperManager.DefaultInstance.GetMapper<AvenantRemiseEnVigueurDto, ModeleAvenantRemiseVigueur>().Map(modeleDto);
            var typesGestion = new List<AlbSelectListItem>
            {
                new AlbSelectListItem { Text="V - en vigueur seule", Descriptif = "V - en vigueur seule", Title = "en vigueur seule", Value = "V"},
                new AlbSelectListItem { Text="M - avec modification", Descriptif = "M - avec modification", Title = "avec modification", Value = "M"}
            };

            model.TypesGestion = typesGestion;
            return model;
        }

        public static AvenantRemiseEnVigueurDto LoadDto(ModeleAvenantRemiseVigueur modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAvenantRemiseVigueur, AvenantRemiseEnVigueurDto>().Map(modele);
        }
    }
}