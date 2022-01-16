using Albingia.Kheops.OP.Application.Infrastructure.Extension;
using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.DataAdapter.DataModel.Repository;
using Albingia.Kheops.OP.Domain;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.DataAdapter {
    public class TraceRepository : ITraceRepository {
        private readonly YpoTracRepository ypoTracRepository;
        private readonly KPTraceRepository kptraceRepository;
        public TraceRepository(YpoTracRepository ypoTracRepository, KPTraceRepository kptraceRepository) {
            this.ypoTracRepository = ypoTracRepository;
            this.kptraceRepository = kptraceRepository;
        }
        public void TraceInfo(TraceContext traceContext) {
            this.ypoTracRepository.Insert(new DataModel.DTO.YpoTrac() {
                Pytyp = traceContext.AffaireId.TypeAffaire.AsCode(),
                Pyipb = traceContext.AffaireId.CodeAffaire,
                Pyalx = traceContext.AffaireId.NumeroAliment,
                Pyavn = traceContext.AffaireId.NumeroAvenant ?? 0,
                Pyttr = traceContext.TypeTraitement,
                Pyvag = traceContext.TypeAction,
                Pyord = traceContext.Ordre,
                Pytra = DateTime.Today.Year,
                Pytrm = DateTime.Today.Month,
                Pytrj = DateTime.Today.Day,
                Pytrh = DateTime.Now.ToIntHM(),
                Pymja = DateTime.Today.Year,
                Pymjm = DateTime.Today.Month,
                Pymjj = DateTime.Today.Day,
                Pymjh = DateTime.Now.ToIntHM(),
                Pylib = traceContext.Libelle,
                Pymju = traceContext.User,
                Pyinf = "I"
            });
            //this.ypoTracRepository.TraceInfo(
            //    traceContext.AffaireId.CodeAffaire,
            //    traceContext.AffaireId.NumeroAliment,
            //    traceContext.AffaireId.TypeAffaire.AsCode(),
            //    traceContext.AffaireId.NumeroAvenant.GetValueOrDefault(),
            //    traceContext.TypeTraitement,
            //    traceContext.TypeAction,
            //    traceContext.Ordre,
            //    traceContext.User,
            //    DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Now.ToIntHM(),
            //    traceContext.Libelle, "I", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, DateTime.Now.ToIntHM());
        }

        public void Log(SimpleTrace trace) {
            try {
                this.kptraceRepository.Create(new DataModel.DTO.KPTrace {
                    Kccalx = trace.AffaireId.NumeroAliment,
                    Kcccrd = trace.DateCreation.Date.ToIntYMD(),
                    Kcccrh = trace.DateCreation.ToIntHMS(),
                    Kcccru = trace.User,
                    Kccfor = trace.Formule,
                    Kccgar = trace.CodeGarantie,
                    Kccipb = trace.AffaireId.CodeAffaire,
                    Kccobj = trace.Objet,
                    Kccopt = trace.Option,
                    Kccrsq = trace.Risque,
                    Kcctyp = trace.AffaireId.TypeAffaire.AsCode(),
                    Kcclib = trace.Commentaire
                });
            }
            catch(Exception e) {
                Trace.WriteLine($"Error in {nameof(TraceRepository)}.{nameof(TraceRepository.Log)}:{Environment.NewLine}{e}");
            }
        }

        public TraceContext GetLastTrace(AffaireId affaireId) {
            var potrc = this.ypoTracRepository.GetByAffaireLast(affaireId.CodeAffaire, affaireId.NumeroAliment, affaireId.NumeroAvenant ?? 0).FirstOrDefault();
            return potrc is null ? null : new TraceContext {
                TypeTraitement = potrc.Pyttr,
                Libelle = potrc.Pylib
            };
        }

        public IEnumerable<SimpleTrace> FindLogsByCommentEndsWith(DateTime date, string endString) {
            return this.kptraceRepository.FindByCommentEnd(date, endString).Select(x => MapSimpleTrace(x)).ToList();
        }

        public void DeleteTraceOldISTransfert(DateTime now, string guid) {
            var trace = this.kptraceRepository.FindByCommentEnd(now, guid).FirstOrDefault();
            if (trace is null) {
                return;
            }
            this.kptraceRepository.Delete(trace);
        }

        private SimpleTrace MapSimpleTrace(DataModel.DTO.KPTrace trace) {
            return new SimpleTrace {
                AffaireId = new AffaireId {
                    NumeroAliment = trace.Kccalx,
                    CodeAffaire = trace.Kccipb,
                    TypeAffaire = trace.Kcctyp.ParseCode<AffaireType>(),
                },
                CodeGarantie = trace.Kccgar,
                Formule = trace.Kccfor,
                Objet = trace.Kccobj,
                User = trace.Kcccru,
                Option = trace.Kccopt,
                Risque = trace.Kccrsq,
                Commentaire = trace.Kcclib,
                DateCreation = Tools.MakeDateTime(trace.Kcccrd, trace.Kcccrh)
            };
        }
    }
}
