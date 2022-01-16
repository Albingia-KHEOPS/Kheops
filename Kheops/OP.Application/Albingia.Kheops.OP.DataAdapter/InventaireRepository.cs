using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Inventaire;
using Albingia.Kheops.OP.Domain.Parametrage.Inventaire;
using Albingia.Kheops.OP.Domain.Extension;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Constants;

namespace Albingia.Kheops.OP.DataAdapter {
    public class InventaireRepository: IInventaireRepository {
        private readonly IReferentialRepository refRepo;
        private readonly IDesignationRepository desiRepository;
        private readonly IParamRepository paramRepository;
        private readonly IParamInventaireRepository paramInvRepository;

        private readonly KpInvenRepository kpInvenRepository;
        private readonly KpInvAppRepository kpInvappRepository;
        private readonly KpInveDRepository kpInvedRepository;
        private readonly HpinvenRepository hpInvenRepository;
        private readonly HpinvappRepository hpInvappRepository;
        private readonly HpinvedRepository hpInvedRepository;

        internal const string CodePerimetreDesignation = "In";

        public InventaireRepository(
            IReferentialRepository refRepo,
            IParamRepository paramRepository,
            IParamInventaireRepository paramInvRepository,
            IDesignationRepository desiRepository,

            KpInvenRepository kpInvenRepository,
            KpInvAppRepository kpInvappRepository,
            KpInveDRepository kpInvedRepository,
            KpDesiRepository kpDesiRepository,
            HpinvenRepository hpInvenRepository,
            HpinvappRepository hpInvappRepository,
            HpinvedRepository hpInvedRepository

            ) {
            this.refRepo = refRepo;
            this.paramRepository = paramRepository;
            this.paramInvRepository = paramInvRepository;
            this.desiRepository = desiRepository;

            this.kpInvenRepository = kpInvenRepository;
            this.kpInvappRepository = kpInvappRepository;
            this.kpInvedRepository = kpInvedRepository;

            this.hpInvenRepository = hpInvenRepository;
            this.hpInvappRepository = hpInvappRepository;
            this.hpInvedRepository = hpInvedRepository;

        }

        public void SaveInventaireApplication(Inventaire inventaire, Garantie gar, int formuleNumber) {
            int num = kpInvappRepository.GetNextNum(inventaire.Affaire);
            kpInvappRepository.Insert(new KpInvApp {
                Kbgalx = inventaire.Affaire.NumeroAliment,
                Kbgavn = inventaire.Affaire.NumeroAvenant,
                Kbgtyp = inventaire.Affaire.TypeAffaire.AsCode(),
                Kbggar = gar.ParamGarantie.ParamGarantie.CodeGarantie,
                Kbgipb = inventaire.Affaire.CodeAffaire,
                Kbgkbeid = inventaire.Id,
                Kbgnum = num,
                Kbgobj = 0,
                Kbgrsq = 0,
                Kbgperi = "GA",
                Kbgfor = formuleNumber
            });
        }

        public void SaveInventaire(Inventaire inventaire) {
            if (inventaire.Affaire.IsHisto) {
                throw new InvalidOperationException("On ne peut pas mettre à jour l'historique");
            }
            var data = GetDataForInventaire(inventaire.Id);

            KpInven arg;
            if (data.Inventaires == null || !data.Inventaires.Any()) {
                arg = new KpInven() { };
            }
            else {
                arg = data.Inventaires.First().Value;
            }

            data.ReverseMapInventaire(inventaire, arg);

            if (arg.Kbeid == 0) {
                kpInvenRepository.Insert(arg);
                inventaire.Id = arg.Kbeid;
            }
            else {
                kpInvenRepository.Update(arg);
            }
            foreach (var item in data.Items) {
                if (item.Kbfid == 0) {
                    item.Kbfkbeid = arg.Kbeid;
                    kpInvedRepository.Insert(item);
                }
                else {
                    kpInvedRepository.Update(item);
                }
            }
            foreach (var item in data.ItemsToDelete) {
                kpInvedRepository.Delete(item);
            }

        }

