using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamFiltres;
using OP.WSAS400.DTO.ParametreFiltre;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    [ErrorHandler]
    public class ParamFiltresController : ControllersBase<ModeleParamFiltresPage>
    {
            #region Membres statiques
            public static readonly int ID_LIGNE_VIDE = -9999;
            public static readonly string MODE_EDITION = "Update";
            public static readonly string MODE_CREATION = "Insert";

        public static List<AlbSelectListItem> LstCibles(string branche)
        {
            List<AlbSelectListItem> value = new List<AlbSelectListItem>();
            value.Add(new AlbSelectListItem { Value = "*", Text = "Toutes cibles", Title = "Toutes cibles", Selected = false });
            if (!string.IsNullOrEmpty(branche))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstBrch = serviceContext.GetCiblesFiltre(branche);
                    lstBrch.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
            }
            return value;
        }

        private static List<AlbSelectListItem> _lstBranches;
        public static List<AlbSelectListItem> LstBranches
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstBranches != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstBranches.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                value.Add(new AlbSelectListItem { Value = "*", Text = "Toutes branches", Title = "Toutes branches", Selected = false });
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var lstBrch = serviceContext.GetBranchesFiltre();
                    lstBrch.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstBranches = value;
                return value;
            }
        }

        public static List<AlbSelectListItem> LstActions
        {
            get
            {
                //Nouvelle instance à chaque récupération de la référence
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                value.Add(new AlbSelectListItem() { Value = "I", Text = "Inclure" });
                value.Add(new AlbSelectListItem() { Value = "E", Text = "Exclure" });
                return value;
            }
        }

        #endregion

        #region Méthodes publiques
        
        [ErrorHandler]
        [AlbApplyUserRole]
        public ActionResult Index()
        {
            model.PageTitle = "Paramétrages des filtres valeurs";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            model.AdditionalParam = "**";
            DisplayBandeau();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult RechercheFiltres(string codeFiltre, string descriptionFiltre, string userRights)
        {
            var toReturn = new List<ModeleLigneFiltre>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var result = serviceContext.LoadListFiltres(codeFiltre, descriptionFiltre, string.Empty);
                if (result.Any())
                    result.ForEach(m => toReturn.Add((ModeleLigneFiltre)m));
                toReturn.ForEach(elm => elm.AdditionalParam = userRights);
            }
            return PartialView("ListeFiltres", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetDetailsFiltre(string idFiltre, string codeFiltre, string userRights)
        {
            var toReturn = new ModeleDetailsFiltre();
            if (!string.IsNullOrEmpty(codeFiltre) && string.IsNullOrEmpty(idFiltre))
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var res = serviceContext.LoadListFiltres(codeFiltre.Replace("'","''").Replace(" ", ""), string.Empty, string.Empty);
                    if (res.Any())
                    {
                        idFiltre = res.FirstOrDefault().Id.ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(idFiltre))
            {
                Int64 iFiltre = 0;
                if (!Int64.TryParse(idFiltre, out iFiltre))
                    throw new AlbTechException(new Exception("Edition impossible, identifiant du filtre incorrect"));

                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var result = serviceContext.GetFiltreDetails(iFiltre);
                    if (result != null)
                    {
                        toReturn = (ModeleDetailsFiltre)result;
                        toReturn.LigneVideBrancheCible = new ModeleLigneBrancheCible { GuidIdPaire = ID_LIGNE_VIDE, ListeActions = LstActions, ListeBranches = LstBranches, DrlCibles = new ModeleDrlCibles(), AdditionalParam = userRights };
                        toReturn.LigneVideBrancheCible.DrlCibles.ListeCibles = LstCibles(string.Empty);
                        toReturn.LigneVideBrancheCible.DrlCibles.GuidIdPaire = ID_LIGNE_VIDE;
                        toReturn.ModeSaisie = MODE_EDITION;
                        toReturn.AdditionalParam = userRights;
                        if (toReturn.ListePairesBrancheCible != null)
                            toReturn.ListePairesBrancheCible.ForEach(elm => elm.AdditionalParam = userRights);
                    }
                }
            }
            else
            {
                toReturn.LigneVideBrancheCible = new ModeleLigneBrancheCible { GuidIdPaire = ID_LIGNE_VIDE, ListeActions = LstActions, ListeBranches = LstBranches, DrlCibles = new ModeleDrlCibles(), AdditionalParam = userRights };
                toReturn.LigneVideBrancheCible.DrlCibles.ListeCibles = LstCibles(string.Empty);
                toReturn.LigneVideBrancheCible.DrlCibles.GuidIdPaire = ID_LIGNE_VIDE;
                toReturn.ModeSaisie = MODE_CREATION;
                toReturn.AdditionalParam = userRights;
            }
            return PartialView("DetailsFiltre", toReturn);
        }

        [ErrorHandler]
        public ActionResult GetCibles(Int64 idLigne, string branche)
        {
            var toReturn = new ModeleDrlCibles();
            toReturn.ListeCibles = LstCibles(branche);
            toReturn.GuidIdPaire = idLigne;
            return PartialView("DrlCibles", toReturn);
        }

        [ErrorHandler]
        public ActionResult EnregistrerFiltre(string mode, Int64 idFiltre, string codeFiltre, string descriptionFiltre, string userRights)
        {
            if (!string.IsNullOrEmpty(mode) && (!string.IsNullOrEmpty(codeFiltre) && (!string.IsNullOrEmpty(descriptionFiltre))))
            {
                if (mode == MODE_EDITION)
                {
                    if (idFiltre < 0)
                        throw new AlbTechException(new Exception("Edition impossible, identifiant du filtre incorrect"));
                    var toReturn = new List<ModeleLigneFiltre>();
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        if (serviceContext.LoadListFiltres(codeFiltre, string.Empty, string.Empty).Any())
                        {
                            var param = new ModeleFiltreLigneDto { Id = idFiltre, Code = codeFiltre.Replace(" ", "").Replace("'", "''"), Libelle = descriptionFiltre.Replace("'", "''") };
                            var result = serviceContext.EnregistrerFiltre(mode, param, GetUser());
                            if (result.Any())
                                result.ForEach(m => toReturn.Add((ModeleLigneFiltre)m));
                            toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                        }
                        else
                            throw new AlbFoncException("Le filtre que vous voulez modifier n'existe plus", trace: false, sendMail: false, onlyMessage: true);
                    }
                    return PartialView("LigneFiltre", toReturn.FirstOrDefault());
                }
                else if (mode == MODE_CREATION)
                {
                    var toReturn = new List<ModeleLigneFiltre>();
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var serviceContext=client.Channel;
                        if (!serviceContext.LoadListFiltres(codeFiltre.Replace(" ", "").Replace("'", "''"), string.Empty, string.Empty).Any())
                        {
                            var param = new ModeleFiltreLigneDto { Id = idFiltre, Code = codeFiltre.Replace(" ", "").Replace("'", "''"), Libelle = descriptionFiltre.Replace("'", "''") };
                            var result = serviceContext.EnregistrerFiltre(mode, param, GetUser());
                            if (result.Any())
                                result.ForEach(m => toReturn.Add((ModeleLigneFiltre)m));
                            toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                            return PartialView("ListeFiltres", toReturn);
                        }
                        else
                            throw new AlbFoncException("Ce code est déjà utilisé", trace: false, sendMail: false, onlyMessage: true);
                    }
                }
            }
            throw new AlbFoncException("Code et description ne peuvent être vides", trace: false, sendMail: false, onlyMessage: true);
        }

        [ErrorHandler]
        public ActionResult EnregistrerPaireBrancheCible(Int64 idFiltre, Int64 idPaire, string action, string branche, string cible, string userRights)
        {
            if (idFiltre < 0)
                throw new AlbTechException(new Exception("Edition impossible, identifiant du filtre incorrect"));
            if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(branche) && !string.IsNullOrEmpty(cible))
            {
                var toReturn = new List<ModeleLigneBrancheCible>();
                //Enregistrement BDD
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    string mode = (idPaire == ID_LIGNE_VIDE ? MODE_CREATION : MODE_EDITION);

                    var resCheck1 = serviceContext.GetListeBrancheCible(idFiltre, null, "*", "*");
                    if (resCheck1.Any())
                    {
                        throw new AlbFoncException("Enregistrement impossible, une règle globale (toutes branches et cibles) est déjà appliquée pour ce filtre", trace: false, sendMail: false, onlyMessage: true);
                    }

                    var resCheck = serviceContext.GetListeBrancheCible(idFiltre, null, branche, cible);
                    if (resCheck.Any() && !resCheck.Exists(elm => elm.GuidIdPaire == idPaire))
                        throw new AlbFoncException("Enregistrement impossible, une règle est déjà appliquée pour la paire branche/cible sélectionnée", trace: false, sendMail: false, onlyMessage: true);

                    if (mode == MODE_EDITION)
                    {
                        resCheck = serviceContext.GetListeBrancheCible(idFiltre, idPaire, string.Empty, string.Empty);
                        if (!resCheck.Any())
                            throw new AlbFoncException("Mise à jour impossible, cette règle n'existe plus, veuillez recharger le filtre selectionné", trace: false, sendMail: false, onlyMessage: true);
                    }
                    var param = new ModeleLigneBrancheCibleDto { GuidIdPaire = idPaire, Action = action, Branche = branche, Cible = cible };
                    var result = serviceContext.EnregistrerPaireBrancheCible(mode, idFiltre, param, GetUser());
                    if (result.Any())
                        result.ForEach(m => toReturn.Add((ModeleLigneBrancheCible)m));
                    toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                    if (idPaire == ID_LIGNE_VIDE)
                        return PartialView("ListePairesBrancheCible", toReturn);
                    else
                        return PartialView("LignePaireBrancheCible", toReturn.FirstOrDefault());
                }
            }
            throw new AlbFoncException("Action, branche et cible ne peuvent être vides", trace: false, sendMail: false, onlyMessage: true);

        }

        [ErrorHandler]
        public ActionResult SupprimerFiltre(Int64 idFiltre, string userRights)
        {
            if (idFiltre > 0)
            {
                var toReturn = new List<ModeleLigneFiltre>();
                var param = new ModeleFiltreLigneDto { Id = idFiltre };
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    string error = serviceContext.SupprimerFiltre(param);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error, trace: true, sendMail: true, onlyMessage: true);

                    //Rechargement des filtres
                    var result = serviceContext.LoadListFiltres(string.Empty, string.Empty, string.Empty);
                    if (result.Any())
                        result.ForEach(m => toReturn.Add((ModeleLigneFiltre)m));
                    toReturn.ForEach(elm => elm.AdditionalParam = userRights);
                }
                return PartialView("ListeFiltres", toReturn);
            }
            throw new AlbFoncException("Impossible de supprimer un filtre sans code", trace: true, sendMail: true, onlyMessage: true);
        }

        [ErrorHandler]
        public ActionResult SupprimerPaireBrancheCible(Int64 idFiltre, Int64 idPaire, string userRights)
        {
            if (idPaire > 0)
            {
                var toReturn = new List<ModeleLigneBrancheCible>();
                var param = new ModeleLigneBrancheCibleDto { GuidIdPaire = idPaire };
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext=client.Channel;
                    var resCheck = serviceContext.GetListeBrancheCible(idFiltre, idPaire, string.Empty, string.Empty);
                    if (!resCheck.Any())
                        throw new AlbFoncException("Suppression impossible, cette règle n'existe plus, veuillez recharger le filtre selectionné", trace: false, sendMail: false, onlyMessage: true);

                    string error = serviceContext.SupprimerPaireBrancheCible(param);
                    if (!string.IsNullOrEmpty(error))
                        throw new AlbFoncException(error, trace: true, sendMail: true, onlyMessage: true);
                }
                return GetDetailsFiltre(idFiltre.ToString(), string.Empty, userRights);
            }
            throw new AlbFoncException("Impossible de supprimer une paire Branche Cible sans code", trace: true, sendMail: true, onlyMessage: true);
        }

        [ErrorHandler]
        public string VerifierFiltre(Int64 idFiltre)
        {
            string msg = string.Empty;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext=client.Channel;
                var resCheck = serviceContext.GetListeBrancheCible(idFiltre, null, string.Empty, string.Empty);
                if (!resCheck.Any())
                    msg += "Le filtre doit avoir au moins une condition de type inclure<br>";
                else
                {
                    if (resCheck.Find(elm => elm.Action == "I") == null)
                        msg += "Le filtre doit avoir au moins une condition de type inclure<br>";
                    var resCheckAll = serviceContext.GetListeBrancheCible(idFiltre, null, "*", "*");
                    if (resCheckAll.Any())
                    {
                        if (resCheck.Count > 1)
                            msg += "Il ne peut y avoir une règle globale et des règles spécifiques pour un même filtre<br>";
                    }
                }
            }
            return msg;
        }

        #endregion

    }
}
