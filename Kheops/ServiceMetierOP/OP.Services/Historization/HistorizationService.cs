using OP.WSAS400.DTO;
using System;
using System.Data;
using System.Diagnostics;

namespace OP.Services.Historization {
    public class HistorizationService {

        public HistorizationService() { }

        public void Archive(IDbConnection connection, HistorizationContext context) {
            string type = null;
#if DEBUG
            var watcher = new Stopwatch();
            watcher.Start();
            
#endif
            using (var archiver = CreateArchiver(connection, context)) {
                type = archiver.GetType().FullName;
                archiver.PerformProcess();
            }
#if DEBUG
            watcher.Stop();
            //Trace.WriteLine($"HistorizationService.Archive ({type}) duration: {watcher.ElapsedMilliseconds}ms");
            watcher = null;
#endif
        }

        private Archiver CreateArchiver(IDbConnection connection, HistorizationContext context) {
            if (context != null) {
                return Archiver.Build(connection, context);
            }

            throw new ArgumentNullException(nameof(context));
        }
    }
}
