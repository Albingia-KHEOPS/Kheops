using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.DTO;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Clauses;
using Albingia.Kheops.OP.Domain.Document;
using Albingia.Kheops.OP.Domain.Extension;
using Albingia.Kheops.OP.Domain.Referentiel;
using Albingia.Kheops.OP.Domain.Transverse;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using static Albingia.Kheops.OP.Application.Infrastructure.Extension.Tools;
using static ALBINGIA.Framework.Common.Constants.AlbConstantesMetiers;

namespace Albingia.Kheops.OP.DataAdapter
{
    public class ClauseRepository : IClauseRepository
    {

        KClauseRepository pclRepo;
        KpClauseRepository clRepo;
        HpclauseRepository hclRepo;
        IDesignationRepository desiRepo;
        IReferentialRepository refRepo;
        IGenericCache cache;

        public ClauseRepository(
            KClauseRepository pclRepo,
            KpClauseRepository clRepo,
            HpclauseRepository hclRepo,
            IDesignationRepository desiRepo,
            IReferentialRepository refRepo,
            IGenericCache cache
        )
        {
            this.clRepo = clRepo;
            this.hclRepo = hclRepo;
            this.pclRepo = pclRepo;
            this.desiRepo = desiRepo;
            this.refRepo = refRepo;
            this.cache = cache;
        }

        public IEnumerable<ParamClause> GetClauseParams()
        {
            var @params = GetParams();
            return @params.Values;
        }

        private Dictionary<long, ParamClause> GetParams()
        {
            return cache.Get(() => pclRepo.GetAll().Select(MapParam).ToDictionary(x => x.Id));
        }

        public ParamClause GetClauseParam(long id) {
            GetParams().TryGetValue(id, out var returnValue);
            return returnValue;
        }

        private ParamClause MapParam(KClause arg)
        {
            var p = new ParamClause
            {
                ActeDeGestion = arg.Kduactg,
                CodeRegroupement = arg.Kdurgp,
                Creation = new UpdateMetadata
                {
                    User = arg.Kducru,
                    Time = MakeNullableDateTime(arg.Kducrd, arg.Kducrh)
                },
                MiseAJour = new UpdateMetadata
                {
                    User = arg.Kdumaju,
                    Time = MakeNullableDateTime(arg.Kdumajd, arg.Kdumajh)
                },
                TypeDeDocument = arg.Kdutdoc.AsEnum<TypeDocument>(),
                Service = arg.Kduserv,
                Id = arg.Kduid,
                IdEmplacement = arg.Kdukdvid,
                IdMotCle = arg.Kdukdxid,
                Designation = desiRepo.GetDesignation(arg.Kdukdwid),
                Libelle = arg.Kdulib,
                LibelleRaccourci = arg.Kdulir,
                NomDoc = arg.Kdudoc,
                Nom = new NomClause(arg.Kdunm1, arg.Kdunm2, arg.Kdunm3, arg.Kduver),
                DateValiditeDebut = MakeNullableDateTime(arg.Kdudatd),
                DateValiditeFin = MakeNullableDateTime(arg.Kdudatf)
            };
            return p;
        }

        public IEnumerable<Clause> GetClauses(AffaireId id)
        {
            IEnumerable<Clause> clauses = (
                id.IsHisto ?
                hclRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value ) :
                clRepo.GetByAffaire(id.TypeAffaire.AsCode(), id.CodeAffaire, id.NumeroAliment )
            ).Select(MapClause);
            return clauses;
        }

        private Clause MapClause(KpClause arg)
        {
            var clause = new Clause
            {
                IsAjoutee = arg.Kcaajt.AsBool(),
                AttributImpression = !arg.Kcaaim.AsBool(""),
                Chapitre = arg.Kcachi,
                Souschapitre = arg.Kcachis,
                CodeAnnexe = arg.Kcaiac,
                Contexte = refRepo.GetValue<Contexte>( arg.Kcactx),
                CreationDate = MakeDateTime(arg.Kcacrd),
                MiseAJourDate = MakeDateTime(arg.Kcamajd),
                AffaireId = new AffaireId() { CodeAffaire = arg.Kcaipb, IsHisto = arg.Kcaavn.HasValue, TypeAffaire = arg.Kcatyp.AsEnum<AffaireType>(), NumeroAliment = arg.Kcaalx, NumeroAvenant = arg.Kcaavn },
                EtapeAffichage = arg.Kcaetaff.AsEnum<Etapes>(),
                EtapeDeGeneration = arg.Kcaetape.AsEnum<Etapes>(),
                Formule = arg.Kcafor,
                Garantie = arg.Kcagar,
                Option = arg.Kcaopt,
                Risque = arg.Kcarsq,
                Objet = arg.Kcaobj,

                GenerateurId = arg.Kcaelgi,
                OrigineGenerateur = arg.Kcaelgo.AsEnum<OrigineClause>(),
                IdClause = arg.Kcakduid,
                ParamClause = this.GetClauseParam(arg.Kcakduid),
                IdUnique = arg.Kcaid,
                IdClauseMere = arg.Kcamer,
                IdDoc = arg.Kcatxl,
                IdInventaire = arg.Kcainven,

                IsAnnexe = arg.Kcaian.AsBool(),
                IsSpecifiqueAvenant = arg.Kcaspa.AsBool(),
                IsTexteLibre = arg.Kcaxtl.AsBool(),
                IsTexteLibreModifie = arg.Kcaxtlm.AsBool(),
                Nature = arg.Kcanta.AsEnum<NatureGeneration>(),
                NomClause = new NomClause(arg.Kcaclnm1, arg.Kcaclnm2, arg.Kcaclnm3, arg.Kcaver),
                NumeroAvenantCreation = arg.Kcaavnc,
                NumeroAvenantModification = arg.Kcaavnm,
                NumeroOrdonnancement = arg.Kcacxi,
                NuneroImpression = arg.Kcaimp,
                Perimetre = arg.Kcaperi.AsEnum<Etapes>(),
                Situation = arg.Kcasit.AsEnum<SituationClause>(),
                SituationDate = MakeNullableDateTime(arg.Kcasitd),
                TypeGenerationDoc = arg.Kcatypd.AsEnum<TypeGenerationDoc>(),
                TypologieSensibilite = arg.Kcatypo.AsEnum<TypologieSensibilite>(),

            };
            return clause;
        }

        public void Reprise(AffaireId id)
        {
            var list = this.clRepo.GetAllByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment).ToList();
            list.ForEach(x => this.clRepo.Delete(x));
            var histo = this.hclRepo.GetByAffaire(AlbConstantesMetiers.TYPE_CONTRAT, id.CodeAffaire, id.NumeroAliment, id.NumeroAvenant.Value - 1).ToList();
            histo.ForEach(x => this.hclRepo.Delete(x));
            histo.ForEach(x => this.clRepo.Insert(x));
        }
    }
}
