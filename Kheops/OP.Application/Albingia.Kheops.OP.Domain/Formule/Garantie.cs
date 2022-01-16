using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using Albingia.Kheops.OP.Domain.Model;
using Albingia.Kheops.OP.Domain.Parametrage.Formule;
using Albingia.Kheops.OP.Domain.Parametrage.Formules;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Risque;
using System;
using System.Collections.Generic;
using System.Linq;
using TruthTable = System.Collections.Generic.IDictionary<(Albingia.Kheops.OP.Domain.Model.CaractereSelection caractere, Albingia.Kheops.OP.Domain.Referentiel.NatureValue nature), Albingia.Kheops.OP.Domain.Parametrage.Formule.NatureSelection>;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public class Garantie : IValidatable
    {
        public const string CodeGareat = "GAREAT";
        public const string CodeGareatAttent = "ATTENT";
        public const string CodeIncendie = "TAINPI";

        private bool isHidden;
        public Garantie ShallowCopy() { return (Garantie)this.MemberwiseClone(); }
        internal OptionBloc parentBloc;
        internal Garantie parentGarantie;

        public virtual Garantie FindGarantieBySeq(long seq)
        {
            return FindGarantieSeqRec(this.SousGaranties, seq);
        }

        public virtual bool IsNew { get; set; } = false;

        public bool IsHidden {
            get {
                return this.parentBloc?.ParamModele.Code == CodeGareat && CodeGarantie == CodeGareatAttent || this.isHidden;
            }
            set {
                if (this.parentBloc is null || this.parentBloc.ParamModele.Code != CodeGareat) {
                    this.isHidden = value;
                }
            }
        }

        private Garantie FindGarantieSeqRec(IEnumerable<Garantie> garanties, long seq)
        {
            if (!garanties.Any())
            {
                return null;
            }
            return garanties.SingleOrDefault(x => x.ParamGarantie.Sequence == seq) ?? FindGarantieSeqRec(garanties.SelectMany(x => x.SousGaranties), seq);
        }

        public virtual Garantie FindGarantieById(Int64 id)
        {
            return FindGarantieRec(SousGaranties, id);
        }

        private Garantie FindGarantieRec(IEnumerable<Garantie> garanties, Int64 id)
        {
            if (!garanties.Any())
            {
                return null;
            }

            return garanties.SingleOrDefault(x => x.Id == id) ?? FindGarantieRec(garanties.SelectMany(x => x.SousGaranties), id);
        }

        public IEnumerable<ValidationError> Validate()
        {
            if (!IsChecked)
            {
                return Enumerable.Empty<ValidationError>();
            }
            var errors = new List<ValidationError>();
            errors.AddRange(this.CheckDates());
            return errors.Concat(SousGaranties.Where(g => g.IsChecked).SelectMany(x => x.Validate()));
        }

        public bool IsChecked
        {
            get
            {
                if (
                     (
                         (Caractere == CaractereSelection.Obligatoire
                         && (
                             Nature != NatureValue.Exclue
                             || NatureRetenue != NatureValue.Exclue)
                            )
                         ||
                         (Caractere == CaractereSelection.DeBase && Nature == NatureValue.Comprise)
                     )
                     && NatureRetenue != NatureValue.Exclue
                   )
                    return true;
                if (
                        Caractere == CaractereSelection.Propose
                        && (
                            (
                             Nature != NatureValue.Exclue
                             && NatureRetenue != NatureValue.None
                             && NatureRetenue != NatureValue.Exclue
                            )
                            || (
                             Nature == NatureValue.Exclue
                             && NatureRetenue != NatureValue.Exclue
                             )
                        )
                  )
                    return true;

                if (Caractere == CaractereSelection.Facultatif)
                {
                    if ((Nature == NatureValue.Comprise || Nature == NatureValue.Accordee)
                        && NatureRetenue != NatureValue.None
                        && NatureRetenue != NatureValue.Exclue)
                        return true;
                    if (Nature == NatureValue.Exclue && (
                        NatureRetenue == NatureValue.Accordee
                        || NatureRetenue == NatureValue.Comprise)
                        )
                        return true;
                }
                return false;
            }
        }

        public string CodeBloc => this.parentBloc?.ParamBloc.Code;

        public List<Garantie> SousGaranties { get; set; } = new List<Garantie>();
        /* KDEGARAN */
        public ParamGarantieHierarchie ParamGarantie { get; set; }

        public int Niveau { get { return ParamGarantie.Niveau; } }

        public bool InventairePossible { get { return (ParamGarantie?.ParamGarantie?.IsInventPossible).GetValueOrDefault(); } }

        public string DesignationGarantie { get { return ParamGarantie.ParamGarantie.DesignationGarantie; } }

        public bool HasPortees { get { return Portees?.Any() == true; } }

        public TarifGarantie Tarif { get; set; }

        /* KDEID */
        public long Id { get; set; }
        /* KDEFOR */
        public int Formule { get; set; }
        /* KDEKDCID */
        //public long BlocId { get; set; }
        ///* KDESEQ */
        //public string Sequence { get; set; }
        /* KDETRI */
        public string Tri { get; set; }
        /* KDENUMPRES */
        public Decimal NumDePresentation { get; set; }
        /* KDEAJOUT */ // N
        public bool IsGarantieAjoutee { get; set; }
        /* KDECAR */ // O,F,P
        public CaractereSelection Caractere { get; set; }
        /* KDENAT */ // A,E
        public NatureValue Nature { get; set; }
        /* KDEGAN */ // A,E,C
        public NatureValue NatureRetenue { get; set; }
        /* KDEKDFID */
        public long LienKPGARAP { get; set; }
        /* KDEDEFG */ // G,U,A,B,R,H,P
        public DefinitionGarantie DefinitionGarantie { get; set; }
        /* KDEKDHID */
        public long LienKPSPEC { get; set; }
        /* KDEDATDEB , KDEHEUDEB */
        public DateTime? DateDebut { get; set; }
        /* KDEDATFIN, KDEHEUFIN */
        public DateTime? DateFinDeGarantie { get; set; }
        /* KDEWHDEB , KDEWDDEB */
        public DateTime? DatestandardDebut { get; set; }
        /* KDEWDFIN , KDEWHFIN */
        public DateTime? DatestandardFin { get; set; }

        /* KDEDUREE */
        public int? Duree { get; set; }
        /* KDEDURUNI */ // A, ''
        public UniteDuree DureeUnite { get; set; }

        /* KDEPRP */ // A, G, ''
        public PeriodeApplication PeriodeApplication { get; set; }
        /* KDETYPEMI */ // P,C
        public TypeEmission TypeEmission { get; set; }
        /* KDEALIREF */
        public bool? IsAlimMontantReference { get; set; }
        /* KDECATNAT */
        public bool? IsApplicationCATNAT { get; set; }
        /* KDEINA */
        public bool? IsIndexe { get; set; }
        /* KDETAXCOD */
        public Taxe Taxe { get; set; }

        internal bool IsSelected => (parentGarantie?.IsSelected ?? parentBloc?.IsSelected ?? false) && this.IsChecked;

        internal void SetBlocParent(OptionBloc optionBloc)
        {
            this.parentBloc = optionBloc;
            this.SousGaranties.ForEach(x => x.SetParents(this));

        }

        private void SetParents(Garantie garantie)
        {
            this.parentGarantie = garantie;
            this.parentBloc = garantie.parentBloc;
            this.SousGaranties.ForEach(x => x.SetParents(this));

        }

        /* KDETAXREP */
        public Decimal RepartitionTaxe { get; set; }

        ///* KDECRU */
        //public string CreationUser { get; set; }
        ///* KDECRD */
        //public string CreationDate { get; set; }


        public ValeursOptionsTarif Assiette { get; set; }
        ///* KDEASVALO */
        //public Decimal AssietteValeurOrigine { get; set; }
        ///* KDEASVALA */
        //public Decimal AssietteValeurActuel { get; set; }
        ///* KDEASVALW */
        //public Decimal AssietteValeurTravail { get; set; }
        ///* KDEASUNIT */ // D, N
        //public Unite AssietteUnite { get; set; }
        ///* KDEASBASE */
        //public BaseCap AssietteBase { get; set; }
        ///* KDEASMOD */
        //public bool? IsAssietteModif { get; set; }
        ///* KDEASOBLI */
        //public bool? AssietteObligatoire { get; set; }
        /* KDEINVSP */
        public bool? InventaireSpecifique { get; set; }
        /* KDEINVEN */
        public long LienKPINVEN { get; set; }
        /* KDETCD */ // NONCL, ''
        public TypeControleDateEnum TypeControleDate { get; set; }

        internal void ResetIds()
        {
            Id = default;
            SousGaranties.ForEach(g => g.ResetIds());
            foreach (var portee in Portees)
            {
                portee.ResetIds();
            }
            Inventaire?.ResetIds();
            Tarif?.ResetIds();
        }

        /* KDEMODI */
        public bool? IsFlagModifie { get; set; }
        /* KDEPIND */
        public bool? IsParameIndex { get; set; }
        /* KDEPCATN */
        public bool? ParametrageCATNAT { get; set; }
        /* KDEPREF */
        public bool ParametrageIsAlimMontantRef { get; set; }
        ///* KDEPPRP */ // A
        //public string ParametrageApplication { get; set; }
        ///* KDEPEMI */ // P
        //public string ParametrageEmission { get; set; }
        /* KDEPTAXC */
        public Taxe ParamCodetaxe { get; set; }
        /* KDEPNTM or C2NTMNIV */
        public bool? ParamIsNatModifiable { get; set; }
        /* KDEALA or C4ALA */ // A,I,C,B
        public AlimentationValue TypeAlimentation { get; set; }

        /// <summary>
        /// Gets available Actions for Portée
        /// </summary>
        public Dictionary<string, string> ActionsPortees
        {
            get
            {
                if (Niveau != 1)
                    return new Dictionary<string, string>();
                var listActions = new Dictionary<string, string>()
                {
                    { "A", "Accordé" }
                };

                if (!TypeAlimentation.ToString().EndsWith("Prime"))
                {
                    listActions.Add("E", "Exclus");
                }

                return listActions;
            }
        }

        /* KDEPALA */ // A,I,C,B
        public AlimentationValue ParamTypeAlimentation { get; set; }
        public string CodeGarantie { get; set; }

        public int? NumeroAvenant { get; set; }

        /* KDECRAVN */ // 0,1,2
        public int NumeroAvenantCreation { get; set; }

        /* KDEMAJAVN */
        public int NumeroAvenantModif { get; set; }

        public Dictionary<int, (int cravn, int majavn)> OriginalAvnNums { get; set; }

        public ICollection<PorteeGarantie> Portees { get; set; } = new List<PorteeGarantie>();

        public Inventaire.Inventaire Inventaire { get; set; }

        public long IdInventaire { get { return (Inventaire?.Id).GetValueOrDefault(); } }

        public List<GarantieRelation> ParamRelations { get; set; } = new List<GarantieRelation>();

        ///* KDEALO */
        //public string AlimentationOriA { get; set; }

        public bool IsVirtualProposed(int? numeroAvenant)
        {
            return ParamGarantie?.Caractere == CaractereSelection.Propose && NumeroAvenantModif == numeroAvenant;
        }

        public bool IsVirtualOptional(int? numeroAvenant)
        {
            return ParamGarantie?.Caractere == CaractereSelection.Facultatif && ParamGarantie.Nature != NatureValue.Exclue && NumeroAvenantCreation == numeroAvenant
                || ParamGarantie?.Caractere == CaractereSelection.Facultatif && ParamGarantie.Nature == NatureValue.Exclue && NumeroAvenantModif == numeroAvenant;
        }

        internal static Garantie InitWithParameter(Affaire.Affaire affaire, bool parentsChecked, ParamGarantieHierarchie garMod, TruthTable table)
        {
            var garantie = new Garantie
            {
                //TODO completer les références avec des proxys Lazy loading
                LienKPSPEC = 0,
                LienKPGARAP = 0,
                LienKPINVEN = 0,

                Inventaire = null,

                Tarif = InitTarifWithParams(affaire, garMod),
                Portees = new List<PorteeGarantie>(),
                Caractere = garMod.Caractere,

                Assiette = new ValeursOptionsTarif
                {
                    Base = (BaseCapitaux)(garMod.Assiette).CoseBase,
                    ExpressionComplexe = null, // null on purpose
                    IsModifiable = garMod.Assiette.Modifiable,
                    IsObligatoire = garMod.Assiette.Obligatoire,
                    Unite = (UniteCapitaux)(garMod.Assiette.Unite),
                    ValeurActualise = garMod.Assiette.Valeur,
                    ValeurOrigine = garMod.Assiette.Valeur,
                    ValeurTravail = garMod.Assiette.Valeur
                },

                NumeroAvenantCreation = affaire.NumeroAvenant,
                NumeroAvenantModif = affaire.NumeroAvenant,
                Taxe = garMod.Taxe,
                DefinitionGarantie = garMod.ParamGarantie.DefinitionGarantie,
                Duree = 0,
                DureeUnite = null,
                CodeGarantie = garMod.ParamGarantie.CodeGarantie,
                Id = 0,
                NumeroAvenant = null,
                InventaireSpecifique = garMod.ParamGarantie.IsInventPossible,
                IsAlimMontantReference = garMod.IsAlimMontantReference ?? false,
                IsApplicationCATNAT = garMod.IsApplicationCATNAT,
                IsFlagModifie = false,
                IsGarantieAjoutee = false,
                IsIndexe = garMod.IsIndexee ?? false,
                IsParameIndex = garMod.IsIndexee ?? false,
                Nature = garMod.Nature,
                NatureRetenue = NatureValue.None,
                NumDePresentation = 1,
                ParamCodetaxe = garMod.Taxe,
                ParametrageCATNAT = garMod.IsApplicationCATNAT,
                ParametrageIsAlimMontantRef = garMod.IsAlimMontantReference ?? false,
                ParamGarantie = garMod,
                ParamIsNatModifiable = garMod.IsNatModifiable,
                RepartitionTaxe = 0,
                Tri = garMod.Tri, // MakeTri(prefix, garMod.Ordre),
                TypeAlimentation = garMod.Assiette.TypeAlimentation,
                ParamTypeAlimentation = garMod.Assiette.TypeAlimentation,
                PeriodeApplication = new PeriodeApplication { Code = "A" },
                TypeControleDate = garMod.TypeControleDate,
                TypeEmission = new TypeEmission { Code = "P" },
                //ParamRelations = 
            };

            if (garMod.Caractere == CaractereSelection.Propose || garMod.Caractere == CaractereSelection.Facultatif && garMod.Nature == NatureValue.Exclue)
            {
                garantie.NatureRetenue = garMod.Nature;
            }

            garantie.SousGaranties = garMod.GarantiesChildren
                .Select(x => InitWithParameter(affaire, parentsChecked && garantie.IsChecked, x, table))
                .OrderBy(g => g.Tri)
                .ToList();
            return garantie;
        }

        public void UpdateCheck(TruthTable table, NatureValue natureRetenue, bool isChecked)
        {
            var waschecked = IsChecked;
            var entry = table[(this.Caractere, this.Nature)];
            if (ParamIsNatModifiable == true && natureRetenue != NatureValue.None)
            {
                NatureRetenue = natureRetenue;
            }
            else
            {
                NatureRetenue = isChecked ? entry.Checked : entry.Unchecked;
            }
            if (waschecked && !IsChecked || !waschecked && IsChecked)
            {
                IsNew = !IsNew;
            }
        }

        private static TarifGarantie InitTarifWithParams(Affaire.Affaire affaire, ParamGarantieHierarchie garMod)
        {
            var ass = garMod.Assiette;
            var pri = garMod.Prime;
            var lci = garMod.LCI;
            var frh = garMod.Franchise;
            var frmin = garMod.FranchiseMin;
            var frmax = garMod.FranchiseMax;
            return new TarifGarantie()
            {
                PrimeValeur = new ValeursOptionsTarif
                {
                    Base = pri.CoseBase,
                    ExpressionComplexe = null,
                    IsModifiable = pri.Modifiable,
                    IsObligatoire = pri.Obligatoire,
                    Unite = pri.Unite,
                    ValeurActualise = pri.Valeur,
                    ValeurOrigine = pri.Valeur,
                    ValeurTravail = 0
                },
                LCI = new ValeursOptionsTarif
                {
                    Base = lci.CoseBase,
                    ExpressionComplexe = InitExpLci(affaire, lci.Expression),
                    IsModifiable = lci.Modifiable,
                    IsObligatoire = lci.Obligatoire,
                    Unite = lci.Unite,
                    ValeurActualise = lci.Valeur,
                    ValeurOrigine = lci.Valeur,
                    ValeurTravail = 0
                },
                Franchise = new ValeursOptionsTarif
                {
                    Base = frh.CoseBase,
                    ExpressionComplexe = InitExpFrh(affaire, frh.Expression),
                    IsModifiable = frh.Modifiable,
                    IsObligatoire = frh.Obligatoire,
                    Unite = frh.Unite,
                    ValeurActualise = frh.Valeur,
                    ValeurOrigine = frh.Valeur,
                    ValeurTravail = 0
                },
                FranchiseMini = new ValeursTarif
                {
                    Base = frmin.CoseBase,
                    Unite = frmin.Unite,
                    ValeurActualise = frmin.Valeur,
                    ValeurOrigine = frmin.Valeur,
                    ValeurTravail = 0
                },
                FranchiseMax = new ValeursTarif
                {
                    Base = frmax.CoseBase,
                    Unite = frmax.Unite,
                    ValeurActualise = frmax.Valeur,
                    ValeurOrigine = frmax.Valeur,
                    ValeurTravail = 0
                },
                ComptantMontantcalcule = 0,
                ComptantMontantForceHT = 0,
                ComptantMontantForceTTC = 0,
                NumeroAvenant = null,
                NumeroTarif = 1,
                PrimeMontantBase = 0, //kdgmntbas
                PrimeProvisionnelle = 0, //kdgprimpro
                TotalMontantCalcule = 0, // KDGTMC
                TotalMontantForce = 0 //KDGTFF
            };
        }

        private static ExpressionComplexeFranchise InitExpFrh(Affaire.Affaire affaire, ParamExpressionComplexeBase expression)
        {
            var frh = expression as ParamExpressionComplexeFranchise;
            if (frh is null)
                return null;
            if (affaire.Expressions.Franchises.ContainsKey(frh.Code))
            {
                return affaire.Expressions.Franchises[frh.Code];
            }
            var newFranchise = new ExpressionComplexeFranchise()
            {
                Id = 0,
                Description = frh.Description,
                Code = frh.Code,
                Designation = frh.Designation,
                Details = frh.Details.Cast<ParamExpressionComplexeDetailFranchise>().Select(InitExpDetFrh).Cast<ExpressionComplexeDetailBase>().ToList(),
                Origine = OrigineExpression.Referentiel,
                IsModifiable = frh.IsModifiable
            };
            affaire.Expressions.Franchises[frh.Code] = newFranchise;
            return newFranchise;
        }

        private static ExpressionComplexeDetailFranchise InitExpDetFrh(ParamExpressionComplexeDetailFranchise arg)
        {
            return new ExpressionComplexeDetailFranchise
            {
                CodeBase = arg.CodeBase,
                CodeIncide = arg.CodeIncide,
                ExprId = 0,
                Id = 0,
                IncideValeur = arg.IncideValeur,
                LimiteDebut = arg.LimiteDebut,
                LimiteFin = arg.LimiteFin,
                Ordre = arg.Ordre,
                Unite = arg.Unite,
                Valeur = arg.Valeur,
                ValeurMax = arg.ValeurMax,
                ValeurMin = arg.ValeurMin
            };
        }

        private static ExpressionComplexeLCI InitExpLci(Affaire.Affaire affaire, ParamExpressionComplexeBase expression)
        {

            var lci = expression as ParamExpressionComplexeLCI;
            if (lci is null)
                return null;
            if (affaire.Expressions.LCIs.ContainsKey(lci.Code))
            {
                return affaire.Expressions.LCIs[lci.Code];
            }
            var newLci = new ExpressionComplexeLCI()
            {
                Id = 0,
                Description = lci.Description,
                Code = lci.Code,
                Designation = lci.Designation,
                Details = lci.Details.Cast<ParamExpressionComplexeDetailLCI>().Select(InitExpDetLCI).Cast<ExpressionComplexeDetailBase>().ToList(),
                Origine = OrigineExpression.Referentiel,
                IsModifiable = lci.IsModifiable
            };
            affaire.Expressions.LCIs[lci.Code] = newLci;
            return newLci;

        }

        private static ExpressionComplexeDetailLCI InitExpDetLCI(ParamExpressionComplexeDetailLCI arg)
        {
            return new ExpressionComplexeDetailLCI
            {
                CodeBase = arg.CodeBase,
                ExprId = 0,
                Id = 0,
                Ordre = arg.Ordre,
                Unite = arg.Unite,
                Valeur = arg.Valeur,
                ConcurrentCodeBase = arg.ConcurrentCodeBase,
                ConcurrentUnite = arg.ConcurrentUnite,
                ConcurrentValeur = arg.ConcurrentValeur
            };
        }

        internal void ApplyDatesOnGuarantie(Option option, Risque.Risque risqueOption, Affaire.Affaire affaire)
        {
            var optDate = option.DateAvenant;
            var appObj = option.Applications.All(x => x.Niveau == ApplicationNiveau.Objet);

            // Modification de la date de début
            bool isCreation = NumeroAvenantCreation == option.NumAvenantModif && option.NumAvenantModif == affaire.NumeroAvenant;
            if (isCreation && optDate.HasValue && (DateDebut < optDate.Value || !DateDebut.HasValue && NumeroAvenantCreation > 0))
            {
                DateDebut = optDate.Value;
            }
            IEnumerable<Objet> objGar = risqueOption.Objets;
            if (HasPortees)
            {
                if (Portees.Any(x => x.Action == ActionValue.Accorde))
                {
                    // Inclusion : filter-in included
                    objGar = risqueOption.Objets.Where(o => this.Portees.Any(x => o.Id.NumObjet == x.NumObjet && o.Id.NumRisque == x.RisqueId));
                }
                else
                {
                    // Exclusion : filter-out excluded
                    objGar = risqueOption.Objets.Where(o => !this.Portees.Any(x => o.Id.NumObjet == x.NumObjet && o.Id.NumRisque == x.RisqueId));
                }
            }
            else if (appObj)
            {
                // Application on objet : filter-in applying
                objGar = option.FilterObjets(risqueOption);
            }
            else
            {
                // No specific objet
                objGar = Enumerable.Empty<Objet>();
            }
            objGar = objGar.ToList();

            DateTime? deb = objGar.Any(x => !x.DateDebut.HasValue) ? null : objGar.Min(x => x.DateDebut);
            DateTime? fin = objGar.Any(x => !x.DateFin.HasValue) ? null : objGar.Max(x => x.DateFin);

            if (!deb.HasValue)
            {
                deb = risqueOption.DateDebut;
            }
            if (!fin.HasValue)
            {
                fin = risqueOption.DateFin;
            }
            if (!deb.HasValue && affaire.DateEffet.HasValue)
            {
                deb = affaire.DateEffet.Value;
            }
            if (isCreation && (!deb.HasValue || deb.Value < optDate))
            {
                deb = optDate;
            }
            if (!fin.HasValue)
            {
                fin = affaire.DateFinCalculee;
            }

            DatestandardDebut = deb;
            if (isCreation && optDate.HasValue && (DateDebut < optDate.Value || !DateDebut.HasValue && NumeroAvenantCreation > 0))
            {
                DateDebut = deb > optDate.Value ? deb : optDate.Value;
            }
            DatestandardFin = fin;

            AdjustDates();
        }

        public void ApplyDatesCurrentAvenant(AffaireId id, Option option, DateTime? dateAvenant) {
            if (dateAvenant.HasValue && !id.IsHisto) {
                if (NumeroAvenantCreation == id.NumeroAvenant && (DatestandardDebut ?? DateTime.MinValue) < dateAvenant.Value) {
                    DatestandardDebut = dateAvenant.Value;
                }
                else if (NumeroAvenantCreation < id.NumeroAvenant && NumeroAvenantCreation > 0
                    && option.AvnDatesHistory != null
                    && option.AvnDatesHistory.TryGetValue(NumeroAvenantCreation, out var dt)
                    && (DatestandardDebut ?? DateTime.MinValue) < dt.Value) {
                    DatestandardDebut = dt.Value;
                }
            }
        }

        public void AdjustDates()
        {
            if (this.ParamGarantie.ParamGarantie.DefinitionGarantie.Code == DefinitionGarantie.ParfaitAchevement
                && this.ParamGarantie.ParamGarantie.DefinitionGarantie.ParamNum1 > 0
                && DatestandardFin.HasValue) {
                var debnew = DatestandardFin.Value.AddMonths(-(int)this.ParamGarantie.ParamGarantie.DefinitionGarantie.ParamNum1).AddMinutes(1);
                //if (IsVirtual)
                {
                    DateDebut = debnew;
                    DateFinDeGarantie = DatestandardFin;
                }
            }
        }

        public bool IsVirtual => Id <= 0;

        public void EnsureInventaire()
        {
            Inventaire = new Inventaire.Inventaire()
            {
                TypeInventaire = ParamGarantie.ParamGarantie.TypeInventaire
            };
        }

        internal void ApplyParameters(Affaire.Affaire affaire, bool parentsChecked, ParamGarantieHierarchie paramGarantieHierarchie, TruthTable table, int level)
        {
            foreach (var garMod in paramGarantieHierarchie.GarantiesChildren)
            {
                var gar = this.FindGarantieBySeq(garMod.Sequence);
                var ischecked = this.IsChecked && parentsChecked;
                if (gar != null)
                {
                    gar.ApplyParameters(affaire, IsChecked, garMod, table, level: level + 1);
                }
                else
                {
                    gar = InitWithParameter(affaire, IsChecked, garMod, table);
                    this.SousGaranties.Add(gar);
                }
            }

            this.Tri = this.ParamGarantie.Tri;
            this.SousGaranties = this.SousGaranties.OrderBy(x => x.Tri).ToList();
        }

        public IEnumerable<ValidationError> CheckDates()
        {
            Option currentOption = this.parentBloc.parent.parent;
            if (this.TypeControleDate == TypeControleDateEnum.Ignore)
            {
                yield break;
            }
            //var isDateDebutForcee = this.ParamGarantie.ParamGarantie.DefinitionGarantie.Code == DefinitionGarantie.ParfaitAchevement && this.ParamGarantie.ParamGarantie.DefinitionGarantie.ParamNum1 > 0;
            var isStrict = this.TypeControleDate == TypeControleDateEnum.Strict;
            if (isStrict || this.TypeControleDate == TypeControleDateEnum.IgnoreEnd)
            {
                if (this.DateDebut < (this.DatestandardDebut ?? DateTime.MinValue))
                {
                    yield return new ValidationError(nameof(Garantie), this.CodeGarantie, this.ParamGarantie.Sequence.ToString(), "La date de début forcée doit être supérieure ou égale à la date de début standard");
                }
                if (this.DateDebut > (this.DatestandardFin ?? DateTime.MaxValue))
                {
                    yield return new ValidationError(nameof(Garantie), this.CodeGarantie, this.ParamGarantie.Sequence.ToString(), "DateDebut", "La date de début forcée doit être inférieure ou égale à la date de fin standard");
                }
            }
            if (isStrict || this.TypeControleDate == TypeControleDateEnum.IgnoreStart)
            {
                if (this.DateSortieEffective < (this.DatestandardDebut ?? DateTime.MinValue))
                {
                    yield return new ValidationError(nameof(Garantie), this.CodeGarantie, this.ParamGarantie.Sequence.ToString(), "DateFin", "La date de fin forcée doit être supérieure ou égale à la date de début standard");
                }
                if (this.DateSortieEffective > (this.DatestandardFin ?? DateTime.MaxValue))
                {
                    yield return new ValidationError(nameof(Garantie), this.CodeGarantie, this.ParamGarantie.Sequence.ToString(), "DateFin", "La date de fin forcée doit être inférieure ou égale à la date de fin standard");
                }
            }
            if (currentOption?.NumAvenantModif > 0)
            {
                if (NumeroAvenantCreation == currentOption.NumAvenantModif)
                {
                    if (this.DateDebut < currentOption.DateAvenant
                        && (isStrict || this.TypeControleDate == TypeControleDateEnum.IgnoreEnd)
                    )
                    {
                        yield return new ValidationError(nameof(Garantie), this.CodeGarantie, this.ParamGarantie.Sequence.ToString(), "DateDebut", "La date de début forcée doit être supérieure ou égale à la date de modification de la formule");
                    }
                }
                else if (NumeroAvenantCreation < currentOption.NumAvenantModif)
                {
                    if (DateSortieEffective.HasValue && DateSortieEffective.Value < currentOption.DateAvenant.Value.AddMinutes(-1)
                        && (isStrict || this.TypeControleDate == TypeControleDateEnum.IgnoreStart)
                    )
                    {
                        yield return new ValidationError(nameof(Garantie), this.CodeGarantie, this.ParamGarantie.Sequence.ToString(), "DateFin", "La date de fin forcée doit être supérieure ou égale à la date de modification de la formule moins une minute");
                    }
                }
            }
            if (DateDebut.HasValue && DateSortieEffective.HasValue && DateDebut > DateSortieEffective)
            {
                yield return new ValidationError(nameof(Garantie), this.CodeGarantie, this.ParamGarantie.Sequence.ToString(), "La date de début forcée doit être inférieure ou égale à la date de fin forcée");
            }
        }

        public DateTime DateDebutCalculee => (DateDebut ?? DatestandardDebut) ?? DateTime.MinValue;
        public DateTime DateFinCalculee
        {
            get
            {
                return (DateSortieEffective ?? DatestandardFin) ?? DateTime.MaxValue;
            }
        }
        public DateTime? DateSortieEffective
        {
            get
            {
                DateTime? fin = null;
                if (DateFinDeGarantie.GetValueOrDefault() != default)
                {
                    fin = DateFinDeGarantie.Value;
                }
                else if (Duree.HasValue && DureeUnite != null)
                {
                    var duree = Duree.Value;
                    var debut = DateDebutCalculee;
                    var unite = DureeUnite.Code.AsEnum<UniteDureeValeur>();
                    fin = debut.AddUnit(duree, unite).AddDays(1).AddMinutes(-1);
                }
                return fin;
            }
        }

        public void ReporterInventaire(bool report, bool wasReport)
        {
            if (Inventaire == null)
            {
                throw new Exception("Pas d'inventaire à reporter");
            }
            if (report)
            {
                if (TypeAlimentation == AlimentationValue.Inventaire)
                {
                    Assiette.ValeurActualise = Inventaire.Valeurs.ValeurActualise;
                    Assiette.ValeurOrigine = Inventaire.Valeurs.ValeurActualise;
                    Assiette.ValeurTravail = 0;
                    Assiette.Unite = Inventaire.Valeurs.Unite;
                    Assiette.Base = new BaseCapitaux { Code = Inventaire.Typedevaleur.Code };
                }
                else
                {
                    Assiette.ValeurTravail = Inventaire.Valeurs.ValeurActualise;
                }
            }
            else if (wasReport)
            {
                if (TypeAlimentation == AlimentationValue.Inventaire)
                {
                    Assiette.ValeurActualise = 0;
                    Assiette.ValeurOrigine = 0;
                    Assiette.Unite = null;
                    Assiette.Base = null;
                }
                Assiette.ValeurTravail = 0;
            }
        }

        public void AssignNumerosAvenant(int numeroAvenant)
        {
            if (IsChecked && !IsVirtual)
            {
                NumeroAvenantModif = numeroAvenant;
                if ((ParamGarantie.Caractere == CaractereSelection.Propose) || (ParamGarantie.Caractere == CaractereSelection.Facultatif
                    && (ParamGarantie.Nature != NatureValue.Exclue || NatureRetenue != NatureValue.Exclue)))
                {
                    NumeroAvenantCreation = numeroAvenant;
                }
            }
        }

        public void RestoreNumerosAvenant(int numeroAvenant)
        {
            if (!IsChecked && !IsVirtual && OriginalAvnNums?.Any(x => x.Key == (numeroAvenant - 1)) == true)
            {
                NumeroAvenantModif = OriginalAvnNums[numeroAvenant - 1].majavn;
                if ((ParamGarantie.Caractere == CaractereSelection.Propose) || (ParamGarantie.Caractere == CaractereSelection.Facultatif
                    && (ParamGarantie.Nature != NatureValue.Exclue || NatureRetenue != NatureValue.Exclue)))
                {
                    NumeroAvenantCreation = OriginalAvnNums[numeroAvenant - 1].cravn;
                }
            }
        }

        internal bool IsSamePeriodAs(Garantie other)
        {
            return (other.DateFinCalculee == this.DateFinCalculee)
        || (other.DateDebutCalculee >= this.DateDebutCalculee && other.DateDebutCalculee < this.DateFinCalculee);
        }
        internal bool IsOverlapplingWith(Garantie other)
        {
            return (other.DateFinCalculee > this.DateDebutCalculee && other.DateFinCalculee <= this.DateFinCalculee)
                || (other.DateDebutCalculee >= this.DateDebutCalculee && other.DateDebutCalculee < this.DateFinCalculee)
                || (other.DateDebutCalculee < this.DateDebutCalculee && other.DateFinCalculee > this.DateFinCalculee)
                || (other.DateDebutCalculee > this.DateDebutCalculee && other.DateFinCalculee < this.DateFinCalculee);
        }

        internal bool IsSubPeriodOf(Garantie other)
        {
            return (other.DateFinCalculee > this.DateDebutCalculee && other.DateFinCalculee <= this.DateFinCalculee)
                && (other.DateDebutCalculee >= this.DateDebutCalculee && other.DateDebutCalculee < this.DateFinCalculee);
        }

        internal IEnumerable<Garantie> Flatten()
        {
            return new[] { this }.Concat(this.SousGaranties.SelectMany(x => x.Flatten()));
        }

        public virtual IEnumerable<Garantie> AllUncheckedGaranties
        {
            get
            {
                if (!IsChecked)
                {
                    // not selected : all under are unselected too
                    return Flatten();
                }
                return SousGaranties.SelectMany(x => x.AllUncheckedGaranties);
            }
        }

        public virtual IEnumerable<Garantie> AllCheckedGaranties
        {
            get
            {
                if (!IsChecked)
                {
                    // not selected : all under are unselected too
                    Enumerable.Empty<Garantie>();
                }
                return new[] { this }.Concat(SousGaranties.SelectMany(x => x.AllCheckedGaranties));
            }
        }
    }
}
