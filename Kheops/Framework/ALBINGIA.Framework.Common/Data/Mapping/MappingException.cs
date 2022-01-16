using System;

namespace ALBINGIA.Framework.Common.Data.Mapping
{
    /// <summary>
    /// Classe d'exception pour le mapping object relationnel
    /// </summary>
    public class MappingException : ApplicationException
    {
        #region Constructors, Destructors
        /// <summary>
        /// Initialisation de l'instance MappingException class avec le mesage d'erreur.
        /// </summary>
        /// <param name="pMessage">Describtion de l'erreur</param>
        public MappingException(string pMessage)
            
        {
            //Logger.TraceEvent(TraceEventType.Error, 0, p_message);
            //ArcLogBase.ArcLogBaseIns log = new ArcLogBase();

            //TODO : ZBO Trace

        }
        #endregion

       
    }
}
