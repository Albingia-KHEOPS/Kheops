using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using OP.DataAccess;
using OP.WSAS400.DTO.Ecran.CreationSaisie;
using OP.WSAS400.DTO.NavigationArbre;
using OP.WSAS400.DTO.Offres;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Globalization;
using System.ServiceModel.Activation;

namespace OP.Services.SaisieCreationOffre
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CreationOffre : ICreationOffre
    {
        #region Création Saisie



        /// <summary>
        /// Creations the saisie get.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public CreationSaisieGetResultDto CreationSaisieGet(CreationSaisieGetQueryDto query)
        {
            var toReturn = new CreationSaisieGetResultDto();
            toReturn.Branches = BrancheRepository.ObtenirBranches(string.Empty, string.Empty);
            return toReturn;
        }

        public CreationSaisieSetResultDto CreationSaisieEnregistrement(CreationSaisieSetQueryDto query, string user)
        {
            string retourMsg = string.Empty;
            string libTrace = string.Empty;

            if (query.Offre.CopyMode)
            {
                if (!query.Offre.CodeOffre.IsEmptyOrNull())
                {
                    libTrace = $"Création via {query.Offre.CodeOffre}-{query.Offre.Version}";
                }
                else
                {
                    libTrace = $"Création via {query.Offre.CodeOffreCopy}-{query.Offre.VersionCopy}";
                }
            }
            else
            {
                if (!query.Offre.CodeOffre.IsEmptyOrNull())
                {
                    libTrace = "Modification Saisie";
                }
                else
                {
                    libTrace = "Création Saisie";
                }
            }

            var toReturn = new CreationSaisieSetResultDto();
            string traitement = AlbConstantesMetiers.Traitement.CopyOffre.AsCode();
            string version = string.Empty;
            string copyCodeOffre = string.Empty;
            int copyVersion = 0;

            if (query.Offre.CopyMode)
            {
                if (!string.IsNullOrEmpty(query.Offre.CodeOffreCopy))
                {
                    copyCodeOffre = query.Offre.CodeOffreCopy;
                    copyVersion = Convert.ToInt32(query.Offre.VersionCopy);
                    query.Offre.CodeOffreCopy = query.Offre.CodeOffre;
                    query.Offre.VersionCopy = query.Offre.Version.HasValue ? query.Offre.Version.Value.ToString(CultureInfo.InvariantCulture) : null;
                    if (copyCodeOffre.Contains("CV"))
                    {
                        traitement = AlbConstantesMetiers.Traitement.CopyInNewOffre.AsCode();
                        query.Offre.CodeOffreCopy = null;
                        query.Offre.VersionCopy = null;
                    }
                }
                else
                {
                    copyCodeOffre = query.Offre.CodeOffre;
                    copyVersion = query.Offre.Version.Value;
                    traitement = AlbConstantesMetiers.Traitement.CopyInNewOffre.AsCode();
                }
            }
            var offre = query.Offre;
            if (offre != null)
            {
                retourMsg = new PoliceServices().SauvegardeOffre(offre, user);

                if (!string.IsNullOrEmpty(retourMsg))
                {
                    toReturn.RetourMsg = retourMsg;
                    return toReturn;
                }
                CommonRepository.ChangeSbr(offre.CodeOffre, offre.Version.Value.ToString(), offre.Type, user);
                toReturn.CodeOffre = offre.CodeOffre.Trim();

                #region Ajout de l'acte de gestion
                CommonRepository.AjouterActeGestion(offre.CodeOffre, offre.Version.Value.ToString(), offre.Type, 0, AlbConstantesMetiers.ACTEGESTION_GESTION, AlbConstantesMetiers.TRAITEMENT_OFFRE, libTrace, user);
                #endregion

                #region Arbre de navigation
                NavigationArbreRepository.SetTraceArbre(new TraceDto
                {
                    CodeOffre = offre.CodeOffre.PadLeft(9, ' '),
                    Version = offre.Version.Value,
                    Type = offre.Type,
                    EtapeGeneration = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Saisie),
                    NumeroOrdreDansEtape = 1,
                    NumeroOrdreEtape = 1,
                    Perimetre = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Saisie),
                    Risque = 0,
                    Objet = 0,
                    IdInventaire = 0,
                    Formule = 0,
                    Option = 0,
                    Niveau = string.Empty,
                    CreationUser = user,
                    PassageTag = "O",
                    PassageTagClause = string.Empty
                });
                #endregion
            }
            if (query.Offre.CopyMode)
            {
                OffreRepository.CopyOffreFromOffre(toReturn.CodeOffre, offre.Version.Value.ToString(), "O", copyCodeOffre, copyVersion.ToString(CultureInfo.InvariantCulture), DateTime.Now, user, traitement, AlbConstantesMetiers.TRAITEMENT_OFFRE);
            }

            return toReturn;
        }



        public string GetBrancheCibleOffre(string codeOffre, string version, string type, ModeConsultation modeNavig)
        {
            return OffreRepository.GetBrancheCibleOffre(codeOffre, version, type);
        }

        #endregion

        #region Création Template

        public OffreDto GetInfoTemplate(string idTemp)
        {
            return ParamTemplatesRepository.GetInfoTemplate(idTemp);

        }
        #endregion

        #region Informations de base transverse

        public void SaveInfoBase(OffreDto offre)
        {
            PoliceRepository.SaveInfoBase(offre);
        }

        #endregion
    }
}

