using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BatchConnecteur;
using NPOI.SS.Util;
using Aspose.Cells;
using ALBINGIA.OP.OP_MVC.Common;
using OP.WSAS400.DTO.AffaireNouvelle;
using OPServiceContract.IAffaireNouvelle;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.Framework.Common.Models.Common;
using OP.WSAS400.DTO.Personnes;
using System.Configuration;
using OP.WSAS400.DTO.Contrats;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using ALBINGIA.Framework.Common.AlbingiaExceptions;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ExcelContratController : ControllersBase<ModeleExcelContratPage>
    {
        protected string pathFolder = @"C:\TestExcelContrat\";//@"\\stockage\PUBLIC\Kheops - Connecteur\AffairesNouvelles";
        protected string directoryTemp = @"C:\TestExcelContrat\TempFile";
        List<string> brancheDispo = new List<string>() { "IA", "MR" };
        List<string> cibleDispo = new List<string>() { "MISSION", "EQDOM" };

        protected static string pathLoad = @"\\stockage\PUBLIC\Kheops - Connecteur\";
        public override bool IsReadonly => false;

        // GET: ExcelContrat
        [ErrorHandler]
        public ActionResult Index(ModeleExcelContratPage model)
        {
            Init(model);
            LoadDropdownData(model);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult GetCibles(string branche)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetCibles(branche, false, ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(
                el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                if (result != null)
                {
                    model.Cible = string.Empty;
                    model.Cibles = result.Where(m => cibleDispo.Contains(m.Code)).Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Title = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false }).ToList();
                }
                return Json(model.Cibles, JsonRequestBehavior.AllowGet);
            }
        }

        [ErrorHandler]
        public void UploadTempFiles(HttpPostedFileBase file)
        {
            Directory.CreateDirectory(directoryTemp);
            file.SaveAs(Path.Combine(directoryTemp, Path.GetFileName(file.FileName)));
        }

        [ErrorHandler]
        public RedirectToRouteResult CreateExcelContrat(ModeleExcelContratPage model)
        {
            var now = DateTime.Now;
            var pathExcel = $@"{System.AppDomain.CurrentDomain.BaseDirectory}ExcelWeb\ExcelTemplates\Structure_{model.Branche}_{model.Cible}.xlsm";

            // Créer les différents dossiers s'ils n'existent pas
            string directory = Path.Combine(pathFolder, "TestExcel", now.ToString("yyyy-MM-dd HHmmss"));
            string directoryLog = Path.Combine(pathFolder, "TestLog");
            Directory.CreateDirectory(directory);
            Directory.CreateDirectory(directoryLog);
            var pathLog = $@"{directoryLog}\log_{now.ToString("yyyy-MM-dd")}.txt";

            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(pathLog, FileMode.Append)))
            {
                try
                {
                    sw.WriteLine($"Création de l'affaire le {now.ToString("dd-MM-yyyy")} à {now.ToString("HH:mm:ss")}");
                    sw.WriteLine($"################");

                    Workbook createdWorkbook = new Workbook(pathExcel);
                    Worksheet sheet = createdWorkbook.Worksheets[0];

                    // Modifier la désignation et le type
                    sheet.Cells[3, 4].PutValue(model.Type);
                    sheet.Cells[5, 1].PutValue(model.Designation);

                    // Modifier le souscripteur et le gestionnaire
                    sheet.Cells[7, 1].PutValue(model.Gestionnaire);
                    sheet.Cells[8, 1].PutValue(model.Souscripteur);

                    // Modifier la date d'effet et la périodicité et l'échéance principale
                    if (model.DateEffetDebut.HasValue)
                        sheet.Cells[10, 1].PutValue(model.DateEffetDebut.Value);
                    if (model.DateEffetFin.HasValue)
                        sheet.Cells[10, 3].PutValue(model.DateEffetFin.Value);
                    if (!string.IsNullOrEmpty(model.DateEcheance))
                        sheet.Cells[12, 1].PutValue(model.DateEcheance);
                    sheet.Cells[11, 1].PutValue(model.Periodicite);
                    // Recalculer la formule de la prochaine échéance
                    sheet.Cells[13, 1].Calculate(new CalculationOptions());

                    // Modifier Courtier et Assuré
                    sheet.Cells[18, 1].PutValue(model.CodeCourtierGestionnaire);
                    sheet.Cells[18, 2].PutValue(model.NomCourtierGestionnaire);
                    sheet.Cells[19, 1].PutValue(model.CodeCourtierApporteur);
                    sheet.Cells[19, 2].PutValue(model.NomCourtierApporteur);
                    sheet.Cells[20, 1].PutValue(model.CodeCourtierPayeur);
                    sheet.Cells[20, 2].PutValue(model.NomCourtierPayeur);
                    sheet.Cells[21, 1].PutValue(model.CodeAssure);
                    sheet.Cells[21, 2].PutValue(model.NomAssure);

                    // Modifier l'adresse
                    sheet.Cells[22, 6].PutValue(model.NumeroVoie);
                    sheet.Cells[22, 8].PutValue(model.ExtensionVoie);
                    sheet.Cells[22, 10].PutValue(model.NomVoie);
                    sheet.Cells[23, 6].PutValue(model.CodePostal);
                    sheet.Cells[23, 8].PutValue(model.Ville);

                    // Ajouter des document s'il y a
                    //var n = model.Files.Split('*').Count();
                    if (!string.IsNullOrWhiteSpace(model.Files))
                    {
                        //if (n > 2)
                        //    sheet.Cells.InsertRows(31, n - 2);
                        //var i = 29; // la première ligne excel qui correspond à un fichier
                        foreach (var file in model.Files.Split('*'))
                        {
                            //sheet.Cells[i, 0].PutValue("URL/Nom/Reference");
                            //sheet.Cells[i, 1].PutValue(Path.Combine(directory, file));
                            //sheet.Cells[i, 2].PutValue(Path.GetFileNameWithoutExtension(file));
                            //sheet.Cells[i, 3].SetFormula("=B6", model.Designation);
                            //Style style = sheet.Cells[i, 3].GetStyle();
                            //style.IsLocked = true;
                            //sheet.Cells[i, 3].SetStyle(style);
                            System.IO.File.Move(Path.Combine(directoryTemp, file), Path.Combine(directory, file));
                            sw.WriteLine($"Upload du document {file}");
                            //i++;
                        }
                    }

                    // Sauvegarder le fichier excel
                    string filename = $"Kheops_{model.Branche}_{now.ToString("dd_MM_yyyy HH_mm_ss")}.xlsm";
                    createdWorkbook.Save(Path.Combine(directory, filename));
                    sw.WriteLine($"Génération du fichier excel {filename}");

                    // Créer le fichier JSON a partir du fichier Excel
                    var jsonFileName = Program.createAffaire(directory, filename);
                    sw.WriteLine($"Génération du fichier JSON {jsonFileName}");

                    // Sauvegarder les informations dans la base de données
                    var codeAffaire = CreateJsonContract(Path.Combine(directory, jsonFileName));
                    // Ajout du code affaire et aliment sur excel
                    sheet.Cells[3, 1].PutValue(codeAffaire);
                    sheet.Cells[3, 2].PutValue(0);
                    createdWorkbook.Save(Path.Combine(directory, filename));
                    sw.WriteLine($"Création affaire en base (code affaire : {codeAffaire})");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    model.MessageErreur = "Les données ont bien été enregistrées.";

                    // Envoyer mail (destinataire a définir)
                    MailModel mail = AlbOpConstants.MainInfoContent;
                    mail.Body = $"Les informations de l'affaire {model.Designation} (<strong>{codeAffaire}</strong>) ont été enregistré avec succès.<br>" +
                        $"Pour accéder à Kheops, suivez le lien suivant : {Request.Url.Scheme + "://" + Request.Url.Host}";
                    mail.Subject = "Batch Connecteur Excel";
                    mail.To = "jean-luc.weng@albingia.fr";//;erwan.coulm@albingia.fr";
                    new AlbMailing().SendMail(mail);
                    
                    return RedirectToAction("Index", "ExcelContrat", model);
                    
                }
                catch (FileNotFoundException)
                {
                    // Log erreur si exception
                    sw.WriteLine($"Erreur survenue : Le fichier excel type correspondant n'a pas été trouvé");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    model.MessageErreur = $"Une erreur est survenue : Le fichier excel type correspondant n'a pas été trouvé.";
                    return RedirectToAction("Index", model);
                }
                catch (Exception ex)
                {
                    // Log erreur si exception
                    sw.WriteLine($"Erreur survenue : {ex.Message}");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    model.MessageErreur = $"Une erreur est survenue : {ex.Message}.";
                    return RedirectToAction("Index", model);
                }
            }
        }

        [ErrorHandler]
        public JsonResult GetOffre(string offreId, string version)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            using (var policeClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var policeContext = policeClient.Channel;
                var offre = serviceContext.InitContratInfoBase(offreId, version, AlbConstantesMetiers.TYPE_OFFRE, string.Empty, GetUser(), ModeConsultation.Standard);

                GestionnaireDto gestionnaire = null;
                SouscripteurDto souscripteur = null;
                if (offre != null)
                {
                    // Get utilisateur si souscripteur/gestionnaire
                    gestionnaire = policeContext.GestionnairesGet(new GestionnairesGetQueryDto { NomGestionnaire = offre.GestionnaireCode, DebutPagination = 1, FinPagination = 10 }).GestionnairesDto.FirstOrDefault();
                    souscripteur = policeContext.SouscripteursGet(new SouscripteursGetQueryDto { NomSouscripteur = offre.SouscripteurCode, DebutPagination = 1, FinPagination = 10 }).SouscripteursDto.FirstOrDefault();
                }

                return Json(new { Offre = offre, SouscripteurActive = souscripteur?.IsSouscripteur, GestionnaireActive = gestionnaire?.IsGestionnaire }, JsonRequestBehavior.AllowGet);
            }
        }

        [ErrorHandler]
        public string LoadExcelContrat()
        {
            DateTime now = DateTime.Now;
            bool OK = true;

            string directoryLog = Path.Combine(pathFolder, "TestLog");
            Directory.CreateDirectory(directoryLog);
            var logFilename = $@"Loadall_log_{now.ToString("yyyy - MM - dd")}.txt";
            var pathLog = $@"{directoryLog}\{logFilename}";
            
            string pathFolderLoad = $@"{pathLoad}AffairesNouvelles\";
            string pathFolderCreated = $@"{pathLoad}AffairesTraitees\";

            //Création du fichier de Log
            using (StreamWriter sw = new StreamWriter(System.IO.File.Open(pathLog, FileMode.Append)))
            {
                sw.WriteLine($"Création des affaires le {now.ToString("dd-MM-yyyy")} à {now.ToString("HH:mm:ss")}");
                sw.WriteLine($"################");


                //Lire le dossier sur le serveur contenant les affaires à traiter par le connecteur
                string[] subdirs = Directory.GetDirectories(pathFolderLoad);
                if (subdirs.Length == 0)
                {
                    return "Aucun dossier à traiter n'a été trouvé.";
                }

                foreach (string dir in subdirs)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);
                    FileInfo[] files = d.GetFiles("*.xlsm");

                    foreach (FileInfo file in files)
                    {
                        sw.WriteLine($"Traitement du dossier {d.Name}");

                        try
                        {
                            //Création de l'affaire
                            var jsonFileName = Program.createAffaire(dir, file.Name.ToString());
                            sw.WriteLine($"Génération du fichier JSON {jsonFileName} : OK");

                            // Sauvegarder les informations dans la base de données
                            var codeAffaire = CreateJsonContract(Path.Combine(dir, jsonFileName));
                            sw.WriteLine($"Création affaire en base (code affaire : {codeAffaire}) : OK");

                            //Une fois l'affaire est traitée, la passer dans le dossier "à traiter"
                            Directory.Move(dir, dir.Replace("AffairesNouvelles", "AffairesTraitees"));
                            sw.WriteLine($"Déplacement du dossier {d.Name} : OK");
                            sw.WriteLine($"################");
                        }
                        catch (Exception ex)
                        {
                            OK = false;
                            sw.WriteLine($"Création du dossier {d.Name} : KO");
                            sw.WriteLine(ex.Message);
                            sw.WriteLine($"################");
                        }
                    }
                }
                sw.WriteLine("");
                sw.WriteLine("");
                return OK ? "Tous les dossiers ont été traités avec succès." :
                    $"Une erreur est survenue sur certains dossier. Voir le fichier de log {logFilename} pour plus d'informations.";
            }
        }

        private void Init(ModeleExcelContratPage model)
        {
            model.PageTitle = "Sauvegarde d'un contrat en fichier excel";
            Directory.CreateDirectory(directoryTemp);
            DirectoryInfo di = new DirectoryInfo(directoryTemp);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        private void LoadDropdownData(ModeleExcelContratPage model)
        {
            using (var policeClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var client = policeClient.Channel;
                var brancheList = client.BranchesGet();
                model.Branches = brancheList.Where(m => brancheDispo.Contains(m.Code)).Select(m => new AlbSelectListItem()
                {
                    Value = m.Code,
                    Text = string.Format("{0} - {1}", m.Code, m.Nom),
                    Title = string.Format("{0} - {1}", m.Code, m.Nom),
                    Descriptif = string.Format("{0} - {1}", m.Code, m.Nom)
                }).ToList();
                model.Branches.Insert(0, new AlbSelectListItem() { Value = "", Text = "" });

                var result = client.GetCibles(model.Branche, false, ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(
                el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                if (result != null)
                {
                    model.Cible = string.Empty;
                    model.Cibles = result.Where(m => cibleDispo.Contains(m.Code)).Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Title = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false }).ToList();
                }
                else
                {
                    model.Cibles = new List<AlbSelectListItem>();
                }

                var periodiciteList = new List<SelectListItem>();
                periodiciteList.Add(new SelectListItem() { Value = "A - Annuel", Text = "A - Annuel" });
                periodiciteList.Add(new SelectListItem() { Value = "E - Unique - Echéancier", Text = "E - Unique - Echéancier" });
                periodiciteList.Add(new SelectListItem() { Value = "R - Régularisation seule", Text = "R - Régularisation seule" });
                periodiciteList.Add(new SelectListItem() { Value = "S - Semestriel", Text = "S - Semestriel" });
                periodiciteList.Add(new SelectListItem() { Value = "T - Trimestriel", Text = "T - Trimestriel" });
                periodiciteList.Add(new SelectListItem() { Value = "U - Unique", Text = "U - Unique" });

                model.Periodicites = periodiciteList;
            }
        }

        private CabinetCourtageDto CabinetCourtageGet(int code)
        {
            CabinetCourtageDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.ObtenirCabinetCourtageComplet(code, 0);
            }
            return toReturn;
        }

        private AssureDto AssureGet(int code)
        {
            AssureDto toReturn;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                toReturn = screenClient.ObtenirAssureComplet(code);
            }
            return toReturn;
        }

        private string CreateJsonContract(string path)
        {
            string filePath = string.IsNullOrEmpty(path) ? ConfigurationManager.AppSettings["ContractJsonFullPath"] : path;
            ContractJsonDto objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ContractJsonDto>(System.IO.File.ReadAllText(filePath));

            var gestionnaire =
                CabinetCourtageGet(objectJson.Courtier.Gestionnaire.Code.ToInteger() ?? 0);
            var apporteur =
                CabinetCourtageGet(objectJson.Courtier.Apporteur.Code.ToInteger() ?? 0);
            var payeur =
                CabinetCourtageGet(objectJson.Courtier.Payeur.Code.ToInteger() ?? 0);
            var assure =
                AssureGet(objectJson.Assure.Code.ToInteger() ?? 0);

            if (gestionnaire.Code == 0 || apporteur.Code == 0 || payeur.Code == 0 || assure == null)
            {
                throw new Exception("Courtier ou Assure inexistant");
            }

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var user = GetUser();

                var serviceContext = client.Channel;

                switch (objectJson.Type)
                {
                    // Création d'un contrat
                    case "P":
                        objectJson = serviceContext.CreationContractsKheops(objectJson, user);
                        break;
                    // Création d'une offre
                    case "O":
                        objectJson = serviceContext.CreationOffersKheops(objectJson, user);
                        break;
                }

                return objectJson.CodeAffaire;
            }
        }

        #region ControllersBase Init
        protected override void SetPageTitle()
        {
            model.PageTitle = "Sauvegarde d'un contrat en fichier excel";
        }

        protected override void LoadInfoPage(string context)
        {
        }

        protected override void UpdateModel()
        {
            model.PageEnCours = NomsInternesEcran.RechercheSaisie.ToString();
            model.ModeNavig = ModeConsultation.Standard.AsCode();
            DisplayBandeau();
        }
        #endregion
    }
}