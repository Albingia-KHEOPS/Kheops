using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Clauses;
using Albingia.Kheops.OP.Domain.Document;
using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using static ALBINGIA.Framework.Common.Constants.AlbConstantesMetiers;

namespace Albingia.Kheops.OP.Application.Infrastructure.Services
{
    public class DocumentService : IDocumentPort
    {

        IDocumentRepository docRepo;
        IReferentialRepository refRepo;
        IAffaireRepository affRepo;
        IClauseRepository clauseRepo;


        public DocumentService(
            IDocumentRepository docRepo,
            IReferentialRepository refRepo,
            IAffaireRepository affRepo,
            IClauseRepository clauseRepo
        )
        {
            this.docRepo = docRepo;
            this.refRepo = refRepo;
            this.affRepo = affRepo;
            this.clauseRepo = clauseRepo;
        }
        public LotDocument GetDocs(AffaireId affaireId, bool work)
        {
            return docRepo.GetByAffaireId(affaireId, work).First();
        }
        public void GenerateDocuments(AffaireId affaireId, string service, string acteGes, string user, DateTime now, bool init, long[] docsId, int attesId, int regulId)
        {
            string codeOffre = affaireId.CodeAffaire;
            int version = affaireId.NumeroAliment;
            string type = affaireId.TypeAffaire.ToString();
            int codeAvn = affaireId.NumeroAvenant ?? 0;

            if (!init) { return; }

            var lot = docRepo.GetByAffaireId(affaireId, work: true).FirstOrDefault();
            var docExts = docRepo.GetDocExtByAffaireId(affaireId);
            var docGens = docRepo.GetDocGenByAffaireId(affaireId, work: true);

            List<CopyDoc> docsToCopy = new List<CopyDoc>();
            IEnumerable<DocumentGenere> docsKept = Enumerable.Empty<DocumentGenere>();

            if (lot != null)
            {
                docsKept = new IEnumerable<DocumentGenere>[] {
                    lot.Documents.Where(x => docsId.Contains(x.IdDoc)),
                    lot.Documents.Where(g => !(g.IsEnCourt && g.Service == service)),
                }.SelectMany(x => x).Distinct();
            }
            //Delete Old Docs in lot ==> at end of function

            var affaire = affRepo.GetById(affaireId);
            var typeTraitement = affaire.TypeTraitement;

            if ((codeAvn != 0 && acteGes != TypeActeDeGestion.Attestation && typeTraitement.Type != TraitementAffaire.RemiseEnVigueur) || typeTraitement.Code == TYPE_AVENANT_BNS)
            {
                acteGes = typeTraitement.Code;
            }
            var label = refRepo.GetValue<ActeGestion>(acteGes)?.Libelle ?? "";

            LotDocument newLot = new LotDocument()
            {
                AffaireId = affaireId,
                AA = 0,
                ActeDeGestion = acteGes,
                IsWorkData = true,
                Libelle = label,
                Numero = 0,
                NumeroActeDeGestion = 1,
                Situation = SituationClause.A,
                Creation = new Domain.Transverse.UpdateMetadata(user, DateTime.Now.Date),
                SituationMetadata = new Domain.Transverse.UpdateMetadata(user, DateTime.Now.Date),
                MiseAJour = new Domain.Transverse.UpdateMetadata(user, DateTime.Now.Date)
            };

            foreach (var kept in docsKept)
            {
                kept.IdDocLot = 0;
            }

            newLot.Documents.AddRange(docsKept);

            IEnumerable<Clause> allClauses = clauseRepo.GetClauses(affaireId).ToList();

            var clauses =
                allClauses
                    .Where(x => !string.IsNullOrEmpty(x.Contexte.TypoDoc) &&
                        x.Situation != SituationClause.None
                        &&
                        (x.Contexte.Code != "SY05CP"
                        || x.Situation == SituationClause.Valide
                        )
                     )
                    .Where(GetFilter(acteGes))
                    .Where(cl => !docsKept.OfType<DocumentGenere>().Any(x => x.CodeClause == cl.NomClause.ComposedName))
                    .OrderBy(x => x.Chapitre)
                        .ThenBy(x => x.Souschapitre)
                        .ThenBy(x => x.NuneroImpression)
                        .ThenBy(x => x.NumeroOrdonnancement)
                    .ToList();

            foreach (var clause in clauses)
            {
                var labelClause = clause.ParamClause?.Libelle;
                var nature = clause.Nature;

                if (clause.Situation == SituationClause.Valide && clause.Contexte.Code.StartsWith("SY"))
                {
                    nature = NatureGeneration.Obligatoire;
                }

                var key = $"DOC_{clause.Contexte.TypoDoc}";
                var chemin = ComposePath(docRepo.GetChemin(key));
                var docName = clause.ParamClause?.NomDoc ?? "";

                var typoDoc = refRepo.GetValue<TypoDocument>(clause.Contexte.TypoDoc);
                StatutGeneration statutGeneration = StatutGeneration.N;
                DocumentExterne docExt = null;
                if (clause.TypeGenerationDoc == TypeGenerationDoc.Externe && clause.IdDoc != 0)
                {
                    docExt = docExts.FirstOrDefault(x => x.IdDoc == clause.IdDoc);
                    if (docExt != null)
                    {
                        labelClause = docExt.Libelle;
                        docName = docExt.Nom;
                        chemin = docExt.Chemin.Replace(docExt.Nom, "");
                        statutGeneration = StatutGeneration.G;
                    }
                }
                if (clause.Contexte.TypoDoc == "REGUL" && clause.IdDoc != 0)
                {

                    var docs = docRepo.GetByAffaireId(affaireId, true);
                    var doc = docGens.FirstOrDefault(x => x.IdDoc == clause.IdDoc);

                    var oldName = doc.Chemin.Trim();
                    var newDocName = oldName.Replace("\\", "/");
                    var parts = newDocName.Substring(newDocName.LastIndexOf('/'));
                    var newFilePath = chemin + $"{codeOffre}_{version.ToString().PadLeft(4, '0')}_{type}/Temp/" + parts.Last();
                    chemin = newFilePath;

                    var docToCopy = new CopyDoc()
                    {
                        AffaireId = affaireId,
                        OldPath = oldName,
                        NewName = newFilePath,
                        Table = "KPDOCW"
                    };
                    statutGeneration = StatutGeneration.G;
                }


                var maitre = "";
                var idLien = 0;
                var typeLien = clause.Contexte.TypoDoc;

                if (acteGes == "ATTES")
                {
                    maitre = "KPATT";
                    idLien = attesId;
                    typeLien = "ATTES";
                }
                else if (acteGes == "REGUL")
                {
                    maitre = "KPRGU";
                    idLien = regulId;
                }
                    newLot.Documents.Add(new DocumentGenere
                    {
                        Chemin = chemin,
                        CodeClause = clause.NomClause.ComposedName,
                        VersionClause = clause.NomClause.NumeroVersion,
                        Destinataire = new Destinataire { Type = TypeDestinataire.Courtier, Courtier = new Courtier { Code = affaire.CourtierGestionnaire.Code }, IsPrincipal = true },
                        Envoi = new Domain.Transverse.UpdateMetadata(user, DateTime.Now),
                        Etape = EtapeGeneration.FIN,
                        IsAccompagnement = false,
                        IsAjoute = false,
                        IsEnCourt = true,
                        IsImprimable = typoDoc.Print.AsBool(),
                        IsLibre = typoDoc.Libre.AsBool("L"),
                        IsTransforme = false,
                        Libelle = labelClause,
                        Lien = new LienDocument
                        {
                            Type = maitre,
                            Id = idLien
                        },
                        NatureGeneration = nature,
                        Nom = docName,
                        NumeroOrdre = (short)clause.Contexte.NumOrdre,
                        ParentId = 0,
                        Service = "PRODU",
                        SituationMetadata = new Domain.Transverse.UpdateMetadata(user, DateTime.Now),
                        SituationDoc = SituationClause.Valide,
                        SituationLot = ComputeSituation(nature),
                        Tampon = new Tampon() { Code = Tampon.TamponValues.Original.AsCode() },
                        TypeEnvoi = refRepo.GetValue<TypeEnvoi>(""),
                        StatutGeneration = statutGeneration,
                        TypoDocument = typoDoc,
                        DocumentExterne =
                            clause.TypeGenerationDoc == TypeGenerationDoc.Externe
                                ? docExt
                                : null
                    });
            }
            if (lot != null) { docRepo.DeleteLot(lot); }
            docRepo.SaveLot(newLot, user);

            //plus de sauvegarde en base
            //docRepo.AddCopyDoc(docsToCopy);
            CopierDocuments(docsToCopy);

        }

