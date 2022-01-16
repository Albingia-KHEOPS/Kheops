using Albingia.Kheops.OP.Domain.Formule;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.ModelesContextMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class ContextMenuController : Controller
    {
        #region Méthodes Publiques
        [HttpPost]
        [ErrorHandler]
        public string GetContextMenu(
            string type,
            string etat,
            string situation,
            string periodicite,
            string copyOffre,
            string modeNavig,
            string brche,
            string typeAccord,
            string generdoc,
            string typeAvt,
            string numAvt,
            bool isBlackListedSeach = false)
        {

            if (ModeConsultation.Standard == modeNavig.ParseCode<ModeConsultation>())
            {
                var ctxMenuUser = GetContextMenuUser(type, brche, isBlackListedSeach);
                return BuildMenuString(
                    ctxMenuUser,
                    type,
                    etat,
                    brche,
                    sit: situation,
                    typeAccord: typeAccord,
                    generDoc: !generdoc.IsEmptyOrNull() && generdoc != "0",
                    typeAvt: typeAvt,
                    periodicite: periodicite,
                    numAvt: numAvt);

            }
            else if (ModeConsultation.Historique == modeNavig.ParseCode<ModeConsultation>())
            {
                return BuildMenuString(GetContextMenuHistorique());

            }

            return string.Empty;
        }

        [HttpPost]
        [ErrorHandler]
        public string GetMenuNouveau(string type, string etat, string brche)
        {
            return BuildMenuString(GetMenuSectionSpecifique(AlbContextMenu.CREER), type, etat, brche);
        }

        [HttpPost]
        [ErrorHandler]
        public string GetMenuAvenant(string type, string etat, string brche, string sit, string periodicite, string situation, string typeAccord, string numAvt, string typeAvt)
        {
            return BuildMenuString(GetMenuSectionSpecifique(AlbContextMenu.AVENANT), type, etat, brche, sit: situation, periodicite: periodicite, typeAccord: typeAccord, menuText: AlbContextMenu.AVENANT.DisplayName(), numAvt: numAvt, typeAvt: typeAvt);
        }

        [HttpPost]
        [ErrorHandler]
        public string GetMenuClause(string type, string user)
        {
            return BuildMenuString(GetMenuSectionClause(type, user), menuSearch: false);
        }

        [HttpPost]
        [ErrorHandler]
        public string GetMenuFormule(string type, string user, bool readOnly, bool blockConditions, bool suppFormule, bool isSorti, int nbOptions = 0)
        {
            return BuildMenuString(GetMenuSectionFormule(type, user, readOnly, blockConditions, suppFormule, isSorti), menuSearch: false);
        }

        [HttpPost]
        [ErrorHandler]
        public string GetMenuOption(string user, bool readOnly, int nbOptions)
        {
            return BuildMenuString(GetMenuForOption(user, readOnly, nbOptions), menuSearch: false);
        }

        [HttpPost]
        [ErrorHandler]
        public string GetMenuInventaire(string type, string user, string invenId)
        {
            return BuildMenuString(GetMenuSectionInventaire(type, user, invenId), menuSearch: false);
        }

        [HttpPost]
        [ErrorHandler]
        public string GetMenuHeaderInventaire(string type, string user)
        {
            return BuildMenuString(GetMenuSectionHeaderInventaire(type, user), menuSearch: false);
        }

        #endregion

        #region Méthodes Privées

        private List<ModeleListItem> GetContextMenuHistorique()
        {
            if (MvcApplication.AlbAllFlatContextMenuUsers == null)
                return null;
            return MvcApplication.AlbAllFlatContextMenuUsers.FindAll(el => el.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower() && el.text == "Consulter").ToList();
        }

        private List<ModeleListItem> GetContextMenuUser(string type, string branche, bool isBlackListedSeach = false)
        {
            switch (type)
            {
                case AlbConstantesMetiers.TYPE_OFFRE:
                    type = AlbConstantesMetiers.TYPE_MENU_OFFRE; break;
                case AlbConstantesMetiers.TYPE_CONTRAT:
                    type = AlbConstantesMetiers.TYPE_MENU_CONTRAT; break;
            }


            if (MvcApplication.AlbAllContextMenuUsers == null)
                return null;
            List<ModeleListItem> toReturnList = new List<ModeleListItem>();
            MvcApplication.AlbAllContextMenuUsers.ForEach(ctxMenu =>
            {
                var tmpMenu = new ModeleListItem
                {
                    action = ctxMenu.action,
                    alias = ctxMenu.alias,
                    icon = ctxMenu.icon,
                    text = ctxMenu.text,
                    type = ctxMenu.type,
                    typeOffreContrat = ctxMenu.typeOffreContrat,
                    Utilisateur = ctxMenu.Utilisateur,
                    width = ctxMenu.width,
                    orderby = ctxMenu.orderby,
                    AlwBranche = ctxMenu.AlwBranche,
                    AlwEtat = ctxMenu.AlwEtat
                };

                if ((ctxMenu.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower())
                      && (ctxMenu.typeOffreContrat == type || ctxMenu.typeOffreContrat == "*"))
                {
                    if (ctxMenu.items != null)
                    {
                        tmpMenu.items = ctxMenu.items.FindAll(subItem =>
                                                              subItem.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower() &&
                                                              (subItem.typeOffreContrat == type || subItem.typeOffreContrat == "*"));
                        tmpMenu.items = tmpMenu.items == null ? null : tmpMenu.items.OrderBy(elm => elm.orderby).ToList();
                    }
                    toReturnList.Add(tmpMenu);
                }
                //SLA : à supprimer lorsque le groupe "Nouveau" sera de type * en bdd
                else if (ctxMenu.type == "group" && ctxMenu.menu == AlbContextMenu.CREER && ctxMenu.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower())
                {
                    if (ctxMenu.items != null)
                    {
                        tmpMenu.items = ctxMenu.items.FindAll(subItem =>
                                        subItem.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower() &&
                                        (subItem.typeOffreContrat == type || subItem.typeOffreContrat == "*"));
                        tmpMenu.items = tmpMenu.items == null ? null : tmpMenu.items.OrderBy(elm => elm.orderby).ToList();
                    }
                    toReturnList.Add(tmpMenu);
                }//SLA : fin à supprimer

            });

            #region Suppression des menus/sous-menus

            toReturnList = AlbTransverse.CleanMenus(toReturnList);

            #endregion


            #region Droits de consultation

            // Consultation / Modification si black listed
            if (isBlackListedSeach)
            {
                toReturnList.RemoveAll(el => el.text != AlbContextMenu.OFFCONT.DisplayName());
                toReturnList.ForEach(el => el.items.RemoveAll(m => !AlbOpConstants.ListActionsModeConsultation.Contains(m.text.ToLower().Trim())
                && !m.text.ToLower().Equals("modifier")));

                return toReturnList.OrderBy(elm => elm.orderby).ToList();
            }

            //Aucuns droits (utilisateur absent de la table)
            if (CacheUserRights.UserRights == null || !CacheUserRights.UserRights.Any())
            {
                toReturnList.RemoveAll(el => el.text != AlbContextMenu.OFFCONT.DisplayName());
                toReturnList.ForEach(el => el.items.RemoveAll(m => !AlbOpConstants.ListActionsModeConsultation.Contains(m.text.ToLower().Trim())));

                return toReturnList.OrderBy(elm => elm.orderby).ToList();
            }

            //Utilisateur ayant des droits supérieurs à la visualisation pour toutes les branches ou une branche précise
            if (CacheUserRights.UserRights.Any(
                el => (el.Branche == "**" || el.Branche == branche) && el.TypeDroit != TypeDroit.V.ToString()))
            {
                return toReturnList.OrderBy(elm => elm.orderby).ToList();
            }

            //Utilisateur en visualisation uniquement (n'importe quelle branche)
            if (!CacheUserRights.UserRights.Any(
                el => el.TypeDroit == TypeDroit.G.ToString() || el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()))
            {
                toReturnList.RemoveAll(el => el.text != AlbContextMenu.OFFCONT.DisplayName());
                toReturnList.ForEach(el => el.items.RemoveAll(m => !AlbOpConstants.ListActionsModeConsultation.Contains(m.text.ToLower().Trim())));


                return toReturnList.OrderBy(elm => elm.orderby).ToList();
            }

            //Utilisateur en visualisation pour une branche précise ?? utile ??
            if (!CacheUserRights.UserRights.Any(el => el.Branche == branche && el.TypeDroit == TypeDroit.G.ToString()))
            {
                toReturnList.RemoveAll(el => el.text != AlbContextMenu.OFFCONT.DisplayName());
                toReturnList.ForEach(el => el.items.RemoveAll(m => !AlbOpConstants.ListActionsModeConsultation.Contains(m.text.ToLower().Trim())));


                return toReturnList.OrderBy(elm => elm.orderby).ToList();
            }

            //Si on ne rentre dans aucuns des cas ci-dessus, retourner uniquement la consultation
            toReturnList.RemoveAll(el => el.text != AlbContextMenu.OFFCONT.DisplayName());
            toReturnList.ForEach(el => el.items.RemoveAll(m => !AlbOpConstants.ListActionsModeConsultation.Contains(m.text.ToLower().Trim())));


            return toReturnList.OrderBy(elm => elm.orderby).ToList();

            #endregion
        }

        private List<ModeleListItem> GetMenuSectionSpecifique(AlbContextMenu section)
        {
            var result = MvcApplication.AlbAllContextMenuUsers != null
              ? AlbTransverse.CleanMenus(MvcApplication.AlbAllContextMenuUsers).Find(
                ctxMenu => (ctxMenu.Utilisateur.ToLower() == AlbSessionHelper.ConnectedUser.ToLower()
                    && ctxMenu.text == section.DisplayName() && ctxMenu.type == "group"))
              : null;
            if (result != null && result.items != null)
                return result.items;
            return null;
        }

        private List<ModeleListItem> GetMenuSectionClause(string typeOffre, string user)
        {
            var result = new List<ModeleListItem>();
            result.Add(new ModeleListItem { action = null, alias = "clausier", icon = "defaultIcon", items = null, orderby = 1, text = "Clausier", type = null, typeOffreContrat = typeOffre, Utilisateur = user, width = null });
            result.Add(new ModeleListItem { action = null, alias = "txtlibre", icon = "defaultIcon", items = null, orderby = 2, text = "Texte libre", type = null, typeOffreContrat = typeOffre, Utilisateur = user, width = null });
            result.Add(new ModeleListItem { action = null, alias = "piecejointe", icon = "defaultIcon", items = null, orderby = 3, text = "Clause de pièce jointe", type = null, typeOffreContrat = typeOffre, Utilisateur = user, width = null });
            return result;
        }

        private List<ModeleListItem> GetMenuSectionFormuleOffre(string user, bool readOnly, int nbOptions)
        {
            var result = new List<ModeleListItem>();
            string type = AlbConstantesMetiers.TYPE_OFFRE;
            if (!readOnly)
            {
                if (nbOptions < Option.MaxNbByFormula)
                {
                    result.Add(new ModeleListItem { action = null, alias = "createOption", icon = "defaultIcon", items = null, orderby = 1, text = "Créer option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                }
                result.Add(new ModeleListItem { action = null, alias = "deleteFormule", icon = "deleteFormule", items = null, orderby = 2, text = "Supprimer formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            }
            return result;
        }

        private List<ModeleListItem> GetMenuSectionFormuleContrat(string user, bool readOnly, bool blockConditions, bool suppFormule, bool isSorti)
        {
            var result = new List<ModeleListItem>();
            string type = AlbConstantesMetiers.TYPE_CONTRAT;
            if (readOnly || isSorti)
            {
                result.Add(new ModeleListItem { action = null, alias = "consultformule", icon = "consulterFormule", items = null, orderby = 1, text = "Consulter formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                if (!isSorti && !blockConditions)
                    result.Add(new ModeleListItem { action = null, alias = "conditionformule", icon = "conditionsFormule", items = null, orderby = 1, text = "Conditions formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            }
            else
            {
                if (!isSorti)
                {
                    result.Add(new ModeleListItem { action = null, alias = "updateformule", icon = "updateFormule", items = null, orderby = 1, text = "Modifier formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                    if (!blockConditions)
                    {
                        result.Add(new ModeleListItem { action = null, alias = "conditionformule", icon = "conditionsFormule", items = null, orderby = 2, text = "Conditions formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                        if (suppFormule)
                            result.Add(new ModeleListItem { action = null, alias = "deleteFormule", icon = "deleteFormule", items = null, orderby = 5, text = "Supprimer formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                    }
                }
            }
            return result;
        }

        private List<ModeleListItem> GetMenuSectionFormule(string type, string user, bool readOnly, bool blockConditions, bool suppFormule, bool isSorti)
        {
            var result = new List<ModeleListItem>();

            if (readOnly || isSorti)
            {
                result.Add(new ModeleListItem { action = null, alias = "consultformule", icon = "consulterFormule", items = null, orderby = 1, text = "Consulter formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                if (!isSorti)
                    result.Add(new ModeleListItem { action = null, alias = "conditionformule", icon = "conditionsFormule", items = null, orderby = 1, text = "Conditions formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            }
            else
            {
                if (!isSorti)
                {
                    result.Add(new ModeleListItem { action = null, alias = "updateformule", icon = "updateFormule", items = null, orderby = 1, text = "Modifier formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                    if (!blockConditions)
                    {
                        result.Add(new ModeleListItem { action = null, alias = "conditionformule", icon = "conditionsFormule", items = null, orderby = 2, text = "Conditions formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                        //result.Add(new ModeleListItem { action = null, alias = "copyFormule", icon = "duplicateFormule", items = null, orderby = 3, text = "Dupliquer formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                        //result.Add(new ModeleListItem { action = null, alias = "createOption", icon = "defaultIcon", items = null, orderby = 4, text = "Créer option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                        if (suppFormule)
                            result.Add(new ModeleListItem { action = null, alias = "deleteFormule", icon = "deleteFormule", items = null, orderby = 5, text = "Supprimer formule", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                    }
                }
            }
            return result;
        }

        public List<ModeleListItem> GetMenuForOption(string user, bool readOnly, int nbOptions)
        {
            var result = new List<ModeleListItem>();
            /*
             * comment: waiting for CP edition
             *
            string type = AlbConstantesMetiers.TYPE_OFFRE;
            if (readOnly) {
                result.Add(new ModeleListItem { action = null, alias = "consultoption", icon = "consulterOption", items = null, nomMenu = null, orderby = 1, text = "Consulter option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                result.Add(new ModeleListItem { action = null, alias = "conditionsOption", icon = "conditionsOption", items = null, nomMenu = null, orderby = 2, text = "Conditions option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            }
            else {
                result.Add(new ModeleListItem { action = null, alias = "updateOption", icon = "defaultIcon", items = null, nomMenu = null, orderby = 1, text = "Modifier option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                result.Add(new ModeleListItem { action = null, alias = "conditionsOption", icon = "conditionsOption", items = null, nomMenu = null, orderby = 2, text = "Conditions option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                if (nbOptions < Option.MaxNbByFormula) {
                    result.Add(new ModeleListItem { action = null, alias = "duplicateOption", icon = "duplicateOption", items = null, nomMenu = null, orderby = 3, text = "Dupliquer option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
                }
                result.Add(new ModeleListItem { action = null, alias = "deleteOption", icon = "deleteOption", items = null, nomMenu = null, orderby = 4, text = "Supprimer option", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            }*/
            return result;
        }

        private List<ModeleListItem> GetMenuSectionInventaire(string type, string user, string invenId)
        {
            var result = new List<ModeleListItem>();
            result.Add(new ModeleListItem { action = null, alias = "saveInven", icon = "saveInventaire", items = null, orderby = 1, text = "Sauvegarder", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            result.Add(new ModeleListItem { action = null, alias = "delInven", icon = "deleteInventaire", items = null, orderby = 2, text = "Supprimer", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            return result;
        }

        private List<ModeleListItem> GetMenuSectionHeaderInventaire(string type, string user)
        {
            var result = new List<ModeleListItem>();
            result.Add(new ModeleListItem { action = null, alias = "addInven", icon = "addInventaire", items = null, orderby = 1, text = "Ajouter", type = null, typeOffreContrat = type, Utilisateur = user, width = null });
            return result;
        }

        private string BuildMenuString(List<ModeleListItem> listeMenu, string type = "", string etat = "", string brche = "", bool menuSearch = true, string sit = "", string typeAccord = "", bool generDoc = false, string typeAvt = "", string periodicite = "", string menuText = "", string numAvt = "")
        {
            var jsonMenu = string.Empty;
            if (listeMenu?.Any() ?? false)
            {
                int cptSep = 1;
                int cptSubMenu = 1;
                var rightAvenant = AlbOpConstants.ClientWorkEnvironment != AlbOpConstants.OPENV_PROD || (CacheUserRights.UserRights?.Any(el => el.TypeDroit == TypeDroit.M.ToString()) ?? false);
                var menuItems = new List<string>();
                foreach (ModeleListItem menu in listeMenu)
                {
                    switch (menu.type?.Trim().ToLower())
                    {
                        case "group":
                            if (AlbContextMenu.AVENANT.DisplayName() == menu.text && rightAvenant && !generDoc || AlbContextMenu.AVENANT.DisplayName() != menu.text)
                            {
                                var subMenu = BuildMenuString(menu.items, type, etat, brche, sit: sit, typeAccord: typeAccord, generDoc: generDoc, periodicite: periodicite, typeAvt: typeAvt, menuText: menu.text, numAvt: numAvt);
                                if (!string.IsNullOrEmpty(subMenu) && subMenu != "{}")
                                {
                                    menuItems.Add("\"subMenu" + cptSubMenu + "\":{\"name\":\"" + menu.text + "\", \"items\": " + subMenu + " }");
                                    cptSubMenu++;
                                }
                            }
                            break;
                        case "splitline":
                            menuItems.Add("\"sep" + cptSep + "\": \"---------\"");
                            cptSep++;
                            break;
                        default:
                            bool alwMenu = true;
                            if (AlbContextMenu.REPRISE.DisplayName() == menu.text &&
                                (sit == "W" || sit == "N") && ((typeAccord != "" && typeAccord != "N") || (typeAvt != AlbConstantesMetiers.TRAITEMENT_AFFNV && typeAvt != AlbConstantesMetiers.TRAITEMENT_OFFRE)))
                                alwMenu = false;
                            if (AlbContextMenu.OPMODIFIER.DisplayName() == menu.text && sit != "X" && sit != "A" && sit != string.Empty)
                                alwMenu = false;
                            if (AlbContextMenu.CONSULTER.DisplayName() != menu.text && generDoc)
                            {
                                alwMenu = false;
                            }
                            if (AlbContextMenu.AVNRS.DisplayName() == menu.text && (periodicite == "U" || periodicite == "E"))
                                alwMenu = false;
                            if ((typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGUL || typeAvt == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF) &&
                                AlbContextMenu.CONSULTER.DisplayName() == menu.text)
                                alwMenu = false;
                            if (AlbContextMenu.AVNRM.DisplayName() == menu.text &&
                                (etat != "V" || sit != "X"))
                                alwMenu = false;
                            //Ouverture systématique de l'option "Remise en vigueur" & "Résiliation"
                            if (AlbContextMenu.AVENANT.DisplayName() == menuText &&
                                AlbContextMenu.AVNRM.DisplayName() != menu.text &&
                                AlbContextMenu.AVNRS.DisplayName() != menu.text &&
                                (typeAccord == "" || typeAccord == "N") &&
                                Convert.ToInt32(numAvt) == 0)
                                alwMenu = false;

                            if (menu.text.Trim().ToLower() == "régularisation" && (typeAvt != "REGUL" && typeAvt != "AVNRG") && etat != "V")
                            {
                                alwMenu = false;
                            }

                            //var user = ";" + AlbSessionHelper.ConnectedUser + ";";
                            //if (menu.text.Trim().ToLower() == "avenant de remise en vigueur" && MvcApplication.USER_REMVIGUEUR.IndexOf(user) < 0)
                            //    alwMenu = false;

                            var alwType = type == AlbConstantesMetiers.TYPE_MENU_POLICE ? AlbConstantesMetiers.TYPE_MENU_CONTRAT : type;

                            var alwEtat = !string.IsNullOrEmpty(menu.AlwEtat) ? menu.AlwEtat.PadRight(10, ' ') : string.Empty;
                            alwEtat = !string.IsNullOrEmpty(menu.AlwEtat) ?
                                                alwType == AlbConstantesMetiers.TYPE_MENU_OFFRE ? alwEtat.Substring(0, 5) :
                                                alwType == AlbConstantesMetiers.TYPE_MENU_CONTRAT ? alwEtat.Substring(5, 5) : alwEtat :
                                                string.Empty;

                            var alwBrche = !string.IsNullOrEmpty(menu.AlwBranche) ? menu.AlwBranche.PadRight(10, ' ') : string.Empty;
                            brche = brche != "CO" ? "HCO" : "CO";

                            if (alwMenu && (((!string.IsNullOrEmpty(menu.typeOffreContrat)) &&
                                (menu.typeOffreContrat == alwType || menu.typeOffreContrat == "*") &&
                                (!string.IsNullOrEmpty(alwEtat)) &&
                                (alwEtat.Contains(etat) || alwEtat.Trim() == "*" || alwEtat.Trim() == "*    *") &&
                                (!string.IsNullOrEmpty(alwBrche)) &&
                                (alwBrche.Trim() == "*" || alwBrche.Trim() == "*    *" || alwBrche == brche)) ||
                                !menuSearch))
                            {
                                menuItems.Add(" \"" + menu.alias + "\":{ \"name\" : \"" + menu.text + "\", \"icon\" : \"" + (string.IsNullOrEmpty(menu.icon) ? "defaultIcon" : menu.icon) + "\"} ");
                            }
                            break;
                    }
                }
                if (menuItems.Any(x => x.ContainsChars()))
                {
                    jsonMenu = string.Join(",", menuItems);
                }
            }
            return jsonMenu.IsEmptyOrNull() ? null : ("{" + jsonMenu + "}");
        }

        #endregion
    }
}
