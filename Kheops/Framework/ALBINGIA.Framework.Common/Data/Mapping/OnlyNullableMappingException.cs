using System;

namespace ALBINGIA.Framework.Common.Data.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class OnlyNullableMappingException : MappingException
    {
        #region Constructors, Destructors
        /// <summary>
        /// Initialisation de l'instance OnlyNullableMappingException class avec le mesage d'erreur.
        /// </summary>
        /// <param name="pMessage">Describtion de l'erreur</param>
        public OnlyNullableMappingException(string pMessage)
            : base(pMessage)
        {
            //TODO : ZBO Trace
        }
        #endregion
    }
}
