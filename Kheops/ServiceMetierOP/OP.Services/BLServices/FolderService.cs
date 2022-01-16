using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Tools;
using Mapster;
using OP.DataAccess;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Contrats;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OP.Services.BLServices
{
    public class FolderService
    {
        private readonly AffaireNouvelleRepository repository;
        private readonly ProgramAS400Repository pgmRepository;
        private readonly IAffairePort affaireService;

        public FolderService(AffaireNouvelleRepository repository, ProgramAS400Repository pgmRepository, IAffairePort affairePort)
        {
            this.repository = repository;
            this.pgmRepository = pgmRepository;
            this.affaireService = affairePort;
        }

        public ContratDto GetBasicFolder(Folder folder)
        {
            var data = this.repository.GetBasicFolder(folder);
            if (data != null)
            {
                return new ContratDto
                {
                    CodeContrat = data.Ipb,
                    Etat = data.Eta,
                    Situation = data.Sit,
                    VersionContrat = data.Alx,
                    Type = data.Typ,
                    NumInterneAvenant = (short)data.Avn,
                    TypeTraitement = data.Ttr,
                    CodePreneurAssurance = data.Asid,
                    Delegation = data.Budbu,
                    Inspecteur = data.UIn,
                    NomPreneurAssurance = data.Asnom,
                    Branche = data.Bra,
                    Cible = data.Cible,
                    Description = data.Ref
                };
            }
            return null;
        }

        public Affaire GetBasicAffaire(Folder folder, bool fromHistory = false)
        {
            this.repository.SetHistoryMode(fromHistory ? ActivityMode.Active : ActivityMode.Inactive);
            try
            {
                var data = this.repository.GetBasicFolder(folder);
                if (data != null)
                {
                    return new Affaire
                    {
                        CodeAffaire = data.Ipb,
                        Etat = data.Eta.ParseCode<EtatAffaire>(),
                        Situation = data.Sit.ParseCode<SituationAffaire>(),
                        CodeMotifSituation = data.Stf,
                        NumeroAliment = data.Alx,
                        TypeAffaire = data.Typ.ParseCode<AffaireType>(),
                        NumeroAvenant = data.Avn,
                        TypeTraitement = new Albingia.Kheops.OP.Domain.Referentiel.TypeTraitement { Code = data.Ttr },
                        Preneur = new Albingia.Kheops.OP.Domain.Affaire.Assure() { Code = data.Asid, NomAssure = data.Asnom },
                        //Delegation = data.Budbu,
                        //Inspecteur = data.UIn,
                        Branche = new Albingia.Kheops.OP.Domain.Referentiel.Branche() { Code = data.Bra },
                        Cible = new Albingia.Kheops.OP.Domain.Referentiel.Cible() { Code = data.Cible },
                        Descriptif = data.Ref,
                        CodeOffre = data.BaseOffre
                    };
                }
                return null;
            }
            catch
            {
                this.repository.ResetHistoryMode();
                throw;
            }
        }

        public bool IsCitrix(Folder folder)
        {
            string statut = this.repository.GetStatutKheops(folder);
            return statut.ToUpper() != "KHE";
        }

        public bool AllowCoAssureur(Folder folder, ModeConsultation mode)
        {
            return this.repository.GetCountByNPL(folder, new[] { "A", "E" }, mode) > 0;
        }

        #region Methods Contracts Kheops

        public ContractJsonDto CreationContractsKheops(PGMFolder pgmFolder, ContractJsonDto contract)
        {
            var anneeEffet = AlbConvert.ConvertStrToDate(contract.DateEffet.Debut).Value.Year.ToString() ?? string.Empty;

            //Récupération d'un numéro IPB
            contract.CodeAffaire = this.pgmRepository.NewIPBContract(pgmFolder, contract.Branche.Code, contract.Branche.Cible.Code, anneeEffet);

            //Création du contrat
            this.repository.CreationContractKheops(contract, pgmFolder.User);

            return contract;
        }

        public ContractJsonDto CreationOffersKheops(PGMFolder pgmFolder, ContractJsonDto offer)
        {
            var anneeEffet = AlbConvert.ConvertStrToDate(offer.DateEffet.Debut).Value.Year.ToString() ?? string.Empty;

            //Récupération d'un numéro IPB
            offer.CodeAffaire = CommonRepository.ObtenirNumeroPolice(DateTime.Now.Year, offer.Branche.Code, offer.Branche.Cible.Code, string.Empty, offer.Type);

            //Création de l'offre
            this.repository.CreationOfferKheops(offer, pgmFolder.User);

            //Modification de l'offre
            this.repository.ModificationOfferKheops(offer, pgmFolder.User);

            return offer;
        }

        public IEnumerable<Garanty> GetAllGaranties(Formule formule)
        {
            return formule.Volets.SelectMany(x => x.AllGaranties);
        }

        public void SetInfosGenContract(PGMFolder pgmFolder, ContractJsonDto contract)
        {
            //Save Informations de Base
            this.repository.UpdBaseContractKheops(contract, pgmFolder.User);
            //Save Informations Entête
            this.repository.UpdEnteteContractKheops(contract, pgmFolder.User);
            //Save Informations Obj
            this.repository.UpdObjContractKheops(contract, pgmFolder.User);
            if (pgmFolder.Type == AlbConstantesMetiers.TYPE_CONTRAT)
                //Add Acte Gestion
                this.repository.SaveActeGestionContract(contract, pgmFolder.User);
        }

        public void SetInfoCommission(PGMFolder pgmFolder, ContractJsonDto contract)
        {
            var infoCommissions = pgmRepository.LoadCommissions(pgmFolder);
            //Save Informations Commission
            this.repository.UpdCommission(contract, infoCommissions, pgmFolder.User);
        }

        public ContractJsonDto SetInfoRsqContract(PGMFolder pgmFolder, ContractJsonDto contract, Risque risque)
        {
            //Save Informations des Risques
            this.repository.GenerateRsqObjContract(contract, risque, pgmFolder.User);
            return contract;
        }

        public void UpdateLCIComplexe(ContractJsonDto contract, IEnumerable<Garanty> allGarantiesLCI)
        {
            this.repository.UpdateLCIComplexe(contract, allGarantiesLCI);
        }


        public void SaveIntercalaireFiles(PGMFolder pgmFolder, ContractJsonDto contract)
        {
            var urlFile = this.repository.GetUrlTypo("INTER");

            foreach (var file in contract.IntercalaireFiles)
            {
                if (!file.Url.IsEmptyOrNull())
                {
                    //Sauvegarde du fichier intercalaire
                    var fileName = $"{file.Url}";
                    var fileParts = fileName.Split('\\');

                    var toto = Path.GetDirectoryName(fileName);

                    var fileDoc = $"{contract.CodeAffaire.ToIPB()}__{contract.Aliment.PadLeft(4, '0')}__{contract.Type}__{fileParts[fileParts.Count() - 1]}";
                    fileDoc = fileDoc.Replace(" ", "-");

                    //if (IOFileManager.IsExistFile("C:\\Projets\\DocsConnecteur\\", fileParts[fileParts.Count() - 1]))
                    if (IOFileManager.IsExistFile(Path.GetDirectoryName(fileName), fileParts[fileParts.Count() - 1]))
                    {
                        IOFileManager.CopyFile($"{fileName}", $"{urlFile}\\{fileDoc}");
                        file.FileDoc = fileDoc;
                        file.FilePath = $"{urlFile}\\{fileDoc}";
                        if (file.Nom.Length > 60) file.Nom = file.Nom.Substring(0, 60);
                    }

                    var idDoc = this.repository.SaveDocsJoints(pgmFolder, contract, file);

                    //Création de la clause de pièce jointe
                    this.repository.InsertClauseJointe(pgmFolder, contract, idDoc);
                }
            }
        }

        #endregion
    }
}
