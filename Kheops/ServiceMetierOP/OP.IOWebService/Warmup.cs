using Albingia.Kheops.OP.Application.Port.Driven;
using Albingia.Kheops.OP.Domain.Referentiel;
using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OP.IOWebService {
    public class Warmup {
        internal static Task Load(ServiceContainer ctx) {

            List<Action<IParamRepository>> fetchParams = new List<Action<IParamRepository>> {
                (r => r.GetDesignations()),
                (r => r.GetTypeInventaires()),
                (r => r.GetExpressionFranchises()),
                (r => r.GetExpressionLCIs()),
                (r => r.GetParamVolets()),
            };
            List<Action<IReferentialRepository>> fetch = new List<Action<IReferentialRepository>> {
                (r => r.GetCibleCategos()),
                (r => r.GetCibles()),
                (r => r.GetUtilisateurs())
            };
            var refTypes = typeof(RefValue).Assembly.GetExportedTypes().Where(x => typeof(RefValue).IsAssignableFrom(x) && !x.IsAbstract);

            var refFetchers = refTypes.Select<Type, Action<IReferentialRepository>>(x =>
                 ((IReferentialRepository rf) => {
                     rf.GetType().GetMethod("GetValues").MakeGenericMethod(x).Invoke(rf, new object[0]);
                 })
            );

            fetch.AddRange(refFetchers);
            List<Action> actions =
                    fetch.Select<Action<IReferentialRepository>, Action>(action => (() => { RunInScope(ctx, action); }))
                .Concat(
                    fetchParams.Select<Action<IParamRepository>, Action>(action => (() => { RunInScope(ctx, action); }))
                ).ToList();
            return Task.Run(() => {
                actions.ForEach(x => x());
            });

        }

        static void RunInScope<T>(ServiceContainer ctx, Action<T> action) {
            using (var scope = ctx.BeginScope()) {
                var inst = scope.GetInstance<T>();
                try {
                    action(inst);
                }
                catch {
                    //Ignore Errors
                }
            }
        }
    }
}