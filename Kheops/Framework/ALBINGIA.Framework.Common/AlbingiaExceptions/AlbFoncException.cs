using System;

namespace ALBINGIA.Framework.Common.AlbingiaExceptions
{
    public class AlbFoncException:AlbException
    {
       
        #region constructeur
        public AlbFoncException(string message, bool trace = false, bool sendMail = false, bool onlyMessage=true)
            : base(new Exception(message), trace, sendMail,onlyMesage:onlyMessage)
        {

        }
        #endregion

        #region Méthodes surchargées 
        public override string Message
        {
            get
            {
                
                return string.IsNullOrEmpty(AlbExpMessage) ? base.Message : AlbExpMessage;
            }

        }
        #endregion
    }
}
