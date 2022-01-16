using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Models.Common;

namespace ALBINGIA.Framework.Common.Tools {
    public class AlbLog {
        private static readonly log4net.ILog log;
        private static PerformanceCounter timeLoadPage { get; set; }
        private static PerformanceCounter timeExecQuery { get; set; }
        public static string AlbWndEventLog { get; set; }

        static AlbLog() {
            log = log4net.LogManager.GetLogger(typeof(AlbLog));
        }
        protected AlbLog() { }

        public enum LogTraceLevel {
            Tous = 0,
            Erreur = EventLogEntryType.Error,
            Information = EventLogEntryType.Information,
            Avertissement = EventLogEntryType.Warning
        }

        public static AlbLog Log(string message, LogTraceLevel traceLevel = LogTraceLevel.Information) {
            var log = new AlbLog();
            switch (traceLevel) {
                case LogTraceLevel.Erreur:
                    log.WriteError(message);
                    break;
                case LogTraceLevel.Avertissement:
                    log.WriteWarning(message);
                    break;
                default:
                    log.WriteInfo(message);
                    break;
            }

            return log;
        }

        public static AlbLog Warn(string warning) {
            var log = new AlbLog();
            log.WriteWarning(warning);
            return log;
        }

        /// <summary>
        /// Log une erreur.
        /// </summary>
        /// <param name="message">Message</param>
        public void WriteError(string message) {
            WriteToEventLog(message, LogTraceLevel.Erreur);
            if (AlbOpConstants.FileLog) {
                log.Error(message);
            }
        }

        /// <summary>
        /// Log une warning.
        /// </summary>
        /// <param name="message">Message</param>
        public void WriteWarning(string message) {
            WriteToEventLog(message, LogTraceLevel.Avertissement);
            if (AlbOpConstants.FileLog) {
                log.Warn(message);
            }
        }

        /// <summary>
        /// Log une Info.
        /// </summary>
        /// <param name="message">Message</param>
        public void WriteInfo(string message) {
            WriteToEventLog(message, LogTraceLevel.Information);
            if (AlbOpConstants.FileLog) {
                log.Info(message);
            }
        }

        /// <summary>
        /// récupèreles traces du journal d'évènnement windows
        /// </summary>
        /// <param name="dateLogDeb">Date début</param>
        /// <param name="dateLogFin">DateFin</param>
        /// <param name="levelTrace">Niveau d'erreur</param>
        /// <param name="contentCriteria">Contenu cherché</param>
        /// <returns></returns>
        public static List<LogTrace> GetLogTrace(DateTime? dateLogDeb, DateTime? dateLogFin, LogTraceLevel levelTrace, string contentCriteria) {
            List<LogTrace> resListTrace = null;
            string source = string.Format("{0}-{1}", AlbOpConstants.ClientWorkEnvironment, AlbOpConstants.ApplicationName);
            var albEventLog = new EventLog { Source = source, Log = source };
            foreach (var evEntry in albEventLog.Entries
                .Cast<EventLogEntry>()
                .Where(evEntry => (!dateLogDeb.HasValue || dateLogDeb.Value.Date <= evEntry.TimeGenerated.Date)
                    && (!dateLogFin.HasValue || dateLogFin.Value.Date >= evEntry.TimeGenerated.Date)
                    && (levelTrace == LogTraceLevel.Tous || evEntry.EntryType == (EventLogEntryType)levelTrace)
                    && (string.IsNullOrEmpty(contentCriteria) || evEntry.Message.ToLower().Contains(contentCriteria.ToLower())))) {
                if (resListTrace == null) {
                    resListTrace = new List<LogTrace>();
                }

                var logTraceLine = new LogTrace {
                    Level = (LogTraceLevel)evEntry.EntryType,
                    DateLog = evEntry.TimeGenerated,
                    Utilisateur = evEntry.UserName,
                    MachineName = evEntry.MachineName,
                    ContentLog = evEntry.Message
                };
                resListTrace.Add(logTraceLine);
            }
            return resListTrace;
        }

        #region Méthodes privées
        private void WriteToEventLog(string message, LogTraceLevel traceLevel) {
            if (AlbOpConstants.WindowEventLog) {
                string source = string.Format("{0}-{1}", AlbOpConstants.ClientWorkEnvironment, AlbOpConstants.ApplicationName);
                if (!EventLog.Exists(source)) {
                    if (!CreateLog(source)) {
                        source = null;
                    }
                }
                if (source != null) {
                    var albEventLog = new EventLog { Source = source, Log = source };
                    albEventLog.WriteEntry(message, traceLevel == 0 ? EventLogEntryType.SuccessAudit : (EventLogEntryType)traceLevel);
                }
            }
        }
        private bool CreateLog(string strLogName) {
            try {
                EventLog.CreateEventSource(strLogName, strLogName);
                var albEventLog = new EventLog { Source = strLogName, Log = strLogName };
                albEventLog.Source = strLogName;
                albEventLog.WriteEntry("Le journal " + strLogName + " a été ajouté avec succés", EventLogEntryType.Information);
                return true;
            }
            catch {
                try {
                    EventLog.WriteEntry("Application", "Erreur de création de la source de journal Log Windows", EventLogEntryType.FailureAudit);
                }
                catch { }
            }
            return false;
        }
        #endregion
    }
}
