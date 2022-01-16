using System;

namespace ALBINGIA.Framework.Common.Data.Mapping
{
    /// <summary>
    /// create a new instance for MappingException class.
    /// </summary>
    
    public class CannotMapMappingException : MappingException
    {
        #region Constructors, Destructors
        /// <summary>
        /// Initialisation de l'instance ApplicationException class avec le mesage d'erreur.
        /// </summary>
        /// <param name="p_message">Describtion de l'erreur</param>
        public CannotMapMappingException(string p_message)
            :base(p_message)
        {
            //TODO : ZBO Trace
        }
        #endregion
    }
}