        public IEnumerable<Inventaire> GetInventaireByFormule(FormuleId id) {
            InventoryData inventoryData;
            if (id.IsHisto) {
                inventoryData = GetDataHistorique(id);
            }
            else {
                inventoryData = GetData(id);
            }
            return inventoryData.AsInventaires().ToList();
        }

        public void Reprise(AffaireId id) {
            var invedList = this.kpInvedRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            invedList.ForEach(x => this.kpInvedRepository.Delete(x));
            var hinvedList = this.hpInvedRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hinvedList.ForEach(x => this.hpInvedRepository.Delete(x));
            hinvedList.ForEach(x => this.kpInvedRepository.Insert(x));
            var invappList = this.kpInvappRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            invappList.ForEach(x => this.kpInvappRepository.Delete(x));
            var hinvappList = this.hpInvappRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hinvappList.ForEach(x => this.hpInvappRepository.Delete(x));
            hinvappList.ForEach(x => this.kpInvappRepository.Insert(x));
            var invList = this.kpInvenRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            invList.ForEach(x => this.kpInvenRepository.Delete(x));
            var hinvList = this.hpInvenRepository.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            hinvList.ForEach(x => this.hpInvenRepository.Delete(x));
            hinvList.ForEach(x => this.kpInvenRepository.Insert(x));
        }

