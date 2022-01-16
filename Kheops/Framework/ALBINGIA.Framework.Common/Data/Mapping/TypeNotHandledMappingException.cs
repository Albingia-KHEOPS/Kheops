using System;

namespace ALBINGIA.Framework.Common.Data.Mapping
{
    /// <summary>
    /// Exception mapping throw when the dataType is not supporting
    /// </summary>
    public class TypeNotHandledMappingException : MappingException
    {
        #region Constructors, Destructors
        /// <summary>
        /// Crée un nouvelle instance de l'exception TypeNotHandledMappingException.
        /// </summary>
        /// <param name="pMessage">A message that describes the error. </param>
        public TypeNotHandledMappingException(string pMessage)
            : base(pMessage)
        {

            //TODO : ZBO Trace
        }
        #endregion
    }
}
