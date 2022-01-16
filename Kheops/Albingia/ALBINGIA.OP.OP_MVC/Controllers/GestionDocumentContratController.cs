using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestionDocumentContrat;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OPServiceContract.ITraitementAffNouv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class GestionDocumentContratController : ControllersBase<ModeleGestionDocumentContratPage>
    {
        #region Membres statiques

        private static List<AlbSelectListItem> _lstFiltreTypeDocument;
        private static List<AlbSelectListItem> LstFiltreTypeDocument
        {
            get
            {
                if (_lstFiltreTypeDocument != null)
                    return _lstFiltreTypeDocument;
                else
                {
                    _lstFiltreTypeDocument = new List<AlbSelectListItem>();
                    _lstFiltreTypeDocument.Add(new AlbSelectListItem
                    {
                        Selected = true,
                        Text = "Tous les documents",
                        Title = "Tous les documents",
                        Value = "*"
                    });
                    _lstFiltreTypeDocument.Add(new AlbSelectListItem
                    {
                        Selected = false,
                        Text = "Sauf avis d'échéance",
                        Title = "Sauf avis d'échéance",
                        Value = "AVISECH"
                    });
                    _lstFiltreTypeDocument.Add(new AlbSelectListItem
                    {
                        Selected = false,
                        Text = "Originaux seulement",
                        Title = "Originaux seulement",
                        Value = "O"
                    });
                    return _lstFiltreTypeDocument;
                }
            }
        }

        private static List<AlbSelectListItem> LstFiltreLot
        {
            get
            {
                List<AlbSelectListItem> toReturn = new List<AlbSelectListItem>();
                toReturn.Add(new AlbSelectListItem
                     {
                         Selected = true,
                         Text = "Tous les lots",
                         Title = "Tous les lots",
                         Value = "*"
                     });
                return toReturn;
            }
        }

        #endregion

        #region Méthodes Publiques

        
        [ErrorHandler]
        public ActionResult OpenGestionDocumentContrat(string id)
        {
            id = InitializeParams(id);

            #region vérification des paramètres
            if (string.IsNullOrEmpty(id))
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", true, true);
            var tIds = id.Split('_');
            if (tIds.Length != 3)
                throw new AlbFoncException("Erreur de chargement de la page:identifiant de la page est null", true, true);
            var codeOffre = tIds[0];
            var version = tIds[1];
            var type = tIds[2];
            int iVersion;
            if (string.IsNullOrEmpty(codeOffre) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(type))
                throw new AlbFoncException("Erreur de chargement de la page:Un des trois paramètres est vide (numéro coffre/Contrat, Version, Type)", true, true);
            if (!int.TryParse(version, out iVersion))
                throw new AlbFoncException("Erreur de chargement de la page:Version non valide", true, true);
            #endregion

            switch (type)
            {
                case AlbConstantesMetiers.TYPE_OFFRE:
                    throw new AlbFoncException("Erreur de chargement de la page : cette page ne peut être ouverte à partir d'une offre", trace: true, sendMail: true, onlyMessage: true);
                case AlbConstantesMetiers.TYPE_CONTRAT:
                    var typeAvt = GetAddParamValue(base.model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            base.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            base.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            base.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            base.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            base.model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    break;
                default:
                    throw new AlbFoncException("Erreur de chargement de la page : cette page ne peut être ouverte qu'à partir d'un contrat", trace: true, sendMail: true, onlyMessage: true);
            }
            ModeleGestionDocumentContrat modelGestionDocument = LoadInfoGestionDocumentContrat(codeOffre, version, type, model.ModeNavig);
            return PartialView("GestionDocumentContrat", modelGestionDocument);
        }

        [ErrorHandler]
        public ActionResult GetListeDocumentsFiltree(string codeOffre, string version, string type, string filtreTypeDocument, string filtreLot)
        {
            var result = GetListeDocuments(codeOffre, version, type);
            if (result != null)
            {
                if (!string.IsNullOrEmpty(filtreTypeDocument) && filtreTypeDocument != "*")
                {
                    switch (filtreTypeDocument)
                    {
                        case "AVISECH":
                            result = result.Where(elm => (elm.TypeDocumentCode != filtreTypeDocument)).ToList(); break;
                        case "O":
                            result = result.Where(elm => (elm.TypeEdition == filtreTypeDocument)).ToList(); break;
                        default:
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(filtreLot) && filtreLot != "*")
                {
                    result = result.Where(elm => elm.Lot == filtreLot).ToList();
                }
            }
            return PartialView("LstDocuments", result);
        }

        [ErrorHandler]
        public ActionResult GetListePiecesJointesDocument(string idDocument)
        {
            Int64 id = 0;
            if (Int64.TryParse(idDocument, out id))
            {
                var result = GetListePiecesJointes(idDocument);
                return PartialView("PiecesJointes", result);
            }
            else
                throw new AlbFoncException("Impossible d'afficher les pièces jointes de ce document", trace: false, sendMail: false, onlyMessage: true);

        }

        #endregion

        #region Méthodes Privées

        private ModeleGestionDocumentContrat LoadInfoGestionDocumentContrat(string codeOffre, string version, string type, string modeNavig)
        {
            ModeleGestionDocumentContrat toReturn = new ModeleGestionDocumentContrat();
            toReturn.CodeContrat = codeOffre.ToUpper();
            toReturn.VersionContrat = version;
            toReturn.ListFiltreLotEdition = LstFiltreLot;
            toReturn.ListFiltreTypeDocument = LstFiltreTypeDocument;
            toReturn.ListDocuments = GetListeDocuments(codeOffre, version, type);
            //Mise à jour de la liste du filtre lot
            if (toReturn.ListDocuments != null)
            {
                toReturn.ListDocuments.Select(m => new { m.Lot })
                        .Distinct()
                        .ToList()
                        .ForEach(elm => toReturn.ListFiltreLotEdition.Add(new AlbSelectListItem
                        {
                            Selected = false,
                            Text = elm.Lot,
                            Value = elm.Lot,
                            Title = elm.Lot
                        }));
            }
            return toReturn;
        }

        private List<ModeleLigneDocument> GetListeDocuments(string codeContrat, string versionContrat, string typeContrat)
        {
            List<ModeleLigneDocument> toReturn = new List<ModeleLigneDocument>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var lstResult = serviceContext.GetListeDocuments(codeContrat, versionContrat, typeContrat, GetUser());
                if (lstResult != null)
                    lstResult.ForEach(elm => toReturn.Add((ModeleLigneDocument)elm));
            }
            toReturn.ForEach(elm => elm.DDateEnvoi = AlbConvert.ConvertIntToDate(elm.DateEnvoi));
            return toReturn;
        }

        private List<ModeleLignePieceJointe> GetListePiecesJointes(string idDocument)
        {
            List<ModeleLignePieceJointe> toReturn = new List<ModeleLignePieceJointe>();
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonAffaireNouvelle>())
            {
                var serviceContext=client.Channel;
                var lstResult = serviceContext.GetListePiecesJointes(idDocument);
                if (lstResult != null)
                    lstResult.ForEach(elm => toReturn.Add((ModeleLignePieceJointe)elm));
            }

            return toReturn;
        }



        #endregion
    }
}