        private static string ComposePath(Chemin chem)
        {
            return "//" + $"{chem.Server}/{chem.Root}/{chem.Environment}/{chem.Location}/".Replace("\\", "/").Replace("//", "/");
        }

        public void CopierDocuments(IEnumerable<CopyDoc> documentsToCopy)
        {
            // plus de sauvegarde en base
            //Récupération des références des fichiers à copier
            //var documentsToCopy = this.docRepo.GetCopyDoc(id);

            //copie physique des documents
            foreach (var doc in documentsToCopy)
            {
                if (string.IsNullOrEmpty(doc.OldPath) || string.IsNullOrEmpty(doc.NewName))
                {
                    continue;
                }

                var oldPathFull = Path.GetFullPath(doc.OldPath);
                var newPathFull = Path.GetFullPath(doc.NewName);

                if (string.Compare(oldPathFull, newPathFull, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    //Don't copy if source and destination are the same
                    continue;
                }

                var pathNewFile = Path.GetDirectoryName(newPathFull);
                if (!Directory.Exists(pathNewFile))
                {
                    // Ensure Directory
                    Directory.CreateDirectory(pathNewFile);
                }

                if (File.Exists(newPathFull))
                {
                    // remove before overwritting
                    File.Delete(Path.Combine(pathNewFile, Path.GetFileName(newPathFull)));
                }

                try
                {
                    // try to copy
                    File.Copy(oldPathFull, newPathFull);
                }
                catch (Exception)
                {
                    throw;

                    // TODO : check this code
                    //// Suppression du ficher dans la table KPDOCEXT
                    //if (doc.AffaireId.CodeAffaire.Trim().StartsWith("CV"))
                    //{
                    //    string delFile = string.Format("DELETE FROM {0} WHERE {1} = {2}", doc.TableCible, doc.TableCible.Trim() == "KPDOCEXT" ? "KERID" : "KEQID", doc.NewGuid);
                    //    DbBase.Settings.ExecuteNonQuery(CommandType.Text, delFile);
                    //}
                    ////throw;
                }

            }

            // plus de sauvegarde en base
            //Effacer les références des fichiers à copier
            //docRepo.ClearCopyDoc(id);
        }


        private static SituationDocumentLot ComputeSituation(NatureGeneration nature)
            =>
            new NatureGeneration[] { NatureGeneration.Obligatoire, NatureGeneration.Proposee }.Contains(nature)
                ? SituationDocumentLot.O
                : SituationDocumentLot.N;

        private Func<Clause, bool> GetFilter(string acteGes)
        {
            switch (acteGes)
            {
                case "":
                case "AVNMD":
                    return x => (
                        (x.Perimetre != Etapes.Attestation || x.EtapeDeGeneration != Etapes.Attestation)
                        && x.EtapeDeGeneration != Etapes.Regule
                        && x.EtapeDeGeneration != Etapes.Resiliation
                    );
                case "REGUL":
                //case "PB":
                //case "BNS":
                    return x => (
                        x.EtapeDeGeneration == Etapes.Regule
                        && (
                            new[] {
                                "SY07REGUL",
                                "SY55LETTR",
                                "SY50BSR"
                            }.Contains(x.Contexte?.Code?.ToUpper())
                        )
                    );
                case "PB":
                    return x => (
                        x.EtapeDeGeneration == Etapes.Regule
                        && (
                            new[] {
                                "SY07PB",
                                "SY50BSR",
                                "SY55LETPB"
                            }.Contains(x.Contexte?.Code?.ToUpper())
                        )
                    );
                case "BNS":
                    return x => (
                        x.EtapeDeGeneration == Etapes.Regule
                        && (
                            new[] {
                                "SY55LETBNS"
                            }.Contains(x.Contexte?.Code?.ToUpper())
                        )
                    );
                case "AVNRS":
                    return x => (
                        x.EtapeDeGeneration == Etapes.Resiliation
                        && (
                            new[] { "SY55LETTR" }.Contains(x.Contexte?.Code?.ToUpper())
                        )
                    );
                case "ATTES":
                    return x =>
                    (x.Perimetre == Etapes.Attestation || x.EtapeDeGeneration == Etapes.Attestation)
                    && (
                        new[] { "SY70ATTES" }.Contains(x.Contexte?.Code?.ToUpper())
                    );
                case "AVNRM":
                    return x =>
                    (x.EtapeDeGeneration == Etapes.RemiseEnVigueur)
                    && (
                        new[] {
                            "SYRVIGUEUR",
                            "SY55LETTR",
                            "SY50BS"
                        }.Contains(x.Contexte?.Code?.ToUpper())
                    );
                default:
                    return x => true;
            }
        }

        public IEnumerable<LotDocument> GetLots(AffaireId affaireId, bool work)
        {
            IEnumerable<LotDocument> lot = docRepo.GetByAffaireId(affaireId, work: true).OrderByDescending(x => x.IdLot).ToList();
            return lot;
        }
    }
}
