using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Extension;
using ALBINGIA.Framework.Common.Extensions;
using Albingia.Kheops.OP.Domain;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Data;
using static Albingia.Kheops.OP.DataAdapter.AffaireRepository;
using Albingia.Kheops.OP.Domain.Document;
using Albingia.Kheops.OP.Domain.Transverse;
using Albingia.Kheops.OP.DataAdapter.DataModel.Interface;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using Albingia.Kheops.OP.Application.Contracts;

namespace Albingia.Kheops.OP.DataAdapter
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly KpdocwRepository dwRepo;
        private readonly KpDocRepository dRepo;
        private readonly KpdoclwRepository lwRepo;
        private readonly KpDocLRepository lRepo;
        private readonly KpdocldwRepository ldwRepo;
        private readonly KpDocLDRepository ldRepo;

        private readonly YAssNomRepository ynrepo;
        private readonly YAssureRepository yarepo;
        private readonly YcourtiRepository ycourtiRepo;
        private readonly KpDocExtRepository dextRepo;
        private readonly KCheminRepository cheminRepo;
        private readonly KpCopDcRepository copRepo;



        private readonly IReferentialRepository refRepo;
        private readonly IGenericCache cache;

        public DocumentRepository(
            KpdocwRepository dwRepo,
            KpDocRepository dRepo,
            KpDocExtRepository dextRepo,
            KpdoclwRepository lwRepo,
            KpDocLRepository lRepo,
            KpdocldwRepository ldwRepo,
            KpDocLDRepository ldRepo,
            YAssNomRepository ynrepo,
            YAssureRepository yarepo,
            YcourtiRepository ycourtiRepo,
            KCheminRepository cheminRepo,
            KpCopDcRepository copRepo,
            IReferentialRepository refRepo,
            IGenericCache cache)
        {
            this.dwRepo = dwRepo;
            this.dRepo = dRepo;
            this.dextRepo = dextRepo;
            this.lwRepo = lwRepo;
            this.lRepo = lRepo;
            this.ldwRepo = ldwRepo;
            this.ldRepo = ldRepo;
            this.refRepo = refRepo;
            this.ynrepo = ynrepo;
            this.yarepo = yarepo;
            this.ycourtiRepo = ycourtiRepo;
            this.cache = cache;
            this.copRepo = copRepo;
            this.cheminRepo = cheminRepo;
        }
        public Assure GetAssureProxy(int code, int numero)
        {
            return new AssureProxy(code, numero, this.yarepo, this.ynrepo, this.refRepo);
        }

        public void DeleteLot(LotDocument lot)
        {
            var (docsDb, docsextDb, lotsDb, lotDetailsDb) = GetData(lot.AffaireId, lot.IsWorkData);
            var lotDb = lotsDb.FirstOrDefault(l => l.Kelid == lot.IdLot);
            if (lotDb == null)
            {
                throw new Exception($"Lot #{lot.IdLot} non trouvé");
            }
            var lotrepo = lot.IsWorkData ? (IKpDocLRepository)lwRepo : lRepo;
            foreach (var docLot in lotDetailsDb)
            {
                KpDoc docGenDb = docsDb.FirstOrDefault(d => d.Keqid == docLot.Kemtypl);
                //KpDocExt docExtDb = docsextDb.FirstOrDefault(d => docLot.Kemtypd == "E" && d.Kerid == docLot.Kemtypl);
                Cleanup(lot.IsWorkData, docLot, docGenDb);
            }
            lotrepo.Delete(lotDb);
        }

        public IEnumerable<LotDocument> GetByAffaireId(AffaireId id, bool work)
        {
            var (docDb, docextDb, lotsDb, lotDetailsDb) = GetData(id, work);
            return lotsDb.Select(x => MapLot(x, lotDetailsDb, docDb, docextDb, work));
        }

        public IEnumerable<DocumentExterne> GetDocExtByAffaireId(AffaireId id)
        {
            var docextDb = this.dextRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment);

            return docextDb.Select(x => MapDocExt(x));
        }

        private DocumentExterne MapDocExt(KpDocExt docExt)
        {
            if (docExt is null) {
                return null;
            }
            var doc = new DocumentExterne()
            {
                Reference = docExt.Kerref,
                TypoDocument = refRepo.GetValue<TypoDocument>(docExt.Kertypo),
                Chemin = docExt.Kerchm,
                Nom = docExt.Kernom,
                Libelle = docExt.Kerlib,
                IdDoc = docExt.Kerid
            };
            return doc;
        }

        private (IEnumerable<KpDoc> docs, IEnumerable<KpDocExt> docExts, IEnumerable<KpDocL> lot, IEnumerable<KpDocLD> lotDetail) GetData(AffaireId id, bool work)
        {
            IEnumerable<KpDoc> docDb;
            IEnumerable<KpDocExt> docextDb;
            IEnumerable<KpDocL> lotsDb;
            IEnumerable<KpDocLD> lotDetailsDb;
            if (work)
            {
                lotsDb = this.lwRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0);
                lotDetailsDb = this.ldwRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0);
                docDb = this.dwRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0);
            }
            else
            {
                lotsDb = this.lwRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0);
                lotDetailsDb = this.ldRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0);
                docDb = this.dwRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0);
            }
            docextDb = this.dextRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment);
            return (docDb, docextDb, lotsDb, lotDetailsDb);
        }

        private LotDocument MapLot(KpDocL x, IEnumerable<KpDocLD> lotDetailsDb, IEnumerable<KpDoc> docDb, IEnumerable<KpDocExt> docExtDb, bool work)
        {
            var l = new LotDocument
            {
                AA = x.Kelsua,
                ActeDeGestion = x.Kelactg,
                AffaireId = new AffaireId
                {
                    TypeAffaire = x.Keltyp.AsEnum<AffaireType>(),
                    CodeAffaire = x.Kelipb,
                    NumeroAliment = x.Kelalx,
                    NumeroAvenant = x.Kelavn
                },
                Creation = new UpdateMetadata
                {
                    User = x.Kelcru,
                    Time = MakeNullableDateTime(x.Kelcrd, x.Kelcrh)
                },
                Situation = x.Kelsit.AsEnum<SituationClause>(),
                SituationMetadata = new UpdateMetadata
                {
                    User = x.Kelstu,
                    Time = MakeNullableDateTime(x.Kelstd, x.Kelsth)
                },
                IdLot = x.Kelid,
                Libelle = x.Kellib,
                MiseAJour = new UpdateMetadata
                {
                    User = x.Kelmaju,
                    Time = MakeNullableDateTime(x.Kelmajd, x.Kelmajh)
                },
                Numero = x.Kelnum,
                NumeroActeDeGestion = x.Kelactn,
                IsWorkData = work
            };
            l.Documents.AddRange(lotDetailsDb.Select(y => MapDoc(l, y, docDb, docExtDb) ));
            return l;
        }

        private DocumentGenere MapDoc(LotDocument l, KpDocLD y, IEnumerable<KpDoc> docDb, IEnumerable<KpDocExt> docExtDb)
        {
            var isGen = y.Kemtypd != "E";
            var docGen = docDb.FirstOrDefault(x => x.Keqid == y.Kemtypl);
            var docExt = docExtDb.FirstOrDefault(x => x.Kerid == y.Kemtypl);
            DocumentGenere doc = null;
            if (docGen != null)
            {
                docExt = isGen ? null : docExtDb.FirstOrDefault(x => x.Kerid == docGen.Keqkesid);
                doc = MapDocGen(docGen, docExt);
            }
            else
            {
                doc = new DocumentGenere();
            }
            SetDocLotDetail(doc, y);


            return doc;
        }

        private DocumentGenere MapDocGen(KpDoc docGen, KpDocExt docExt)
        {

            var doc = new DocumentGenere()
            {
                IdDoc = docGen.Keqid,
                // KEQNTA
                NatureGeneration = docGen.Keqnta.AsEnum<NatureGeneration>(),
                // KEQENVU ...
                Envoi = new UpdateMetadata { User = docGen.Keqenvu, Time = MakeNullableDateTime(docGen.Keqenvd, docGen.Keqenvh) },
                IsImprimable = docGen.Keqdimp.AsBool(),
                IsAjoute = docGen.Keqajt.AsBool(),
                IsEnCourt = docGen.Keqeco.AsBool(),
                IsAccompagnement = !docGen.Keqdacc.AsBool("N"),
                Etape = docGen.Keqetap.AsEnum<EtapeGeneration>(),
                Libelle = docGen.Keqlib,
                CodeClause = docGen.Keqcdoc,
                Chemin = docGen.Keqchm,
                Nom = docGen.Keqnom,
                TypoDocument = refRepo.GetValue<TypoDocument>(docGen.Keqtdoc),
                Lien = new LienDocument
                {
                    Id = docGen.Keqlien,
                    Type = docGen.Keqmait
                },
                ParentId = docGen.Keqorid,
                IsLibre = docGen.Keqtgl.AsBool("L"),
                IsTransforme = docGen.Keqtrs.AsBool(),
                VersionClause = docGen.Keqver,
                StatutGeneration = docGen.Keqstg.AsEnum<StatutGeneration>(),
                Service = docGen.Keqserv,
                DocumentExterne = MapDocExt(docExt),
                SituationDoc = docGen.Keqsit.AsEnum<SituationClause>()
            };
            return doc;
        }

        private void SetDocLotDetail(DocumentBase doc, KpDocLD ld)
        {
            doc.Tampon = refRepo.GetValue<Tampon>(ld.Kemtamp);

            var typeDest = ld.Kemtyds.AsEnum<TypeDestinataire>();
            var dest = new Destinataire
            {
                Type = typeDest,
                IsPrincipal = !ld.Kemdstp.AsBool("N") // "O" n'existe pas ...
            };
            if (typeDest == TypeDestinataire.Courtier)
            {
                dest.Courtier = GetCourtierProxy(ld.Kemids, ld.Keminl);
            }
            else if (typeDest == TypeDestinataire.Assure)
            {
                dest.Assure = GetAssureProxy(ld.Kemids, ld.Keminl);
            }
            doc.Destinataire = dest;
            doc.TypeEnvoi = refRepo.GetValue<TypeEnvoi>(ld.Kemtyenv);
            doc.SituationLot = ld.Kemsit.AsEnum<SituationDocumentLot>();
            doc.SituationMetadata = new UpdateMetadata
            {
                Time = MakeNullableDateTime(ld.Kemstd, ld.Kemsth),
                User = ld.Kemstu
            };
            doc.NumeroOrdre = (short)ld.Kemord;
            doc.IdDoc = ld.Kemtypl;
            doc.IdDocLot = ld.Kemid;
        }

        private Domain.Affaire.Courtier GetCourtierProxy(int code, int numero)
        {
            return new CourtierProxy(code, numero, ycourtiRepo, refRepo);
        }

        public void SaveLot(LotDocument lot, string userName)
        {
            var (docDb, docextDb, lotsDb, lotDetailsDb) = GetData(lot.AffaireId, lot.IsWorkData);
            var lotDb = lotsDb.FirstOrDefault(x => x.Kelid == lot.IdLot);
            UpdateMetadata(lot, userName, lotDb?.Kelsit.AsEnum<SituationClause>() != lot.Situation);


            KpDocL newDb = ReverseMapLot(lot);
            var repo = lot.IsWorkData ? (IKpDocLRepository)lwRepo : lRepo;
            if (!newDb.Equals(lotDb))
            {
                if (newDb.Kelid <= 0)
                {
                    newDb.Kelid = repo.NewId();
                    repo.Insert(newDb);
                    lot.IdLot = newDb.Kelid;
                }
                else
                {
                    repo.Update(newDb);
                }
            }
            var dets = lotDetailsDb.Where(x => x.Kemkelid == lotDb?.Kelid).ToDictionary(x => x.Kemid);
            var docs = docDb.ToDictionary(x => x.Keqid);
            var exts = docextDb.ToDictionary(x => x.Kerid);
            foreach (var det in dets.Values.Where(x => !lot.Documents.Any(y => x.Kemid == y.IdDocLot)))
            {
                Cleanup(lot.IsWorkData, det, docs.GetValueOrDefault(det.Kemid));
            }
            foreach (var doc in lot.Documents)
            {
                KpDoc kpDoc = docs.GetValueOrDefault(doc.IdDoc);
                var kpDocExt = exts.GetValueOrDefault(doc.DocumentExterne?.IdDoc ?? 0);
                KpDocLD kpDocLD = dets.GetValueOrDefault(doc.IdDocLot);
                SaveDoc(doc, lot, kpDocLD, kpDoc, kpDocExt);
            }
        }

        private void UpdateMetadata(LotDocument lot, string userName, bool updateSituation)
        {
            var now = DateTime.Now;
            if (lot.IdLot <= 0)
            {
                lot.Creation.Time = now;
                lot.Creation.User = userName;
            }
            lot.MiseAJour.Time = now;
            lot.MiseAJour.User = userName;
            if (updateSituation)
            {
                lot.SituationMetadata.Time = now;
                lot.SituationMetadata.User = userName;
            }
        }

        private void Cleanup(bool work, KpDocLD det, KpDoc kpDoc)
        {
            var ldrepo = work ? (IKpDocLdRepository)ldwRepo : ldRepo;
            var drepo = work ? (IKpDocRepository)dwRepo : dRepo;

            ldrepo.Delete(det);
            if (kpDoc != null)
            {
                drepo.Delete(kpDoc);
            }

        }

        private void SaveDoc(DocumentGenere doc, LotDocument lot, KpDocLD kpDocLD, KpDoc kpDoc, KpDocExt kpDocExt)
        {
            KpDocLD ld = ReverseMapLD(lot, doc);
            var isUpdate = true;
            var ldRepo = lot.IsWorkData ? (IKpDocLdRepository)this.ldwRepo : this.ldRepo;
            if (kpDocLD is null)
            {
                if (ld.Kemid == 0L) { ld.Kemid = ldRepo.NewId(); }
                doc.IdDocLot = ld.Kemid;
                isUpdate = false;
            }

            KpDoc docDb = ReverseMapDoc(lot, doc);
            var repo = lot.IsWorkData ? (IKpDocRepository)dwRepo : dRepo;

            if (kpDoc is null)
            {
                if (docDb.Keqid == 0L)
                {
                    docDb.Keqid = repo.NewId();
                }
                repo.Insert(docDb);
                doc.IdDoc = docDb.Keqid;
            }
            else
            {
                repo.Update(docDb);
            }

            ld.Kemtypl = docDb.Keqid;
            ld.Kemtypd = "G";

            if ( ! (doc.DocumentExterne is null ))
            {
                DocumentExterne dext = doc.DocumentExterne;
                KpDocExt extDb = ReverseMapDocExt(lot, doc, dext);

                if (kpDocExt is null)
                {
                    if (extDb.Kerid == 0L) { extDb.Kerid = dextRepo.NewId(); }
                    dextRepo.Insert(extDb);
                }
                else
                {
                    dextRepo.Update(extDb);
                }
                //ld.Kemtypl = extDb.Kerid;
                ld.Kemtypd = "E";

            }
            if (isUpdate)
            {
                ldRepo.Update(ld);
            }
            else
            {
                ldRepo.Insert(ld);
            }

        }

        public void RemoveLot(LotDocument lot)
        {
            var repo = lot.IsWorkData ? (IKpDocLRepository)lwRepo : lRepo;
            repo.Delete(ReverseMapLot(lot));
        }


        private void UdpateDataWith(KpDocL newDb, LotDocument lot)
        {
            throw new NotImplementedException();
        }

        private KpDocL ReverseMapLot(LotDocument lot) =>
            new KpDocL
            {
                Kelactg = lot.ActeDeGestion,
                Kelactn = lot.NumeroActeDeGestion,
                Kelalx = lot.AffaireId.NumeroAliment,
                Kelavn = (int)lot.AffaireId.NumeroAvenant,
                Kelcrd = lot.Creation.Time.AsDate(),
                Kelcrh = lot.Creation.Time.AsTime4(),
                Kelcru = lot.Creation.User,
                Kelemi = "",
                Kelid = lot.IdLot,
                Kelipb = lot.AffaireId.CodeAffaire,
                Kelipk = 0,
                Kellib = lot.Libelle,
                Kelmajd = lot.MiseAJour.Time.AsDate(),
                Kelmajh = lot.MiseAJour.Time.AsTime4(),
                Kelmaju = lot.MiseAJour.User,
                Kelnum = 0,
                Kelsbr = "",
                Kelserv = "PRODU",
                Kelstd = lot.SituationMetadata.Time.AsDate(),
                Kelsth = lot.SituationMetadata.Time.AsTime4(),
                Kelstu = lot.SituationMetadata.User,
                Kelsit = lot.Situation.AsCode(),
                Kelsua = lot.AA,
                Keltyp = lot.AffaireId.TypeAffaire.AsCode()
            };

        private KpDocLD ReverseMapLD(LotDocument lot, DocumentGenere doc)
        {

            var de = doc.DocumentExterne;
            var d = new KpDocLD
            {
                Kemaem = "",
                Kemaemo = "",
                Kemdoca = 0,
                Kemdstp = doc.Destinataire.IsPrincipal.ToYesNo("", "N"),
                Kemenvd = doc.Envoi?.Time.AsDate() ?? 0,
                Kemenvh = doc.Envoi?.Time.AsTime4() ?? 0,
                Kemenvu = doc.Envoi?.User ?? "",
                Kemid = doc.IdDocLot,
                Kemids = doc.Destinataire?.Courtier?.Code ?? doc.Destinataire?.Assure?.Code ?? 0,
                Keminl = doc.Destinataire?.Courtier?.Numero ?? 0,
                Kemkelid = lot.IdLot,
                Kemkesid = 0,
                Kemlmai = 0,
                Kemnbex = 1,
                Kemnta = doc.NatureGeneration.AsCode() ?? "",
                Kemord = doc.NumeroOrdre,
                Kemsit = doc.SituationLot.AsCode(),
                Kemstd = doc.SituationMetadata.Time.AsDate(),
                Kemsth = doc.SituationMetadata.Time.AsTime4(),
                Kemstu = doc.SituationMetadata.User,
                Kemtamp = doc.Tampon.Code,
                Kemtydif = "COUR",
                Kemtyds = doc.Destinataire.Type.AsCode(),
                Kemtyenv = doc.TypeEnvoi.Code,
                Kemtyi = "",
                Kemtypd = doc != null ? "G" : de != null ? "E" : "",
                Kemtypl = doc.IdDoc
            };

            return d;

        }

        private KpDocExt ReverseMapDocExt(LotDocument lot, DocumentGenere docGen, DocumentExterne doc)
        {
            var documentExterne = new KpDocExt
            {
                Kerid = doc.IdDoc,
                Keractg = lot.ActeDeGestion,
                Keralx = lot.AffaireId.NumeroAliment,
                Keravn = lot.AffaireId.NumeroAvenant.GetValueOrDefault(0),
                Kertyp = lot.AffaireId.TypeAffaire.AsCode(),
                Keripb = lot.AffaireId.CodeAffaire,
                Kerchm = doc.Chemin,
                Kercrd = lot.Creation.Time.AsDate(),
                Kercrh = lot.Creation.Time.AsTime4(),
                Kercru = lot.Creation.User,
                Kerstd = lot.SituationMetadata.Time.AsDate(),
                Kersth = lot.SituationMetadata.Time.AsTime4(),
                Kerstu = lot.SituationMetadata.User,
                Kerlib = doc.Libelle,
                Kernom = doc.Nom,
                Kernum = 0,
                Kerord = docGen.NumeroOrdre,
                Kersbr = "0",
                Kerserv = "PRODU",
                Kersit = docGen.SituationDoc.AsCode(),
                Kersua = lot.AA,
                Kerref = doc.Reference,
                Kertypo = doc.TypoDocument.Code
            };
            return documentExterne;
        }
        private KpDoc ReverseMapDoc(LotDocument lot, DocumentGenere doc)
        {

            var documentGenere = new KpDoc
            {
                Keqid = doc.IdDoc,
                Keqactg = lot.ActeDeGestion,
                Keqactn = lot.NumeroActeDeGestion,
                Keqajt = doc.IsAjoute.ToYesNo(),
                Keqalx = lot.AffaireId.NumeroAliment,
                Keqavn = lot.AffaireId.NumeroAvenant.GetValueOrDefault(0),
                Keqtyp = lot.AffaireId.TypeAffaire.AsCode(),
                Keqipb = lot.AffaireId.CodeAffaire,
                Keqcdoc = doc.CodeClause,
                Keqchm = doc.Chemin,
                Keqcrd = lot.Creation.Time.AsDate(),
                Keqcrh = lot.Creation.Time.AsTime4(),
                Keqcru = lot.Creation.User,
                Keqmajd = lot.MiseAJour.Time.AsDate(),
                Keqmajh = lot.MiseAJour.Time.AsTime4(),
                Keqmaju = lot.MiseAJour.User,
                Keqstd = lot.SituationMetadata.Time.AsDate(),
                Keqsth = lot.SituationMetadata.Time.AsTime4(),
                Keqstu = lot.SituationMetadata.User,
                Keqenvd = doc.Envoi.Time.AsDate(),
                Keqenvh = doc.Envoi.Time.AsTime4(),
                Keqenvu = doc.Envoi.User,
                Keqdacc = doc.IsAccompagnement.ToYesNo("", "N"),
                Keqdimp = doc.IsImprimable.ToYesNo(),
                Keqeco = doc.IsEnCourt.ToYesNo(),
                Keqetap = doc.Etape.AsCode(),
                Keqids = doc.Destinataire?.Courtier?.Code ?? doc.Destinataire?.Assure?.Code ?? 0,
                Keqinl = doc.Destinataire?.Courtier?.Numero ?? doc.Destinataire?.Assure?.Numero ?? 0,
                Keqkemid = 0, // TODO : chec why not doc.IdDocLot,
                Keqkesid = doc.DocumentExterne?.IdDoc ?? 0,
                Keqlib = doc.Libelle,
                Keqlien = doc.Lien.Id,
                Keqmait = doc.Lien.Type,
                Keqnbex = 0,
                Keqnom = doc.Nom,
                Keqnta = doc.NatureGeneration.AsCode(),
                Keqnum = 0,
                Keqord = doc.NumeroOrdre,
                Keqorid = doc.ParentId,
                Keqsbr = "0",
                Keqserv = doc.Service,
                Keqsit = doc.SituationDoc.AsCode(),
                Keqstg = doc.StatutGeneration.AsCode(),
                Keqsua = lot.AA,
                Keqtae = "",
                Keqtdoc = doc.TypoDocument.Code,
                Keqtedi = "O",
                Keqtgl = doc.IsLibre.ToYesNo("L", "F"),
                Keqtrs = doc.IsTransforme.ToYesNo(),
                Keqtyds = doc.Destinataire?.Type.AsCode(),
                Keqtyi = "",
                Keqver = doc.VersionClause


            };

            return documentGenere;

        }
        public IEnumerable<Chemin> GetChemins()
        {
            return GetCheminsInternal().Values;
        }

        private Dictionary<string, Chemin> GetCheminsInternal()
        {
            return cache.Get(() => cheminRepo.GetAll().Select(x => new Chemin
            {
                Key = x.Khmcle,
                Creation = new UpdateMetadata(x.Khmcru, MakeNullableDateTime(x.Khmcrd)),
                MiseAJour = new UpdateMetadata(x.Khmmju, MakeNullableDateTime(x.Khmmjd)),
                Designation = x.Khmdes,
                Environment = x.Khmenv,
                Location = x.Khmchm,
                PathType = x.Khmtch,
                Root = x.Khmrac,
                Server = x.Khmsrv
            }).ToDictionary(x => x.Key));
        }

        public Chemin GetChemin(string cle)
        {
            GetCheminsInternal().TryGetValue(cle, out var val);
            return val;
        }

        public void AddCopyDoc(IEnumerable<CopyDoc> docsToCopy)
        {
            foreach (var doc in docsToCopy)
            {
                KpCopDc cop = new KpCopDc
                {
                    Khqtyp = doc.AffaireId.TypeAffaire.AsCode(),
                    Khqipb = doc.AffaireId.CodeAffaire,
                    Khqalx = doc.AffaireId.NumeroAliment,
                    Khqavn = doc.AffaireId.NumeroAvenant ?? 0,
                    Khqcode = doc.NewNumber,
                    Khqnomd = doc.NewName,
                    Khqoldc = doc.OldPath,
                    Khqoldid = doc.OldID,
                    Khqtable = doc.Table,
                };

                copRepo.Insert(cop);
            }
        }

        public IEnumerable<CopyDoc> GetCopyDoc(AffaireId id)
            =>
            this.copRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0).Select(
                    d => new CopyDoc
                    {
                        AffaireId = id,
                        NewName = d.Khqnomd,
                        NewNumber = d.Khqcode,
                        OldPath = d.Khqoldc,
                        OldID = d.Khqoldid,
                        Table = d.Khqtable
                    }
                );


        public void ClearCopyDoc(AffaireId id)
            =>
            this.copRepo.ClearForAffaire(id.CodeAffaire, id.TypeAffaire.AsCode(), id.NumeroAliment, id.NumeroAvenant ?? 0);

        public IEnumerable<DocumentGenere> GetDocGenByAffaireId(AffaireId id, bool work)
        {
            var dgs = work ?
                this.dwRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0) :
                this.dRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant ?? 0) 
                ;
            var des = 
                this.dextRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment)
                ;

            return dgs.Select(x => MapDocGen(x, des.FirstOrDefault(y => y.Kerid == x.Keqkesid)));
        }
    }


}
