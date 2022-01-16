using ALBINGIA.Framework.Common.Extensions;
using EmitMapper;
using OP.WSAS400.DTO.FormuleGarantie;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    [Serializable]
    public class ModeleDetailsGarantie
    {
        public bool isReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the code garantie.
        /// </summary>
        /// <value>
        /// The code garantie.
        /// </value>
        public string CodeGarantie { get; set; }
        public string Garantie { get; set; }
        public string LibelleGarantie { get; set; }

        public string IsRegul { get; set; }
        public string LibGrilleRegul { get; set; }
        /// <summary>
        /// Gets or sets the caractere.
        /// </summary>
        /// <value>
        /// The caractere.
        /// </value>
        [Display(Name = "Caractère")]
        public string Caractere { get; set; }
        /// <summary>
        /// Gets or sets the nature STD.
        /// </summary>
        /// <value>
        /// The nature STD.
        /// </value>
        [Display(Name = "Nature standard")]
        public string NatureStd { get; set; }
        /// <summary>
        /// Gets or sets the nature.
        /// </summary>
        /// <value>
        /// The nature.
        /// </value>
        [Display(Name = "Nature retenue")]

        public string Nature { get; set; }
        /// <summary>
        /// Gets or sets the natures.
        /// </summary>
        /// <value>
        /// The natures.
        /// </value>
        public List<AlbSelectListItem> Natures { get; set; }
        /// <summary>
        /// Gets or sets the date deb STD.
        /// </summary>
        /// <value>
        /// The date deb STD.
        /// </value>

        [Display(Name = "Début")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateDebStd { get; set; }
        /// <summary>
        /// Gets or sets the heure deb STD.
        /// </summary>
        /// <value>
        /// The heure deb STD.
        /// </value>
        [Display(Name = "à")]
        public TimeSpan? HeureDebStd { get; set; }
        /// <summary>
        /// Gets or sets the date fin STD.
        /// </summary>
        /// <value>
        /// The date fin STD.
        /// </value>
        [Display(Name = "Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateFinStd { get; set; }
        /// <summary>
        /// Gets or sets the heure fin STD.
        /// </summary>
        /// <value>
        /// The heure fin STD.
        /// </value>
        [Display(Name = "à")]
        public TimeSpan? HeureFinStd { get; set; }
        /// <summary>
        /// Gets or sets the date deb.
        /// </summary>
        /// <value>
        /// The date deb.
        /// </value>
        [Display(Name = "Début")]
        public DateTime? DateDeb { get; set; }
        /// <summary>
        /// Gets or sets the heure deb.
        /// </summary>
        /// <value>
        /// The heure deb.
        /// </value>
        [Display(Name = "à")]
        public TimeSpan? HeureDeb { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [fin effet].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [fin effet]; otherwise, <c>false</c>.
        /// </value>
        public bool isFinEffet { get; set; }
        /// <summary>
        /// Gets or sets the date fin.
        /// </summary>
        /// <value>
        /// The date fin.
        /// </value>
        [Display(Name = "Fin")]
        public DateTime? DateFin { get; set; }
        /// <summary>
        /// Gets or sets the heure fin.
        /// </summary>
        /// <value>
        /// The heure fin.
        /// </value>
        [Display(Name = "à")]
        public TimeSpan? HeureFin { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is duree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is duree; otherwise, <c>false</c>.
        /// </value>
        public bool isDuree { get; set; }
        /// <summary>
        /// Gets or sets the duree.
        /// </summary>
        /// <value>
        /// The duree.
        /// </value>
        [Display(Name = "Durée")]
        public string Duree { get; set; }
        /// <summary>
        /// Gets or sets the duree unite.
        /// </summary>
        /// <value>
        /// The duree unite.
        /// </value>
        public string DureeUnite { get; set; }
        /// <summary>
        /// Gets or sets the duree unites.
        /// </summary>
        /// <value>
        /// The duree unites.
        /// </value>
        public List<AlbSelectListItem> DureeUnites { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [garantie indexee].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [garantie indexee]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Garantie indexée")]
        public bool GarantieIndexe { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModeleDetailsGarantiePage"/> is CATNAT.
        /// </summary>
        /// <value>
        ///   <c>true</c> if CATNAT; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Soumis CATNAT")]
        public bool CATNAT { get; set; }
        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        [Display(Name = "Application")]
        public string Application { get; set; }
        /// <summary>
        /// Gets or sets the applications.
        /// </summary>
        /// <value>
        /// The applications.
        /// </value>
        public List<AlbSelectListItem> Applications { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModeleDetailsGarantiePage"/> is LCI.
        /// </summary>
        /// <value>
        ///   <c>true</c> if LCI; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "LCI")]
        public bool LCI { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [montant ref].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [montant ref]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Incl. montant réf")]
        public bool InclusMontant { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModeleDetailsGarantiePage"/> is franchise.
        /// </summary>
        /// <value>
        ///   <c>true</c> if franchise; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Franchise")]
        public bool Franchise { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModeleDetailsGarantiePage"/> is assiette.
        /// </summary>
        /// <value>
        ///   <c>true</c> if assiette; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Assiette")]
        public bool Assiette { get; set; }
        /// <summary>
        /// Gets or sets the type emission.
        /// </summary>
        /// <value>
        /// The type emission.
        /// </value>
        [Display(Name = "Type d'émission")]
        public string TypeEmission { get; set; }
        public List<AlbSelectListItem> TypesEmission { get; set; }
        /// <summary>
        /// Gets or sets the taxe.
        /// </summary>
        /// <value>
        /// The taxe.
        /// </value>
        [Display(Name = "Code Taxe")]
        public string CodeTaxe { get; set; }
        /// <summary>
        /// Gets or sets the taxes.
        /// </summary>
        /// <value>
        /// The taxes.
        /// </value>
        public List<AlbSelectListItem> CodesTaxe { get; set; }
        /// <summary>
        /// Gets or sets the definition.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        [Display(Name = "Définition garantie")]
        public string Definition { get; set; }
        [Display(Name = "Alimentation")]
        public string AlimAssiette { get; set; }
        public List<AlbSelectListItem> AlimAssiettes { get; set; }
        public string TypeControleDate { get; set; }
        public bool AvnReadOnly { get; set; }
        public Int64 AvnCreation { get; set; }
        public string GarTemp { get; set; }

        public static explicit operator ModeleDetailsGarantie(FormuleGarantieDetailsDto DetailsDto)
        {
            var modeleDetailGaranties = ObjectMapperManager.DefaultInstance.GetMapper<FormuleGarantieDetailsDto, ModeleDetailsGarantie>().Map(DetailsDto);

            modeleDetailGaranties.isDuree = !string.IsNullOrEmpty(DetailsDto.Duree) ? true : false;
            modeleDetailGaranties.isFinEffet = !string.IsNullOrEmpty(DetailsDto.DateFin.ToString()) ? true : false;

            modeleDetailGaranties.DureeUnites.Clear();
            DetailsDto.DureeUnites.ToList().ForEach(
                elem => modeleDetailGaranties.DureeUnites.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            modeleDetailGaranties.Natures.Clear();
            DetailsDto.Natures.ToList().ForEach(
                elem => modeleDetailGaranties.Natures.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            modeleDetailGaranties.Applications.Clear();
            DetailsDto.Applications.ToList().ForEach(
                elem => modeleDetailGaranties.Applications.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            modeleDetailGaranties.TypesEmission.Clear();
            DetailsDto.TypesEmission.ToList().ForEach(
                elem => modeleDetailGaranties.TypesEmission.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            modeleDetailGaranties.CodesTaxe.Clear();
            DetailsDto.CodesTaxe.ToList().ForEach(
                elem => modeleDetailGaranties.CodesTaxe.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            modeleDetailGaranties.AlimAssiettes.Clear();
            DetailsDto.AlimAssiettes.ToList().ForEach(
                elem => modeleDetailGaranties.AlimAssiettes.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );

            return modeleDetailGaranties;
        }

        public static FormuleGarantieDetailsDto LoadDto(ModeleDetailsGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleDetailsGarantie, FormuleGarantieDetailsDto>().Map(modele);
        }

    }
}
