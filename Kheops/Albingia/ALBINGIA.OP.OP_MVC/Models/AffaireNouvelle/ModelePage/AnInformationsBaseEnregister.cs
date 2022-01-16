using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Globalization;

namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage
{
    public class AnInformationsBaseEnregister : MetaModelsBase
    {
        public string txtParamRedirect { get; set; }
        public string CodeContrat { get; set; }
        public long VersionContrat { get; set; }
        public string Type { get; set; }
        public string Branche { get; set; }
        public string SouscripteurSelect { get; set; }
        public string SouscripteurCode { get; set; }
        public string SouscripteurNom { get; set; }
        public DateTime DateAccord { get; set; }
        public string Cible { get; set; }
        public string GestionnaireCode { get; set; }
        public string GestionnaireNom { get; set; }
        public string ContratMere { get; set; }
        public string NumAliment { get; set; }
        public string NumContratRemplace { get; set; }
        public string NumAlimentRemplace { get; set; }
        public bool EditMode { get; set; }
        public DateTime DateEffet { get; set; }
        public string TypeContrat { get; set; }
        public TimeSpan? HeureEffet { get; set; }
        public string HeureEffetHours { get; set; }
        public string HeureEffetMinutes { get; set; }
        public bool ContratRemplace { get; set; }
        public int CodeCourtierApporteur { get; set; }
        public string GpIdentiqueApporteur { get; set; }
        public int CodeCourtierGestionnaire { get; set; }
        public int CodeCourtierPayeur { get; set; }
        public int CodeInterlocuteurGestionnaire { get; set; }
        public int CodeInterlocuteurApporteur { get; set; }
        public int CodeInterlocuteurPayeur { get; set; }
        public string Reference { get; set; }
        public string Encaissement { get; set; }
        public int NumeroAssur { get; set; }
        public string NomAssur { get; set; }
        public string DepartementAssur { get; set; }
        public string VilleAssur { get; set; }
        public string MotClef1 { get; set; }
        public string MotClef2 { get; set; }
        public string MotClef3 { get; set; }
        public string Descriptif { get; set; }
        public int NumChronoObsv { get; set; }
        public string Observation { get; set; }

        public string PreneurEstAssure { get; set; }

        public bool CopyMode { get; set; }
        public string CodeContratCopy { get; set; }
        public string VersionCopy { get; set; }
        public bool TemplateMode { get; set; }//Si mode edition template
        //Adresse du risque
        //public ModeleContactAdresse ContactAdresse { get; set; }

        //public string ContactAddresseMetaData_Batiment { get; set; }
        //public string ContactAddresseMetaData_No { get; set; }
        //public string ContactAddresseMetaData_Extension { get; set; }
        //public string ContactAddresseMetaData_Voie { get; set; }
        //public string ContactAddresseMetaData_Distribution { get; set; }
        //public string ContactAddresseMetaData_CodePostal { get; set; }
        //public string ContactAddresseMetaData_Ville { get; set; }
        //public string ContactAddresseMetaData_CodePostalCedex { get; set; }
        //public string ContactAddresseMetaData_VilleCedex { get; set; }
        //public string ContactAddresseMetaData_Pays { get; set; }
        //public string ContactAddresseMetaData_MatriculeHexavia { get; set; }
        //public int ContactAddresseMetaData_NoChrono { get; set; }

        public string ContactAdresse_Batiment { get; set; }
        public string ContactAdresse_No { get; set; }
        public string ContactAdresse_Extension { get; set; }
        public string ContactAdresse_Voie { get; set; }
        public string ContactAdresse_Distribution { get; set; }
        public string ContactAdresse_CodePostal { get; set; }
        public string ContactAdresse_Ville { get; set; }
        public string ContactAdresse_CodePostalCedex { get; set; }
        public string ContactAdresse_VilleCedex { get; set; }
        public string ContactAdresse_Pays { get; set; }
        public string ContactAdresse_MatriculeHexavia { get; set; }
        public string ContactAdresse_Latitude { get; set; }
        public string ContactAdresse_Longitude  { get; set; }


        public int ContactAdresse_NoChrono { get; set; }

        public string TypeAvt { get; set; }
        public string NumAvenant { get; set; }

        public DateTime DateEffetAvenant { get; set; }
        public TimeSpan? HeureEffetAvenant { get; set; }

        public string NomCourtierGestionnaire { get; set; }
        public string NomInterlocuteurGestionnaire { get; set; }
        public string NomCourtierApporteur { get; set; }
        public string NomCourtierPayeur { get; set; }
        public ContratInfoBaseDto LoadContratInfoBaseDto()
        {
            var contratInfoBase = new ContratInfoBaseDto();
            //Informations de base
            contratInfoBase.CodeContratCopy = this.CodeContratCopy;
            contratInfoBase.VersionCopy = this.VersionCopy;
            contratInfoBase.CopyMode = this.CopyMode;
            contratInfoBase.TemplateMode = this.TemplateMode;
            contratInfoBase.NumAvenant = this.NumAvenant;
            contratInfoBase.TypeAvt = this.TypeAvt;

            contratInfoBase.CodeContrat = this.CodeContrat;
            contratInfoBase.VersionContrat = this.VersionContrat;
            contratInfoBase.Type = this.Type;
            contratInfoBase.Branche = this.Branche;
            contratInfoBase.Cible = this.Cible;
            contratInfoBase.SouscripteurCode = this.SouscripteurCode;
            contratInfoBase.GestionnaireCode = this.GestionnaireCode;
            contratInfoBase.TypePolice = this.TypeContrat;
            contratInfoBase.ContratMere = this.ContratMere;
            contratInfoBase.NumAliment = this.NumAliment;
            contratInfoBase.ContratRemplace = !string.IsNullOrEmpty(this.NumContratRemplace);
            contratInfoBase.NumContratRemplace = this.NumContratRemplace;
            contratInfoBase.NumAlimentRemplace = !string.IsNullOrEmpty(this.NumAlimentRemplace) ? this.NumAlimentRemplace : !string.IsNullOrEmpty(this.NumContratRemplace) ? "0" : string.Empty;
            contratInfoBase.DateAccordAnnee = Int16.Parse(this.DateAccord.Year.ToString());
            contratInfoBase.DateAccordMois = Int16.Parse(this.DateAccord.Month.ToString());
            contratInfoBase.DateAccordJour = Int16.Parse(this.DateAccord.Day.ToString());
            contratInfoBase.Etat = "N";
            contratInfoBase.Situation = "A";
            //if (this.HeureEffet.HasValue)
            //    contratInfoBase.DateEffetHeure = AlbConvert.ConvertTimeToIntMinute(this.HeureEffet.Value).Value;
            contratInfoBase.DateEffetAnnee = Int16.Parse(this.DateEffet.Year.ToString());
            contratInfoBase.DateEffetMois = Int16.Parse(this.DateEffet.Month.ToString());
            contratInfoBase.DateEffetJour = Int16.Parse(this.DateEffet.Day.ToString());
            if (!string.IsNullOrEmpty(this.HeureEffetHours) && !string.IsNullOrEmpty(this.HeureEffetMinutes))
                contratInfoBase.DateEffetHeure = AlbConvert.ConvertTimeToIntMinute(new TimeSpan(int.Parse(this.HeureEffetHours), int.Parse(this.HeureEffetMinutes), 0)).Value;

            //Courtiers
            contratInfoBase.CourtierApporteur = this.CodeCourtierApporteur;
            //if (!string.IsNullOrEmpty(GpIdentiqueApporteur))
            //{
            //    contratInfoBase.CourtierGestionnaire = this.CodeCourtierApporteur;
            //    contratInfoBase.CourtierPayeur = this.CodeCourtierApporteur;
            //}
            //else
            //{
            contratInfoBase.CourtierGestionnaire = this.CodeCourtierGestionnaire;
            contratInfoBase.CourtierPayeur = this.CodeCourtierPayeur;
            //}

            contratInfoBase.CodeInterlocuteur = this.CodeInterlocuteurGestionnaire;
            contratInfoBase.RefCourtier = this.Reference;
            contratInfoBase.Encaissement = this.Encaissement;

            //Preneur d'assurance
            contratInfoBase.CodePreneurAssurance = this.NumeroAssur;
            contratInfoBase.PreneurEstAssure = !string.IsNullOrEmpty(this.PreneurEstAssure);

            //Information du contrat
            contratInfoBase.CodeMotsClef1 = this.MotClef1;
            contratInfoBase.CodeMotsClef2 = this.MotClef2;
            contratInfoBase.CodeMotsClef3 = this.MotClef3;
            contratInfoBase.Descriptif = this.Descriptif;

            if (!string.IsNullOrEmpty(this.Observation))
            {
                contratInfoBase.Obersvations = this.Observation.Replace("\r\n", "<br>").Replace("\n", "<br>");
            }
            else
                contratInfoBase.Obersvations = string.Empty;
            contratInfoBase.NumChronoObsv = this.NumChronoObsv;

            Int32 numVoie = 0;
            Int32 cp = 0;
            Int32 cpCedex = 0;
            string cpFormat = !string.IsNullOrEmpty(this.ContactAdresse_CodePostal) ? this.ContactAdresse_CodePostal.Length >= 3 ? this.ContactAdresse_CodePostal.Substring(this.ContactAdresse_CodePostal.Length - 3, 3)
                                                                                                           : this.ContactAdresse_CodePostal
                                                             : string.Empty;
            string cpCedexFormat = !string.IsNullOrEmpty(this.ContactAdresse_CodePostalCedex) ? this.ContactAdresse_CodePostalCedex.Length >= 3 ? this.ContactAdresse_CodePostalCedex.Substring(this.ContactAdresse_CodePostalCedex.Length - 3, 3)
                                                                                                                        : this.ContactAdresse_CodePostalCedex
                                                                          : string.Empty;


            contratInfoBase.AdresseContrat = new AdressePlatDto
            {
                Batiment = this.ContactAdresse_Batiment,
                NumeroVoie = Int32.TryParse(this.ContactAdresse_No.Split(new char[] { '/', '-' })[0], out numVoie) ? numVoie : -1,
                NumeroVoie2 = this.ContactAdresse_No.Contains("/") || this.ContactAdresse_No.Contains("-") ? this.ContactAdresse_No.Split(new char[] { '/', '-' })[1] : "",
                ExtensionVoie = this.ContactAdresse_Extension,
                NomVoie = this.ContactAdresse_Voie,
                NumeroChrono = this.ContactAdresse_NoChrono,
                BoitePostale = this.ContactAdresse_Distribution,
                CodePostal = Int32.TryParse(cpFormat, out cp) ? cp : -1,
                NomVille = this.ContactAdresse_Ville,
                CodePostalCedex = Int32.TryParse(cpCedexFormat, out cpCedex) ? cpCedex : -1,
                NomCedex = this.ContactAdresse_VilleCedex,
                NomPays = this.ContactAdresse_Pays,
                MatriculeHexavia = this.ContactAdresse_MatriculeHexavia,
                Latitude = this.ContactAdresse_Latitude.IsEmptyOrNull() ? 0 : Decimal.Parse(this.ContactAdresse_Latitude.Replace(".", ","), NumberStyles.Any),
                Longitude = this.ContactAdresse_Longitude.IsEmptyOrNull() ? 0 : Decimal.Parse(this.ContactAdresse_Longitude.Replace(".", ","), NumberStyles.Any),
                Departement = !string.IsNullOrEmpty(this.ContactAdresse_CodePostal) && this.ContactAdresse_CodePostal.Length == 5 ? this.ContactAdresse_CodePostal.Substring(0, 2) : string.Empty
            };
            contratInfoBase.AnneeEffetAvenant = Int16.Parse(this.DateEffetAvenant.Year.ToString());
            contratInfoBase.MoisEffetAvenant = Int16.Parse(this.DateEffetAvenant.Month.ToString());
            contratInfoBase.JourEffetAvenant = Int16.Parse(this.DateEffetAvenant.Day.ToString());
            contratInfoBase.HeureEffetAvenant = this.HeureEffetAvenant.HasValue ? AlbConvert.ConvertTimeMinuteToInt(this.HeureEffetAvenant).Value : 0;

            contratInfoBase.NomCourtierGest = this.NomCourtierGestionnaire;
            contratInfoBase.NomInterlocuteur = this.NomInterlocuteurGestionnaire;
            contratInfoBase.NomCourtierAppo = this.NomCourtierApporteur;
            contratInfoBase.NomCourtierPayeur = this.NomCourtierPayeur;
            contratInfoBase.NomPreneurAssurance = this.NomAssur;
            return contratInfoBase;
        }
    }
}