        private InventoryData GetData(FormuleId id) {
            InventoryData inventoryData = new InventoryData(this, refRepo, paramRepository, paramInvRepository, desiRepository);
            inventoryData.Inventaires = kpInvenRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment).Where(x => inventoryData.Portees[x.Kbeid].Any()).ToDictionary(x => x.Kbeid);
            inventoryData.Portees = kpInvappRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment).Where(x => x.Kbgfor == id.NumeroFormule).ToLookup(x => x.Kbgkbeid);
            inventoryData.Items = kpInvedRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment).Where(x => inventoryData.Inventaires.ContainsKey(x.Kbfkbeid)).ToList();
            inventoryData.Designations = desiRepository.GetDesignationsByAffaire(id);
            return inventoryData;
        }

        private InventoryData GetDataHistorique(FormuleId id) {
            InventoryData inventoryData = new InventoryData(this, refRepo, paramRepository, paramInvRepository, desiRepository);
            inventoryData.Inventaires = hpInvenRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).Where(x => inventoryData.Portees[x.Kbeid].Any()).ToDictionary(x => x.Kbeid);
            inventoryData.Portees = hpInvappRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).Where(x => x.Kbgfor == id.NumeroFormule).ToLookup(x => x.Kbgkbeid);
            inventoryData.Items = hpInvedRepository.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value).Where(x => inventoryData.Inventaires.ContainsKey(x.Kbfkbeid)).ToList();
            inventoryData.Designations = desiRepository.GetDesignationsByAffaire(id);
            return inventoryData;
        }

        public Inventaire GetInventaireByGarantieId(long garantieId) {
            return GetDataInventaireForGarantie(garantieId).AsInventaires().FirstOrDefault();

        }

        private InventoryData GetDataForInventaire(long inventaireId) {
            var invent = kpInvenRepository.Get(inventaireId);
            return GetInventaireInternal(invent);
        }

        private InventoryData GetDataInventaireForGarantie(long garantieId) {
            var invent = kpInvenRepository.GetByGarantie(garantieId).FirstOrDefault();
            if (invent == null) {
                return null;
            }
            return GetInventaireInternal(invent);
        }

        public Inventaire GetInventaireById(long invenId) {
            if (invenId == default(long)) {
                return null;
            }
            var invent = kpInvenRepository.Get(invenId);
            if (invent == null) {
                return null;
            }

            return GetInventaireInternal(invent).AsInventaires().FirstOrDefault();
        }

        private InventoryData GetInventaireInternal(KpInven invent) {
            if (invent == null) {
                return new InventoryData(this, refRepo, paramRepository, paramInvRepository, desiRepository);
            }
            long invenId = invent.Kbeid;
            var inventoryData = new InventoryData(this, refRepo, paramRepository, paramInvRepository, desiRepository);
            var id = InventoryData.MakeAffaireId(invent);
            inventoryData.Portees = kpInvappRepository.GetByInven(invenId).ToLookup(x => x.Kbgkbeid);
            inventoryData.Inventaires = new Dictionary<long, KpInven> { [invent.Kbeid] = invent };
            inventoryData.Items = kpInvedRepository.GetByInven(invenId).ToList();
            inventoryData.Designations = desiRepository.GetDesignationsByAffaire(id);
            return inventoryData;
        }

        public Inventaire GetInventaireByGarantieId(long garantieId, int numAvenant) {
            var invent = hpInvenRepository.GetByGarantie(garantieId, numAvenant).FirstOrDefault();
            if (invent == null) {
                return null;
            }
            return GetInventaireInternal(invent).AsInventaires().FirstOrDefault();

        }
        public Inventaire GetInventaireById(long invenId, int numAvenant) {
            if (invenId == default(long)) {
                return null;
            }
            var invent = hpInvenRepository.Get(invenId, numAvenant);
            if (invent == null) {
                return null;
            }

            return GetInventaireInternal(invent).AsInventaires().FirstOrDefault();
        }

        private Inventaire GetInventairehistoriqueInternal(KpInven invent) {
            long invenId = invent.Kbeid;
            var inventoryData = new InventoryData(this, refRepo, paramRepository, paramInvRepository, desiRepository);
            var id = InventoryData.MakeAffaireId(invent);
            inventoryData.Portees = hpInvappRepository.GetByInven(invenId, invent.Kbeavn.Value).ToLookup(x => x.Kbgkbeid);
            inventoryData.Inventaires = new Dictionary<long, KpInven> { [invent.Kbeid] = invent };
            inventoryData.Items = hpInvedRepository.GetByInven(invenId, invent.Kbeavn.Value).ToList();
            inventoryData.Designations = desiRepository.GetDesignationsByAffaire(id);
            return inventoryData.AsInventaires().FirstOrDefault();
        }

        internal int UpdateDesi(int desiChrono, string value, AffaireId affId, IDictionary<int, string> desis)
        {
            var desi = desis.GetValueOrDefault(desiChrono);
            //if (!(string.IsNullOrEmpty(value) && string.IsNullOrEmpty(desi) || desi == value))
            {
                desiChrono = desiRepository.UpdateDesi(value, affId, desiChrono, CodePerimetreDesignation);
            }
            return desiChrono;
        }

        public Inventaire GetInventaireById(long id, int? numAvenant) {
            if (numAvenant.HasValue) {
                return GetInventaireById(id, numAvenant.Value);
            }
            return GetInventaireById(id);
        }

        public Inventaire GetInventaireByGarantieId(long id, int? numAvenant) {
            if (numAvenant.HasValue) {
                return GetInventaireByGarantieId(id, numAvenant.Value);
            }
            return GetInventaireByGarantieId(id);
        }

        public void DeleteInventaire(long inventaireId) {
            desiRepository.DeleteForInventaire(inventaireId);
            kpInvedRepository.DeleteForGarantie(inventaireId);
            kpInvenRepository.Delete(new KpInven { Kbeid = inventaireId });
            kpInvappRepository.DeleteInven(inventaireId);
        }

        private class InventoryData {
            public InventoryData(InventaireRepository invRepo, IReferentialRepository refRepo, IParamRepository paramRepo, IParamInventaireRepository paramInvRepository, IDesignationRepository desiRepo) {
                this.refRepo = refRepo;
                this.paramRepo = paramRepo;
                this.paramInvRepository = paramInvRepository;
                this.invRepo = invRepo;
                this.desiRepo = desiRepo;
            }

            public ILookup<long, KpInvApp> Portees { get; internal set; } = new KpInvApp[0].ToLookup(x => x.Kbgkbeid);
            public Dictionary<long, KpInven> Inventaires { get; internal set; } = new KpInven[0].ToDictionary(x => x.Kbeid);
            public ICollection<KpInveD> Items { get; internal set; } = new List<KpInveD>();
            public ICollection<KpInveD> ItemsToDelete { get; internal set; } = new List<KpInveD>();

            public IDictionary<int, string> Designations { get; internal set; } = new Dictionary<int, string>();

            private readonly IReferentialRepository refRepo;
            private readonly IParamRepository paramRepo;
            private readonly IParamInventaireRepository paramInvRepository;
            private readonly InventaireRepository invRepo;
            private readonly IDesignationRepository desiRepo;

            public IEnumerable<Inventaire> AsInventaires() {
                //throw new NotImplementedException();
                return Inventaires.Select(x => MapInventaire(x.Value));
            }

            private Inventaire MapInventaire(KpInven arg) {
                var inventaire = new Inventaire() {
                    Affaire = MakeAffaireId(arg),
                    ChronoDesi = arg.Kbekadid,
                    Designation = this.Designations.GetValueOrDefault((int)arg.Kbekadid) ?? string.Empty,
                    Id = arg.Kbeid,
                    Descriptif = arg.Kbedesc,
                    IsHTnotTTC = (arg.Kbevah == "?") ? default(bool?) : (arg.Kbevah == "H"),
                    NumChrono = arg.Kbechr,
                    ReportvaleurObjet = arg.Kberepval.AsNullableBool(),
                    Typedevaleur = refRepo.GetValue<TypeValeurRisque>(arg.Kbevat),
                    TypeInventaire = this.paramInvRepository.GetTypeInventaire(arg.Kbekagid),
                    Valeurs = new ValeursUnite {
                        ValeurActualise = arg.Kbevaa,
                        ValeurOrigine = arg.Kbeval,
                        ValeurTravail = arg.Kbevaw,
                        Unite = refRepo.GetValue<UniteValeurRisque>(arg.Kbevau)
                    },
                    ValeursIndice = new Valeurs {
                        ValeurActualise = arg.Kbeiva,
                        ValeurOrigine = arg.Kbeivo,
                        ValeurTravail = arg.Kbeivw
                    }
                };
                inventaire.Items = this.Items
                    .Where(item => item.Kbfkbeid == inventaire.Id)
                    .Select(item => Build(inventaire.TypeInventaire.TypeItem, item))
                    .ToList();
                return inventaire;
            }

            internal (KpInven, IEnumerable<KpInveD>) ReverseMapInventaire(Inventaire invent, KpInven arg) {

                arg.Kbeipb = invent.Affaire.CodeAffaire;
                arg.Kbealx = invent.Affaire.NumeroAliment;
                arg.Kbetyp = invent.Affaire.TypeAffaire.AsCode();
                arg.Kbeavn = invent.Affaire.IsHisto ? arg.Kbeavn : null;

                arg.Kbekadid = invRepo.UpdateDesi((int)arg.Kbekadid, invent.Designation, invent.Affaire, Designations);

                arg.Kbeid = invent.Id;
                arg.Kbedesc = invent.Descriptif;
                arg.Kbevah = invent.IsHTnotTTC.HasValue ? invent.IsHTnotTTC.Value ? "H" : "T" : "";
                arg.Kbechr = invent.NumChrono;
                arg.Kberepval = invent.ReportvaleurObjet.ToYesNo();
                arg.Kbevat = invent.Typedevaleur?.Code ?? String.Empty;
                arg.Kbekagid = invent.TypeInventaire.Id;

                arg.Kbevaa = invent.Valeurs.ValeurActualise;
                arg.Kbeval = invent.Valeurs.ValeurOrigine;
                arg.Kbevaw = invent.Valeurs.ValeurTravail;
                arg.Kbevau = invent.Valeurs.Unite?.Code ?? String.Empty;


                arg.Kbeiva = invent.ValeursIndice.ValeurActualise;
                arg.Kbeivo = invent.ValeursIndice.ValeurOrigine;
                arg.Kbeivw = invent.ValeursIndice.ValeurTravail;


                var itemsList = new List<KpInveD>();
                foreach (var item in invent.Items) {
                    var itItem = Items.FirstOrDefault();
                    var it = itItem != null ? new KpInveD(itItem) : new KpInveD();
                    ReverseMap(item, it, arg);
                    itemsList.Add(it);
                    if (itItem != null) {
                        Items.Remove(itItem);
                    }
                }

                ItemsToDelete = Items.Where(x => !itemsList.Any(y => y.Kbfid == x.Kbfid)).ToList();
                Items = itemsList;

                return (arg, itemsList);
            }

            internal static AffaireId MakeAffaireId(KpInven arg) {
                return new AffaireId() {
                    CodeAffaire = arg.Kbeipb,
                    NumeroAliment = arg.Kbealx,
                    TypeAffaire = arg.Kbetyp.ParseCode<AffaireType>(),
                    IsHisto = arg.Kbeavn.HasValue,
                    NumeroAvenant = arg.Kbeavn
                };
            }
            internal static AffaireId MakeAffaireId(KpInveD invendet) {
                return new AffaireId() {
                    CodeAffaire = invendet.Kbfipb,
                    NumeroAliment = invendet.Kbfalx,
                    TypeAffaire = invendet.Kbftyp.ParseCode<AffaireType>(),
                    IsHisto = invendet.Kbfavn.HasValue,
                    NumeroAvenant = invendet.Kbfavn
                };
            }

            private InventaireItem Build(TypeInventaireItem type, KpInveD det) {
                var item = MakeItem(type);
                item.NumeroLigne = det.Kbfnumlgn;
                if (item is Activite act) {
                    act.ChiffreAffaire = det.Kbfmnt1;
                    act.PourcentageChiffreAffaire = det.Kbfmnt2;
                    if (act is ActivitesImmobilieres) {
                        act.Code = refRepo.GetValue<TypeImmobilier>(det.Kbfcmat);
                    }
                    else if (act is LocationTiers) {
                        act.Code = refRepo.GetValue<TypeLocation>(det.Kbfcmat);
                    }
                    else if (act is ProductionRealisationAudio) {
                        act.Code = refRepo.GetValue<TypeProdution>(det.Kbfcmat);
                    }
                }
                if (item is ActiviteLocalisee loc) {
                    loc.Lieu = det.Kbfsite;
                    loc.Designation = det.Kbfdesc;
                    loc.DateDebut = MakeNullableDateTime(det.Kbfdatdeb, det.Kbfdebheu);
                    loc.DateFin = MakeNullableDateTime(det.Kbfdatfin, det.Kbffinheu);
                    loc.NatureLieu = refRepo.GetValue<NatureLieu>(det.Kbfntli);
                    if (loc is ManifestationAssure ma) {
                        ma.CodePostal = det.Kbfcp.ToFrenchPostalCode();
                        ma.Ville = det.Kbfville;
                        ma.Montant = det.Kbfmnt1;
                    }
                    // else if (loc is TournageAssure) { }
                }
                else if (item is Marchandises mrch) {
                    mrch.Nature = Designations.GetValueOrDefault((int)det.Kbfkadid);
                    mrch.Montant = det.Kbfmnt1;
                    if (mrch is MarchandisesTransportees t) {
                        t.Depart = Designations.GetValueOrDefault((int)det.Kbfsit2);
                        t.Destination = Designations.GetValueOrDefault((int)det.Kbfsit3);
                        t.Modalites = Designations.GetValueOrDefault((int)det.Kbfdes2);
                        t.DateDebut = MakeNullableDateTime(det.Kbfdatdeb/*, det.Kbfdebheu*/);
                        t.DateFin = MakeNullableDateTime(det.Kbfdatfin/*, det.Kbffinheu*/);
                    }
                    else if (mrch is MarchandisesStockees st) {
                        st.Lieu = det.Kbfsite;
                        st.CodePostal = det.Kbfcp.ToFrenchPostalCode();
                        st.Ville = det.Kbfville;
                        st.Pays = refRepo.GetValue<Pays>(det.Kbfpay);
                    }
                }
                else if (item is Personne pers) {
                    pers.DateNaissance = MakeNullableDateTime(det.Kbfdatnai);
                    pers.Fonction = det.Kbffonc;
                    pers.Nom = det.Kbfnom;
                    pers.Prenom = det.Kbfpnom;
                    //if (pers is PersonneDesignee pdes) { }

                    if (pers is PersonneIndispo pi) {
                        pi.Montant = det.Kbfmnt1;
                        pi.Franchise = Designations.GetValueOrDefault((int)det.Kbfkadfh);
                    }
                    if (pers is PersonneDesigneeIndispo pdi) {
                        pdi.Extention = refRepo.GetValue<Indisponibilite>(det.Kbfext);
                        pers.DateNaissance = MakeNullableDateTime(det.Kbfdatnai * 10000 + 100 + 1);

                    }
                    else if (pers is PersonneDesigneeIndispoTournage pdit) {
                        pdit.Extention = refRepo.GetValue<IndisponibiliteTournage>(det.Kbfext);
                    }
                }
                else if (item is Materiel mat) {
                    mat.Designation = det.Kbfdesc;
                    mat.Valeur = det.Kbfmnt1;
                    if (mat is MaterielAssure ma) {
                        ma.Code = refRepo.GetValue<TypeMateriel>(det.Kbfcmat);
                        ma.Reference = det.Kbfmsr;
                    }
                    //else if (mat is PosteAssure) { } else if (mat is BienAssure) { }
                }
                else if (item is VehiculePourTransport veh) {
                    veh.Marque = det.Kbfmrq;
                    veh.Modele = det.Kbfmod;
                    veh.Immatriculation = det.Kbfimm;
                    veh.DateDebut = MakeNullableDateTime(det.Kbfdatdeb/*, det.Kbfdebheu*/);
                    veh.DateFin = MakeNullableDateTime(det.Kbfdatfin/*, det.Kbffinheu*/);
                    veh.Montant = det.Kbfmnt1;
                }
                else if (item is MultipleSituation mul) {
                    mul.RaisonSociale = det.Kbfdesc;
                    mul.Adresse = det.Kbfsite;
                    mul.CodePostal = det.Kbfcp.ToFrenchPostalCode();
                    mul.Ville = det.Kbfville;
                    mul.DateDebut = MakeNullableDateTime(det.Kbfdatdeb, det.Kbfdebheu);
                    mul.DateFin = MakeNullableDateTime(det.Kbfdatfin, det.Kbffinheu);
                    mul.RisquesLocatifs = refRepo.GetValue<RisqueLocatif>(det.Kbfrlo);
                    mul.Renonciation = refRepo.GetValue<Renonciation>(det.Kbfren);
                    mul.Qualite = refRepo.GetValue<QualiteJuridique>(det.Kbfqua);
                    mul.ValeurRisquesLocatifs = det.Kbfmnt1;
                    mul.Surface = det.Kbfmnt2;
                    mul.Contenu = det.Kbfmnt3;
                    mul.ValeurMaterielsBureautique = det.Kbfmnt4;
                }
                return item;
            }

            private void ReverseMap(InventaireItem item, KpInveD det, KpInven inven) {
                det.Kbfkbeid = inven.Kbeid;
                det.Kbfnumlgn = item.NumeroLigne;
                det.Kbfipb = inven.Kbeipb;
                det.Kbfalx = inven.Kbealx;
                det.Kbftyp = inven.Kbetyp;
                det.Kbfavn = inven.Kbeavn;
                AffaireId affId = MakeAffaireId(det);
                if (item is Activite act) {
                    det.Kbfmnt1 = act.ChiffreAffaire;
                    det.Kbfmnt2 = act.PourcentageChiffreAffaire;
                    det.Kbfcmat = act.Code.Code;
                }
                if (item is ActiviteLocalisee loc) {
                    det.Kbfsite = loc.Lieu;
                    det.Kbfdesc = loc.Designation;
                    (det.Kbfdatdeb, det.Kbfdebheu) = loc.DateDebut.AsDateHour();
                    (det.Kbfdatfin, det.Kbffinheu) = loc.DateFin.AsDateHour();
                    det.Kbfntli = loc.NatureLieu.Code;
                    if (loc is ManifestationAssure ma) {
                        det.Kbfcp = int.Parse(ma.CodePostal);
                        det.Kbfville = ma.Ville;
                        det.Kbfmnt1 = ma.Montant;
                    }
                    // else if (loc is TournageAssure) { }
                }
                else if (item is Marchandises mrch) {
                    det.Kbfkadid = this.invRepo.UpdateDesi((int)det.Kbfkadid, mrch.Nature, affId, Designations);
                    det.Kbfmnt1 = mrch.Montant;
                    if (mrch is MarchandisesTransportees t) {
                        det.Kbfsit2 = this.invRepo.UpdateDesi((int)det.Kbfsit2, t.Depart, affId, Designations);
                        det.Kbfsit3 = this.invRepo.UpdateDesi((int)det.Kbfsit3, t.Destination, affId, Designations);
                        det.Kbfdes2 = this.invRepo.UpdateDesi((int)det.Kbfdes2, t.Modalites, affId, Designations);
                        det.Kbfdatdeb = t.DateDebut.AsDate();
                        det.Kbfdatfin = t.DateFin.AsDate();
                    }
                    else if (mrch is MarchandisesStockees st) {
                        det.Kbfsite = st.Lieu;
                        det.Kbfcp = int.Parse(st.CodePostal);
                        det.Kbfville = st.Ville;
                        det.Kbfpay = st.Pays.Code;
                    }
                }
                else if (item is Personne pers) {
                    det.Kbfdatnai = pers.DateNaissance.AsDate();
                    det.Kbffonc = pers.Fonction;
                    det.Kbfnom = pers.Nom;
                    det.Kbfpnom = pers.Prenom;

                    if (pers is PersonneIndispo pi) {
                        det.Kbfmnt1 = pi.Montant;
                        det.Kbfkadfh = this.invRepo.UpdateDesi((int)det.Kbfkadfh, pi.Franchise, affId, Designations);
                    }
                    if (pers is PersonneDesigneeIndispo pdi) {
                        det.Kbfext = pdi.Extention.Code;
                        det.Kbfdatnai = pers.DateNaissance.NYear();

                    }
                    else if (pers is PersonneDesigneeIndispoTournage pdit) {
                        det.Kbfext = pdit.Extention.Code;
                    }
                }
                else if (item is Materiel mat) {
                    det.Kbfdesc = mat.Designation;
                    det.Kbfmnt1 = mat.Valeur;
                    if (mat is MaterielAssure ma) {
                        det.Kbfcmat = ma.Code.Code;
                        det.Kbfmsr = ma.Reference;
                    }
                    //else if (mat is PosteAssure) { } else if (mat is BienAssure) { }
                }
                else if (item is VehiculePourTransport veh) {
                    det.Kbfmrq = veh.Marque;
                    det.Kbfmod = veh.Modele;
                    det.Kbfimm = veh.Immatriculation;
                    det.Kbfdatdeb = veh.DateDebut.AsDate();
                    det.Kbfdatfin = veh.DateFin.AsDate();
                    det.Kbfmnt1 = veh.Montant;
                }
                else if (item is MultipleSituation mul) {
                    det.Kbfdesc = mul.RaisonSociale;
                    det.Kbfsite = mul.Adresse;
                    det.Kbfcp = int.Parse(mul.CodePostal);
                    det.Kbfville = mul.Ville;
                    (det.Kbfdatdeb, det.Kbfdebheu) = mul.DateDebut.AsDateHour();
                    (det.Kbfdatfin, det.Kbffinheu) = mul.DateFin.AsDateHour();
                    det.Kbfrlo = mul.RisquesLocatifs.Code;
                    det.Kbfren = mul.Renonciation.Code;
                    det.Kbfqua = mul.Qualite.Code;
                    det.Kbfmnt1 = mul.ValeurRisquesLocatifs;
                    det.Kbfmnt2 = mul.Surface;
                    det.Kbfmnt3 = mul.Contenu;
                    det.Kbfmnt4 = mul.ValeurMaterielsBureautique;
                }
            }


            private static InventaireItem MakeItem(TypeInventaireItem type) {
                switch (type) {
                    case TypeInventaireItem.ManifestationAssurees:
                        return new ManifestationAssure();
                    case TypeInventaireItem.TournagesAssures:
                        return new TournageAssure();
                    case TypeInventaireItem.PersonneDesigneeIndispo:
                        return new PersonneDesigneeIndispo();
                    case TypeInventaireItem.PersonneDesignee:
                        return new PersonneDesignee();
                    case TypeInventaireItem.PersonneDesigneeIndispoTournage:
                        return new PersonneDesigneeIndispoTournage();
                    case TypeInventaireItem.Materielsassures:
                        return new MaterielAssure();
                    case TypeInventaireItem.Biensassures:
                        return new BienAssure();
                    case TypeInventaireItem.Postesassures:
                        return new PosteAssure();
                    case TypeInventaireItem.MultiplesSituations:
                        return new MultipleSituation();
                    case TypeInventaireItem.Audioproduction:
                        return new ProductionRealisationAudio();
                    case TypeInventaireItem.Audiolocation:
                        return new LocationTiers();
                    case TypeInventaireItem.ProImmo:
                        return new ActivitesImmobilieres();
                    case TypeInventaireItem.Marchandises:
                        return new MarchandisesTransportees();
                    case TypeInventaireItem.Stockage:
                        return new MarchandisesStockees();
                    case TypeInventaireItem.ParcVehicules:
                        return new VehiculePourTransport();
                    default:
                        throw new NotSupportedException($"{type} n'est pas un type d'inventaire recconu");
                }
            }
            private static TypeInventaireItem GetTypeItem(InventaireItem type) {
                switch (type) {
                    case ManifestationAssure a:
                        return TypeInventaireItem.ManifestationAssurees;
                    case TournageAssure a:
                        return TypeInventaireItem.TournagesAssures;
                    case PersonneDesigneeIndispo a:
                        return TypeInventaireItem.PersonneDesigneeIndispo;
                    case PersonneDesignee a:
                        return TypeInventaireItem.PersonneDesignee;
                    case PersonneDesigneeIndispoTournage a:
                        return TypeInventaireItem.PersonneDesigneeIndispoTournage;
                    case MaterielAssure a:
                        return TypeInventaireItem.Materielsassures;
                    case BienAssure a:
                        return TypeInventaireItem.Biensassures;
                    case PosteAssure a:
                        return TypeInventaireItem.Postesassures;
                    case MultipleSituation a:
                        return TypeInventaireItem.MultiplesSituations;
                    case ProductionRealisationAudio a:
                        return TypeInventaireItem.Audioproduction;
                    case LocationTiers a:
                        return TypeInventaireItem.Audiolocation;
                    case ActivitesImmobilieres a:
                        return TypeInventaireItem.ProImmo;
                    case MarchandisesTransportees a:
                        return TypeInventaireItem.Marchandises;
                    case MarchandisesStockees a:
                        return TypeInventaireItem.Stockage;
                    case VehiculePourTransport a:
                        return TypeInventaireItem.ParcVehicules;
                    default:
                        throw new NotSupportedException($"{type.GetType().FullName} n'est pas un type d'inventaire recconu");

                }
            }
        }


    }

}
