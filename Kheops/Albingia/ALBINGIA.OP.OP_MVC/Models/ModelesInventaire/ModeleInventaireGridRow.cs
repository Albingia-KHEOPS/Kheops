using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using EmitMapper;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesInventaire
{
    [Serializable]
    public class ModeleInventaireGridRow
    {
        public Int64 InventaireType { get; set; }
        public int Code { get; set; }
        public string Designation { get; set; }
        public int LienADH { get; set; }
        public string Lieu { get; set; }
        public int CodePostal { get; set; }
        public string Ville { get; set; }
        public DateTime? DateDeb { get; set; }
        public string DateDebStr
        {
            get
            {

                return DateDeb.HasValue ? string.Format("{0}/{1}/{2}", DateDeb.Value.Day.ToString().PadLeft(2, '0'), DateDeb.Value.Month.ToString().PadLeft(2, '0'), DateDeb.Value.Year) : string.Empty;
            }
        }
        public TimeSpan? HeureDeb { get; set; }
        public string HeureDebStr
        {
            get
            {
                return HeureDeb.HasValue ? string.Format("{0}:{1}", HeureDeb.Value.Hours.ToString().PadLeft(2, '0'), HeureDeb.Value.Minutes.ToString().PadLeft(2, '0')) : string.Empty;
            }
        }
        public DateTime? DateFin { get; set; }
        public string DateFinStr
        {
            get
            {
                return DateFin.HasValue ? string.Format("{0}/{1}/{2}", DateFin.Value.Day.ToString().PadLeft(2, '0'), DateFin.Value.Month.ToString().PadLeft(2, '0'), DateFin.Value.Year) : string.Empty;
            }
        }
        public TimeSpan? HeureFin { get; set; }
        public string HeureFinStr
        {
            get
            {
                return HeureDeb.HasValue ? string.Format("{0}:{1}", HeureFin.Value.Hours.ToString().PadLeft(2, '0'), HeureFin.Value.Minutes.ToString().PadLeft(2, '0')) : string.Empty;
            }
        }
        public int NbPers { get; set; }
        public int NbEvenement { get; set; }
        public decimal Montant { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Fonction { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string AnneeNaissance { get; set; }
        public string DateNaissanceStr
        {
            get
            {
                return DateNaissance.HasValue ? string.Format("{0}/{1}/{2}", DateNaissance.Value.Day.ToString().PadLeft(2, '0'), DateNaissance.Value.Month.ToString().PadLeft(2, '0'), DateNaissance.Value.Year) : string.Empty;
            }
        }
        public Int64 CapitalDeces { get; set; }
        public Int64 CapitalIP { get; set; }
        public bool AccidentSeul { get; set; }
        public bool AvantProd { get; set; }
        public string NumSerie { get; set; }
        public string Description { get; set; }
        public string NatureLieu { get; set; }
        public string DescNatLieu { get; set; }
        public List<AlbSelectListItem> NaturesLieu { get; set; }
        public string CodeMateriel { get; set; }
        public string DescMat { get; set; }
        public List<AlbSelectListItem> CodesMateriel { get; set; }
        public string CodeExtension { get; set; }
        public string DescExtension { get; set; }
        public List<AlbSelectListItem> CodesExtension { get; set; }
        public string Franchise { get; set; }
        public string CodeQualite { get; set; }
        public string DescQualite { get; set; }
        public List<AlbSelectListItem> CodesQualite { get; set; }
        public string CodeRenonce { get; set; }
        public string DescRenonce { get; set; }
        public List<AlbSelectListItem> CodesRenonce { get; set; }
        public string CodeRsqLoc { get; set; }
        public string DescRsqLoc { get; set; }
        public List<AlbSelectListItem> CodesRsqLoc { get; set; }

        public decimal Mnt2 { get; set; }
        public decimal Contenu { get; set; }
        public decimal MatBur { get; set; }

        public string Modele { get; set; }
        public string Marque { get; set; }
        public string Immatriculation { get; set; }

        public int NatureMarchandise { get; set; }
        public string DescNatureMarchandise { get; set; }

        public int Depart { get; set; }
        public string DescDepart { get; set; }

        public int Destination { get; set; }
        public string DescDestination { get; set; }

        public int Modalite { get; set; }
        public string DescModalite { get; set; }

        public string  Pays { get; set; }
        public string DescPays { get; set; }
        public List<AlbSelectListItem> ListPays { get; set; }
        public ModeleContactAdresse Adresse { get; set; }

        public ModeleInventaireGridRow()
        {
            Code = -9999;
            Adresse = new ModeleContactAdresse();
        }

        public static explicit operator ModeleInventaireGridRow(InventaireGridRowDto inventaireGridRowDto)
        {
            var toReturn = ObjectMapperManager.DefaultInstance.GetMapper<InventaireGridRowDto, ModeleInventaireGridRow>().Map(inventaireGridRowDto);
            if (inventaireGridRowDto.Adresse != null)
            {
                int depart = 0;
                Int32.TryParse(inventaireGridRowDto.Adresse.Departement, out depart);
                string codePostal = string.Empty;
                string codePX = string.Empty;
                //string codePostal = depart.ToString("D2") + currentObj.AdresseObjet.CodePostal.ToString("D3");
                //string codePX = depart.ToString("D2") + currentObj.AdresseObjet.CodePostalCedex.ToString("D3");
                if (depart > 0)
                {
                    codePostal = depart.ToString("D2") + inventaireGridRowDto.Adresse.CodePostal.ToString("D3");
                    codePX = depart.ToString("D2") + inventaireGridRowDto.Adresse.CodePostalCedex.ToString("D3");
                }
                else
                {
                    codePostal = inventaireGridRowDto.Adresse.CodePostal.ToString();
                    codePX = inventaireGridRowDto.Adresse.CodePostalCedex.ToString();
                }

                toReturn.Adresse = new ModeleContactAdresse
                {
                    Batiment = inventaireGridRowDto.Adresse.Batiment,
                    CodePostal = codePostal,
                    CodePostalCedex = codePX,
                    MatriculeHexavia = inventaireGridRowDto.Adresse.MatriculeHexavia,
                    No = inventaireGridRowDto.Adresse.NumeroVoie == 0 ? string.Empty : inventaireGridRowDto.Adresse.NumeroVoie.ToString(),
                    No2 = inventaireGridRowDto.Adresse.NumeroVoie2,
                    NoChrono = inventaireGridRowDto.Adresse.NumeroChrono,
                    Pays = inventaireGridRowDto.Adresse.NomPays,
                    Ville = inventaireGridRowDto.Adresse.NomVille,
                    VilleCedex = inventaireGridRowDto.Adresse.NomCedex,
                    Voie = inventaireGridRowDto.Adresse.NomVoie,
                    Distribution = inventaireGridRowDto.Adresse.BoitePostale,
                    Extension = inventaireGridRowDto.Adresse.ExtensionVoie,
                    Latitude = inventaireGridRowDto.Adresse.Latitude?.ToString(),
                    Longitude = inventaireGridRowDto.Adresse.Longitude?.ToString()
                };
            }
            else
                toReturn.Adresse = new ModeleContactAdresse(11, false, true);
            
            return toReturn;
        }

        public static InventaireGridRowDto LoadDto(ModeleInventaireGridRow modele)
        {
            ModeleContactAdresse adresse = modele.Adresse;
            modele.Adresse = new ModeleContactAdresse();
            var toReturn = ObjectMapperManager.DefaultInstance.GetMapper<ModeleInventaireGridRow, InventaireGridRowDto>().Map(modele);
            if (adresse == null)
            {
                toReturn.Adresse = new AdressePlatDto();
            }
            else
            {
                int numVoie = 0;
                int cp = 0;
                int cpCedex = 0;
                string cpFormat = !string.IsNullOrEmpty(adresse.CodePostal) && adresse.CodePostal != "0"
                    ? adresse.CodePostal.Length >= 3
                        ? adresse.CodePostal.Substring(adresse.CodePostal.Length - 3, 3)
                        : adresse.CodePostal
                    : string.Empty;
                string cpCedexFormat = !string.IsNullOrEmpty(adresse.CodePostalCedex) && adresse.CodePostalCedex != "0"
                    ? adresse.CodePostalCedex.Length >= 3
                        ? adresse.CodePostalCedex.Substring(adresse.CodePostalCedex.Length - 3, 3)
                        : adresse.CodePostalCedex
                    : string.Empty;

                toReturn.Adresse = new AdressePlatDto
                {
                    MatriculeHexavia = adresse.MatriculeHexavia,
                    NumeroChrono = adresse.NoChrono.HasValue ? adresse.NoChrono.Value : 0,
                    BoitePostale = adresse.Distribution,
                    ExtensionVoie = adresse.Extension,
                    NomVoie = adresse.Voie,
                    NumeroVoie = Int32.TryParse(adresse.No.Split(new char[] { '/', '-' })[0], out numVoie) ? numVoie : 0,
                    NumeroVoie2 = adresse.No.Contains("/") || adresse.No.Contains("-") ? adresse.No.Split(new char[] { '/', '-' })[1] : "",
                    Batiment = adresse.Batiment,
                    CodePostal = Int32.TryParse(cpFormat, out cp) ? cp : 0,
                    NomVille = adresse.Ville,
                    CodePostalCedex = Int32.TryParse(cpCedexFormat, out cpCedex) ? cpCedex : 0,
                    NomCedex = adresse.VilleCedex,
                    Departement = !string.IsNullOrEmpty(adresse.CodePostal) && adresse.CodePostal.Length == 5 ? adresse.CodePostal.Substring(0, 2) : string.Empty,
                    Latitude = !adresse.Latitude.IsEmptyOrNull() ? Convert.ToDecimal(adresse.Latitude.Replace(".", ",")) : 0,
                    Longitude = !adresse.Longitude.IsEmptyOrNull() ? Convert.ToDecimal(adresse.Longitude.Replace(".", ",")) : 0
                };
            }
            return toReturn;
        }

    }
}